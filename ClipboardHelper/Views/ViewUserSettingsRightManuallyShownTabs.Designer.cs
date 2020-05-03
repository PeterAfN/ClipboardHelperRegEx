using ClipboardHelperRegEx.ModifiedControls;

namespace ClipboardHelper.Views
{
    partial class ViewUserSettingsRightManuallyShownTabs
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.line3 = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.line2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.line1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxRight = new System.Windows.Forms.GroupBox();
            this.textRight = new System.Windows.Forms.RichTextBox();
            this.groupBoxLeft = new System.Windows.Forms.GroupBox();
            this.listLeft = new ModifiedListBox();
            this.rightClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.add = new System.Windows.Forms.ToolStripMenuItem();
            this.editName = new System.Windows.Forms.ToolStripMenuItem();
            this.delete = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.line3.SuspendLayout();
            this.line2.SuspendLayout();
            this.line1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBoxRight.SuspendLayout();
            this.groupBoxLeft.SuspendLayout();
            this.rightClickMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.line3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.line2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.line1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(631, 118);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // line3
            // 
            this.line3.AutoSize = true;
            this.line3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.line3.Controls.Add(this.label3);
            this.line3.Location = new System.Drawing.Point(3, 85);
            this.line3.MinimumSize = new System.Drawing.Size(0, 30);
            this.line3.Name = "line3";
            this.line3.Size = new System.Drawing.Size(423, 30);
            this.line3.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.MinimumSize = new System.Drawing.Size(0, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(417, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "2. To change the order the ManuallyShownTabs are shown, drag them with left mouse" +
    ".";
            // 
            // line2
            // 
            this.line2.AutoSize = true;
            this.line2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.line2.Controls.Add(this.label2);
            this.line2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.line2.Location = new System.Drawing.Point(3, 49);
            this.line2.MinimumSize = new System.Drawing.Size(0, 30);
            this.line2.Name = "line2";
            this.line2.Size = new System.Drawing.Size(625, 30);
            this.line2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.MinimumSize = new System.Drawing.Size(0, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(348, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "1. To add, rename or delete a ManuallyShownTab, right click on the left.";
            // 
            // line1
            // 
            this.line1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.line1.AutoSize = true;
            this.line1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.line1.Controls.Add(this.label1);
            this.line1.Location = new System.Drawing.Point(3, 3);
            this.line1.MinimumSize = new System.Drawing.Size(0, 40);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(625, 40);
            this.line1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(604, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Info: ManuallyShownTabs are only shown when the user selects them and can\'t be au" +
    "tomatically shown. ManuallyShownTabs aren\'t dependant on changes in the clipboar" +
    "d.";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.groupBoxRight, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBoxLeft, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 118);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(631, 320);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // groupBoxRight
            // 
            this.groupBoxRight.AutoSize = true;
            this.groupBoxRight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxRight.BackColor = System.Drawing.Color.White;
            this.groupBoxRight.Controls.Add(this.textRight);
            this.groupBoxRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxRight.Location = new System.Drawing.Point(183, 3);
            this.groupBoxRight.Name = "groupBoxRight";
            this.groupBoxRight.Size = new System.Drawing.Size(445, 314);
            this.groupBoxRight.TabIndex = 4;
            this.groupBoxRight.TabStop = false;
            this.groupBoxRight.Text = "Tab content [Editable]:";
            this.groupBoxRight.Visible = false;
            // 
            // textRight
            // 
            this.textRight.BackColor = System.Drawing.Color.White;
            this.textRight.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textRight.ForeColor = System.Drawing.Color.Black;
            this.textRight.Location = new System.Drawing.Point(3, 16);
            this.textRight.Name = "textRight";
            this.textRight.Size = new System.Drawing.Size(439, 295);
            this.textRight.TabIndex = 2;
            this.textRight.Text = "";
            this.textRight.WordWrap = false;
            // 
            // groupBoxLeft
            // 
            this.groupBoxLeft.AutoSize = true;
            this.groupBoxLeft.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxLeft.BackColor = System.Drawing.Color.White;
            this.groupBoxLeft.Controls.Add(this.listLeft);
            this.groupBoxLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxLeft.Location = new System.Drawing.Point(3, 3);
            this.groupBoxLeft.Name = "groupBoxLeft";
            this.groupBoxLeft.Size = new System.Drawing.Size(174, 314);
            this.groupBoxLeft.TabIndex = 3;
            this.groupBoxLeft.TabStop = false;
            this.groupBoxLeft.Text = "Tab order [Draggable]:";
            // 
            // listLeft
            // 
            this.listLeft.AllowDrop = true;
            this.listLeft.BackColor = System.Drawing.Color.White;
            this.listLeft.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listLeft.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listLeft.ForeColor = System.Drawing.Color.Black;
            this.listLeft.FormattingEnabled = true;
            this.listLeft.HorizontalScrollbar = true;
            this.listLeft.Location = new System.Drawing.Point(3, 16);
            this.listLeft.Name = "listLeft";
            this.listLeft.SelectionColor = System.Drawing.Color.Black;
            this.listLeft.SelectionTextColor = System.Drawing.Color.White;
            this.listLeft.Size = new System.Drawing.Size(168, 295);
            this.listLeft.TabIndex = 0;
            this.listLeft.TextColor = System.Drawing.Color.Black;
            // 
            // rightClickMenu
            // 
            this.rightClickMenu.DropShadowEnabled = false;
            this.rightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.add,
            this.editName,
            this.delete});
            this.rightClickMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.rightClickMenu.Name = "addEditDelete";
            this.rightClickMenu.Size = new System.Drawing.Size(128, 70);
            // 
            // add
            // 
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(127, 22);
            this.add.Text = "add";
            // 
            // editName
            // 
            this.editName.Name = "editName";
            this.editName.Size = new System.Drawing.Size(127, 22);
            this.editName.Text = "edit name";
            // 
            // delete
            // 
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(127, 22);
            this.delete.Text = "delete";
            // 
            // ViewUserSettingsRightManuallyShownTabs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ViewUserSettingsRightManuallyShownTabs";
            this.Size = new System.Drawing.Size(631, 438);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.line3.ResumeLayout(false);
            this.line3.PerformLayout();
            this.line2.ResumeLayout(false);
            this.line2.PerformLayout();
            this.line1.ResumeLayout(false);
            this.line1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBoxRight.ResumeLayout(false);
            this.groupBoxLeft.ResumeLayout(false);
            this.rightClickMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel line2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel line3;
        private System.Windows.Forms.ToolStripMenuItem add;
        private System.Windows.Forms.ToolStripMenuItem delete;
        private System.Windows.Forms.FlowLayoutPanel line1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem editName;
        public System.Windows.Forms.ContextMenuStrip rightClickMenu;
        public System.Windows.Forms.GroupBox groupBoxRight;
        public System.Windows.Forms.GroupBox groupBoxLeft;
        public ModifiedListBox listLeft;
        public System.Windows.Forms.RichTextBox textRight;
    }
}
