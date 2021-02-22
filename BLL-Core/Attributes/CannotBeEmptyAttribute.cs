using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BLL_Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CannotBeEmptyAttribute : ValidationAttribute, IClientValidatable
    {
        private const string defaultError = "'{0}' must have at least one element.";
        public CannotBeEmptyAttribute()
            : base(defaultError)
        {
        }

        public override bool IsValid(object value)
        {
            IList list = value as IList;
            return (list != null && list.Count > 0);
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(this.ErrorMessageString, name);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule();
            rule.ValidationType = "listcountisgreaterthanzero";
            rule.ErrorMessage = "Please choose at least one option";
            //mcvrTwo.ValidationParameters.Add
            //("param", DateTime.Now.ToString("dd-MM-yyyy"));
            yield return rule;
        }
    }
}
