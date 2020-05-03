using System;
using System.Net;

namespace ClipboardHelperRegEx.BusinessLogic
{
    // ReSharper disable once UnusedType.Global
    public class CookieAwareWebClient : WebClient
    {
        public CookieAwareWebClient()
        {
            CookieContainer = new CookieContainer();
        }

        private CookieContainer CookieContainer { get; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest)base.GetWebRequest(address);
            if (request == null) return null;
            request.CookieContainer = CookieContainer;
            return request;
        }
    }
}