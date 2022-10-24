using ISPRO.Persistence.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISPRO.Persistence.Entities
{
    public class ManagerAccount : AbstractUser
    {
        public override UserType UserType { get; set; } = UserType.MANAGER;

        [Required]
        [Display(Name = "Max Allowed Projects")]
        [Range(0,1000)]
        public int MaxAllowedProjects { get; set; } = 1;

        public List<Project> Projects { get; set; } = new List<Project>();
    }
}
