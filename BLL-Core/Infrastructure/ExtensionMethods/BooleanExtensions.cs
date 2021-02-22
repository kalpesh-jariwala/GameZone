namespace BLL_Core.Infrastructure.ExtensionMethods
{
    public static class BooleanExtensions
    {
        public static bool IsNull<T>(this T thing)
        {
            // ReSharper disable CompareNonConstrainedGenericWithNull
            return thing == null;
            // ReSharper restore CompareNonConstrainedGenericWithNull
        }

        public static bool IsNotNull<T>(this T thing)
        {
            // ReSharper disable CompareNonConstrainedGenericWithNull
            return thing != null;
            // ReSharper restore CompareNonConstrainedGenericWithNull
        }

        public static (bool parsed, bool value) CustomParse(this /*ref*/ bool value, string parseString)
        { //this can be changed to use ref, but requires all developers to have a reasonably up to date version of c#
            var boolParsed = bool.TryParse(parseString, out bool tempBool);
            #region my_extra_parsing
            if (!boolParsed)
            {
                switch (parseString.ToLower().Trim())
                {
                    case "1":
                        boolParsed = true;
                        tempBool = true;
                        break;
                    case "0":
                        boolParsed = true;
                        tempBool = false;
                        break;
                    case "y":
                        boolParsed = true;
                        tempBool = true;
                        break;
                    case "n":
                        boolParsed = true;
                        tempBool = false;
                        break;
                    case "yes":
                        boolParsed = true;
                        tempBool = true;
                        break;
                    case "no":
                        boolParsed = true;
                        tempBool = false;
                        break;
                    case "t":
                        boolParsed = true;
                        tempBool = true;
                        break;
                    case "f":
                        boolParsed = true;
                        tempBool = false;
                        break;
                    case "✓":
                        boolParsed = true;
                        tempBool = true;
                        break;
                    case "☑":
                        boolParsed = true;
                        tempBool = true;
                        break;
                    case "✅":
                        boolParsed = true;
                        tempBool = true;
                        break;
                    case "✔":
                        boolParsed = true;
                        tempBool = true;
                        break;
                    case "✘":
                        boolParsed = true;
                        tempBool = false;
                        break;
                    case "❌":
                        boolParsed = true;
                        tempBool = false;
                        break;
                    case "✖":
                        boolParsed = true;
                        tempBool = false;
                        break;
                    case "✕":
                        boolParsed = true;
                        tempBool = false;
                        break;
                    case "❎":
                        boolParsed = true;
                        tempBool = false;
                        break;
                    case "☓":
                        boolParsed = true;
                        tempBool = false;
                        break;
                    case "✗":
                        boolParsed = true;
                        tempBool = false;
                        break;
                    default:
                        break;
                }
            }
            #endregion
            //value = tempBool;
            return (boolParsed, tempBool);
        }
    }
}