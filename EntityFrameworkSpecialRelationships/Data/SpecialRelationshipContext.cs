using EntityFrameworkSpecialRelationships.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkSpecialRelationships.Data
{
    public class SpecialRelationshipContext : DbContext
    {
        public SpecialRelationshipContext(DbContextOptions<SpecialRelationshipContext> options) : base(options) { }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyOwnership> PropertyOwnerships { get; set; }
    }
}
