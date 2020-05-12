using System.Threading;
using System.Windows.Forms;
using ClipboardHelperRegEx.Properties;
using System.Windows.Interop;
using System;

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
                    1, 
                    100); // 100 ms delay between retries
            }
            catch (Exception e)
            {
                //MessageBox.Show(e + "\r\n\r\nClipboard Helper RegEx could not access the Clipboard. The program will try to again.");
                try
                {
                    NativeMethods.CloseClipboard();
                    System.Windows.Forms.Clipboard.SetDataObject(
                        text, // Text to store in clipboard
                        true, // Do not keep after our application exits
                        5, // Retry 5 times
                        100); // 100 ms delay between retries
                }
                catch (Exception e2)
                {
                    MessageBox.Show(e2.ToString() + "\r\n\r\nThe retry failed.");
                    //throw;
                }
            }
            finally
            {
                if (lockWasTaken) Monitor.Exit(LockerClipboardSet);
            }
        }
    }
}
