using System;
using System.Collections.Generic;
using System.Linq;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public static class TagNewLine
    {
        public static string Handle(IEnumerable<string> splitContent, Tags.UsedIn usedIn)
        {
            if (splitContent.Count() != 1) return "Error while parsing";
            switch (usedIn)
            {
                case Tags.UsedIn.MainDisplay:
                    return "NewLine()";
                case Tags.UsedIn.Settings:
                    return "NewLine()";
                case Tags.UsedIn.SingleSelection:
                    return "NewLine()";
                case Tags.UsedIn.NestedTags:
                    return string.Empty;
                case Tags.UsedIn.Pasting:
                    return "\n";
                default:
                    throw new ArgumentOutOfRangeException(nameof(usedIn), usedIn, null);
            }
        }
    }
}