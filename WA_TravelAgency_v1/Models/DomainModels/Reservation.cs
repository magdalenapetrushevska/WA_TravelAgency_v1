using WA_TravelAgency_v1.Models.Enums;
using WA_TravelAgency_v1.Models.Identity;

namespace WA_TravelAgency_v1.Models.DomainModels
{
    public class Reservation : BaseEntity
    {
        public Guid? OfferId { get; set; }
        //to do: vouchers
        public int AmountToPay { get; set; }
        public DateTime ReservationDate { get; set; }
        //public Guid VoucherId { get; set; }
        public NoYes Paid { get; set; }
        public int AmountPaid { get; set; }
        public OfferStatus Status { get; set; }
        public Offer? Offer { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? Passenger { get; set; }
        public int NumOfPassengers { get; set; }

    }


}
