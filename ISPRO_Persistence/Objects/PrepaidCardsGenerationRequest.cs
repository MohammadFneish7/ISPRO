using ISPRO.Persistence.Entities;
using ISPRO.Persistence.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISPRO.Persistence.Objects
{
    public class PrepaidCardsGenerationRequest
    {
        [ForeignKey("Subscription")]
        public int SubscriptionId { get; set; }

        [Required]
        public Subscription Subscription { get; set; }

        [Required]
        [Display(Name = "Expiry Date")]
        [DataType(DataType.DateTime)]
        public DateTime? ExpiryDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Recharge Period (Days)")]
        [Range(0, 1000)]
        public int RechargePeriod { get; set; } = 30;

        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; } = 0;

        [Required]
        public Currency Currency { get; set; } = Currency.USD;

        [Required]
        [Range(1, 100)]
        [Display(Name = "Number Of Cards")]
        public int NumberOfCards { get; set; } = 1;

        [NotMapped]
        public string ModelError { get; set; } = string.Empty;
    }
}
