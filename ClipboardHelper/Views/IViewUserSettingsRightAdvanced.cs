using ClipboardHelperRegEx.ModifiedControls;
using System;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.Views
{
    public interface IViewUserSettingsRightAdvanced
    {
        ModifiedListBox ListBoxFiles { get; }
        ContextMenuStrip RightClickMenu { get; }
        FileDialog OpenFileDialog { get; }

        event EventHandler Load;
        event MouseEventHandler MouseUpListbox;
        event EventHandler AddClickMenu;
        event EventHandler DeleteClickMenu;
        event EventHandler ReplaceClickMenu;
    }
}