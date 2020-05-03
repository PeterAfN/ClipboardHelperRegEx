using ClipboardHelperRegEx.Properties;
using ClipboardHelperRegEx.Views;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.BusinessLogic.Presenters
{
    public sealed class PresenterMin : IDisposable
    {
        private readonly Clipboard _clipboard;
        private readonly IResourcesService _resources;
        private readonly IViewMain _viewMain;
        private readonly IViewMin _viewMin;

        public PresenterMin(
            IViewMin viewMin,
            IViewMain viewMain,
            IResourcesService resources)
        {
            _viewMin = viewMin;
            _viewMain = viewMain;
            _resources = resources;

            //set initial form settings
            if (_viewMin != null)
            {
                _viewMin.VisibilityFormIcon1(false);
                _viewMin.VisibilityFormIcon2(false);
                _viewMin.StartPosition = FormStartPosition.Manual;
                if (_resources != null) _viewMin.SetImageFormIcon3(_resources.Closed);
                _viewMin.FormResizable = false;
                _viewMin.FormMovable = false;
                _viewMin.SnapToScreenEdge = false;

                //subscribe to events
                _viewMin.VisibleChanged += OnVisibleChanged;
                _viewMin.MouseEntersFormIcon3 += OnMouseEntersFormIcon3;
                _viewMin.MouseLeavesFormIcon3 += OnMouseLeavesFormIcon3;
                _viewMin.MouseClicksFormIcon3 += OnMouseClicksFormIcon3;
                _viewMin.MouseEnterLabelTitleTop += OnMouseEnterLabelTitleTop;
            }

            _clipboard = new Clipboard();
            Clipboard.Changed += Clipboard_Changed;
        }

        private Screen ActiveMonitor { get; set; } = Screen.PrimaryScreen;

        private void Clipboard_Changed(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(Clipboard.Text))
                _viewMin.LabelTitleTop.Text =
                    Resources.PresenterMin_Clipboard_Changed_In_Clipboard_now__ + Clipboard.Text;
        }

        private void OnMouseEnterLabelTitleTop(object sender, EventArgs args)
        {
            _viewMain.Show();
            _viewMain.RefreshAll = true;
            _viewMin.Hide();
        }

        private void OnVisibleChanged(object sender, EventArgs e)
        {
            _viewMin.Size = new Size(_viewMain.Size.Width, 16);
            ActiveMonitor = FindCurrentMonitor(_viewMain);
            _viewMin.Location = new Point(ActiveMonitor.WorkingArea.Right - _viewMain.Size.Width,
                ActiveMonitor.WorkingArea.Bottom - _viewMin.Size.Height);
        }

        private void OnMouseEntersFormIcon3(object sender, EventArgs e)
        {
            _viewMin.SetImageFormIcon3(_resources.ClosedSelected);
        }

        private void OnMouseLeavesFormIcon3(object sender, EventArgs e)
        {
            _viewMin.SetImageFormIcon3(_resources.Closed);
        }

        private void OnMouseClicksFormIcon3(object sender, EventArgs e)
        {
            _viewMin.Hide();
        }

        private static Screen FindCurrentMonitor(IViewMain form)
        {
            return Screen.FromRectangle(new Rectangle(form.Location, form.Size));
        }

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        private void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing) _clipboard.Dispose();
            _disposedValue = true;
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion
    }
}