using ClipboardHelperRegEx.Properties;
using ClipboardHelperRegEx.Views;
using System;
using System.Diagnostics;
using System.IO;

namespace ClipboardHelperRegEx.BusinessLogic.Presenters
{
    public class PresenterUserSettingsRightHelp
    {

        private readonly IViewUserSettingsRightHelp _view;

        public PresenterUserSettingsRightHelp(IViewUserSettingsRightHelp view)
        {
            _view = view;

            if (view != null) view.Load += View_Load;
        }

        private void View_Load(object sender, EventArgs e)
        {
            _view.OnLinkLabelHelpLinkClicked += View_OnLinkLabelHelp_LinkClicked;
            _view.OnLinkLabelLicenseLinkClicked += View_OnLinkLabelLicense_LinkClicked;
        }

        private static void View_OnLinkLabelLicense_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            var licenseFile = Path.Combine(Path.GetTempPath(), "License.txt");
            File.WriteAllText(licenseFile, Resources.License);
            Process.Start(licenseFile);
        }

        private static void View_OnLinkLabelHelp_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            var helpFile = Path.Combine(Path.GetTempPath(), "Help.txt");
            File.WriteAllText(helpFile, Resources.Help);
            Process.Start(helpFile);
        }
    }
}
