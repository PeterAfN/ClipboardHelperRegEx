using ClipboardHelperRegEx.Properties;
using ClipboardHelperRegEx.Views;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.BusinessLogic.Presenters
{
    public sealed class PresenterUserSettingsRightAdvanced : IDisposable
    {
        private readonly IViewDialog _dialog;
        private readonly ISettingsService _settings;
        private readonly IViewUserSettingsRightAdvanced _view;
        private readonly IViewUserSettingsRightAutoShownTabs _rightAutoShownTabs;
        private readonly ISettingsServiceXmlSerialization _settingsServiceXmlSerialization;
        private readonly IViewUserSettingsRightManuallyShownTabs _rightManuallyShownTabs;
        private readonly Validate _validate;
        private readonly FileData _fileData;

        public PresenterUserSettingsRightAdvanced
        (
            IViewUserSettingsRightAdvanced view,
            IViewDialog dialog,
            ISettingsService settings,
            IViewUserSettingsRightAutoShownTabs rightAutoShownTabs,
            ISettingsServiceXmlSerialization settingsServiceXmlSerialization,
            IViewUserSettingsRightManuallyShownTabs rightManuallyShownTabs,
            Validate validate,
            FileData fileData
        )
        {
            _view = view;
            _dialog = dialog;
            _settings = settings;
            _rightAutoShownTabs = rightAutoShownTabs;
            _settingsServiceXmlSerialization = settingsServiceXmlSerialization;
            _rightManuallyShownTabs = rightManuallyShownTabs;
            _rightManuallyShownTabs = rightManuallyShownTabs;
            _validate = validate;
            _fileData = fileData;

            //subscribe to events   
            if (view != null) view.Load += View_Load;
        }

        private void View_Load(object sender, EventArgs e)
        {
            _view.DeleteClickMenu += View_DeleteClickMenu;
            _view.AddClickMenu += View_AddClickMenu;
            _view.ReplaceClickMenu += View_ReplaceClickMenu;
            _view.MouseUpListbox += View_MouseUpListbox;
            AddFilesToListboxFromSettings();
            _dialog.ClickOkMouseButton += Dialog_ClickOkMouseButton;
        }

        private void AddFilesToListboxFromSettings()
        {
            _view.ListBoxFiles.Items.Clear();
            _view.ListBoxFiles.Items.Add(("AutoShownTabs.xml"));
            _view.ListBoxFiles.Items.Add(("ManuallyShownTabs.xml"));
            _view.ListBoxFiles.Items.Add(("Macvendor.csv"));
            var files = _settings.AdvancedFiles.OfType<string>().ToArray().Select(Path.GetFileName).ToList();
            // ReSharper disable once CoVariantArrayConversion
            _view.ListBoxFiles.Items.AddRange(files.ToArray());
        }

        private void AddFilesToSettings(string filePath)
        {
            if (_settings.AdvancedFiles.Contains(filePath)) return;
            _settings.AdvancedFiles.Add(filePath);
            _settings.Save();
            AddFilesToListboxFromSettings();
        }

        private void DeleteFilesFromSettings(string filePath)
        {
            _settings.AdvancedFiles.Remove(filePath);
            _settings.Save();
            AddFilesToListboxFromSettings();
        }

        private int _location;

        private void View_MouseUpListbox(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            _location = _view.ListBoxFiles.IndexFromPoint(e.Location);
            if (_location != -1)
            {
                _view.ListBoxFiles.SelectedIndex = _location;
                if (_location == 0 || _location == 1 || _location == 2)
                {
                    _view.RightClickMenu.Items[0].Enabled = true; //add
                    _view.RightClickMenu.Items[1].Enabled = false; //delete
                    _view.RightClickMenu.Items[2].Enabled = true; //replace
                }
                else
                {
                    _view.RightClickMenu.Items[0].Enabled = true;
                    _view.RightClickMenu.Items[1].Enabled = true;
                    _view.RightClickMenu.Items[2].Enabled = false;
                }
            }
            else
            {
                _view.RightClickMenu.Items[0].Enabled = true;
                _view.RightClickMenu.Items[1].Enabled = false;
                _view.RightClickMenu.Items[2].Enabled = false;
            }

            _view.RightClickMenu.Show(new Point(Control.MousePosition.X + 12, Control.MousePosition.Y + 8));
        }

        private string _fileName;
        private string _destFilePath;

        private enum FileMethod
        {
            Add,
            Delete,
            Replace
        }

        private FileMethod _fileMethod;

        private void View_ReplaceClickMenu(object sender, EventArgs e)
        {
            _view.OpenFileDialog.Title = Resources
                .PresenterUserSettingsRightAdvanced__view_ReplaceClickMenu_Replace_XML_settings_File;
            switch (_view.ListBoxFiles.SelectedIndex)
            {
                case 0:
                    _view.OpenFileDialog.Filter = Resources
                        .PresenterUserSettingsRightAdvanced__view_ReplaceClickMenu_XML_file_AutoShownTabs_xml;
                    _view.OpenFileDialog.FileName = "AutoShownTabs.xml";
                    break;
                case 1:
                    _view.OpenFileDialog.Filter = Resources
                        .PresenterUserSettingsRightAdvanced__view_ReplaceClickMenu_XML_file_ManuallyShownTabs_xml;
                    _view.OpenFileDialog.FileName = "ManuallyShownTabs.xml";
                    break;
                case 2:
                    _view.OpenFileDialog.Filter = Resources.
                        PresenterUserSettingsRightAdvanced_View_ReplaceClickMenu_CSV_file___Macvendor_csvl;
                    _view.OpenFileDialog.FileName = "Macvendor.csv";
                    break;
            }

            _view.OpenFileDialog.InitialDirectory = @"C:\";
            var result = _view.OpenFileDialog.ShowDialog(); // Show the dialog.
            if (result != DialogResult.OK) return;
            _fileName = _view.OpenFileDialog.FileName;
            var fileNameWithExtension = Path.GetFileName(_view.OpenFileDialog.FileName);
            _destFilePath = _fileData.ExeFilePath + @"\" + fileNameWithExtension;
            ShowWarningDialog(fileNameWithExtension, FileMethod.Replace);
        }

        private void View_AddClickMenu(object sender, EventArgs e)
        {
            _view.OpenFileDialog.Title =
                Resources.PresenterUserSettingsRightAdvanced__view_AddClickMenu_Add_CSV_data_File;
            _view.OpenFileDialog.Filter =
                Resources.PresenterUserSettingsRightAdvanced__view_AddClickMenu_CSV_files___csv;
            _view.OpenFileDialog.FileName = "";
            _view.OpenFileDialog.InitialDirectory = @"C:\";
            var result = _view.OpenFileDialog.ShowDialog(); // Show the dialog.
            if (result != DialogResult.OK) return;
            _fileName = _view.OpenFileDialog.FileName;
            var fileNameWithExtension = Path.GetFileName(_view.OpenFileDialog.FileName);
            _destFilePath = _fileData.ExeFilePath + @"\" + fileNameWithExtension;
            ShowWarningDialog(fileNameWithExtension, FileMethod.Add);
        }

        private void View_DeleteClickMenu(object sender, EventArgs e)
        {
            _fileName = _fileData.ExeFilePath + @"\" + _view.ListBoxFiles.SelectedItem;
            _destFilePath = _fileName;
            var fileNameWithExtension = Path.GetFileName(_destFilePath);
            ShowWarningDialog(fileNameWithExtension, FileMethod.Delete);
        }

        private void ShowWarningDialog(string fileNameWithExtension, FileMethod fileMethod)
        {
            _fileMethod = fileMethod;
            switch (fileMethod)
            {
                case FileMethod.Add:
                    _dialog.SetText("Do you want to add " + fileNameWithExtension + "?");
                    break;
                case FileMethod.Delete:
                    _dialog.SetText("Do you want to delete " + fileNameWithExtension + "? This can't be cancelled!");
                    break;
                case FileMethod.Replace:
                    _dialog.SetText("Do you want to replace " + fileNameWithExtension + "? This can't be cancelled!");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileMethod), fileMethod, null);
            }

            _dialog.Tag = ToString();
            _dialog.UserInput.Hide();
            _dialog.Show();
        }

        private void Dialog_ClickOkMouseButton(object sender, EventArgs e)
        {
            if (_dialog.Tag.ToString() != ToString()) return;
            switch (_fileMethod)
            {
                case FileMethod.Add:
                    File.Copy(_fileName, _destFilePath, true);
                    AddFilesToSettings(_destFilePath);
                    break;
                case FileMethod.Delete:
                    File.Delete(_fileName);
                    DeleteFilesFromSettings(_destFilePath);
                    break;
                case FileMethod.Replace:
                    // ReSharper disable once NotAccessedVariable
                    switch (_view.ListBoxFiles.SelectedIndex)
                    {
                        case 0:
                            // ReSharper disable once RedundantAssignment
                            _ = new ValidateXml(_fileName, ValidateXml.XmlType.Auto);
                            //MessageBox.Show("ValidateXml.Passed=" + ValidateXml.Passed);
                            if (ValidateXml.Passed)
                            {
                                //MessageBox.Show("_fileName=" + _fileName);
                                //MessageBox.Show("_destFilePath=" + _destFilePath);
                                File.Copy(_fileName, _destFilePath, true);
                                UpdateAutoTab();
                                _settingsServiceXmlSerialization.AutoShownTabs =
                                    _settingsServiceXmlSerialization.AutoShownTabs; //To trigger ( PresenterMainSplContPanelUpTabs.SettingsServiceXmlSerialization_PropertyChanged) when new values
                            }
                            break;
                        // ReSharper disable once RedundantAssignment
                        case 1:
                            _ = new ValidateXml(_fileName, ValidateXml.XmlType.Manual);
                            if (ValidateXml.Passed)
                            {
                                //MessageBox.Show("_fileName=" + _fileName);
                                //MessageBox.Show("_destFilePath=" + _destFilePath);
                                File.Copy(_fileName, _destFilePath, true);
                                UpdateManualTab();
                                _settingsServiceXmlSerialization.ManuallyShownTabs =
                                    _settingsServiceXmlSerialization.ManuallyShownTabs; //To trigger ( PresenterMainSplContPanelUpTabs.SettingsServiceXmlSerialization_PropertyChanged) when new values
                            }
                            break;
                        case 2:
                            File.Copy(_fileName, _destFilePath, true);
                            break;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sender),
                        Resources.PresenterUserSettingsRightAdvanced_Dialog_ClickOkMouseButton_An_error_occured_while_Dialog_ClickOkMouseButton_);
            }
        }

        private void UpdateManualTab()
        {
            _validate.MergeNewXmlWithOldXmlManual(true);
            if (!_rightManuallyShownTabs.Loaded) return;
            _rightManuallyShownTabs.ListLeft.Items.Clear();
            foreach (var element in _rightManuallyShownTabs.ManuallyShownTabs.List)
                _rightManuallyShownTabs.ListLeft.Items.Add(element.Name);
        }

        private void UpdateAutoTab()
        {
            _validate.MergeNewXmlWithOldXmlAuto(true);
            if (!_rightAutoShownTabs.Loaded) return;
            _rightAutoShownTabs.RuleNames.Items.Clear();
            foreach (var element in _rightAutoShownTabs.Settings.List)
                _rightAutoShownTabs.RuleNames.Items.Add(element.Name);
            ClearContent();
            HideControls();
        }

        private void ClearContent()
        {
            _rightAutoShownTabs.RegExString.Clear();
            _rightAutoShownTabs.ItemsList.Clear();
            _rightAutoShownTabs.ListLeftSimulated.Clear();
        }

        private void HideControls()
        {
            _rightAutoShownTabs.Group2RegEx.Hide();
            _rightAutoShownTabs.Group3Items.Hide();
            _rightAutoShownTabs.Group4Help.Hide();
            _rightAutoShownTabs.GroupBoxRightResult.Hide();
            _rightAutoShownTabs.ListLeftSimulated.Hide();
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
        ~PresenterUserSettingsRightAdvanced()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        #endregion Dispose
    }
}