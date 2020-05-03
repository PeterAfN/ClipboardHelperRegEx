using ClipboardHelperRegEx.Properties;
using NHotkey;
using NHotkey.WindowsForms;
using System;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public class ShortcutKeys
    {
        private bool KeyAlreadySigned { get; set; }
        private Keys NewKeyComb { get; set; } = Keys.None;
        private Action Action { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="name">
        ///     A unique name for the key combination.
        ///     Use this same unique name when removing the key combination with
        ///     method RemoveShortcutKey.
        /// </param>
        /// <param name="keys">The key combination to trigger the provided action</param>
        /// <param name="action">The action to trigger when key combination pressed.</param>
        public void AddShortcutKey(string name, Keys keys, Action action)
        {
            while (true)
            {
                Action = action;
                NewKeyComb = keys;
                if (KeyAlreadySigned == false)
                {
                    KeyAlreadySigned = true;
                    try
                    {
                        HotkeyManager.Current.AddOrReplace(name, NewKeyComb, OnShortCutKeysPressed);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(
                            Resources
                                .ShortcutKeys_AddShortcutKey_Couldn_t_set_Shortcut_key__Another_app_is_probably_using_it__Please_change_the_key_combination_,
                            Resources.ShortcutKeys_AddShortcutKey_ClipboardHelper);
                    }
                }
                else if (KeyAlreadySigned)
                {
                    RemoveShortcutKey(name);
                    name = "AppShortcutKey";
                    keys = NewKeyComb;
                    continue;
                }

                break;
            }
        }

        /// <summary>
        ///     Is run when shortcut keys are pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnShortCutKeysPressed(object sender, HotkeyEventArgs e)
        {
            Action();
            e.Handled = true;
        }

        /// <summary>
        ///     Removes the shortCutKey. Returns true if the shortcut key was removed.
        /// </summary>
        /// <param name="name"></param>
        private void RemoveShortcutKey(string name)
        {
            if (KeyAlreadySigned != true) return;
            HotkeyManager.Current.Remove(name);
            KeyAlreadySigned = false;
        }
    }
}