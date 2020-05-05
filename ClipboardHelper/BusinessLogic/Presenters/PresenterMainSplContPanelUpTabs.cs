using ClipboardHelper.Views;
using ClipboardHelperRegEx.ModifiedControls;
using ClipboardHelperRegEx.Properties;
using ClipboardHelperRegEx.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.BusinessLogic.Presenters
{
    public sealed class PresenterMainSplContPanelUpTabs : IDisposable
    {
        public enum LineChangeType
        {
            AutoMulti,
            AutoSingle,
            ManualMulti,
            ManualSingle,
            None
        }

        private static readonly object Locker = new object();
        private static readonly object Locker2 = new object();

        private static bool _hasChangeableContentChangeFinished = true;
        private static CancellationToken _cancellationToken;
        private static CancellationTokenSource _tokenSource;
        private readonly Clipboard _clipboard;
        private readonly Pasting _pasting;
        private readonly ISettingsService _settings;
        private readonly ISettingsServiceXmlSerialization _settingsXml;
        private readonly ShortcutKeys _shortcutKeys = new ShortcutKeys();
        private readonly List<Tags.TagType> _tagTypes = new List<Tags.TagType>();

        private readonly IViewMainSplContPanelUpTabs _view;
        private readonly IViewMain _viewMain;
        private readonly IViewMainSplContPanelDown _viewMainSplContPanelDown;
        private readonly IViewMin _viewMin;
        private readonly IViewUserSettingsButtonsDown _viewUserSettingsButtonsDown;
        private int _counter;
        private int _ctr = 1;
        private History _history;
        private bool _historyLeftVisible;
        private bool _historyRightVisible;
        private int _index;
        private bool _isMultiSelectionClassReady = true;
        private bool _keyPressedChangeable;
        private List<string> _linesForPasting;
        private ModifiedListBox _listBoxAuto;
        private ListboxMultiSelection _listboxMultiSelection;
        private List<int> _selectedIndexes;
        private int _totalNrOfLinesToPaste;
        private List<Tuple<string, int>> _tupleItemIndex;
        private UIKeyNavigation _uiKeyNavigation;

        public PresenterMainSplContPanelUpTabs
        (
            IViewMainSplContPanelUpTabs view,
            IViewMain viewMain,
            IViewMin viewMin,
            ISettingsService settingsService,
            Pasting pasting,
            IViewMainSplContPanelDown viewMainSplContPanelDown,
            ISettingsServiceXmlSerialization settingsServiceXmlSerialization,
            IViewUserSettingsButtonsDown viewUserSettingsButtonsDown
        )
        {
            _view = view;
            _viewMain = viewMain;
            _viewMin = viewMin;
            _settings = settingsService;
            _pasting = pasting;
            _viewMainSplContPanelDown = viewMainSplContPanelDown;
            _settingsXml = settingsServiceXmlSerialization;
            _viewUserSettingsButtonsDown = viewUserSettingsButtonsDown;

            //Make things ready
            _tagTypes.AddRange(Tags.GetAllTagTypes());
            if (_view != null) _view.Load += View_Load;
            _clipboard = new Clipboard();
        }

        private ModifiedListBox ListboxSelected { get; set; }

        private void View_VisibleChanged(object sender, EventArgs e)
        {
            ListboxSelected.Focus();
        }

        private void View_Load(object sender, EventArgs e)
        {
            _view.AutoShownTabsRam = _settingsXml.AutoShownTabs;
            _settingsXml.PropertyChanged += SettingsServiceXmlSerialization_PropertyChanged;

            Clipboard.Changed += Clipboard_Changed;
            _history = new History
            (
                HistoryChanged,
                SetLeftHistoryIconStatus,
                SetRightHistoryIconStatus, UpdateHistoryNavigationPosition
                );

            //Make thing ready.           
            CreateTabs();
            Auto_AddListboxToTabPage();
            Manual_AddListBoxesToTabPages();
            ListboxSelected = _listBoxAuto;
            CancelAndInitiateNewMultiSelectionAndUiNavigation();


            _history.AddAndRefreshIconStatus(History.UpdateMethod.NewValue,
                "initialDummy");


            FillOneOrManyAutoOrManualListBoxesWithContent(LineChangeType.ManualMulti);
            SubscribeToEvents();
            TriggerEventsWhetherItemsAreSelectedOrNot(e);
            _settings.PropertyChanged += Settings_PropertyChanged;
            _viewUserSettingsButtonsDown.CancelIsClickedRestoreSettings +=
                ViewUserSettingsButtonsDown_OnCancelIsClickedRestoreSettings;
            _viewUserSettingsButtonsDown.SaveIsClicked += ViewUserSettingsButtonsDown_OnSaveIsClicked;
            var programsAlternativePasting = _settings.AppearanceProgramsAlternativePasting.Split(new[]
                {
                    ";",
                    ",",
                    ":"
                },
                StringSplitOptions.None);
            Pasting.ConsoleList.Clear();
            Pasting.ConsoleList.AddRange(new List<string>(programsAlternativePasting));
            const Keys appShortcutKeys = Keys.Escape;
            _shortcutKeys.AddShortcutKey(ToString(), appShortcutKeys, ClearAnyPossibleSelection);
            _listBoxAuto.Select();
            _view.VisibleChanged += View_VisibleChanged;
        }

        private void UiKeyNavigation_OnAnyDigitKeyUp(object sender, EventArgs e)
        {
            var index = ListboxSelected.SelectedIndex;
            //if (index == -1) return;
            ListboxSelected.SelectedIndices.Remove(index);
            _listboxMultiSelection.HandleKeyUp(index);
        }

        private void UiKeyNavigation_OnNavigationUp(object sender, EventArgs e)
        {
            var index = ListboxSelected.SelectedIndex - 1;
            //if (index == -2) return;
            ListboxSelected.SelectedIndices.Remove(index + 1);
            if (ListboxSelected.SelectedItems.Count > 1) ListboxSelected.ClearSelected();
            _listboxMultiSelection.HandleKeyUp(index);
        }

        private void UiKeyNavigation_OnNavigationDown(object sender, EventArgs e)
        {
            var index = ListboxSelected.SelectedIndex + 1;
            //if (index == 0) return;
            ListboxSelected.SelectedIndices.Remove(index - 1);
            if (ListboxSelected.SelectedItems.Count > 1) ListboxSelected.ClearSelected();
            _listboxMultiSelection.HandleKeyUp(index);
        }

        private void Clipboard_Changed(object sender, EventArgs args)
        {
            //Thread Safe locker.
            var lockWasTaken = false;
            try
            {
                Monitor.Enter(Locker, ref lockWasTaken);
                if (IsThisTextFromOurListboxSelection()) return;
                if (IsThisTextANewMatch())
                {
                    Tags.Cancel(); //Aborts filling lines (DelayedTags from web...)
                    if (_settings.AppearanceFocus) _viewMain.Activate();
                    if (!_settings.Activated) return;
                    _history.AddAndRefreshIconStatus(History.UpdateMethod.NewValue,
                        ViewMainSplContPanelUpTabs.ClipboardStored);
                    _viewMainSplContPanelDown.PositionText = (_history.NavigationPosition - 1).ToString(System.Globalization.CultureInfo.InvariantCulture);
                    _pasting.Cancel();
                    _view.TriggerEventOnNewClipboardText(EventArgs.Empty); //Triggers event to show the app if hidden.
                    if (_view.TabControl.SelectedIndex != 0)
                        _view.TabControl.SelectedIndex = 0;
                    FillOneOrManyAutoOrManualListBoxesWithContent(LineChangeType.AutoMulti);
                    if (!_viewMain.Visible)
                    {
                        if (!_settings.AppearanceFocus)
                            _viewMain.ShowInactiveTopmost(_viewMain as Form);
                        else
                            _viewMain.Show();
                    }

                    if (!ListboxSelected.Focused)
                        ListboxSelected.Focus();
                }
                else
                {
                    ClearAnyPossibleSelection(); //deletes clipboard, therefore we save the clipboard value.
                }
            }
            finally
            {
                if (lockWasTaken) Monitor.Exit(Locker);
            }
        }

        private bool IsThisTextANewMatch()
        {
            if (ViewMainSplContPanelUpTabs.ClipboardStored == Clipboard.Text || !IsRegexMatch(Clipboard.Text) ||
                Pasting.SendingKeys) return false;
            ViewMainSplContPanelUpTabs.ClipboardStored = Clipboard.Text;
            return true;
        }

        private bool IsThisTextFromOurListboxSelection()
        {
            if (string.IsNullOrEmpty(_pasting.LastPastedItem) || string.IsNullOrEmpty(Clipboard.Text))
                return !string.IsNullOrEmpty(_pasting.LastPastedItem) || string.IsNullOrEmpty(Clipboard.Text);
            var haveWeSetTextInClipboard = !string.IsNullOrEmpty(_pasting.LastPastedItem);
            if (!haveWeSetTextInClipboard) return true;
            return Clipboard.Text == _pasting.LastPastedItem.Trim();
        }

        private bool IsRegexMatch(string text)
        {
            return text != null && _view.AutoShownTabsRam.List.Any(t => Regex.IsMatch(text, t.RegEx));
        }

        private void ClearAnyPossibleSelection()
        {
            if (_pasting != null || _listboxMultiSelection != null)
                View_OnMetroTabControl_SelectedIndexChanged(this, EventArgs.Empty); //clears things     
        }

        private void ViewUserSettingsButtonsDown_OnSaveIsClicked(object sender, EventArgs e)
        {
            // SettingsServiceXmlSerialization_PropertyChanged is used instead
        }

        private void ViewUserSettingsButtonsDown_OnCancelIsClickedRestoreSettings(object sender, EventArgs e)
        {
            _pasting.Cancel();
            var programsAlternativePasting = _settings.AppearanceProgramsAlternativePasting.Split(new[]
                {
                    ";",
                    ",",
                    ":"
                },
                StringSplitOptions.None);
            Pasting.ConsoleList.Clear();
            Pasting.ConsoleList.AddRange(new List<string>(programsAlternativePasting));
        }

        private static void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "AppearanceColorTitle":
                case "AppearanceColorInfo":
                case "AppearanceColorWebUrlGoTo":
                case "AppearanceColorOutlookSearch":
                    break;
            }
        }

        private void SettingsServiceXmlSerialization_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CreateTabs();
            Auto_AddListboxToTabPage();
            Manual_AddListBoxesToTabPages();
            CancelAndInitiateNewMultiSelectionAndUiNavigation();
            switch (e.PropertyName)
            {
                case "AutoShownTabs":
                    FillOneOrManyAutoOrManualListBoxesWithContent(LineChangeType.AutoMulti);
                    if (_view != null) _view.AutoShownTabsRam = _settingsXml.AutoShownTabs;
                    break;
                case "ManuallyShownTabs":
                    FillOneOrManyAutoOrManualListBoxesWithContent(LineChangeType.ManualMulti);
                    if (_view != null) _view.ManuallyShownTabsRam = _settingsXml.ManuallyShownTabs;
                    break;
            }
            SubscribeToEvents();
            View_OnMetroTabControl_SelectedIndexChanged(this, EventArgs.Empty); //clears things  
        }

        /// <summary>
        ///     When selected tab changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void View_OnMetroTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListboxSelected.SelectedItems.Count > 0)
                Pasting_MultiPastingDeactivated(sender, PastingDeactivatedEventArgs.Empty);
            _pasting?.Cancel();
            ListboxSelected = null;
            //if (_view.TabControl.SelectedIndex != -1)
                if (_view.TabControl.SelectedIndex == 0)
                {
                    if (_listBoxAuto != null)
                    {
                        _listBoxAuto.ClearSelected();
                        ListboxSelected = _listBoxAuto;
                        _uiKeyNavigation.Cancel();
                        _uiKeyNavigation = null;
                        _uiKeyNavigation = new UIKeyNavigation(ListboxSelected, _view.TabControl);
                        ListboxSelected.Focus();
                    }

                    _viewMainSplContPanelDown.HistoryLeft.Visible = _historyLeftVisible;
                    _viewMainSplContPanelDown.HistoryRight.Visible = _historyRightVisible;
                    _viewMainSplContPanelDown.Position.Visible = true;
                }
                else
                {
                    if (_view.ListBoxesManual.Count != 0)
                    {
                        ListboxSelected = _view.ListBoxesManual[_view.TabControl.SelectedIndex - 1];
                        _uiKeyNavigation.Cancel();
                        _uiKeyNavigation = null;
                        _uiKeyNavigation = new UIKeyNavigation(ListboxSelected, _view.TabControl);
                        ListboxSelected.Focus();
                        if (_view.ListBoxesManual[_view.TabControl.SelectedIndex - 1].SelectedItems.Count > 0)
                            _view.ListBoxesManual[_view.TabControl.SelectedIndex - 1]?.ClearSelected();
                    }

                    _viewMainSplContPanelDown.HistoryLeft.Visible = false;
                    _viewMainSplContPanelDown.HistoryRight.Visible = false;
                    _viewMainSplContPanelDown.Position.Visible = false;
                }

            CancelAndInitiateNewMultiSelectionAndUiNavigation();
            SubscribeToEvents();
            _view.TriggerEventOnNoItemsSelected(e);
        }

        private void CancelAndInitiateNewMultiSelectionAndUiNavigation()
        {
            _listboxMultiSelection
                ?.Cancel(); //Important, otherwise we leave behind events which will be run in the background.
            _listboxMultiSelection = null;
            _listboxMultiSelection = new ListboxMultiSelection(ListboxSelected);
            _uiKeyNavigation?.Cancel();
            _uiKeyNavigation = null;
            _uiKeyNavigation = new UIKeyNavigation(ListboxSelected, _view.TabControl);
        }

        private void SubscribeToEvents()
        {
            UnsubscribeToEvents(); //safety
            _viewMainSplContPanelDown.HistoryLeftMouseClick += ViewMainSplContPanelDown_HistoryLeftMouseClick;
            _viewMainSplContPanelDown.HistoryRightMouseClick += ViewMainSplContPanelDown_HistoryRightMouseClick;
            _viewMain.FormLostFocusToOutsideApp += ViewMain_FormLostFocusToOutsideApp;
            _viewMin.OnViewMinFormLostFocusToOutsideApp += ViewMin_FormLostFocusToOutsideApp;
            _pasting.OnPastingDeactivated += Pasting_MultiPastingDeactivated;
            _pasting.OnPastingOccured += Pasting_PastingOccured;
            _listboxMultiSelection.OnSelectionReady += ListboxMultiSelection_OnSelectionReady;
            _view.TabControl.MouseWheel += MetroTabControl1_MouseWheel;
            _view.OnMetroTabControlSelectedIndexChanged += View_OnMetroTabControl_SelectedIndexChanged;
            _listboxMultiSelection.OnSelectionNotReady += ListboxMultiSelection_OnSelectionNotReady;
            _uiKeyNavigation.OnAnyDigitKeyUp += UiKeyNavigation_OnAnyDigitKeyUp;
            _uiKeyNavigation.OnNavigationDown += UiKeyNavigation_OnNavigationDown;
            _uiKeyNavigation.OnNavigationUp += UiKeyNavigation_OnNavigationUp;
            ListboxSelected.SelectedIndexChanged += ListboxSelected_SelectedIndexChanged;
            _view.OnTextBoxChangeableContentTextChanged += View_OnTextBoxChangeableContent_TextChanged;
            _view.OnTextBoxChangeableContentKeyPress += View_OnTextBoxChangeableContent_KeyPress;
        }

        private void ListboxSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            TriggerEventsWhetherItemsAreSelectedOrNot(e);
        }

        private void UnsubscribeToEvents()
        {
            if (_viewMain != null) _viewMain.FormLostFocusToOutsideApp -= ViewMain_FormLostFocusToOutsideApp;
            if (_viewMin != null) _viewMin.OnViewMinFormLostFocusToOutsideApp -= ViewMin_FormLostFocusToOutsideApp;
            if (_viewMainSplContPanelDown != null)
                _viewMainSplContPanelDown.HistoryLeftMouseClick -= ViewMainSplContPanelDown_HistoryLeftMouseClick;
            if (_viewMainSplContPanelDown != null)
                _viewMainSplContPanelDown.HistoryRightMouseClick -= ViewMainSplContPanelDown_HistoryRightMouseClick;
            _pasting.OnPastingDeactivated -= Pasting_MultiPastingDeactivated;
            _pasting.OnPastingOccured -= Pasting_PastingOccured;
            _listboxMultiSelection.OnSelectionReady -= ListboxMultiSelection_OnSelectionReady;
            _view.TabControl.MouseWheel -= MetroTabControl1_MouseWheel;
            _view.OnMetroTabControlSelectedIndexChanged -= View_OnMetroTabControl_SelectedIndexChanged;
            _listboxMultiSelection.OnSelectionNotReady -= ListboxMultiSelection_OnSelectionNotReady;
            _uiKeyNavigation.OnAnyDigitKeyUp -= UiKeyNavigation_OnAnyDigitKeyUp;
            _uiKeyNavigation.OnNavigationDown -= UiKeyNavigation_OnNavigationDown;
            _uiKeyNavigation.OnNavigationUp -= UiKeyNavigation_OnNavigationUp;
            ListboxSelected.SelectedIndexChanged -= ListboxSelected_SelectedIndexChanged;
            _view.OnTextBoxChangeableContentTextChanged -= View_OnTextBoxChangeableContent_TextChanged;
            _view.OnTextBoxChangeableContentKeyPress -= View_OnTextBoxChangeableContent_KeyPress;
        }

        private void CreateTabs()
        {
            _view.OnMetroTabControlSelectedIndexChanged -= View_OnMetroTabControl_SelectedIndexChanged;
            _view.TabControl.Controls.Clear();
            _view.OnMetroTabControlSelectedIndexChanged += View_OnMetroTabControl_SelectedIndexChanged;
            _view.ManuallyShownTabsRam.List.Clear();
            _view.ManuallyShownTabsRam.List.AddRange(_settingsXml.ManuallyShownTabs.List);
            if (_view.ManuallyShownTabsRam.List.Count == 0) return;
            for (var i = 0; i < _view.ManuallyShownTabsRam.List.Count + 1; i++)
                if (i == 0)
                {
                    var tabPage = new TabPage
                    {
                        Text = Resources.PresenterMainSplContPanelUpTabs_CreateTabs_Auto
                    };
                    _view.TabControl.Controls.Add(tabPage);
                }
                else
                {
                    var tabPage = new TabPage
                    {
                        Text = _view.ManuallyShownTabsRam.List[i - 1].Name
                    };
                    _view.TabControl.Controls.Add(tabPage);
                }
        }

        private void Auto_AddListboxToTabPage()
        {
            _listBoxAuto = null;
            _listBoxAuto = new ModifiedListBox
            {
                Name = "ListAuto",
                SelectionMode = SelectionMode.MultiExtended,
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                BorderStyle = BorderStyle.None,
                DrawMode = DrawMode.OwnerDrawFixed,
                RightToLeft = RightToLeft.No,
                FormattingEnabled = true,
                Location = new Point(3, 3),
                SelectionColor = _settings.AppearanceColorSelection,
                SelectionTextColor = Color.White,
                Size = new Size(599, 500),
                TabIndex = 0,
                TextColor = Color.Black
            };
            _view.GroupBoxAuto.Controls.Add(_listBoxAuto);
            _listBoxAuto.BringToFront();
            _view.TabControl.TabPages[0].Controls.Add(_view.GroupBoxAuto);
        }

        private void Manual_AddListBoxesToTabPages()
        {
            //remove all pages except "auto" tab.
            foreach (TabPage page in _view.TabControl.TabPages)
            {
                if (page.Text == Resources.PresenterMainSplContPanelUpTabs_CreateTabs_Auto) continue;
                if (_view.TabControl.TabPages[_ctr] != null) _view.TabControl.TabPages[_ctr].Controls.Clear();
                _ctr += 1;
            }

            _ctr = 1;
            _view.ListBoxesManual.Clear();
            if (_view.ManuallyShownTabsRam.List.Count != 0)
                for (var i = 0; i < _view.ManuallyShownTabsRam.List.Count; i++)
                {
                    _view.ListBoxesManual.Add(new ModifiedListBox
                    {
                        Name = _view.ManuallyShownTabsRam.List[i].Name,
                        SelectionMode = SelectionMode.MultiExtended,
                        SelectionColor = _settings.AppearanceColorSelection,
                        BackColor = Color.White,
                        Location = new Point(0, 0),
                        Dock = DockStyle.Fill,
                        Margin = new Padding(0),
                        BorderStyle = BorderStyle.None,
                        DrawMode = DrawMode.OwnerDrawFixed,
                        RightToLeft = RightToLeft.No,
                        FormattingEnabled = true
                    }
                    );
                    _view.ListBoxesManual[i].Name = i.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    if (_view.TabControl.TabPages[i + 1] != null)
                        _view.TabControl.TabPages[i + 1].Controls.Add(_view.ListBoxesManual[i]);
                }
        }



        private void FillOneOrManyAutoOrManualListBoxesWithContent(LineChangeType lineChangeType)
        {
            switch (lineChangeType)
            {
                case LineChangeType.None: //initial auto, position 0
                    MakeSpecificListboxReady(0, LineChangeType.None);
                    break;
                case LineChangeType.AutoMulti:
                    _view.AutoShownTabsRam = _settingsXml.AutoShownTabs; //load from Hard drive to RAM
                    if (ViewMainSplContPanelUpTabs.ClipboardStored != null)
                        foreach (var t in _view.AutoShownTabsRam.List)
                        {
                            if (!Regex.IsMatch(ViewMainSplContPanelUpTabs.ClipboardStored,
                                t.RegEx)) continue;
                            MakeSpecificListboxReady(t.Items.Count, LineChangeType.AutoMulti);
                            _view.GroupBoxAuto.Text = t.Name;
                            try
                            {
                                TransformLinesForOneListbox(_view.NavigationPosition, _view.NavigationPositionAndId.Values[_view.NavigationPosition], LineChangeType.AutoMulti, t.Items);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("line 516: " + e.Message + @"\r\n\r\" + "_view.NavigationPosition=" +
                                                _view.NavigationPosition +
                                                "_view.NavigationPositionAndId.Count=" +
                                                _view.NavigationPositionAndId.Count);
                                FillOneOrManyAutoOrManualListBoxesWithContent(LineChangeType.AutoMulti);
                            }
                        }
                    else _viewMain.LabelTitleTop.Text = string.Empty;

                    break;
                case LineChangeType.ManualMulti:
                    if (_view.ManuallyShownTabsRam != null)
                        if (_view.ManuallyShownTabsRam.List.Count != 0)
                            for (var i = 0; i < _view.ManuallyShownTabsRam.List.Count; i++)
                            {
                                MakeSpecificListboxReady(_view.ManuallyShownTabsRam.List[i]
                                        .Lines.Count,
                                    LineChangeType.ManualMulti,
                                    i);
                                try
                                {
                                    TransformLinesForOneListbox(_view.NavigationPosition, _view.NavigationPositionAndId.Values[_view.NavigationPosition], LineChangeType.ManualMulti,
                                            _view.ManuallyShownTabsRam.List[i].Lines, i);
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("line 541: " + e.Message);
                                }
                            }
                    break;
                case LineChangeType.AutoSingle:
                    break;
                case LineChangeType.ManualSingle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lineChangeType), lineChangeType, null);
            }
        }

        /// <summary>
        ///     Lists must be emptied before new values can be inserted at any index by an async method.
        /// </summary>
        /// <param name="nrItems"></param>
        /// <param name="lineChangeType"></param>
        /// <param name="manualTabIndex"></param>
        private void MakeSpecificListboxReady(int nrItems, LineChangeType lineChangeType, int manualTabIndex = 0)
        {
            switch (lineChangeType)
            {
                case LineChangeType.None: //initial auto, position 0
                    _listBoxAuto.UiThread
                    (delegate
                        {
                            _listBoxAuto?.Items.Clear();
                        }
                    );
                    break;
                case LineChangeType.ManualMulti:
                    _view.TabControl.UiThread
                    (delegate
                        {
                            if (_view.ListBoxesManual.Count != 0) _view.ListBoxesManual[manualTabIndex].Items.Clear();
                            for (var i = 0; i < nrItems; i++)
                                _view.ListBoxesManual[manualTabIndex].Items.Add(string.Empty);
                        }
                    );
                    break;
                case LineChangeType.AutoMulti:
                    _listBoxAuto.UiThread
                    (delegate
                        {
                            _listBoxAuto?.Items.Clear();
                            for (var i = 0; i < nrItems; i++) _listBoxAuto?.Items.Add(string.Empty);
                        }
                    );
                    break;
                case LineChangeType.AutoSingle:
                    break;
                case LineChangeType.ManualSingle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lineChangeType), lineChangeType, null);
            }
        }

        private void TransformLinesForOneListbox(
            int navigationPosition,
            int id,
            LineChangeType lineChangeType,
            List<string> items,
            int tabPageIndexManual = 0)
        {
            var tags = new Tags();
            _pasting?.Cancel();
            items = Lines.Edit(Lines.EditingMethods.InsertNumbering, items);
            switch (lineChangeType)
            {
                case LineChangeType.AutoMulti:
                    tags.TransformLines(
                        navigationPosition,
                        id,
                        lineChangeType,
                        0, //note: relevant only for manual pasting!
                        items,
                        _tagTypes,
                        TransformLinesForOneSpecificListbox,
                        ViewMainSplContPanelUpTabs.ClipboardStored ?? "");
                    break;
                case LineChangeType.ManualMulti:
                    tags.TransformLines(
                        navigationPosition,
                        id,
                        lineChangeType,
                        tabPageIndexManual,
                        items,
                        _tagTypes,
                        TransformLinesForOneSpecificListbox,
                        ViewMainSplContPanelUpTabs.ClipboardStored ?? "");
                    break;
                case LineChangeType.AutoSingle:
                    break;
                case LineChangeType.ManualSingle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lineChangeType), lineChangeType, null);
            }

            _viewMain.LabelTitleTop.Text = Resources
                .PresenterMainSplContPanelUpTabs_TransformLinesForOneListbox_Please_select_an_item_or_items_in_the_list_;
        }



        /// <summary>
        ///     This is called async by external class.
        /// </summary>
        /// <param name="navigationPosition"></param>
        /// <param name="id"></param>
        /// <param name="lineChangeType"></param>
        /// <param name="tabIndexManual"></param>
        /// <param name="lines"></param>
        /// <param name="changedContentEdited"></param>
        /// <param name="changeableContent"></param>
        /// <param name="lineIndex"></param>
        [SuppressMessage("ReSharper", "CoVariantArrayConversion")]
        [SuppressMessage("ReSharper", "ImplicitlyCapturedClosure")]
        private void TransformLinesForOneSpecificListbox(
            int navigationPosition,
            int id,
            LineChangeType lineChangeType,
            int tabIndexManual,
            IList<string> lines,
            bool changedContentEdited = false,
            string changeableContent = "",
            int lineIndex = -1
        )
        {
            //Thread Safe locker.
            var lockWasTaken = false;
            try
            {
                Monitor.Enter(Locker2, ref lockWasTaken);

                Debug.WriteLine("_view.NavigationPosition=" + _view.NavigationPosition);
                Debug.WriteLine("navigationPosition=" + navigationPosition);
                Debug.WriteLine("_view.NavigationPositionAndId.Values[_view.NavigationPosition]=" + _view.NavigationPositionAndId.Values[_view.NavigationPosition]);
                Debug.WriteLine("id=" + id);

                if (_view.NavigationPosition != navigationPosition ||
                    _view.NavigationPositionAndId.Values[_view.NavigationPosition] != id)
                {
                    Debug.WriteLine("returning overstepping");
                    return;
                }

                switch (lineChangeType)
                {
                    case LineChangeType.AutoMulti:
                        _hasChangeableContentChangeFinished = true;
                        if (!Tags.TokenSource.Token.IsCancellationRequested)
                            ListboxSelected.UiThread
                            (delegate
                            {
                                _listBoxAuto.Items.Clear();
                                _listBoxAuto.Items.AddRange(lines.ToArray());
                                switch (changedContentEdited)
                                {
                                    case true:
                                        break;
                                    case false:
                                        if (changeableContent.Length == 0)
                                        {
                                            _view.PanelChangeableContent.Visible = false;
                                        }
                                        else
                                        {
                                            _view.TextBoxChangeableContent.Text = changeableContent;
                                            _view.PanelChangeableContent.Visible = true;
                                        }

                                        break;
                                }
                            }
                            );
                        break;
                    case LineChangeType.AutoSingle:
                        if (!Tags.TokenSource.Token.IsCancellationRequested)
                            ListboxSelected.UiThread
                            (delegate
                            {
                                try
                                {
                                    if (_listBoxAuto.Items.Count <= lineIndex) return;
                                    if (_listBoxAuto.Items[lineIndex] == null) return;
                                    _listBoxAuto.Items.RemoveAt(lineIndex);
                                    if (lines.Count > 1)
                                        _listBoxAuto.Items.Insert(lineIndex, lines.ElementAt(lineIndex));
                                    else if (lines.Count == 1)
                                        _listBoxAuto.Items.Insert(lineIndex, lines.ElementAt(0));
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("line 740: " + e.Message);
                                    throw;
                                }
                            }
                            );
                        break;
                    case LineChangeType.ManualMulti:
                        if (!Tags.TokenSource.Token.IsCancellationRequested)
                            ListboxSelected.UiThread
                            (delegate
                            {
                                _view.ListBoxesManual[tabIndexManual].Items.Clear();
                                _view.ListBoxesManual[tabIndexManual].Items.AddRange(lines.ToArray());
                            }
                            );
                        break;
                    case LineChangeType.ManualSingle:
                        if (!Tags.TokenSource.Token.IsCancellationRequested)
                            ListboxSelected.UiThread
                            (delegate
                            {
                                try
                                {
                                    if (_view.ListBoxesManual[tabIndexManual].Items.Count <= lineIndex) return;
                                    if (_view.ListBoxesManual[tabIndexManual] == null) return;
                                    _view.ListBoxesManual[tabIndexManual].Items.RemoveAt(lineIndex);
                                    if (lines.Count > 1)
                                        _view.ListBoxesManual[tabIndexManual].Items
                                            .Insert(lineIndex, lines.ElementAt(lineIndex));
                                    else if (lines.Count == 1)
                                        _view.ListBoxesManual[tabIndexManual].Items
                                            .Insert(lineIndex, lines.ElementAt(0));
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("line 775: " + e.Message);
                                    throw;
                                }
                            }
                            );
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(lineChangeType), lineChangeType, null);
                }
            }
            finally
            {
                if (lockWasTaken) Monitor.Exit(Locker2);
            }


        }

        private async void View_OnTextBoxChangeableContent_TextChanged(object sender, EventArgs e)
        {
            if (!_keyPressedChangeable || !_hasChangeableContentChangeFinished) return;
            _hasChangeableContentChangeFinished = false;
            _keyPressedChangeable = false;
            _tokenSource = new CancellationTokenSource();
            _cancellationToken = _tokenSource.Token;
            var tags = new Tags();
            //Async Task so that HasChangeableContentChangeFinished can change to true.
            await Task.Run(() => tags.TransformLines(
                _view.NavigationPosition,
                _view.NavigationPositionAndId.Values[_view.NavigationPosition],
                LineChangeType.AutoMulti,
                0, //note: relevant only for manual pasting!
                ListboxSelected.Items.Cast<string>().ToList(),
                _tagTypes,
                TransformLinesForOneSpecificListbox,
                ViewMainSplContPanelUpTabs.ClipboardStored ?? "",
                true,
                _view.TextBoxChangeableContent.Text
            ), _cancellationToken).ConfigureAwait(true);
        }

        private void View_OnTextBoxChangeableContent_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_hasChangeableContentChangeFinished)
                _keyPressedChangeable = true;
            else
                e.Handled = true;
        }

        //is updated from the History class
        private void UpdateHistoryNavigationPosition(int position, int id)
        {
            if (position >= _view.NavigationPosition)
                switch (_whichHistoryIconClicked)
                {
                    case WhichHistoryIconClicked.Right:
                        if (!_view.NavigationPositionAndId.ContainsKey(position))
                            _view.NavigationPositionAndId.Add(position, id);
                        break;
                    case WhichHistoryIconClicked.Left:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(position));
                }
            _view.NavigationPosition = position;
        }

        private enum WhichHistoryIconClicked
        {
            Right,
            Left
        }

        private WhichHistoryIconClicked _whichHistoryIconClicked;

        private void ViewMainSplContPanelDown_HistoryRightMouseClick(object sender, EventArgs e)
        {
            _whichHistoryIconClicked = WhichHistoryIconClicked.Right;
            _history.AddAndRefreshIconStatus(History.UpdateMethod.RightClick);
        }

        private void ViewMainSplContPanelDown_HistoryLeftMouseClick(object sender, EventArgs e)
        {
            _whichHistoryIconClicked = WhichHistoryIconClicked.Left;
            _history.AddAndRefreshIconStatus(History.UpdateMethod.LeftClick);
        }

        private void SetLeftHistoryIconStatus(bool status)
        {
            _viewMainSplContPanelDown.HistoryLeft.Enabled = status;
            _viewMainSplContPanelDown.HistoryLeft.Visible = status;
            _historyLeftVisible = status;
        }

        private void SetRightHistoryIconStatus(bool status)
        {
            _viewMainSplContPanelDown.HistoryRight.Enabled = status;
            _viewMainSplContPanelDown.HistoryRight.Visible = status;
            _historyRightVisible = status;
        }


        /// <summary>
        /// Called by action from the History class - HandleLeftClick() - HandleRightClick
        /// </summary>
        private void HistoryChanged()
        {
            ViewMainSplContPanelUpTabs.ClipboardStored = _history.Values[_history.NavigationPosition - 1];
            _pasting.Cancel();
            _viewMainSplContPanelDown.PositionText = (_history.NavigationPosition - 1).ToString(System.Globalization.CultureInfo.InvariantCulture);
            switch (_whichHistoryIconClicked)
            {
                case WhichHistoryIconClicked.Right:
                    FillOneOrManyAutoOrManualListBoxesWithContent(LineChangeType.AutoMulti);
                    break;
                case WhichHistoryIconClicked.Left:
                    FillOneOrManyAutoOrManualListBoxesWithContent((_history.NavigationPosition - 1) == 0
                        ? LineChangeType.None
                        : LineChangeType.AutoMulti);
                    break;
                default:
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                    throw new ArgumentOutOfRangeException();
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }

        }

        private void ViewMain_FormLostFocusToOutsideApp(object sender, EventArgs e)
        {
            SendPastingClassWithLinesThatCanBePasted();
        }

        private void ViewMin_FormLostFocusToOutsideApp(object sender, EventArgs e)
        {
            SendPastingClassWithLinesThatCanBePasted();
        }

        /// <summary>
        ///     Enables multi pasting by providing the Pasting class with transformed lines.
        /// </summary>
        private void SendPastingClassWithLinesThatCanBePasted()
        {
            if (!SelectionErrorCheckPassed()) return;
            if (_linesForPasting == null || _linesForPasting.Count == 0) return;
            if (ListboxSelected.SelectedItems.Count == 1 && ContainNotSelectableLine(_linesForPasting[0]))
            {
                //do nothing
            }
            else if (ListboxSelected.SelectedItems.Count > 0)
            {
                _totalNrOfLinesToPaste = _linesForPasting.Count;
                _counter = 0;
                _pasting.TextList = _linesForPasting; //enables multi pasting  
                _viewMain.LabelTitleTop.Text = Resources
                    .PresenterMainSplContPanelUpTabs_SendPastingClassWithPastableLines_Paste_with_LEFT_CTRL_V__Abort_pasting_with_ESC_;
                _viewMin.LabelTitleTop.Text = Resources
                    .PresenterMainSplContPanelUpTabs_SendPastingClassWithPastableLines_Paste_with_LEFT_CTRL_V__Abort_pasting_with_ESC_;
            }
        }

        private void Pasting_PastingOccured(object sender, PastingOccuredEventArgs e)
        {
            if (_tupleItemIndex == null) return;
            Tuple<string, int> result = null;
            _counter += 1;
            if (_pasting.LastPastedItem != null)
            {
                var lpi = _pasting.LastPastedItem.Replace("\r",
                    string.Empty); //remove \r because we added it earlier in the pasting class.
                result = _tupleItemIndex.FirstOrDefault(v => v.Item1 == lpi);
            }

            if (result != null)
                _listboxMultiSelection.ThisIndexHasBeenChangedProgrammatically = result.Item2; //important to set
            if (result != null) ListboxSelected.SelectedIndices.Remove(result.Item2);
            if (result != null) _tupleItemIndex?.Remove(result);
            if (!_tupleItemIndex.Any())
            {
                ListboxMultiSelection.SelectedIndexesOrdered.Clear();
                ListboxMultiSelection.SelectedItemsOrdered.Clear();
                _listboxMultiSelection.SelectionOld.Clear();
            }

            if (_counter > _totalNrOfLinesToPaste) //safety, in case program har a logic fault
                _counter = _totalNrOfLinesToPaste;
            _viewMain.LabelTitleTop.Text = _counter +
                                           Resources
                                               .PresenterMainSplContPanelUpTabs_Pasting_PastingOccured__out_of_ +
                                           _totalNrOfLinesToPaste +
                                           Resources
                                               .PresenterMainSplContPanelUpTabs_Pasting_PastingOccured__pasted_;
            _viewMin.LabelTitleTop.Text = _counter +
                                          Resources
                                              .PresenterMainSplContPanelUpTabs_Pasting_PastingOccured__out_of_ +
                                          _totalNrOfLinesToPaste +
                                          Resources.PresenterMainSplContPanelUpTabs_Pasting_PastingOccured__pasted_;
            if (SelectionErrorCheckPassed())
            {
            }
        }

        private void TriggerEventsWhetherItemsAreSelectedOrNot(EventArgs e)
        {
            if (ListboxSelected == null) return;
            if (ListboxSelected.SelectedItems.Count == 1)
            {
                if (ValidUrlSelected(ListboxSelected.SelectedItem.ToString())) return;
                if (ContainNotSelectableLine(ListboxSelected.SelectedItem.ToString())) return;
                _viewMain.LabelTitleTop.Text = Resources
                    .PresenterMainSplContPanelUpTabs_TriggerEventsWhetherItemsAreSelectedOrNot_Select_another_app_to_paste_line__Clear_selection_with_ESC_;
                _viewMin.LabelTitleTop.Text = Resources
                    .PresenterMainSplContPanelUpTabs_TriggerEventsWhetherItemsAreSelectedOrNot_Select_another_app_to_paste_line__Clear_selection_with_ESC_;
                _view.TriggerEventOnItemsSelected(e);
            }
            else if (ListboxSelected.SelectedItems.Count > 0)
            {
                _viewMain.LabelTitleTop.Text = Resources
                    .PresenterMainSplContPanelUpTabs_TriggerEventsWhetherItemsAreSelectedOrNot_Select_another_app_to_paste_lines__Clear_selection_with_ESC_;
                _viewMin.LabelTitleTop.Text = Resources
                    .PresenterMainSplContPanelUpTabs_TriggerEventsWhetherItemsAreSelectedOrNot_Select_another_app_to_paste_lines__Clear_selection_with_ESC_;
                _view.TriggerEventOnItemsSelected(e);
            }
            else if (ListboxSelected.SelectedItems.Count <= 0)
            {
                _viewMain.LabelTitleTop.Text = Resources
                    .PresenterMainSplContPanelUpTabs_TransformLinesForOneListbox_Please_select_an_item_or_items_in_the_list_;
                _view.TriggerEventOnNoItemsSelected(e);
            }
        }

        private void MetroTabControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            _view.TabControl.SelectedIndex = _view.TabControl.SelectedIndex + 1;
        }

        private void ListboxMultiSelection_OnSelectionNotReady(object sender, EventArgs e)
        {
            _isMultiSelectionClassReady = false;
        }

        /// <summary>
        ///     when selection is ready and the listbox is still in focus. Used when clicking listbox items and that opens a Web
        ///     Browser tab or
        ///     search Microsoft Outlook, instead of selecting an listbox item.
        /// </summary>
        private void ListboxMultiSelection_OnSelectionReady(object sender, EventArgs e)
        {
            _isMultiSelectionClassReady = true;
            if (!SelectionErrorCheckPassed()) return;
            if (ListboxSelected?.SelectedItem == null) return;
            TriggerEventsWhetherItemsAreSelectedOrNot(e);
            _tupleItemIndex = new List<Tuple<string, int>>();
            _index = 0;
            var tags = new Tags();
            CreateLinesThatCanBePasted(); //fills linesForPasting with new lines
            foreach (var item in _linesForPasting)
            {
                _tupleItemIndex.Add(Tuple.Create(item, ListboxMultiSelection.SelectedIndexesOrdered[_index]));
                _index += 1;
            }

            if (Tags.StringContainsTags(ListboxSelected?.SelectedItem?.ToString(),
                new List<Tags.TagType> { Tags.TagType.WebUrlGoTo, Tags.TagType.OutlookSearch, Tags.TagType.ConsoleOpen }))
                if (ListboxSelected != null)
                    tags.TransformLine(new List<string> { ListboxSelected.SelectedItem?.ToString() }[0],
                        Tags.UsedIn.SingleSelection,
                        new List<Tags.TagType>
                            {Tags.TagType.WebUrlGoTo, Tags.TagType.OutlookSearch, Tags.TagType.ConsoleOpen},
                        ViewMainSplContPanelUpTabs.ClipboardStored);
            ListboxSelected?.Focus(); //So event is Listbox_KeyPress(object sender, KeyPressEventArgs e) is triggered
        }

        private void CreateLinesThatCanBePasted()
        {
            if (!SelectionErrorCheckPassed()) return;
            if (ListboxSelected.SelectedItems.Count <= 0) return;
            var linesStripped = RemoveLinesMarkedAsNotSelectable(ListboxMultiSelection.SelectedItemsOrdered,
                ListboxMultiSelection.SelectedIndexesOrdered);
            var tags = new Tags();
            _linesForPasting = new List<string>();
            foreach (var item in linesStripped)
                _linesForPasting.Add(tags.TransformLine(item, Tags.UsedIn.Pasting, _tagTypes,
                    ViewMainSplContPanelUpTabs.ClipboardStored ?? ""));
            _linesForPasting = Lines.Edit(Lines.EditingMethods.RemoveNumbering, _linesForPasting);
            _linesForPasting = Lines.Edit(Lines.EditingMethods.Clean, _linesForPasting);
        }

        private IEnumerable<string> RemoveLinesMarkedAsNotSelectable(List<string> selectedLines,
            List<int> selectedIndexes)
        {
            _selectedIndexes = selectedIndexes;
            var gg = selectedLines;
            if (selectedIndexes.Count < gg.Count) return gg;
            for (var i = 0; i < gg.Count; i++)
            {
                if (!ContainNotSelectableLine(gg[i])) continue;
                gg.RemoveAt(i);
                _selectedIndexes.RemoveAt(i);
                i -= 1;
            }

            return gg;
        }

        private void Pasting_MultiPastingDeactivated(object sender, PastingDeactivatedEventArgs e)
        {
            if (!SelectionErrorCheckPassed()) return;
            _viewMain.LabelTitleTop.Text = Resources
                .PresenterMainSplContPanelUpTabs_Pasting_MultipastingDeactivated_Pasting_finished__Please_select_an_item_or_items_in_the_list_;
            _viewMin.LabelTitleTop.Text = Resources
                .PresenterMainSplContPanelUpTabs_Pasting_MultipastingDeactivated_Pasting_finished__Please_select_an_item_or_items_in_the_list_;
        }

        private bool ValidUrlSelected(string url)
        {
            return ListboxSelected.SelectedItems.Count == 1 &&
                   _settings.AppearanceOpenBrowser &&
                   ValidUrl(url);
        }

        /// <summary>
        ///     Is an Url valid?
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static bool ValidUrl(string url)
        {
            var result = Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                         && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        private static bool ContainNotSelectableLine(string line)
        {
            return line.Contains("NotSelectableLine()") || line.Contains("WebUrlGoTo(") ||
                   line.Contains("OutlookSearch(") || line.Contains("ConsoleOpen(");
        }

        private bool SelectionErrorCheckPassed()
        {
            if (ListboxMultiSelection.SelectedIndexesOrdered == null ||
                ListboxMultiSelection.SelectedItemsOrdered == null) return false;
            if (ListboxMultiSelection.SelectedIndexesOrdered.Count ==
                ListboxMultiSelection.SelectedItemsOrdered.Count && _isMultiSelectionClassReady)
                return true;
            _viewMain.LabelTitleTop.Text = Resources
                .PresenterMainSplContPanelUpTabs_SelectionErrorCheckPassed_Program_wasn_t_ready__selection_has_been_cleared__Please_select_again_;
            return false;
        }

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        //It's a known fault which Microsoft won't fix, therefore it's suppressed.
        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_clipboard")]
        private void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {
                _clipboard?.Dispose();
                _listBoxAuto?.Dispose();
                ListboxSelected?.Dispose();
            }

            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}