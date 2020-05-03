using ClipboardHelperRegEx.BusinessLogic;
using ClipboardHelperRegEx.ModifiedControls;
using ClipboardHelperRegEx.Views;
using System;
using System.Windows.Forms;

// ReSharper disable ConvertToAutoProperty
// ReSharper disable ConvertToAutoPropertyWhenPossible

namespace ClipboardHelper.Views
{
    public partial class ViewUserSettingsRightAutoShownTabs : UserControl,
        IViewUserSettingsRightAutoShownTabs
    {
        private AutoShownTabs _settings;

        public ViewUserSettingsRightAutoShownTabs()
        {
            InitializeComponent();
            CreateEvents();
            GroupBoxRightResult.Visible = false;
            Group4Help.Visible = false;
            Group3Items.Visible = false;
            Group2RegEx.Visible = false;
            deleteButton.Visible = false;
            renameButton.Visible = false;
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

        public bool Loaded { get; set; } = false;

        public AutoShownTabs Settings
        {
            get
            {
                _settings?.List.Sort((t1, t2) => string.Compare(t1.Name, t2.Name, StringComparison.CurrentCulture));
                return _settings;
            }
            set
            {
                if (value == null) return;
                _settings = value;
                _settings.List.Sort((t1, t2) => string.Compare(t1.Name, t2.Name, StringComparison.CurrentCulture));
            }
        }

        public event EventHandler EnabledChangedView;

        public event EventHandler ListLeftSimulatedTextChanged;

        public event EventHandler ItemsListTextChanged;

        public event EventHandler RegExStringTextChanged;

        public event EventHandler RuleNamesSelectionChanged;

        public event EventHandler ClickNewButton;

        public event EventHandler ClickDeleteButton;

        public event EventHandler ClickRenameButton;


        public FlowLayoutPanel FlowLayoutPanelRegExHelp
        {
            get { return flowLayoutPanelRegExHelp; }
        }


        public ModifiedListBox ListRightResult
        {
            get { return listRightResult; }
        }

        public GroupBox GroupBoxRightResult
        {
            get { return groupBoxRightResult; }
        }

        public RichTextBox ListLeftSimulated
        {
            get { return listLeftSimulated; }
        }

        public GroupBox Group4Help
        {
            get { return group4Help; }
        }

        public RichTextBox ItemsList
        {
            get { return itemsList; }
        }

        public GroupBox Group3Items
        {
            get { return group3Items; }
        }

        public RichTextBox RegExString
        {
            get { return regExString; }
        }

        public GroupBox Group2RegEx
        {
            get { return group2RegEx; }
        }

        public AdvancedComboBox RuleNames
        {
            get { return ruleNames; }
        }

        public Button DeleteButton
        {
            get { return deleteButton; }
        }

        public Button RenameButton
        {
            get { return renameButton; }
        }

        private void CreateEvents()
        {
            newButton.Click += OnClickNewButton;
            deleteButton.Click += OnClickDeleteButton;
            renameButton.Click += OnClickRenameButton;
            ruleNames.SelectedIndexChanged += OnRuleNamesSelectionChanged;
            regExString.TextChanged += OnRegExStringTextChanged;
            itemsList.TextChanged += OnItemsListTextChanged;
            listLeftSimulated.TextChanged += OnListLeftSimulatedTextChanged;
            EnabledChanged += OnEnabledChanged;
        }

        private void OnEnabledChanged(object sender, EventArgs e)
        {
            EnabledChangedView?.Invoke(this, e);
        }

        private void OnListLeftSimulatedTextChanged(object sender, EventArgs e)
        {
            ListLeftSimulatedTextChanged?.Invoke(this, e);
        }

        private void OnItemsListTextChanged(object sender, EventArgs e)
        {
            ItemsListTextChanged?.Invoke(this, e);
        }

        private void OnRegExStringTextChanged(object sender, EventArgs e)
        {
            RegExStringTextChanged?.Invoke(this, e);
        }

        private void OnRuleNamesSelectionChanged(object sender, EventArgs e)
        {
            RuleNamesSelectionChanged?.Invoke(this, e);
        }

        private void OnClickNewButton(object sender, EventArgs e)
        {
            ClickNewButton?.Invoke(this, e);
        }

        private void OnClickDeleteButton(object sender, EventArgs e)
        {
            ClickDeleteButton?.Invoke(this, e);
        }

        private void OnClickRenameButton(object sender, EventArgs e)
        {
            ClickRenameButton?.Invoke(this, e);
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference");
        }
    }
}