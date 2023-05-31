using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WA_TravelAgency_v1.Models.Enums;
using WA_TravelAgency_v1.Models.Identity;

namespace WA_TravelAgency_v1.Models.DomainModels
{
    public class Reservation : BaseEntity
    {
        public Guid? OfferId { get; set; }
        [Display(Name = "Amount to pay")]
        public int AmountToPay { get; set; }
        [Display(Name = "Date of reservation")]
        public DateTime ReservationDate { get; set; }
        public NoYes Paid { get; set; }
        [Display(Name = "Amount paid")]
        public int AmountPaid { get; set; }
        public OfferStatus Status { get; set; }
        public Offer? Offer { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? Passenger { get; set; }
        [Display(Name = "Number of passengers")]
        public int NumOfPassengers { get; set; }

    }


}
