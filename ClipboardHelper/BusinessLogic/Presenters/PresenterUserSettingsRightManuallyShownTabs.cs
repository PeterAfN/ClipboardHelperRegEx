using ClipboardHelperRegEx.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.BusinessLogic.Presenters
{
    public class PresenterUserSettingsRightManuallyShownTabs
    {
        private readonly IViewDialog _dialog;
        private readonly ISettingsServiceXmlSerialization _settingsServiceXmlSerialization;
        private readonly IViewUserSettingsRightManuallyShownTabs _view;
        private readonly IViewUserSettings _viewUserSettings;
        private readonly IViewUserSettingsButtonsDown _viewUserSettingsButtonsDown;

        private int _indexMouseDown;
        private string _selectedMenu = "";

        public PresenterUserSettingsRightManuallyShownTabs
        (
            IViewUserSettingsRightManuallyShownTabs manuallyShownTabs,
            IViewDialog dialog,
            IViewUserSettings viewUserSettings,
            IViewUserSettingsButtonsDown viewUserSettingsButtonsDown,
            ISettingsServiceXmlSerialization settingsServiceXmlSerialization
        )
        {
            _view = manuallyShownTabs;
            _dialog = dialog;
            _viewUserSettings = viewUserSettings;
            _viewUserSettingsButtonsDown = viewUserSettingsButtonsDown;
            _settingsServiceXmlSerialization = settingsServiceXmlSerialization;

            //subscribe to events
            if (_view != null)
            {
                _view.Load += OnLoadedManuallyShownTabs;
                _view.DragDropListBox += ManuallyShownTabs_DragDropListBox;
                _view.DragOverListBox += ManuallyShownTabs_DragOverListBox;
                _view.MouseDownListBox += ManuallyShownTabs_MouseDownListBox;
                _view.MouseUpListbox += ManuallyShownTabs_MouseUpListbox;
                _view.AddClickMenu += ManuallyShownTabs_AddClickMenu;
                _view.DeleteClickMenu += ManuallyShownTabs_DeleteClickMenu;
                _view.EditNameClickMenu += ManuallyShownTabs_EditNameClickMenu;
                _view.TxtChanged += ViewManuallyShownTabs_TxtChanged;
            }

            if (_dialog == null) return;
            _dialog.ClickCancelMouseButton += Dialog_ClickCancelMouseButton;
            _dialog.ClickOkMouseButton += Dialog_ClickOkMouseButton;
        }

        private void ViewUserSettingsButtonsDown_ApplyClicked(object sender, EventArgs e)
        {
            if (_view.Loaded)
                _settingsServiceXmlSerialization.ManuallyShownTabs = _view.ManuallyShownTabs;
        }

        private void ViewUserSettingsButtonsDown_CancelClicked(object sender, EventArgs e)
        {
            _view.ManuallyShownTabs.List.Clear();
            OnLoadedManuallyShownTabs(this, EventArgs.Empty);
        }

        private void IViewUserSettingsButtonsDown_OkClicked(object sender, EventArgs e)
        {
            if (_view.Loaded)
                _settingsServiceXmlSerialization.ManuallyShownTabs = _view.ManuallyShownTabs;
        }

        private void ViewManuallyShownTabs_TxtChanged(object sender, EventArgs e)
        {
            if (_view.ListLeft.SelectedIndex == -1) return;
            var position = _view.ListLeft.SelectedIndex;
            _view.ManuallyShownTabs.List[position].Lines.Clear();
            _view.ManuallyShownTabs.List[position].Lines.AddRange(_view.TextRight.Lines.ToList());
        }

        private int MeasureListboxItemMaxLength(ListBox listbox)
        {
            var outDataMax = listbox.Items.Cast<object>()
                .Select((t,
                    i) => TextRenderer.MeasureText(_view.ListLeft.Items[i]
                            .ToString(),
                        _view.ListLeft.Font)
                    .Width)
                .Concat(new[]
                {
                    0
                })
                .Max();
            return outDataMax + 30;
        }

        private void Dialog_ClickOkMouseButton(object sender, EventArgs e)
        {
            if (_dialog.Tag.ToString() == ToString()) //don't react to other dialog users
            {
                _dialog.UserInput.Show(); //if hidden when delete
                _viewUserSettings.Enabled = true;
                switch (_selectedMenu)
                {
                    case "Add":
                        var position = _view.ListLeft.SelectedIndex;
                        var item = _dialog.UserInput.Text;
                        if (position != -1)
                        {
                            _view.ListLeft.Items.Insert(position, item);
                            _view.ManuallyShownTabs.List.Insert(position,
                                new ManuallyShownTab
                                {
                                    Name = item,
                                    Lines = new List<string>
                                    {
                                        "Please add text here..."
                                    }
                                });
                        }
                        else
                        {
                            _view.ListLeft.Items.Add(item);
                            _view.ManuallyShownTabs.List.Add(new ManuallyShownTab
                            {
                                Name = item,
                                Lines = new List<string>
                                {
                                    "Please add text here..."
                                }
                            });
                        }

                        break;
                    case "EditName":
                        position = _view.ListLeft.SelectedIndex;
                        item = _dialog.UserInput.Text;
                        _view.ListLeft.Items[position] = _dialog.UserInput.Text;
                        _view.ManuallyShownTabs.List[position].Name = item;
                        break;
                    case "Delete":
                        if (_view.ManuallyShownTabs.List.Count == 1) return;
                        position = _view.ListLeft.SelectedIndex;
                        if (position != -1)
                        {
                            _view.ListLeft.Items.RemoveAt(position);
                            _view.ManuallyShownTabs.List.RemoveAt(position);
                        }

                        break;
                }

                _view.ListLeft.HorizontalExtent = MeasureListboxItemMaxLength(_view.ListLeft);
            }
        }

        private void Dialog_ClickCancelMouseButton(object sender, EventArgs e)
        {
            _dialog.UserInput.Show();
            _viewUserSettings.Enabled = true;
            _selectedMenu = "";
        }

        private void ManuallyShownTabs_AddClickMenu(object sender, EventArgs e)
        {
            _viewUserSettings.Enabled = false;
            _dialog.SetText("Please enter the name of the new template to be added:");
            _dialog.Tag = ToString();
            _dialog.Show();
            _selectedMenu = "Add";
        }

        private void ManuallyShownTabs_EditNameClickMenu(object sender, EventArgs e)
        {
            _viewUserSettings.Enabled = false;
            _dialog.SetText("Please enter the new template name for: " +
                            _view.ListLeft.Items[_view.ListLeft.SelectedIndex]);
            _dialog.Tag = ToString();
            _dialog.Show();
            _selectedMenu = "EditName";
        }

        private void ManuallyShownTabs_DeleteClickMenu(object sender, EventArgs e)
        {
            _viewUserSettings.Enabled = false;
            _dialog.SetText("Do you want to delete: " +
                            _view.ListLeft.Items[_view.ListLeft.SelectedIndex] + "?");
            _dialog.Tag = ToString();
            _dialog.UserInput.Hide();
            _dialog.Show();
            _selectedMenu = "Delete";
        }

        private void ManuallyShownTabs_MouseUpListbox(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            var location = _view.ListLeft.IndexFromPoint(e.Location);
            if (location != -1)
            {
                _view.ListLeft.SelectedIndex = location;
                _view.RightClickMenu.Items[1].Enabled = true;
                _view.RightClickMenu.Items[2].Enabled = true;
            }
            else
            {
                _view.RightClickMenu.Items[1].Enabled = false; //edit name
                _view.RightClickMenu.Items[2].Enabled = false; //delete
            }

            _view.RightClickMenu.Show(new Point(Control.MousePosition.X + 12, Control.MousePosition.Y + 8));
        }

        private void ManuallyShownTabs_MouseDownListBox(object sender, MouseEventArgs e)
        {
            _view.GroupBoxRight.Visible = false;
            _indexMouseDown = _view.ListLeft.SelectedIndex = _view.ListLeft.IndexFromPoint(e.X, e.Y);
            if (e.Button != MouseButtons.Left) return;
            if (Cursor.Current != null) _view.Cursor = new Cursor(Cursor.Current.Handle);
            Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y);
            if (_view.ListLeft.SelectedItem == null) return;
            _view.TextRight.Lines = _view.ManuallyShownTabs.List[_view.ListLeft.SelectedIndex].Lines.ToArray();
            _view.GroupBoxRight.Visible = true;
            _view.TextRight.Focus();
            _selectedMenu = "Edit";
            _view.ListLeft.DoDragDrop(_view.ListLeft.SelectedItem, DragDropEffects.Move);
        }

        private static void ManuallyShownTabs_DragOverListBox(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void ManuallyShownTabs_DragDropListBox(object sender, DragEventArgs e)
        {
            var point = _view.ListLeft.PointToClient(new Point(e.X,
                e.Y)); //dirty fix so stop cursor isn't shown briefly
            var index = _view.ListLeft.IndexFromPoint(point);
            if (index < 0) index = _view.ListLeft.Items.Count - 1;
            var data = _view.ListLeft.SelectedItem;

            _view.ListLeft.Items.Remove(data);
            _view.ListLeft.Items.Insert(index, data);
            Move(_indexMouseDown, index, _view.ManuallyShownTabs.List);
            _view.ListLeft.SelectedIndex = index;
        }

        private static void Move(int oldIndex, int newIndex, IList<ManuallyShownTab> list)
        {
            var item = list[oldIndex];
            list.RemoveAt(oldIndex);
            list.Insert(newIndex, item);
        }

        private void OnLoadedManuallyShownTabs(object sender, EventArgs e)
        {
            _view.Loaded = true;
            _viewUserSettings.SetGroupBoxText("ManuallyShownTabs:");
            _view.ManuallyShownTabs = _settingsServiceXmlSerialization.ManuallyShownTabs;
            _view.ListLeft.Items.Clear();
            foreach (var element in _view.ManuallyShownTabs.List) _view.ListLeft.Items.Add(element.Name);
            _view.ListLeft.HorizontalExtent = MeasureListboxItemMaxLength(_view.ListLeft);
            _viewUserSettingsButtonsDown.OkClicked += IViewUserSettingsButtonsDown_OkClicked;
            _viewUserSettingsButtonsDown.CancelClicked += ViewUserSettingsButtonsDown_CancelClicked;
            _viewUserSettingsButtonsDown.ApplyClicked += ViewUserSettingsButtonsDown_ApplyClicked;
        }
    }
}