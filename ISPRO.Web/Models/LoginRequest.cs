using ISPRO.Persistence.Entities;
using ISPRO.Persistence.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISPRO.Web.Models
{
    public class LoginRequest
    {
        [Required]
        [MaxLength(300)]
        public string Username { get; set; }

        [Required]
        [MaxLength(300)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ModelError { get; set; } = string.Empty;
    }
}
