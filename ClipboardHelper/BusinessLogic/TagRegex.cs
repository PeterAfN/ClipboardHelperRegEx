using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public static class TagRegex
    {
        public static string Handle(List<string> splitContent, string clipboard = "")
        {
            if (splitContent == null) return null;
            switch (splitContent.Count)
            {
                case 1:
                    return RegexMatch(clipboard, splitContent[0]);
                case 2:
                    return RegexMatch(splitContent[0], splitContent[1]);
                default:
                    return "Error while parsing";
            }
        }

        /// <summary>
        ///     Matches and transform the regex inside the tags content and returns the first matched result.
        /// </summary>
        /// <param name="inData"></param>
        /// <param name="inRegex"></param>
        /// <returns></returns>
        private static string RegexMatch(string inData, string inRegex)
        {
            var outData = "";
            if (inData == null) return outData;
            if (Regex.IsMatch(inData, inRegex))
            {
                var listOfMatches = Regex.Matches(inData, inRegex);
                var partialComponents = listOfMatches.Cast<Match>().Select(match => match.Value).ToList();
                outData = partialComponents[0];
            }
            else
            {
                outData = string.Empty;
            }

            return outData;
        }
    }
}