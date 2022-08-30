using ISPRO.Persistence.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISPRO.Persistence.Entities
{
    public class CashPayment : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [ForeignKey("UserAccount")]
        public string UserAccountId { get; set; }

        [Required]
        public UserAccount? UserAccount { get; set; }

        [Required]
        [Display(Name = "Payment Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [UIHint("DateTimePicker")]
        public DateTime? PaymentDate { get; set; }

        [Required]
        [Display(Name = "Recharge Period")]
        public TimeSpan RechargePeriod { get; set; } = TimeSpan.FromDays(30);

        [Required]
        public int Ammount { get; set; } = 0;

        [Required]
        public Currency Currency { get; set; } = Currency.USD;
    }
}