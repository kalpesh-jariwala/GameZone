using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Core.Infrastructure.ExtensionMethods
{
    public static class EnumExtensions
    {
        public static string GetEnumDisplayName<T>(T value) where T : struct
        {
            string retVal = value.ToString();
            // Get the MemberInfo object for supplied enum value
            var memberInfo = value.GetType().GetMember(value.ToString());
            if (memberInfo == null || memberInfo.Length != 1)
                return retVal;

            // Get DisplayAttibute on the supplied enum value
            var displayAttribute = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false)
                as DisplayAttribute[];
            if (displayAttribute == null || displayAttribute.Length != 1)
                return retVal;

            return displayAttribute[0].Name;
        }
       
    }
}
