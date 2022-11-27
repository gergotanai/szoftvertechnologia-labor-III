using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkSpecialRelationships.Models
{
    [Owned]
    [Table("GiftCards")]
    public class GiftCard
    {
        public string Shop { get; set; }
        public int Amount { get; set; }
    }
}
