using ClipboardHelperRegEx.BusinessLogic;
using ClipboardHelperRegEx.ModifiedControls;
using ClipboardHelperRegEx.Views;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

// ReSharper disable ConvertToAutoPropertyWhenPossible
// ReSharper disable ConvertToAutoProperty

namespace ClipboardHelper.Views
{
    public partial class ViewMainSplContPanelUpTabs : UserControl, IViewMainSplContPanelUpTabs
    {
        private AutoShownTabs _autoShownTabs;

        public ViewMainSplContPanelUpTabs()
        {
            InitializeComponent();
            CreateEvents();
        }

        public static string ClipboardStored { get; set; }

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

        public event KeyPressEventHandler OnTextBoxChangeableContentKeyPress;

        public event EventHandler OnTextBoxChangeableContentTextChanged;

        public event EventHandler OnMetroTabControlSelectedIndexChanged;

        public event EventHandler NoItemsSelected;

        public void TriggerEventOnNoItemsSelected(EventArgs e)
        {
            NoItemsSelected?.Invoke(this, e);
        }

        public event EventHandler ItemsSelected;

        public void TriggerEventOnItemsSelected(EventArgs e)
        {
            ItemsSelected?.Invoke(this, e);
        }

        public AutoShownTabs AutoShownTabsRam
        {
            get
            {
                _autoShownTabs.List.Sort((t1, t2) => string.Compare(t1.Name, t2.Name, StringComparison.Ordinal));
                return _autoShownTabs;
            }
            set
            {
                if (value == null) return;
                _autoShownTabs = value;
                _autoShownTabs.List.Sort((t1, t2) => string.Compare(t1.Name, t2.Name, StringComparison.Ordinal));
            }
        }

        public ManuallyShownTabs ManuallyShownTabsRam { get; set; } = new ManuallyShownTabs
        (
        );

        public MetroTabControl TabControl
        {
            get { return metroTabControl; }
        }

        public GroupBox GroupBoxAuto
        {
            get { return groupBoxAuto; }
        }

        public Panel PanelChangeableContent
        {
            get { return panelChangeableContent; }
        }

        public TextBox TextBoxChangeableContent
        {
            get { return textBoxChangeableContent; }
        }

        public List<ModifiedListBox> ListBoxesManual { get; } = new List<ModifiedListBox>();

        public event EventHandler NewClipboardText;

        public void TriggerEventOnNewClipboardText(EventArgs e)
        {
            NewClipboardText?.Invoke(this, e);
        }

        private void CreateEvents()
        {
            metroTabControl.SelectedIndexChanged += MetroTabControl_SelectedIndexChanged;
            textBoxChangeableContent.TextChanged += TextBoxChangeableContent_TextChanged;
            textBoxChangeableContent.KeyPress += TextBoxChangeableContent_KeyPress;
        }

        private void TextBoxChangeableContent_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnTextBoxChangeableContentKeyPress?.Invoke(textBoxChangeableContent, e);
        }

        private void TextBoxChangeableContent_TextChanged(object sender, EventArgs e)
        {
            OnTextBoxChangeableContentTextChanged?.Invoke(textBoxChangeableContent, e);
        }

        private void MetroTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnMetroTabControlSelectedIndexChanged?.Invoke(metroTabControl, e);
        }
    }
}