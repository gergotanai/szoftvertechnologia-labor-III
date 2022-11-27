using EntityFrameworkSpecialRelationships.Data;
using EntityFrameworkSpecialRelationships.Enums;
using EntityFrameworkSpecialRelationships.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EntityFrameworkSpecialRelationships
{
    public class Program
    {
        static void CreateData(SpecialRelationshipContext db)
        {
            Person person1 = new() { Name = "Norman Hill", Age = 30, IdentityCard = new IdentityCard { DocumentNumber = "000001XX" }, GiftCards = new[] { new GiftCard { Shop = "Tesco", Amount = 100 }, new GiftCard { Shop = "Spar", Amount = 50 } } };
            Person person2 = new() { Name = "Charles Barrett", Age = 32 };
            Person person3 = new() { Name = "Malik Faure", Age = 45, IdentityCard = new IdentityCard { DocumentNumber = "000003XX" } };
            Person person4 = new() { Name = "Nina Morris", Age = 24, GiftCards = new[] { new GiftCard { Shop = "Tesco", Amount = 120 } } };

            Property property1 = new() { County = "United States", Street = "Pecan Acres Ln", Number = 2459 };
            Property property2 = new() { County = "Switzerland", Street = "Rue Chazière", Number = 9541 };
            Property property3 = new() { County = "New Zealand", Street = "Hugh Watt Drive", Number = 9033 };
            Property property4 = new() { County = "United States", Street = "Brown Terrace", Number = 3833 };

            PropertyOwnership propertyOwnership1 = new() { People = new[] { person1 }, Properties = new[] { property1 }, Ownership = Ownership.LeaseToOwn };
            PropertyOwnership propertyOwnership2 = new() { People = new[] { person2 }, Properties = new[] { property2, property3}, Ownership = Ownership.Own };
            // Can't add property4 since it would have to Ownership types
            // PropertyOwnership propertyOwnership2 = new() { People = new[] { person2 }, Properties = new[] { property2, property3, property4 }, Ownership = Ownership.Own };
            PropertyOwnership propertyOwnership3 = new() { People = new[] { person3, person4 }, Properties = new[] { property4 }, Ownership = Ownership.Rent };

            db.Add(propertyOwnership1);
            db.Add(propertyOwnership2);
            db.Add(propertyOwnership3);

            db.SaveChanges();
        }

        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var options = new DbContextOptionsBuilder<SpecialRelationshipContext>()
                .UseSqlServer(config.GetConnectionString("SpecialRelationshipsDB"))
                .Options;

            using var db = new SpecialRelationshipContext(options);

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            CreateData(db);
        }
    }
}
