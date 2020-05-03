using System;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public static class TagWebGet
    {

        public static string Handle(string line, Tags.UsedIn usedIn)
        {
            if (line == null) return string.Empty;
            if (!ValidUrl(line.Trim())) return string.Empty;
            switch (usedIn)
            {
                case Tags.UsedIn.MainDisplay:
                case Tags.UsedIn.Settings:
                    try
                    {
                        var client = new WebClient
                        {
                            Timeout = 1500
                        };

                        var outData = client.DownloadString(line);
                        client.Dispose();
                        return outData;
                    }
                    catch (System.Net.Sockets.SocketException e)
                    {
                        return e.ToString();
                    }
                    catch (Exception)
                    {
                        return string.Empty;
                        //throw;
                    }
                case Tags.UsedIn.SingleSelection:
                case Tags.UsedIn.NestedTags:
                case Tags.UsedIn.Pasting:
                    return line;
                default:
                    throw new ArgumentOutOfRangeException(nameof(usedIn), usedIn, null);
            }
        }

        private static bool ValidUrl(string url)
        {
            var result = Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                         && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

    }
}
