using ClipboardHelperRegEx.Properties;
using ClipboardHelperRegEx.Views;
using System;
using System.Diagnostics;
using System.IO;
using Template;

namespace ClipboardHelper.Views
{
    internal partial class ViewAbout : FormTemplate, IViewAbout
    {
        public ViewAbout()
        {
            InitializeComponent();
            CreateEvents();
        }

        public event EventHandler ClickedOkButton;

        public void SetProduct(string product)
        {
            labelProductName.Text = product;
        }

        public void SetVersion(string version)
        {
            labelVersion.Text = Resources.ViewAbout_SetVersion_Version__ + version;
        }

        public void SetCopyright(string copyright)
        {

        }

        public void SetCompany(string company)
        {

        }

        public void SetDescription(string description)
        {
            textBoxDescription.Text = description;
        }

        private void CreateEvents()
        {
            okButton.Click += OnClickOkButton;
        }

        private void OnClickOkButton(object sender, EventArgs e) => ClickedOkButton?.Invoke(this, e);

        private void LinkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            var licenseFile = Path.Combine(Path.GetTempPath(), "License.txt");
            File.WriteAllText(licenseFile, Resources.License);
            Process.Start(licenseFile);
        }

        private void LinkLabel2_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/PeterAfN");
        }
    }
}