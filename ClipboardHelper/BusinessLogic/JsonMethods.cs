using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text.RegularExpressions;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public class JsonMethods
    {
        private readonly SortedList<string, string> _cachedUrls = new SortedList<string, string>();

        private string _parsedJsonResults;
        public System.Uri Url { get; set; }
        public string Downloaded { get; set; }
        public string CurrentCached { get; private set; }


        public bool Cached()
        {
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var cachedUrl in _cachedUrls)
            {
                if (cachedUrl.Key != Url.ToString()) continue;
                CurrentCached = cachedUrl.Value;
                return true;
            }

            return false;
        }

        public string GetField(string downloadedString, string jsonField)
        {
            //only first occurrences of json is parsed. To do in the future: choose which json occurence to parse.
            const string regex = "{(?>[^{}]+|{(?<x>)|}(?<-x>))*(?(x)(?!))}";
            if (!Regex.IsMatch(downloadedString, regex)) return downloadedString;
            var listOfMatches = Regex.Matches(downloadedString, regex);
            var partialComponents = listOfMatches.Cast<Match>().Select(match => match.Value).ToList();
            _parsedJsonResults = partialComponents[0];
            var obj = JObject.Parse(_parsedJsonResults);
            var jsonResult = obj.Descendants()
                .OfType<JProperty>()
                .Select(p => new KeyValuePair<string, object>(p.Path,
                    p.Value.Type == JTokenType.Array || p.Value.Type == JTokenType.Object
                        ? null
                        : p.Value));
            foreach (var kvp in jsonResult)
                if (jsonField == kvp.Key)
                    return kvp.Value.ToString();
            return "";
        }

        //https://stackoverflow.com/questions/11118712/webclient-accessing-page-with-credentials
        public string Download(int timeout, string usr = "", SecureString psw = null)
        {
            using (var client = new WebClient())
            {
                if (!string.IsNullOrEmpty(usr))
                {
                    client.UseDefaultCredentials = false;
                    Downloaded = DownloadJsonFromUrl(client, timeout, Url.ToString());
                    return Downloaded;
                }

                client.UseDefaultCredentials = true;
                client.Credentials =
                    new NetworkCredential(usr, psw);
                Downloaded = DownloadJsonFromUrl(client, timeout, Url.ToString());
                return Downloaded;
            }
        }

        private static string DownloadJsonFromUrl(WebClient client, int timeout, string url)
        {
            client.Timeout = timeout;
            var downloadedString = client.DownloadString(url);
            return string.IsNullOrEmpty(downloadedString) ? string.Empty : downloadedString;
        }

        public void Cache()
        {
            _cachedUrls.Add(Url.ToString(), Downloaded);
        }
    }
}