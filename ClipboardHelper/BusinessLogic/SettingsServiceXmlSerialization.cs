using ClipboardHelperRegEx.Properties;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Exception = System.Exception;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public class SettingsServiceXmlSerialization : ISettingsServiceXmlSerialization
    {
        private string _appDataFilePathAuto;
        private string _appDataFilePathManual;
        private string _exeFilePathAuto;
        private string _exeFilePathManual;

        public SettingsServiceXmlSerialization()
        {
            CreateFilePaths();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //Stored on the computer on the hard drive - Existing version
        public ManuallyShownTabs ManuallyShownTabs
        {
            get
            {
                try
                {
                    return XmlSerialization.ReadFromXmlFile<ManuallyShownTabs>(_appDataFilePathManual);
                }
                catch (Exception)
                {
                    MessageBox.Show(Resources.SettingsServiceXmlSerialization_ManuallyShownTabs_ +
                                    _appDataFilePathManual + Resources.SettingsServiceXmlSerialization_ManuallyShownTabs_ +
                                    _appDataFilePathManual +
                                    Resources.SettingsServiceXmlSerialization_ManuallyShownTabs__error_ +
                                    string.Join("",
                                        DateTime.Now.ToString(CultureInfo.InvariantCulture).Split(':', '/')) +
                                    Resources.SettingsServiceXmlSerialization_AutoShownTabs__xml,
                        Resources.SettingsServiceXmlSerialization_ManuallyShownTabs_Clipboard_Helper_Error,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (!Directory.Exists(Validate.ExeFilePath))
                        Directory.CreateDirectory(Validate.ExeFilePath);
                    File.Copy(_appDataFilePathManual,
                        _appDataFilePathManual + Resources.SettingsServiceXmlSerialization_ManuallyShownTabs__error_ +
                        string.Join("", DateTime.Now.ToString(CultureInfo.InvariantCulture).Split(':', '/')) +
                        Resources.SettingsServiceXmlSerialization_AutoShownTabs__xml, true);
                    File.Copy(Validate.ManualSourceFilePath, _appDataFilePathManual, true);
                    return XmlSerialization.ReadFromXmlFile<ManuallyShownTabs>(_appDataFilePathManual);
                }

            }
            set
            {
                XmlSerialization.WriteToXmlFile(_appDataFilePathManual, value);
                this.Notify(PropertyChanged);
            }
        }

        //Stored in Visual Studio
        public ManuallyShownTabs ManuallyShownTabsNew
        {
            get { return XmlSerialization.ReadFromXmlFile<ManuallyShownTabs>(_exeFilePathManual); }
        }

        public AutoShownTabs AutoShownTabs
        {
            get
            {
                try
                {
                    return XmlSerialization.ReadFromXmlFile<AutoShownTabs>(_appDataFilePathAuto);
                }
                catch (Exception)
                {
                    MessageBox.Show(Resources.SettingsServiceXmlSerialization_ManuallyShownTabs_ +
                                    _appDataFilePathAuto + Resources.SettingsServiceXmlSerialization_ManuallyShownTabs_ +
                                    _appDataFilePathAuto +
                                    Resources.SettingsServiceXmlSerialization_ManuallyShownTabs__error_ +
                                    string.Join("",
                                        DateTime.Now.ToString(CultureInfo.InvariantCulture).Split(':', '/')) +
                                    Resources.SettingsServiceXmlSerialization_AutoShownTabs__xml,
                        Resources.SettingsServiceXmlSerialization_ManuallyShownTabs_Clipboard_Helper_Error,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (!Directory.Exists(Validate.ExeFilePath))
                        Directory.CreateDirectory(Validate.ExeFilePath);
                    File.Copy(_appDataFilePathAuto,
                        _appDataFilePathAuto + Resources.SettingsServiceXmlSerialization_ManuallyShownTabs__error_ +
                        string.Join("", DateTime.Now.ToString(CultureInfo.InvariantCulture).Split(':', '/')) +
                        Resources.SettingsServiceXmlSerialization_AutoShownTabs__xml, true);
                    File.Copy(Validate.AutoSourceFilePath, _appDataFilePathAuto, true);
                    return XmlSerialization.ReadFromXmlFile<AutoShownTabs>(_appDataFilePathAuto);
                }

            }
            set
            {
                XmlSerialization.WriteToXmlFile(_appDataFilePathAuto, value);
                this.Notify(PropertyChanged);
            }
        }

        public AutoShownTabs AutoShownTabsNew
        {
            get { return XmlSerialization.ReadFromXmlFile<AutoShownTabs>(_exeFilePathAuto); }
        }

        private void CreateFilePaths()
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var userFilePath = Path.Combine(localAppData, AssemblyInformation.AssemblyTitle);
            _appDataFilePathManual = Path.Combine(userFilePath, "ManuallyShownTabs.xml");
            _exeFilePathManual = GetSourceFilePath("ManuallyShownTabs.xml");
            _appDataFilePathAuto = Path.Combine(userFilePath, "AutoShownTabs.xml");
            _exeFilePathAuto = GetSourceFilePath("AutoShownTabs.xml");
        }

        private static string GetSourceFilePath(string filename)
        {
            return Path.Combine(Application.StartupPath, @"Resources\" + filename);
        }
    }
}