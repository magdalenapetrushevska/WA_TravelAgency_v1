using System.ComponentModel.DataAnnotations;

namespace WA_TravelAgency_v1.Models.DomainModels
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
