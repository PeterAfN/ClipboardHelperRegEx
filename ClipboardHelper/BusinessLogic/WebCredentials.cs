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
            FQDN = uri.Host;
        }


        NetworkCredential cred;
        public object usr;
        public SecureString pwd;
        public object dmn;
        public ResultsFromPasswordPrompt ResultFromPasswordPrompt { get; set; }
        public enum ResultsFromPasswordPrompt { Ok, OkNoSave, Cancel }
        public string Url { get; set; }
        public bool SaveCredentials { get; set; }
        public string FQDN { get; set; }


        public void Prompt()
        {
            try
            {
                bool saveCredentials = SaveCredentials = true;
                cred = CredentialManager.PromptForCredentials(FQDN, ref saveCredentials, "Please provide credentials for " + FQDN, "Clipboard Helper");
                if (cred == null) ResultFromPasswordPrompt = ResultsFromPasswordPrompt.Cancel;
                else if (saveCredentials == false) ResultFromPasswordPrompt = ResultsFromPasswordPrompt.OkNoSave;
                else ResultFromPasswordPrompt = ResultsFromPasswordPrompt.Ok;
                if (cred != null) usr = cred.UserName;
                if (cred != null) pwd = cred.SecurePassword;
                if (cred != null) dmn = cred.Domain;
                SaveCredentials = saveCredentials;
            }
            catch (Exception)
            {
                throw;
            }
        }


        private NetworkCredential Get(string webAddress)
        {
            return cred = CredentialManager.GetCredentials(FQDN);
        }


        public bool Saved()
        {
            if (FQDN == string.Empty) return false;
            cred = CredentialManager.GetCredentials(FQDN);
            if (cred == null)
                return false;
            else
            {
                usr = cred.UserName;
                pwd = cred.SecurePassword;
                dmn = cred.Domain;
                return true;
            }
        }


        public bool SavedTemporary()
        {
            foreach (Tuple<string, string, string, SecureString> tempCred in temporary)
            {
                if (tempCred.Item1 == FQDN)
                {
                    usr = tempCred.Item2;
                    dmn = tempCred.Item3;
                    pwd = tempCred.Item4;
                    return true;
                }
            }
            return false;
        }


        public void Save()
        {
            if (SaveCredentials) CredentialManager.SaveCredentials(FQDN, cred);
        }


        /// <summary>
        /// First string is FQDN, second string is Username, third string is domain name, fourth securestring is Password.
        /// </summary>
        static readonly List<Tuple<string, string, string, SecureString>> temporary = new List<Tuple<string, string, string, SecureString>>();


        public void SaveTemporary()
        {
            temporary.Add(new Tuple<string, string, string, SecureString>(FQDN, cred.UserName, cred.Domain, cred.SecurePassword));
        }

    }
}
