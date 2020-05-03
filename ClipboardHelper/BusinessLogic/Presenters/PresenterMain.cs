using ClipboardHelperRegEx.Views;
using ClipboardHelperRegEx.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;
using ClipboardHelper.Views;

namespace ClipboardHelperRegEx.BusinessLogic.Presenters
{
    public sealed class PresenterMain : IDisposable
    {
        private static readonly object Object = new object();
        private readonly NotifyIcon _notifyIcon = new NotifyIcon();
        private readonly Pasting _pasting;
        private readonly IResourcesService _resources;
        private readonly ISettingsService _settings;
        private readonly ShortcutKeys _shortcutKeys = new ShortcutKeys();
        private readonly IViewMain _view;
        private readonly IViewAbout _viewAbout;
        private readonly IViewMainSplContPanelUpTabs _viewMainSplContPanelUp;
        private readonly IViewMin _viewMin;
        private readonly IViewUserSettings _viewUserSettings;
        private readonly IViewUserSettingsButtonsDown _viewUserSettingsButtonsDown;
        private FormFading _formHiding;
        private FormVisibility _formMinVisibility;
        private bool _whenFinishedComplementaryWork;

        private ViewPureTextInfo _viewPureTextInfo;

        public PresenterMain(
            IViewMain viewMain,
            IViewMin viewMin,
            IViewUserSettings viewUserSettings,
            IViewAbout viewAbout,
            ISettingsService settings,
            IResourcesService resources,
            IViewMainSplContPanelUpTabs viewMainSplContPanelUp,
            IViewUserSettingsButtonsDown viewUserSettingsButtonsDown,
            Pasting pasting
        )
        {
            _view = viewMain;
            _viewMin = viewMin;
            _viewAbout = viewAbout;
            _settings = settings;
            _resources = resources;
            _viewUserSettings = viewUserSettings;
            _viewMainSplContPanelUp = viewMainSplContPanelUp;
            _viewUserSettingsButtonsDown = viewUserSettingsButtonsDown;
            _pasting = pasting;

            //set initial form settings
            if (_settings != null)
            {
                if (_view != null)
                {
                    _view.Size = _settings.SizeMain;
                    _view.StartPosition = FormStartPosition.Manual;
                    _view.Location = _settings.Location;
                }
            }

            if (settings != null && settings.Activated)
            {
                if (_resources != null)
                    _view?.SetNotifyIconImage(IsWindowsLightThemeActive()
                        ? _resources.ActivatedForLightTheme
                        : _resources.ActivatedForDarkTheme);
            }
            else
            {
                if (_resources != null)
                    _view?.SetNotifyIconImage(IsWindowsLightThemeActive()
                        ? _resources.DeactivatedForLightTheme
                        : _resources.DeactivatedForDarkTheme);
            }

            if (_view == null) return;
            _view.SetNotifyIconVisible(true);
            if (_resources != null)
            {
                _view.SetImageFormIcon1(settings != null && settings.Locked ? _resources.Locked : _resources.Unlocked);
                _view.SetImageFormIcon2(_resources.MinimizeUnselected);
                _view.SetImageFormIcon3(_resources.Closed);
            }

            //subscribe to events
            _view.Load += OnLoadView;
        }

        private bool ItemsSelected { get; set; }

        private static bool IsWindowsLightThemeActive()
        {
            var isLightMode = true;
            var v = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                "SystemUsesLightTheme", "1");
            if (v != null && v.ToString() == "0")
                isLightMode = false;
            return isLightMode;
        }

