using ClipboardHelperRegEx.ModifiedControls;

namespace ClipboardHelper.Views
{
    partial class ViewUserSettingsRightAdvanced
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanelFiles = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxFiles = new System.Windows.Forms.GroupBox();
            this.labelAdvancedInfo = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.rightClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.add = new System.Windows.Forms.ToolStripMenuItem();
            this.delete = new System.Windows.Forms.ToolStripMenuItem();
            this.replace = new System.Windows.Forms.ToolStripMenuItem();
            this.listBoxFiles = new ModifiedListBox();
            this.tableLayoutPanelFiles.SuspendLayout();
            this.groupBoxFiles.SuspendLayout();
            this.rightClickMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelFiles
            // 
            this.tableLayoutPanelFiles.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelFiles.ColumnCount = 1;
            this.tableLayoutPanelFiles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelFiles.Controls.Add(this.groupBoxFiles, 0, 3);
            this.tableLayoutPanelFiles.Controls.Add(this.labelAdvancedInfo, 0, 0);
            this.tableLayoutPanelFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelFiles.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelFiles.Name = "tableLayoutPanelFiles";
            this.tableLayoutPanelFiles.RowCount = 4;
            this.tableLayoutPanelFiles.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelFiles.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelFiles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelFiles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelFiles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelFiles.Size = new System.Drawing.Size(574, 435);
            this.tableLayoutPanelFiles.TabIndex = 0;
            // 
            // groupBoxFiles
            // 
            this.groupBoxFiles.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxFiles.Controls.Add(this.listBoxFiles);
            this.groupBoxFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxFiles.Location = new System.Drawing.Point(3, 49);
            this.groupBoxFiles.Name = "groupBoxFiles";
            this.groupBoxFiles.Size = new System.Drawing.Size(568, 383);
            this.groupBoxFiles.TabIndex = 10;
            this.groupBoxFiles.TabStop = false;
            this.groupBoxFiles.Text = "Files added: [Right click to add file or delete file]";
            // 
            // labelAdvancedInfo
            // 
            this.labelAdvancedInfo.AutoSize = true;
            this.labelAdvancedInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAdvancedInfo.Location = new System.Drawing.Point(3, 0);
            this.labelAdvancedInfo.Name = "labelAdvancedInfo";
            this.labelAdvancedInfo.Size = new System.Drawing.Size(568, 26);
            this.labelAdvancedInfo.TabIndex = 0;
            this.labelAdvancedInfo.Text = "Info: To add or delete data- or settings files, right click on the files list ben" +
    "eath. Manual- and Auto files are settings files and can\'t be deleted, only repla" +
    "ced. \r\n";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // rightClickMenu
            // 
            this.rightClickMenu.DropShadowEnabled = false;
            this.rightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.add,
            this.delete,
            this.replace});
            this.rightClickMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.rightClickMenu.Name = "addEditDelete";
            this.rightClickMenu.Size = new System.Drawing.Size(113, 70);
            // 
            // add
            // 
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(112, 22);
            this.add.Text = "add";
            // 
            // delete
            // 
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(112, 22);
            this.delete.Text = "delete";
            // 
            // replace
            // 
            this.replace.Name = "replace";
            this.replace.Size = new System.Drawing.Size(112, 22);
            this.replace.Text = "replace";
            // 
            // listBoxFiles
            // 
            this.listBoxFiles.BackColor = System.Drawing.Color.White;
            this.listBoxFiles.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxFiles.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxFiles.FormattingEnabled = true;
            this.listBoxFiles.Location = new System.Drawing.Point(3, 16);
            this.listBoxFiles.Name = "listBoxFiles";
            this.listBoxFiles.SelectionColor = System.Drawing.Color.Black;
            this.listBoxFiles.SelectionTextColor = System.Drawing.Color.White;
            this.listBoxFiles.Size = new System.Drawing.Size(562, 364);
            this.listBoxFiles.TabIndex = 0;
            this.listBoxFiles.TextColor = System.Drawing.Color.Black;
            // 
            // ViewUserSettingsAreaRightAdvanced
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tableLayoutPanelFiles);
            this.Name = "ViewUserSettingsAreaRightAdvanced";
            this.Size = new System.Drawing.Size(574, 435);
            this.tableLayoutPanelFiles.ResumeLayout(false);
            this.tableLayoutPanelFiles.PerformLayout();
            this.groupBoxFiles.ResumeLayout(false);
            this.rightClickMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelFiles;
        private System.Windows.Forms.Label labelAdvancedInfo;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.GroupBox groupBoxFiles;
        public System.Windows.Forms.ContextMenuStrip rightClickMenu;
        private System.Windows.Forms.ToolStripMenuItem add;
        private System.Windows.Forms.ToolStripMenuItem delete;
        private ModifiedListBox listBoxFiles;
        private System.Windows.Forms.ToolStripMenuItem replace;
    }
}