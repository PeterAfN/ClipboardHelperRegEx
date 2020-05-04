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
            //System.Windows.Forms.MessageBox.Show("a");
            //if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri)) return null;
            //System.Windows.Forms.MessageBox.Show("b");
            var webCredentials = new WebCredentials(url);
            //System.Windows.Forms.MessageBox.Show("c");
            _jSon.Url = url;
            string outData;
            if (_jSon.Cached())
                return _jSon.GetField(_jSon.CurrentCached, jsonField);
            //System.Windows.Forms.MessageBox.Show("d");
            if (webCredentials.SavedTemporary() || webCredentials.Saved())
                try
                {
                    //System.Windows.Forms.MessageBox.Show("e");
                    outData = _jSon.Download(TimeoutJson, webCredentials.usr.ToString(), webCredentials.pwd);
                    _jSon.Cache();
                    return _jSon.GetField(outData, jsonField);
                }
                catch (WebException ex) when (ex.Message.Contains("401"))
                {
                    //System.Windows.Forms.MessageBox.Show("f");
                    outData = WhenDownloadingOfJsonFailed(webCredentials, _jSon);
                    return _jSon.GetField(outData, jsonField);
                }
                catch (WebException ex)
                {
                    //System.Windows.Forms.MessageBox.Show("g");
                    return ex.Message;
                }
                catch (Exception)
                {
                    //System.Windows.Forms.MessageBox.Show("h");
                    outData = WhenDownloadingOfJsonFailed(webCredentials, _jSon);
                    return _jSon.GetField(outData, jsonField);
                    //throw;
                }
            try
            {
                //System.Windows.Forms.MessageBox.Show("i");
                outData = _jSon.Download(TimeoutJson);
                _jSon.Cache();
                return _jSon.GetField(outData, jsonField);
            }
            catch (WebException ex) when (ex.Message.Contains("401"))
            {
                //System.Windows.Forms.MessageBox.Show("j");
                outData = WhenDownloadingOfJsonFailed(webCredentials, _jSon);
                return _jSon.GetField(outData, jsonField);
            }
            catch (WebException ex)
            {
                //System.Windows.Forms.MessageBox.Show("k");
                _jSon.Downloaded = ex.Message;
                _jSon.Cache();
                return ex.Message;
            }
            catch
            {
                //System.Windows.Forms.MessageBox.Show("l");
                outData = WhenDownloadingOfJsonFailed(webCredentials, _jSon);

                return _jSon.GetField(outData, jsonField);
                //throw;
            }
        }

        private string WhenDownloadingOfJsonFailed(WebCredentials webCredentials, JsonMethods jSon)
        {
            //System.Windows.Forms.MessageBox.Show("m");
            var webCred = webCredentials;
            var jSn = jSon;
            try
            {
                //System.Windows.Forms.MessageBox.Show("n");
                webCredentials.Prompt();
                string outData;
                switch (webCredentials.ResultFromPasswordPrompt)
                {
                    case WebCredentials.ResultsFromPasswordPrompt.Ok:
                        //System.Windows.Forms.MessageBox.Show("o");
                        webCredentials.Save();
                        outData = jSon.Download(TimeoutJson, webCred.usr.ToString(), webCred.pwd);
                        jSn.Cache();
                        return outData;
                    case WebCredentials.ResultsFromPasswordPrompt.OkNoSave:
                        //System.Windows.Forms.MessageBox.Show("p");
                        webCredentials.SaveTemporary();
                        outData = jSon.Download(TimeoutJson, webCred.usr.ToString(), webCred.pwd);
                        jSn.Cache();
                        return outData;
                    default:
                        return "credentials prompt cancelled.";
                }
            }
            catch (WebException ex) when (ex.Message.Contains("401"))
            {
                //System.Windows.Forms.MessageBox.Show("q");
                return WhenDownloadingOfJsonFailed(webCred, jSn);
            }
            catch (WebException ex)
            {
                //System.Windows.Forms.MessageBox.Show("r");
                return ex.Message;
            }
            catch (Exception)
            {
                //System.Windows.Forms.MessageBox.Show("s");
                return WhenDownloadingOfJsonFailed(webCred, jSn);
               
            }
        }
    }
}