using ClipboardHelperRegEx.ModifiedControls;
using ClipboardHelperRegEx.Views;
using System.Windows.Forms;

namespace ClipboardHelper.Views
{
    public partial class ViewMainSplCont : UserControl, IViewMainSplCont
    {

        public ViewMainSplCont()
        {
            InitializeComponent();
            CreateEvents();
        }

        public event SplitterEventHandler SplitterMovedSplitContainer;

        public ModifiedSplitContainer SplitContainer { get; private set; }

        private void CreateEvents()
        {
            SplitContainer.SplitterMoved += OnSplitterMovedSplitContainer;
        }

        private void OnSplitterMovedSplitContainer(object sender, SplitterEventArgs e)
        {
            SplitterMovedSplitContainer?.Invoke(this, e);
        }
    }
}