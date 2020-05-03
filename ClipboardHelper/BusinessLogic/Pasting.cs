using ClipboardHelperRegEx.Properties;
using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace ClipboardHelperRegEx.BusinessLogic
{
    public class PastingActivatedEventArgs : EventArgs
    {
    }

    public class PastingOccuredEventArgs : EventArgs
    {
    }

    public class PastingDeactivatedEventArgs : EventArgs
    {
        public new static readonly PastingDeactivatedEventArgs Empty = new PastingDeactivatedEventArgs();
    }

    public class Pasting
    {
        /// <summary>
        ///     List of programs that uses alternative console pasting method. This is a list of programs that don't accept pasting
        ///     with CTRL+V.
        /// </summary>
        //public static  List<string> ConsoleList = new List<string>();
        public static List<string> ConsoleList { get; } = new List<string>();

        private static string _cl = string.Empty;

        private static bool _vUp = true;
        private static bool _ctrlUp = true;

        private static readonly object LockerMouseWheel = new object();

        /// <summary>
        ///     Class to send simulated key strokes.
        /// </summary>
        private readonly InputSimulator _inputSimulator = new InputSimulator();

        /// <summary>
        ///     Class to receive events when keys are down or up. Detects keys globally in Windows.
        /// </summary>
        private IKeyboardMouseEvents _mGlobalHook;

        private bool _singleSelection;

        /// <summary>
        /// </summary>
        private List<string> _textList;

        public Pasting()
        {
            Subscribe();
            _textList = new List<string>();
        }

        /// <summary>
        ///     Has combination ctrl + v has been pressed?
        /// </summary>
        private static bool CtrlVDetected { get; set; }

        /// <summary>
        ///     Has ctrl been released before key V is released?
        /// </summary>
        private static bool CtrlUpBeforeVGotUp { get; set; }

        /// <summary>
        ///     Is sending of keys active?
        /// </summary>
        public static bool SendingKeys { get; private set; }

        /// <summary>
        ///     The last item that was pasted by the program.
        /// </summary>
        public string LastPastedItem { get; private set; }

        private ReceivingPrograms ReceivingProgram { get; set; }

        /// <summary>
        /// </summary>
        private ContentTypes ContentType { get; set; } = ContentTypes.NotList;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
        public List<string> TextList
        {
            get { return _textList; }
            set
            {
                if (value == null) return;
                _textList = value;
                InitiateOrUpdateValuesDependingOnNrOfItemsInList(value.Count, true);
            }
        }

        /// <summary>
        ///     Event when pasting is activated.
        /// </summary>
        public event EventHandler<PastingActivatedEventArgs> OnPastingActivated;

        /// <summary>
        ///     Event when pasting becomes not activated.
        /// </summary>
        public event EventHandler<PastingDeactivatedEventArgs> OnPastingDeactivated;

        /// <summary>
        ///     Event when pasting has occured in mode.
        /// </summary>
        public event EventHandler<PastingOccuredEventArgs> OnPastingOccured;

        /// <summary>
        ///     Cancels pasting
        /// </summary>
        public void Cancel()
        {
            if (TextList != null) TextList = null;
            Unsubscribe();
            OnPastingDeactivated?.Invoke(this, new PastingDeactivatedEventArgs());
        }

        private void GlobalHook_KeyDown(object sender, KeyEventArgs e)
        {
            switch (ContentType)
            {
                case ContentTypes.NotList
                    : //Ignore repeated pasting when only the LAST text item is left in the text list. This happens when ctrl+v is continuously pressed. 
                    if (e.KeyCode == Keys.V && TextList.Count == 0)
                        e.SuppressKeyPress = true;
                    break;
                case ContentTypes.List:
                    switch (SendingKeys)
                    {
                        case true:
                            switch (e.KeyCode)
                            {
                                case Keys.Packet:
                                case Keys.V:
                                case Keys.LControlKey:
                                case Keys.Enter:
                                    break;
                                default:
                                    e.SuppressKeyPress = true;
                                    break;
                            }

                            break;
                        case false:
                            switch (e.KeyCode)
                            {
                                case Keys.V when e.KeyCode == Keys.V && e.Control && _vUp:
                                    _vUp = false;
                                    _ctrlUp = false;
                                    e.SuppressKeyPress = true;
                                    if (IsActiveWindowInThisList(new List<string> { "Clipboard Helper" })) //This program.
                                    {
                                        ReceivingProgram = ReceivingPrograms.ThisProgram;
                                    }
                                    else if (IsActiveWindowInThisList(ConsoleList)
                                    ) //Specific programs with alternative pasting mode: MobaXterm; mRemoteNG; PuTTY; Windows PowerShell; Kommandotolken.
                                    {
                                        ReceivingProgram = ReceivingPrograms.Console;
                                        CtrlVDetected = true;
                                    }
                                    else //All other programs.
                                    {
                                        ReceivingProgram = ReceivingPrograms.Standard;
                                        _cl = GetTextForVirtualKeys();
                                        _cl = _cl.Replace("\n", "\r\n"); //So Notepad can handle line break.
                                        CtrlVDetected = true;
                                        LastPastedItem = _cl; //When we receive new clipboard value, we know we sent it.
                                        SetTextClipboard.Start(_cl);
                                        SendingKeys = true;
                                        _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.LCONTROL);
                                        _inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LCONTROL,
                                            VirtualKeyCode.VK_V);
                                        //prevents empty clipboard outputted when multi pasting.
                                        //This can happen if the user pastes, repeatedly, very fast.
                                        Thread.Sleep(3);
                                        if (TextList.Count != 0)
                                            _inputSimulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                                        SendingKeys = false;
                                        _inputSimulator.Keyboard.KeyDown(VirtualKeyCode.LCONTROL);
                                        OnPastingOccured?.Invoke(this, new PastingOccuredEventArgs());
                                        InitiateOrUpdateValuesDependingOnNrOfItemsInList(TextList.Count);
                                    }

                                    break;
                                case Keys.V:
                                    if (ContentType == ContentTypes.List && !_ctrlUp)
                                        //Ignore repeated pasting when NOT the last text item is left in the text list.
                                        //This happens when ctrl+v is continuously pushed down. 
                                        e.SuppressKeyPress = true;
                                    _vUp = false;
                                    break;
                                case Keys.LControlKey:
                                    _ctrlUp = false;
                                    break;
                                default:
                                    CtrlVDetected = false;
                                    break;
                            }

                            break;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sender),
                        Resources.Pasting_GlobalHook_KeyDown_An_error_occured_while_pasting);
            }
        }

        private void GlobalHook_KeyUp(object sender, KeyEventArgs e)
        {
            switch (ContentType)
            {
                case ContentTypes.NotList:
                    switch (e.KeyCode)
                    {
                        case Keys.V when SendingKeys:
                            e.SuppressKeyPress = true;
                            break;
                        case Keys.V when SendingKeys == false:
                            Unsubscribe();
                            OnPastingDeactivated?.Invoke(this, new PastingDeactivatedEventArgs());
                            Reset_V_and_Control();
                            break;
                    }

                    break;
                case ContentTypes.List:
                    switch (e.KeyCode)
                    {
                        case Keys.V when !SendingKeys:
                            _vUp = true;
                            break;
                        case Keys.LControlKey when !SendingKeys:
                            _ctrlUp = true;
                            break;
                    }

                    switch (ReceivingProgram)
                    {
                        case ReceivingPrograms.ThisProgram: //Don't paste to own window if this program has focus.
                            e.SuppressKeyPress = true;
                            break;
                        case ReceivingPrograms.Standard: //All other programs gets standard pasting.
                            break;
                        case ReceivingPrograms.Console
                            : //MobaXterm; mRemoteNG; PuTTY; Windows PowerShell; Kommandotolken.
                            switch (SendingKeys)
                            {
                                case true:
                                    switch (e.KeyCode)
                                    {
                                        case Keys.Packet:
                                        case Keys.LControlKey:
                                        case Keys.Enter:
                                            break;
                                        default:
                                            e.SuppressKeyPress = true;
                                            break;
                                    }

                                    break;
                                case false:
                                    switch (e.KeyCode)
                                    {
                                        case Keys.V when e.KeyCode == Keys.V && e.Control:
                                            CtrlVDetected = false;
                                            CtrlUpBeforeVGotUp = false;
                                            e.SuppressKeyPress = true;
                                            SendingKeys = true;
                                            _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.LCONTROL);
                                            SendVirtualKeys(GetTextForVirtualKeys());
                                            SendingKeys = false;
                                            _inputSimulator.Keyboard.KeyDown(VirtualKeyCode.LCONTROL);
                                            InitiateOrUpdateValuesDependingOnNrOfItemsInList(TextList.Count);
                                            break;
                                        case Keys.V when CtrlVDetected && CtrlUpBeforeVGotUp:
                                            CtrlVDetected = false;
                                            CtrlUpBeforeVGotUp = false;
                                            e.SuppressKeyPress = true;
                                            SendingKeys = true;
                                            SendVirtualKeys(GetTextForVirtualKeys());
                                            SendingKeys = false;
                                            InitiateOrUpdateValuesDependingOnNrOfItemsInList(TextList.Count);
                                            break;
                                        case Keys.LControlKey:
                                            CtrlUpBeforeVGotUp = true;
                                            SendingKeys = false;
                                            break;
                                    }

                                    break;
                            }

                            break;
                    }

                    break;
            }
        }

        private static void Reset_V_and_Control()
        {
            _vUp = true;
            _ctrlUp = true;
        }

        /// <summary>
        ///     Sends simulated key strokes. These are interpreted as real keystrokes by other programs.
        /// </summary>
        /// <param name="text"></param>
        private void SendVirtualKeys(string text)
        {
            var lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var i = 0;
            var count = lines.Length;
            foreach (var line in lines)
            {
                switch (ContentType)
                {
                    case ContentTypes.List when _singleSelection:
                        if (!string.IsNullOrEmpty(line))
                            _inputSimulator.Keyboard.TextEntry(line);
                        if (i + 1 != count) //not last line
                            _inputSimulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                        else //last line
                            OnPastingOccured?.Invoke(this, new PastingOccuredEventArgs());
                        break;
                    case ContentTypes.List when !_singleSelection:
                        if (!string.IsNullOrEmpty(line))
                            _inputSimulator.Keyboard.TextEntry(line);
                        _inputSimulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                        if (TextList != null)
                            if (i + 1 == count && TextList.Count != 0) //last line and not last selection
                                OnPastingOccured?.Invoke(this, new PastingOccuredEventArgs());
                            else if (i + 1 == count && TextList.Count == 0) //last line and last selection
                                OnPastingOccured?.Invoke(this, new PastingOccuredEventArgs());
                        break;
                }

                i += 1;
            }
        }

        private static bool IsActiveWindowInThisList(IReadOnlyCollection<string> programs)
        {
            if (programs.Count == 0)
                return false;
            var actW = GetForegroundProcessName();
            foreach (var program in programs)
            {
                if (string.IsNullOrEmpty(program)) continue;
                if (actW.ToLower(CultureInfo.CurrentCulture).Contains(program.ToLower(CultureInfo.CurrentCulture)))
                    return true;
            }

            return false;
        }

        private string GetTextForVirtualKeys()
        {
            switch (ContentType)
            {
                case ContentTypes.List:
                    if (TextList != null)
                        LastPastedItem = RemoveAndReturnFirstLine();
                    break;
                case ContentTypes.NotList:
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"An error occured while pasting");
            }

            return LastPastedItem;
        }

        private string RemoveAndReturnFirstLine()
        {
            try
            {
                LastPastedItem = TextList[0];
                TextList.RemoveAt(0);
                return LastPastedItem;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.Pasting_RemoveAndReturnFirstLine_An_error_occured_when_getting_the_next_item_to_paste__ + ex, Resources.Pasting_RemoveAndReturnFirstLine_Clipboard_Helper_error, MessageBoxButtons.OK);
                Cancel();
                //throw;
                return string.Empty;
            }
        }

        private void InitiateOrUpdateValuesDependingOnNrOfItemsInList(int nrOfPasteItems, bool initiate = false)
        {
            if (initiate)
            {
                if (nrOfPasteItems == 1 || nrOfPasteItems > 1)
                {
                    Subscribe();
                    OnPastingActivated?.Invoke(this, new PastingActivatedEventArgs());
                }

                _singleSelection = nrOfPasteItems == 1;
            }

            switch (nrOfPasteItems)
            {
                case 0:
                    SetTextClipboard.Start(LastPastedItem);
                    if (ContentType != ContentTypes.NotList)
                        ContentType = ContentTypes.NotList;
                    break;
                case 1:
                    if (ContentType != ContentTypes.List)
                        ContentType = ContentTypes.List;
                    break;
                default:
                    if (nrOfPasteItems > 1)
                        if (ContentType != ContentTypes.List)
                            ContentType = ContentTypes.List;
                    break;
            }
        }

        private void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead.
            if (_mGlobalHook != null) Unsubscribe(); //safety
            _mGlobalHook = Hook.GlobalEvents();
            _mGlobalHook.MouseWheel += GlobalHook_MouseWheel;
            _mGlobalHook.KeyDown += GlobalHook_KeyDown;
            _mGlobalHook.KeyUp += GlobalHook_KeyUp;
        }

        private void Unsubscribe()
        {
            _mGlobalHook.MouseWheel -= GlobalHook_MouseWheel;
            _mGlobalHook.KeyDown -= GlobalHook_KeyDown;
            _mGlobalHook.KeyUp -= GlobalHook_KeyUp;
            _mGlobalHook.Dispose();
        }

        private void GlobalHook_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!SendingKeys) return;
            //Thread Safe locker.
            var lockWasTaken = false;
            try
            {
                Monitor.Enter(LockerMouseWheel, ref lockWasTaken);
                //Because when we simulate ctrl is down (after we send virtualKeys) and the user scrolls the mouse wheel, this leads
                //to zooming in the active app. We don't want that, therefore we simulate ctrl up.
                _mGlobalHook.KeyDown -= GlobalHook_KeyDown;
                _mGlobalHook.KeyUp -= GlobalHook_KeyUp;
                _inputSimulator.Keyboard.KeyPress(VirtualKeyCode.LCONTROL);
                _mGlobalHook.KeyDown += GlobalHook_KeyDown;
                _mGlobalHook.KeyUp += GlobalHook_KeyUp;
            }
            finally
            {
                if (lockWasTaken) Monitor.Exit(LockerMouseWheel);
            }
        }

        // Returns the name of the process owning the foreground window.
        private static string GetForegroundProcessName()
        {
            var hwnd = NativeMethods.GetForegroundWindow();
            // The foreground window can be NULL in certain circumstances, 
            // such as when a window is losing activation.
            // ReSharper disable once AssignmentIsFullyDiscarded
            _ = NativeMethods.GetWindowThreadProcessId(hwnd, out var pid);
            foreach (var p in Process.GetProcesses())
                if (p.Id == pid)
                    return p.ProcessName;
            return "Unknown";
        }

        private enum ReceivingPrograms
        {
            Standard,
            Console,
            ThisProgram
        }

        /// <summary>
        /// </summary>
        private enum ContentTypes
        {
            NotList,
            List
        }
    }
}