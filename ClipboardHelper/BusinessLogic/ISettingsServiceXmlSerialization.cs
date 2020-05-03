using System.ComponentModel;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public interface ISettingsServiceXmlSerialization : INotifyPropertyChanged
    {
        ManuallyShownTabs ManuallyShownTabs { get; set; }
        ManuallyShownTabs ManuallyShownTabsNew { get; }

        AutoShownTabs AutoShownTabs { get; set; }
        AutoShownTabs AutoShownTabsNew { get; }
    }
}