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
            this.countryCount0 = new System.Windows.Forms.NumericUpDown();
            this.developmentLimit0 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.developmentLimit1 = new System.Windows.Forms.NumericUpDown();
            this.countryCount1 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.developmentLimit2 = new System.Windows.Forms.NumericUpDown();
            this.countryCount2 = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.developmentLimitFinal = new System.Windows.Forms.NumericUpDown();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.countryCount0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.developmentLimit0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.developmentLimit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.countryCount1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.developmentLimit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.countryCount2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.developmentLimitFinal)).BeginInit();
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
            // countryCount0
            // 
            this.countryCount0.Location = new System.Drawing.Point(193, 38);
            this.countryCount0.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.countryCount0.Name = "countryCount0";
            this.countryCount0.Size = new System.Drawing.Size(50, 20);
            this.countryCount0.TabIndex = 3;
            // 
            // developmentLimit0
            // 
            this.developmentLimit0.Location = new System.Drawing.Point(329, 38);
            this.developmentLimit0.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.developmentLimit0.Name = "developmentLimit0";
            this.developmentLimit0.Size = new System.Drawing.Size(53, 20);
            this.developmentLimit0.TabIndex = 4;
            this.developmentLimit0.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(249, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "countries over";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(388, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "development";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(388, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "development";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(249, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "countries over";
            // 
            // developmentLimit1
            // 
            this.developmentLimit1.Location = new System.Drawing.Point(329, 62);
            this.developmentLimit1.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.developmentLimit1.Name = "developmentLimit1";
            this.developmentLimit1.Size = new System.Drawing.Size(53, 20);
            this.developmentLimit1.TabIndex = 8;
            this.developmentLimit1.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // countryCount1
            // 
            this.countryCount1.Location = new System.Drawing.Point(193, 62);
            this.countryCount1.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.countryCount1.Name = "countryCount1";
            this.countryCount1.Size = new System.Drawing.Size(50, 20);
            this.countryCount1.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(388, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "development";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(249, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "countries over";
            // 
            // developmentLimit2
            // 
            this.developmentLimit2.Location = new System.Drawing.Point(329, 85);
            this.developmentLimit2.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.developmentLimit2.Name = "developmentLimit2";
            this.developmentLimit2.Size = new System.Drawing.Size(53, 20);
            this.developmentLimit2.TabIndex = 12;
            this.developmentLimit2.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // countryCount2
            // 
            this.countryCount2.Location = new System.Drawing.Point(193, 85);
            this.countryCount2.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.countryCount2.Name = "countryCount2";
            this.countryCount2.Size = new System.Drawing.Size(50, 20);
            this.countryCount2.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(388, 112);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "development";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(205, 112);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(118, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "unlimited countries over";
            // 
            // developmentLimitFinal
            // 
            this.developmentLimitFinal.Location = new System.Drawing.Point(329, 110);
            this.developmentLimitFinal.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.developmentLimitFinal.Name = "developmentLimitFinal";
            this.developmentLimitFinal.Size = new System.Drawing.Size(53, 20);
            this.developmentLimitFinal.TabIndex = 16;
            this.developmentLimitFinal.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 480);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.developmentLimitFinal);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.developmentLimit2);
            this.Controls.Add(this.countryCount2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.developmentLimit1);
            this.Controls.Add(this.countryCount1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.developmentLimit0);
            this.Controls.Add(this.countryCount0);
            this.Controls.Add(this.CreateModButton);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.RegionList);
            this.Name = "MainForm";
            this.Text = "Semi-plausible Randomizer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.countryCount0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.developmentLimit0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.developmentLimit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.countryCount1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.developmentLimit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.countryCount2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.developmentLimitFinal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox RegionList;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel SelectionRegionsInfo;
        private System.Windows.Forms.Button CreateModButton;
        private System.Windows.Forms.NumericUpDown countryCount0;
        private System.Windows.Forms.NumericUpDown developmentLimit0;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown developmentLimit1;
        private System.Windows.Forms.NumericUpDown countryCount1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown developmentLimit2;
        private System.Windows.Forms.NumericUpDown countryCount2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown developmentLimitFinal;
    }
}