        private void OnLoadView(object sender, EventArgs e)
        {
            _view.LabelTitleTop.Text = Resources.PresenterMain_OnLoadView_Clipboard_Helper;
            InitiateFormHiding();
            _view.InitiateControls();
            _view.AddControls();
            _viewMainSplContPanelUp.ItemsSelected += ViewMainSplContPanelUp_OnItemsSelected;
            _viewMainSplContPanelUp.NoItemsSelected += ViewMainSplContPanelUp_OnItemsSelected_OnNoItemsSelected;
            _viewMainSplContPanelUp.NewClipboardText += ViewMainSplContPanelUpAutoShownTabs_NewClipboardText;
            _viewMin.MouseEnterLabelTitleTop += ViewMin_MouseEnterLabelTitleTop;
            SetAppShortcutKey();
            _view.FormGetsFocus = _settings.AppearanceFocus;
            _pasting.OnPastingActivated += Pasting_OnPastingActivated;
            _pasting.OnPastingDeactivated += MultiPasting_Deactivated;
            List<Icon> notifyIconsDynamic;
            if (IsWindowsLightThemeActive())
            {
                notifyIconsDynamic = new List<Icon>
                {
                    _resources.DynamicIcon1ForLightTheme,
                    _resources.DynamicIcon2ForLightTheme,
                    _resources.DynamicIcon3ForLightTheme,
                    _resources.DynamicIcon4ForLightTheme,
                    _resources.DynamicIcon5ForLightTheme,
                    _resources.DynamicIcon6ForLightTheme,
                    _resources.DynamicIcon7ForLightTheme,
                    _resources.DynamicIcon8ForLightTheme
                };
                _notifyIcon.Initiate(_resources.ActivatedForLightTheme, notifyIconsDynamic, _view.SetNotifyIconImage);
            }
            else
            {
                notifyIconsDynamic = new List<Icon>
                {
                    _resources.DynamicIcon1ForDarkTheme,
                    _resources.DynamicIcon2ForDarkTheme,
                    _resources.DynamicIcon3ForDarkTheme,
                    _resources.DynamicIcon4ForDarkTheme,
                    _resources.DynamicIcon5ForDarkTheme,
                    _resources.DynamicIcon6ForDarkTheme,
                    _resources.DynamicIcon7ForDarkTheme,
                    _resources.DynamicIcon8ForDarkTheme
                };
                _notifyIcon.Initiate(_resources.ActivatedForDarkTheme, notifyIconsDynamic, _view.SetNotifyIconImage);
            }

            _viewUserSettingsButtonsDown.CancelIsClickedRestoreSettings +=
                ViewUserSettingsButtonsDown_OnCancelIsClickedRestoreSettings;
            _view.ClickToolStripMenuItemSettings += OnClickToolStripMenuItemSettings;
            _view.ClickToolStripMenuItemVisa += OnClickToolStripMenuItemVisa;
            _view.ClickToolStripMenuItemOm += OnClickToolStripMenuItemOm;
            _view.ClickToolStripMenuItemEnd += OnClickToolStripMenuItemEnd;
            _view.ClickToolStripMenuItemDeActivate += OnClickToolStripMenuItemDeactivate;
            _view.MouseUpNotifyIconProgram += OnMouseUpNotifyIconProgram;
            _view.MouseEntersFormIcon1 += OnMouseEntersFormIcon1;
            _view.MouseLeavesFormIcon1 += OnMouseLeavesFormIcon1;
            _view.MouseClicksFormIcon1 += OnMouseClicksFormIcon1;
            _view.MouseEntersFormIcon2 += OnMouseEntersFormIcon2;
            _view.MouseLeavesFormIcon2 += OnMouseLeavesFormIcon2;
            _view.MouseClicksFormIcon2 += OnMouseClicksFormIcon2;
            _view.MouseEntersFormIcon3 += OnMouseEntersFormIcon3;
            _view.MouseLeavesFormIcon3 += OnMouseLeavesFormIcon3;
            _view.MouseClicksFormIcon3 += OnMouseClicksFormIcon3;
            _view.ResizingEnd += OnResizingEnd;
            _settings.PropertyChanged += Settings_PropertyChanged;
            _viewUserSettingsButtonsDown.SaveIsClicked += ViewUserSettingsButtonsDown_OnSaveIsClicked;
        }

        private void Pasting_OnPastingActivated(object sender, PastingActivatedEventArgs e)
        {
            _notifyIcon.SetDynamic();
        }

