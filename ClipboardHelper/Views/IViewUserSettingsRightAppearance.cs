using ClipboardHelperRegEx.ModifiedControls;
using System;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.Views
{
    public interface IViewUserSettingsRightAppearance
    {
        TextBox ProgramsAlternativePasting { get; }
        CheckBox IsClosingEnabled { get; }
        CheckBox DoAutoStartProgram { get; }
        CheckBox IsFocusEnabled { get; }
        CheckBox IsCtrlEnabled { get; }
        CheckBox IsAltEnabled { get; }
        CheckBox IsShiftEnabled { get; }
        CheckBox IsWinEnabled { get; }
        AdvancedComboBox FadeTypeChoice { get; }
        AdvancedComboBox ShortcutKeysComboBox { get; }
        Label FadingText { get; }
        Label SecondsText { get; }
        TextBox Seconds { get; }

        bool Enabled { get; }
        bool Loaded { get; set; }
        AdvancedComboBox AppearanceColorChoices { get; }
        ColorDialog ColorDialog { get; }
        Label LabelTextColorSample { get; }
        TextBox Password { get; }
        Button ResetToFactorySettings { get; }
        event EventHandler Load;

        event EventHandler OnProgramsAlternativePastingTextChanged;
        event EventHandler SelectedItemChangedShortcutKeys;
        event EventHandler SelectedItemChangedFadeTypeChoice;
        event EventHandler CheckedChangedIsWinEnabled;
        event EventHandler CheckedChangedIsShiftEnabled;
        event EventHandler CheckedChangedIsAltEnabled;
        event EventHandler CheckedChangedIsCtrlEnabled;
        event EventHandler CheckedChangedIsFocusEnabled;
        event EventHandler CheckedChangedIsAutoStartEnabled;
        event EventHandler CheckedChangedIsHidingEnabled;
        event EventHandler TextChangedSeconds;
        event EventHandler EnabledChangedView;
        event EventHandler OnViewUserSettingsRightAppearanceVisibleChanged;

        event MouseEventHandler OnLabelTextColorSampleMouseClick;
        event EventHandler OnAppearanceColorChoicesSelectedIndexChanged;
    }
}