using ISPRO.Helpers;
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
    [NotMapped]
    public abstract class AbstractUser : BaseEntity
    {
        [Key]
        [Required]
        [MaxLength(50)]
        public virtual string Username { get; set; }

        [MaxLength(50)]
        [Display(Name = "Display Name")]
        public virtual string? DisplayName { get; set; }

        [MaxLength(150)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [MaxLength(50)]
        public string? Mobile { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [Display(Name = "Expiry Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required]
        public virtual DateTime? ExpiryDate { get; set; } = DateTime.Now;

        [Display(Name = "User Type")]
        [Editable(false)]
        public virtual UserType UserType { get; set; }

        public bool Active { get; set; } = true;

        [NotMapped]
        [Display(Name = "Is Expired")]
        public bool IsExpired { get => ExpiryDate.HasValue && ExpiryDate.Value < DateTime.Now; }
    }
}
