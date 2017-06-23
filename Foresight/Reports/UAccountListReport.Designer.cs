namespace ScalableApps.Foresight.Win.Reports
{
    partial class UAccountListReport
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnShowLedger = new System.Windows.Forms.Button();
            this.btnInvertSelection = new System.Windows.Forms.Button();
            this.chkPartyGrouping = new System.Windows.Forms.CheckBox();
            this.cmbAmtFormat = new System.Windows.Forms.ComboBox();
            this.lblAmtFormat = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Size = new System.Drawing.Size(551, 43);
            this.pnlHeader.TabIndex = 5;
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
            this.lvwReport.TabIndex = 2;
            this.lvwReport.UseCompatibleStateImageBehavior = false;
            this.lvwReport.View = System.Windows.Forms.View.Details;
            this.lvwReport.SelectedIndexChanged += new System.EventHandler(this.lvwReport_SelectedIndexChanged);
            this.lvwReport.DoubleClick += new System.EventHandler(this.lvwReport_DoubleClick);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.Red;
            this.btnClose.Location = new System.Drawing.Point(523, 48);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(23, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnShowLedger
            // 
            this.btnShowLedger.Location = new System.Drawing.Point(5, 48);
            this.btnShowLedger.Name = "btnShowLedger";
            this.btnShowLedger.Size = new System.Drawing.Size(91, 23);
            this.btnShowLedger.TabIndex = 1;
            this.btnShowLedger.Text = "&Show Ledger...";
            this.btnShowLedger.UseVisualStyleBackColor = true;
            this.btnShowLedger.Click += new System.EventHandler(this.btnShowLedger_Click);
            // 
            // btnInvertSelection
            // 
            this.btnInvertSelection.Location = new System.Drawing.Point(102, 48);
            this.btnInvertSelection.Name = "btnInvertSelection";
            this.btnInvertSelection.Size = new System.Drawing.Size(89, 23);
            this.btnInvertSelection.TabIndex = 3;
            this.btnInvertSelection.Text = "Invert Selection";
            this.btnInvertSelection.UseVisualStyleBackColor = true;
            this.btnInvertSelection.Click += new System.EventHandler(this.btnInvertSelection_Click);
            // 
            // chkPartyGrouping
            // 
            this.chkPartyGrouping.AutoSize = true;
            this.chkPartyGrouping.Location = new System.Drawing.Point(197, 51);
            this.chkPartyGrouping.Name = "chkPartyGrouping";
            this.chkPartyGrouping.Size = new System.Drawing.Size(96, 17);
            this.chkPartyGrouping.TabIndex = 4;
            this.chkPartyGrouping.Text = "Party Grouping";
            this.chkPartyGrouping.UseVisualStyleBackColor = true;
            this.chkPartyGrouping.Visible = false;
            this.chkPartyGrouping.CheckedChanged += new System.EventHandler(this.chkPartyGrouping_CheckedChanged);
            // 
            // cmbAmtFormat
            // 
            this.cmbAmtFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAmtFormat.FormattingEnabled = true;
            this.cmbAmtFormat.Location = new System.Drawing.Point(356, 50);
            this.cmbAmtFormat.Name = "cmbAmtFormat";
            this.cmbAmtFormat.Size = new System.Drawing.Size(80, 21);
            this.cmbAmtFormat.TabIndex = 7;
            this.cmbAmtFormat.SelectedIndexChanged += new System.EventHandler(this.cmbAmtFormat_SelectedIndexChanged);
            // 
            // lblAmtFormat
            // 
            this.lblAmtFormat.AutoSize = true;
            this.lblAmtFormat.Location = new System.Drawing.Point(308, 54);
            this.lblAmtFormat.Name = "lblAmtFormat";
            this.lblAmtFormat.Size = new System.Drawing.Size(42, 13);
            this.lblAmtFormat.TabIndex = 6;
            this.lblAmtFormat.Text = "&Format:";
            // 
            // UAccountListReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbAmtFormat);
            this.Controls.Add(this.lblAmtFormat);
            this.Controls.Add(this.chkPartyGrouping);
            this.Controls.Add(this.btnInvertSelection);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnShowLedger);
            this.Controls.Add(this.lvwReport);
            this.Name = "UAccountListReport";
            this.Size = new System.Drawing.Size(551, 351);
            this.Load += new System.EventHandler(this.UAccountListReport_Load);
            this.Controls.SetChildIndex(this.pnlHeader, 0);
            this.Controls.SetChildIndex(this.lvwReport, 0);
            this.Controls.SetChildIndex(this.btnShowLedger, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnInvertSelection, 0);
            this.Controls.SetChildIndex(this.chkPartyGrouping, 0);
            this.Controls.SetChildIndex(this.lblAmtFormat, 0);
            this.Controls.SetChildIndex(this.cmbAmtFormat, 0);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        
        #endregion

        private System.Windows.Forms.ListView lvwReport;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnShowLedger;
        private System.Windows.Forms.Button btnInvertSelection;
        private System.Windows.Forms.CheckBox chkPartyGrouping;
        private System.Windows.Forms.ComboBox cmbAmtFormat;
        private System.Windows.Forms.Label lblAmtFormat;
    }
}
