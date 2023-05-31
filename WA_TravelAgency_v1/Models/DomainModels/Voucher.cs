using System.ComponentModel.DataAnnotations.Schema;
using WA_TravelAgency_v1.Models.Enums;
using WA_TravelAgency_v1.Models.Identity;

namespace WA_TravelAgency_v1.Models.DomainModels
{
    [Table("Voucher")]
    public class Voucher : BaseEntity
    {
        public Boolean IsUsed { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser Passenger { get; set; }
    }
}
