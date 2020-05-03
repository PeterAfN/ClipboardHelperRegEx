namespace ClipboardHelper.Views
{
    partial class ViewMainSplContPanelDown
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelHistoryLeft = new System.Windows.Forms.Panel();
            this.historyLeft = new System.Windows.Forms.PictureBox();
            this.panelHistoryPosition = new System.Windows.Forms.Panel();
            this.labelPositionH = new System.Windows.Forms.Label();
            this.panelHistoryRight = new System.Windows.Forms.Panel();
            this.historyRight = new System.Windows.Forms.PictureBox();
            this.clipboardLabel = new System.Windows.Forms.Label();
            this.textboxClipboard = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelHistoryLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.historyLeft)).BeginInit();
            this.panelHistoryPosition.SuspendLayout();
            this.panelHistoryRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.historyRight)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textboxClipboard, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(528, 112);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panelHistoryLeft);
            this.panel1.Controls.Add(this.panelHistoryPosition);
            this.panel1.Controls.Add(this.panelHistoryRight);
            this.panel1.Controls.Add(this.clipboardLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.MaximumSize = new System.Drawing.Size(0, 19);
            this.panel1.MinimumSize = new System.Drawing.Size(0, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(522, 19);
            this.panel1.TabIndex = 1;
            // 
            // panelHistoryLeft
            // 
            this.panelHistoryLeft.Controls.Add(this.historyLeft);
            this.panelHistoryLeft.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelHistoryLeft.Location = new System.Drawing.Point(463, 0);
            this.panelHistoryLeft.Name = "panelHistoryLeft";
            this.panelHistoryLeft.Size = new System.Drawing.Size(17, 19);
            this.panelHistoryLeft.TabIndex = 3;
            // 
            // historyLeft
            // 
            this.historyLeft.BackColor = System.Drawing.Color.White;
            this.historyLeft.BackgroundImage = global::ClipboardHelperRegEx.Properties.Resources.LeftArrow;
            this.historyLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.historyLeft.Enabled = false;
            this.historyLeft.Location = new System.Drawing.Point(0, 0);
            this.historyLeft.MaximumSize = new System.Drawing.Size(16, 16);
            this.historyLeft.MinimumSize = new System.Drawing.Size(16, 16);
            this.historyLeft.Name = "historyLeft";
            this.historyLeft.Size = new System.Drawing.Size(16, 16);
            this.historyLeft.TabIndex = 2;
            this.historyLeft.TabStop = false;
            this.historyLeft.Visible = false;
            // 
            // panelHistoryPosition
            // 
            this.panelHistoryPosition.Controls.Add(this.labelPositionH);
            this.panelHistoryPosition.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelHistoryPosition.Location = new System.Drawing.Point(480, 0);
            this.panelHistoryPosition.Name = "panelHistoryPosition";
            this.panelHistoryPosition.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.panelHistoryPosition.Size = new System.Drawing.Size(25, 19);
            this.panelHistoryPosition.TabIndex = 4;
            // 
            // labelPositionH
            // 
            this.labelPositionH.BackColor = System.Drawing.Color.White;
            this.labelPositionH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPositionH.ForeColor = System.Drawing.Color.Black;
            this.labelPositionH.Location = new System.Drawing.Point(0, 0);
            this.labelPositionH.Name = "labelPositionH";
            this.labelPositionH.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.labelPositionH.Size = new System.Drawing.Size(25, 18);
            this.labelPositionH.TabIndex = 1;
            this.labelPositionH.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panelHistoryRight
            // 
            this.panelHistoryRight.Controls.Add(this.historyRight);
            this.panelHistoryRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelHistoryRight.Location = new System.Drawing.Point(505, 0);
            this.panelHistoryRight.Name = "panelHistoryRight";
            this.panelHistoryRight.Size = new System.Drawing.Size(17, 19);
            this.panelHistoryRight.TabIndex = 5;
            // 
            // historyRight
            // 
            this.historyRight.BackColor = System.Drawing.Color.White;
            this.historyRight.BackgroundImage = global::ClipboardHelperRegEx.Properties.Resources.RightArrow;
            this.historyRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.historyRight.Enabled = false;
            this.historyRight.ErrorImage = null;
            this.historyRight.InitialImage = null;
            this.historyRight.Location = new System.Drawing.Point(0, 0);
            this.historyRight.MaximumSize = new System.Drawing.Size(16, 16);
            this.historyRight.MinimumSize = new System.Drawing.Size(16, 16);
            this.historyRight.Name = "historyRight";
            this.historyRight.Size = new System.Drawing.Size(16, 16);
            this.historyRight.TabIndex = 0;
            this.historyRight.TabStop = false;
            this.historyRight.Visible = false;
            // 
            // clipboardLabel
            // 
            this.clipboardLabel.AutoSize = true;
            this.clipboardLabel.BackColor = System.Drawing.Color.White;
            this.clipboardLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.clipboardLabel.ForeColor = System.Drawing.Color.Black;
            this.clipboardLabel.Location = new System.Drawing.Point(0, 0);
            this.clipboardLabel.Margin = new System.Windows.Forms.Padding(0);
            this.clipboardLabel.Name = "clipboardLabel";
            this.clipboardLabel.Padding = new System.Windows.Forms.Padding(0, 2, 3, 0);
            this.clipboardLabel.Size = new System.Drawing.Size(92, 15);
            this.clipboardLabel.TabIndex = 0;
            this.clipboardLabel.Text = "In Clipboard now:";
            // 
            // textboxClipboard
            // 
            this.textboxClipboard.BackColor = System.Drawing.Color.White;
            this.textboxClipboard.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textboxClipboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textboxClipboard.Location = new System.Drawing.Point(5, 25);
            this.textboxClipboard.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.textboxClipboard.Name = "textboxClipboard";
            this.textboxClipboard.ReadOnly = true;
            this.textboxClipboard.Size = new System.Drawing.Size(523, 87);
            this.textboxClipboard.TabIndex = 2;
            this.textboxClipboard.Text = "";
            // 
            // ViewMainSplContPanelDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ViewMainSplContPanelDown";
            this.Size = new System.Drawing.Size(528, 112);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelHistoryLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.historyLeft)).EndInit();
            this.panelHistoryPosition.ResumeLayout(false);
            this.panelHistoryRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.historyRight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label clipboardLabel;
        private System.Windows.Forms.PictureBox historyLeft;
        public System.Windows.Forms.Label labelPositionH;
        private System.Windows.Forms.PictureBox historyRight;
        private System.Windows.Forms.Panel panelHistoryLeft;
        private System.Windows.Forms.Panel panelHistoryPosition;
        private System.Windows.Forms.Panel panelHistoryRight;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox textboxClipboard;
    }
}
