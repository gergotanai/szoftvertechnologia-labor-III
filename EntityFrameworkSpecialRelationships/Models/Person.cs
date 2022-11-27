using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkSpecialRelationships.Models
{
    [Table("People")]
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        [Required]
        // Required is needed to prevent this warning message:
        // The entity type 'IdentityCard' is an optional dependent using table sharing without any required non shared property that could be used to identify whether the entity exists. If all nullable properties contain a null value in database then an object instance won't be created in the query. Add a required property to create instances with null values for other properties or mark the incoming navigation as required to always create an instance.
        public IdentityCard IdentityCard { get; set; }
        public ICollection<GiftCard> GiftCards { get; set; }
        public PropertyOwnership PropertyOwnership { get; set; }
    }
}
