using ClipboardHelperRegEx.ModifiedControls;

namespace ClipboardHelper.Views
{
    partial class ViewUserSettingsRightAutoShownTabs
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.group1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.newButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.deleteButton = new System.Windows.Forms.Button();
            this.renameButton = new System.Windows.Forms.Button();
            this.group2RegEx = new System.Windows.Forms.GroupBox();
            this.regExString = new System.Windows.Forms.RichTextBox();
            this.flowLayoutPanelRegExHelp = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.group3Items = new System.Windows.Forms.GroupBox();
            this.itemsList = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxLeftSimulated = new System.Windows.Forms.GroupBox();
            this.listLeftSimulated = new System.Windows.Forms.RichTextBox();
            this.groupBoxRightResult = new System.Windows.Forms.GroupBox();
            this.group4Help = new System.Windows.Forms.GroupBox();
            this.ruleNames = new AdvancedComboBox();
            this.listRightResult = new ModifiedListBox();
            this.tableLayoutPanel2.SuspendLayout();
            this.group1.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.group2RegEx.SuspendLayout();
            this.flowLayoutPanelRegExHelp.SuspendLayout();
            this.group3Items.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBoxLeftSimulated.SuspendLayout();
            this.groupBoxRightResult.SuspendLayout();
            this.group4Help.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.group1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanelRegExHelp, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.group2RegEx, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.group3Items, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.group4Help, 0, 4);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(865, 570);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // group1
            // 
            this.group1.AutoSize = true;
            this.group1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.group1.Controls.Add(this.flowLayoutPanel3);
            this.group1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.group1.Location = new System.Drawing.Point(3, 3);
            this.group1.Name = "group1";
            this.group1.Size = new System.Drawing.Size(859, 48);
            this.group1.TabIndex = 9;
            this.group1.TabStop = false;
            this.group1.Text = "1. To create/edit a AutoTriggeredTab, please:";
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.AutoSize = true;
            this.flowLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel3.Controls.Add(this.label1);
            this.flowLayoutPanel3.Controls.Add(this.newButton);
            this.flowLayoutPanel3.Controls.Add(this.label2);
            this.flowLayoutPanel3.Controls.Add(this.ruleNames);
            this.flowLayoutPanel3.Controls.Add(this.deleteButton);
            this.flowLayoutPanel3.Controls.Add(this.renameButton);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(853, 29);
            this.flowLayoutPanel3.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "press";
            // 
            // newButton
            // 
            this.newButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.newButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.newButton.Location = new System.Drawing.Point(41, 3);
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(46, 23);
            this.newButton.TabIndex = 0;
            this.newButton.Text = "New";
            this.newButton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(93, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "or select a Tab:";
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteButton.Location = new System.Drawing.Point(329, 3);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(54, 23);
            this.deleteButton.TabIndex = 1;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            // 
            // renameButton
            // 
            this.renameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.renameButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.renameButton.Location = new System.Drawing.Point(389, 3);
            this.renameButton.Name = "renameButton";
            this.renameButton.Size = new System.Drawing.Size(63, 23);
            this.renameButton.TabIndex = 5;
            this.renameButton.Text = "Rename";
            this.renameButton.UseVisualStyleBackColor = true;
            // 
            // group2RegEx
            // 
            this.group2RegEx.Controls.Add(this.regExString);
            this.group2RegEx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.group2RegEx.Location = new System.Drawing.Point(3, 76);
            this.group2RegEx.Name = "group2RegEx";
            this.group2RegEx.Size = new System.Drawing.Size(859, 49);
            this.group2RegEx.TabIndex = 6;
            this.group2RegEx.TabStop = false;
            this.group2RegEx.Text = "2. Please insert RegEx*.";
            // 
            // regExString
            // 
            this.regExString.BackColor = System.Drawing.Color.White;
            this.regExString.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.regExString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.regExString.ForeColor = System.Drawing.Color.SteelBlue;
            this.regExString.Location = new System.Drawing.Point(3, 16);
            this.regExString.Multiline = false;
            this.regExString.Name = "regExString";
            this.regExString.Size = new System.Drawing.Size(853, 30);
            this.regExString.TabIndex = 1;
            this.regExString.Text = "(^(\\d{1,3}\\.){3}\\d{1,3}\\seth\\s[1-5]/\\d\\d?)%(^(\\d{1,3}\\.){3}\\d{1,3}\\seth\\s[1-5]/\\d" +
    "\\d?)%";
            this.regExString.WordWrap = false;
            // 
            // flowLayoutPanelRegExHelp
            // 
            this.flowLayoutPanelRegExHelp.AutoSize = true;
            this.flowLayoutPanelRegExHelp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelRegExHelp.Controls.Add(this.label3);
            this.flowLayoutPanelRegExHelp.Controls.Add(this.linkLabel1);
            this.flowLayoutPanelRegExHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelRegExHelp.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.flowLayoutPanelRegExHelp.Location = new System.Drawing.Point(3, 57);
            this.flowLayoutPanelRegExHelp.Name = "flowLayoutPanelRegExHelp";
            this.flowLayoutPanelRegExHelp.Size = new System.Drawing.Size(859, 13);
            this.flowLayoutPanelRegExHelp.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(273, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "    *See Microsoft website for help when creating RegEx:";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkLabel1.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.linkLabel1.Location = new System.Drawing.Point(282, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(516, 13);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "https://docs.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-la" +
    "nguage-quick-reference";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked);
            // 
            // group3Items
            // 
            this.group3Items.Controls.Add(this.itemsList);
            this.group3Items.Dock = System.Windows.Forms.DockStyle.Fill;
            this.group3Items.Location = new System.Drawing.Point(3, 131);
            this.group3Items.MinimumSize = new System.Drawing.Size(0, 180);
            this.group3Items.Name = "group3Items";
            this.group3Items.Size = new System.Drawing.Size(859, 215);
            this.group3Items.TabIndex = 7;
            this.group3Items.TabStop = false;
            this.group3Items.Text = "3. When Clipboard content changes and rules make a match, please insert what is s" +
    "hown:";
            // 
            // itemsList
            // 
            this.itemsList.AutoWordSelection = true;
            this.itemsList.BackColor = System.Drawing.Color.White;
            this.itemsList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.itemsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemsList.ForeColor = System.Drawing.Color.SteelBlue;
            this.itemsList.Location = new System.Drawing.Point(3, 16);
            this.itemsList.Name = "itemsList";
            this.itemsList.Size = new System.Drawing.Size(853, 196);
            this.itemsList.TabIndex = 0;
            this.itemsList.Text = "";
            this.itemsList.WordWrap = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63F));
            this.tableLayoutPanel1.Controls.Add(this.groupBoxRightResult, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxLeftSimulated, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(853, 196);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // groupBoxLeftSimulated
            // 
            this.groupBoxLeftSimulated.AutoSize = true;
            this.groupBoxLeftSimulated.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxLeftSimulated.BackColor = System.Drawing.Color.White;
            this.groupBoxLeftSimulated.Controls.Add(this.listLeftSimulated);
            this.groupBoxLeftSimulated.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.groupBoxLeftSimulated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxLeftSimulated.ForeColor = System.Drawing.Color.Black;
            this.groupBoxLeftSimulated.Location = new System.Drawing.Point(3, 3);
            this.groupBoxLeftSimulated.Name = "groupBoxLeftSimulated";
            this.groupBoxLeftSimulated.Size = new System.Drawing.Size(309, 190);
            this.groupBoxLeftSimulated.TabIndex = 6;
            this.groupBoxLeftSimulated.TabStop = false;
            this.groupBoxLeftSimulated.Text = "simulated Clipboard text:";
            // 
            // listLeftSimulated
            // 
            this.listLeftSimulated.BackColor = System.Drawing.Color.White;
            this.listLeftSimulated.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listLeftSimulated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listLeftSimulated.ForeColor = System.Drawing.Color.SteelBlue;
            this.listLeftSimulated.Location = new System.Drawing.Point(3, 16);
            this.listLeftSimulated.Name = "listLeftSimulated";
            this.listLeftSimulated.Size = new System.Drawing.Size(303, 171);
            this.listLeftSimulated.TabIndex = 1;
            this.listLeftSimulated.Text = "";
            this.listLeftSimulated.WordWrap = false;
            // 
            // groupBoxRightResult
            // 
            this.groupBoxRightResult.AutoSize = true;
            this.groupBoxRightResult.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxRightResult.BackColor = System.Drawing.Color.White;
            this.groupBoxRightResult.Controls.Add(this.listRightResult);
            this.groupBoxRightResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxRightResult.Location = new System.Drawing.Point(318, 3);
            this.groupBoxRightResult.Name = "groupBoxRightResult";
            this.groupBoxRightResult.Padding = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.groupBoxRightResult.Size = new System.Drawing.Size(532, 190);
            this.groupBoxRightResult.TabIndex = 5;
            this.groupBoxRightResult.TabStop = false;
            this.groupBoxRightResult.Text = "...which is represented as these Selectable Items: ";
            // 
            // group4Help
            // 
            this.group4Help.AutoSize = true;
            this.group4Help.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.group4Help.BackColor = System.Drawing.Color.White;
            this.group4Help.Controls.Add(this.tableLayoutPanel1);
            this.group4Help.Dock = System.Windows.Forms.DockStyle.Fill;
            this.group4Help.Location = new System.Drawing.Point(3, 352);
            this.group4Help.Name = "group4Help";
            this.group4Help.Size = new System.Drawing.Size(859, 215);
            this.group4Help.TabIndex = 4;
            this.group4Help.TabStop = false;
            this.group4Help.Text = "gyug";
            // 
            // ruleNames
            // 
            this.ruleNames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ruleNames.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ruleNames.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ruleNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ruleNames.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ruleNames.HighlightColor = System.Drawing.Color.Black;
            this.ruleNames.HighlightFontColor = System.Drawing.Color.White;
            this.ruleNames.Location = new System.Drawing.Point(180, 4);
            this.ruleNames.Name = "ruleNames";
            this.ruleNames.Size = new System.Drawing.Size(143, 21);
            this.ruleNames.Sorted = true;
            this.ruleNames.TabIndex = 3;
            // 
            // listRightResult
            // 
            this.listRightResult.BackColor = System.Drawing.Color.White;
            this.listRightResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listRightResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listRightResult.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listRightResult.FormattingEnabled = true;
            this.listRightResult.Items.AddRange(new object[] {
            "Regex doesn\'t  match."});
            this.listRightResult.Location = new System.Drawing.Point(10, 16);
            this.listRightResult.Name = "listRightResult";
            this.listRightResult.SelectionColor = System.Drawing.Color.Black;
            this.listRightResult.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listRightResult.SelectionTextColor = System.Drawing.Color.White;
            this.listRightResult.Size = new System.Drawing.Size(519, 171);
            this.listRightResult.TabIndex = 0;
            this.listRightResult.TextColor = System.Drawing.Color.Black;
            // 
            // ViewUserSettingsRightAutoShownTabs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tableLayoutPanel2);
            this.MinimumSize = new System.Drawing.Size(474, 495);
            this.Name = "ViewUserSettingsRightAutoShownTabs";
            this.Size = new System.Drawing.Size(865, 570);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.group2RegEx.ResumeLayout(false);
            this.flowLayoutPanelRegExHelp.ResumeLayout(false);
            this.flowLayoutPanelRegExHelp.PerformLayout();
            this.group3Items.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBoxLeftSimulated.ResumeLayout(false);
            this.groupBoxRightResult.ResumeLayout(false);
            this.group4Help.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox group3Items;
        private System.Windows.Forms.GroupBox group2RegEx;
        private System.Windows.Forms.RichTextBox itemsList;
        private System.Windows.Forms.RichTextBox regExString;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelRegExHelp;
        private System.Windows.Forms.GroupBox group1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button newButton;
        private System.Windows.Forms.Label label2;
        private AdvancedComboBox ruleNames;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button renameButton;
        private System.Windows.Forms.GroupBox group4Help;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBoxRightResult;
        private ModifiedListBox listRightResult;
        private System.Windows.Forms.GroupBox groupBoxLeftSimulated;
        private System.Windows.Forms.RichTextBox listLeftSimulated;
    }
}
