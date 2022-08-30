using ISPRO.Persistence.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISPRO.Persistence.Entities
{
    public class ManagerAccount : AbstractUser
    {
        public override UserType UserType { get; set; } = UserType.MANAGER;

        public List<Project> Projects { get; set; } = new List<Project>();
    }
}
