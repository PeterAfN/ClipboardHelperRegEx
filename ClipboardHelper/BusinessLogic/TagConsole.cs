using System;
using System.Diagnostics;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public static class TagConsole
    {
        public static string Handle(string line, Tags.UsedIn usedIn)
        {
            switch (usedIn)
            {
                case Tags.UsedIn.MainDisplay:
                case Tags.UsedIn.Settings:
                    return "ConsoleOpen(" + line + ")";
                case Tags.UsedIn.SingleSelection:
                    OpenConsole(line);
                    break;
                case Tags.UsedIn.NestedTags:
                case Tags.UsedIn.Pasting:
                    return string.Empty;
                default:
                    throw new ArgumentOutOfRangeException(nameof(usedIn), usedIn, null);
            }

            return "Error while parsing";
        }

        private static void OpenConsole(string commands)
        {
            Base64EncodedCommand(commands);
        }

        private static void Base64EncodedCommand(string commands)
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var psCommand = @"cd " + "\"" + desktopPath + "\"; " + commands;
            var psCommandBytes = System.Text.Encoding.Unicode.GetBytes(psCommand);
            var psCommandBase64 = Convert.ToBase64String(psCommandBytes);
            var startInfo = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -noexit -ExecutionPolicy unrestricted -EncodedCommand {psCommandBase64}",
                UseShellExecute = false
            };
            Process.Start(startInfo);
        }
    }
}