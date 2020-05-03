using ClipboardHelperRegEx.Views;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.BusinessLogic.Presenters
{
    public class PresenterAbout
    {
        private readonly IResourcesService _resources;
        private readonly IViewAbout _viewAbout;

        public PresenterAbout(IViewAbout viewAbout,
            IResourcesService resources)
        {
            _viewAbout = viewAbout;
            _resources = resources;

            //set initial form settings
            if (_viewAbout == null) return;
            _viewAbout.Size = new Size(660, 423);
            _viewAbout.StartPosition = FormStartPosition.CenterScreen;
            if (_resources != null) _viewAbout.SetImageFormIcon1(_resources.Closed);
            _viewAbout.VisibilityFormIcon2(false);
            _viewAbout.VisibilityFormIcon3(false);
            _viewAbout.FormResizable = false;
            _viewAbout.Size = new Size(449, 229);

            //subscribe to events
            _viewAbout.Load += OnLoadedViewAboutSettings;
            _viewAbout.MouseEntersFormIcon1 += OnMouseEntersFormIcon1;
            _viewAbout.MouseLeavesFormIcon1 += OnMouseLeavesFormIcon1;
            _viewAbout.MouseClicksFormIcon1 += OnMouseClicksFormIcon1;
            _viewAbout.ClickedOkButton += OnClickedOkButton;
        }

        private void OnClickedOkButton(object sender, EventArgs args)
        {
            _viewAbout.Hide();
        }

        private void OnMouseEntersFormIcon1(object sender, EventArgs e)
        {
            _viewAbout.SetImageFormIcon1(_resources.ClosedSelected);
        }

        private void OnMouseLeavesFormIcon1(object sender, EventArgs e)
        {
            _viewAbout.SetImageFormIcon1(_resources.Closed);
        }

        private void OnMouseClicksFormIcon1(object sender, EventArgs e)
        {
            _viewAbout.Hide();
        }

        private void OnLoadedViewAboutSettings(object sender, EventArgs e)
        {
            _viewAbout.SetProduct(AssemblyInformation.AssemblyProduct);
            _viewAbout.SetVersion(AssemblyInformation.AssemblyVersion);
            _viewAbout.SetCopyright(AssemblyInformation.AssemblyCopyright);
            _viewAbout.SetCompany(AssemblyInformation.AssemblyCompany);
            _viewAbout.SetDescription(AssemblyInformation.AssemblyDescription + "\r\n\r\n" +
            "This project includes code from these developers:\r\n\r\n- AdysTech.CredentialManager\r\nhttps://github.com/AdysTech/CredentialManager\r\n- Version 1.9.1\r\n- License: MIT License\r\nhttps://github.com/AdysTech/CredentialManager/blob/master/License.md\r\n\r\n- csv\r\nhttps://github.com/stevehansen/csv/\r\n- Version 1.0.38\r\n- License: MIT License\r\nhttps://github.com/stevehansen/csv/blob/master/LICENSE\r\n\r\n- InputSimulator\r\nhttps://github.com/michaelnoonan/inputsimulator\r\n- Version 1.0.4\r\n- License: MIT License\r\nhttps://github.com/michaelnoonan/inputsimulator/blob/master/LICENSE\r\n\r\n- MetroModernUI\r\nhttps://github.com/dennismagno/metroframework-modern-ui\r\n- Version 1.4.0\r\n- License: MIT License\r\nhttps://github.com/dennismagno/metroframework-modern-ui/blob/master/LICENSE.md\r\n\r\n- MouseKeyHook\r\nhttps://github.com/gmamaladze/globalmousekeyhook\r\n- Version 5.6.0\r\n- License: MIT License\r\nhttps://github.com/gmamaladze/globalmousekeyhook/blob/vNext/LICENSE.txt\r\n\r\n- Newtonsoft.Json\r\nhttps://github.com/JamesNK/Newtonsoft.Json\r\n- Version 12.0.3\r\n- License: MIT License\r\nhttps://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md\r\n\r\n- NHotkey\r\nhttps://github.com/thomaslevesque/NHotkey\r\n- Version 2.0.0\r\n- License: Apache License 2.0\r\nhttps://github.com/thomaslevesque/NHotkey/blob/master/LICENSE.md\r\n\r\n- NHotkey.WindowsForms\r\nhttps://github.com/thomaslevesque/NHotkey\r\n- Version 2.0.0\r\n- License: Apache License 2.0\r\nhttps://github.com/thomaslevesque/NHotkey/blob/master/LICENSE.md"
            );
        }
    }
}