using System;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public static class TagChangeableContent
    {
        public static string Handle(string line, Tags.UsedIn usedIn, Action<string> action, bool changedContentEdited,
            string changedContent)
        {
            switch (changedContentEdited)
            {
                case true: /*don't set view.TextBoxChangeableContent.Text since the user changed it.*/
                    switch (usedIn)
                    {
                        case Tags.UsedIn.MainDisplay:
                            return "ChangeableContent(" + changedContent + ")";
                        case Tags.UsedIn.NestedTags:
                            return changedContent;
                        case Tags.UsedIn.Pasting:
                            break;
                        case Tags.UsedIn.Settings:
                            break;
                        case Tags.UsedIn.SingleSelection:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(usedIn), usedIn, null);
                    }

                    break;
                case false:
                    action?.Invoke(line); //sets the text box TextBoxChangeableContent.Text.
                    switch (usedIn)
                    {
                        case Tags.UsedIn.MainDisplay:
                        case Tags.UsedIn.Settings:
                        case Tags.UsedIn.SingleSelection:
                            return "ChangeableContent(" + line + ")";
                        case Tags.UsedIn.NestedTags:
                        case Tags.UsedIn.Pasting:
                            return line;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(usedIn), usedIn, null);
                    }
            }

            return "Error while parsing";
        }
    }
}