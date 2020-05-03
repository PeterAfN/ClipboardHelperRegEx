using ClipboardHelperRegEx.Properties;
using ClipboardHelperRegEx.Views;
using System;

namespace ClipboardHelperRegEx.BusinessLogic.Presenters
{
    public class PresenterUserSettingsDownButtons
    {
        private readonly ISettingsService _settings;
        private readonly IViewUserSettingsButtonsDown _view;
        private readonly IViewUserSettings _viewUserSettings;

        public PresenterUserSettingsDownButtons
        (
            IViewUserSettingsButtonsDown viewUserSettingsButtonsDown,
            IViewUserSettings viewUserSettings,
            ISettingsService settings
        )
        {
            _view = viewUserSettingsButtonsDown;
            _viewUserSettings = viewUserSettings;
            _settings = settings;

            //subscribe to events
            if (_view != null) _view.Load += OnLoadedViewUserSettings;
        }

        private void OnApplyClicked(object sender, EventArgs e)
        {
            _settings.Save();
            _view.SetStatusOfApplyButton();
            _view.OnSaveIsClicked(e);
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            Settings.Default.Reload();
            _settings.Save();
            _viewUserSettings.Hide();
            _view.OnCancelIsClickedRestoreSettings(e);
        }

        private void OnOkClicked(object sender, EventArgs e)
        {
            _settings.Save();
            _viewUserSettings.Hide();
            _view.OnSaveIsClicked(e);
        }

        private void OnLoadedViewUserSettings(object sender, EventArgs e)
        {
            _view.CreateControls();
            _view.AddControls();
            _view.OkClicked += OnOkClicked;
            _view.CancelClicked += OnCancelClicked;
            _view.ApplyClicked += OnApplyClicked;
        }
    }
}