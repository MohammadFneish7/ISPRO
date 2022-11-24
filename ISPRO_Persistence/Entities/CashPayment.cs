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
        public string UserAccountName { get; set; }

        [Required]
        public UserAccount UserAccount { get; set; }

        [Required]
        [Display(Name = "Payment Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [UIHint("DateTimePicker")]
        public DateTime PaymentDate { get; set; }

        [Required]
        [Display(Name = "Recharge Period (Days)")]
        [Range(0, 1000)]
        public int RechargePeriod { get; set; } = 30;

        [Required]
        [Range(0, double.MaxValue)]
        public double Ammount { get; set; } = 0;

        [Required]
        public Currency Currency { get; set; } = Currency.USD;
    }
}