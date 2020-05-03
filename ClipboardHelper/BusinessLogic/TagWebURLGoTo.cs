using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public static class TagWebUrlGoTo
    {
        public static void Handle(List<string> splitContent)
        {
            if (splitContent != null && !ValidUrl(splitContent[0])) return;
            if (splitContent == null) return;
            var cleanedUpUrl = Lines.CleanOneLine(splitContent[0]);
            Process.Start(cleanedUpUrl); //Opens in last used instance of default OS Web Browser.
        }

        private static bool ValidUrl(string url)
        {
            var result = Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                         && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }
    }
}