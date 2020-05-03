using ClipboardHelperRegEx.Views;
using System;
using System.Windows.Forms;

// ReSharper disable ConvertToAutoProperty
// ReSharper disable ConvertToAutoPropertyWhenPossible

namespace ClipboardHelper.Views
{
    public partial class ViewMainSplContPanelDown : UserControl, IViewMainSplContPanelDown
    {
        public ViewMainSplContPanelDown()
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

        public event EventHandler HistoryLeftMouseEnter;
        public event EventHandler HistoryLeftMouseLeave;
        public event EventHandler HistoryLeftMouseClick;
        public event EventHandler HistoryRightMouseEnter;
        public event EventHandler HistoryRightMouseLeave;
        public event EventHandler HistoryRightMouseClick;

        public Label Position
        {
            get { return labelPositionH; }
        }

        public RichTextBox Clipboard
        {
            get { return textboxClipboard; }
        }

        public PictureBox HistoryLeft
        {
            get { return historyLeft; }
        }

        public PictureBox HistoryRight
        {
            get { return historyRight; }
        }

        public string PositionText
        {
            get { return labelPositionH.Text; }
            set { labelPositionH.Text = value; }
        }

        private void CreateEvents()
        {
            historyLeft.MouseEnter += HistoryLeft_MouseEnter;
            historyLeft.MouseLeave += HistoryLeft_MouseLeave;
            historyLeft.MouseClick += HistoryLeft_MouseClick;
            historyRight.MouseEnter += HistoryRight_MouseEnter;
            historyRight.MouseLeave += HistoryRight_MouseLeave;
            historyRight.MouseClick += HistoryRight_MouseClick;
        }

        private void HistoryLeft_MouseEnter(object sender, EventArgs e)
        {
            HistoryLeftMouseEnter?.Invoke(this, e);
        }

        private void HistoryLeft_MouseLeave(object sender, EventArgs e)
        {
            HistoryLeftMouseLeave?.Invoke(this, e);
        }

        private void HistoryLeft_MouseClick(object sender, EventArgs e)
        {
            HistoryLeftMouseClick?.Invoke(this, e);
        }

        private void HistoryRight_MouseEnter(object sender, EventArgs e)
        {
            HistoryRightMouseEnter?.Invoke(this, e);
        }

        private void HistoryRight_MouseLeave(object sender, EventArgs e)
        {
            HistoryRightMouseLeave?.Invoke(this, e);
        }

        private void HistoryRight_MouseClick(object sender, EventArgs e)
        {
            HistoryRightMouseClick?.Invoke(this, e);
        }
    }
}