using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClipboardHelper.Views
{
    public partial class ViewPureTextInfo : Form
    {
        private readonly Timer _formClose = new Timer();

        public ViewPureTextInfo(string text)
        {
            InitializeComponent();
            labelInfoText.Text = text;
            _formClose.Interval = 1000;
            _formClose.Tick += FormClose_Tick;
            _formClose.Start();
            Show();
            Location = new Point(MousePosition.X - 180, MousePosition.Y - 43);
        }

        private void FormClose_Tick(object sender, EventArgs e)
        {
            _formClose.Stop();
            _formClose.Tick -= FormClose_Tick;
            _formClose.Dispose();
            Close();
        }
    }
}