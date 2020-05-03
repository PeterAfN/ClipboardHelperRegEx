using ClipboardHelperRegEx.ModifiedControls;
using ClipboardHelperRegEx.Views;
using System;
using System.Windows.Forms;

// ReSharper disable ConvertToAutoPropertyWhenPossible
// ReSharper disable ConvertToAutoProperty

namespace ClipboardHelper.Views
{
    public partial class ViewUserSettingsRightAppearance : UserControl, IViewUserSettingsRightAppearance
    {
        public ViewUserSettingsRightAppearance()
        {
            InitializeComponent();
            CreateEvents();
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

        public event EventHandler OnProgramsAlternativePastingTextChanged;

        public event MouseEventHandler OnLabelTextColorSampleMouseClick;

        public event EventHandler OnAppearanceColorChoicesSelectedIndexChanged;

        public event EventHandler OnViewUserSettingsRightAppearanceVisibleChanged;

        public event EventHandler EnabledChangedView;

        public event EventHandler TextChangedSeconds;

        public event EventHandler SelectedItemChangedShortcutKeys;

        public event EventHandler SelectedItemChangedFadeTypeChoice;

        public event EventHandler CheckedChangedIsWinEnabled;

        public event EventHandler CheckedChangedIsShiftEnabled;

        public event EventHandler CheckedChangedIsAltEnabled;

        public event EventHandler CheckedChangedIsCtrlEnabled;

        public event EventHandler CheckedChangedIsFocusEnabled;

        public event EventHandler CheckedChangedIsAutoStartEnabled;

        public event EventHandler CheckedChangedIsHidingEnabled;

        public TextBox ProgramsAlternativePasting
        {
            get { return textBoxProgramsAlternativePasting; }
        }

        public CheckBox IsClosingEnabled
        {
            get { return isClosingEnabled; }
        }

        public AdvancedComboBox FadeTypeChoice
        {
            get { return fadeTypeChoice; }
        }

        public TextBox Seconds
        {
            get { return seconds; }
        }

        public Label FadingText
        {
            get { return fadingText; }
        }

        public Label SecondsText
        {
            get { return secondsText; }
        }

        public CheckBox DoAutoStartProgram
        {
            get { return doAutostartProgram; }
        }

        public CheckBox IsFocusEnabled
        {
            get { return isFocusEnabled; }
        }

        public CheckBox IsCtrlEnabled
        {
            get { return isCTRLEnabled; }
        }

        public CheckBox IsAltEnabled
        {
            get { return isALTEnabled; }
        }

        public CheckBox IsShiftEnabled
        {
            get { return isSHIFTEnabled; }
        }

        public CheckBox IsWinEnabled
        {
            get { return isWINEnabled; }
        }

        public AdvancedComboBox ShortcutKeysComboBox
        {
            get { return shortcutKeysComboBox; }
        }

        public ColorDialog ColorDialog
        {
            get { return colorDialog1; }
        }

        public Label LabelTextColorSample
        {
            get { return labelTextColorSample; }
        }

        public AdvancedComboBox AppearanceColorChoices
        {
            get { return appearanceColorChoices; }
        }

        public TextBox Password
        {
            get { return textBoxSecurePassword; }
        }

        public Button ResetToFactorySettings
        {
            get { return resetToFactorySettings; }
        }

        private void CreateEvents()
        {
            textBoxProgramsAlternativePasting.TextChanged += TextBoxProgramsAlternativePasting_TextChanged;
            isClosingEnabled.CheckedChanged += IsClosingEnabled_CheckedChanged;
            doAutostartProgram.CheckedChanged += DoAutoStartProgram_CheckedChanged;
            isFocusEnabled.CheckedChanged += IsFocusEnabled_CheckedChanged;
            isCTRLEnabled.CheckedChanged += IsCTRLEnabled_CheckedChanged;
            isALTEnabled.CheckedChanged += IsALTEnabled_CheckedChanged;
            isSHIFTEnabled.CheckedChanged += IsSHIFTEnabled_CheckedChanged;
            isWINEnabled.CheckedChanged += IsWINEnabled_CheckedChanged;
            fadeTypeChoice.SelectedIndexChanged += FadeTypeChoice_SelectedIndexChanged;
            shortcutKeysComboBox.SelectedIndexChanged += ShortcutKeysComboBox_SelectedIndexChanged;
            Seconds.TextChanged += Seconds_TextChanged;
            EnabledChanged += OnEnabledChanged;
            VisibleChanged += ViewUserSettingsRightAppearance_VisibleChanged;
            appearanceColorChoices.SelectedIndexChanged += AppearanceColorChoices_SelectedIndexChanged;
            labelTextColorSample.MouseClick += LabelTextColorSample_MouseClick;
        }

        private void TextBoxProgramsAlternativePasting_TextChanged(object sender, EventArgs e)
        {
            OnProgramsAlternativePastingTextChanged?.Invoke(textBoxProgramsAlternativePasting, e);
        }

        private void LabelTextColorSample_MouseClick(object sender, MouseEventArgs e)
        {
            OnLabelTextColorSampleMouseClick?.Invoke(this, e);
        }

        private void AppearanceColorChoices_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnAppearanceColorChoicesSelectedIndexChanged?.Invoke(this, e);
        }

        private void ViewUserSettingsRightAppearance_VisibleChanged(object sender, EventArgs e)
        {
            OnViewUserSettingsRightAppearanceVisibleChanged?.Invoke(
                OnViewUserSettingsRightAppearanceVisibleChanged, e);
        }

        private void OnEnabledChanged(object sender, EventArgs e)
        {
            EnabledChangedView?.Invoke(this, e);
        }

        private void Seconds_TextChanged(object sender, EventArgs e)
        {
            TextChangedSeconds?.Invoke(Seconds, e);
        }

        private void ShortcutKeysComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedItemChangedShortcutKeys?.Invoke(this, e);
        }

        private void FadeTypeChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedItemChangedFadeTypeChoice?.Invoke(this, e);
        }

        private void IsWINEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckedChangedIsWinEnabled?.Invoke(this, e);
        }

        private void IsSHIFTEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckedChangedIsShiftEnabled?.Invoke(this, e);
        }

        private void IsALTEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckedChangedIsAltEnabled?.Invoke(this, e);
        }

        private void IsCTRLEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckedChangedIsCtrlEnabled?.Invoke(this, e);
        }

        private void IsFocusEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckedChangedIsFocusEnabled?.Invoke(this, e);
        }

        private void DoAutoStartProgram_CheckedChanged(object sender, EventArgs e)
        {
            CheckedChangedIsAutoStartEnabled?.Invoke(this, e);
        }

        private void IsClosingEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckedChangedIsHidingEnabled?.Invoke(this, e);
        }
    }
}