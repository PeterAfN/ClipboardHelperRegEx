using ClipboardHelperRegEx.Views;
using System;
using System.Drawing;
using System.Windows.Forms;
using Template;

namespace ClipboardHelper.Views
{
    public partial class ViewMain : FormTemplate, IViewMain
    {
        private readonly ViewMainSplCont _viewMainSplCont;
        private readonly ViewMainSplContPanelDown _viewMainSplContPanelDown;
        private readonly ViewMainSplContPanelUpTabs _viewMainSplContPanelUp;

        public ViewMain
        (
            ViewMainSplCont viewMainSplCont,
            ViewMainSplContPanelUpTabs viewMainSplContPanelUp,
            ViewMainSplContPanelDown viewMainSplContPanelDown
        )
        {
            _viewMainSplCont = viewMainSplCont;
            _viewMainSplContPanelUp = viewMainSplContPanelUp;
            _viewMainSplContPanelDown = viewMainSplContPanelDown;

            InitializeComponent();
            CreateEvents();
        }

        public event EventHandler FormLostFocusToOutsideApp;
        public event EventHandler ClickToolStripMenuItemVisa;
        public event EventHandler ClickToolStripMenuItemOm;
        public event EventHandler ClickToolStripMenuItemEnd;
        public event EventHandler ClickToolStripMenuItemDeActivate;
        public event EventHandler ClickToolStripMenuItemSettings;
        public event MouseEventHandler MouseUpNotifyIconProgram;

        public void InitiateControls()
        {
            //Caption
            labelTitleTop.SendToBack();
        }

        public void AddControls()
        {
            Controls.Add(_viewMainSplCont);
            _viewMainSplCont.BringToFront();
            _viewMainSplCont.SplitContainer.Panel1.Controls.Add(_viewMainSplContPanelUp);
            _viewMainSplCont.SplitContainer.Panel2.Controls.Add(_viewMainSplContPanelDown);
            panelTitle.Refresh();
        }

        public Label LabelTitleTop
        {
            get { return labelTitleTop; }
        }

        /// <summary>
        ///     Sometimes the form isn't shown correctly when returning from hidden.
        ///     This is due to the method CreateParams which really effectively
        ///     eliminates flickering when resizing form, therefore we want it active.
        ///     Therefore this method should be called when Form has returned from hidden state.
        /// </summary>
        public bool RefreshAll
        {
            get { return true; } //dummy, just to prevent falsely shown analyzing errors
            // ReSharper disable once ValueParameterNotUsed
            set { Invalidate(true); }
        }

        public bool Resizing
        {
            get { return FormResizing; }
        }

        public void SetNotifyIconVisible(bool visible)
        {
            notifyIconProgram.Visible = visible;
        }

        public void SetNotifyIconImage(Icon icon)
        {
            notifyIconProgram.Icon = icon;
        }

        public void ShowContextMenu(Point location)
        {
            contextMenuStrip1.Show(location);
        }

        public void ShowNotifyIconProgram(bool show)
        {
            notifyIconProgram.Visible = show;
        }

        public void SetTextMenuDeActivate(string text)
        {
            toolStripMenuItemAvaktivera.Text = text;
        }

        private void CreateEvents()
        {
            toolStripMenuItemInställningar.Click += OnClickToolStripMenuItemSettings;
            toolStripMenuItemAvaktivera.Click += OnClickToolStripMenuItemDeActivate;
            toolStripMenuItemAvsluta.Click += OnClickToolStripMenuItemEnd;
            toolStripMenuItemOm.Click += OnClickToolStripMenuItemOm;
            toolStripMenuItemVisa.Click += OnClickToolStripMenuItemVisa;
            notifyIconProgram.MouseUp += OnMouseUpToolStripMenuItemSettings;
            OnFormLostFocusToOutsideApp += ViewMain_OnFormLostFocusToOutsideApp;
        }

        private void ViewMain_OnFormLostFocusToOutsideApp(object sender, EventArgs e)
        {
            FormLostFocusToOutsideApp?.Invoke(this, e);
        }

        private void OnClickToolStripMenuItemVisa(object sender, EventArgs e)
        {
            ClickToolStripMenuItemVisa?.Invoke(this, e);
        }

        private void OnClickToolStripMenuItemOm(object sender, EventArgs e)
        {
            ClickToolStripMenuItemOm?.Invoke(this, e);
        }

        private void OnClickToolStripMenuItemEnd(object sender, EventArgs e)
        {
            ClickToolStripMenuItemEnd?.Invoke(this, e);
        }

        private void OnClickToolStripMenuItemDeActivate(object sender, EventArgs e)
        {
            ClickToolStripMenuItemDeActivate?.Invoke(this, e);
        }

        private void OnClickToolStripMenuItemSettings(object sender, EventArgs e)
        {
            ClickToolStripMenuItemSettings?.Invoke(this, e);
        }

        private void OnMouseUpToolStripMenuItemSettings(object sender, MouseEventArgs e)
        {
            MouseUpNotifyIconProgram?.Invoke(this, e);
        }
    }
}