using ISPRO.Persistence.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISPRO.Persistence.Entities
{
    public class AdminAccount : AbstractUser
    {
        public override UserType UserType { get => UserType.ADMIN; set => base.UserType = UserType.ADMIN; }
    }
}
