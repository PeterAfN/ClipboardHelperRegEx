using System;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.Views
{
    public interface IViewUserSettingsRightHelp
    {
        event EventHandler Load;
        LinkLabel LinkLabelLicense { get; }
        LinkLabel LinkLabelHelp { get; }
        event LinkLabelLinkClickedEventHandler OnLinkLabelLicenseLinkClicked;
        event LinkLabelLinkClickedEventHandler OnLinkLabelHelpLinkClicked;
    }
}
