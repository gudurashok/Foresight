namespace Ferry.Win.Forms
{
    partial class FCompanyGroupPeriods
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCompanyGroupPeriods));
            this.pnlCommandBar = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lvwCoGroups = new System.Windows.Forms.ListView();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.lblImportFrom = new System.Windows.Forms.Label();
            this.txtImportFrom = new System.Windows.Forms.TextBox();
            this.btnImportFrom = new System.Windows.Forms.Button();
            this.lblProvider = new System.Windows.Forms.Label();
            this.lblProviderName = new System.Windows.Forms.Label();
            this.lvwCoPeriods = new System.Windows.Forms.ListView();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.pnlCommandBar.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Size = new System.Drawing.Size(348, 43);
            this.pnlHeader.TabIndex = 0;
            // 
            // picLogo
            // 
            this.picLogo.Location = new System.Drawing.Point(305, 2);
            // 
            // lblTitle
            // 
            this.lblTitle.Size = new System.Drawing.Size(302, 14);
            this.lblTitle.Text = "Enter company group details. Optionally import details";
            // 
            // pnlCommandBar
            // 
            this.pnlCommandBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCommandBar.BackColor = System.Drawing.SystemColors.Control;
            this.pnlCommandBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCommandBar.Controls.Add(this.btnSave);
            this.pnlCommandBar.Controls.Add(this.btnClose);
            this.pnlCommandBar.Location = new System.Drawing.Point(-1, 380);
            this.pnlCommandBar.Name = "pnlCommandBar";
            this.pnlCommandBar.Size = new System.Drawing.Size(350, 35);
            this.pnlCommandBar.TabIndex = 4;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.Location = new System.Drawing.Point(137, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(270, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lvwCoGroups
            // 
            this.lvwCoGroups.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwCoGroups.CheckBoxes = true;
            this.lvwCoGroups.FullRowSelect = true;
            this.lvwCoGroups.GridLines = true;
            this.lvwCoGroups.HideSelection = false;
            this.lvwCoGroups.Location = new System.Drawing.Point(2, 94);
            this.lvwCoGroups.MultiSelect = false;
            this.lvwCoGroups.Name = "lvwCoGroups";
            this.lvwCoGroups.Size = new System.Drawing.Size(343, 99);
            this.lvwCoGroups.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvwCoGroups.TabIndex = 2;
            this.lvwCoGroups.UseCompatibleStateImageBehavior = false;
            this.lvwCoGroups.View = System.Windows.Forms.View.Details;
            this.lvwCoGroups.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvwCoGroups_ItemChecked);
            // 
            // pnlDetails
            // 
            this.pnlDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDetails.Controls.Add(this.lblImportFrom);
            this.pnlDetails.Controls.Add(this.txtImportFrom);
            this.pnlDetails.Controls.Add(this.btnImportFrom);
            this.pnlDetails.Controls.Add(this.lblProvider);
            this.pnlDetails.Controls.Add(this.lblProviderName);
            this.pnlDetails.Location = new System.Drawing.Point(2, 45);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Size = new System.Drawing.Size(343, 47);
            this.pnlDetails.TabIndex = 1;
            // 
            // lblImportFrom
            // 
            this.lblImportFrom.AutoSize = true;
            this.lblImportFrom.Location = new System.Drawing.Point(0, 0);
            this.lblImportFrom.Name = "lblImportFrom";
            this.lblImportFrom.Size = new System.Drawing.Size(65, 13);
            this.lblImportFrom.TabIndex = 5;
            this.lblImportFrom.Text = "&Import From:";
            // 
            // txtImportFrom
            // 
            this.txtImportFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtImportFrom.Location = new System.Drawing.Point(70, 2);
            this.txtImportFrom.Name = "txtImportFrom";
            this.txtImportFrom.ReadOnly = true;
            this.txtImportFrom.Size = new System.Drawing.Size(244, 20);
            this.txtImportFrom.TabIndex = 6;
            this.txtImportFrom.TabStop = false;
            // 
            // btnImportFrom
            // 
            this.btnImportFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportFrom.Location = new System.Drawing.Point(314, 1);
            this.btnImportFrom.Name = "btnImportFrom";
            this.btnImportFrom.Size = new System.Drawing.Size(25, 21);
            this.btnImportFrom.TabIndex = 7;
            this.btnImportFrom.Text = "...";
            this.btnImportFrom.UseVisualStyleBackColor = true;
            this.btnImportFrom.Click += new System.EventHandler(this.btnImportFrom_Click);
            // 
            // lblProvider
            // 
            this.lblProvider.AutoSize = true;
            this.lblProvider.Location = new System.Drawing.Point(0, 21);
            this.lblProvider.Name = "lblProvider";
            this.lblProvider.Size = new System.Drawing.Size(49, 13);
            this.lblProvider.TabIndex = 8;
            this.lblProvider.Text = "&Provider:";
            // 
            // lblProviderName
            // 
            this.lblProviderName.AutoSize = true;
            this.lblProviderName.Location = new System.Drawing.Point(67, 24);
            this.lblProviderName.Name = "lblProviderName";
            this.lblProviderName.Size = new System.Drawing.Size(0, 13);
            this.lblProviderName.TabIndex = 9;
            // 
            // lvwCoPeriods
            // 
            this.lvwCoPeriods.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwCoPeriods.CheckBoxes = true;
            this.lvwCoPeriods.FullRowSelect = true;
            this.lvwCoPeriods.GridLines = true;
            this.lvwCoPeriods.HideSelection = false;
            this.lvwCoPeriods.Location = new System.Drawing.Point(2, 195);
            this.lvwCoPeriods.MultiSelect = false;
            this.lvwCoPeriods.Name = "lvwCoPeriods";
            this.lvwCoPeriods.Size = new System.Drawing.Size(343, 182);
            this.lvwCoPeriods.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvwCoPeriods.TabIndex = 3;
            this.lvwCoPeriods.UseCompatibleStateImageBehavior = false;
            this.lvwCoPeriods.View = System.Windows.Forms.View.Details;
            this.lvwCoPeriods.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lvwCoPeriods_ItemCheck);
            // 
            // FCompanyGroup
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(348, 414);
            this.Controls.Add(this.pnlCommandBar);
            this.Controls.Add(this.lvwCoGroups);
            this.Controls.Add(this.pnlDetails);
            this.Controls.Add(this.lvwCoPeriods);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 370);
            this.Name = "FCompanyGroup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Company groups";
            this.Resize += new System.EventHandler(this.FCompanyGroup_Resize);
            this.Controls.SetChildIndex(this.pnlHeader, 0);
            this.Controls.SetChildIndex(this.lvwCoPeriods, 0);
            this.Controls.SetChildIndex(this.pnlDetails, 0);
            this.Controls.SetChildIndex(this.lvwCoGroups, 0);
            this.Controls.SetChildIndex(this.pnlCommandBar, 0);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.pnlCommandBar.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
            this.pnlDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlCommandBar;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListView lvwCoGroups;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.Label lblProvider;
        private System.Windows.Forms.Label lblProviderName;
        private System.Windows.Forms.ListView lvwCoPeriods;
        private System.Windows.Forms.Label lblImportFrom;
        private System.Windows.Forms.TextBox txtImportFrom;
        private System.Windows.Forms.Button btnImportFrom;
    }
}