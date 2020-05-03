using ClipboardHelperRegEx.Views;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.BusinessLogic.Presenters
{
    public class PresenterMainSplCont
    {
        private readonly IViewMainSplCont _view;
        private readonly IViewMain _viewMain;

        public PresenterMainSplCont(IViewMainSplCont view, IViewMain viewMain)
        {
            _view = view;
            _viewMain = viewMain;

            //subscribe to events
            if (view != null) view.Load += View_Load;
        }

        private void View_Load(object sender, EventArgs e)
        {
            _view.SplitterMovedSplitContainer += SplitContainer_SplitterMoved;
            _viewMain.MinimumSize = MinimumHeightUpdate();
        }

        private void SplitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (!_viewMain.Resizing)
                _viewMain.MinimumSize = MinimumHeightUpdate();
        }

        /// <summary>
        ///     Updates the forms minimum height, so the lower Spl.Con.
        ///     panel can't be hidden when decreasing the form size.
        /// </summary>
        /// <returns></returns>
        private Size MinimumHeightUpdate()
        {
            var result = _viewMain.Height -
                         (_view.SplitContainer.Panel1.ClientSize.Height - _view.SplitContainer.Panel1MinSize);
            if(result > 0)
                return new Size(_viewMain.MinimumSize.Width, _viewMain.Height -
                                                         (_view.SplitContainer.Panel1.ClientSize.Height -
                                                          _view.SplitContainer.Panel1MinSize));
            else
            {
                return new Size(_viewMain.MinimumSize.Width, 0);
            }
        }
    }
}