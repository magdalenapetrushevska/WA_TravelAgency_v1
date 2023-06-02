﻿using Stripe;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WA_TravelAgency_v1.Models.DomainModels
{
    public class Promotion : BaseEntity
    {
        public string Title { get; set; }
        public Guid? OfferId { get; set; }
        public Offer? Offer { get; set; }
        public int Discount { get; set; }
        [Display(Name = "Start date of promotion")]
        public DateTime StartDateOfPromotion { get; set; }
        [Display(Name = "End date of promotion")]
        public DateTime EndDateOfPromotion { get; set; }
    }
}
