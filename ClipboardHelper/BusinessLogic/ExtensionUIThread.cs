using System;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.BusinessLogic
{
    //used unmodified from https://www.codeproject.com/Articles/37642/Avoiding-InvokeRequired
    public static class ExtensionUiThread
    {
        public static void UiThread(this Control control, Action code) // this is preferred, use this.
        {
            if (control != null && control.InvokeRequired)
            {
                control.BeginInvoke(code);
                return;
            }

            code?.Invoke();
        }
    }
}