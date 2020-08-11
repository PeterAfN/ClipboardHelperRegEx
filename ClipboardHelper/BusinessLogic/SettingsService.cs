using ClipboardHelperRegEx.Properties;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public class SettingsService : ISettingsService
    {
        public SettingsService()
        {
            Settings.Default.SettingChanging += OnSettingChanging;
            if (Validate.FirstRunEver)
                SaveFormLowerRightScreenPositionToSettings();
            if (Validate.FirstRunThisVersion)
            {
                FixForRemovingOldVersionAutoStartup();
                ApplyProgramAutoStartSettingToRegistry();
            }

            if (!IsVisibleOnAnyScreen(new Rectangle(Settings.Default.location, Settings.Default.sizeMain))) //Check if the amount of physical screens has changed and the form is now outside the desktop, unreachable. If so, reset position
                SaveFormLowerRightScreenPositionToSettings();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Save()
        {
            Settings.Default.Save();
            ApplyProgramAutoStartSettingToRegistry();
        }

        /// <summary>
        ///     Updates the settings with the location so that the forms fits the lower right screen.
        ///     If multiple monitors, it gets the location of the rightmost monitor.
        /// </summary>
        public static void SaveFormLowerRightScreenPositionToSettings()
        {
            var rightmost = Screen.AllScreens[0];
            foreach (var screen in Screen.AllScreens)
                if (screen.WorkingArea.Right > rightmost.WorkingArea.Right)
                    rightmost = screen;
            Settings.Default.location =
                new Point(rightmost.WorkingArea.Right -
                          Settings.Default.sizeMain.Width,
                    rightmost.WorkingArea.Bottom - Settings.Default.sizeMain.Height);
            Settings.Default.Save(); //creates folder C:\Users\****\AppData\Local\ClipboardHelper
        }

        /// <summary>
        ///     Applies the setting "auto start this program when windows starts" to the operating system.
        /// </summary>
        public static void ApplyProgramAutoStartSettingToRegistry()
        {
            var rkApp = Registry.CurrentUser. OpenSubKey(
                "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            var startPath =
                Environment.GetFolderPath(Environment.SpecialFolder.Programs)
                + @"\ClipboardHelperRegEx\ClipboardHelperRegEx.appref-ms";
            if (Settings.Default.appearanceAutostart)
                rkApp?.SetValue("ClipboardHelperRegEx.exe", startPath);
            else
                rkApp?.DeleteValue("ClipboardHelperRegEx.exe", false);
        }

        private static void FixForRemovingOldVersionAutoStartup()
        {
            var rkAppOld = Registry.CurrentUser.OpenSubKey(
                "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (rkAppOld.GetValue("ClipboardHelper.exe") != null)
            {
                rkAppOld?.DeleteValue("ClipboardHelper.exe", false);
            }

        }

        private static bool IsVisibleOnAnyScreen(Rectangle rect)
        {
            return Screen.AllScreens.Any(screen => screen.WorkingArea.IntersectsWith(rect));
        }

        /// <summary>
        ///     Run when any setting change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnSettingChanging(object sender, SettingChangingEventArgs e)
        {
        }

        #region Properties.Settings.Default

        public bool Activated
        {
            get { return Settings.Default.activated; }
            set
            {
                if (Settings.Default.activated == value) return;
                Settings.Default.activated = value;
                this.Notify(PropertyChanged);
            }
        }

        public string AppearanceProgramsAlternativePasting
        {
            get { return Settings.Default.appearanceProgramsAlternativePasting; }
            set
            {
                if (Settings.Default.appearanceProgramsAlternativePasting == value) return;
                Settings.Default.appearanceProgramsAlternativePasting = value;
                this.Notify(PropertyChanged);
            }
        }

        public Point Location
        {
            get { return Settings.Default.location; }
            set
            {
                if (Settings.Default.location == value) return;
                Settings.Default.location = value;
                this.Notify(PropertyChanged);
            }
        }

        public bool Locked
        {
            get { return Settings.Default.locked; }
            set
            {
                if (Settings.Default.locked == value) return;
                Settings.Default.locked = value;
                this.Notify(PropertyChanged);
            }
        }

        public Size SizeMain
        {
            get { return Settings.Default.sizeMain; }
            set
            {
                if (Settings.Default.sizeMain == value) return;
                Settings.Default.sizeMain = value;
                this.Notify(PropertyChanged);
            }
        }

        public bool AppearanceAlt
        {
            get { return Settings.Default.appearanceALT; }
            set
            {
                if (Settings.Default.appearanceALT == value) return;
                Settings.Default.appearanceALT = value;
                this.Notify(PropertyChanged);
            }
        }

        public bool AppearanceAutoStart
        {
            get { return Settings.Default.appearanceAutostart; }
            set
            {
                if (Settings.Default.appearanceAutostart == value) return;
                Settings.Default.appearanceAutostart = value;
                this.Notify(PropertyChanged);
            }
        }

        public bool AppearanceCtrl
        {
            get { return Settings.Default.appearanceCTRL; }
            set
            {
                if (Settings.Default.appearanceCTRL == value) return;
                Settings.Default.appearanceCTRL = value;
                this.Notify(PropertyChanged);
            }
        }

        public string AppearanceFadeChoices
        {
            get { return Settings.Default.appearanceFadeChoices; }
            set
            {
                if (Settings.Default.appearanceFadeChoices == value) return;
                Settings.Default.appearanceFadeChoices = value;
                this.Notify(PropertyChanged);
            }
        }

        public bool AppearanceFocus
        {
            get { return Settings.Default.appearanceFocus; }
            set
            {
                if (Settings.Default.appearanceFocus == value) return;
                Settings.Default.appearanceFocus = value;
                this.Notify(PropertyChanged);
            }
        }

        public bool AppearanceOpenBrowser
        {
            get { return Settings.Default.appearanceOpenBrowser; }
        }

        public int AppearanceSecondsShowing
        {
            get { return Settings.Default.appearanceSecondsShowing; }
            set
            {
                if (Settings.Default.appearanceSecondsShowing == value) return;
                Settings.Default.appearanceSecondsShowing = value;
                this.Notify(PropertyChanged);
            }
        }

        public string AppearanceSelectedKey
        {
            get { return Settings.Default.appearanceSelectedKey; }
            set
            {
                if (Settings.Default.appearanceSelectedKey == value) return;
                Settings.Default.appearanceSelectedKey = value;
                this.Notify(PropertyChanged);
            }
        }

        public bool AppearanceShift
        {
            get { return Settings.Default.appearanceSHIFT; }
            set
            {
                if (Settings.Default.appearanceSHIFT == value) return;
                Settings.Default.appearanceSHIFT = value;
                this.Notify(PropertyChanged);
            }
        }

        public bool AppearanceWin
        {
            get { return Settings.Default.appearanceWIN; }
            set
            {
                if (Settings.Default.appearanceWIN == value) return;
                Settings.Default.appearanceWIN = value;
                this.Notify(PropertyChanged);
            }
        }

        public Color AppearanceColorTitle
        {
            get { return Settings.Default.appearanceColorTitle; }
            set
            {
                if (Settings.Default.appearanceColorTitle == value) return;
                Settings.Default.appearanceColorTitle = value;
                this.Notify(PropertyChanged);
            }
        }

        public Color AppearanceColorInfo
        {
            get { return Settings.Default.appearanceColorInfo; }
            set
            {
                if (Settings.Default.appearanceColorInfo == value) return;
                Settings.Default.appearanceColorInfo = value;
                this.Notify(PropertyChanged);
            }
        }

        public Color AppearanceColorWebUrlGoTo
        {
            get { return Settings.Default.appearanceColorWebUrlGoTo; }
            set
            {
                if (Settings.Default.appearanceColorWebUrlGoTo == value) return;
                Settings.Default.appearanceColorWebUrlGoTo = value;
                this.Notify(PropertyChanged);
            }
        }

        public Color AppearanceColorOutlookSearch
        {
            get { return Settings.Default.appearanceColorOutlookSearch; }
            set
            {
                if (Settings.Default.appearanceColorOutlookSearch == value) return;
                Settings.Default.appearanceColorOutlookSearch = value;
                this.Notify(PropertyChanged);
            }
        }

        public Color AppearanceColorSelection
        {
            get { return Settings.Default.appearanceColorSelection; }
            set
            {
                if (Settings.Default.appearanceColorSelection == value) return;
                Settings.Default.appearanceColorSelection = value;
                this.Notify(PropertyChanged);
            }
        }

        public string AppearanceColorChoices
        {
            get { return Settings.Default.appearanceColorChoices; }
        }

        public System.Collections.Specialized.StringCollection AdvancedFiles
        {
            get { return Settings.Default.advancedFiles; }
        }


        #endregion Properties.Settings.Default
    }
}