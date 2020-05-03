using System.Collections.Generic;
using System.Linq;

namespace ClipboardHelperRegEx.BusinessLogic
{
    /// <summary>
    ///     Lines are listbox lines or items. This class formats lines in different ways.
    /// </summary>
    internal static class Lines
    {
        public enum EditingMethods
        {
            InsertNumbering,
            RemoveNumbering,
            Clean
        }

        public static List<string> Edit(EditingMethods editingMethods, List<string> input)
        {
            switch (editingMethods)
            {
                case EditingMethods.InsertNumbering:
                    return InsertNumbering(input);
                case EditingMethods.RemoveNumbering:
                    return RemoveNumbering(input);
                case EditingMethods.Clean:
                    return CleanManyLines(input);
                default:
                    return null;
            }
        }

        /// <summary>
        ///     Add initial line numbering starting from 1 and counting up.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static List<string> InsertNumbering(List<string> input)
        {
            var outList = input;
            var y = 0;
            for (var x = 0; x < input.Count; x++)
            {
                if (string.IsNullOrEmpty(input[x]) || input[x] == "\r" || input[x].Contains("NotSelectableLine()") ||
                    input[x].Contains("Caption"))
                {
                    if (y + 1 < 10)
                        outList[x] = "     " + input[x];
                    else
                        outList[x] = "      " + input[x];
                    y--;
                }
                else
                {
                    if (y + 1 < 10) outList[x] = y + 1 + "   " + input[x];
                    else outList[x] = y + 1 + "  " + input[x];
                }

                y++;
            }

            return outList;
        }

        /// <summary>
        ///     Removes any initial line numbering.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static List<string> RemoveNumbering(IReadOnlyCollection<string> input)
        {
            var output = new List<string>();
            if (input != null)
            {
                var digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                output.AddRange(input.Select(line => line.TrimStart(digits)));
                return output;
            }

            output = new List<string> { "", "" };
            return output;
        }

        /// <summary>
        ///     Cleans and trims newline and any empty space
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static List<string> CleanManyLines(IReadOnlyCollection<string> input)
        {
            var output = new List<string>();
            if (input != null)
            {
                output.AddRange(input.Select(CleanOneLine));
                return output;
            }

            output = new List<string> { "", "" };
            return output;
        }

        public static string CleanOneLine(string line)
        {
            line = line.TrimStart();
            line = line.TrimStart(' ', '\t');
            line = line.TrimEnd();
            line = line.TrimEnd(' ', '\t');
            line = line.Replace(@"\newline\", "\n");
            return line;
        }
    }
}