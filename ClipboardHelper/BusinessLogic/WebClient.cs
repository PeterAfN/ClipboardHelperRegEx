using System;
using System.Net;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public class WebClient : System.Net.WebClient
    {
        public int Timeout { get; set; }

        protected override WebRequest GetWebRequest(Uri uri)
        {
            var lWebRequest = base.GetWebRequest(uri);
            if (lWebRequest == null) return null;
            lWebRequest.Timeout = Timeout;
            ((HttpWebRequest)lWebRequest).ReadWriteTimeout = Timeout;
            return lWebRequest;
        }
    }
}