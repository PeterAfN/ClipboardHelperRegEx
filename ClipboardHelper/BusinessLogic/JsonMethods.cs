﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public class JsonMethods
    {
        public JsonMethods()
        {
        }


        public enum ResultsFromDownloading
        {
            OK, Empty, Error501, Timeout
        }


        public string Url { get; set; }
        public string Downloaded { get; set; }
        public ResultsFromDownloading ResultFromDownloading { get; set; }
        /* static */
        readonly SortedList<string, string> CachedUrls = new SortedList<string, string>();
        public string CurrentCached { get; set; }


        public bool Cached()
        {
            foreach (KeyValuePair<string, string> cachedUrl in CachedUrls)
            {
                if (cachedUrl.Key == Url)
                {
                    CurrentCached = cachedUrl.Value;
                    return true;
                }
            }
            return false;
        }


        string parsedJsonResults;

        public string GetField(string downloadedString, string jsonField)
        {
            //only first occurance of json is parsed. To do in the future: choose which json occurance to parse.
            string regex = "{(?>[^{}]+|{(?<x>)|}(?<-x>))*(?(x)(?!))}";
            MatchCollection listOfMatches;
            if (Regex.IsMatch(downloadedString, regex))
            {
                listOfMatches = Regex.Matches(downloadedString, regex);
                var partialComponents = listOfMatches.Cast<Match>().Select(match => match.Value).ToList();
                parsedJsonResults = partialComponents[0];
                var obj = JObject.Parse(parsedJsonResults);
                var JsonResult = obj.Descendants()
                    .OfType<JProperty>()
                    .Select(p => new KeyValuePair<string, object>(p.Path,
                        p.Value.Type == JTokenType.Array || p.Value.Type == JTokenType.Object
                            ? null : p.Value));
                foreach (var kvp in JsonResult)
                {
                    if (jsonField == kvp.Key)
                    {
                        return kvp.Value.ToString();
                    }
                }
                return "";
            }
            return downloadedString;
        }


        //https://stackoverflow.com/questions/11118712/webclient-accessing-page-with-credentials
        public string Download(string usr = "", SecureString psw = null)
        {
            using (var client = new System.Net.WebClient())
            {
                if (string.IsNullOrEmpty(usr))
                {
                    client.UseDefaultCredentials = false;
                    Downloaded = DownloadJsonFromUrl(client, Url);
                    return Downloaded;
                }
                else
                {
                    client.UseDefaultCredentials = true;
                    client.Credentials =
                        new NetworkCredential(usr, psw);
                    Downloaded = DownloadJsonFromUrl(client, Url);
                    return Downloaded;
                }
            }
        }


        private static string DownloadJsonFromUrl(System.Net.WebClient client, string url)
        {
            return client.DownloadString(url);
        }


        public void Cache()
        {
            CachedUrls.Add(Url, Downloaded);
        }
    }
}
