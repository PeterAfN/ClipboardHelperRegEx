using System.ComponentModel;
using System.Drawing;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public interface ISettingsService : INotifyPropertyChanged
    {
        Size SizeMain { get; set; }
        bool Activated { get; set; }
        Point Location { get; set; }
        bool Locked { get; set; }
        string AppearanceProgramsAlternativePasting { get; set; }

        string AppearanceSelectedKey { get; set; }
        bool AppearanceAutoStart { get; set; }
        string AppearanceFadeChoices { get; set; }
        bool AppearanceFocus { get; set; }
        bool AppearanceOpenBrowser { get; }
        int AppearanceSecondsShowing { get; set; }

        Color AppearanceColorSelection { get; set; }
        Color AppearanceColorTitle { get; set; }
        Color AppearanceColorInfo { get; set; }
        Color AppearanceColorWebUrlGoTo { get; set; }
        Color AppearanceColorOutlookSearch { get; set; }

        string AppearanceColorChoices { get; }

        bool AppearanceAlt { get; set; }
        bool AppearanceCtrl { get; set; }
        bool AppearanceShift { get; set; }
        bool AppearanceWin { get; set; }

        System.Collections.Specialized.StringCollection AdvancedFiles { get; }

        void Save();
    }
}