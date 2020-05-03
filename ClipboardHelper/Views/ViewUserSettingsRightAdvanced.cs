using ClipboardHelperRegEx.ModifiedControls;
using ClipboardHelperRegEx.Views;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using System.Windows.Input;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using MouseEventHandler = System.Windows.Forms.MouseEventHandler;

namespace ClipboardHelper.Views
{
    [SuppressMessage("ReSharper", "ConvertToAutoPropertyWhenPossible")]
    public partial class ViewUserSettingsRightAdvanced : UserControl, IViewUserSettingsRightAdvanced
    {
        public ViewUserSettingsRightAdvanced()
        {
            InitializeComponent();
            CreateEvents();
        }

        private void CreateEvents()
        {
            listBoxFiles.MouseUp += ListBoxFiles_MouseUp;
            add.Click += Add_Click;
            delete.Click += Delete_Click;
            replace.Click += Replace_Click;
        }

        public event MouseEventHandler MouseUpListbox;
        public event EventHandler AddClickMenu;
        public event EventHandler DeleteClickMenu;
        public event EventHandler ReplaceClickMenu;

        public ModifiedListBox ListBoxFiles
        {
            get { return listBoxFiles; }
        }

        public ContextMenuStrip RightClickMenu
        {
            get { return rightClickMenu; }
        }

        public FileDialog OpenFileDialog
        {
            get { return openFileDialog; }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            DeleteClickMenu?.Invoke(delete, e);
        }

        private void Add_Click(object sender, EventArgs e)
        {
            AddClickMenu?.Invoke(add, e);
        }

        private void Replace_Click(object sender, EventArgs e)
        {
            ReplaceClickMenu?.Invoke(add, e);
        }

        private void ListBoxFiles_MouseUp(object sender, MouseEventArgs e)
        {
            MouseUpListbox?.Invoke(typeof(MouseDevice), e);
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