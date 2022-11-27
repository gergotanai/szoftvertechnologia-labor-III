using Microsoft.EntityFrameworkCore;

namespace PetModels
{
    public class PetsContext : DbContext
    {
        public PetsContext(DbContextOptions<PetsContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dog>().ToTable("Dogs");
            modelBuilder.Entity<Cat>().ToTable("Cats");
        }

        public DbSet<Cat> Cats { get; set; }
        public DbSet<Dog> Dogs { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pet> Pets { get; set; }
    }
}
