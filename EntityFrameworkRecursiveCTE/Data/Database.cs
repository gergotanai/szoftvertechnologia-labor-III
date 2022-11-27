using EntityFrameworkRecursiveCTE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EntityFrameworkRecursiveCTE.Data
{
    public class RecursiveCTEContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public IQueryable<Employee> AllReports(int id)
        {
            return Employees.FromSqlRaw(
            @"WITH OrganizationalChart (Id, Name, Title, ManagerId, Below) AS (
                    SELECT Id, Name, Title, ManagerId, 0
                    FROM dbo.Employees
                    WHERE Employees.Id = {0}
                    UNION ALL
                    SELECT e.Id, e.Name, e.Title, e.ManagerId, o.Below + 1
                    FROM dbo.Employees AS e
                    INNER JOIN OrganizationalChart AS o ON e.ManagerId = o.Id
                ) SELECT * from OrganizationalChart", id);
        }

        //public IQueryable<Employee> AllReports(int id)
        //{
        //    return Employees.FromSqlRaw("Exec AllReports {0}", id);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasData(new List<Employee>()
                {
                    new() {Id = 1, Name = "Charles Montgomery Burns", Title = "Owner"},
                    new() {Id = 2, Name = "Waylon Smithers, Jr.", Title = "Assistant", ManagerId = 1},
                    new() {Id = 3, Name = "Lenny Leonard", Title = "Technical Supervisor", ManagerId = 2},
                    new() {Id = 4, Name = "Carl Carlson", Title = "Safety Operations Supervisor", ManagerId = 2},
                    new() {Id = 5, Name = "Inanimate Carbon Rod", Title = "Rod", ManagerId = 4},
                    new() {Id = 6, Name = "Homer Simpson", Title = "Safety Inspector", ManagerId = 5}
                });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder
                .UseSqlServer(config.GetConnectionString("EntityFrameworkRecursiveDB"));
        }
    }
}
