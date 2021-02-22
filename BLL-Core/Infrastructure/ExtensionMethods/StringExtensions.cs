using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BLL_Core.Infrastructure.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string NormaliseForHtmlId(this string value)
        {
            var normalized = value.Normalize(NormalizationForm.FormD);
            var resultBuilder = new StringBuilder();
            foreach (var character in normalized)
            {
                var category = CharUnicodeInfo.GetUnicodeCategory(character);
                if (category == UnicodeCategory.LowercaseLetter
                    || category == UnicodeCategory.UppercaseLetter
                    || category == UnicodeCategory.SpaceSeparator)
                    resultBuilder.Append(character);
            }
            return Regex.Replace(resultBuilder.ToString(), @"\s+", "");
        }


        public static bool IsNot(this string value, string checkvalue)
        {
            return value != checkvalue;
        }

        /// <summary>
        ///     Checks whether the given Email-Parameter is a valid E-Mail address.
        /// </summary>
        /// <param name="email">Parameter-string that contains an E-Mail address.</param>
        /// <returns>
        ///     True, when Parameter-string is not null and
        ///     contains a valid E-Mail address;
        ///     otherwise false.
        /// </returns>
        public static bool IsEmail(this string email)
        {
            // Regular expression, which is used to validate an E-Mail address.
            string pattern =
                @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

            // updated pattern with pattern from register user input (changed by amjad to accept + in the email address
            pattern =
                @"^([a-zA-Z0-9_\-\.]+)([\+a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            if (email != null) return Regex.IsMatch(email, pattern);
            else return false;
        }
        public static string GetValidEmails(this string email)
        {
            var listOfEmail = email.Split(',');
            var validEmails = "";
            foreach (var emails in listOfEmail)
            {
                if (emails != null && emails.IsEmail())
                    validEmails = validEmails.IsNotNullOrEmpty() ? validEmails + "," + emails : validEmails + emails;
            }
            return validEmails.IsNullOrEmpty() ? "dontuse@playwaze.com" : validEmails;
        }

        /// <summary>
        ///     If item is null or empty then return this as the alternative
        /// </summary>
        /// <param name="value"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string IsNullEmptyThenReturnThis(this string value, string replacement)
        {
            if (string.IsNullOrEmpty(value))
            {
                return replacement;
            }
            return value;
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNotNullOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static string SportNameFromId(this string phrase)
        {
            if (phrase.IsNullOrEmpty())
            {
                return string.Empty;
            }
            if (phrase.Contains("/"))
            {
                string str = phrase.Substring(phrase.IndexOf('/') + 1);
                return str;
            }

            return phrase;
        }

        public static string GenerateSlug(this string phrase)
        {
            if (phrase.IsNullOrEmpty())
            {
                return string.Empty;
            }

            string str = phrase.RemoveAccent().ToLower();

            str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); // invalid chars           
            str = Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space   
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim(); // cut and trim it   
            str = Regex.Replace(str, @"\s", "-"); // hyphens   

            return str;
        }

        public static string GenerateCommunityCode(this string phrase)
        {
            if (phrase.IsNullOrEmpty())
            {
                return string.Empty;
            }

            string str = phrase.RemoveAccent().ToLower();

            str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); // invalid chars           
            str = Regex.Replace(str, @"\s+", "").Trim(); // remove spaces   
            str = str.Substring(0, str.Length <= 4 ? str.Length : 4).Trim(); // cut and trim it   
            str = Regex.Replace(str, @"\s", "-"); // hyphens   

            Random random = new Random();
            var number = random.Next(0, 9999).ToString("D4");

            return str + number;
        }

        public static string GenerateSlug(this string phrase, IList<string> existingphrases)
        {
            int count = 2;
            string str = phrase.RemoveAccent().ToLower();

            str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); // invalid chars           
            str = Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space   
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim(); // cut and trim it   
            str = Regex.Replace(str, @"\s", "-"); // hyphens   

            while (existingphrases.Contains(str))
            {
                str += "-" + count;
                count++;
            }

            return str;
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return Encoding.ASCII.GetString(bytes);
        }

        public static string ToJSON(this object obj)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

        public static string ToJSON(this object obj, int recursionDepth)
        {
            var serializer = new JavaScriptSerializer();
            serializer.RecursionLimit = recursionDepth;
            return serializer.Serialize(obj);
        }

        public static bool IsNumeric(this string input)
        {
            if (input.IsNullOrEmpty()) return false;
            long retNum;
            return long.TryParse(input, System.Globalization.NumberStyles.Integer, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
        }

        private static string DuplicateTicksForSql(this string s)
        {
            return s.Replace("'", "''");
        }

        public static string ToDelimitedString(this List<string> list, string delimiter = ":", bool insertSpaces = false, string qualifier = "", bool duplicateTicksForSQL = false)
        {
            var result = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                string initialStr = duplicateTicksForSQL ? list[i].DuplicateTicksForSql() : list[i];
                result.Append((qualifier == string.Empty) ? initialStr : string.Format("{1}{0}{1}", initialStr, qualifier));
                if (i < list.Count - 1)
                {
                    result.Append(delimiter);
                    if (insertSpaces)
                    {
                        result.Append(' ');
                    }
                }
            }
            return result.ToString();
        }

        public static string StripSeasonId(this string id)
        {

            if (id.IsNullOrEmpty()) return string.Empty;
            if (!id.Contains("_"))
            {
                return id;
            }
            return id.Substring(0, id.IndexOf("_"));
        }

        public static string StripHTML(this string inputString)
        {
            string pattern = @"<(.|\n)*?>";
            var result = Regex.Replace(inputString, pattern, string.Empty);
            result = result.Replace("&nbsp;", " ");
            return result;
        }

        /// <summary>
        /// Reduces any amount of whitespace in a row to one, will not remove leading or trailing whitespace
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ReduceWhitespace(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            var newString = new StringBuilder();
            bool previousIsWhitespace = false;
            for (int i = 0; i < value.Length; i++)
            {
                if (char.IsWhiteSpace(value[i]))
                {
                    if (previousIsWhitespace)
                    {
                        continue;
                    }
                    previousIsWhitespace = true;
                }
                else
                {
                    previousIsWhitespace = false;
                }
                newString.Append(value[i]);
            }
            return newString.ToString();
        }

        /// <summary>
        /// Provides an easy way to build a string of several parts
        /// </summary>
        /// <param name="value"></param>
        /// <param name="addString"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string FormattedConcat(this string value, string addString, string separator = ", ")
        {
            if (string.IsNullOrWhiteSpace(addString))
                return value;
            if (string.IsNullOrWhiteSpace(value))
                return addString;
            return string.Concat(value.Trim(), separator, addString.Trim());
        }

        /// <summary>
        /// Provides an easy way to build a string of several parts, adds the given string to the start
        /// </summary>
        /// <param name="value"></param>
        /// <param name="addString"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string FormattedConcatPrepend(this string value, string addString, string separator = ", ")
        {
            if (string.IsNullOrWhiteSpace(addString))
                return value;
            if (string.IsNullOrWhiteSpace(value))
                return addString;
            return string.Concat(addString.Trim(), separator, value.Trim());
        }

        /// <summary>
        /// Returns true if the string is no null or only whitespace and is entirely in upper case
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsAllCaps(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;
            return string.Equals(value, value.ToUpper());
        }

        /// <summary>
        /// Returns a string with typical replacements or equivalents of null, set to null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string NaToNull(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            var lowerVal = value.ToLower().Trim();
            if (string.Equals("na", lowerVal) || string.Equals("n/a", lowerVal) || string.Equals("n.a", lowerVal) ||
                string.Equals("n-a", lowerVal) || string.Equals("null", lowerVal) || string.Equals("not applicable", lowerVal) ||
                string.Equals("no answer", lowerVal) || string.Equals("not available", lowerVal) || string.Equals("tbc", lowerVal) ||
                string.Equals("tbd", lowerVal))
                return null;
            return value;
        }

        /// <summary>
        /// Compares the strings with all whitespace removed and ToLower
        /// </summary>
        /// <param name="value"></param>
        /// <param name="otherVal"></param>
        /// <returns></returns>
        public static bool EqualContent(this string value, string otherVal)
        {
            var valueNull = string.IsNullOrWhiteSpace(value);
            var otherNull = string.IsNullOrWhiteSpace(otherVal);
            if (valueNull && otherNull)
                return true;
            if (valueNull || otherNull)
                return false;
            var alteredValue = value.ToLower().Replace(" ", "");
            var alteredOther = otherVal.ToLower().Replace(" ", "");
            return alteredValue.Equals(alteredOther);
        }

        public static bool Similar(this string value, string otherVal, int tolerance = 5)
        {
            if (value == null && otherVal == null)
                return true;
            if (value == null || otherVal == null)
                return false;

            return true;
        }

        /// <summary>
        /// Calculates the number of operations needed to change from one string to the other (technically gets the optimal string alignment distance)
        /// </summary>
        /// <param name="string1"></param>
        /// <param name="string2"></param>
        /// <returns></returns>
        public static int DamerauLevenshteinDistance(this string string1, string string2)
        { //Shamelessly stolen from http://mihkeltt.blogspot.com/2009/04/dameraulevenshtein-distance.html
            //and https://en.wikipedia.org/wiki/Damerau%E2%80%93Levenshtein_distance#Optimal_string_alignment_distance
            if (string.IsNullOrEmpty(string1))
            {
                if (!string.IsNullOrEmpty(string2))
                    return string2.Length;
                return 0;
            }
            if (string.IsNullOrEmpty(string2))
            {
                if (!string.IsNullOrEmpty(string1))
                    return string1.Length;
                return 0;
            }

            int length1 = string1.Length;
            int length2 = string2.Length;

            int[,] d = new int[length1 + 1, length2 + 1];

            int cost, del, ins, sub;

            for (int i = 0; i <= d.GetUpperBound(0); i++)
                d[i, 0] = i;

            for (int i = 0; i <= d.GetUpperBound(1); i++)
                d[0, i] = i;

            for (int i = 1; i <= d.GetUpperBound(0); i++)
            {
                for (int j = 1; j <= d.GetUpperBound(1); j++)
                {
                    if (string1[i - 1] == string2[j - 1])
                        cost = 0;
                    else
                        cost = 1;

                    del = d[i - 1, j] + 1;
                    ins = d[i, j - 1] + 1;
                    sub = d[i - 1, j - 1] + cost;

                    d[i, j] = Math.Min(del, Math.Min(ins, sub));

                    if (i > 1 && j > 1 && string1[i - 1] == string2[j - 2] && string1[i - 2] == string2[j - 1])
                        d[i, j] = Math.Min(d[i, j], d[i - 2, j - 2] + cost);
                }
            }

            return d[d.GetUpperBound(0), d.GetUpperBound(1)];
        }

        public static string ToNotation(this string origin)
        {
            if (string.IsNullOrEmpty(origin))
                return origin;
            var value = origin.ReduceWhitespace().ToLower().Replace("%20", " ").Replace(' ', '_').Replace("/", "");
            return value;
        }

        public static List<string> ToLower(this IEnumerable<string> list)
        {
            var returnList = new List<string>();
            foreach (var item in list)
            {
                if (item.IsNotNullOrEmpty())
                    returnList.Add(item.ToLower());
            }

            return returnList;
        }

        public static string BuildUrlFromHost(this string host, bool isSecured = true, string port = null)
        {
            return string.Concat(isSecured ? "https://" : "http://", host, port ?? "");
        }



        /// <summary>
        /// Use the current culture info to convert a string to Title Case
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String ToTitleCase(this string value)
        {
            var cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            return cultureInfo.TextInfo.ToTitleCase(value.ToLower());
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
