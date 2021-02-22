using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Core.Infrastructure.ExtensionMethods
{
    public static class IntegerExtensions
    {
        public static string ToStringEquivalent(this int value, bool toLower = false)
        {
            string stringEquivalent = "";
            int tempNum = value;
            int num;
            do
            {
                num = tempNum % 26;
                if (num == 0) num = 26;
                stringEquivalent = string.Concat(stringEquivalent, (char)(num + 64));
                tempNum /= 26;
            } while (tempNum > 0 && !(num == 26 && tempNum == 1));
            return toLower ? stringEquivalent.ToLower() : stringEquivalent;
        }
    }
}
