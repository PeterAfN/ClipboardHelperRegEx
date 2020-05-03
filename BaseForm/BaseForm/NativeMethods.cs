using System;
using System.Runtime.InteropServices;

namespace Template
{
    public static class NativeMethods
    {
        // struct for box shadow
        public struct Margins
        {
            // ReSharper disable once NotAccessedField.Global
            public int LeftWidth;
            // ReSharper disable once NotAccessedField.Global
            public int RightWidth;
            // ReSharper disable once NotAccessedField.Global
            public int TopHeight;
            // ReSharper disable once NotAccessedField.Global
            public int BottomHeight;
        }

        [DllImport("dwmapi.dll")]
        internal static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMarInset);

        [DllImport("dwmapi.dll")]
        internal static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        internal static extern bool SetWindowPos(
         int hWnd,             // Window handle
         int hWndInsertAfter,  // Placement-order handle
         int x,                // Horizontal position
         int y,                // Vertical position
         int cx,               // Width
         int cy,               // Height
         uint uFlags);         // Window positioning flags

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    }
}
