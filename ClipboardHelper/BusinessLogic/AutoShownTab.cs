using System.Collections.Generic;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public class AutoShownTab
    {
        public string Name { get; set; }

        public string RegEx { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
        public List<string> Items { get; set; } = new List<string>();

        public string SimulatedClipboard { get; set; }
    }
}