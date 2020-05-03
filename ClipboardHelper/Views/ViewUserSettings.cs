using ClipboardHelperRegEx.Properties;
using ClipboardHelperRegEx.Views;
using System.Drawing;
using System.Windows.Forms;
using Template;

namespace ClipboardHelper.Views
{
    public partial class ViewUserSettings : FormTemplate, IViewUserSettings
    {
        private readonly TableLayoutPanel _settings;
        private readonly GroupBox _settingsLeft;
        private readonly ViewUserSettingsLeftMenu _settingsLeftTreeview;
        private readonly GroupBox _settingsRight;
        private readonly ViewUserSettingsDownButtons _viewUserSettingsButtonsDown;

        public ViewUserSettings(TableLayoutPanel settings,
            ViewUserSettingsDownButtons viewUserSettingsButtonsDown,
            GroupBox settingsLeft,
            GroupBox settingsRight,
            ViewUserSettingsLeftMenu settingsLeftTreeview
        )
        {
            _settings = settings;
            _viewUserSettingsButtonsDown = viewUserSettingsButtonsDown;
            _settingsLeft = settingsLeft;
            _settingsRight = settingsRight;
            _settingsLeftTreeview = settingsLeftTreeview;
            InitializeComponent();
        }

        public void SetGroupBoxText(string text)
        {
            _settingsRight.Text = text;
        }

        public void InitiateControls()
        {
            //Caption
            labelTitleTop.Text = Resources.ViewUserSettings_InitiateControls_Settings;

            //settingsArea          
            _settings.BackColor = Color.White;
            _settings.ColumnCount = 2;
            _settings.Dock = DockStyle.Fill;
            _settings.ColumnStyles.Add(new ColumnStyle());
            _settings.ColumnStyles.Add(new ColumnStyle());
            _settings.RowCount = 1;
            _settings.RowStyles.Add(new RowStyle());
            _settings.RowStyles.Add(new RowStyle(SizeType.Absolute, 406F));

            //settingsAreaLeft          
            _settingsLeft.AutoSize = true;
            _settingsLeft.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _settingsLeft.BackColor = Color.White;
            _settingsLeft.Dock = DockStyle.Fill;
            _settingsLeft.ForeColor = Color.Black;
            _settingsLeft.MinimumSize = new Size(143, 0);
            _settingsLeft.Name = "groupBoxSettingsMenu";
            _settingsLeft.Size = new Size(143, 400);
            _settingsLeft.TabIndex = 9;
            _settingsLeft.TabStop = false;
            _settingsLeft.Text = Resources.ViewUserSettings_InitiateControls_Settings_;

            //settingsAreaRight          
            _settingsRight.BackColor = Color.White;
            _settingsRight.Text = "";
            _settingsRight.Dock = DockStyle.Fill;
            _settingsRight.AutoSize = true;
            _settingsRight.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _settingsRight.FlatStyle = FlatStyle.Flat;
            _settingsRight.ForeColor = Color.Black;

            //viewUserSettingsButtonsDown
            _viewUserSettingsButtonsDown.BackColor = Color.White;
            _viewUserSettingsButtonsDown.BackColor = Color.White;
            _viewUserSettingsButtonsDown.Dock = DockStyle.Bottom;
            _viewUserSettingsButtonsDown.Name = "groupBoxSettingsMenu";
            _viewUserSettingsButtonsDown.Size = new Size(0, 30);
        }

        public void AddControls()
        {
            _settings.Controls.Add(_settingsRight, 1, 0);
            _settings.Controls.Add(_settingsLeft, 0, 0);
            _settingsLeft.Controls.Add(_settingsLeftTreeview);
            Controls.Add(_settings);
            Controls.Add(_viewUserSettingsButtonsDown);
            Controls.Add(panelSeparatorLine);
            Controls.Add(panelTitle);
        }

        public void ChangeRightView(UserControl activeView)
        {
            if (activeView == null) return;
            activeView.Enabled = false;
            activeView.Enabled =
                true; //to create an event every time user control is shown. Shown event doesn't exist for UserControl
            _settingsRight.Controls.Clear();
            _settingsRight.Controls.Add(activeView);
        }
    }
}