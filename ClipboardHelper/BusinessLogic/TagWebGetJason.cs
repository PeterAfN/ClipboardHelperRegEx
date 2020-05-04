using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public class TagWebGetJason
    {
        private readonly JsonMethods _jSon;

        public TagWebGetJason()
        {
            _jSon = new JsonMethods();
        }

        private int TimeoutJson { get; } = 10000;

        public string Handle(string line, List<string> splitContent)
        {
            if (splitContent != null && splitContent.Count == 3)
                switch (splitContent[2])
                {
                    case "toLowerCase":
                        splitContent[0] = splitContent[0].ToLower(CultureInfo.CurrentCulture);
                        break;
                    case "toUpperCase":
                        splitContent[0] = splitContent[0].ToUpper(CultureInfo.CurrentCulture);
                        break;
                }

            if (splitContent != null && (splitContent.Count != 2 && splitContent.Count != 3)) return "Error while parsing";
            if (line == null) return null;
            if (splitContent == null) return null;
            var output = WebGetJson(line.Split(';')[0], splitContent[1]);
            return output;
        }

        private string WebGetJson(string url, string jsonField)
        {
            if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri)) return null;
            var webCredentials = new WebCredentials(uri);
            _jSon.Url = uri;
            string outData;
            if (_jSon.Cached())
                return _jSon.GetField(_jSon.CurrentCached, jsonField);
            if (webCredentials.SavedTemporary() || webCredentials.Saved())
                try
                {
                    outData = _jSon.Download(TimeoutJson, webCredentials.Usr.ToString(), webCredentials.Pwd);
                    _jSon.Cache();
                    return _jSon.GetField(outData, jsonField);
                }
                catch (WebException ex) when (ex.Message.Contains("401"))
                {
                    outData = WhenDownloadingOfJsonFailed(webCredentials, _jSon);
                    return _jSon.GetField(outData, jsonField);
                }
                catch (WebException ex)
                {
                    return ex.Message;
                }
                catch (Exception)
                {
                    outData = WhenDownloadingOfJsonFailed(webCredentials, _jSon);
                    return _jSon.GetField(outData, jsonField);
                    //throw;
                }
            try
            {
                outData = _jSon.Download(TimeoutJson);
                _jSon.Cache();
                return _jSon.GetField(outData, jsonField);
            }
            catch (WebException ex) when (ex.Message.Contains("401"))
            {
                outData = WhenDownloadingOfJsonFailed(webCredentials, _jSon);
                return _jSon.GetField(outData, jsonField);
            }
            catch (WebException ex)
            {
                _jSon.Downloaded = ex.Message;
                _jSon.Cache();
                return ex.Message;
            }
            catch
            {
                outData = WhenDownloadingOfJsonFailed(webCredentials, _jSon);

                return _jSon.GetField(outData, jsonField);
                //throw;
            }
        }

        private string WhenDownloadingOfJsonFailed(WebCredentials webCredentials, JsonMethods jSon)
        {
            var webCred = webCredentials;
            var jSn = jSon;
            try
            {
                webCredentials.Prompt();
                string outData;
                switch (webCredentials.ResultFromPasswordPrompt)
                {
                    case WebCredentials.ResultsFromPasswordPrompt.Ok:
                        webCredentials.Save();
                        outData = jSon.Download(TimeoutJson, webCred.Usr.ToString(), webCred.Pwd);
                        jSn.Cache();
                        return outData;
                    case WebCredentials.ResultsFromPasswordPrompt.OkNoSave:
                        webCredentials.SaveTemporary();
                        outData = jSon.Download(TimeoutJson, webCred.Usr.ToString(), webCred.Pwd);
                        jSn.Cache();
                        return outData;
                    default:
                        return "credentials prompt cancelled.";
                }
            }
            catch (WebException ex) when (ex.Message.Contains("401"))
            {
                return WhenDownloadingOfJsonFailed(webCred, jSn);
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
            catch (Exception)
            {
                throw;
                return WhenDownloadingOfJsonFailed(webCred, jSn);
               
            }
        }
    }
}