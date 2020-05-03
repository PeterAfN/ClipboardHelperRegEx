using System;

namespace ClipboardHelperRegEx.Views
{
    public interface IViewUserSettingsButtonsDown
    {
        event EventHandler Load;
        event EventHandler OkClicked;
        event EventHandler CancelClicked;
        event EventHandler ApplyClicked;
        void CreateControls();
        void AddControls();
        void SetStatusOfApplyButton();
        event EventHandler CancelIsClickedRestoreSettings;
        void OnCancelIsClickedRestoreSettings(EventArgs e);
        event EventHandler SaveIsClicked;
        void OnSaveIsClicked(EventArgs e);
    }
}