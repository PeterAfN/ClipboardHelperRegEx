using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public class History
    {
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
            Action<bool> actionOnRightEnabled
        )
        {
            ActionOnHistoryChanged = actionOnHistoryChanged;
            ActionOnLeftEnabled = actionOnLeftEnabled;
            ActionOnRightEnabled = actionOnRightEnabled;
        }

        public List<string> Values { get; } = new List<string>();

        public int NavigationPosition { get; private set; } = -1;

        public SortedList<int, int> NavigationPositionAndId { get; } = new SortedList<int, int>();

        private Action ActionOnHistoryChanged { get; }
        private Action<bool> ActionOnLeftEnabled { get; }
        private Action<bool> ActionOnRightEnabled { get; }

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
            if (NavigationPosition == 1)
                ActionOnLeftEnabled(false);
            NavigationPosition -= 1;
            ActionOnHistoryChanged();
        }

        private void HandleRightClick()
        {
            ActionOnLeftEnabled(true);
            if (NavigationPosition == Values.Count - 2)
                ActionOnRightEnabled(false);
            NavigationPosition += 1;
            ActionOnHistoryChanged();
        }

        private void HandleNewValue(string newValue)
        {
            NavigationPosition += 1;
            var random = new Random();
            if (NavigationPosition != -1)
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