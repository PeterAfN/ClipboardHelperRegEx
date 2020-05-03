using System.Windows.Forms;

namespace ClipboardHelperRegEx.Views
{
    public interface IViewUserSettingsMenuLeft
    {
        Control.ControlCollection Controls { get; }
        TreeView Treeview1 { get; }
        event DrawTreeNodeEventHandler MenuDrawNode;
        event TreeNodeMouseClickEventHandler MouseClickNode;
        void InitiateControl();
        void AddNodes();
        void ModifiedTreeviewDrawNode(DrawTreeNodeEventArgs e);
    }
}