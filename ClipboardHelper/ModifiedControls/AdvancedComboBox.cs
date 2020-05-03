using System.Drawing;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.ModifiedControls
{
    //modified from https://stackoverflow.com/questions/13212179/changing-the-color-of-combobox-highlighting

    /// <summary>
    ///     Provides ability to change the selected items HighlightColor and selected items HighlightFontColor, to
    ///     non-standard.
    /// </summary>
    public class AdvancedComboBox : ComboBox
    {
        public AdvancedComboBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            HighlightColor = Color.Black;
            DrawItem += AdvancedComboBox_DrawItem;
        }

        public Color HighlightColor { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global (reSharper doesn't seem to show this correct)
        public Color HighlightFontColor { get; set; }

        private void AdvancedComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            var combo = sender as ComboBox;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                using (var colorBrush = new SolidBrush(HighlightColor))
                {
                    e.Graphics.FillRectangle(colorBrush,
                        e.Bounds);
                }
                using (var colorBrush = new SolidBrush(Color.White))
                {
                    if (combo != null)
                        e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font,
                            colorBrush,
                            new Point(e.Bounds.X, e.Bounds.Y));
                }
            }
            else
            {
                if (combo != null)
                {
                    using (var colorBrush = new SolidBrush(combo.BackColor))
                    {
                        e.Graphics.FillRectangle(colorBrush,
                            e.Bounds);
                    }
                    using (var colorBrush = new SolidBrush(combo.ForeColor))
                    {
                        e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font,
                            colorBrush,
                            new Point(e.Bounds.X, e.Bounds.Y));
                    }
                }
            }
            e.DrawFocusRectangle();
        }
    }
}