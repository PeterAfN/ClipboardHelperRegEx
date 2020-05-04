using AdysTech.CredentialManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public class WebCredentials
    {
        public enum ResultsFromPasswordPrompt
        {
            Ok,
            OkNoSave,
            Cancel
        }

        /// <summary>
        ///     First string is FQDN, second string is Username, third string is domain name, fourth securestring is Password.
        /// </summary>
        private static readonly List<Tuple<string, string, string, SecureString>> Temporary =
            new List<Tuple<string, string, string, SecureString>>();

        private NetworkCredential _cred;
        public SecureString Pwd { get; private set; }
        public object Usr { get; private set; }

        public WebCredentials(Uri url)
        {
            Url = url;
            if (Url == null) return;
            var uri = new Uri(Url.ToString());
            Fqdn = uri.Host;
        }

        public ResultsFromPasswordPrompt ResultFromPasswordPrompt { get; private set; }

        private Uri Url { get; }
        private bool SaveCredentials { get; set; }
        private string Fqdn { get; }

        private object _dmn;

        public void Prompt()
        {
            var saveCredentials = SaveCredentials = true;
            _cred = CredentialManager.PromptForCredentials(Fqdn, ref saveCredentials,
                "Please provide credentials for " + Fqdn, "Clipboard Helper");
            if (_cred == null) ResultFromPasswordPrompt = ResultsFromPasswordPrompt.Cancel;
            else if (saveCredentials == false) ResultFromPasswordPrompt = ResultsFromPasswordPrompt.OkNoSave;
            else ResultFromPasswordPrompt = ResultsFromPasswordPrompt.Ok;
            if (_cred != null) Usr = _cred.UserName;
            if (_cred != null) Pwd = _cred.SecurePassword;
            if (_cred != null) _dmn = _cred.Domain;

            SaveCredentials = saveCredentials;
        }

        public bool Saved()
        {
            MessageBox.Show("1");
            if (string.IsNullOrEmpty(Fqdn)) return false;
            MessageBox.Show("2");
            _cred = CredentialManager.GetCredentials(Fqdn);
            MessageBox.Show("3");
            if (_cred == null)
                return false;
            MessageBox.Show("4");
            Usr = _cred.UserName;
            Pwd = _cred.SecurePassword;
            _dmn = _cred.Domain;
            MessageBox.Show("5");
            return true;
        }

        public bool SavedTemporary()
        {
            foreach (var tempCred in Temporary.Where(tempCred => tempCred.Item1 == Fqdn))
            {
                Usr = tempCred.Item2;
                _dmn = tempCred.Item3;
                Pwd = tempCred.Item4;
                return true;
            }

            return false;
        }

        public void Save()
        {
            if (SaveCredentials) CredentialManager.SaveCredentials(Fqdn, _cred);
        }

        public void SaveTemporary()
        {
            Temporary.Add(new Tuple<string, string, string, SecureString>(Fqdn, _cred.UserName, _cred.Domain,
                _cred.SecurePassword));
        }
    }
}