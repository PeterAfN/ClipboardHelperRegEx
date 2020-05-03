using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Interop;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public sealed class Clipboard : IDisposable
    {
        public delegate void DgEventRaiserEventHandler(object sender, EventArgs e);

        /// <summary>
        ///     Text in Windows Clipboard
        /// </summary>
        private static string _text;

        private static readonly object Locker = new object();

        // Instantiate a SafeHandle instance.
        private readonly SafeHandle _handle = new SafeFileHandle(IntPtr.Zero, true);
        private readonly HwndSource _hwndSource = new HwndSource(0, 0, 0, 0, 0, 0, 0, null, NativeMethods.HwndMessage);

        // Flag: Has Dispose already been called?
        private bool _disposed;

        public Clipboard()
        {
            ClipboardMonitor();
        }

        public static string Text
        {
            get { return _text; }
            private set { _text = value?.Trim(); }
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        ///     Subscribe to action when new text value in the Clipboard has changed.
        /// </summary>
        public static event DgEventRaiserEventHandler Changed;

        /// <summary>
        ///     Receives events and in this case we look for text changes in Windows Clipboard
        /// </summary>
        private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg != NativeMethods.WmClipboardUpdate) return IntPtr.Zero;
            //Thread Safe locker, because WndProc reports same clipboard value twice.
            var lockWasTaken = false;
            try
            {
                Monitor.Enter(Locker, ref lockWasTaken);
                var oldText = Text;
                Text = GetTextClipboard.Start();
                if (Text != oldText) Changed?.Invoke(hwnd, EventArgs.Empty);
                handled = true;
            }
            finally
            {
                if (lockWasTaken) Monitor.Exit(Locker);
            }

            return IntPtr.Zero;
        }


        /// <summary>
        ///     Start listening for changes in the Windows Clipboard
        /// </summary>
        private void ClipboardMonitor()
        {
            _hwndSource.AddHook(WndProc);
            NativeMethods.AddClipboardFormatListener(_hwndSource.Handle);
        }

        // Protected implementation of Dispose pattern.
        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _handle.Dispose();
                NativeMethods.RemoveClipboardFormatListener(_hwndSource.Handle);
                _hwndSource.RemoveHook(WndProc);
                try
                {
                    _hwndSource?.Dispose();
                }
                catch (Exception)
                {
                    //if here, already disposed
                }
            }
            _disposed = true;
        }
    }
}