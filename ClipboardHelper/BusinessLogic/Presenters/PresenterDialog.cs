using ClipboardHelperRegEx.Views;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.BusinessLogic.Presenters
{
    public class PresenterDialog
    {
        private readonly IViewDialog _viewDialog;

        public PresenterDialog(IViewDialog viewDialog)
        {
            _viewDialog = viewDialog;

            //set initial form settings
            if (_viewDialog == null) return;
            _viewDialog.Size = _viewDialog.MinimumSize;
            _viewDialog.PanelTitle.Hide();
            _viewDialog.PanelSeparatorLine.Hide();

            //subscribe to events
            _viewDialog.ClickOkMouseButton += OnClickOkMouseButton;
            _viewDialog.ClickCancelMouseButton += OnClickCancelMouseButton;
            _viewDialog.VisibleChanged += ViewDialog_VisibleChanged;
            _viewDialog.FormResizable = false;
            _viewDialog.FormMovable = false;
        }

        private void ViewDialog_VisibleChanged(object sender, EventArgs e)
        {
            var tb = (Form)sender;
            if (tb.Visible)
            {
                _viewDialog.BringToFront();
                _viewDialog.Location = new Point(Cursor.Position.X - 20, Cursor.Position.Y - 70);
                _viewDialog.UserInput.Text = "";
            }

            _viewDialog.UserInput.Focus();
        }

        private void OnClickCancelMouseButton(object sender, EventArgs args)
        {
            _viewDialog.Hide();
        }

        private void OnClickOkMouseButton(object sender, EventArgs args)
        {
            _viewDialog.Hide();
        }
    }
}