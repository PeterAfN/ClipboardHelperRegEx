using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.Views
{
    public interface IViewMin
    {
        Point Location { get; set; }
        Size Size { get; set; }
        Label LabelTitleTop { get; }
        FormStartPosition StartPosition { get; set; }
        bool FormResizable { get; set; }
        bool FormMovable { get; set; }
        bool SnapToScreenEdge { get; set; }
        void Show();
        void Hide();
        void SetImageFormIcon3(Image icon);
        void VisibilityFormIcon1(bool show);
        void VisibilityFormIcon2(bool show);
        event EventHandler MouseEntersFormIcon3;
        event EventHandler MouseLeavesFormIcon3;
        event EventHandler MouseClicksFormIcon3;
        event EventHandler VisibleChanged;
        event EventHandler MouseEnterLabelTitleTop;
        event EventHandler OnViewMinFormLostFocusToOutsideApp;
    }
}