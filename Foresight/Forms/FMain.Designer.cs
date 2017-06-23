namespace ScalableApps.Foresight.Win.Forms
{
    partial class FMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMain));
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblUserCaption = new System.Windows.Forms.Label();
            this.pnlCommandBar = new System.Windows.Forms.Panel();
            this.lblCommandBar = new System.Windows.Forms.Label();
            this.lvwCommandBar = new System.Windows.Forms.ListView();
            this.Command = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tabMain = new ScalableApps.Foresight.Win.Controls.iTabControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.updateLedgersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.companyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lnkAbout = new System.Windows.Forms.LinkLabel();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.pnlCommandBar.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.lblUserName);
            this.pnlHeader.Controls.Add(this.lblUserCaption);
            this.pnlHeader.Size = new System.Drawing.Size(784, 43);
            this.pnlHeader.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlHeader.Controls.SetChildIndex(this.lblUserCaption, 0);
            this.pnlHeader.Controls.SetChildIndex(this.lblUserName, 0);
            this.pnlHeader.Controls.SetChildIndex(this.picLogo, 0);
            // 
            // picLogo
            // 
            this.picLogo.Location = new System.Drawing.Point(741, 2);
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.Cyan;
            this.lblTitle.Location = new System.Drawing.Point(11, 9);
            this.lblTitle.Size = new System.Drawing.Size(73, 23);
            // 
            // lblUserName
            // 
            this.lblUserName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUserName.AutoSize = true;
            this.lblUserName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserName.ForeColor = System.Drawing.Color.Yellow;
            this.lblUserName.Location = new System.Drawing.Point(651, 13);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(84, 14);
            this.lblUserName.TabIndex = 8;
            this.lblUserName.Text = "Ashok Guduru";
            // 
            // lblUserCaption
            // 
            this.lblUserCaption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUserCaption.AutoSize = true;
            this.lblUserCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserCaption.ForeColor = System.Drawing.Color.White;
            this.lblUserCaption.Location = new System.Drawing.Point(610, 13);
            this.lblUserCaption.Name = "lblUserCaption";
            this.lblUserCaption.Size = new System.Drawing.Size(35, 14);
            this.lblUserCaption.TabIndex = 7;
            this.lblUserCaption.Text = "User:";
            // 
            // pnlCommandBar
            // 
            this.pnlCommandBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCommandBar.Controls.Add(this.lblCommandBar);
            this.pnlCommandBar.Controls.Add(this.lvwCommandBar);
            this.pnlCommandBar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlCommandBar.Location = new System.Drawing.Point(0, 67);
            this.pnlCommandBar.Name = "pnlCommandBar";
            this.pnlCommandBar.Size = new System.Drawing.Size(233, 473);
            this.pnlCommandBar.TabIndex = 0;
            this.pnlCommandBar.Resize += new System.EventHandler(this.pnlCommandBar_Resize);
            // 
            // lblCommandBar
            // 
            this.lblCommandBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCommandBar.BackColor = System.Drawing.Color.Maroon;
            this.lblCommandBar.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCommandBar.ForeColor = System.Drawing.Color.White;
            this.lblCommandBar.Location = new System.Drawing.Point(1, 1);
            this.lblCommandBar.Name = "lblCommandBar";
            this.lblCommandBar.Size = new System.Drawing.Size(229, 20);
            this.lblCommandBar.TabIndex = 0;
            this.lblCommandBar.Text = "Command bar";
            this.lblCommandBar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lvwCommandBar
            // 
            this.lvwCommandBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwCommandBar.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Command});
            this.lvwCommandBar.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvwCommandBar.FullRowSelect = true;
            this.lvwCommandBar.GridLines = true;
            this.lvwCommandBar.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvwCommandBar.HideSelection = false;
            this.lvwCommandBar.Location = new System.Drawing.Point(1, 22);
            this.lvwCommandBar.MultiSelect = false;
            this.lvwCommandBar.Name = "lvwCommandBar";
            this.lvwCommandBar.Size = new System.Drawing.Size(229, 448);
            this.lvwCommandBar.TabIndex = 0;
            this.lvwCommandBar.UseCompatibleStateImageBehavior = false;
            this.lvwCommandBar.View = System.Windows.Forms.View.Details;
            this.lvwCommandBar.DoubleClick += new System.EventHandler(this.lvwCommandBar_DoubleClick);
            this.lvwCommandBar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvwCommandBar_KeyDown);
            // 
            // Command
            // 
            this.Command.Text = "Command Name";
            this.Command.Width = 225;
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter1.Location = new System.Drawing.Point(233, 67);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 473);
            this.splitter1.TabIndex = 15;
            this.splitter1.TabStop = false;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 540);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
            this.statusStrip1.TabIndex = 14;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tabMain
            // 
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabMain.Location = new System.Drawing.Point(236, 67);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(548, 473);
            this.tabMain.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateLedgersToolStripMenuItem,
            this.companyToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 43);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // updateLedgersToolStripMenuItem
            // 
            this.updateLedgersToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateLedgersToolStripMenuItem.Name = "updateLedgersToolStripMenuItem";
            this.updateLedgersToolStripMenuItem.Size = new System.Drawing.Size(107, 20);
            this.updateLedgersToolStripMenuItem.Text = "&Update Ledgers";
            this.updateLedgersToolStripMenuItem.Click += new System.EventHandler(this.updateLedgersToolStripMenuItem_Click);
            // 
            // companyToolStripMenuItem
            // 
            this.companyToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.companyToolStripMenuItem.Name = "companyToolStripMenuItem";
            this.companyToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.companyToolStripMenuItem.Text = "&Company...";
            this.companyToolStripMenuItem.Click += new System.EventHandler(this.companyToolStripMenuItem_Click);
            // 
            // lnkAbout
            // 
            this.lnkAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkAbout.AutoSize = true;
            this.lnkAbout.LinkColor = System.Drawing.Color.Gray;
            this.lnkAbout.Location = new System.Drawing.Point(744, 50);
            this.lnkAbout.Name = "lnkAbout";
            this.lnkAbout.Size = new System.Drawing.Size(35, 13);
            this.lnkAbout.TabIndex = 3;
            this.lnkAbout.TabStop = true;
            this.lnkAbout.Text = "About";
            this.lnkAbout.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAbout_LinkClicked);
            // 
            // FMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.lnkAbout);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pnlCommandBar);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "FMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Foresight";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FMain_FormClosed);
            this.Load += new System.EventHandler(this.FMain_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FMain_KeyDown);
            this.Controls.SetChildIndex(this.pnlHeader, 0);
            this.Controls.SetChildIndex(this.menuStrip1, 0);
            this.Controls.SetChildIndex(this.statusStrip1, 0);
            this.Controls.SetChildIndex(this.pnlCommandBar, 0);
            this.Controls.SetChildIndex(this.splitter1, 0);
            this.Controls.SetChildIndex(this.tabMain, 0);
            this.Controls.SetChildIndex(this.lnkAbout, 0);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.pnlCommandBar.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlCommandBar;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblUserCaption;
        private System.Windows.Forms.ListView lvwCommandBar;
        private System.Windows.Forms.ColumnHeader Command;
        private System.Windows.Forms.Label lblCommandBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private ScalableApps.Foresight.Win.Controls.iTabControl tabMain;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem updateLedgersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem companyToolStripMenuItem;
        private System.Windows.Forms.LinkLabel lnkAbout;
    }
}