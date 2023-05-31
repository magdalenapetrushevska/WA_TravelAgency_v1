using System.ComponentModel.DataAnnotations.Schema;
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
        public int PricePerPerson { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public OfferStatus Status { get; set; }
        public Transport? Transport { get; set; }
        public Destination? Destination { get; set; }
        public virtual OfferParameters? OfferParameters { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
    }
}
