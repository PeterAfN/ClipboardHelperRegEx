using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.Views
{
    public interface IViewAbout
    {
        Size Size { get; set; }
        FormStartPosition StartPosition { get; set; }
        bool FormResizable { get; set; }
        void Show();
        void Hide();
        void SetImageFormIcon1(Image icon);
        void VisibilityFormIcon2(bool show);
        void VisibilityFormIcon3(bool show);
        void SetProduct(string product);
        void SetVersion(string version);
        void SetCopyright(string copyright);
        void SetCompany(string company);
        void SetDescription(string description);

        event EventHandler Load;
        event EventHandler MouseEntersFormIcon1;
        event EventHandler MouseLeavesFormIcon1;
        event EventHandler MouseClicksFormIcon1;
        event EventHandler ClickedOkButton;
    }
}