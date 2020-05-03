using AdysTech.CredentialManager;
using ClipboardHelperRegEx.Properties;
using ClipboardHelperRegEx.Views;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.BusinessLogic.Presenters
{
    public class PresenterUserSettingsRightAppearance
    {
        private readonly IViewDialog _dialog;
        private readonly ISettingsService _settings;
        private readonly IViewUserSettingsRightAppearance _view;
        private readonly IViewMain _viewMain;
        private readonly IViewUserSettingsButtonsDown _viewUserSettingsButtonsDown;
        private readonly Validate _validate;
        private NetworkCredential _cred;


        public PresenterUserSettingsRightAppearance
        (
            IViewUserSettingsRightAppearance view,
            ISettingsService settings,
            IViewUserSettingsButtonsDown viewUserSettingsButtonsDown,
            IViewMain viewMain,
            Validate validate,
            IViewDialog dialog
        )
        {
            _view = view;
            _settings = settings;
            _viewUserSettingsButtonsDown = viewUserSettingsButtonsDown;
            _viewMain = viewMain;
            _validate = validate;
            _dialog = dialog;

            //subscribe to events   
            if (view != null) view.Load += View_Load;

            _cred = new NetworkCredential();
        }

        private void View_OnProgramsAlternativePasting_TextChanged(object sender, EventArgs e)
        {
            _settings.AppearanceProgramsAlternativePasting = _view.ProgramsAlternativePasting.Text;
        }

        /// <summary>
        ///     When these special settings are changed from the main window,
        ///     the settings in the settings window must get updated also.
        ///     These two settings are special since they are 2-way settings.
        ///     They must be reflected att all times.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_settings.Locked)
            {
                if (_view != null) _view.IsClosingEnabled.Checked = false;
            }
            else
            {
                if (_view != null) _view.IsClosingEnabled.Checked = true;
            }
        }

        private void View_TextChangedSeconds(object sender, EventArgs e)
        {
            _settings.AppearanceSecondsShowing = ParseOrDefault(_view.Seconds.Text);
        }

        private static int ParseOrDefault(string someStr)
        {
            return int.TryParse(someStr, out var result) ? result : 0;
        }

        private void View_Load(object sender, EventArgs e)
        {
            _view.OnProgramsAlternativePastingTextChanged += View_OnProgramsAlternativePasting_TextChanged;
            _view.SelectedItemChangedShortcutKeys += View_SelectedItemChangedShortcutKeys;
            _view.SelectedItemChangedFadeTypeChoice += View_SelectedItemChangedFadeTypeChoice;
            _view.CheckedChangedIsWinEnabled += View_CheckedChangedIsWINEnabled;
            _view.CheckedChangedIsShiftEnabled += View_CheckedChangedIsSHIFTEnabled;
            _view.CheckedChangedIsAltEnabled += View_CheckedChangedIsALTEnabled;
            _view.CheckedChangedIsCtrlEnabled += View_CheckedChangedIsCTRLEnabled;
            _view.CheckedChangedIsFocusEnabled += View_CheckedChangedIsFocusEnabled;
            _view.CheckedChangedIsAutoStartEnabled += View_CheckedChangedIsAutoStartEnabled;
            _view.CheckedChangedIsHidingEnabled += View_CheckedChangedIsHidingEnabled;
            _view.TextChangedSeconds += View_TextChangedSeconds;
            _view.OnAppearanceColorChoicesSelectedIndexChanged += View_OnAppearanceColorChoices_SelectedIndexChanged;
            _view.OnLabelTextColorSampleMouseClick += View_OnLabelTextColorSample_MouseClick;
            _dialog.ClickCancelMouseButton += Dialog_ClickCancelMouseButton;
            _dialog.ClickOkMouseButton += Dialog_ClickOkMouseButton;
            _view.ResetToFactorySettings.Click += ResetToFactorySettings_Click1;
            _viewUserSettingsButtonsDown.OkClicked += ViewUserSettingsButtonsDown_OkClicked;
            _viewUserSettingsButtonsDown.CancelClicked += ViewUserSettingsButtonsDown_CancelClicked;
            _viewUserSettingsButtonsDown.ApplyClicked += ViewUserSettingsButtonsDown_ApplyClicked;
            _settings.PropertyChanged += Settings_PropertyChanged;

            _view.Loaded = true;
            _view.IsClosingEnabled.Checked = !_settings.Locked;
            _view.DoAutoStartProgram.Checked = _settings.AppearanceAutoStart;
            _view.IsFocusEnabled.Checked = _settings.AppearanceFocus;
            _view.IsCtrlEnabled.Checked = _settings.AppearanceCtrl;
            _view.IsAltEnabled.Checked = _settings.AppearanceAlt;
            _view.IsShiftEnabled.Checked = _settings.AppearanceShift;
            _view.IsWinEnabled.Checked = _settings.AppearanceWin;
            _view.FadeTypeChoice.SelectedItem = _settings.AppearanceFadeChoices;
            _view.ShortcutKeysComboBox.SelectedItem = _settings.AppearanceSelectedKey;
            _view.FadingText.Enabled = !_settings.Locked;
            _view.SecondsText.Enabled = !_settings.Locked;
            _view.Seconds.Text = _settings.AppearanceSecondsShowing.ToString(System.Globalization.CultureInfo.InvariantCulture);
            _view.Seconds.Enabled = !_settings.Locked;
            View_CheckedChangedIsHidingEnabled(sender, e);
            _view.EnabledChangedView += View_EnabledChanged;
            _view.ProgramsAlternativePasting.Text = _settings.AppearanceProgramsAlternativePasting;
            _view.OnViewUserSettingsRightAppearanceVisibleChanged +=
                View_OnViewUserSettingsRightAppearance_VisibleChanged;

            _view.AppearanceColorChoices.SelectedItem = _settings.AppearanceColorChoices;
            _view.LabelTextColorSample.ForeColor = _settings.AppearanceColorTitle;
            if (CredentialManager.GetCredentials("ClipboardHelper") != null)
            {
                _view.Password.Text = CredentialManager.GetCredentials("ClipboardHelper").Password;
                _cred = CredentialManager.GetCredentials("ClipboardHelper");
            }

            _view.Password.TextChanged += Password_TextChanged;
        }


        private void ResetToFactorySettings_Click1(object sender, EventArgs e)
        {
            _dialog.Tag = "ResetToFactorySettingsClocked";
            _dialog.UserInput.Hide();
            _dialog.SetText("Do you want to reset to Factory settings?" +
                            " The program will be restarted.");
            _dialog.Show();
        }

        private void Dialog_ClickOkMouseButton(object sender, EventArgs e)
        {
            if (_dialog.Tag.ToString() != "ResetToFactorySettingsClocked") return;
            Validate.Reset = true;
            _validate.MergeNewXmlWithOldXmlAuto();
            _validate.MergeNewXmlWithOldXmlManual();
            Settings.Default.Reset();
            Settings.Default.Reload();
            Settings.Default.Save();
            SettingsService.ApplyProgramAutoStartSettingToRegistry();
            SettingsService.SaveFormLowerRightScreenPositionToSettings();
            if (!Debugger.IsAttached)
                Application.Restart();
        }

        private static void Dialog_ClickCancelMouseButton(object sender, EventArgs e)
        {
        }

        private void Password_TextChanged(object sender, EventArgs e)
        {
            _cred.Password = ((TextBox)sender).Text;
        }

        private void ViewUserSettingsButtonsDown_ApplyClicked(object sender, EventArgs e)
        {
            _cred.Domain = Environment.UserDomainName;
            _cred.UserName = "ClipboardHelper";
            CredentialManager.SaveCredentials("ClipboardHelper", _cred);
        }

        private static void ViewUserSettingsButtonsDown_CancelClicked(object sender, EventArgs e)
        {
        }

        private void ViewUserSettingsButtonsDown_OkClicked(object sender, EventArgs e)
        {
            _cred.Domain = Environment.UserDomainName;
            _cred.UserName = "ClipboardHelper";
            CredentialManager.SaveCredentials("ClipboardHelper", _cred);
        }

        private void View_OnLabelTextColorSample_MouseClick(object sender, MouseEventArgs e)
        {
            // Show the color dialog.
            _view.ColorDialog.CustomColors = new[]
            {
                ColorToInt(Color.LightSteelBlue),
                ColorToInt(Color.YellowGreen),
                ColorToInt(Color.SteelBlue),
                ColorToInt(Color.DeepSkyBlue)
            };
            var result = _view.ColorDialog.ShowDialog();
            // See if user pressed ok.
            if (result != DialogResult.OK) return;
            switch ((string)_view.AppearanceColorChoices.SelectedItem)
            {
                case "OutlookSearch":
                    _settings.AppearanceColorOutlookSearch = _view.ColorDialog.Color;
                    break;
                case "Caption":
                    _settings.AppearanceColorTitle = _view.ColorDialog.Color;
                    break;
                case "Info":
                    _settings.AppearanceColorInfo = _view.ColorDialog.Color;
                    break;
                case "WebUrlGoTo":
                    _settings.AppearanceColorWebUrlGoTo = _view.ColorDialog.Color;
                    break;
                case "LineSelection":
                    _settings.AppearanceColorSelection = _view.ColorDialog.Color;
                    break;
            }

            _view.LabelTextColorSample.ForeColor = _view.ColorDialog.Color;
        }

        private static int ColorToInt(Color color)
        {
            return color.R | (color.G << 8) | (color.B << 16);
        }

        private void View_OnAppearanceColorChoices_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedText = (string)_view.AppearanceColorChoices.SelectedItem;
            switch (selectedText)
            {
                case "OutlookSearch":
                    _view.LabelTextColorSample.ForeColor = Settings.Default.appearanceColorOutlookSearch;
                    break;
                case "Caption":
                    _view.LabelTextColorSample.ForeColor = Settings.Default.appearanceColorTitle;
                    break;
                case "Info":
                    _view.LabelTextColorSample.ForeColor = Settings.Default.appearanceColorInfo;
                    break;
                case "WebUrlGoTo":
                    _view.LabelTextColorSample.ForeColor = Settings.Default.appearanceColorWebUrlGoTo;
                    break;
                case "LineSelection":
                    _view.LabelTextColorSample.ForeColor = Settings.Default.appearanceColorSelection;
                    break;
            }
        }

        /// <summary>
        ///     Is only run from the program icon context menu.
        ///     This method is useful only in a case when the setting has already been loaded once and cancel pressed --> this is
        ///     hidden.
        ///     When shown again, then it will update values since loaded is only ran when its initially shown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void View_OnViewUserSettingsRightAppearance_VisibleChanged(object sender, EventArgs e)
        {
            _view.IsClosingEnabled.Checked = !_settings.Locked;
            _view.DoAutoStartProgram.Checked = _settings.AppearanceAutoStart;
            _view.IsFocusEnabled.Checked = _settings.AppearanceFocus;
            _view.IsCtrlEnabled.Checked = _settings.AppearanceCtrl;
            _view.IsAltEnabled.Checked = _settings.AppearanceAlt;
            _view.IsShiftEnabled.Checked = _settings.AppearanceShift;
            _view.IsWinEnabled.Checked = _settings.AppearanceWin;
            _view.FadeTypeChoice.SelectedItem = _settings.AppearanceFadeChoices;
            _view.ShortcutKeysComboBox.SelectedItem = _settings.AppearanceSelectedKey;
            _view.FadingText.Enabled = !_settings.Locked;
            _view.SecondsText.Enabled = !_settings.Locked;
            _view.Seconds.Text = _settings.AppearanceSecondsShowing.ToString(System.Globalization.CultureInfo.InvariantCulture);
            _view.Seconds.Enabled = !_settings.Locked;
            _view.ProgramsAlternativePasting.Text = _settings.AppearanceProgramsAlternativePasting;
            _view.AppearanceColorChoices.SelectedItem = _settings.AppearanceColorChoices;
            _view.LabelTextColorSample.ForeColor = _settings.AppearanceColorTitle;
        }

        private void View_EnabledChanged(object sender, EventArgs e)
        {
            //Run every time this presenters view is shown, except when loaded. It is not run run when shown from the program icon context menu
            if (_view.Enabled && _view.Loaded) View_CheckedChangedIsHidingEnabled(sender, e);
        }

        private void View_CheckedChangedIsHidingEnabled(object sender, EventArgs e)
        {
            if (_view.IsClosingEnabled.Checked)
            {
                _view.Seconds.Enabled = true;
                _view.FadingText.Enabled = true;
                _view.SecondsText.Enabled = true;
                _view.FadeTypeChoice.Enabled = true;
                _settings.Locked = false;
            }
            else
            {
                _view.Seconds.Enabled = false;
                _view.FadingText.Enabled = false;
                _view.SecondsText.Enabled = false;
                _view.FadeTypeChoice.Enabled = false;
                _settings.Locked = true;
            }
        }

        private void View_CheckedChangedIsAutoStartEnabled(object sender, EventArgs e)
        {
            _settings.AppearanceAutoStart = _view.DoAutoStartProgram.Checked;
        }

        private void View_CheckedChangedIsFocusEnabled(object sender, EventArgs e)
        {
            _settings.AppearanceFocus = _view.IsFocusEnabled.Checked;
            _viewMain.FormGetsFocus = _settings.AppearanceFocus;
        }

        private void View_CheckedChangedIsCTRLEnabled(object sender, EventArgs e)
        {
            _settings.AppearanceCtrl = _view.IsCtrlEnabled.Checked;
        }

        private void View_CheckedChangedIsALTEnabled(object sender, EventArgs e)
        {
            _settings.AppearanceAlt = _view.IsAltEnabled.Checked;
        }

        private void View_CheckedChangedIsSHIFTEnabled(object sender, EventArgs e)
        {
            _settings.AppearanceShift = _view.IsShiftEnabled.Checked;
        }

        private void View_CheckedChangedIsWINEnabled(object sender, EventArgs e)
        {
            _settings.AppearanceWin = _view.IsWinEnabled.Checked;
        }

        private void View_SelectedItemChangedFadeTypeChoice(object sender, EventArgs e)
        {
            _settings.AppearanceFadeChoices = (string)_view.FadeTypeChoice.SelectedItem;
        }

        private void View_SelectedItemChangedShortcutKeys(object sender, EventArgs e)
        {
            _settings.AppearanceSelectedKey = (string)_view.ShortcutKeysComboBox.SelectedItem;
        }
    }
}