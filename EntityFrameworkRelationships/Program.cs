using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkRelationships
{
    public class Program
    {
        static void CreateData(RelationshipsContext db)
        {
            ManyToManyWithModeledLeft left1 = new() { LeftInt = 1 };
            ManyToManyWithModeledLeft left2 = new() { LeftInt = 2 };

            ManyToManyWithModeledRight right1 = new() { RightInt = 3 };
            ManyToManyWithModeledRight right2 = new() { RightInt = 4 };

            ManyToManyRelationship rel1 = new() { Lefts = new[] { left1, left2 }, Rights = new[] { right1, right2 } };

            db.Add(rel1);

            db.SaveChanges();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            using var db = new RelationshipsContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            CreateData(db);

            foreach (var r in db.ManyToManyWithModeledLefts)
                Console.WriteLine(r);

            foreach (var r in db.ManyToManyRelationships)
                Console.WriteLine(r);
        }
    }
}
