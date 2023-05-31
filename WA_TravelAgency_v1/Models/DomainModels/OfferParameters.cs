using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace WA_TravelAgency_v1.Models.DomainModels
{
    [Table("OfferParameters")]
    public class OfferParameters : BaseEntity
    {
        [Display(Name = "Мinimum number of passengers for sure realization")]
        public int МinNumOfPassengersForSureRealization { get; set; }
        [Display(Name = "Мinimum number of passengers for gratis")]
        public int МinNumOfPassForGratis { get; set; }
        public Guid? OfferId { get; set; }
        public virtual Offer? Offer { get; set; }
    }
}
