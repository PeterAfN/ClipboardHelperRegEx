using ClipboardHelperRegEx.ModifiedControls;
using System;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.Views
{
    public interface IViewMainSplCont
    {
        ModifiedSplitContainer SplitContainer { get; }

        event EventHandler Load;
        event SplitterEventHandler SplitterMovedSplitContainer;
    }
}