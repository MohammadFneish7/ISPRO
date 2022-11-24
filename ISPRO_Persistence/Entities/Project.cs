using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISPRO.Persistence.Entities
{
    public class Project : BaseEntity
    {
        [Key]
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string? Description { get; set; }

        [ForeignKey("ProjectManager")]
        [Display(Name = "Project Manager")]
        public string ProjectManagerUsername { get; set; }

        [Required]
        [Display(Name = "Project Manager")]
        public ManagerAccount ProjectManager { get; set; }

        [Display(Name = "User Accounts")]
        public List<UserAccount> UserAccounts { get; set; } = new List<UserAccount>();

        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
