using ClipboardHelperRegEx.BusinessLogic.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public class Tags
    {
        /// <summary>
        ///     Tags which the user can use in the program.
        /// </summary>
        public enum TagType
        {
            NoTag,
            Regex,
            RegexReplace,
            ChangeableContent,
            WebUrlGoTo,
            OutlookSearch,
            RegexCsvFileGet,
            Password,
            Encrypt,
            NotSelectableLine,
            Caption,
            Info,
            Clipboard,
            WebGetJason,
            WebGet,
            FromUnixTimeMilliSecondsToUtc,
            ConsoleOpen,
            NewLine
        }

        /// <summary>
        ///     Different scenarios where tag transformation is occurring.
        /// </summary>
        public enum UsedIn
        {
            MainDisplay,
            Pasting,
            Settings,
            SingleSelection,
            NestedTags
        }

        private static CancellationToken _cancellationToken;

        public static CancellationTokenSource TokenSource { get; private set; }

        /// <summary>
        ///     Tags that can be time consuming to parse.
        /// </summary>
        private readonly List<TagType> _slowTags = new List<TagType>
        {
            TagType.RegexCsvFileGet, TagType.Password, TagType.WebGetJason, TagType.WebGet
        };

        private readonly TagWebGetJason _tagWebGetJason;

        private string _changeableContent = string.Empty;
        private string _changedContent = string.Empty;
        private bool _changedContentEdited;

        public Tags()
        {
            _tagWebGetJason = new TagWebGetJason();
        }

        private Action<int, int, PresenterMainSplContPanelUpTabs.LineChangeType, int, IList<string>, bool, string, int> Action { get; set; }

        private List<string> Lines { get; set; }

        private IList<string> LinesOut { get; set; }

        /// <summary>
        ///     Cancel tag.
        /// </summary>
        public static void Cancel()
        {
            TokenSource?.Cancel(true);
        }

        public static IEnumerable<TagType> GetAllTagTypes()
        {
            var allTagTypes = Enum.GetValues(typeof(TagType)).Cast<TagType>().ToList();
            return allTagTypes;
        }

        private static Dictionary<int, string> CreateLinesWithIndex(IReadOnlyList<string> lines)
        {
            var output = new Dictionary<int, string>();
            for (var i = 0; i < lines.Count; i++) output.Add(i, lines[i]);
            return output;
        }

        private Dictionary<int, string> GetFastLines(Dictionary<int, string> lines)
        {
            return lines.Where(line => !StringContainsTags(line.ToString(),
                    _slowTags))
                .ToDictionary(line => line.Key,
                    line => line.Value);
        }

        private Dictionary<int, string> GetSlowLines(Dictionary<int, string> lines)
        {
            return lines.Where(line => StringContainsTags(line.ToString(),
                    _slowTags))
                .ToDictionary(line => line.Key,
                    line => line.Value);
        }

        /// <summary>
        ///     Transforms tags for presentation for the user. There are five places where presentation occurs: In the settings,
        ///     in the program main form, when pasting to another program, when clicking on a item (opens url in web browser) or
        ///     in tags inside tags (nested tags).
        /// </summary>
        public async void TransformLines(
            int navigationPosition,
            int id,
            PresenterMainSplContPanelUpTabs.LineChangeType lineChangeType,
            int tabIndex,
            List<string> lines,
            List<TagType> tags,
            Action<int, int, PresenterMainSplContPanelUpTabs.LineChangeType, int, IList<string>, bool, string, int> action,
            string clipboard = "",
            bool changedContentEdited = false,
            string changedContent = "")
        {
            Cancel();
            Action = action;
            Lines = lines;
            TokenSource = new CancellationTokenSource();
            _cancellationToken = TokenSource.Token;
            _changedContent = changedContent;
            _changedContentEdited = changedContentEdited;
            if (Lines == null) return;
            var linesWithIndex = CreateLinesWithIndex(Lines);

            var linesFast = GetFastLines(linesWithIndex);
            LinesOut = new List<string>(lines);
            foreach (var line in linesFast)
                LinesOut[line.Key] = TransformLine(line.Value, UsedIn.MainDisplay, tags, clipboard);

            //Set fast lines
            if (Action == null) return;
            {
                Action(navigationPosition, id, lineChangeType, tabIndex, LinesOut, changedContentEdited,
                    _changeableContent, -1);

                var linesSlow = GetSlowLines(linesWithIndex);
                var stripped = StripOutEverythingExceptLineNumberingAndInfoTag(linesSlow);
                foreach (var line in stripped)
                    LinesOut[line.Key] = TransformLine(line.Value, UsedIn.MainDisplay, tags, clipboard);

                //Set slow lines
                Action(navigationPosition, id, lineChangeType, tabIndex, LinesOut, changedContentEdited,
                    _changeableContent, -1);

                //Set slow lines async
                if (linesSlow.Count != 0)
                    await Task.Run(() => SetLinesAsync(navigationPosition, id, lineChangeType, tabIndex,
                            linesSlow, tags, Action, _cancellationToken, clipboard, changedContentEdited),
                        _cancellationToken).ConfigureAwait(true); //parse delayed Tags
            }
        }

        private void SetLinesAsync
        (
            int navigationPosition,
            int id,
            PresenterMainSplContPanelUpTabs.LineChangeType lineChangeType,
            int tabIndex,
            Dictionary<int, string> linesSlow,
            List<TagType> tags,
            Action<int, int, PresenterMainSplContPanelUpTabs.LineChangeType, int, IList<string>, bool, string, int> action,
            CancellationToken cancelToken,
            string clipboard = "",
            bool isChangedContentEdited = false
        )
        {
            foreach (var line in linesSlow)
            {
                if (cancelToken.IsCancellationRequested)
                    return;
                if (StringContainsTags(line.Value, new List<TagType> { TagType.WebGetJason }))
                {
                    LinesOut[line.Key] = TransformLine(line.Value, UsedIn.MainDisplay, tags, clipboard);
                    switch (lineChangeType)
                    {
                        case PresenterMainSplContPanelUpTabs.LineChangeType.AutoMulti:
                        case PresenterMainSplContPanelUpTabs.LineChangeType.AutoSingle:
                            action(
                                navigationPosition,
                                id,
                                PresenterMainSplContPanelUpTabs.LineChangeType.AutoSingle,
                                tabIndex,
                                new List<string> { LinesOut[line.Key] }, isChangedContentEdited,
                                _changeableContent,
                                line.Key);
                            break;
                        case PresenterMainSplContPanelUpTabs.LineChangeType.ManualMulti:
                        case PresenterMainSplContPanelUpTabs.LineChangeType.ManualSingle:
                            action(
                                navigationPosition,
                                id,
                                PresenterMainSplContPanelUpTabs.LineChangeType.ManualSingle,
                                tabIndex,
                                new List<string> { LinesOut[line.Key] }, isChangedContentEdited,
                                _changeableContent,
                                line.Key);
                            break;
                    }
                }
                else
                {
                    LinesOut[line.Key] = TransformLine(line.Value, UsedIn.MainDisplay, tags, clipboard);
                    action(navigationPosition, id, lineChangeType, tabIndex, LinesOut, isChangedContentEdited, _changeableContent, -1);
                }
            }
            action(navigationPosition, id, lineChangeType, tabIndex, LinesOut, isChangedContentEdited, _changeableContent, -1);
        }

        public string TransformLine(string line, UsedIn usedIn, List<TagType> tags, string clipboard = "")
        {
            var lineOut = string.Empty;
            try
            {
                while (!string.IsNullOrEmpty(line))
                    if (StringContainsTags(line, tags))
                    {
                        if (tags == null) continue;
                        var first = GetTagsOrderedInPosition(line, tags);
                        var firstPos = first.ElementAt(0).Key;
                        var firstMatch = first.ElementAt(0).Value;
                        if (firstPos > 0)
                        {
                            lineOut += line.Substring(0, firstPos); //Line from tag-end to next tag-start
                            line = line.Remove(0, firstPos);
                        }
                        else
                        {
                            lineOut += ParseTag( //Line from tag-start to tag-end
                                GetContentFromTagAndContent(firstMatch.ToString()),
                                GetTagFromTagAndContent(firstMatch.ToString()),
                                tags,
                                usedIn,
                                clipboard
                            );
                            line = line.Remove(0, firstMatch.Length);
                        }
                    }
                    else
                    {
                        lineOut += line;
                        line = string.Empty;
                    }

                return lineOut;
            }
            catch (System.Net.WebException)
            {
                return lineOut + "404 Not Found";
            }

            //catch (Exception)
            //{
            //    //return lineOut + "Line could not be transformed.";
            //    throw;
            //}
        }

        private string ParseTag(string contents, TagType tag, List<TagType> tagTypes, UsedIn usedIn,
            string clipboard = "")
        {
            var line = new StringBuilder(contents);
            var content = CreateAListOfSemicolonSeparatedContentForATag(contents, tagTypes);
            var ctr = 0;
            foreach (var item in content.ToList())
            {
                if (StringContainsTags(item, tagTypes)) //Is content a nested Tag? 
                {
                    content[ctr] = TransformLine(item, UsedIn.NestedTags, tagTypes, clipboard); //Recursive.   
                    if (!string.IsNullOrEmpty(item)) line = line.Replace(item, content[ctr]);
                }
                else
                {
                    content[ctr] = item;
                    if (!string.IsNullOrEmpty(item)) line = line.Replace(item, content[ctr]);
                }

                ctr += 1;
            }

            return Handle(line.ToString(), tag, usedIn, content, clipboard);
        }

        private string Handle(string line, TagType tag, UsedIn usedIn, List<string> splitContent, string clipboard = "")
        {
            splitContent = RemoveDelimiterEscapeCharacter(splitContent);
            switch (tag)
            {
                case TagType.Regex:
                    return TagRegex.Handle(splitContent, clipboard);
                case TagType.RegexReplace:
                    return TagRegexReplace.Handle(splitContent, clipboard);
                case TagType.WebUrlGoTo when usedIn == UsedIn.SingleSelection:
                    TagWebUrlGoTo.Handle(splitContent);
                    break;
                case TagType.ChangeableContent:
                    return TagChangeableContent.Handle(line, usedIn, SetChangeableContent, _changedContentEdited,
                        _changedContent);
                case TagType.OutlookSearch when usedIn == UsedIn.SingleSelection:
                    var unused = new TagOutlook(splitContent[0]);
                    break;
                case TagType.OutlookSearch when usedIn != UsedIn.SingleSelection:
                    return tag + "(" + line + ")"; //tag is removed in ModifiedListbox
                case TagType.RegexCsvFileGet:
                    return TagRegexCsvFileGet.Handle(splitContent);
                case TagType.Password:
                    return TagPassword.Handle(splitContent, usedIn);
                case TagType.Info:
                    return TagInfo.Handle(line, splitContent, usedIn);
                case TagType.Clipboard:
                    return clipboard;
                case TagType.WebGetJason:
                    return _tagWebGetJason.Handle(line, splitContent);
                case TagType.WebGet:
                    return TagWebGet.Handle(line, usedIn);
                case TagType.FromUnixTimeMilliSecondsToUtc:
                    return TagFromUnixTimeMilliSecondsToUtc.Handle(splitContent);
                case TagType.ConsoleOpen:
                    return TagConsole.Handle(line, usedIn);
                case TagType.NewLine:
                    return TagNewLine.Handle(splitContent, usedIn);
                case TagType.Caption:
                    return tag + "(" + line + ")"; //tag is removed in ModifiedListbox
                case TagType.NotSelectableLine:
                    return tag + "(" + line + ")"; //tag is removed in ModifiedListbox
                case TagType.WebUrlGoTo when usedIn != UsedIn.SingleSelection:
                    return tag + "(" + line + ")"; //tag is removed in ModifiedListbox
                case TagType.NoTag:
                    break;
                case TagType.Encrypt:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tag), tag, null);
            }

            return line;
        }

        private void SetChangeableContent(string content)
        {
            _changeableContent = content;
        }

        private static List<string> RemoveDelimiterEscapeCharacter(IEnumerable<string> splitContent)
        {
            return splitContent.Select(str => str.Replace("/;", ";")).ToList();
        }

        private static Dictionary<int, Match> GetTagsOrderedInPosition(string line, List<TagType> tags)
        {
            var sortedMatches = new SortedList<int, Match>();
            foreach (var tag in tags) //Find out position for first tag.
            {
                var matches = Regex.Matches(line, CreateRegexGetTagAndContent(tag)).Cast<Match>().ToList();
                foreach (var item in matches)
                    sortedMatches.Add(item.Index, item);
            }

            if (sortedMatches.Count > 0)
                return new Dictionary<int, Match>
                {
                    {sortedMatches.ElementAt(0).Key, sortedMatches.ElementAt(0).Value}
                };
            return null;
        }

        private static List<string> CreateAListOfSemicolonSeparatedContentForATag(string line, List<TagType> tags)
        {
            var first = GetTagsOrderedInPosition(line, tags);
            TagType tagType;
            if (first != null)
            {
                var match = first.ElementAt(0).Value;
                tagType = GetTagFromTagAndContent(match.ToString());
            }
            else
            {
                tagType = TagType.NoTag;
            }

            var listOfTagAndContent = Regex.Matches(line, CreateRegexGetTagAndContent(tagType)).Cast<Match>().ToList();
            var queue = new Queue<Match>(listOfTagAndContent);
            var lineWithDummies = RegexReplace(line, CreateRegexGetTagAndContent(tagType), "dummy");
            var separatedItems =
                SplitAndTrimContent(
                    lineWithDummies); //Example: RegexReplace(       RegexReplace(Clipboard();[:_\-. ,];)       ;        (.{2})(.{2})(.{2})(.{2})(.{2})(.{2})     ;      $1$2-$3$4-$5$6       )
            var outData =
                new List<string>(); //                                                     ^         ^        ^                                                 ^
            foreach (var separatedItem in separatedItems
            ) //                                               don'tdetect  don'tdetect  detect                                            detect
                if (separatedItem.Contains("dummy"))
                {
                    outData.Add(queue.First().ToString());
                    queue.Dequeue();
                }
                else
                {
                    outData.Add(separatedItem);
                }

            return outData;
        }

        public static bool StringContainsTags(string content, IEnumerable<TagType> tags)
        {
            return content != null && tags
                       .Select(tag =>
                           Regex.Matches(content,
                                   @"((?<=" + tag + @"\())((?>\((?<DEPTH>)|\)(?<-DEPTH>)|[^()]+)*(?=\))(?(DEPTH)(?!)))")
                               .Cast<Match>().ToList())
                       .Any(nestedTagMatchesInContent => nestedTagMatchesInContent.Count > 0);
        }

        private static string CreateRegexGetTagAndContent(TagType tagType)
        {
            return @"(" + tagType + @"\((?<=" + tagType +
                   @"\())((?>\((?<DEPTH>)|\)(?<-DEPTH>)|[^()]+)*(?=\))(?(DEPTH)(?!)))\)";
        }

        private static IEnumerable<string> SplitAndTrimContent(object inData)
        {
            var regex = new Regex(@"(?<!/);"); // Do not split with "/;", "/" is escape character
            var split = regex.Split(inData.ToString()).ToList();
            return split.Select(item => item.Trim()).ToList();
        }

        private static string RegexReplace(string inData, string pattern, string replacement)
        {
            var rgx = new Regex(pattern);
            return rgx.Replace(inData, replacement);
        }

        private static TagType GetTagFromTagAndContent(string tagAndContent)
        {
            const string stopAt = "(";
            if (string.IsNullOrWhiteSpace(tagAndContent)) return TagType.NoTag;
            var charLocation = tagAndContent.IndexOf(stopAt, StringComparison.Ordinal);
            if (charLocation <= 0) return TagType.NoTag;
            return Enum.TryParse(tagAndContent.Substring(0, charLocation), out TagType tagType)
                ? tagType
                : TagType.NoTag;
        }


        private static string GetContentFromTagAndContent(string line)
        {
            var output = line.Substring(line.IndexOf('(') + 1, line.LastIndexOf(')') - line.IndexOf('(') - 1);
            return output;
        }

        /// <summary>
        ///     This creates temporary lines to show while Delayed tags have finished.
        ///     It strips away everything except line numbering and Info() tag.
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        private static Dictionary<int, string> StripOutEverythingExceptLineNumberingAndInfoTag(
            Dictionary<int, string> lines)
        {
            const string regex = @".*(Info\((?<=Info\())((?>\((?<DEPTH>)|\)(?<-DEPTH>)|[^()]+)*(?=\))(?(DEPTH)(?!)))\)";
            return lines.ToDictionary(line => line.Key, line => RegexMatch(line.Value, regex));
        }

        private static string RegexMatch(string inData, string inRegex)
        {
            var outData = "";
            if (inData == null) return outData;
            if (Regex.IsMatch(inData, inRegex))
            {
                var listOfMatches = Regex.Matches(inData, inRegex);
                var partialComponents = listOfMatches.Cast<Match>().Select(match => match.Value).ToList();
                outData = partialComponents[0];
            }
            else
            {
                outData = string.Empty;
            }

            return outData;
        }
    }
}