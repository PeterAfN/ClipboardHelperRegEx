using ClipboardHelperRegEx.BusinessLogic;
using ClipboardHelperRegEx.ModifiedControls;
using System;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.Views
{
    public interface IViewUserSettingsRightManuallyShownTabs
    {
        bool Loaded { get; set; }
        ManuallyShownTabs ManuallyShownTabs { get; set; }

        Cursor Cursor { get; set; }
        GroupBox GroupBoxRight { get; }

        ModifiedListBox ListLeft { get; }
        RichTextBox TextRight { get; }
        ContextMenuStrip RightClickMenu { get; }

        event EventHandler Load;
        event DragEventHandler DragDropListBox;
        event DragEventHandler DragOverListBox;
        event MouseEventHandler MouseDownListBox;
        event MouseEventHandler MouseUpListbox;
        event EventHandler TxtChanged;
        event EventHandler AddClickMenu;
        event EventHandler DeleteClickMenu;
        event EventHandler EditNameClickMenu;
    }
}