using System.Reflection;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.ModifiedControls
{
    public class ModifiedSplitContainer : SplitContainer
    {
        public ModifiedSplitContainer()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            var objMethodInfo = typeof(Control).GetMethod(
                "SetStyle", BindingFlags.NonPublic | BindingFlags.Instance);

            var objArgs = new object[]
            {
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true
            };

            if (objMethodInfo == null) return;
            objMethodInfo.Invoke(Panel1, objArgs);
            objMethodInfo.Invoke(Panel2, objArgs);
        }
    }
}