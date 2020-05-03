using System;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.Views
{
    public interface IViewMainSplContPanelDown
    {
        PictureBox HistoryLeft { get; }

        PictureBox HistoryRight { get; }

        RichTextBox Clipboard { get; }

        Label Position { get; }

        string PositionText { get; set; }
        event EventHandler Load;

        event EventHandler HistoryLeftMouseEnter;

        event EventHandler HistoryLeftMouseLeave;

        event EventHandler HistoryLeftMouseClick;

        event EventHandler HistoryRightMouseEnter;

        event EventHandler HistoryRightMouseLeave;

        event EventHandler HistoryRightMouseClick;
    }
}