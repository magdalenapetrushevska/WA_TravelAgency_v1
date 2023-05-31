using System.ComponentModel.DataAnnotations.Schema;
using WA_TravelAgency_v1.Models.Enums;

namespace WA_TravelAgency_v1.Models.DomainModels
{
    [Table("Transport")]
    public class Transport : BaseEntity
    {
        public Vehicle Vehicle { get; set; }
        public string Company { get; set; }
        public string Capacity { get; set; }
        public ICollection<Offer>? Offers { get; set; }

    }
}
