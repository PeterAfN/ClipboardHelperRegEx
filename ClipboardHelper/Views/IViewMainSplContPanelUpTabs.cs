using ClipboardHelperRegEx.BusinessLogic;
using ClipboardHelperRegEx.ModifiedControls;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.Views
{
    public interface IViewMainSplContPanelUpTabs
    {
        MetroTabControl TabControl { get; }

        GroupBox GroupBoxAuto { get; }
        Panel PanelChangeableContent { get; }
        TextBox TextBoxChangeableContent { get; }

        AutoShownTabs AutoShownTabsRam { get; set; }
        ManuallyShownTabs ManuallyShownTabsRam { get; set; }

        List<ModifiedListBox> ListBoxesManual { get; }
        event EventHandler Load;
        event EventHandler VisibleChanged;

        void TriggerEventOnNoItemsSelected(EventArgs e);
        void TriggerEventOnItemsSelected(EventArgs e);
        void TriggerEventOnNewClipboardText(EventArgs e);

        event EventHandler NoItemsSelected;
        event EventHandler ItemsSelected;
        event EventHandler NewClipboardText;
        event EventHandler OnMetroTabControlSelectedIndexChanged;
        event EventHandler OnTextBoxChangeableContentTextChanged;
        event KeyPressEventHandler OnTextBoxChangeableContentKeyPress;
    }
}