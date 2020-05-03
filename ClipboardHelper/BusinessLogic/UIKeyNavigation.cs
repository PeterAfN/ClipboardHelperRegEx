using MetroFramework.Controls;
using System;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public class UIKeyNavigation
    {
        public delegate void DgEventRaiserEventHandler(object sender, EventArgs e);

        private readonly MetroTabControl _metroTabControl;

        private DateTime _lastKeyPress;
        private ListBox _listBox;
        private string _searchString;

        private int _sI;

        public UIKeyNavigation(ListBox listBox, MetroTabControl metroTabControl)
        {
            _listBox = listBox;
            _metroTabControl = metroTabControl;
            Subscribe();
        }

        public event DgEventRaiserEventHandler OnAnyDigitKeyUp;
        public event DgEventRaiserEventHandler OnNavigationUp;
        public event DgEventRaiserEventHandler OnNavigationDown;

        public void Cancel()
        {
            Unsubscribe();
            _listBox = null;
        }

        private void Subscribe()
        {
            Unsubscribe();
            if (_listBox == null) return;
            _listBox.KeyDown += ListBox_KeyDown;
            _listBox.KeyPress += ListBox_KeyPress;
            _listBox.KeyUp += ListBox_KeyUp;
        }

        private void Unsubscribe()
        {
            if (_listBox == null) return;
            _listBox.KeyDown -= ListBox_KeyDown;
            _listBox.KeyPress -= ListBox_KeyPress;
            _listBox.KeyUp -= ListBox_KeyUp;
        }

        private void ListBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.XButton1:
                    break;
                case Keys.XButton2:
                    break;
                case Keys.Escape:
                    //This global key is handled by OnEscapePressed() in PresenterMainSplContPanelUpTabs.
                    break;
                case Keys.Left:
                    HandleNavigationLeftKey();
                    break;
                case Keys.Right:
                    HandleNavigationRightKey();
                    break;
                case Keys.Up:
                    HandleNavigationUpKey();
                    break;
                case Keys.Down:
                    HandleNavigationDownKey();
                    break;
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    return; //wont reach end, suppresses key. Handle key in KeyUp.
            }

            e.SuppressKeyPress = true;
            e.Handled = true;
        }

        private void ListBox_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    OnAnyDigitKeyUp?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }

        private void ListBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //used and modified from http://stackoverflow.com/questions/12929984/select-listbox-item-using-the-keyboard
            var newDate = DateTime.Now;
            var diff = newDate - _lastKeyPress;
            if (diff.TotalSeconds >= 0.5)
                _searchString = string.Empty;
            _searchString += e.KeyChar;
            _listBox.ClearSelected();
            _listBox.SelectedIndex = _listBox.FindString(_searchString, 0);
            _lastKeyPress = newDate;
            e.Handled = true;
        }

        private void HandleNavigationDownKey()
        {
            _sI = _listBox.SelectedIndex;
            if (_sI < _listBox.Items.Count - 1)
            {
                OnNavigationDown?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                _listBox.ClearSelected();
                if (_sI != -1) _listBox.SetSelected(_sI, true);
            }
        }

        private void HandleNavigationUpKey()
        {
            _sI = _listBox.SelectedIndex;
            if (_sI > 0)
            {
                OnNavigationUp?.Invoke(this, EventArgs.Empty);
            }
            else if (_sI == 0)
            {
                _listBox.ClearSelected();
                if (_sI != -1) _listBox.SetSelected(_sI, true);
            }
        }

        private void HandleNavigationRightKey()
        {
            if (_metroTabControl.SelectedIndex == _metroTabControl.TabCount - 1)
            {
                //reach the end, do nothing.
            }
            else
            {
                _metroTabControl.SelectedIndex += 1;
            }
        }

        private void HandleNavigationLeftKey()
        {
            if (_metroTabControl.SelectedIndex == 0)
            {
            }
            else
            {
                _metroTabControl.SelectedIndex -= 1;
            }
        }
    }
}