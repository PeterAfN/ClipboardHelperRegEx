using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClipboardHelperRegEx.BusinessLogic
{
    //used from http://www.blackwasp.co.uk/INotifyPropertyChangedExt.aspx
    public static class ExtensionINotifyPropertyChanged
    {
        public static void Notify(
            this INotifyPropertyChanged sender,
            PropertyChangedEventHandler handler,
            [CallerMemberName] string propertyName = "")
        {
            if (handler == null) return;
            var args = new PropertyChangedEventArgs(propertyName);
            handler(sender, args);
        }
    }
}