using Microsoft.AspNetCore.Identity;
using WA_TravelAgency_v1.Models.DomainModels;

namespace WA_TravelAgency_v1.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public String? Name { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
        public virtual Voucher Voucher { get; set; }
    }
}
