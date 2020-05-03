using ClipboardHelperRegEx.Views;
using System.Windows.Forms;

namespace ClipboardHelper.Views
{
    public partial class ViewUserSettingsRightHelp : UserControl, IViewUserSettingsRightHelp
    {
        public ViewUserSettingsRightHelp()
        {
            InitializeComponent();
            CreateEvents();
        }

        private void CreateEvents()
        {
            linkLabelHelp.LinkClicked += LinkLabelHelp_LinkClicked;
            linkLabelLicense.LinkClicked += LinkLabelLicense_LinkClicked;
        }

        public event LinkLabelLinkClickedEventHandler OnLinkLabelLicenseLinkClicked;
        public event LinkLabelLinkClickedEventHandler OnLinkLabelHelpLinkClicked;

        private void LinkLabelLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OnLinkLabelLicenseLinkClicked?.Invoke(this, e);
        }

        private void LinkLabelHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OnLinkLabelHelpLinkClicked?.Invoke(this, e);
        }

        public LinkLabel LinkLabelLicense
        {
            get { return linkLabelLicense; }
        }

        public LinkLabel LinkLabelHelp
        {
            get { return linkLabelHelp; }
        }

        //Removes almost all flickering when user control and its containing user controls are resized. This affects all controls.
        //unmodified from https://stackoverflow.com/questions/8046560/how-to-stop-flickering-c-sharp-winforms
        protected override CreateParams CreateParams
        {
            get
            {
                var handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000; // WS_EX_COMPOSITED       
                return handleParam;
            }
        }
    }
}