        private void ViewUserSettingsButtonsDown_OnSaveIsClicked(object sender, EventArgs e)
        {
            if (_formHiding != null)
                _formHiding.CursorTimeOutsideFormUntilStartHiding = _settings.AppearanceSecondsShowing;
            SetAppShortcutKey();
        }

        private void ViewUserSettingsButtonsDown_OnCancelIsClickedRestoreSettings(object sender, EventArgs e)
        {
            _view.SetImageFormIcon1(_settings.Locked ? _resources.Locked : _resources.Unlocked);
        }

        private void InitiateFormHiding()
        {
            _formHiding = new FormFading((Form)_view)
            {
                CursorTimeOutsideFormUntilStartHiding = _settings.AppearanceSecondsShowing
            };
            _formHiding.WorkFinished += FormHiding_WorkFinished;
            _formHiding.Shown += FormHiding_Shown;
            SetVisibilityStatus(FormFading.Visibility.ValueInSettings, FormFading.Visibility.ValueInSettings,
                FormFading.ChangeVisibility.Instantly, FormFading.ApplyWhen.Instantly);
        }

        /// <summary>
        ///     When program has matched with new a new Clipboard text.
        /// </summary>
        private void ViewMainSplContPanelUpAutoShownTabs_NewClipboardText(object sender, EventArgs e)
        {
            SetVisibilityStatus(FormFading.Visibility.Showing, FormFading.Visibility.Hiding,
                FormFading.ChangeVisibility.Gradually, FormFading.ApplyWhen.Instantly);
        }

        /// <summary>
        ///     Always when mouse enters min form, it gets hidden and main form is shown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewMin_MouseEnterLabelTitleTop(object sender, EventArgs e)
        {
            SetVisibilityStatus(FormFading.Visibility.Showing, FormFading.Visibility.Hiding,
                FormFading.ChangeVisibility.Gradually, FormFading.ApplyWhen.Instantly, false);
        }

