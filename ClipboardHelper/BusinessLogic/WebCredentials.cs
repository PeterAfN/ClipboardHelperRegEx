using System;
using System.Collections.Generic;
using System.Net;
using System.Security;
using AdysTech.CredentialManager;

namespace ClipboardHelperRegEx.BusinessLogic
{

    public class WebCredentials
    {

        public WebCredentials(string url)
        {
            this.Url = url;
            Uri uri = new Uri(Url, UriKind.Absolute);
            Fqdn = uri.Host;
        }

        private NetworkCredential _cred;

        public object Usr { get; private set; }

        public SecureString Pwd { get; private set; }

        public ResultsFromPasswordPrompt ResultFromPasswordPrompt { get; private set; }

        public enum ResultsFromPasswordPrompt { Ok, OkNoSave, Cancel }

        private string Url { get; set; }

        private bool SaveCredentials { get; set; }

        private string Fqdn { get; set; }

        public void Prompt()
        {
            try
            {
                var saveCredentials = SaveCredentials = true;
                _cred = CredentialManager.PromptForCredentials(Fqdn, ref saveCredentials, "Please provide credentials for " + Fqdn, "Clipboard Helper");
                if (_cred == null) ResultFromPasswordPrompt = ResultsFromPasswordPrompt.Cancel;
                else if (saveCredentials == false) ResultFromPasswordPrompt = ResultsFromPasswordPrompt.OkNoSave;
                else ResultFromPasswordPrompt = ResultsFromPasswordPrompt.Ok;
                if (_cred != null) Usr = _cred.UserName;
                if (_cred != null) Pwd = _cred.SecurePassword;
                SaveCredentials = saveCredentials;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Saved()
        {
            if (string.IsNullOrEmpty(Fqdn)) return false;
            _cred = CredentialManager.GetCredentials(Fqdn);
            if (_cred == null)
                return false;
            else
            {
                Usr = _cred.UserName;
                Pwd = _cred.SecurePassword;
                return true;
            }
        }


        public bool SavedTemporary()
        {
            foreach (var tempCred in Temporary)
            {
                if (tempCred.Item1 != Fqdn) continue;
                Usr = tempCred.Item2;
                Pwd = tempCred.Item4;
                return true;
            }
            return false;
        }


        public void Save()
        {
            if (SaveCredentials) CredentialManager.SaveCredentials(Fqdn, _cred);
        }


        /// <summary>
        /// First string is FQDN, second string is Username, third string is domain name, fourth securestring is Password.
        /// </summary>
        private static readonly List<Tuple<string, string, string, SecureString>> Temporary = new List<Tuple<string, string, string, SecureString>>();


        public void SaveTemporary()
        {
            Temporary.Add(new Tuple<string, string, string, SecureString>(Fqdn, _cred.UserName, _cred.Domain, _cred.SecurePassword));
        }

    }
}
