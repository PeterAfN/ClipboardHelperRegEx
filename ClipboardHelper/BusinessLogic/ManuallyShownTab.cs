using System.Collections.Generic;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public class ManuallyShownTab
    {
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
        public List<string> Lines { get; set; } = new List<string>();
    }
}