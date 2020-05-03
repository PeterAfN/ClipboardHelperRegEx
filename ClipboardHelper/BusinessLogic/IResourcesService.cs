using System.Drawing;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public interface IResourcesService
    {
        Image Closed { get; }
        Image ClosedSelected { get; }
        Image Locked { get; }
        Image LockedSelected { get; }
        Image MinimizeSelected { get; }
        Image MinimizeUnselected { get; }
        Image Unlocked { get; }
        Image UnlockedSelected { get; }
        Icon ActivatedForDarkTheme { get; }
        Icon DeactivatedForDarkTheme { get; }
        Icon ActivatedForLightTheme { get; }
        Icon DeactivatedForLightTheme { get; }
        Icon DynamicIcon1ForDarkTheme { get; }
        Icon DynamicIcon2ForDarkTheme { get; }
        Icon DynamicIcon3ForDarkTheme { get; }
        Icon DynamicIcon4ForDarkTheme { get; }
        Icon DynamicIcon5ForDarkTheme { get; }
        Icon DynamicIcon6ForDarkTheme { get; }
        Icon DynamicIcon7ForDarkTheme { get; }
        Icon DynamicIcon8ForDarkTheme { get; }
        Icon DynamicIcon1ForLightTheme { get; }
        Icon DynamicIcon2ForLightTheme { get; }
        Icon DynamicIcon3ForLightTheme { get; }
        Icon DynamicIcon4ForLightTheme { get; }
        Icon DynamicIcon5ForLightTheme { get; }
        Icon DynamicIcon6ForLightTheme { get; }
        Icon DynamicIcon7ForLightTheme { get; }
        Icon DynamicIcon8ForLightTheme { get; }
    }
}