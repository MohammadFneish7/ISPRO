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
    public abstract class BaseEntity
    {
        [Display(Name = "Creation Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Editable(false)]
        public virtual DateTime? CreationDate { get; set; }

        [Display(Name = "Last Update")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Editable(false)]
        public virtual DateTime? LastUpdate { get; set; }

        [NotMapped]
        public string ModelError { get; set; } = string.Empty;
    }
}
