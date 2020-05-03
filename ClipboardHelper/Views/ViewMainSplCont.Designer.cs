using ClipboardHelperRegEx.ModifiedControls;

namespace ClipboardHelper.Views
{
    partial class ViewMainSplCont
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
            this.SplitContainer = new ModifiedSplitContainer();
            this.lineDraggableSplCon = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).BeginInit();
            this.SplitContainer.Panel2.SuspendLayout();
            this.SplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.SplitContainer.BackColor = System.Drawing.Color.White;
            this.SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.SplitContainer.ForeColor = System.Drawing.Color.White;
            this.SplitContainer.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer.Margin = new System.Windows.Forms.Padding(0);
            this.SplitContainer.Name = "SplitContainer";
            this.SplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.SplitContainer.Panel1.BackColor = System.Drawing.Color.White;
            this.SplitContainer.Panel1.ForeColor = System.Drawing.Color.White;
            this.SplitContainer.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.SplitContainer.Panel1MinSize = 0;
            // 
            // splitContainer.Panel2
            // 
            this.SplitContainer.Panel2.BackColor = System.Drawing.Color.White;
            this.SplitContainer.Panel2.Controls.Add(this.lineDraggableSplCon);
            this.SplitContainer.Panel2.ForeColor = System.Drawing.Color.White;
            this.SplitContainer.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.SplitContainer.Size = new System.Drawing.Size(347, 347);
            this.SplitContainer.SplitterDistance = 263;
            this.SplitContainer.TabIndex = 0;
            // 
            // lineDraggableSplCon
            // 
            this.lineDraggableSplCon.BackColor = System.Drawing.Color.Black;
            this.lineDraggableSplCon.Dock = System.Windows.Forms.DockStyle.Top;
            this.lineDraggableSplCon.Location = new System.Drawing.Point(0, 0);
            this.lineDraggableSplCon.Margin = new System.Windows.Forms.Padding(0);
            this.lineDraggableSplCon.MaximumSize = new System.Drawing.Size(0, 1);
            this.lineDraggableSplCon.MinimumSize = new System.Drawing.Size(0, 1);
            this.lineDraggableSplCon.Name = "lineDraggableSplCon";
            this.lineDraggableSplCon.Size = new System.Drawing.Size(347, 1);
            this.lineDraggableSplCon.TabIndex = 0;
            // 
            // ViewMainSplCont
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.SplitContainer);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "ViewMainSplCont";
            this.Size = new System.Drawing.Size(347, 347);
            this.SplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).EndInit();
            this.SplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel lineDraggableSplCon;
    }
}
