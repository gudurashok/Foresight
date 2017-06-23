namespace Ferry.Win.Forms
{
    partial class FCompany
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCompany));
            this.lvwList = new System.Windows.Forms.ListView();
            this.pnlPeriodCommandBar = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnEditPeriod = new System.Windows.Forms.Button();
            this.btnDeletePeriod = new System.Windows.Forms.Button();
            this.btnAddPeriods = new System.Windows.Forms.Button();
            this.btnAddPeriod = new System.Windows.Forms.Button();
            this.txtPeriodDataPath = new System.Windows.Forms.TextBox();
            this.lblPeriodDataPath = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.pnlPeriodCommandBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Size = new System.Drawing.Size(561, 43);
            // 
            // picLogo
            // 
            this.picLogo.Location = new System.Drawing.Point(518, 2);
            // 
            // lblTitle
            // 
            this.lblTitle.Size = new System.Drawing.Size(451, 14);
            this.lblTitle.Text = "Select company, company-periods to be imported. Optionally define new periods";
            // 
            // lvwList
            // 
            this.lvwList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwList.CheckBoxes = true;
            this.lvwList.FullRowSelect = true;
            this.lvwList.GridLines = true;
            this.lvwList.HideSelection = false;
            this.lvwList.Location = new System.Drawing.Point(7, 48);
            this.lvwList.MultiSelect = false;
            this.lvwList.Name = "lvwList";
            this.lvwList.Size = new System.Drawing.Size(547, 263);
            this.lvwList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvwList.TabIndex = 0;
            this.lvwList.UseCompatibleStateImageBehavior = false;
            this.lvwList.View = System.Windows.Forms.View.Details;
            this.lvwList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lvwList_ItemCheck);
            this.lvwList.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvwList_ItemChecked);
            this.lvwList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvwList_ItemSelectionChanged);
            // 
            // pnlPeriodCommandBar
            // 
            this.pnlPeriodCommandBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPeriodCommandBar.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlPeriodCommandBar.Controls.Add(this.btnClose);
            this.pnlPeriodCommandBar.Controls.Add(this.btnImport);
            this.pnlPeriodCommandBar.Controls.Add(this.btnEditPeriod);
            this.pnlPeriodCommandBar.Controls.Add(this.btnDeletePeriod);
            this.pnlPeriodCommandBar.Controls.Add(this.btnAddPeriods);
            this.pnlPeriodCommandBar.Controls.Add(this.btnAddPeriod);
            this.pnlPeriodCommandBar.Location = new System.Drawing.Point(-2, 341);
            this.pnlPeriodCommandBar.Name = "pnlPeriodCommandBar";
            this.pnlPeriodCommandBar.Size = new System.Drawing.Size(565, 35);
            this.pnlPeriodCommandBar.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(479, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Enabled = false;
            this.btnImport.Location = new System.Drawing.Point(361, 4);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 3;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnEditPeriod
            // 
            this.btnEditPeriod.Enabled = false;
            this.btnEditPeriod.Location = new System.Drawing.Point(166, 4);
            this.btnEditPeriod.Name = "btnEditPeriod";
            this.btnEditPeriod.Size = new System.Drawing.Size(75, 23);
            this.btnEditPeriod.TabIndex = 1;
            this.btnEditPeriod.Text = "Edit";
            this.btnEditPeriod.UseVisualStyleBackColor = true;
            this.btnEditPeriod.Click += new System.EventHandler(this.btnEditPeriod_Click);
            // 
            // btnDeletePeriod
            // 
            this.btnDeletePeriod.Enabled = false;
            this.btnDeletePeriod.Location = new System.Drawing.Point(247, 4);
            this.btnDeletePeriod.Name = "btnDeletePeriod";
            this.btnDeletePeriod.Size = new System.Drawing.Size(75, 23);
            this.btnDeletePeriod.TabIndex = 2;
            this.btnDeletePeriod.Text = "Delete";
            this.btnDeletePeriod.UseVisualStyleBackColor = true;
            this.btnDeletePeriod.Click += new System.EventHandler(this.btnDeletePeriod_Click);
            // 
            // btnAddPeriods
            // 
            this.btnAddPeriods.Location = new System.Drawing.Point(3, 4);
            this.btnAddPeriods.Name = "btnAddPeriods";
            this.btnAddPeriods.Size = new System.Drawing.Size(75, 23);
            this.btnAddPeriods.TabIndex = 0;
            this.btnAddPeriods.Text = "Add Periods";
            this.btnAddPeriods.UseVisualStyleBackColor = true;
            this.btnAddPeriods.Click += new System.EventHandler(this.btnAddPeriods_Click);
            // 
            // btnAddPeriod
            // 
            this.btnAddPeriod.Location = new System.Drawing.Point(85, 4);
            this.btnAddPeriod.Name = "btnAddPeriod";
            this.btnAddPeriod.Size = new System.Drawing.Size(75, 23);
            this.btnAddPeriod.TabIndex = 0;
            this.btnAddPeriod.Text = "Add Period";
            this.btnAddPeriod.UseVisualStyleBackColor = true;
            this.btnAddPeriod.Click += new System.EventHandler(this.btnAddPeriod_Click);
            // 
            // txtPeriodDataPath
            // 
            this.txtPeriodDataPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPeriodDataPath.BackColor = System.Drawing.Color.White;
            this.txtPeriodDataPath.Location = new System.Drawing.Point(36, 315);
            this.txtPeriodDataPath.Name = "txtPeriodDataPath";
            this.txtPeriodDataPath.ReadOnly = true;
            this.txtPeriodDataPath.Size = new System.Drawing.Size(518, 20);
            this.txtPeriodDataPath.TabIndex = 9;
            this.txtPeriodDataPath.TabStop = false;
            // 
            // lblPeriodDataPath
            // 
            this.lblPeriodDataPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPeriodDataPath.AutoSize = true;
            this.lblPeriodDataPath.Location = new System.Drawing.Point(4, 318);
            this.lblPeriodDataPath.Name = "lblPeriodDataPath";
            this.lblPeriodDataPath.Size = new System.Drawing.Size(32, 13);
            this.lblPeriodDataPath.TabIndex = 10;
            this.lblPeriodDataPath.Text = "Path:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(4, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(327, 14);
            this.label1.TabIndex = 8;
            this.label1.Text = "Optionally you can add, edit, and delete company periods.";
            // 
            // FCompany
            // 
            this.AcceptButton = this.btnImport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(561, 376);
            this.Controls.Add(this.lblPeriodDataPath);
            this.Controls.Add(this.txtPeriodDataPath);
            this.Controls.Add(this.pnlPeriodCommandBar);
            this.Controls.Add(this.lvwList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(480, 345);
            this.Name = "FCompany";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FCompany";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FCompany_FormClosed);
            this.Load += new System.EventHandler(this.FCompany_Load);
            this.Controls.SetChildIndex(this.lvwList, 0);
            this.Controls.SetChildIndex(this.pnlPeriodCommandBar, 0);
            this.Controls.SetChildIndex(this.txtPeriodDataPath, 0);
            this.Controls.SetChildIndex(this.lblPeriodDataPath, 0);
            this.Controls.SetChildIndex(this.pnlHeader, 0);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.pnlPeriodCommandBar.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvwList;
        private System.Windows.Forms.Panel pnlPeriodCommandBar;
        private System.Windows.Forms.Button btnDeletePeriod;
        private System.Windows.Forms.Button btnAddPeriod;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.TextBox txtPeriodDataPath;
        private System.Windows.Forms.Label lblPeriodDataPath;
        private System.Windows.Forms.Button btnEditPeriod;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAddPeriods;

    }
}