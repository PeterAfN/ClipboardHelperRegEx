using ClipboardHelperRegEx.Properties;
using ClipboardHelperRegEx.Views;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClipboardHelper.Views
{
    public partial class ViewUserSettingsDownButtons : UserControl, IViewUserSettingsButtonsDown
    {
        private readonly Button _apply;
        private readonly Button _cancel;
        private readonly Button _ok;
        private FlowLayoutPanel _buttonsAreaDown;

        public ViewUserSettingsDownButtons()
        {
            InitializeComponent();
            _ok = new Button { Dock = DockStyle.Right };
            _cancel = new Button();
            _apply = new Button();


            _ok.Click += OnOkClicked;
            _cancel.Click += OnCancelClicked;
            _apply.Click += OnApplyClicked;
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

        public event EventHandler OkClicked;

        public event EventHandler CancelClicked;

        public event EventHandler ApplyClicked;

        public void CreateControls()
        {
            _ok.Text = Resources.ViewUserSettingsDownButtons_CreateControls_Save;
            _ok.FlatStyle = FlatStyle.Flat;
            _ok.Dock = DockStyle.Right;
            _ok.Size = new Size(75, 25);
            _ok.ForeColor = Color.Black;
            _ok.BackColor = Color.White;

            _cancel.Text = Resources.ViewUserSettingsDownButtons_CreateControls_Cancel;
            _cancel.Size = new Size(75, 25);
            _cancel.FlatStyle = FlatStyle.Flat;
            _cancel.ForeColor = Color.Black;
            _cancel.BackColor = Color.White;

            _apply.Text = Resources.ViewUserSettingsDownButtons_CreateControls_Apply;
            _apply.Size = new Size(75, 25);
            _apply.FlatStyle = FlatStyle.Flat;
            _apply.ForeColor = Color.Black;
            _apply.BackColor = Color.White;

            _buttonsAreaDown = new FlowLayoutPanel
            {
                BackColor = Color.White,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false
            };
        }

        public void AddControls()
        {
            Controls.Add(_buttonsAreaDown);
            _buttonsAreaDown.Controls.Add(_apply);
            _buttonsAreaDown.Controls.Add(_cancel);
            _buttonsAreaDown.Controls.Add(_ok);
        }

        public void SetStatusOfApplyButton()
        {
            //in the future: Disable apply when pushed and enabled when a settings has changed.
        }

        public event EventHandler CancelIsClickedRestoreSettings;

        public void OnCancelIsClickedRestoreSettings(EventArgs e)
        {
            CancelIsClickedRestoreSettings?.Invoke(this, e);
        }

        public event EventHandler SaveIsClicked;

        public void OnSaveIsClicked(EventArgs e)
        {
            SaveIsClicked?.Invoke(this, e);
        }

        private void OnOkClicked(object sender, EventArgs e)
        {
            OkClicked?.Invoke(this, e);
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            CancelClicked?.Invoke(this, e);
        }

        private void OnApplyClicked(object sender, EventArgs e)
        {
            ApplyClicked?.Invoke(this, e);
        }
    }
}