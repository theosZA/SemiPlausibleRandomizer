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
            this.SuspendLayout();
            // 
            // RegionList
            // 
            this.RegionList.FormattingEnabled = true;
            this.RegionList.Location = new System.Drawing.Point(12, 38);
            this.RegionList.Name = "RegionList";
            this.RegionList.Size = new System.Drawing.Size(175, 304);
            this.RegionList.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 480);
            this.Controls.Add(this.RegionList);
            this.Name = "MainForm";
            this.Text = "Semi-plausible Randomizer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox RegionList;
    }
}

