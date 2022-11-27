using Microsoft.EntityFrameworkCore;
using UniversityWebAPI.Models;

namespace UniversityWebAPI.Data
{
    public class UniversityContext : DbContext
    {
        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasOne(c => c.Editor).WithMany(t => t.CoursesEdited);
            modelBuilder.Entity<Course>().HasOne(c => c.Author).WithMany(t => t.CoursesWritten);
        }

        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
    }
}