        /// <summary>
        ///     When certain settings are changed, view must be updated immediately.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Locked":
                    _view?.SetImageFormIcon1(_settings.Locked ? _resources.Locked : _resources.Unlocked);
                    break;
            }
        }

        /// <summary>
        ///     Sets main and min forms visibility. Note: Min forms visibility is automatically set in method
        ///     FormHiding_StateHasChanged()
        ///     afterwards when main form visibility setting has been finished in formHiding.SetStatusTo().
        /// </summary>
        /// <param name="mainVisibility"></param>
        /// <param name="minVisibility"></param>
        /// <param name="visibilityChangeType"></param>
        /// <param name="applyWhen"></param>
        /// <param name="whenFinishedComplementaryWork"></param>
        private void SetVisibilityStatus(FormFading.Visibility mainVisibility, FormFading.Visibility minVisibility,
            FormFading.ChangeVisibility visibilityChangeType = FormFading.ChangeVisibility.NotDefined,
            FormFading.ApplyWhen applyWhen = FormFading.ApplyWhen.NotDefined, bool whenFinishedComplementaryWork = true)
        {
            lock (Object)
            {
                switch (mainVisibility)
                {
                    case FormFading.Visibility.Hiding:
                    case FormFading.Visibility.Showing:
                        _formHiding.SetStatusTo(mainVisibility, visibilityChangeType, applyWhen);
                        break;
                    case FormFading.Visibility.ValueInSettings:
                        if (applyWhen == FormFading.ApplyWhen.Instantly)
                            _formHiding.SetStatusTo(FormFading.Visibility.Hiding, FormFading.ChangeVisibility.Instantly,
                                FormFading.ApplyWhen.Instantly); //for shortcut key
                        else
                            switch (_settings.AppearanceFadeChoices)
                            {
                                case "Fade out and minimize":
                                    _formHiding.SetStatusTo(FormFading.Visibility.Hiding,
                                        FormFading.ChangeVisibility.Gradually,
                                        FormFading.ApplyWhen.WhenMouseLeavesForm);
                                    break;
                                case "Fade out and close":
                                    _formHiding.SetStatusTo(FormFading.Visibility.Hiding,
                                        FormFading.ChangeVisibility.Gradually,
                                        FormFading.ApplyWhen.WhenMouseLeavesForm);
                                    break;
                                case "Minimize":
                                    _formHiding.SetStatusTo(FormFading.Visibility.Hiding,
                                        FormFading.ChangeVisibility.Instantly,
                                        FormFading.ApplyWhen.WhenMouseLeavesForm);
                                    break;
                                case "Close":
                                    _formHiding.SetStatusTo(FormFading.Visibility.Hiding,
                                        FormFading.ChangeVisibility.Instantly,
                                        FormFading.ApplyWhen.WhenMouseLeavesForm);
                                    break;
                            }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(mainVisibility), mainVisibility, null);
                }

                _formMinVisibility = (FormVisibility)minVisibility;
                _whenFinishedComplementaryWork = whenFinishedComplementaryWork;
            }
        }

        /// <summary>
        ///     Is run when when forms visibility change has finished in method SetVisibilityStatus(). Purpose is to also set
        ///     the min forms visibility.
        /// </summary>
        private void FormHiding_WorkFinished(object sender, EventArgs args)
        {
            if (!_whenFinishedComplementaryWork) return;
            switch (_formMinVisibility)
            {
                case FormVisibility.Showing:
                    _viewMin.Show();
                    break;
                case FormVisibility.Hiding:
                    _viewMin.Hide();
                    break;
                case FormVisibility.ValueInSettings:
                    switch (_settings.AppearanceFadeChoices)
                    {
                        case "Fade out and minimize":
                        case "Minimize":
                            _viewMin.Show();
                            break;
                        case "Fade out and close":
                        case "Close":
                            _viewMin.Hide();
                            break;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sender),
                        Resources.PresenterMain_FormHiding_WorkFinished_An_error_occured_while_setting_the_min_forms_visibility_);
            }
        }

        /// <summary>
        ///     Set the programs global shortcut keys from settings (hard drive) to memory (ram)
        /// </summary>
        private void SetAppShortcutKey()
        {
            var appShortcutKeys = (_settings.AppearanceAlt ? Keys.Alt : Keys.None) |
                                  (_settings.AppearanceCtrl ? Keys.Control : Keys.None) |
                                  (_settings.AppearanceShift ? Keys.Shift : Keys.None) |
                                  (_settings.AppearanceWin ? Keys.LWin : Keys.None) |
                                  (Keys)Enum.Parse(typeof(Keys), _settings.AppearanceSelectedKey);
            _shortcutKeys.AddShortcutKey(ToString(), appShortcutKeys, OnShortCutKeysPressed);
        }

        /// <summary>
        ///     Logic for when correct global shortcut keys are pressed.
        /// </summary>
        private void OnShortCutKeysPressed()
        {
            if (_settings.Activated)
                SetVisibilityStatus(FormFading.Visibility.Showing, FormFading.Visibility.Hiding,
                    FormFading.ChangeVisibility.Gradually, FormFading.ApplyWhen.Instantly);
        }

        /// <summary>
        ///     Is run when form is shown and has its full opacity. This is used when going from min size form to normal size form.
        /// </summary>
        private void FormHiding_Shown(object sender, EventArgs args)
        {
            if (ItemsSelected && !_settings.Locked)
                SetVisibilityStatus(FormFading.Visibility.ValueInSettings, FormFading.Visibility.ValueInSettings,
                    FormFading.ChangeVisibility.NotDefined, FormFading.ApplyWhen.WhenMouseLeavesForm);
            if (!_settings.AppearanceFocus) return;
            _view.Activate();
            _view.ShowInactiveTopmost(_view as Form);
        }

        /// <summary>
        ///     Form can't get automatically hidden if no items are selected.
        /// </summary>
        private void ViewMainSplContPanelUp_OnItemsSelected_OnNoItemsSelected(object sender, EventArgs e)
        {
            ItemsSelected = false;
        }

        /// <summary>
        ///     Form can get automatically hidden if items are selected.
        /// </summary>
        private void ViewMainSplContPanelUp_OnItemsSelected(object sender, EventArgs e)
        {
            ItemsSelected = true;
            if (!_settings.Locked && _view.Visible)
                SetVisibilityStatus(FormFading.Visibility.ValueInSettings, FormFading.Visibility.ValueInSettings,
                    FormFading.ChangeVisibility.NotDefined, FormFading.ApplyWhen.WhenMouseLeavesForm);
        }

        /// <summary>
        ///     lock icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseEntersFormIcon1(object sender, EventArgs e)
        {
            _view.SetImageFormIcon1(_settings.Locked ? _resources.LockedSelected : _resources.UnlockedSelected);
        }

        /// <summary>
        ///     lock icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseLeavesFormIcon1(object sender, EventArgs e)
        {
            _view.SetImageFormIcon1(_settings.Locked ? _resources.Locked : _resources.Unlocked);
        }

        /// <summary>
        ///     lock icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseClicksFormIcon1(object sender, EventArgs e) //lock button clicked
        {
            if (_settings.Locked)
            {
                _view.SetImageFormIcon1(_resources.UnlockedSelected); //unlock
                if (ItemsSelected)
                    SetVisibilityStatus(FormFading.Visibility.ValueInSettings, FormFading.Visibility.ValueInSettings,
                        FormFading.ChangeVisibility.NotDefined, FormFading.ApplyWhen.WhenMouseLeavesForm);
            }
            else
            {
                _view.SetImageFormIcon1(_resources.LockedSelected); //lock
                SetVisibilityStatus(FormFading.Visibility.Showing, FormFading.Visibility.Hiding,
                    FormFading.ChangeVisibility.Gradually, FormFading.ApplyWhen.Instantly);
            }

            _settings.Locked = !_settings.Locked;
            _settings.Save();
        }

        /// <summary>
        ///     minimize icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseEntersFormIcon2(object sender, EventArgs e)
        {
            _view.SetImageFormIcon2(_resources.MinimizeSelected);
        }

        /// <summary>
        ///     minimize icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseLeavesFormIcon2(object sender, EventArgs e)
        {
            _view.SetImageFormIcon2(_resources.MinimizeUnselected);
        }

        /// <summary>
        ///     minimize icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseClicksFormIcon2(object sender, EventArgs e)
        {
            _formHiding.Enabled = false; //don't trigger mouse leave form accidentally.
            SetVisibilityStatus(FormFading.Visibility.Hiding, FormFading.Visibility.Showing,
                FormFading.ChangeVisibility.Instantly, FormFading.ApplyWhen.Instantly);
        }

        /// <summary>
        ///     close icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseEntersFormIcon3(object sender, EventArgs e)
        {
            _view.SetImageFormIcon3(_resources.ClosedSelected);
        }

        /// <summary>
        ///     close icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseLeavesFormIcon3(object sender, EventArgs e)
        {
            _view.SetImageFormIcon3(_resources.Closed);
        }

        /// <summary>
        ///     close icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseClicksFormIcon3(object sender, EventArgs e)
        {
            _formHiding.Enabled = false; //don't trigger mouse leave form accidentally.
            SetVisibilityStatus(FormFading.Visibility.Hiding, FormFading.Visibility.Hiding,
                FormFading.ChangeVisibility.Instantly, FormFading.ApplyWhen.Instantly);
        }

        /// <summary>
        ///     System tray icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseUpNotifyIconProgram(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    try
                    {
                        if (!string.IsNullOrEmpty(Clipboard.Text))
                        {
                            var stringLength = Clipboard.Text.Length;
                            if (stringLength > 1000000)
                            {
                                _viewPureTextInfo = new ViewPureTextInfo(
                                    "Max limit of 1 000 000 characters reached! Clipboard text formatting has not been removed!");
                            }
                            else
                            {
                                SetTextClipboard.Start(Clipboard.Text);
                                _viewPureTextInfo = new ViewPureTextInfo("Text formatting removed from Clipboard!");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(Resources.PresenterMain_OnMouseUpNotifyIconProgram_An_error_occured_when_setting_value_to_the_clipboard__ + ex, Resources.PresenterMain_OnMouseUpNotifyIconProgram_Clipboard_Helper_error, MessageBoxButtons.OK);
                        throw;
                    }

                    break;
                case MouseButtons.None:
                    break;
                case MouseButtons.Right:
                    _view.ShowContextMenu(new Point(Control.MousePosition.X, Control.MousePosition.Y - 126));
                    break;
                case MouseButtons.Middle:
                    if (!_view.Visible && _settings.Activated)
                        SetVisibilityStatus(FormFading.Visibility.Showing, FormFading.Visibility.Hiding,
                            FormFading.ChangeVisibility.Gradually, FormFading.ApplyWhen.Instantly);
                    break;
                case MouseButtons.XButton1:
                    break;
                case MouseButtons.XButton2:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sender),
                        Resources.PresenterMain_OnMouseUpNotifyIconProgram_An_error_occured_while_OnMouseUpNotifyIconProgram_);
            }
        }

        /// <summary>
        ///     main form resizing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResizingEnd(object sender, EventArgs e)
        {
            _settings.Location = _view.Location;
            _settings.SizeMain = _view.Size;
            _settings.Save();
        }

        /// <summary>
        ///     System tray icon menu item Deactivate.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnClickToolStripMenuItemDeactivate(object sender, EventArgs args)
        {
            if (_settings.Activated)
            {
                _settings.Activated = false;
                _view.SetNotifyIconImage(IsWindowsLightThemeActive()
                    ? _resources.DeactivatedForLightTheme
                    : _resources.DeactivatedForDarkTheme);

                _view.SetTextMenuDeActivate("Activate");
            }
            else if (!_settings.Activated)
            {
                _settings.Activated = true;
                _view.SetNotifyIconImage(IsWindowsLightThemeActive()
                    ? _resources.ActivatedForLightTheme
                    : _resources.ActivatedForDarkTheme);
                _view.SetTextMenuDeActivate("Deactivate");
            }
        }

        /// <summary>
        ///     System tray icon menu item Close.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnClickToolStripMenuItemEnd(object sender, EventArgs args)
        {
            _view.ShowNotifyIconProgram(false);
            Application.Exit();
        }

        /// <summary>
        ///     System tray icon menu item About.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnClickToolStripMenuItemOm(object sender, EventArgs args)
        {
            _viewAbout.Show();
        }

        /// <summary>
        ///     System tray icon menu item Show.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnClickToolStripMenuItemVisa(object sender, EventArgs args)
        {
            if (!_view.Visible && _settings.Activated)
                SetVisibilityStatus(FormFading.Visibility.Showing, FormFading.Visibility.Hiding,
                    FormFading.ChangeVisibility.Gradually, FormFading.ApplyWhen.Instantly);
        }

        /// <summary>
        ///     System tray icon menu item Settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickToolStripMenuItemSettings(object sender, EventArgs e)
        {
            var fc = Application.OpenForms["viewUserSettings"];
            if (fc != null)
                _viewUserSettings.Show();
            else
                _viewUserSettings.Show();
        }

        private void MultiPasting_Deactivated(object sender, PastingDeactivatedEventArgs e)
        {
            _notifyIcon.SetStatic();
        }

        private enum FormVisibility
        {
            Hiding,
            Showing,
            ValueInSettings
        }

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        //The CA2213 warning is suppressed since Microsoft has acknowledged that this isn't an error, but a known fault with FxCop:
        //https://stackoverflow.com/questions/36229230/ca2213-warning-when-using-null-conditional-operator-to-call-dispose/36229431
        //https://github.com/dotnet/roslyn-analyzers/issues/695
        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_notifyIcon")]
        [SuppressMessage("ReSharper", "UseNullPropagation")]
        private void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {
                if (_notifyIcon != null) _notifyIcon?.Dispose();
                if (_formHiding != null) _formHiding.Dispose();
                if (_viewPureTextInfo != null) _viewPureTextInfo.Dispose();
            }

            _disposedValue = true;
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}