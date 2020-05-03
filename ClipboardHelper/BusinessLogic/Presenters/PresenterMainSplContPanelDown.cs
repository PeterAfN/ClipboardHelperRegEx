using ClipboardHelperRegEx.Properties;
using ClipboardHelperRegEx.Views;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ClipboardHelperRegEx.BusinessLogic.Presenters
{
    public sealed class PresenterMainSplContPanelDown : IDisposable
    {
        private readonly IViewMainSplContPanelDown _view;
        private Clipboard _clipboard;

        public PresenterMainSplContPanelDown(IViewMainSplContPanelDown view)
        {
            _view = view;
            //subscribe to events
            if (view != null) view.Load += View_Load;
        }

        private static void View_HistoryRightMouseClick(object sender, EventArgs e)
        {
        }


        private static void View_HistoryLeftMouseClick(object sender, EventArgs e)
        {
        }

        private void View_HistoryRightMouseLeave(object sender, EventArgs e)
        {
            _view.HistoryRight.BackgroundImage = Resources.RightArrow;
        }

        private void View_HistoryRightMouseEnter(object sender, EventArgs e)
        {
            _view.HistoryRight.BackgroundImage = Resources.RightArrowSelected;
        }

        private void View_HistoryLeftMouseLeave(object sender, EventArgs e)
        {
            _view.HistoryLeft.BackgroundImage = Resources.LeftArrow;
        }

        private void View_HistoryLeftMouseEnter(object sender, EventArgs e)
        {
            _view.HistoryLeft.BackgroundImage = Resources.LeftArrowSelected;
        }

        private void View_Load(object sender, EventArgs e)
        {
            _clipboard = new Clipboard();
            Clipboard.Changed += Clipboard_Changed;

            _view.HistoryLeftMouseEnter += View_HistoryLeftMouseEnter;
            _view.HistoryLeftMouseLeave += View_HistoryLeftMouseLeave;
            _view.HistoryLeftMouseClick += View_HistoryLeftMouseClick;
            _view.HistoryRightMouseEnter += View_HistoryRightMouseEnter;
            _view.HistoryRightMouseLeave += View_HistoryRightMouseLeave;
            _view.HistoryRightMouseClick += View_HistoryRightMouseClick;
        }

        private void Clipboard_Changed(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(Clipboard.Text) && !string.IsNullOrEmpty(Clipboard.Text))
                _view.Clipboard.Text = Clipboard.Text;
        }

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        //The CA2213 warning is suppressed since Microsoft has acknowledged that this isn't an error, but a known fault with FxCop:
        //https://stackoverflow.com/questions/36229230/ca2213-warning-when-using-null-conditional-operator-to-call-dispose/36229431
        //https://github.com/dotnet/roslyn-analyzers/issues/695
        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_clipboard")]
        private void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing) _clipboard?.Dispose();
            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}