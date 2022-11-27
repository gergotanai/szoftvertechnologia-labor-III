using EntityFrameworkSpecialRelationships.Enums;

namespace EntityFrameworkSpecialRelationships.Models
{
    public class PropertyOwnership
    {
        public int Id { get; set; }
        public ICollection<Person> People { get; set; }
        public ICollection<Property> Properties { get; set; }
        public Ownership Ownership { get; set; }
    }
}
