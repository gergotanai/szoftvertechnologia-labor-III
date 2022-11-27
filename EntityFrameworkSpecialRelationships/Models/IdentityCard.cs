using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkSpecialRelationships.Models
{
    [Owned]
    public class IdentityCard
    {
        public string DocumentNumber { get; set; }
    }
}
