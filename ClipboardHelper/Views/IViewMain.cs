using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.Views
{
    public interface IViewMain
    {
        Point Location { get; set; }
        Size Size { get; set; }
        bool Visible { get; }
        bool FormGetsFocus { get; set; }
        FormStartPosition StartPosition { get; set; }
        bool Resizing { get; }
        Size MinimumSize { get; set; }
        int Height { get; }
        bool RefreshAll { get; set; }
        Label LabelTitleTop { get; }
        void Activate();
        void ShowInactiveTopmost(Form frm);
        void Show();
        void ShowNotifyIconProgram(bool show);
        void SetImageFormIcon1(Image icon);
        void SetImageFormIcon2(Image icon);
        void SetImageFormIcon3(Image icon);
        void InitiateControls();
        void AddControls();

        event EventHandler Load;
        event EventHandler ResizingEnd;
        event EventHandler MouseEntersFormIcon1;
        event EventHandler MouseLeavesFormIcon1;
        event EventHandler MouseClicksFormIcon1;
        event EventHandler MouseEntersFormIcon2;
        event EventHandler MouseLeavesFormIcon2;
        event EventHandler MouseClicksFormIcon2;
        event EventHandler MouseEntersFormIcon3;
        event EventHandler MouseLeavesFormIcon3;
        event EventHandler MouseClicksFormIcon3;
        event EventHandler ClickToolStripMenuItemSettings;
        event EventHandler ClickToolStripMenuItemVisa;
        event EventHandler ClickToolStripMenuItemOm;
        event EventHandler ClickToolStripMenuItemEnd;
        event EventHandler ClickToolStripMenuItemDeActivate;
        event MouseEventHandler MouseUpNotifyIconProgram;
        event EventHandler FormLostFocusToOutsideApp;

        void SetNotifyIconImage(Icon icon);
        void SetNotifyIconVisible(bool visible);
        void ShowContextMenu(Point location);
        void SetTextMenuDeActivate(string text);
    }
}