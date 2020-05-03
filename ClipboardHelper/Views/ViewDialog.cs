using ClipboardHelperRegEx.Views;
using System;
using System.Windows.Forms;
using Template;

// ReSharper disable ConvertToAutoProperty

namespace ClipboardHelper.Views
{
    public partial class ViewDialog : FormTemplate, IViewDialog
    {
        public ViewDialog()
        {
            InitializeComponent();
            CreateEvents();
            AcceptButton = buttonOk;
        }

        public event EventHandler ClickOkMouseButton;
        public event EventHandler ClickCancelMouseButton;


        public void SetText(string text)
        {
            userInformation.Text = text;
        }

        public TextBox UserInput
        {
            get { return userInput; }
        }

        public Panel PanelTitle
        {
            get { return panelTitle; }
        }

        public Panel PanelSeparatorLine
        {
            get { return panelSeparatorLine; }
        }

        private void CreateEvents()
        {
            buttonOk.Click += OnClickOkMouseButton;
            buttonCancel.Click += OnClickCancelMouseButton;
        }

        private void OnClickOkMouseButton(object sender, EventArgs e)
        {
            ClickOkMouseButton?.Invoke(this, e);
        }

        private void OnClickCancelMouseButton(object sender, EventArgs e)
        {
            ClickCancelMouseButton?.Invoke(this, e);
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
        }
    }
}