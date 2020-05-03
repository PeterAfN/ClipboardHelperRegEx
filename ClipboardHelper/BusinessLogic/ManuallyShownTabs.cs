using System.Collections.Generic;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public class ManuallyShownTabs
    {
        //https://softwareengineering.stackexchange.com/questions/196125/is-it-a-good-practice-to-create-a-classcollection-of-another-class
        //http://blog.danskingdom.com/saving-and-loading-a-c-objects-data-to-an-xml-json-or-binary-file/

        public List<ManuallyShownTab> List { get; } = new List<ManuallyShownTab>();
    }
}