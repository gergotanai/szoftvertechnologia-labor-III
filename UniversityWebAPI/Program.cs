using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using UniversityWebAPI.Data;
using UniversityWebAPI.Models;

static void CreateData(UniversityContext db)
{
    Classroom r1 = new Classroom { Number = "R101" };
    Classroom r2 = new Classroom { Number = "R202" };

    Teacher t1 = new Teacher { Name = "Miss Anderson", Classroom = r1 };
    Teacher t2 = new Teacher { Name = "Miss Bingham" };

    Course c1 = new Course { Title = "Introduction to EF Core", Author = t1, Editor = t2 };
    Course c2 = new Course { Title = "Basic Car Maintenance", Author = t2, Editor = t1 };

    Student s1 = new Student { Name = "Jenny Jones", Classroom = r1 };
    Student s2 = new Student { Name = "Kenny Kent", Classroom = r1 };
    Student s3 = new Student { Name = "Lucy Locket", Classroom = r1 };
    Student s4 = new Student { Name = "Micky Most", Classroom = r2 };
    Student s5 = new Student { Name = "Nelly Norton", Classroom = r2 };
    Student s6 = new Student { Name = "Ozzy Osborne", Classroom = r2 };


    c1.Students = new Student[] { s1, s2, s3, s4 };
    c2.Students = new Student[] { s3, s4, s5, s6 };

    db.Add(c1);
    db.Add(c2);

    db.SaveChanges();
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddDbContext<UniversityContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddSwaggerDocument(configure => configure.Title = "University");
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Database initialization
using (var scope = app.Services.CreateScope())
{
    // Prevent NSwag build errors
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<UniversityContext>();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        CreateData(db);
    }
    catch (PlatformNotSupportedException) { }
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseOpenApi();
app.UseSwaggerUi3();

app.Run();
