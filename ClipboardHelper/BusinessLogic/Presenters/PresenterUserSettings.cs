using ClipboardHelperRegEx.Views;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.BusinessLogic.Presenters
{
    public class PresenterUserSettings
    {
        private readonly IResourcesService _resources;
        private readonly IViewUserSettings _viewUserSettings;

        public PresenterUserSettings(IViewUserSettings viewUserSettings,
            IResourcesService resources)
        {
            _viewUserSettings = viewUserSettings;
            _resources = resources;

            //set initial form settings
            if (_viewUserSettings == null) return;
            _viewUserSettings.Size = new Size(1200, 800);
            _viewUserSettings.StartPosition = FormStartPosition.CenterScreen;
            if (_resources != null) _viewUserSettings.SetImageFormIcon1(_resources.Closed);
            _viewUserSettings.VisibilityFormIcon2(false);
            _viewUserSettings.VisibilityFormIcon3(false);

            //subscribe to events
            _viewUserSettings.Load += OnLoadedViewUserSettings;
        }

        private void ViewUserSettings_MouseClicksFormIcon1(object sender, EventArgs e)
        {
            _viewUserSettings.Hide();
        }

        private void ViewUserSettings_MouseLeavesFormIcon1(object sender, EventArgs e)
        {
            _viewUserSettings.SetImageFormIcon1(_resources.Closed);
        }

        private void ViewUserSettings_MouseEntersFormIcon1(object sender, EventArgs e)
        {
            _viewUserSettings.SetImageFormIcon1(_resources.ClosedSelected);
        }

        private void OnLoadedViewUserSettings(object sender, EventArgs e)
        {
            _viewUserSettings.InitiateControls();
            _viewUserSettings.AddControls();
            _viewUserSettings.MouseEntersFormIcon1 += ViewUserSettings_MouseEntersFormIcon1;
            _viewUserSettings.MouseLeavesFormIcon1 += ViewUserSettings_MouseLeavesFormIcon1;
            _viewUserSettings.MouseClicksFormIcon1 += ViewUserSettings_MouseClicksFormIcon1;
        }
    }
}