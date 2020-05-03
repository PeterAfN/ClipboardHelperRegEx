namespace ClipboardHelper.Views
{
    partial class ViewDialog
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
        public void InitializeComponent()
        {
            this.userInput = new System.Windows.Forms.TextBox();
            this.flowLayoutButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.userInformation = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon1)).BeginInit();
            this.panelTitle.SuspendLayout();
            this.flowLayoutButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTitleTop
            // 
            this.labelTitleTop.Size = new System.Drawing.Size(0, 16);
            this.labelTitleTop.Text = "";
            // 
            // FormIcon3
            // 
            this.FormIcon3.Location = new System.Drawing.Point(34, 0);
            // 
            // panelSpace1
            // 
            this.panelSpace1.Location = new System.Drawing.Point(50, 0);
            // 
            // FormIcon4
            // 
            this.FormIcon4.Location = new System.Drawing.Point(51, 0);
            // 
            // panelSpace2
            // 
            this.panelSpace2.Location = new System.Drawing.Point(67, 0);
            // 
            // FormIcon5
            // 
            this.FormIcon5.Location = new System.Drawing.Point(68, 0);
            // 
            // FormIcon2
            // 
            this.FormIcon2.Location = new System.Drawing.Point(17, 0);
            // 
            // panel1Space
            // 
            this.panel1Space.Location = new System.Drawing.Point(33, 0);
            // 
            // panelSeparatorLine
            // 
            this.panelSeparatorLine.AutoSize = true;
            this.panelSeparatorLine.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelSeparatorLine.Dock = System.Windows.Forms.DockStyle.None;
            this.panelSeparatorLine.Enabled = false;
            this.panelSeparatorLine.Size = new System.Drawing.Size(0, 0);
            this.panelSeparatorLine.Visible = false;
            // 
            // FormIcon1
            // 
            this.FormIcon1.Location = new System.Drawing.Point(0, 0);
            // 
            // panel11
            // 
            this.panel11.Location = new System.Drawing.Point(16, 0);
            // 
            // panelTitle
            // 
            this.panelTitle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.None;
            this.panelTitle.Enabled = false;
            this.panelTitle.Size = new System.Drawing.Size(84, 18);
            this.panelTitle.Visible = false;
            // 
            // userInput
            // 
            this.userInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userInput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userInput.Location = new System.Drawing.Point(4, 44);
            this.userInput.Name = "userInput";
            this.userInput.Size = new System.Drawing.Size(273, 20);
            this.userInput.TabIndex = 1;
            this.userInput.WordWrap = false;
            // 
            // flowLayoutButtons
            // 
            this.flowLayoutButtons.AutoSize = true;
            this.flowLayoutButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutButtons.Controls.Add(this.buttonCancel);
            this.flowLayoutButtons.Controls.Add(this.buttonOk);
            this.flowLayoutButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutButtons.Location = new System.Drawing.Point(4, 64);
            this.flowLayoutButtons.MaximumSize = new System.Drawing.Size(0, 29);
            this.flowLayoutButtons.MinimumSize = new System.Drawing.Size(0, 29);
            this.flowLayoutButtons.Name = "flowLayoutButtons";
            this.flowLayoutButtons.Size = new System.Drawing.Size(273, 29);
            this.flowLayoutButtons.TabIndex = 1;
            this.flowLayoutButtons.WrapContents = false;
            // 
            // buttonCancel
            // 
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.ForeColor = System.Drawing.Color.Black;
            this.buttonCancel.Location = new System.Drawing.Point(198, 3);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOk.ForeColor = System.Drawing.Color.Black;
            this.buttonOk.Location = new System.Drawing.Point(117, 3);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.ButtonOk_Click);
            // 
            // userInformation
            // 
            this.userInformation.AutoSize = true;
            this.userInformation.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userInformation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.userInformation.ForeColor = System.Drawing.Color.Black;
            this.userInformation.Location = new System.Drawing.Point(4, 28);
            this.userInformation.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.userInformation.MaximumSize = new System.Drawing.Size(230, 0);
            this.userInformation.Name = "userInformation";
            this.userInformation.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.userInformation.Size = new System.Drawing.Size(174, 16);
            this.userInformation.TabIndex = 10;
            this.userInformation.Text = "Change this text to appropriate text:\r\n";
            // 
            // ViewDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(281, 94);
            this.Controls.Add(this.userInformation);
            this.Controls.Add(this.userInput);
            this.Controls.Add(this.flowLayoutButtons);
            this.MinimumSize = new System.Drawing.Size(242, 52);
            this.Name = "ViewDialog";
            this.Padding = new System.Windows.Forms.Padding(4, 3, 4, 1);
            this.Controls.SetChildIndex(this.flowLayoutButtons, 0);
            this.Controls.SetChildIndex(this.userInput, 0);
            this.Controls.SetChildIndex(this.userInformation, 0);
            this.Controls.SetChildIndex(this.panelTitle, 0);
            this.Controls.SetChildIndex(this.panelSeparatorLine, 0);
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon1)).EndInit();
            this.panelTitle.ResumeLayout(false);
            this.panelTitle.PerformLayout();
            this.flowLayoutButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox userInput;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutButtons;
        private System.Windows.Forms.Label userInformation;
        public System.Windows.Forms.Button buttonCancel;
        public System.Windows.Forms.Button buttonOk;
    }
}
