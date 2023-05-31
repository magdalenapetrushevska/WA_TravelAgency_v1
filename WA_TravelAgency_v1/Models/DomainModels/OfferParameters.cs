using System.ComponentModel.DataAnnotations.Schema;

namespace WA_TravelAgency_v1.Models.DomainModels
{
    [Table("OfferParameters")]
    public class OfferParameters : BaseEntity
    {
        public int DiscountForChildren { get; set; }
        public int Discount { get; set; }
        public int МinDaysForReservation { get; set; }
        public Guid? OfferId { get; set; }
        public virtual Offer? Offer { get; set; }
    }
}
