using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WA_TravelAgency_v1.Models.DomainModels
{
    [Table("Destination")]
    public class Destination : BaseEntity
    {
        [Required]
        public string Country { get; set; }
        public string? City { get; set; }
        public string? Accommodation { get; set; }
        public ICollection<Offer>? Offers { get; set; }
    }
}
