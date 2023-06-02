using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using WA_TravelAgency_v1.Models.Enums;

namespace WA_TravelAgency_v1.Models.DomainModels
{
    [Table("Offer")]
    public class Offer : BaseEntity
    {
        public string Name { get; set; }
        public Guid? TransportId { get; set; }
        public Guid? DestinationId { get; set; }
        public OfferType Type { get; set; }
        [Display(Name = "Price per person")]
        public int PricePerPerson { get; set; }
        [Display(Name = "From date")]
        public DateTime FromDate { get; set; }
        [Display(Name = "To date")]
        public DateTime ToDate { get; set; }
        public OfferStatus Status { get; set; }
        [Display(Name = "Sure realization")]
        public NoYes SureRealization { get; set; }
        public Transport? Transport { get; set; }
        public Destination? Destination { get; set; }
        public virtual OfferParameters? OfferParameters { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
        public Guid? PromotionId { get; set; }
        public virtual Promotion? Promotion { get; set; }
    }
}
