namespace EntityFrameworkSpecialRelationships.Models
{
    public class Property
    {
        public int Id { get; set; }
        public string County { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public PropertyOwnership PropertyOwnership { get; set; }
    }
}
