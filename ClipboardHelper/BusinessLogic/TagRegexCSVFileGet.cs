using System;
using System.Collections.Generic;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public static class TagRegexCsvFileGet
    {
        public static string Handle(List<string> splitContent)
        {
            if (splitContent != null && splitContent.Count != 3) return "Error while parsing";
            try
            {
                var file = new FileData();
                if (splitContent != null)
                {
                    // ReSharper disable once AssignmentIsFullyDiscarded
                    _ = bool.TryParse(splitContent[2], out var ignoreCase);
                    var result = file.Read(splitContent[0], splitContent[1], ignoreCase);
                    return string.IsNullOrEmpty(result) ? "search finished, nothing found" : result;
                }
            }
            catch (Exception)
            {
                return "File could not be read.";
            }

            return null;
        }
    }
}