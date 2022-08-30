using ISPRO.Persistence.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISPRO.Persistence.Entities
{
    public class UserAccount : AbstractUser
    {
        [ForeignKey("Project")]
        public string ProjectName { get; set; }

        [Required]
        public Project Project { get; set; }

        [ForeignKey("Subscription")]
        public int SubscriptionId { get; set; }

        [Required]
        public Subscription Subscription { get; set; }

        public override UserType UserType { get => UserType.USER_ACCOUNT; set => base.UserType = UserType.USER_ACCOUNT; }

        [Required]
        [Display(Name = "Resume Date")]
        [DataType(DataType.DateTime)]
        public virtual DateTime? ResumeDate { get; set; } = DateTime.Now;

        [Display(Name = "Pre-Paid Cards")]
        public List<PrePaidCard> PrePaidCards { get; set; } = new List<PrePaidCard>();

        [Display(Name = "Cash Payments")]
        public List<CashPayment> CashPayments { get; set; } = new List<CashPayment>();

    }
}
