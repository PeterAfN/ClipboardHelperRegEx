using ClipboardHelper.Views;
using ClipboardHelperRegEx.BusinessLogic;
using ClipboardHelperRegEx.BusinessLogic.Presenters;
using ClipboardHelperRegEx.Properties;
using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace ClipboardHelperRegEx
{

    internal static class Program
    {

        /// <summary>
        ///     The main entry point for the application which follows the design pattern of Passive (Model) View Presenter
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Validate.CheckIfFirstTimeRunning();

            if (IsAnotherInstanceOfThisProgramRunning(Assembly.GetEntryAssembly()?.GetName().Name)) return;

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            Application.ThreadException += ApplicationOnThreadException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //The program is initiated in this class. The different presenters handle all the user input from the views so the program
            //behaves correctly.

            //User controls
            var settingsArea = new TableLayoutPanel { Dock = DockStyle.Fill };
            var settingsLeft = new GroupBox { Dock = DockStyle.Left };
            var settingsRight = new GroupBox { Dock = DockStyle.Fill };
            var settingsMenuLeft = new ViewUserSettingsLeftMenu { Dock = DockStyle.Fill };
            var settingsRightAppearance = new ViewUserSettingsRightAppearance { Dock = DockStyle.Fill };
            var settingsRightAutoShownTabs = new ViewUserSettingsRightAutoShownTabs
            { Dock = DockStyle.Fill };
            var settingsRightManuallyShownTabs = new ViewUserSettingsRightManuallyShownTabs
            { Dock = DockStyle.Fill };
            var settingsDownButtons = new ViewUserSettingsDownButtons { Dock = DockStyle.Bottom };
            var settingsAdvanced = new ViewUserSettingsRightAdvanced { Dock = DockStyle.Fill };
            var settingsHelp = new ViewUserSettingsRightHelp { Dock = DockStyle.Fill };
            var mainSplCont = new ViewMainSplCont { Dock = DockStyle.Fill };
            var mainSplContPanelUpTabs = new ViewMainSplContPanelUpTabs { Dock = DockStyle.Fill };
            var mainSplContPanelDown = new ViewMainSplContPanelDown { Dock = DockStyle.Fill };

            //Built-in Visual Studio settings
            ISettingsService settings = new SettingsService();

            //Not built-in Visual Studio settings
            ISettingsServiceXmlSerialization settingsServiceXmlSerialization = new SettingsServiceXmlSerialization();

            //Built-in Visual Studio resources
            IResourcesService resources = new ResourcesService();

            //Other classes
            var validate = new Validate(settingsRightManuallyShownTabs, settingsRightAutoShownTabs, settingsServiceXmlSerialization);
            var pasting = new Pasting();
            var fileData = new FileData();

            //Views
            var viewAbout = new ViewAbout();
            var viewMain = new ViewMain(mainSplCont, mainSplContPanelUpTabs, mainSplContPanelDown);
            var viewMin = new ViewMin();
            var viewUserSettings = new ViewUserSettings(settingsArea, settingsDownButtons, settingsLeft,
                settingsRight, settingsMenuLeft);
            var viewDialog = new ViewDialog();

            //Presenters
            var unused1 = new PresenterAbout(viewAbout, resources);
            var unused2 = new PresenterMain(
                viewMain,
                viewMin,
                viewUserSettings,
                viewAbout,
                settings,
                resources,
                mainSplContPanelUpTabs,
                settingsDownButtons,
                pasting);
            var unused3 = new PresenterMainSplCont(mainSplCont, viewMain);
            var unused4 = new PresenterMainSplContPanelUpTabs(
                mainSplContPanelUpTabs,
                viewMain,
                viewMin,
                settings,
                pasting,
                mainSplContPanelDown,
                settingsServiceXmlSerialization,
                settingsDownButtons);
            var unused5 = new PresenterMainSplContPanelDown(mainSplContPanelDown);
            var unused6 = new PresenterMin(viewMin, viewMain, resources);
            var unused7 = new PresenterUserSettings(
                viewUserSettings,
                resources);
            var unused8 = new PresenterUserSettingsLeftMenu(
                settingsMenuLeft,
                viewUserSettings,
                settingsRightAppearance,
                settingsRightAutoShownTabs,
                settingsRightManuallyShownTabs,
                settingsAdvanced,
                settingsHelp);
            var unused9 = new PresenterUserSettingsRightAppearance(
                settingsRightAppearance,
                settings,
                settingsDownButtons,
                viewMain,
                validate,
                viewDialog);
            var unused10 = new PresenterUserSettingsRightAutoShownTabs(
                settingsRightAutoShownTabs,
                viewDialog,
                viewUserSettings,
                settingsDownButtons,
                settingsServiceXmlSerialization,
                mainSplContPanelUpTabs);
            var unused11 = new PresenterUserSettingsRightManuallyShownTabs(
                settingsRightManuallyShownTabs,
                viewDialog,
                viewUserSettings,
                settingsDownButtons,
                settingsServiceXmlSerialization);
            var unused12 = new PresenterUserSettingsDownButtons(
                settingsDownButtons,
                viewUserSettings,
                settings);
            var unused13 = new PresenterUserSettingsRightAdvanced(
                settingsAdvanced,
                viewDialog,
                settings,
                settingsRightAutoShownTabs,
                settingsServiceXmlSerialization,
                settingsRightManuallyShownTabs,
                validate,
                fileData);
            var unused14 = new PresenterDialog(viewDialog);
            var unused15 = new PresenterUserSettingsRightHelp(
                settingsHelp);

            Application.Run(viewMain);

            //Disposing
            settingsRightAppearance.Dispose();
            settingsRightAutoShownTabs.Dispose();
            settingsRightManuallyShownTabs.Dispose();
            settingsAdvanced.Dispose();
            settingsHelp.Dispose();
            viewAbout.Dispose();
            viewMain.Dispose();
            viewMin.Dispose();
            viewUserSettings.Dispose();
            viewDialog.Dispose();
            unused2.Dispose();
            unused4.Dispose();
            unused5.Dispose();
            unused6.Dispose();
            unused13.Dispose();
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var message = "A fault occured.\r\n" + $"{((Exception)e.ExceptionObject).Message}\r\n";
            Console.WriteLine(@"Fault {0}: {1}", DateTimeOffset.Now, e.ExceptionObject);
            MessageBox.Show(message, Resources.Program_ApplicationOnThreadException);
        }

        private static void ApplicationOnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            var message = "A fault occured.\r\n" + $"{e.Exception.Message}\r\n";
            Console.WriteLine(@"Fault {0}: {1}", DateTimeOffset.Now, e.Exception);
            MessageBox.Show(message, Resources.Program_CurrentDomainOnUnhandledException);
        }

        private static bool IsAnotherInstanceOfThisProgramRunning(string programName)
        {
            var mutex = new Mutex(false, programName);
            try
            {
                return !mutex.WaitOne(0, false);
            }
            finally
            {
                mutex.Close();
            }
        }
    }
}