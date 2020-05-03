using ClipboardHelperRegEx.Views;
using System.Drawing;
using System.Windows.Forms;

namespace ClipboardHelper.Views
{
    public partial class ViewUserSettingsLeftMenu : UserControl, IViewUserSettingsMenuLeft
    {
        public ViewUserSettingsLeftMenu()
        {
            InitializeComponent();
            Treeview1 = new TreeView();
            Treeview1.DrawNode += OnMenuDrawNode;
            Treeview1.NodeMouseClick += OnMouseClickNode;
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

        public TreeView Treeview1 { get; }

        public event DrawTreeNodeEventHandler MenuDrawNode;

        public event TreeNodeMouseClickEventHandler MouseClickNode;

        public void InitiateControl()
        {
            Treeview1.DrawMode = TreeViewDrawMode.OwnerDrawText;
            Treeview1.HideSelection = false;
            Treeview1.BorderStyle = BorderStyle.None;
            Treeview1.Dock = DockStyle.Fill;
            Treeview1.Indent = 19;
            Treeview1.Name = "treeView";
            Treeview1.TabIndex = 1;
            Treeview1.BackColor = Color.White;
            Treeview1.ForeColor = Color.Black;
            Treeview1.Visible = true;
        }

        public void AddNodes()
        {
            var treeNode = new TreeNode("Appearance");
            Treeview1.Nodes.Add(treeNode);
            Treeview1.SelectedNode = treeNode;
            treeNode = new TreeNode("AutoShownTabs");
            Treeview1.Nodes.Add(treeNode);
            treeNode = new TreeNode("ManuallyShownTabs");
            Treeview1.Nodes.Add(treeNode);
            treeNode = new TreeNode("Advanced");
            Treeview1.Nodes.Add(treeNode);
            treeNode = new TreeNode("Help");
            Treeview1.Nodes.Add(treeNode);
        }

        public void ModifiedTreeviewDrawNode(DrawTreeNodeEventArgs e)
        {
            if (e != null && e.Node == null) return;

            // if tree view's HideSelection property is "True", 
            // this will always returns "False" on unfocused treeview
            var selected = e != null && (e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected;

            // we need to do owner drawing only on a selected node
            // and when the treeview is unfocused, else let the OS do it for us
            if (selected)
            {
                // ReSharper disable once ConstantConditionalAccessQualifier
                var font = e?.Node.NodeFont ?? e.Node.TreeView.Font;
                e.Graphics.FillRectangle(Brushes.Black, e.Bounds);
                TextRenderer.DrawText(e.Graphics, e.Node.Text, font, e.Bounds, Color.White,
                    TextFormatFlags.GlyphOverhangPadding);
            }
            else
            {
                if (e != null) e.DrawDefault = true;
            }
        }

        private void OnMenuDrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            MenuDrawNode?.Invoke(this, e);
        }

        private void OnMouseClickNode(object sender, TreeNodeMouseClickEventArgs e)
        {
            MouseClickNode?.Invoke(this, e);
        }
    }
}