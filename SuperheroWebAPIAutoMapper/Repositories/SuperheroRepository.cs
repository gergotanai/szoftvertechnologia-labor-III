using SuperheroWebAPIAutoMapper.Models;

namespace SuperheroWebAPIAutoMapper.Repositories
{
    public class SuperheroRepository
    {
        public static List<Superhero> Superheros = new()
        {
            new Superhero {
                Id = 1,
                Name ="Spider Man",
                FirstName = "Peter",
                LastName = "Parker",
                Place = "New York City",
                DateAdded = new DateTime(2001, 08, 10),
                DateModified = null
            },
            new Superhero {
                Id = 2,
                Name ="Iron Man",
                FirstName = "Tony",
                LastName = "Stark",
                Place = "Malibu",
                DateAdded = new DateTime(1970, 05, 29),
                DateModified = null
            }
        };
    }
}
