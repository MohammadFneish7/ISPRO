using ISPRO.Persistence.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

namespace ISPRO.Persistence.Entities
{
    public class PrePaidCard : BaseEntity
    {
        [Key]
        [Required]
        public string Id { get; set; }

        [ForeignKey("Subscription")]
        public int SubscriptionId { get; set; }

        [Required]
        public Subscription Subscription { get; set; }

        [ForeignKey("Consumer")]
        [Display(Name = "Consumer")]
        public string? ConsumerName { get; set; }

        public UserAccount? Consumer { get; set; }

        [Required]
        [Display(Name = "Expiry Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ExpiryDate { get; set; } = DateTime.Now;

        [Display(Name = "Consumption Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ConsumptionDate { get; set; }

        [Required]
        [Display(Name = "Recharge Period (Days)")]
        [Range(0, 1000)]
        public int RechargePeriod { get; set; } = 30;

        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; } = 0;

        [NotMapped]
        [Display(Name = "Is Expired")]
        public bool IsExpired { get => ExpiryDate.HasValue && ExpiryDate.Value < DateTime.Now; }

        [NotMapped]
        [Display(Name = "Is Consumed")]
        public bool IsConsumed => Consumer != null;

        [Required]
        public Currency Currency { get; set; } = Currency.USD;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static string GenerateId()
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = string.Format("{0}{1}", "ISPRO-PPC", DateTime.Now.Ticks);
                byte[] saltedPasswordAsBytes = Encoding.UTF8.GetBytes(saltedPassword);
                return BitConverter.ToString((sha256.ComputeHash(saltedPasswordAsBytes))).Replace("-", string.Empty).Substring(0,24);
            }
        }
    }
}