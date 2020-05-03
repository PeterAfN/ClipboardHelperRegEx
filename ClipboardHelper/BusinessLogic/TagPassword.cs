using AdysTech.CredentialManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public static class TagPassword
    {
        public static string Handle(IEnumerable<string> splitContent, Tags.UsedIn usedIn)
        {
            if (splitContent.Count() != 1) return "Error while parsing";
            switch (usedIn)
            {
                case Tags.UsedIn.MainDisplay:
                case Tags.UsedIn.Settings:
                case Tags.UsedIn.SingleSelection:
                    return "Password(************)";
                case Tags.UsedIn.NestedTags:
                case Tags.UsedIn.Pasting:
                    return CredentialManager.GetCredentials("ClipboardHelper").Password ?? string.Empty;
                default:
                    throw new ArgumentOutOfRangeException(nameof(usedIn), usedIn, null);
            }
        }
    }
}