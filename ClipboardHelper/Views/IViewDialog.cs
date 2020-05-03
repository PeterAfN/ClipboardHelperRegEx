using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.Views
{
    public interface IViewDialog
    {
        Size Size { get; set; }
        Size MinimumSize { get; }
        Point Location { get; set; }
        Panel PanelTitle { get; }
        Panel PanelSeparatorLine { get; }
        bool FormResizable { get; set; }
        bool FormMovable { get; set; }
        bool Visible { get; }
        object Tag { get; set; }
        TextBox UserInput { get; }
        void Show();
        void Hide();
        void BringToFront();
        void SetText(string text);
        event EventHandler ClickOkMouseButton;
        event EventHandler ClickCancelMouseButton;
        event EventHandler VisibleChanged;
    }
}