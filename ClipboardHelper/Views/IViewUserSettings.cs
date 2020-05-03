using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.Views
{
    public interface IViewUserSettings
    {
        Size Size { get; set; }
        FormStartPosition StartPosition { get; set; }
        bool Enabled { get; set; }
        void VisibilityFormIcon2(bool show);
        void VisibilityFormIcon3(bool show);
        void SetImageFormIcon1(Image icon);
        event EventHandler Load;
        event EventHandler MouseEntersFormIcon1;
        event EventHandler MouseLeavesFormIcon1;
        event EventHandler MouseClicksFormIcon1;
        void Show();
        void Hide();
        void InitiateControls();
        void AddControls();
        void ChangeRightView(UserControl activeView);
        void SetGroupBoxText(string text);
    }
}