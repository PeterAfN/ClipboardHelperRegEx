using ClipboardHelperRegEx.BusinessLogic;
using ClipboardHelperRegEx.ModifiedControls;
using ClipboardHelperRegEx.Views;
using System;
using System.Windows.Forms;

namespace ClipboardHelper.Views
{
    public partial class ViewUserSettingsRightManuallyShownTabs : UserControl,
        IViewUserSettingsRightManuallyShownTabs
    {

        public ViewUserSettingsRightManuallyShownTabs()
        {
            InitializeComponent();
            ListLeft.AllowDrop = true;
            CreateEvents();
        }


        public bool Loaded { get; set; } = false;
        public ManuallyShownTabs ManuallyShownTabs { get; set; } = new ManuallyShownTabs();

        public event EventHandler TxtChanged;

        public event DragEventHandler DragDropListBox;

        public event DragEventHandler DragOverListBox;

        public event MouseEventHandler MouseDownListBox;

        public event MouseEventHandler MouseUpListbox;

        public event EventHandler AddClickMenu;

        public event EventHandler DeleteClickMenu;

        public event EventHandler EditNameClickMenu;

        public GroupBox GroupBoxRight
        {
            get { return groupBoxRight; }
        }

        public ModifiedListBox ListLeft
        {
            get { return listLeft; }
        }

        public RichTextBox TextRight
        {
            get { return textRight; }
        }

        public ContextMenuStrip RightClickMenu
        {
            get { return rightClickMenu; }
        }

        private void CreateEvents()
        {
            ListLeft.DragDrop += OnDragDropListBox;
            ListLeft.DragOver += OnDragOverListBox;
            ListLeft.MouseDown += OnMouseDownListBox;
            ListLeft.MouseUp += OnMouseUpListbox;
            add.Click += OnAddClickMenu;
            delete.Click += OnDeleteClickMenu;
            editName.Click += OnEditNameClickMenu;
            TextRight.TextChanged += OnTextChanged;
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            TxtChanged?.Invoke(this, e);
        }

        private void OnDragDropListBox(object sender, DragEventArgs e)
        {
            DragDropListBox?.Invoke(this, e);
        }

        private void OnDragOverListBox(object sender, DragEventArgs e)
        {
            DragOverListBox?.Invoke(this, e);
        }

        private void OnMouseDownListBox(object sender, MouseEventArgs e)
        {
            MouseDownListBox?.Invoke(this, e);
        }

        private void OnMouseUpListbox(object sender, MouseEventArgs e)
        {
            MouseUpListbox?.Invoke(this, e);
        }

        private void OnAddClickMenu(object sender, EventArgs e)
        {
            AddClickMenu?.Invoke(this, e);
        }

        private void OnDeleteClickMenu(object sender, EventArgs e)
        {
            DeleteClickMenu?.Invoke(this, e);
        }

        private void OnEditNameClickMenu(object sender, EventArgs e)
        {
            EditNameClickMenu?.Invoke(this, e);
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
    }
}