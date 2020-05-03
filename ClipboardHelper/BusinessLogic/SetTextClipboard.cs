using System.Threading;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public static class SetTextClipboard
    {
        private static readonly object LockerClipboardSet = new object();

        public static void Start(string text)
        {
            SetText(text);
        }

        private static void SetText(string text)
        {
            //Thread Safe locker.
            var lockWasTaken = false;
            try
            {
                Monitor.Enter(LockerClipboardSet, ref lockWasTaken);
                System.Windows.Forms.Clipboard.SetDataObject(
                    text, // Text to store in clipboard
                    true, // Do not keep after our application exits
                    20, // Retry 20 times
                    200); // 200 ms delay between retries
            }
            //catch
            //{
            //    MessageBox.Show(Resources.SetTextClipboard_SetText_Windows_Clipboard_could_not_set_text_, Resources.SetTextClipboard_SetText_Clipboard_Helper_information,
            //        MessageBoxButtons.OK);
            //}
            finally
            {

                if (lockWasTaken) Monitor.Exit(LockerClipboardSet);
            }
        }
    }
}
