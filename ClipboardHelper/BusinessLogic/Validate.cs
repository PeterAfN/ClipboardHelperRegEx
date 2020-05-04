using ClipboardHelper.Views;
using ClipboardHelperRegEx.Properties;
using ClipboardHelperRegEx.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public sealed class Validate : IDisposable
    {

        private readonly ISettingsService _settings;
        private readonly IViewDialog _dialog;
        private readonly ISettingsServiceXmlSerialization _settingsServiceXmlSerialization;
        private readonly IViewUserSettingsRightAutoShownTabs _viewAuto;
        private readonly IViewUserSettingsRightManuallyShownTabs _viewManual;

        public Validate
        (
            IViewUserSettingsRightManuallyShownTabs manuallyShownTabs,
            IViewUserSettingsRightAutoShownTabs autoShownTabs,
            ISettingsServiceXmlSerialization settingsServiceXmlSerialization
        )
        {
            _viewManual = manuallyShownTabs;
            _viewAuto = autoShownTabs;
            _settingsServiceXmlSerialization = settingsServiceXmlSerialization;

            _settings = new SettingsService();
            _dialog = new ViewDialog();
            _dialog.ClickOkMouseButton += Dialog_ClickOkMouseButton;
            _dialog.ClickCancelMouseButton += Dialog_ClickCancelMouseButton;
            SettingsFiles();
            DataFilesUserHasAdded();
            DataFilesIncludedFromStart();

        }

        public static bool FirstRunThisVersion { get; private set; }
        public static bool FirstRunEver { get; private set; }
        public static bool Reset { get; set; }

        private void Dialog_ClickCancelMouseButton(object sender, EventArgs e)
        {
            _dialog.Hide();
        }

        private void Dialog_ClickOkMouseButton(object sender, EventArgs e)
        {
            _dialog.Hide();
        }

        private void DataFilesUserHasAdded()
        {
            var index = 0;
            foreach (var settingsFile in _settings.AdvancedFiles)
            {
                if (!File.Exists(settingsFile))
                {
                    var r = Screen.PrimaryScreen.Bounds;
                    _dialog.Location = new Point(r.Right / 2, r.Bottom / 2);
                    _dialog.BringToFront();
                    _dialog.SetText(AddFileToInfoText(settingsFile));
                    _dialog.UserInput.Hide();
                    _settings.AdvancedFiles.RemoveAt(index);
                    DataFilesUserHasAdded(); // can't continue iterating foreach when item removed.
                    _dialog.Show();
                    _settings.Save();
                    return;
                }
                index++;
            }
        }
        private static void DataFilesIncludedFromStart()
        {
            
            AppDataFilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); //_appDataFilePath	"C:\\Users\\****\\AppData\\Local"
            ExeFilePath = Path.Combine(AppDataFilePath, AssemblyInformation.AssemblyTitle);  //_exeFilePath	"C:\\Users\\****\\AppData\\Local\\ClipboardHelperRegEx"
            var macVendorSourceFilePath = Path.Combine(Application.StartupPath, @"Resources\" + "macvendor.csv");
            var macVendorDestFilePath = Path.Combine(ExeFilePath, "macvendor.csv");
            if (File.Exists(macVendorDestFilePath)) return;
            File.Copy(macVendorSourceFilePath, macVendorDestFilePath, true);
        }

        private string _files = string.Empty;

        private string AddFileToInfoText(string file)
        {
            _files += "\r\n" + file;
            return "Clipboard Helper - File validation \r\n\r\n" + "These files will be removed since they are missing:\r\n" + _files;
        }

        private static string AppDataFilePath { get; set; }
        public static string ExeFilePath { get; private set; }
        public static string AutoSourceFilePath { get; private set; }
        public static string ManualSourceFilePath { get; private set; }
        private static string AutoDestFilePath { get; set; }
        private static string ManualDestFilePath { get; set; }

        private void SettingsFiles()
        {
            AppDataFilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); //_appDataFilePath	"C:\\Users\\****\\AppData\\Local"
            ExeFilePath = Path.Combine(AppDataFilePath, AssemblyInformation.AssemblyTitle);  //_exeFilePath	"C:\\Users\\****\\AppData\\Local\\ClipboardHelperRegEx"
            AutoSourceFilePath = Path.Combine(Application.StartupPath, @"Resources\" + "AutoShownTabs.xml");
            ManualSourceFilePath = Path.Combine(Application.StartupPath, @"Resources\" + "ManuallyShownTabs.xml");
            AutoDestFilePath = Path.Combine(ExeFilePath, "AutoShownTabs.xml");
            ManualDestFilePath = Path.Combine(ExeFilePath, "ManuallyShownTabs.xml");
            MessageBox.Show("1 FirstRunThisVersion=" + FirstRunThisVersion + " FirstRunEver=" + FirstRunEver);
            if (!Directory.Exists(ExeFilePath))
            {
                MessageBox.Show("2 FirstRunThisVersion=" + FirstRunThisVersion + " FirstRunEver=" + FirstRunEver);
                Directory.CreateDirectory(ExeFilePath);
            }

            if (!File.Exists(AutoDestFilePath))
            {
                MessageBox.Show("3 FirstRunThisVersion=" + FirstRunThisVersion + " FirstRunEver=" + FirstRunEver);
                File.Copy(AutoSourceFilePath, AutoDestFilePath, true);
            }
            else
            {
                MessageBox.Show("4 FirstRunThisVersion=" + FirstRunThisVersion + " FirstRunEver=" + FirstRunEver);
                MergeNewXmlWithOldXmlAuto();
            }

            if (!File.Exists(ManualDestFilePath))
            {
                MessageBox.Show("5 FirstRunThisVersion=" + FirstRunThisVersion + " FirstRunEver=" + FirstRunEver);
                File.Copy(ManualSourceFilePath, ManualDestFilePath, true);
            }
            else if (FirstRunEver) //If xml and file exists --> merge
            {
                MessageBox.Show("merge FirstRunThisVersion=" + FirstRunThisVersion + " FirstRunEver=" + FirstRunEver);
                MergeNewXmlWithOldXmlManual();
            }
            
        }

        public void MergeNewXmlWithOldXmlManual(bool userImportingFile = false)
        {
            if (userImportingFile)
            {
                _viewManual.ManuallyShownTabs = _settingsServiceXmlSerialization.ManuallyShownTabs;
            }
            else
            {
                if (!FirstRunThisVersion && !Reset) return;
                var xmlNew = _settingsServiceXmlSerialization.ManuallyShownTabsNew.List;   //new in Visual Studio
                _viewManual.ManuallyShownTabs = _settingsServiceXmlSerialization.ManuallyShownTabs;         //old saved on hard drive
                var newToAdd = new List<ManuallyShownTab>();
                foreach (var nw in xmlNew)
                {
                    var found = false;
                    var count = _viewManual.ManuallyShownTabs.List.Count;
                    for (var j = 0; j < count; j++)
                        if (nw.Name == _viewManual.ManuallyShownTabs.List[j].Name)
                            found = true;
                    if (!found) newToAdd.Add(nw);
                }
                _viewManual.ManuallyShownTabs.List.AddRange(newToAdd);
                _settingsServiceXmlSerialization.ManuallyShownTabs = _viewManual.ManuallyShownTabs;
            }
        }

        public void MergeNewXmlWithOldXmlAuto(bool userImportingFile = false)
        {
            if (userImportingFile)
            {
                _viewAuto.Settings = _settingsServiceXmlSerialization.AutoShownTabs;
            }
            else
            {
                if (!FirstRunThisVersion && !Reset) return;
                var xmlNew = _settingsServiceXmlSerialization.AutoShownTabsNew.List; //new in Visual Studio
                _viewAuto.Settings = _settingsServiceXmlSerialization.AutoShownTabs; //old saved on hard drive
                var newToAdd = new List<AutoShownTab>();
                foreach (var nw in xmlNew)
                {
                    var found = false;
                    var count = _viewAuto.Settings.List.Count;
                    for (var j = 0; j < count; j++)
                        if (nw.Name == _viewAuto.Settings.List[j].Name)
                            found = true;
                    if (!found) newToAdd.Add(nw);
                }

                _viewAuto.Settings.List.AddRange(newToAdd);
                _settingsServiceXmlSerialization.AutoShownTabs = _viewAuto.Settings;
            }
        }

        //If running from visual studio, the AssemblyVersion must be increased manually.
        //If not running from visual studio, but ClickOnce instead, the setting "Automatically 
        //increment revision with each publish" must be clicked.
        public static void CheckIfFirstTimeRunning()
        {
            //When running from Visual Studio it checks for files present at
            //C:\Users\(Windows username)\AppData\Local\(Visual Studio username)\ClipboardHelper.exe_Url_asz4r5fvkztxm1n0xfptltiaxswjw20q
            //Version is changed in AssemblyVersion in AssemblyInfo.cs

            MessageBox.Show("a FirstRunThisVersion=" + FirstRunThisVersion + " FirstRunEver=" + FirstRunEver);
            if (Debugger.IsAttached)
            {
                MessageBox.Show("b FirstRunThisVersion=" + FirstRunThisVersion + " FirstRunEver=" + FirstRunEver);
                if (Settings.Default.UpgradeRequired)
                {
                    //First time running the program with this version.
                    MessageBox.Show("c FirstRunThisVersion=" + FirstRunThisVersion + " FirstRunEver=" + FirstRunEver);
                    FirstRunThisVersion = true;
                    // ReSharper disable once LocalizableElement
                    //MessageBox.Show("First time running the program with this version from Visual Studio");
                    Settings.Default.Upgrade();
                    Settings.Default.Reload();
                    Settings.Default.UpgradeRequired = false;
                    Settings.Default.Save();
                }

                if (!Settings.Default.FirstTimeRunForThisVersion) return;
                //First time ever running the program.
                MessageBox.Show("d FirstRunThisVersion=" + FirstRunThisVersion + " FirstRunEver=" + FirstRunEver);
                FirstRunEver = true;
                // ReSharper disable once LocalizableElement
                //MessageBox.Show("First time ever running the program from Visual Studio");
                Settings.Default.FirstTimeRunForThisVersion = false;
                Settings.Default.Save();
            }
            //If NOT running from Visual Studio then it checks for the ClickOnce version.
            else
            {
                MessageBox.Show("e FirstRunThisVersion=" + FirstRunThisVersion + " FirstRunEver=" + FirstRunEver);
                if (Settings.Default.FirstTimeRunForThisVersion)
                {
                    //First time ever running the program.
                    MessageBox.Show("f FirstRunThisVersion=" + FirstRunThisVersion + " FirstRunEver=" + FirstRunEver);
                    FirstRunEver = true;
                    // ReSharper disable once LocalizableElement
                    //MessageBox.Show("First time ever running the program from exe");
                    Settings.Default.FirstTimeRunForThisVersion = false;
                    Settings.Default.Save();
                }
                MessageBox.Show("g FirstRunThisVersion=" + FirstRunThisVersion + " FirstRunEver=" + FirstRunEver);
                if (!System.Deployment.Application.ApplicationDeployment.CurrentDeployment.IsFirstRun) return;
                //First time running the program with this version.
                MessageBox.Show("h FirstRunThisVersion=" + FirstRunThisVersion + " FirstRunEver=" + FirstRunEver);
                FirstRunThisVersion = true;
                // ReSharper disable once LocalizableElement
                //MessageBox.Show("First time running the program with this version from exe");
                Settings.Default.Upgrade();
                Settings.Default.Reload();
                Settings.Default.UpgradeRequired = false;
                Settings.Default.Save();
            }
        }

        #region Dispose

        private bool _isDisposed;

        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        private void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                // free managed resources
            }

            // free native resources if there are any.
            _isDisposed = true;
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources, but leave the other methods
        // exactly as they are.
        ~Validate()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        #endregion Dispose
    }
}
