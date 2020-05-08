using System;
using System.Runtime.InteropServices;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public static class NativeMethods
    {
        /// <summary>
        ///     Sent when the contents of the clipboard have changed.
        /// </summary>
        internal const int WmClipboardUpdate = 0x031D;

        /// <summary>
        ///     To find message-only windows, specify HWND_MESSAGE in the hwndParent parameter of the FindWindowEx function.
        /// </summary>
        internal static IntPtr HwndMessage = new IntPtr(-3);

        ////https://codereview.stackexchange.com/questions/115417/monitoring-the-clipboard
        /// <summary>
        ///     Places the given window in the system-maintained clipboard format listener list.
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool AddClipboardFormatListener(IntPtr hwnd);

        /// <summary>
        ///     Removes the given window from the system-maintained clipboard format listener list.
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool RemoveClipboardFormatListener(IntPtr hwnd);


        #region Win32

        //https://stackoverflow.com/questions/33003958/getting-byte-from-getclipboarddata-native-method

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsClipboardFormatAvailable(uint format);

        [DllImport("User32.dll", SetLastError = true)]
        internal static extern IntPtr GetClipboardData(uint uFormat);

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseClipboard();

        [DllImport("Kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("Kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport("Kernel32.dll", SetLastError = true)]
        internal static extern int GlobalSize(IntPtr hMem);

        internal const uint CfUnicodeText = 13U;

        #endregion


        #region TagOutlook

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        // Get a handle to an application window.
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        //Send strings to other windows.
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        internal static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

        //Finds applications child windows
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass,
            string lpszWindow);

        //Activate an application window.
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool ShowWindowAsync(IntPtr windowHandle, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool SetForegroundWindow(IntPtr windowHandle);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern bool IsZoomed(IntPtr hWnd);

        #endregion


        #region Pasting - get foreground window

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr GetForegroundWindow();

        // The GetWindowThreadProcessId function retrieves the identifier of the thread
        // that created the specified window and, optionally, the identifier of the
        // process that created the window.
        [DllImport("user32.dll")]
        internal static extern int GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        #endregion
    }
}