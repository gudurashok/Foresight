namespace ScalableApps.Foresight.Win.Reports
{
    partial class UBuyingTrendReport
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
            this.btnCompany = new System.Windows.Forms.Button();
            this.btnParty = new System.Windows.Forms.Button();
            this.txtParty = new System.Windows.Forms.TextBox();
            this.cmbAmtFormat = new System.Windows.Forms.ComboBox();
            this.lblAmtFormat = new System.Windows.Forms.Label();
            this.btnShow = new System.Windows.Forms.Button();
            this.lblParty = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Size = new System.Drawing.Size(551, 43);
            this.pnlHeader.TabIndex = 9;
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
            this.lvwReport.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvwReport.TabIndex = 8;
            this.lvwReport.UseCompatibleStateImageBehavior = false;
            this.lvwReport.View = System.Windows.Forms.View.Details;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.Red;
            this.btnClose.Location = new System.Drawing.Point(523, 48);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(23, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCompany
            // 
            this.btnCompany.Location = new System.Drawing.Point(4, 48);
            this.btnCompany.Name = "btnCompany";
            this.btnCompany.Size = new System.Drawing.Size(59, 23);
            this.btnCompany.TabIndex = 1;
            this.btnCompany.Text = "Company...";
            this.btnCompany.UseVisualStyleBackColor = true;
            this.btnCompany.Click += new System.EventHandler(this.btnCompany_Click);
            // 
            // btnParty
            // 
            this.btnParty.Location = new System.Drawing.Point(101, 48);
            this.btnParty.Name = "btnParty";
            this.btnParty.Size = new System.Drawing.Size(59, 23);
            this.btnParty.TabIndex = 3;
            this.btnParty.Text = "Select...";
            this.btnParty.UseVisualStyleBackColor = true;
            this.btnParty.Click += new System.EventHandler(this.btnParty_Click);
            // 
            // txtParty
            // 
            this.txtParty.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtParty.Location = new System.Drawing.Point(166, 49);
            this.txtParty.Name = "txtParty";
            this.txtParty.ReadOnly = true;
            this.txtParty.Size = new System.Drawing.Size(66, 20);
            this.txtParty.TabIndex = 4;
            this.txtParty.TabStop = false;
            // 
            // cmbAmtFormat
            // 
            this.cmbAmtFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbAmtFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAmtFormat.FormattingEnabled = true;
            this.cmbAmtFormat.Location = new System.Drawing.Point(282, 49);
            this.cmbAmtFormat.Name = "cmbAmtFormat";
            this.cmbAmtFormat.Size = new System.Drawing.Size(80, 21);
            this.cmbAmtFormat.TabIndex = 6;
            this.cmbAmtFormat.SelectedIndexChanged += new System.EventHandler(this.cmbAmtFormat_SelectedIndexChanged);
            // 
            // lblAmtFormat
            // 
            this.lblAmtFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAmtFormat.AutoSize = true;
            this.lblAmtFormat.Location = new System.Drawing.Point(238, 53);
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
            // lblParty
            // 
            this.lblParty.AutoSize = true;
            this.lblParty.Location = new System.Drawing.Point(67, 53);
            this.lblParty.Name = "lblParty";
            this.lblParty.Size = new System.Drawing.Size(34, 13);
            this.lblParty.TabIndex = 2;
            this.lblParty.Text = "&Party:";
            // 
            // UBuyingTrendReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblParty);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.cmbAmtFormat);
            this.Controls.Add(this.lblAmtFormat);
            this.Controls.Add(this.txtParty);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvwReport);
            this.Controls.Add(this.btnCompany);
            this.Controls.Add(this.btnParty);
            this.Name = "UBuyingTrendReport";
            this.Size = new System.Drawing.Size(551, 351);
            this.Load += new System.EventHandler(this.UBuyingTrendReport_Load);
            this.Controls.SetChildIndex(this.btnParty, 0);
            this.Controls.SetChildIndex(this.btnCompany, 0);
            this.Controls.SetChildIndex(this.lvwReport, 0);
            this.Controls.SetChildIndex(this.pnlHeader, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.txtParty, 0);
            this.Controls.SetChildIndex(this.lblAmtFormat, 0);
            this.Controls.SetChildIndex(this.cmbAmtFormat, 0);
            this.Controls.SetChildIndex(this.btnShow, 0);
            this.Controls.SetChildIndex(this.lblParty, 0);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvwReport;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCompany;
        private System.Windows.Forms.Button btnParty;
        private System.Windows.Forms.TextBox txtParty;
        private System.Windows.Forms.ComboBox cmbAmtFormat;
        private System.Windows.Forms.Label lblAmtFormat;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Label lblParty;
    }
}
