using ClipboardHelperRegEx.Properties;
using ClipboardHelperRegEx.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ClipboardHelperRegEx.BusinessLogic.Presenters
{
    public class PresenterUserSettingsRightAutoShownTabs
    {

        private readonly List<Tags.TagType> _tags = new List<Tags.TagType>
        {
            Tags.TagType.Regex,
            Tags.TagType.RegexReplace,
            Tags.TagType.Clipboard,
            Tags.TagType.WebUrlGoTo,
            Tags.TagType.ChangeableContent,
            Tags.TagType.OutlookSearch,
            Tags.TagType.RegexCsvFileGet,
            Tags.TagType.Encrypt,
            Tags.TagType.NotSelectableLine,
            Tags.TagType.Caption,
            Tags.TagType.Info,
            Tags.TagType.WebGetJason,
            Tags.TagType.WebGet,
            Tags.TagType.FromUnixTimeMilliSecondsToUtc,
            Tags.TagType.ConsoleOpen,
            Tags.TagType.NewLine
        };

        private readonly IViewDialog _dialog;
        private readonly ISettingsServiceXmlSerialization _settingsServiceXmlSerialization;
        private readonly IViewMainSplContPanelUpTabs _viewMainSplContPanelUpTabs;
        private readonly IViewUserSettingsRightAutoShownTabs _view;
        private readonly IViewUserSettings _viewUserSettings;
        private readonly IViewUserSettingsButtonsDown _viewUserSettingsButtonsDown;

        private string _clipboardSimulated;
        private string _selectedContextMenu = "";
        private int _selectedRuleIndex;

        public PresenterUserSettingsRightAutoShownTabs
        (
            IViewUserSettingsRightAutoShownTabs viewRules,
            IViewDialog dialog,
            IViewUserSettings viewUserSettings,
            IViewUserSettingsButtonsDown viewUserSettingsButtonsDown,
            ISettingsServiceXmlSerialization settingsServiceXmlSerialization,
            IViewMainSplContPanelUpTabs viewMainSplContPanelUpTabs
        )
        {
            _view = viewRules;
            _dialog = dialog;
            _viewUserSettings = viewUserSettings;
            _viewUserSettingsButtonsDown = viewUserSettingsButtonsDown;
            _settingsServiceXmlSerialization = settingsServiceXmlSerialization;
            _viewMainSplContPanelUpTabs = viewMainSplContPanelUpTabs;

            //subscribe to events   
            if (_view == null) return;
            _view.Load += ViewRules_Load;
            _view.ListLeftSimulatedTextChanged += ViewRules_ListLeftSimulatedTextChanged;
            _view.ItemsListTextChanged += ViewRules_ItemsListTextChanged;
            _view.RegExStringTextChanged += ViewRules_RegExStringTextChanged;
            _view.RuleNamesSelectionChanged += ViewRules_RuleNamesSelectionChanged;
            _view.ClickNewButton += ViewRules_ClickNewButton;
            _view.ClickDeleteButton += ViewRules_ClickDeleteButton;
            _view.ClickRenameButton += ViewRules_ClickRenameButton;
            if (_dialog != null)
            {
                _dialog.ClickCancelMouseButton += Dialog_ClickCancelMouseButton;
                _dialog.ClickOkMouseButton += Dialog_ClickOkMouseButton;
            }

            _view.EnabledChangedView += View_EnabledChanged;
        }

        private string ClipboardSimulated
        {
            get { return _clipboardSimulated; }
            set { _clipboardSimulated = value.Trim(); }
        }

        private void View_EnabledChanged(object sender, EventArgs e)
        {
            //Run every time this presenters view is shown, except when loaded
            if (_view.Enabled && _view.Loaded && !_dialog.Visible) UpdateListboxRightText();
        }

        private void ViewRules_ClickRenameButton(object sender, EventArgs e)
        {
            _selectedRuleIndex = _view.RuleNames.SelectedIndex;
            _dialog.SetText("Please enter the new name:");
            _dialog.Tag = ToString();
            _dialog.Show();
            _selectedContextMenu = "Rename";
            _view.Group2RegEx.Hide();
            _view.FlowLayoutPanelRegExHelp.Hide();
            _view.Group3Items.Hide();
            _view.Group4Help.Hide();
            _viewUserSettings.Enabled = false;
        }

        private void ViewRules_ClickDeleteButton(object sender, EventArgs e)
        {
            _selectedRuleIndex = _view.RuleNames.SelectedIndex;
            _view.Group2RegEx.Hide();
            _view.FlowLayoutPanelRegExHelp.Hide();
            _view.Group3Items.Hide();
            _view.Group4Help.Hide();
            _viewUserSettings.Enabled = false;
            _dialog.Tag = ToString();
            _dialog.UserInput.Hide();
            _dialog.SetText("Do you want to delete: " +
                            _view.RuleNames.SelectedItem + "?");
            _dialog.Show();
            _selectedContextMenu = "Delete";
        }

        private void ViewUserSettingsButtonsDown_ApplyClicked(object sender, EventArgs e)
        {
            if (_view.Loaded) _settingsServiceXmlSerialization.AutoShownTabs = _view.Settings;
        }

        private void ViewUserSettingsButtonsDown_CancelClicked(object sender, EventArgs e)
        {
            _view.Settings?.List.Clear();
            HideControls();
            ViewRules_Load(this, EventArgs.Empty);
        }

        private void IViewUserSettingsButtonsDown_OkClicked(object sender, EventArgs e)
        {
            if (_view.Loaded) //in case this class hasn't been shown by the user
                _settingsServiceXmlSerialization.AutoShownTabs = _view.Settings;
        }

        private void Dialog_ClickOkMouseButton(object sender, EventArgs e)
        {
            if (_dialog.Tag.ToString() != ToString()) return;
            _dialog.UserInput.Show(); //if hidden when delete
            _viewUserSettings.Enabled = true;
            var inputTxt = _dialog.UserInput.Text;
            switch (_selectedContextMenu)
            {
                case "New":
                    _view.Group2RegEx.Show();
                    _view.FlowLayoutPanelRegExHelp.Show();
                    HideControls();
                    ClearContent();
                    _view.Settings.List.Add(new AutoShownTab
                    {
                        Name = inputTxt,
                        RegEx = string.Empty,
                        Items = new List<string>(),
                        SimulatedClipboard = "Please edit this text as needed."
                    });
                    _view.RuleNames.Items.Add(inputTxt);
                    _view.RuleNames.SelectedIndex = _view.RuleNames.FindStringExact(inputTxt);
                    _view.Group2RegEx.Show();
                    _view.FlowLayoutPanelRegExHelp.Show();
                    _view.DeleteButton.Visible = true;
                    _view.RenameButton.Visible = true;
                    _view.Group2RegEx.Text =
                        Resources
                            .PresenterUserSettingsRightAutoShownTabs_Dialog_ClickOkMouseButton__2_Please_edit___insert_the_RegEx_to_be_associated_with___ +
                        _view.Settings.List[_view.RuleNames.SelectedIndex].Name + Resources
                            .PresenterUserSettingsRightAutoShownTabs_Dialog_ClickOkMouseButton___;
                    break;
                case "Rename":
                    _view.Settings.List[_selectedRuleIndex].Name = inputTxt;
                    UpdateOrderRuleNames();
                    _view.RuleNames.SelectedIndex = _view.RuleNames.FindStringExact(inputTxt);
                    break;
                case "Delete":
                    if (_view.RuleNames.Items.Count == 1) return;
                    HideControls();
                    ClearContent();
                    _view.RuleNames.Items.RemoveAt(_selectedRuleIndex);
                    _view.Settings.List.RemoveAt(_selectedRuleIndex);
                    _view.DeleteButton.Visible = false;
                    _view.RenameButton.Visible = false;
                    break;
            }
        }

        private void Dialog_ClickCancelMouseButton(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_view?.RuleNames?.SelectedItem?.ToString())) return;
            _view.Group2RegEx.Show();
            _view.FlowLayoutPanelRegExHelp.Show();
            _view.Group3Items.Show();
            _view.Group4Help.Show();
        }

        private void ViewRules_ClickNewButton(object sender, EventArgs e)
        {
            _dialog.SetText("Please enter the name of the new AutoTab to be added:");
            _dialog.Show();
            _dialog.Tag = ToString();
            _selectedContextMenu = "New";
            _view.Group2RegEx.Hide();
            _view.FlowLayoutPanelRegExHelp.Hide();
            _view.Group3Items.Hide();
            _view.Group4Help.Hide();
            _viewUserSettings.Enabled = false;
        }

        private void ClearContent()
        {
            _view.RegExString.Clear();
            _view.ItemsList.Clear();
            _view.ListLeftSimulated.Clear();
        }

        private void HideControls()
        {
            _view.Group2RegEx.Hide();
            _view.FlowLayoutPanelRegExHelp.Hide();
            _view.Group3Items.Hide();
            _view.Group4Help.Hide();
            _view.GroupBoxRightResult.Hide();
            _view.ListLeftSimulated.Hide();
        }

        private void ShowControls()
        {
            _view.Group2RegEx.Show();
            _view.FlowLayoutPanelRegExHelp.Show();
            _view.Group3Items.Show();
            _view.Group4Help.Show();
            _view.GroupBoxRightResult.Show();
            _view.ListLeftSimulated.Show();
        }

        private void ViewRules_RuleNamesSelectionChanged(object sender, EventArgs e)
        {
            //if (_view.RuleNames.SelectedIndex == -1) return;
            if (_view.RuleNames.Focused)
            {
                if (Tags.TokenSource != null)
                    Tags.TokenSource.Cancel(true); //Aborts filling lines (DelayedTags from web...)
                var ruleName = string.Empty;
                if (_view.RuleNames.SelectedIndex != -1)
                    ruleName = _view.Settings.List[_view.RuleNames.SelectedIndex].Name;
                _view.DeleteButton.Visible = true;
                _view.RenameButton.Visible = true;
                ClearContent();
                if (_view.RuleNames.SelectedIndex != -1)
                    _view.RegExString.Text =
                    _view.Settings.List[_view.RuleNames.SelectedIndex].RegEx;
                if (_view.RuleNames.SelectedIndex != -1)
                    _view.ItemsList.Lines =
                    _view.Settings.List[_view.RuleNames.SelectedIndex].Items.ToArray();
                if (_view.RuleNames.SelectedIndex != -1)
                    _view.ListLeftSimulated.Text =
                    ClipboardSimulated =
                        _view.Settings.List[_view.RuleNames.SelectedIndex].SimulatedClipboard;
                _view.Group2RegEx.Text =
                    Resources
                        .PresenterUserSettingsRightAutoShownTabs_Dialog_ClickOkMouseButton__2_Please_edit___insert_the_RegEx_to_be_associated_with___ +
                    ruleName + Resources
                        .PresenterUserSettingsRightAutoShownTabs_ViewRules_RuleNamesSelectionChanged___;
                _view.Group3Items.Text =
                    Resources
                        .PresenterUserSettingsRightAutoShownTabs_ViewRules_RuleNamesSelectionChanged__3__When_Clipboard_text_content_changes_and_RegEx_for_Tab___ +
                    ruleName +
                    Resources
                        .PresenterUserSettingsRightAutoShownTabs_ViewRules_RuleNamesSelectionChanged___matches_with_it__please_insert_what_is_shown_;
                _view.Group4Help.Text =
                    Resources
                        .PresenterUserSettingsRightAutoShownTabs_ViewRules_RuleNamesSelectionChanged_Help__Test_if_the_regEx_rule_for_Tab___ +
                    ruleName +
                    Resources
                        .PresenterUserSettingsRightAutoShownTabs_ViewRules_RuleNamesSelectionChanged___functions_correctly_by_inserting_;
                ShowControls();
            }

            UpdateListboxRightText();
        }

        private void ViewRules_RegExStringTextChanged(object sender, EventArgs e)
        {
            if (!_view.RegExString.Focused) return;
            if (_view.RuleNames.SelectedIndex == -1) return;
            var ruleName = string.Empty;
            if (_view.RuleNames.SelectedIndex != -1)
                ruleName = _view.Settings.List[_view.RuleNames.SelectedIndex].Name;
            if (_view.Group3Items.Visible != true) _view.Group3Items.Show();
            _view.Settings.List[_view.RuleNames.SelectedIndex].RegEx =
                _view.RegExString.Text;
            if (_view.RuleNames.SelectedIndex != -1)
                _view.Group3Items.Text =
                Resources
                    .PresenterUserSettingsRightAutoShownTabs_ViewRules_RegExStringTextChanged__3__When_Clipboard_text_content_changes_and_RegEx_for___ +
                _view.Settings.List[_view.RuleNames.SelectedIndex].Name + Resources
                    .PresenterUserSettingsRightAutoShownTabs_ViewRules_RuleNamesSelectionChanged___matches_with_it__please_insert_what_is_shown_;
            UpdateListboxRightText();
            if (IsValidRegex(_view.RegExString.Text))
                _view.Group2RegEx.Text =
                    Resources
                        .PresenterUserSettingsRightAutoShownTabs_Dialog_ClickOkMouseButton__2_Please_edit___insert_the_RegEx_to_be_associated_with___ +
                    ruleName +
                    Resources.PresenterUserSettingsRightAutoShownTabs_ViewRules_RegExStringTextChanged___ +
                    Resources
                        .PresenterUserSettingsRightAutoShownTabs_ViewRules_RegExStringTextChanged___RegEx_is_valid_;
            else
                _view.Group2RegEx.Text =
                    Resources
                        .PresenterUserSettingsRightAutoShownTabs_Dialog_ClickOkMouseButton__2_Please_edit___insert_the_RegEx_to_be_associated_with___ +
                    ruleName +
                    Resources.PresenterUserSettingsRightAutoShownTabs_ViewRules_RegExStringTextChanged___ +
                    Resources
                        .PresenterUserSettingsRightAutoShownTabs_ViewRules_RegExStringTextChanged___RegEx_not_valid_;
        }

        private static bool IsValidRegex(string pattern)
        {
            if (string.IsNullOrEmpty(pattern)) return false;
            try
            {
                var unused = Regex.Match("", pattern);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }

        private void ViewRules_ItemsListTextChanged(object sender, EventArgs e)
        {
            if (_view.ItemsList.Focused)
                if (_view.RuleNames.SelectedIndex != -1)
                {
                    _view.Settings.List[_view.RuleNames.SelectedIndex].Items.Clear();
                    _view.Settings.List[_view.RuleNames.SelectedIndex].Items.AddRange(_view.ItemsList.Lines);
                    if (_view.Group4Help.Visible != true) _view.Group4Help.Show();
                    _view.ListLeftSimulated.Show();
                }

            UpdateListboxRightText();
        }

        private void InitiateListbox(int nrItems)
        {
            _view.ListRightResult.UiThread
            (delegate
                {
                    _view.ListRightResult.Items.Clear();
                    for (var i = 0; i < nrItems; i++)
                        _view.ListRightResult.Items.Add(string.Empty);
                }
            );
        }

        private void InsertItemsListbox(int dummyPosition, int dummyPositionAndId, PresenterMainSplContPanelUpTabs.LineChangeType dummyNotRelevant,
            int dummyNotRelevant2,
            IEnumerable<string> lines,
            bool changedContentEdited = false,
            string changeableContent = "",
            int dummy = -1)
        {
            if (Tags.TokenSource.Token.IsCancellationRequested) return;
            _view.ListRightResult.UiThread
            (delegate
                {
                    //try
                    //{
                    _view.ListRightResult.Items.Clear();
                    if (lines == null) return;
                    // ReSharper disable once CoVariantArrayConversion
                    _view.ListRightResult.Items.AddRange(lines.ToArray());
                    //}
                    //catch (Exception ex)
                    //{
                    //    MessageBox.Show(Resources.PresenterUserSettingsRightAutoShownTabs_InsertItemsListbox_An_error_occured_in_the_user_setting_when_setting_a_new_item_to_listbox__ + ex, Resources.PresenterUserSettingsRightAutoShownTabs_InsertItemsListbox_Clipboard_Helper_error, MessageBoxButtons.OK);
                    //}
                }
            );
        }

        private void UpdateListboxRightText()
        {
            try
            {
                if (ClipboardSimulated == null) return;
                if (Regex.IsMatch(ClipboardSimulated, _view.RegExString.Text))
                {
                    _view.ListRightResult.Items.Clear();
                    var tags = new Tags();
                    var unused = new List<string>();
                    InitiateListbox(_view.ItemsList.Lines.Length);
                    tags.TransformLines(_viewMainSplContPanelUpTabs.NavigationPosition, _viewMainSplContPanelUpTabs.NavigationPositionAndId.Values[_viewMainSplContPanelUpTabs.NavigationPosition],
                        PresenterMainSplContPanelUpTabs.LineChangeType.AutoMulti,
                        0, //Note: only relevant with manual tags, not here.
                        _view.ItemsList.Lines.ToList(),
                        _tags,
                        InsertItemsListbox,
                        ClipboardSimulated,
                        false, //note: Not relevant here
                        string.Empty //note: Not relevant here
                    );
                }
                else
                {
                    _view.ListRightResult.Items.Clear();
                    _view.ListRightResult.Items.Add("Regex doesn't match with Clipboard.");
                }
            }
            catch (Exception) //catch regex which isn't valid.
            {
                var ruleName = string.Empty;
                if (_view.RuleNames.SelectedIndex != -1)
                    ruleName = _view.Settings.List[_view.RuleNames.SelectedIndex].Name;
                _view.Group2RegEx.Text =
                    Resources
                        .PresenterUserSettingsRightAutoShownTabs_Dialog_ClickOkMouseButton__2_Please_edit___insert_the_RegEx_to_be_associated_with___ +
                    ruleName +
                    Resources.PresenterUserSettingsRightAutoShownTabs_UpdateListboxRightText___ +
                    Resources
                        .PresenterUserSettingsRightAutoShownTabs_ViewRules_RegExStringTextChanged___RegEx_not_valid_;
                _view.ListRightResult.Items.Clear(); //user has entered wrong RexEx string in the settings.
                _view.ListRightResult.Items.Add("Regex doesn't match with Clipboard.");
            }
        }

        private void ViewRules_ListLeftSimulatedTextChanged(object sender, EventArgs e)
        {
            ClipboardSimulated = _view.ListLeftSimulated.Text;
            if (string.IsNullOrEmpty(_view.ListLeftSimulated.Text)) return;
            _view.GroupBoxRightResult.Show();
            UpdateListboxRightText();
            _view.Settings.List[_view.RuleNames.SelectedIndex].SimulatedClipboard = _view.ListLeftSimulated.Text;
        }

        private void ViewRules_Load(object sender, EventArgs e)
        {
            ClearContent();
            HideControls();
            _view.Settings = _settingsServiceXmlSerialization.AutoShownTabs;
            UpdateOrderRuleNames();
            _view.DeleteButton.Visible = false;
            _view.RenameButton.Visible = false;
            _view.Loaded = true;
            UpdateListboxRightText();
            _viewUserSettingsButtonsDown.OkClicked += IViewUserSettingsButtonsDown_OkClicked;
            _viewUserSettingsButtonsDown.CancelClicked += ViewUserSettingsButtonsDown_CancelClicked;
            _viewUserSettingsButtonsDown.ApplyClicked += ViewUserSettingsButtonsDown_ApplyClicked;
        }

        private void UpdateOrderRuleNames()
        {
            _view.RuleNames.Items.Clear();
            foreach (var element in _view.Settings.List) _view.RuleNames.Items.Add(element.Name);
        }
    }
}