using ClipboardHelperRegEx.BusinessLogic;
using ClipboardHelperRegEx.ModifiedControls;
using System;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.Views
{
    public interface IViewUserSettingsRightAutoShownTabs
    {
        FlowLayoutPanel FlowLayoutPanelRegExHelp { get; }
        ModifiedListBox ListRightResult { get; }
        GroupBox GroupBoxRightResult { get; }
        RichTextBox ListLeftSimulated { get; }
        GroupBox Group4Help { get; }
        RichTextBox ItemsList { get; }
        GroupBox Group3Items { get; }
        RichTextBox RegExString { get; }
        GroupBox Group2RegEx { get; }
        AdvancedComboBox RuleNames { get; }
        Button DeleteButton { get; }
        Button RenameButton { get; }

        bool Enabled { get; }
        bool Loaded { get; set; }

        void Hide();
        AutoShownTabs Settings { get; set; }
        event EventHandler Load;

        event EventHandler ListLeftSimulatedTextChanged;
        event EventHandler ItemsListTextChanged;
        event EventHandler RegExStringTextChanged;
        event EventHandler RuleNamesSelectionChanged;
        event EventHandler ClickNewButton;
        event EventHandler ClickDeleteButton;
        event EventHandler ClickRenameButton;
        event EventHandler EnabledChangedView;
    }
}