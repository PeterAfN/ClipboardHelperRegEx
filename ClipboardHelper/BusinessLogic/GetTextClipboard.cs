using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public static class GetTextClipboard
    {
        public static string Start()
        {
            if (!NativeMethods.IsClipboardFormatAvailable(NativeMethods.CfUnicodeText))
                return null;

            try
            {
                if (!NativeMethods.OpenClipboard(IntPtr.Zero))
                    return null;

                var handle = NativeMethods.GetClipboardData(NativeMethods.CfUnicodeText);
                if (handle == IntPtr.Zero)
                    return null;

                var pointer = IntPtr.Zero;

                try
                {
                    pointer = NativeMethods.GlobalLock(handle);
                    if (pointer == IntPtr.Zero)
                        return null;

                    var size = NativeMethods.GlobalSize(handle);
                    var buff = new byte[size];

                    Marshal.Copy(pointer, buff, 0, size);

                    return Encoding.Unicode.GetString(buff).TrimEnd('\0');
                }
                finally
                {
                    if (pointer != IntPtr.Zero)
                        NativeMethods.GlobalUnlock(handle);
                }
            }
            finally
            {
                NativeMethods.CloseClipboard();
            }
        }
    }
}