namespace ClipboardHelper.Views
{
    partial class ViewMain
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewMain));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemOm = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemVisa = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemInställningar = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAvaktivera = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAvsluta = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIconProgram = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon1)).BeginInit();
            this.panelTitle.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTitleTop
            // 
            this.labelTitleTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitleTop.Size = new System.Drawing.Size(0, 13);
            this.labelTitleTop.Text = "";
            // 
            // FormIcon3
            // 
            this.FormIcon3.Location = new System.Drawing.Point(378, 0);
            // 
            // panelSpace1
            // 
            this.panelSpace1.Location = new System.Drawing.Point(394, 0);
            // 
            // FormIcon4
            // 
            this.FormIcon4.Location = new System.Drawing.Point(395, 0);
            // 
            // panelSpace2
            // 
            this.panelSpace2.Location = new System.Drawing.Point(411, 0);
            // 
            // FormIcon5
            // 
            this.FormIcon5.Location = new System.Drawing.Point(412, 0);
            // 
            // FormIcon2
            // 
            this.FormIcon2.Location = new System.Drawing.Point(361, 0);
            // 
            // panel1Space
            // 
            this.panel1Space.Location = new System.Drawing.Point(377, 0);
            // 
            // panelSeparatorLine
            // 
            this.panelSeparatorLine.BackColor = System.Drawing.Color.White;
            this.panelSeparatorLine.Location = new System.Drawing.Point(3, 21);
            this.panelSeparatorLine.Size = new System.Drawing.Size(428, 1);
            // 
            // FormIcon1
            // 
            this.FormIcon1.Location = new System.Drawing.Point(344, 0);
            // 
            // panel11
            // 
            this.panel11.Location = new System.Drawing.Point(360, 0);
            // 
            // panelTitle
            // 
            this.panelTitle.Location = new System.Drawing.Point(3, 3);
            this.panelTitle.Size = new System.Drawing.Size(428, 18);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemOm,
            this.toolStripMenuItemVisa,
            this.toolStripMenuItemInställningar,
            this.toolStripMenuItemAvaktivera,
            this.toolStripMenuItemAvsluta});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 136);
            // 
            // toolStripMenuItemOm
            // 
            this.toolStripMenuItemOm.Name = "toolStripMenuItemOm";
            this.toolStripMenuItemOm.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItemOm.Text = "About";
            // 
            // toolStripMenuItemVisa
            // 
            this.toolStripMenuItemVisa.Name = "toolStripMenuItemVisa";
            this.toolStripMenuItemVisa.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItemVisa.Text = "Show";
            // 
            // toolStripMenuItemInställningar
            // 
            this.toolStripMenuItemInställningar.Name = "toolStripMenuItemInställningar";
            this.toolStripMenuItemInställningar.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItemInställningar.Text = "Settings";
            // 
            // toolStripMenuItemAvaktivera
            // 
            this.toolStripMenuItemAvaktivera.Name = "toolStripMenuItemAvaktivera";
            this.toolStripMenuItemAvaktivera.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItemAvaktivera.Text = "Deactivate";
            // 
            // toolStripMenuItemAvsluta
            // 
            this.toolStripMenuItemAvsluta.Name = "toolStripMenuItemAvsluta";
            this.toolStripMenuItemAvsluta.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItemAvsluta.Text = "Shutdown";
            // 
            // notifyIconProgram
            // 
            this.notifyIconProgram.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIconProgram.Visible = true;
            // 
            // ViewMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(434, 773);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(150, 159);
            this.Name = "ViewMain";
            this.Opacity = 0.99D;
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "Window - Clipboard Helper";
            this.Controls.SetChildIndex(this.panelTitle, 0);
            this.Controls.SetChildIndex(this.panelSeparatorLine, 0);
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormIcon1)).EndInit();
            this.panelTitle.ResumeLayout(false);
            this.panelTitle.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOm;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemVisa;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemInställningar;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAvaktivera;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAvsluta;
        private System.Windows.Forms.NotifyIcon notifyIconProgram;
        public System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}

