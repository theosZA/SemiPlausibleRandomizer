namespace SemiPlausibleRandomizer
{
    partial class MainForm
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
            this.RegionList = new System.Windows.Forms.CheckedListBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.SelectionRegionsInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.CreateModButton = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RegionList
            // 
            this.RegionList.FormattingEnabled = true;
            this.RegionList.Location = new System.Drawing.Point(12, 38);
            this.RegionList.Name = "RegionList";
            this.RegionList.Size = new System.Drawing.Size(175, 304);
            this.RegionList.TabIndex = 0;
            this.RegionList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.RegionList_ItemCheck);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelectionRegionsInfo});
            this.statusStrip1.Location = new System.Drawing.Point(0, 458);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(457, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // SelectionRegionsInfo
            // 
            this.SelectionRegionsInfo.Name = "SelectionRegionsInfo";
            this.SelectionRegionsInfo.Size = new System.Drawing.Size(163, 17);
            this.SelectionRegionsInfo.Text = "0 regions, 0 areas, 0 provinces";
            // 
            // CreateModButton
            // 
            this.CreateModButton.Location = new System.Drawing.Point(60, 348);
            this.CreateModButton.Name = "CreateModButton";
            this.CreateModButton.Size = new System.Drawing.Size(75, 23);
            this.CreateModButton.TabIndex = 2;
            this.CreateModButton.Text = "Create Mod";
            this.CreateModButton.UseVisualStyleBackColor = true;
            this.CreateModButton.Click += new System.EventHandler(this.CreateModButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 480);
            this.Controls.Add(this.CreateModButton);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.RegionList);
            this.Name = "MainForm";
            this.Text = "Semi-plausible Randomizer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox RegionList;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel SelectionRegionsInfo;
        private System.Windows.Forms.Button CreateModButton;
    }
}

