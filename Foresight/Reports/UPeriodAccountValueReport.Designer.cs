namespace ScalableApps.Foresight.Win.Reports
{
    partial class UPeriodAccountValueReport
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
            this.lvwReport = new System.Windows.Forms.ListView();
            this.btnSelectCompany = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnShowAll = new System.Windows.Forms.Button();
            this.nudTopNCount = new System.Windows.Forms.NumericUpDown();
            this.lblTopNCount = new System.Windows.Forms.Label();
            this.cmbAmtFormat = new System.Windows.Forms.ComboBox();
            this.lblAmtFormat = new System.Windows.Forms.Label();
            this.btnShow = new System.Windows.Forms.Button();
            this.chkPartyGrouping = new System.Windows.Forms.CheckBox();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopNCount)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Size = new System.Drawing.Size(551, 43);
            this.pnlHeader.TabIndex = 10;
            // 
            // lblTitle
            // 
            this.lblTitle.TabIndex = 0;
            // 
            // lblDescription
            // 
            this.lblDescription.TabIndex = 1;
            // 
            // lvwReport
            // 
            this.lvwReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwReport.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvwReport.FullRowSelect = true;
            this.lvwReport.GridLines = true;
            this.lvwReport.HideSelection = false;
            this.lvwReport.Location = new System.Drawing.Point(5, 76);
            this.lvwReport.MultiSelect = false;
            this.lvwReport.Name = "lvwReport";
            this.lvwReport.Size = new System.Drawing.Size(541, 271);
            this.lvwReport.TabIndex = 9;
            this.lvwReport.UseCompatibleStateImageBehavior = false;
            this.lvwReport.View = System.Windows.Forms.View.Details;
            this.lvwReport.DoubleClick += new System.EventHandler(this.lvwReport_DoubleClick);
            this.lvwReport.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lvwReport_KeyPress);
            // 
            // btnSelectCompany
            // 
            this.btnSelectCompany.Location = new System.Drawing.Point(71, 48);
            this.btnSelectCompany.Name = "btnSelectCompany";
            this.btnSelectCompany.Size = new System.Drawing.Size(60, 23);
            this.btnSelectCompany.TabIndex = 2;
            this.btnSelectCompany.Text = "Select...";
            this.btnSelectCompany.UseVisualStyleBackColor = true;
            this.btnSelectCompany.Click += new System.EventHandler(this.btnSelectCompany_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.Red;
            this.btnClose.Location = new System.Drawing.Point(523, 48);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(23, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnShowAll
            // 
            this.btnShowAll.Location = new System.Drawing.Point(5, 48);
            this.btnShowAll.Name = "btnShowAll";
            this.btnShowAll.Size = new System.Drawing.Size(60, 23);
            this.btnShowAll.TabIndex = 1;
            this.btnShowAll.Text = "Show All";
            this.btnShowAll.UseVisualStyleBackColor = true;
            this.btnShowAll.Click += new System.EventHandler(this.btnShowAll_Click);
            // 
            // nudTopNCount
            // 
            this.nudTopNCount.Location = new System.Drawing.Point(167, 49);
            this.nudTopNCount.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudTopNCount.Name = "nudTopNCount";
            this.nudTopNCount.Size = new System.Drawing.Size(42, 20);
            this.nudTopNCount.TabIndex = 4;
            this.nudTopNCount.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // lblTopNCount
            // 
            this.lblTopNCount.AutoSize = true;
            this.lblTopNCount.Location = new System.Drawing.Point(137, 53);
            this.lblTopNCount.Name = "lblTopNCount";
            this.lblTopNCount.Size = new System.Drawing.Size(29, 13);
            this.lblTopNCount.TabIndex = 3;
            this.lblTopNCount.Text = "&Top:";
            // 
            // cmbAmtFormat
            // 
            this.cmbAmtFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAmtFormat.FormattingEnabled = true;
            this.cmbAmtFormat.Location = new System.Drawing.Point(261, 49);
            this.cmbAmtFormat.Name = "cmbAmtFormat";
            this.cmbAmtFormat.Size = new System.Drawing.Size(80, 21);
            this.cmbAmtFormat.TabIndex = 6;
            this.cmbAmtFormat.SelectedIndexChanged += new System.EventHandler(this.cmbAmtFormat_SelectedIndexChanged);
            // 
            // lblAmtFormat
            // 
            this.lblAmtFormat.AutoSize = true;
            this.lblAmtFormat.Location = new System.Drawing.Point(218, 53);
            this.lblAmtFormat.Name = "lblAmtFormat";
            this.lblAmtFormat.Size = new System.Drawing.Size(42, 13);
            this.lblAmtFormat.TabIndex = 5;
            this.lblAmtFormat.Text = "&Format:";
            // 
            // btnShow
            // 
            this.btnShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShow.Location = new System.Drawing.Point(474, 48);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(43, 23);
            this.btnShow.TabIndex = 0;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // chkPartyGrouping
            // 
            this.chkPartyGrouping.AutoSize = true;
            this.chkPartyGrouping.Location = new System.Drawing.Point(347, 51);
            this.chkPartyGrouping.Name = "chkPartyGrouping";
            this.chkPartyGrouping.Size = new System.Drawing.Size(96, 17);
            this.chkPartyGrouping.TabIndex = 7;
            this.chkPartyGrouping.Text = "Party Grouping";
            this.chkPartyGrouping.UseVisualStyleBackColor = true;
            // 
            // UPeriodAccountValueReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkPartyGrouping);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.cmbAmtFormat);
            this.Controls.Add(this.lblAmtFormat);
            this.Controls.Add(this.nudTopNCount);
            this.Controls.Add(this.lblTopNCount);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvwReport);
            this.Controls.Add(this.btnShowAll);
            this.Controls.Add(this.btnSelectCompany);
            this.Name = "UPeriodAccountValueReport";
            this.Size = new System.Drawing.Size(551, 351);
            this.Load += new System.EventHandler(this.UPeriodAccountValueReport_Load);
            this.Controls.SetChildIndex(this.btnSelectCompany, 0);
            this.Controls.SetChildIndex(this.btnShowAll, 0);
            this.Controls.SetChildIndex(this.lvwReport, 0);
            this.Controls.SetChildIndex(this.pnlHeader, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.lblTopNCount, 0);
            this.Controls.SetChildIndex(this.nudTopNCount, 0);
            this.Controls.SetChildIndex(this.lblAmtFormat, 0);
            this.Controls.SetChildIndex(this.cmbAmtFormat, 0);
            this.Controls.SetChildIndex(this.btnShow, 0);
            this.Controls.SetChildIndex(this.chkPartyGrouping, 0);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopNCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvwReport;
        private System.Windows.Forms.Button btnSelectCompany;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnShowAll;
        private System.Windows.Forms.NumericUpDown nudTopNCount;
        private System.Windows.Forms.Label lblTopNCount;
        private System.Windows.Forms.ComboBox cmbAmtFormat;
        private System.Windows.Forms.Label lblAmtFormat;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.CheckBox chkPartyGrouping;
    }
}
