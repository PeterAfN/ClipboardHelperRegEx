using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Template
{
    public class FormLostFocusToOutsideApp : EventArgs { }

    public partial class FormTemplate : Form
    {
        protected FormTemplate()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            TopLevel = true;
            VisibilityFormIcon1(true);
            VisibilityFormIcon2(true);
            VisibilityFormIcon3(true);
            VisibilityFormIcon4(false);
            VisibilityFormIcon5(false);
            CreateEvents();
        }

        public bool FormResizable { get; set; } = true;
        public bool FormMovable { get; set; } = true;
        private bool FormShadowShown { get; } = true;
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public bool FormGetsFocus { get; set; } = true;
        protected bool FormResizing { get; private set; } = true;
        public bool SnapToScreenEdge { get; set; } = true;

        public event EventHandler MouseEntersFormIcon1;
        private void OnMouseEntersFormIcon1(object sender, EventArgs e) => MouseEntersFormIcon1?.Invoke(this, new EventArgs());
        public event EventHandler MouseLeavesFormIcon1;
        private void OnMouseLeavesFormIcon1(object sender, EventArgs e) => MouseLeavesFormIcon1?.Invoke(this, new EventArgs());
        public event EventHandler MouseClicksFormIcon1;
        private void OnMouseClicksFormIcon1(object sender, EventArgs e) => MouseClicksFormIcon1?.Invoke(this, new EventArgs());
        public event EventHandler MouseEntersFormIcon2;
        private void OnMouseEntersFormIcon2(object sender, EventArgs e) => MouseEntersFormIcon2?.Invoke(this, new EventArgs());
        public event EventHandler MouseLeavesFormIcon2;
        private void OnMouseLeavesFormIcon2(object sender, EventArgs e) => MouseLeavesFormIcon2?.Invoke(this, new EventArgs());
        public event EventHandler MouseClicksFormIcon2;
        private void OnMouseClicksFormIcon2(object sender, EventArgs e) => MouseClicksFormIcon2?.Invoke(this, new EventArgs());
        public event EventHandler MouseEntersFormIcon3;
        private void OnMouseEntersFormIcon3(object sender, EventArgs e) => MouseEntersFormIcon3?.Invoke(this, new EventArgs());
        public event EventHandler MouseLeavesFormIcon3;
        private void OnMouseLeavesFormIcon3(object sender, EventArgs e) => MouseLeavesFormIcon3?.Invoke(this, new EventArgs());
        public event EventHandler MouseClicksFormIcon3;
        private void OnMouseClicksFormIcon3(object sender, EventArgs e) => MouseClicksFormIcon3?.Invoke(this, new EventArgs());

        public event EventHandler ResizingEnd = delegate { };
        private void OnResizingEnd(object sender, EventArgs e) => ResizingEnd(this, new EventArgs());

        public event EventHandler<FormLostFocusToOutsideApp> OnFormLostFocusToOutsideApp;

        private void CreateEvents()
        {
            FormIcon1.MouseEnter += OnMouseEntersFormIcon1;
            FormIcon1.MouseLeave += OnMouseLeavesFormIcon1;
            FormIcon1.MouseClick += OnMouseClicksFormIcon1;
            FormIcon2.MouseEnter += OnMouseEntersFormIcon2;
            FormIcon2.MouseLeave += OnMouseLeavesFormIcon2;
            FormIcon2.MouseClick += OnMouseClicksFormIcon2;
            FormIcon3.MouseEnter += OnMouseEntersFormIcon3;
            FormIcon3.MouseLeave += OnMouseLeavesFormIcon3;
            FormIcon3.MouseClick += OnMouseClicksFormIcon3;
            ResizeEnd += OnResizingEnd;
        }

        public void SetImageFormIcon1(Image im) { FormIcon1.Image = im; }
        public void SetImageFormIcon2(Image im) { FormIcon2.Image = im; }
        public void SetImageFormIcon3(Image im) { FormIcon3.Image = im; }
        public void VisibilityFormIcon1(bool show) { FormIcon1.Visible = show; }
        public void VisibilityFormIcon2(bool show) { FormIcon2.Visible = show; }
        public void VisibilityFormIcon3(bool show) { FormIcon3.Visible = show; }
        private void VisibilityFormIcon4(bool show) { FormIcon4.Visible = show; }
        private void VisibilityFormIcon5(bool show) { FormIcon5.Visible = show; }

        private const int ResizeHandleSize = 10;

        protected override void WndProc(ref Message m) // Listen for operating system messages.
        {
            var handled = false;
            if (Visible)
            {
                if (m.Msg == Win32.WmNchittest && FormResizable)//  http://stackoverflow.com/questions/17748446/custom-resize-handle-in-border-less-form-c-sharp
                {
                    var formSize = Size;
                    var screenPoint = new Point(m.LParam.ToInt32());
                    var clientPoint = PointToClient(screenPoint);
                    var boxes = new Dictionary<UInt32, Rectangle>()
                    {
                    {Win32.Htbottomleft, new Rectangle(0, formSize.Height - ResizeHandleSize, ResizeHandleSize, ResizeHandleSize)},
                    {Win32.Htbottom, new Rectangle(ResizeHandleSize, formSize.Height - ResizeHandleSize, formSize.Width - 2*ResizeHandleSize, ResizeHandleSize)},
                    {Win32.Htbottomright, new Rectangle(formSize.Width - ResizeHandleSize, formSize.Height - ResizeHandleSize, ResizeHandleSize, ResizeHandleSize)},
                    {Win32.Htright, new Rectangle(formSize.Width - ResizeHandleSize, ResizeHandleSize, ResizeHandleSize, formSize.Height - 2*ResizeHandleSize)},
                    {Win32.Httopright, new Rectangle(formSize.Width - ResizeHandleSize, 0, ResizeHandleSize, ResizeHandleSize) },
                    {Win32.Httop, new Rectangle(ResizeHandleSize, 0, formSize.Width - 2*ResizeHandleSize, ResizeHandleSize) },
                    {Win32.Httopleft, new Rectangle(0, 0, ResizeHandleSize, ResizeHandleSize) },
                    {Win32.Htleft, new Rectangle(0, ResizeHandleSize, ResizeHandleSize, formSize.Height - 2*ResizeHandleSize) }
                    };
                    foreach (var hitBox in boxes.Where(hitBox => hitBox.Value.Contains(clientPoint)))
                    {
                        m.Result = (IntPtr)hitBox.Key;
                        handled = true;
                        break;
                    }
                }
                else switch (m.Msg)
                {
                    // Form shadow
                    case Win32.WmNcpaint when FormShadowShown:
                    {
                        var v = 2;
                        NativeMethods.DwmSetWindowAttribute(Handle, 2, ref v, 4);
                        var margins = new NativeMethods.Margins()
                        {
                            BottomHeight = 1,
                            LeftWidth = 1,
                            RightWidth = 1,
                            TopHeight = 1
                        };
                        NativeMethods.DwmExtendFrameIntoClientArea(Handle, ref margins);
                        handled = true;
                        break;
                    }
                    case Win32.WmActivate:
                    {
                        if ((int)m.WParam == Win32.WaInactive)             // form inactive
                        {
                            OnFormLostFocusToOutsideApp?.Invoke(this, new FormLostFocusToOutsideApp());
                            handled = true;
                        }

                        break;
                    }
                }
            }
            if (!handled) base.WndProc(ref m);
        }

        private const int FormBorderThickness = 1;
        private new Rectangle Top
        {
            get { return new Rectangle(0, 0, ClientSize.Width, FormBorderThickness); }
        }

        private new Rectangle Left
        {
            get { return new Rectangle(0, 0, FormBorderThickness, ClientSize.Height); }
        }

        private new Rectangle Bottom
        {
            get
            {
                return new Rectangle(0, ClientSize.Height - FormBorderThickness, ClientSize.Width, FormBorderThickness);
            }
        }

        private new Rectangle Right
        {
            get
            {
                return new Rectangle(ClientSize.Width - FormBorderThickness, 0, FormBorderThickness, ClientSize.Height);
            }
        }

        private SolidBrush BorderBrush { get; } = new SolidBrush(Color.SteelBlue);
        protected override void OnPaint(PaintEventArgs e) // to get a windows 10 style form border
        {
            e.Graphics.FillRectangle(BorderBrush, Top);
            e.Graphics.FillRectangle(BorderBrush, Left);
            e.Graphics.FillRectangle(BorderBrush, Right);
            e.Graphics.FillRectangle(BorderBrush, Bottom);
        }

        private void Form_ResizeBegin(object sender, EventArgs e)
        {
            FormResizing = true;
        }

        private void Form_ResizeEnd(object sender, EventArgs e)
        {
            FormResizing = false;
            var scn = Screen.FromPoint(Location);
            if (SnapToScreenEdge)
            {
                if (SnapToScreenEdges(base.Left, scn.WorkingArea.Left))
                    base.Left = scn.WorkingArea.Left;
                if (SnapToScreenEdges(base.Top, scn.WorkingArea.Top))
                    base.Top = scn.WorkingArea.Top;
                if (SnapToScreenEdges(scn.WorkingArea.Right, base.Right))
                    base.Left = scn.WorkingArea.Right - Width;
                if (SnapToScreenEdges(scn.WorkingArea.Bottom, base.Bottom))
                    base.Top = scn.WorkingArea.Bottom - Height;
            }
            RefreshAll();
        }

        private void RefreshAll()
        {
            Refresh();
        }

        private static bool SnapToScreenEdges(int screenPosEdge, int formPosEdge)
        {
            const int snapDist = 18;
            var delta = Math.Abs(screenPosEdge - formPosEdge);
            return delta <= snapDist;
        }

        private Point _cursorPosition;
        private Point _formLocation;
        private bool _mouseDown;

        private void Form_MouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            _cursorPosition = Cursor.Position;
            _formLocation = Location;
            _mouseDown = true;
        }

        private void Form_MouseMove()
        {
            if (!FormMovable || !_mouseDown) return;
            var x = Cursor.Position.X - _cursorPosition.X;
            var y = Cursor.Position.Y - _cursorPosition.Y;
            Location = new Point(_formLocation.X + x, _formLocation.Y + y);
        }

        private void Form_MouseUp(EventArgs e)
        {
            _mouseDown = false;
            OnResizeEnd(e);
        }

        private void PanelTitle_MouseDown(object sender, MouseEventArgs e)
        {
            Form_MouseDown(e);
        }

        private void PanelTitle_MouseMove(object sender, MouseEventArgs e)
        {
            Form_MouseMove();
        }

        private void PanelTitle_MouseUp(object sender, MouseEventArgs e)
        {
            Form_MouseUp(e);
        }

        private void LabelTitleTop_MouseDown(object sender, MouseEventArgs e)
        {
            Form_MouseDown(e);
        }

        private void LabelTitleTop_MouseMove(object sender, MouseEventArgs e)
        {
            Form_MouseMove();
        }

        private void LabelTitleTop_MouseUp(object sender, MouseEventArgs e)
        {
            Form_MouseUp(e);
        }

        private void FormTemplate_Activated(object sender, EventArgs e)
        {
            //RefreshAll(); in windows 7 this functions fine, when showing from min form
        }

        // used un modified from https://stackoverflow.com/questions/156046/show-a-form-without-stealing-focus/156159#156159
        private const int SwShownoactivate = 4;
        private const int HwndTopmost = -1;
        private const uint SwpNoactivate = 0x0010;

        public void ShowInactiveTopmost(Form frm)
        {
            NativeMethods.ShowWindow(frm.Handle, SwShownoactivate);
            NativeMethods.SetWindowPos(frm.Handle.ToInt32(), HwndTopmost,
            frm.Left, frm.Top, frm.Width, frm.Height,
            SwpNoactivate);
        }
    }
}
