using System;
using System.Collections.Generic;
using BLL_Core.Attributes;
using PlayCaddy.Core.Infrastructure;

namespace BLL_Core.Infrastructure.ExtensionMethods
{
    /// <summary>
    ///     Static enumerations helper class
    /// </summary>
    public static class EnumerationExtensions
    {
        public static T ToEnum<T>(this int value)
        {
            return (T) Enum.Parse(typeof (T), value.ToString());
        }

        public static T ToEnum<T>(this string value)
        {
            return (T) Enum.Parse(typeof (T), value);
        }

        /// <summary>
        ///     Retrieves the description of an enum value if one is set
        /// </summary>
        /// <param name="en_">The enum value</param>
        /// <returns>The description if one was set otherwise the name of the enum value</returns>
        public static string Description(this Enum en)
        {
            // get the type of the enum
            var type = en.GetType();

            // retrieve the member information of the type
            var memberInfo = type.GetMember(en.ToString());
            //if (memberInfo != null && memberInfo.Length > 0)
            if (memberInfo.Length > 0)
            {
                // retrieve any attributes set on the enum type
                var attributes = memberInfo[0].GetCustomAttributes(typeof (EnumDescriptionAttribute), false);

                if (attributes.Length > 0)
                {
                    // return the enum description
                    return ((EnumDescriptionAttribute) attributes[0]).Text;
                }
            }

            // default - return the value of the enum. when the description attribute has not been applied
            return en.ToString();
        }

        public static IList<EnumKeyValue> GetDescriptions<T>(this Enum enumobj)
        {
            var enumType = typeof (T);

            // Can't use type constraints on value types, so have to do check like this
            if (enumType.BaseType != typeof (Enum))
                throw new ArgumentException("T must be of type System.Enum");

            var enumValArray = Enum.GetValues(enumType);
            var enumNames = Enum.GetNames(enumType);
            var i = 0;

            var nameValues = new List<EnumKeyValue>();

            foreach (var val in enumValArray)
            {
                nameValues.Add(
                    new EnumKeyValue(
                        enumNames[i],
                        ((Enum) Enum.Parse(typeof (T), val.ToString())).Description()
                        )
                    );
                i++;
            }

            return nameValues;
        }
    }
}