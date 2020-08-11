using System;
using System.Diagnostics;
using WindowsInput;
using WindowsInput.Native;

namespace ClipboardHelperRegEx.BusinessLogic
{
    internal class TagOutlook
    {
        private const int WmSettext = 0x000c;

        private const int SwShowMinNoActive = 7;
        private const int SwShowMinimized = 2;
        private const int SwShowMaximized = 3;
        private const int SwRestore = 9;

        private readonly InputSimulator _s = new InputSimulator();

        internal TagOutlook(string searchData)
        {
            SearchData = searchData;
            Search();
        }

        private string SearchData { get; }

        /// <summary>
        ///     Returns true if Outlook already is open. Returns false if not open.
        ///     was already open
        /// </summary>
        /// <returns></returns>
        private static bool IsOutlookOpen()
        {
            var procCount = 0;
            var processlist = Process.GetProcessesByName("OUTLOOK");
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var unused in processlist) procCount++;
            return procCount > 0;
        }

        /// <summary>
        ///     Checks if other window is minimized.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        private static bool IsMinimized(IntPtr hWnd)
        {
            return NativeMethods.IsIconic(hWnd);
        }

        /// <summary>
        ///     Checks if other window is maximized.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        private static bool IsMaximized(IntPtr hWnd)
        {
            return NativeMethods.IsZoomed(hWnd);
        }

        private static void MinimizeWindow(IntPtr handle)
        {
            NativeMethods.ShowWindow(handle, SwShowMinNoActive);
        }

        private void Search()
        {
            if (!IsOutlookOpen()) return;
            //download and use a program named IuSpy to find the controls names
            var hWnd = NativeMethods.FindWindow("rctrl_renwnd32", null);
            if (hWnd.Equals(IntPtr.Zero)) return;

            var hWndChild1 = NativeMethods.FindWindowEx(hWnd, IntPtr.Zero, "MsoCommandBarDock", "MsoDockTop");
            if (hWndChild1.Equals(IntPtr.Zero)) return;

            var hWndChild1A =
                NativeMethods.FindWindowEx(hWndChild1, IntPtr.Zero, "MsoCommandBar", "Ribbon");
            if (hWndChild1A.Equals(IntPtr.Zero)) return;

            var hWndChild1B =
                NativeMethods.FindWindowEx(hWndChild1A, IntPtr.Zero, "MsoWorkPane", "Ribbon");
            if (hWndChild1B.Equals(IntPtr.Zero)) return;

            var hWndChild2 =
                NativeMethods.FindWindowEx(hWndChild1B, IntPtr.Zero, "NUIPane", "");
            if (hWndChild2.Equals(IntPtr.Zero)) return;

            var hWndChild3 = NativeMethods.FindWindowEx(hWndChild2, IntPtr.Zero, "NetUIHWND", "");
            if (hWndChild3.Equals(IntPtr.Zero)) return;

            var hWndChild4 = NativeMethods.FindWindowEx(hWndChild3, IntPtr.Zero, "NetUICtrlNotifySink", "");
            if (hWndChild4.Equals(IntPtr.Zero)) return;

            var hWndChild5 = NativeMethods.FindWindowEx(hWndChild4, IntPtr.Zero, "RICHEDIT60W", "");
            if (hWndChild5.Equals(IntPtr.Zero)) return;

            //https://stackoverflow.com/questions/6228089/how-do-i-bring-an-unmanaged-application-window-to-front-and-make-it-the-active
            MinimizeWindow(hWnd);
            NativeMethods.ShowWindowAsync(hWnd, SwShowMinimized);

            if (IsMaximized(hWnd)) NativeMethods.ShowWindowAsync(hWnd, SwShowMaximized);
            else if (IsMinimized(hWnd)) NativeMethods.ShowWindowAsync(hWnd, SwRestore);
            NativeMethods.SetForegroundWindow(hWnd);
            // ReSharper disable once AssignmentIsFullyDiscarded
            _ = NativeMethods.SendMessage(hWndChild5, WmSettext, 0, SearchData + "\n");
            _s?.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LCONTROL,
                VirtualKeyCode.VK_E); //Ctrl+E focuses search box in outlook.
            _s?.Keyboard.KeyPress(VirtualKeyCode.RETURN);
        }
    }
}