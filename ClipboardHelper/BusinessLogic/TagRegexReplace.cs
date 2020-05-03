using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public static class TagRegexReplace
    {
        public static string Handle(List<string> splitContent, string clipboard = "")
        {
            var outData = string.Empty;
            if (splitContent != null && (splitContent.Count != 3 && splitContent.Count != 2)) return outData;
            if (splitContent == null) return outData;
            switch (splitContent.Count)
            {
                case 2:
                    outData = RegexReplace(clipboard, splitContent[0], splitContent[1]);
                    break;
                case 3:
                    outData = RegexReplace(splitContent[0], splitContent[1], splitContent[2]);
                    break;
                default:
                    outData = "Error while parsing";
                    break;
            }

            return outData;
        }

        private static string RegexReplace(string inData, string pattern, string replacement)
        {
            var rgx = new Regex(pattern);
            return rgx.Replace(inData, replacement);
        }
    }
}