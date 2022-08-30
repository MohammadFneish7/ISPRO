using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISPRO.Persistence.Entities
{
    public class Subscription : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        public int Bandwidth { get; set; } = 0;

        [Required]
        public int Quota { get; set; } = 0;

        [ForeignKey("Project")]
        public string ProjectName { get; set; }

        [Required]
        public Project Project { get; set; }

        public List<UserAccount> Subscribers { get; set; } = new List<UserAccount>();

        [Display(Name = "Pre-Paid Cards")]
        public List<PrePaidCard> PrePaidCards { get; set; } = new List<PrePaidCard>();
    }
}
