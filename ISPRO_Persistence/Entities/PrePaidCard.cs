using ISPRO.Persistence.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISPRO.Persistence.Entities
{
    public class PrePaidCard : BaseEntity
    {
        [Key]
        [Required]
        public long Code { get; set; }

        [ForeignKey("Subscription")]
        public int SubscriptionId { get; set; }

        [Required]
        public Subscription Subscription { get; set; }

        [ForeignKey("Consumer")]
        public string ConsumerId { get; set; }

        public UserAccount? Consumer { get; set; }

        [Required]
        [Display(Name = "Expiry Date")]
        [DataType(DataType.DateTime)]
        public DateTime? ExpiryDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Consumption Date")]
        [DataType(DataType.DateTime)]
        public DateTime? ConsumptionDate { get; set; }

        [Required]
        [Display(Name = "Recharge Period")]
        public TimeSpan RechargePeriod { get; set; } = TimeSpan.FromDays(30);

        [Required]
        public int Price { get; set; } = 0;

        [NotMapped]
        public bool Consumed => Consumer != null;

        [Required]
        public Currency Currency { get; set; } = Currency.USD;
    }
}