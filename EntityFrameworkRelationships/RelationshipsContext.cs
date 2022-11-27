using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkRelationships
{
    public class RelationshipsContext : DbContext
    {
        // 1 to 1 (owned)
        public DbSet<OneToOneOwner> OneToOneOwners { get; set; }
        // 1 to Many
        public DbSet<OneToMany> OneToManys { get; set; }
        public DbSet<OneToManyItem> OneToManyItems { get; set; }
        // 1 to Many (owned)
        public DbSet<OneToManyOwner> OneToManyOwners { get; set; }
        // Many To Many (Transparent)
        public DbSet<ManyToManyLeft> ManyToManyLefts { get; set; }
        public DbSet<ManyToManyRight> ManyToManyRights { get; set; }
        // Many To Many (Modeled Relationship)
        public DbSet<ManyToManyWithModeledLeft> ManyToManyWithModeledLefts { get; set; }
        public DbSet<ManyToManyWithModeledRight> ManyToManyWithModeledRights { get; set; }
        public DbSet<ManyToManyRelationship> ManyToManyRelationships { get; set; }
        // Hierarchical
        public DbSet<Hierarchical> Hierarchicals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("RelationshipsDB"));
        }
    }

    public class OneToOneOwner
    {
        public int Id { get; set; }
        public OneToOneOwned Owned { get; set; }
    }

    [Owned]
    public class OneToOneOwned
    {
        [Required]
        // Required is needed to prevent this warning message
        // The entity type 'OneToOneOwned' is an optional dependent using table sharing without any required non shared property that could be used to identify whether the entity exists. If all nullable properties contain a null value in database then an object instance won't be created in the query. Add a required property to create instances with null values for other properties or mark the incoming navigation as required to always create an instance.
        public string Value { get; set; }
    }

    public class OneToMany
    {
        public int Id { get; set; }
        public List<OneToManyItem> Items { get; set; }
    }

    public class OneToManyItem
    {
        public int Id { get; set; }
        public int OneToManyId { get; set; }
        public OneToMany OneToMany { get; set; }
    }

    public class OneToManyOwner
    {
        public int Id { get; set; }
        public List<OneToManyOwnedItem> Items { get; set; }
    }

    [Owned]
    public class OneToManyOwnedItem
    {
        public string Name { get; set; }
    }

    public class ManyToManyLeft
    {
        public int Id { get; set; }
        public List<ManyToManyRight> Rights { get; set; }
    }

    public class ManyToManyRight
    {
        public int Id { get; set; }
        public List<ManyToManyLeft> Lefts { get; set; }
    }

    public class ManyToManyWithModeledLeft
    {
        public int Id { get; set; }
        public int LeftInt { get; set; }
        public ManyToManyRelationship Relationship { get; set; }
    }

    public class ManyToManyWithModeledRight
    {
        public int Id { get; set; }
        public int RightInt { get; set; }
        public ManyToManyRelationship Relationship { get; set; }
    }

    public enum Ownership
    {
        Own,
        LeaseToOwn,
        Rent
    }

    public class ManyToManyRelationship
    {
        public int Id { get; set; }
        public Ownership Ownership { get; set; }
        public ICollection<ManyToManyWithModeledLeft> Lefts { get; set; }
        public ICollection<ManyToManyWithModeledRight> Rights { get; set; }

    }

    public class Hierarchical
    {
        public int Id { get; set; }
        public Hierarchical Parent { get; set; }
        public List<Hierarchical> Children { get; set; }
    }
}
