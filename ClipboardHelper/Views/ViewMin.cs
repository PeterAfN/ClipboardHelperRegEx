using ClipboardHelperRegEx.Views;
using System;
using System.Windows.Forms;
using Template;

namespace ClipboardHelper.Views
{
    public partial class ViewMin : FormTemplate, IViewMin
    {
        public ViewMin()
        {
            InitializeComponent();
            CreateEvents();
        }

        public event EventHandler OnViewMinFormLostFocusToOutsideApp;

        public event EventHandler MouseEnterLabelTitleTop;

        public Label LabelTitleTop
        {
            get { return labelTitleTop; }
        }

        private void CreateEvents()
        {
            labelTitleTop.MouseEnter += OnMouseEnterLabelTitleTop;
            OnFormLostFocusToOutsideApp += ViewMin_FormLostFocusToOutsideApp;
        }

        private void ViewMin_FormLostFocusToOutsideApp(object sender, EventArgs e)
        {
            OnViewMinFormLostFocusToOutsideApp?.Invoke(this, EventArgs.Empty);
        }

        private void OnMouseEnterLabelTitleTop(object sender, EventArgs e)
        {
            MouseEnterLabelTitleTop?.Invoke(this, e);
        }
    }
}