using ClipboardHelperRegEx.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.ModifiedControls
{
    /// <summary>
    ///     Provides ability to change the selected items HighlightColor and selected items HighlightFontColor, to
    ///     non-standard.
    /// </summary>
    public class ModifiedListBox : ListBox
    {
        private readonly Dictionary<string, Color> _whichColorShouldTagsHave = new Dictionary<string, Color>();
        private bool _isItemSelected;
        private string _line;

        private ListBox _listbox;
        private List<Match> _matches;

        /// <summary>
        ///     Paints listbox text with partially different colors. The string which
        ///     should be colored with different text must be within parenthesis:
        ///     name(textWithColor).
        ///     Change in the dictionary the name and which color the name should have
        /// </summary>
        private ModifiedListBox(Dictionary<string, Color> whichColorShouldTagsHave = null)
        {
            base.DrawMode = DrawMode.OwnerDrawFixed;
            SelectionColor = Color.DeepSkyBlue;
            TextColor = Color.Black;
            SelectionTextColor = Color.White;
            DrawItem += ModifiedListbox_DrawItem;
            if (whichColorShouldTagsHave != null)
            {
                _whichColorShouldTagsHave = whichColorShouldTagsHave;
            }
            else
            {
                _whichColorShouldTagsHave.Add("Caption", Settings.Default.appearanceColorTitle);
                _whichColorShouldTagsHave.Add("Info", Settings.Default.appearanceColorInfo);
                _whichColorShouldTagsHave.Add("WebUrlGoTo", Settings.Default.appearanceColorWebUrlGoTo);
                _whichColorShouldTagsHave.Add("OutlookSearch", Settings.Default.appearanceColorOutlookSearch);
                _whichColorShouldTagsHave.Add("Regex", Color.Black);
                _whichColorShouldTagsHave.Add("RegexReplace", Color.Black);
                _whichColorShouldTagsHave.Add("NotSelectableLine", Color.Red);
                _whichColorShouldTagsHave.Add("Password", Color.Silver);
                _whichColorShouldTagsHave.Add("Clipboard", Color.Black);
                _whichColorShouldTagsHave.Add("ChangeableContent", Color.Orange);
                _whichColorShouldTagsHave.Add("RegexCsvFileGet", Color.Black);
                _whichColorShouldTagsHave.Add("Encrypt", Color.Black);
                _whichColorShouldTagsHave.Add("WebGetJason", Color.Black);
                _whichColorShouldTagsHave.Add("FromUnixTimeMilliSecondsToUTC", Color.Black);
                _whichColorShouldTagsHave.Add("ConsoleOpen", Color.White);
                _whichColorShouldTagsHave.Add("NewLine", Color.Black);
            }
        }

        //[Obsolete("This constructor only exists for the benefit of the designer...")]
        public ModifiedListBox() : this(null)
        {
        }

        public Color SelectionColor { get; set; }
        public Color SelectionTextColor { get; set; }
        public Color TextColor { get; set; }

        private void ModifiedListbox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            _listbox = sender as ListBox;
            try
            {
                if (e.Index > -1) _line = _listbox?.Items[e.Index].ToString();
            }
            catch (Exception)
            {
                _line = ""; //this is only a fix for the design view, unclear why it gives an error.
                //throw;
            }

            _isItemSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            if (_listbox != null && e.Index >= 0 && e.Index < _listbox.Items.Count)
            {
                var g = e.Graphics;
                // Background Color
                var backgroundColorBrush = new SolidBrush(_isItemSelected ? SelectionColor : _listbox.BackColor);
                if (!string.IsNullOrEmpty(_line))
                {
                    g.FillRectangle(backgroundColorBrush, e.Bounds);
                    var regexAll = string.Empty;
                    foreach (var name in _whichColorShouldTagsHave.Keys) regexAll += name + @"\([\s\S]*?\)" + @"|";
                    regexAll = regexAll.Remove(regexAll.Length - 1);
                    _matches = Regex.Matches(_line ?? throw new InvalidOperationException(), regexAll).Cast<Match>()
                        .ToList();

                    if ((_line.Contains("NotSelectableLine") || _line.Contains("WebUrlGoTo") ||
                         _line.Contains("OutlookSearch") || _line.Contains("ConsoleOpen")) && _isItemSelected)
                    {
                        backgroundColorBrush = new SolidBrush(_listbox.BackColor);
                        g.FillRectangle(backgroundColorBrush, e.Bounds);
                    }
                    else
                    {
                        if (_matches.Count == 0)
                        {
                            backgroundColorBrush = new SolidBrush(_isItemSelected ? SelectionTextColor : TextColor);
                            g.DrawString(_line, e.Font, backgroundColorBrush, _listbox.GetItemRectangle(e.Index).Location);
                        }
                    }

                    HandleMatches(sender, e);
                }
                else if (_isItemSelected)
                {
                    backgroundColorBrush = new SolidBrush(SelectionColor);
                    g.FillRectangle(backgroundColorBrush, e.Bounds);
                }
                backgroundColorBrush.Dispose();
            }

            e.DrawFocusRectangle();
        }

        private void HandleMatches(object sender, DrawItemEventArgs e)
        {
            _listbox = sender as ListBox;
            var output = string.Empty;

            for (var i = 0; i < _matches.Count; i++)
            {
                Color color;
                if (
                    _isItemSelected
                    && !(_line.Contains("NotSelectableLine") || _line.Contains("WebUrlGoTo") ||
                         _line.Contains("OutlookSearch") || _line.Contains("ConsoleOpen")))
                    color = SelectionTextColor;
                else color = TextColor;

                //Line beginning - only before first match 
                string match;
                if (i == 0 && _matches[i].Index > 0)
                {
                    match = _line.Substring(0, _matches[i].Index);
                    DrawForegroundCustom(sender, e, color, output, match);
                    output += match;
                }

                //Match 
                match = _matches[i].ToString().Split('(', ')')[1]; //removes tag
                DrawForegroundCustom(sender, e, GetColorForMatch(_matches[i].ToString()), output, match);
                output += match;

                //Between Matches
                var start = _matches[i].Index + _matches[i].Length;
                if (i + 1 < _matches.Count)
                {
                    var stringLength = _matches[i + 1].Index - start;
                    match = _line.Substring(start, stringLength);
                }
                else //Line end - only after last, finishing, match 
                {
                    var length = _line.Length;
                    match = start >= length ? string.Empty : _line.Substring(start, length - /* 1 -*/ start);
                }

                DrawForegroundCustom(sender, e, /*TextColor*/color, output, match);
                output += match;
            }
        }

        private Color GetColorForMatch(string match)
        {
            var index = match.IndexOf('('); //get the match name before character '('
            var matchPartial = match.Substring(0, index);
            if (_isItemSelected
                && !(_line.Contains("NotSelectableLine") || _line.Contains("WebUrlGoTo") ||
                     _line.Contains("OutlookSearch") || _line.Contains("NewLine") || _line.Contains("ConsoleOpen")) && matchPartial != "Info") return SelectionTextColor;
            if (index >= 0)
                if (_whichColorShouldTagsHave.TryGetValue(matchPartial, out var color))
                    return color;
            return Color.Empty;
        }

        private void DrawForegroundCustom(object sender, DrawItemEventArgs e,
            Color color, string textBeforeDraw, string textToDraw)
        {
            _listbox = sender as ListBox;
            if (string.IsNullOrEmpty(textToDraw)) return;
            using (var nameTextColorBrush = new SolidBrush(color))
            {
                var g = e.Graphics;
                if (_listbox != null)
                    g.DrawString(
                        textToDraw,
                        e.Font,
                        nameTextColorBrush,
                        _listbox.GetItemRectangle(e.Index).Location.X +
                        MeasureDisplayStringWidth(e.Graphics, textBeforeDraw, e.Font),
                        _listbox.GetItemRectangle(e.Index).Location.Y
                    );
            }
        }

        //Modified from http://www.codeproject.com/Articles/2118/Bypass-Graphics-MeasureString-limitations#premain1
        private static int MeasureDisplayStringWidth(Graphics graphics, string text, Font font)
        {
            if (string.IsNullOrEmpty(text)) return 0;
            RectangleF rect;
            Region[] regions;
            using (var format = new StringFormat())
            {
                rect = new RectangleF(0, 0, 1000, 1000);
                CharacterRange[] ranges = { new CharacterRange(0, text.Length) };
                format.SetMeasurableCharacterRanges(ranges);
                format.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
                regions = graphics.MeasureCharacterRanges(text, font, rect, format);
            }
            rect = regions[0].GetBounds(graphics);
            return (int)(rect.Right + 1.0f) - 3;
        }
    }
}