using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISPRO.Helpers
{
    public class ReflectionHelper
    {
        public void CopyNullFromOld(object oldObj, object newObj)
        {
            foreach (var prop_old in oldObj.GetType().GetProperties())
            {
                foreach (var prop_new in newObj.GetType().GetProperties())
                {
                    if(prop_old.Name.Equals(prop_new.Name) && prop_new.GetValue(newObj) == null)
                    {
                        prop_new.SetValue(newObj, prop_old.GetValue(oldObj));
                    }
                }
            }
        }
    }
}
