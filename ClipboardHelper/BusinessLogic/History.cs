using System;
using System.Collections.Generic;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public class History
    {
        private int _navigationPosition;

        public enum UpdateMethod
        {
            LeftClick,
            RightClick,
            NewValue
        }

        public History
        (
            Action actionOnHistoryChanged,
            Action<bool> actionOnLeftEnabled,
            Action<bool> actionOnRightEnabled,
            Action<int, int> actionMirrorNavigationPosition
        )
        {
            ActionOnHistoryChanged = actionOnHistoryChanged;
            ActionOnLeftEnabled = actionOnLeftEnabled;
            ActionOnRightEnabled = actionOnRightEnabled;
            ActionMirrorNavigationPosition = actionMirrorNavigationPosition;
        }

        public List<string> Values { get; } = new List<string>();

        public int NavigationPosition
        {
            get { return _navigationPosition; }
            private set
            {
                if (NavigationPosition > value)
                    ActionMirrorNavigationPosition(NavigationPosition - 2, NavigationPositionAndId.Values[NavigationPosition - 2]);
                else
                    ActionMirrorNavigationPosition(NavigationPosition, NavigationPositionAndId.Values[NavigationPosition]);
                _navigationPosition = value;
            }
        }

        private SortedList<int, int> NavigationPositionAndId { get; } = new SortedList<int, int>();

        private Action ActionOnHistoryChanged { get; }
        private Action<bool> ActionOnLeftEnabled { get; }
        private Action<bool> ActionOnRightEnabled { get; }
        private Action<int, int> ActionMirrorNavigationPosition { get; }

        public void AddAndRefreshIconStatus(UpdateMethod updateMethods, string newValue = "")
        {
            switch (updateMethods)
            {
                case UpdateMethod.LeftClick:
                    HandleLeftClick();
                    break;
                case UpdateMethod.RightClick:
                    HandleRightClick();
                    break;
                case UpdateMethod.NewValue:
                    HandleNewValue(newValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(updateMethods), updateMethods, null);
            }
        }

        private void HandleLeftClick()
        {
            ActionOnRightEnabled(true);
            if (NavigationPosition == 2)
                ActionOnLeftEnabled(false);
            NavigationPosition -= 1;
            ActionOnHistoryChanged();
        }

        private void HandleRightClick()
        {
            ActionOnLeftEnabled(true);
            if (NavigationPosition == Values.Count - 1)
                ActionOnRightEnabled(false);
            NavigationPosition += 1;
            ActionOnHistoryChanged();
        }

        private void HandleNewValue(string newValue)
        {
            var random = new Random();
            if (NavigationPosition != NavigationPositionAndId.Count)
                RemoveRangeFromIndexToEnd(NavigationPosition);
            NavigationPositionAndId.Add(NavigationPosition, random.Next(0, 10000));
            Values.RemoveRange(NavigationPosition, Values.Count - NavigationPosition); //truncate from index
            Values.Insert(NavigationPosition, newValue);
            if (NavigationPosition != 0)
            {
                ActionOnLeftEnabled(true);
                ActionOnRightEnabled(false);
            }
            NavigationPosition += 1;
        }

        private void RemoveRangeFromIndexToEnd(int startIndex)
        {
            for (var x = NavigationPositionAndId.Count - 1; x >= startIndex; x--)
            {
                NavigationPositionAndId.RemoveAt(x);
            }
        }
    }
}