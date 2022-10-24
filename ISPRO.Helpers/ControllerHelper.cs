using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISPRO.Helpers
{
    public class ControllerHelper
    {
        public bool ValidateModelStateParentFieldByStrField(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary ModelState,string parentField, string strField, dynamic strFieldValue)
        {
            if (ModelState[parentField]?.ValidationState == ModelValidationState.Invalid)
            {
                if (strFieldValue != null)
                {
                    if (ModelState[parentField] != null)
                    {
                        ModelState[parentField].ValidationState = ModelValidationState.Valid;
                        ModelState[parentField].Errors.Clear();
                    }
                    if (ModelState[strField] != null)
                    {
                        ModelState[strField].ValidationState = ModelValidationState.Valid;
                        ModelState[strField].Errors.Clear();
                    }

                    return true;
                }
                else
                {
                    foreach (var error in ModelState[parentField].Errors)
                        ModelState.AddModelError(strField, error.ErrorMessage);

                    if (ModelState[strField] != null)
                        ModelState[strField].ValidationState = ModelValidationState.Invalid;

                }
                return false;
            }
            return true;
        }
    }
}
