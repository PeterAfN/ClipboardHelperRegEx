using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public class ListboxMultiSelection
    {
        public delegate void DgEventRaiserEventHandler(object sender, EventArgs e);

        private bool _ctrlPressed;
        private int _mouseDownIndex;
        private int _mouseUpIndex;

        private int _posY = Cursor.Position.Y;
        private bool _shiftPressed;

        private int _shiftSelectionStartIndex;
        //public List<bool> SelectionOld;

        public List<bool> SelectionOld { get; private set; }

        public ListboxMultiSelection(ListBox listbox)
        {
            Listbox = listbox;
            SubscribeToEvents();
        }

        private ListBox Listbox { get; set; }
        //public static List<int> SelectedIndexesOrdered { get; set; } = new List<int>();
        //public static List<string> SelectedItemsOrdered { get; set; } = new List<string>();

        public static List<int> SelectedIndexesOrdered { get; private set; } = new List<int>();

        public static List<string> SelectedItemsOrdered { get; private set; } = new List<string>();

        /// <summary>
        ///     If selection is altered programmatically (and not by mouse) then this must be set.
        /// </summary>
        public int ThisIndexHasBeenChangedProgrammatically { get; set; } = -1;

        private void SubscribeToEvents()
        {
            UnsubscribeToEvents();
            Listbox.MouseDown += Listbox_MouseDown;
            Listbox.MouseUp += Listbox_MouseUp;
            Listbox.SelectedIndexChanged += Listbox_SelectedIndexChanged;
        }

        private void UnsubscribeToEvents()
        {
            if (Listbox != null) Listbox.MouseDown -= Listbox_MouseDown;
            if (Listbox != null) Listbox.MouseUp -= Listbox_MouseUp;
            if (Listbox != null) Listbox.SelectedIndexChanged -= Listbox_SelectedIndexChanged;
        }

        public void Cancel(bool unsubscribeToEvents = true)
        {
            if (unsubscribeToEvents)
            {
                UnsubscribeToEvents();
                Listbox = null;
            }

            SelectedIndexesOrdered?.Clear();
            SelectedItemsOrdered?.Clear();
            _mouseDownIndex = -1;
            _posY = -1;
            _ctrlPressed = false;
            _shiftPressed = false;
            _mouseUpIndex = -1;
            SelectionOld = null;
            _shiftSelectionStartIndex = -1;
        }

        /// <summary>
        ///     To be used when one selection is removed programmatically and not by mouse.
        /// </summary>
        private void Listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPressedModifierKeys();
            if (_ctrlPressed || _shiftPressed) return;
            var selectionCurrent = GetListboxItemsSelection(Listbox.Items.Count, Listbox.SelectedIndices);
            var selectionChangeType = DetermineSelectionChangeType(SelectionOld, selectionCurrent,
                ThisIndexHasBeenChangedProgrammatically, ThisIndexHasBeenChangedProgrammatically);
            SelectedIndexesOrdered = GetSelectedIndexesInOrder(selectionChangeType, selectionCurrent,
                SelectedIndexesOrdered, ThisIndexHasBeenChangedProgrammatically,
                ThisIndexHasBeenChangedProgrammatically, Listbox);
            if (SelectedItemsOrdered != null)
                if (SelectedItemsOrdered.Count != 0)
                    SelectedItemsOrdered.RemoveAt(0);
            SelectionOld = GetListboxItemsSelection(Listbox.Items.Count, Listbox.SelectedIndices);
        }

        public event DgEventRaiserEventHandler OnSelectionReady;
        public event DgEventRaiserEventHandler OnSelectionNotReady;

        /// <summary>
        ///     Handles only single selection with keyboard.
        /// </summary>
        public void HandleKeyUp(int selectedIndex)
        {
            OnSelectionNotReady?.Invoke(this, EventArgs.Empty);
            Listbox.SelectedIndexChanged -= Listbox_SelectedIndexChanged;
            Listbox.SelectedIndex = selectedIndex;
            if (Listbox.SelectedIndex == -1)
            {
                OnSelectionReady?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                _mouseDownIndex = selectedIndex;
                _mouseUpIndex = selectedIndex;
                SelectionOld = GetListboxItemsSelection(Listbox.Items.Count, Listbox.SelectedIndices);
                SelectedItemsOrdered?.Clear();
                SelectedItemsOrdered = new List<string> { Listbox.SelectedItem.ToString() };
                SelectedIndexesOrdered?.Clear();
                SelectedIndexesOrdered = new List<int> { Listbox.SelectedIndex };
                HandleSelectionChange();
                OnSelectionReady?.Invoke(this, EventArgs.Empty);
            }

            Listbox.SelectedIndexChanged += Listbox_SelectedIndexChanged;
        }

        private void Listbox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            Listbox.SelectedIndexChanged -= Listbox_SelectedIndexChanged;
            if (Listbox.Items.Count <= 0) return;
            OnSelectionNotReady?.Invoke(this, EventArgs.Empty);
            if (CheckIfThumbMouseButtonsHaveBeenPressed(e)) return;
            _mouseDownIndex = Listbox.IndexFromPoint(e.Location);
            //if selection starts beneath the listbox, we don't want anything selected,
            //since the selection is "buggy" and doesn't select as one would assume.
            if (_mouseDownIndex != -1) return;
            if (Listbox.SelectedIndex != -1) Listbox.SetSelected(Listbox.SelectedIndex, false);
            Listbox.Enabled = false;
            Listbox.Enabled = true;
        }

        /// <summary>
        ///     Handle selection when user lifts mouse button after selection.
        /// </summary>
        private void HandleSelectionChange()
        {
            try
            {
                Listbox.Enabled = false;
                SetPressedModifierKeys();
                var selectionCurrent = GetListboxItemsSelection(Listbox.Items.Count, Listbox.SelectedIndices);
                if (!AnyItemsSelected(selectionCurrent))
                {
                    Listbox.MouseDown -= Listbox_MouseDown;
                    Listbox.MouseDown += Listbox_MouseDown;
                    Listbox.Enabled = true;
                }
                else
                {
                    if (SpecialCase_CheckIfTooFastUSerMouseMovementWhenMouseUp(_mouseDownIndex, _mouseUpIndex,
                        selectionCurrent))
                    {
                        _mouseUpIndex = _mouseDownIndex;
                        Listbox.SelectedItems.Clear();
                        Listbox.SelectedIndex = _mouseUpIndex;
                    }

                    CorrectSelectionErrors(selectionCurrent);
                    var selectionChangeType = DetermineSelectionChangeType(SelectionOld, selectionCurrent,
                        _mouseDownIndex, _mouseUpIndex);
                    SelectedIndexesOrdered = GetSelectedIndexesInOrder(selectionChangeType, selectionCurrent,
                        SelectedIndexesOrdered, _mouseDownIndex, _mouseUpIndex, Listbox);
                    SelectedItemsOrdered =
                        StripSelectionToOutData(Listbox.Items.Cast<string>().ToList(), SelectedIndexesOrdered);
                    SelectionOld = selectionCurrent;
                    _ctrlPressed = false;
                    _shiftPressed = false;
                    Listbox.MouseDown -= Listbox_MouseDown;
                    Listbox.MouseDown += Listbox_MouseDown;
                    Listbox.Enabled = true;
                }
            }
            catch
            {
                SelectedIndexesOrdered = null;
                SelectedItemsOrdered = null;
                _mouseDownIndex = -1;
                _posY = -1;
                _ctrlPressed = false;
                _shiftPressed = false;
                _mouseUpIndex = -1;
                SelectionOld = null;
                _shiftSelectionStartIndex = -1;
                //throw;
            }
        }

        private void Listbox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            ThisIndexHasBeenChangedProgrammatically = -1;
            if (Listbox.SelectedIndex == -1) //true when nothing is selected
            {
            }
            else if (!CheckIfThumbMouseButtonsHaveBeenPressed(e))
            {
                if (Listbox.Items.Count > 0)
                {
                    Listbox.MouseDown -= Listbox_MouseDown;
                    _mouseUpIndex = Listbox.IndexFromPoint(Listbox.PointToClient(Cursor.Position));
                    HandleSelectionChange();
                    if (Listbox.SelectedIndex != -1) OnSelectionReady?.Invoke(this, EventArgs.Empty);
                }
            }

            Listbox.SelectedIndexChanged += Listbox_SelectedIndexChanged;
        }

        /// <summary>
        ///     Make correct when mouse up index an mouse down index doesn't correlate with selection.
        /// </summary>
        private void CorrectSelectionErrors(IReadOnlyList<bool> selectionCurrent)
        {
            if (_ctrlPressed || _shiftPressed) return;
            var differenceIndexDownUp = Math.Abs(_mouseUpIndex - _mouseDownIndex) + 1;
            var nrOfSelectedLines = 0;
            var first = 0;
            var firstIsSet = false;
            var last = 0;
            for (var i = 0; i < selectionCurrent.Count; i++)
            {
                if (!selectionCurrent[i]) continue;
                if (!firstIsSet)
                {
                    first = i;
                    firstIsSet = true;
                }

                last = i;
                nrOfSelectedLines += 1;
            }

            if (differenceIndexDownUp == nrOfSelectedLines) return;
            if (_mouseDownIndex > _mouseUpIndex)
            {
                _mouseDownIndex = last;
                _mouseUpIndex = first;
            }
            else
            {
                _mouseDownIndex = first;
                _mouseUpIndex = last;
            }
        }

        private static bool CheckIfThumbMouseButtonsHaveBeenPressed(MouseEventArgs e)
        {
            {
                return e?.Button is MouseButtons.XButton1 || e?.Button is MouseButtons.XButton2;
            }
        }

        /// <summary>
        ///     Create out data upon selection order
        /// </summary>
        /// <param name="toBeStripped"></param>
        /// <param name="indexesOut"></param>
        /// <returns></returns>
        private static List<string> StripSelectionToOutData(List<string> toBeStripped, IReadOnlyList<int> indexesOut)
        {
            if (indexesOut.Count <= 0) return new List<string>();
            if (indexesOut[0] == -1) return toBeStripped;
            var output = new List<string>();
            foreach (var t in indexesOut)
                try
                {
                    if (!(toBeStripped.Count < t))
                        output.Add(toBeStripped[t]);
                }
                catch (Exception)
                {
                    //throw;
                }

            return output;
        }

        /// <summary>
        ///     Returns the selection in order they were selected. Handles all combinations of selections with mouse click, ctrl
        ///     click, shift click and
        ///     combinations of them. It also supports selecting with dragging and selecting items. It DOES NOT support dragging +
        ///     ctrl or shift pressed.
        /// </summary>
        /// <param name="selectionType"></param>
        /// <param name="itemsSelection"></param>
        /// <param name="selectedIndexesOrderedOld"></param>
        /// <param name="mouseDownIndex"></param>
        /// <param name="mouseUpIndex"></param>
        /// <param name="listbox"></param>
        /// <returns></returns>
        private List<int> GetSelectedIndexesInOrder(HowSelectionHasChanged selectionType,
            IReadOnlyCollection<bool> itemsSelection, List<int> selectedIndexesOrderedOld, int mouseDownIndex,
            int mouseUpIndex, ListBox listbox)
        {
            var output = new List<int>();
            switch (selectionType)
            {
                case HowSelectionHasChanged.Error: break;
                case HowSelectionHasChanged.NoChange:
                    output = selectedIndexesOrderedOld;
                    break;
                case HowSelectionHasChanged.WithClick:
                    SelectedIndexesOrdered?.Clear();
                    output.Add(mouseUpIndex);
                    _shiftSelectionStartIndex = mouseUpIndex;
                    break;
                case HowSelectionHasChanged.WithClickDrag:
                    mouseUpIndex =
                        SpecialCase_CheckIfDragSelectionEndedOutsideFormAndCorrectMouseUpIndex(mouseUpIndex,
                            itemsSelection, listbox);
                    mouseDownIndex =
                        SpecialCase_CheckIfDragSelectionStartedOutsideFormAndCorrectMouseDownIndex(mouseDownIndex,
                            itemsSelection, listbox);
                    if (mouseDownIndex < mouseUpIndex)
                        output.AddRange(MakeSelection(mouseDownIndex, mouseUpIndex, true));
                    else if (mouseDownIndex > mouseUpIndex)
                        output.AddRange(MakeSelection(mouseUpIndex, mouseDownIndex, false));
                    else output.Add(mouseDownIndex);
                    _shiftSelectionStartIndex = mouseDownIndex;
                    break;
                case HowSelectionHasChanged.IncreaseWithCtrLandClick:
                case HowSelectionHasChanged.DecreaseWithCtrLandClick:
                    output.AddRange(IndexSelectOrDeselect(selectedIndexesOrderedOld, mouseUpIndex));
                    _shiftSelectionStartIndex = mouseUpIndex;
                    break;
                case HowSelectionHasChanged.WithCtrLandClickDrag:
                case HowSelectionHasChanged.WithShiftAndClickDrag:
                case HowSelectionHasChanged.WithShiftAndCtrlClickDrag:
                    output.AddRange(selectedIndexesOrderedOld);
                    RevertSelection(ref listbox, selectedIndexesOrderedOld);
                    SelectedIndexesOrdered.Clear();
                    break;
                case HowSelectionHasChanged.WithShiftAndClick:
                    if (_shiftSelectionStartIndex < mouseUpIndex)
                        output.AddRange(MakeSelection(_shiftSelectionStartIndex, mouseUpIndex, true));
                    else if (_shiftSelectionStartIndex > mouseUpIndex)
                        output.AddRange(MakeSelection(mouseUpIndex, _shiftSelectionStartIndex, false));
                    listbox.SetSelected(_shiftSelectionStartIndex,
                        true); //this is not set if item earlier deselected with ctrl. 
                    break;
                case HowSelectionHasChanged.WithShiftAndCtrlClick:
                    output.AddRange(selectedIndexesOrderedOld);
                    if (_shiftSelectionStartIndex < mouseUpIndex)
                        output.AddRange(MakeSelection(_shiftSelectionStartIndex + 1, mouseUpIndex,
                            true)); //+1 is because it is added already
                    else if (_shiftSelectionStartIndex > mouseUpIndex)
                        output.AddRange(MakeSelection(mouseUpIndex, _shiftSelectionStartIndex - 1,
                            false)); //-1 is because it is added already
                    listbox.SetSelected(_shiftSelectionStartIndex,
                        true); //this is not set if item earlier deselected with ctrl.              
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(selectionType), selectionType, null);
            }

            return output;
        }


        /// <summary>
        ///     Corrects when the user mouse up listbox index isn't correct due the user moves the mouse too fast for the program
        ///     to detect.
        ///     It checks if only one item is selected. If so, then the mouse down and mouse up index should be the same. If not
        ///     the same, true is returned.
        /// </summary>
        /// <param name="mouseDownIndex"></param>
        /// <param name="mouseUpIndex"></param>
        /// <param name="itemsSelection"></param>
        /// <returns></returns>
        private static bool SpecialCase_CheckIfTooFastUSerMouseMovementWhenMouseUp(int mouseDownIndex, int mouseUpIndex,
            IEnumerable<bool> itemsSelection)
        {
            bool output;
            if (itemsSelection.Count(c => c) == 1 && mouseDownIndex != mouseUpIndex)
                output = true; //is nr of selected items exactly one and mouse up and down isn't the same?
            else output = false;
            return output;
        }

        private int SpecialCase_CheckIfDragSelectionEndedOutsideFormAndCorrectMouseUpIndex(int mouseUpIndex,
            IReadOnlyCollection<bool> itemsSelection,
            Control listbox)
        {
            int output;
            //if mouseUpIndex is -1, then the user selection has ended outside the form.
            if (mouseUpIndex == -1)
            {
                //DETERMINE WHETHER THE SELECTION ENDED ABOVE THE FORM OR BENEATH IT.
                //posY is the y-coordinate where the mouse was released outside the form
                if (_posY < listbox.PointToScreen(Point.Empty).Y)
                    output = 0; //return first index
                else output = itemsSelection.Count - 1; //return last index  
            }
            else
            {
                output = mouseUpIndex; //no change
            }

            return output;
        }

        //this should be obsolete, since we already disable selection when it's started beneath.
        private int SpecialCase_CheckIfDragSelectionStartedOutsideFormAndCorrectMouseDownIndex(int mouseDownIndex,
            IReadOnlyCollection<bool> itemsSelection,
            Control listbox)
        {
            int output;
            //if mouseDownIndex is -1, then the user selection has ended outside the form.
            if (mouseDownIndex == -1)
            {
                //DETERMINE WHETHER THE SELECTION ENDED ABOVE THE FORM OR BENEATH IT.
                //posY is the y-coordinate where the mouse was released outside the form
                if (_posY > listbox.PointToScreen(Point.Empty).Y)
                    output = itemsSelection.Count - 1; //return last index 
                else output = 0; //return first index 
            }
            else
            {
                output = mouseDownIndex; //no change
            }

            return output;
        }

        private static void RevertSelection(ref ListBox listbox, IEnumerable<int> selectionIndex)
        {
            listbox.ClearSelected();
            foreach (var t in selectionIndex)
                if (t > -1)
                    listbox.SetSelected(t, true);
        }

        private static IEnumerable<int> IndexSelectOrDeselect(IList<int> selectedIndexes, int indexAddOrDel)
        {
            var output = new List<int>();
            if (selectedIndexes != null)
            {
                if (selectedIndexes.IndexOf(indexAddOrDel) != -1)
                    selectedIndexes.RemoveAt(selectedIndexes.IndexOf(indexAddOrDel));
                else
                    selectedIndexes.Add(indexAddOrDel);
                output.AddRange(selectedIndexes);
            }
            else
            {
                output.Add(indexAddOrDel);
            }

            return output;
        }

        /// <summary>
        ///     Selects items between an interval and returns the updated information.
        /// </summary>
        /// <param name="lowerIndex"></param>
        /// <param name="higherIndex"></param>
        /// <param name="increasingSelection"></param>
        /// <returns></returns>
        private static IEnumerable<int> MakeSelection(int lowerIndex, int higherIndex, bool increasingSelection)
        {
            var output = new List<int>();
            if (increasingSelection)
                for (var i = lowerIndex; i <= higherIndex; i++)
                    output.Add(i);
            else
                for (var i = higherIndex; i >= lowerIndex; i--)
                    output.Add(i);
            return output;
        }

        /// <summary>
        ///     Sets which modifier keys were pressed. If pressed, the value in bool "ShiftPressed" and bool "CtrlPressed", are set
        ///     to "true"
        /// </summary>
        private void SetPressedModifierKeys()
        {
            if (ThisIndexHasBeenChangedProgrammatically > -1
            ) //ctrl is down but that is due to pasting with ctrl + v. When we paste we set ThisIndexHasBeenChangedProgrammatically
            {
                _shiftPressed = false;
                _ctrlPressed = false;
            }
            else
            {
                // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
                switch (Control.ModifierKeys)
                {
                    case Keys.Control | Keys.Shift:
                        _shiftPressed = true;
                        _ctrlPressed = true;
                        break;
                    case Keys.Control:
                        _ctrlPressed = true;
                        break;
                    case Keys.Shift:
                        _shiftPressed = true;
                        break;
                }
            }
        }

        /// <summary>
        ///     Returns the selection type. Determines if modifier key have been used and which modifier key have been used.
        /// </summary>
        /// <param name="listOld"></param>
        /// <param name="listCurrent"></param>
        /// <param name="mouseDownIndex"></param>
        /// <param name="mouseUpIndex"></param>
        /// <returns></returns>
        private HowSelectionHasChanged DetermineSelectionChangeType(IReadOnlyList<bool> listOld,
            IReadOnlyList<bool> listCurrent,
            int mouseDownIndex,
            int mouseUpIndex)
        {
            var output = HowSelectionHasChanged.NoChange;
            var changeCounter = 0;
            switch (listOld)
            {
                case null when listCurrent == null:
                    return HowSelectionHasChanged.NoChange; //if null nothing have changed
                case null when _shiftPressed && _ctrlPressed:
                    return HowSelectionHasChanged
                        .WithShiftAndCtrlClick; //when shift+ctrl on null (we want the location of shift later)
                case null when _shiftPressed:
                    return HowSelectionHasChanged
                        .WithShiftAndClick; //when shift on null (we want the location of shift later)
                case null when _ctrlPressed:
                    return HowSelectionHasChanged.IncreaseWithCtrLandClick; //when ctrl on null
                case null when !_ctrlPressed && !_shiftPressed && mouseDownIndex != mouseUpIndex:
                    return HowSelectionHasChanged.WithClickDrag; //when mouse/key-dragging on null
                case null:
                    return HowSelectionHasChanged.WithClick; //when mouse/key-dragging on null
            }

            if (mouseDownIndex != mouseUpIndex && _shiftPressed && _ctrlPressed)
                return HowSelectionHasChanged.WithShiftAndCtrlClickDrag; //when mouse/key-dragging with ctrl and shift
            if (mouseDownIndex != mouseUpIndex && _shiftPressed)
                return HowSelectionHasChanged.WithShiftAndClickDrag; //when mouse/key-dragging with shift
            if (mouseDownIndex != mouseUpIndex && _ctrlPressed)
                return HowSelectionHasChanged.WithCtrLandClickDrag; //when mouse/key-dragging with ctrl
            if (mouseDownIndex != mouseUpIndex) return HowSelectionHasChanged.WithClickDrag; //when mouse/key-dragging.
            if (listOld.All(x => x == false) && _shiftPressed && _ctrlPressed)
                return HowSelectionHasChanged.WithShiftAndCtrlClick; //when shift+shift on nothing selected
            if (listOld.All(x => x == false) && _shiftPressed)
                return HowSelectionHasChanged.WithShiftAndClick; //when shift on nothing selected
            if (listOld.All(x => x == false) && _ctrlPressed)
                return HowSelectionHasChanged.IncreaseWithCtrLandClick; //when ctrl on nothing selected
            if (listOld.All(x => x == false) && !_shiftPressed && !_ctrlPressed)
                return HowSelectionHasChanged.WithClick; //when mouse/key-click on nothing selected
            if (listOld.All(x => x == false) && !_shiftPressed && !_ctrlPressed && mouseDownIndex != mouseUpIndex)
                return HowSelectionHasChanged.WithClickDrag; //when mouse/key-drag on nothing selected
            for (var i = 0; i < listOld.Count; i++)
            {
                if (i >= listCurrent.Count) continue;
                if (listOld[i] == listCurrent[i]) continue;
                changeCounter += 1;
                if (changeCounter != 1) continue;
                if (_shiftPressed && _ctrlPressed) output = HowSelectionHasChanged.WithShiftAndCtrlClick;
                else if (_shiftPressed) output = HowSelectionHasChanged.WithShiftAndClick;
                else if (listCurrent[i]) output = HowSelectionHasChanged.IncreaseWithCtrLandClick;
                else if (!listCurrent[i]) output = HowSelectionHasChanged.DecreaseWithCtrLandClick;
            }

            if (changeCounter != 0 && changeCounter < 2) return output;
            if (_shiftPressed && _ctrlPressed) output = HowSelectionHasChanged.WithShiftAndCtrlClick;
            else if (_shiftPressed) output = HowSelectionHasChanged.WithShiftAndClick;
            else if (!_shiftPressed && !_ctrlPressed) output = HowSelectionHasChanged.WithClick;
            return output;
        }

        /// <summary>
        ///     Returns a list with values "true" if the items are selected and false if the items are not selected.
        /// </summary>
        /// <param name="itemsCount">Number of items the have</param>
        /// <param name="selectedIndices">List with selected indexes number</param>
        /// <returns></returns>
        private static List<bool> GetListboxItemsSelection(int itemsCount,
            ListBox.SelectedIndexCollection selectedIndices)
        {
            var selectionItems = new List<bool>(itemsCount);
            for (var i = 0; i < itemsCount; i++) selectionItems.Add(selectedIndices.Contains(i));
            return selectionItems;
        }

        /// <summary>
        ///     Check if any items are selected.
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        private static bool AnyItemsSelected(IEnumerable<bool> lines)
        {
            return lines.Any(line => line);
        }

        /// <summary>
        ///     Different types of listbox selection. Some are not allowed but needs to be detected.
        /// </summary>
        private enum HowSelectionHasChanged
        {
            Error,
            NoChange,
            IncreaseWithCtrLandClick,
            DecreaseWithCtrLandClick,
            WithCtrLandClickDrag,
            WithClick,
            WithClickDrag,
            WithShiftAndClick,
            WithShiftAndClickDrag,
            WithShiftAndCtrlClick,
            WithShiftAndCtrlClickDrag
        }
    }
}