namespace ScalableApps.Foresight.Win.Forms
{
    partial class FAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FAbout));
            this.pnlCopyrightNotice = new System.Windows.Forms.Panel();
            this.lblCopyrightNotice = new System.Windows.Forms.Label();
            this.picTitle = new System.Windows.Forms.PictureBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lnkiScalableWeb = new System.Windows.Forms.LinkLabel();
            this.lnkEMail = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.pnlCopyrightNotice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlCopyrightNotice
            // 
            this.pnlCopyrightNotice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCopyrightNotice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCopyrightNotice.Controls.Add(this.lblCopyrightNotice);
            this.pnlCopyrightNotice.Location = new System.Drawing.Point(-2, 231);
            this.pnlCopyrightNotice.Name = "pnlCopyrightNotice";
            this.pnlCopyrightNotice.Size = new System.Drawing.Size(450, 61);
            this.pnlCopyrightNotice.TabIndex = 0;
            // 
            // lblCopyrightNotice
            // 
            this.lblCopyrightNotice.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCopyrightNotice.Location = new System.Drawing.Point(1, 2);
            this.lblCopyrightNotice.Name = "lblCopyrightNotice";
            this.lblCopyrightNotice.Size = new System.Drawing.Size(444, 56);
            this.lblCopyrightNotice.TabIndex = 0;
            this.lblCopyrightNotice.Text = resources.GetString("lblCopyrightNotice.Text");
            // 
            // picTitle
            // 
            this.picTitle.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picTitle.BackgroundImage")));
            this.picTitle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picTitle.Location = new System.Drawing.Point(91, 22);
            this.picTitle.Name = "picTitle";
            this.picTitle.Size = new System.Drawing.Size(265, 75);
            this.picTitle.TabIndex = 1;
            this.picTitle.TabStop = false;
            // 
            // picLogo
            // 
            this.picLogo.Image = global::ScalableApps.Foresight.Win.Properties.Resources.Foresight;
            this.picLogo.Location = new System.Drawing.Point(201, 103);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(44, 43);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLogo.TabIndex = 6;
            this.picLogo.TabStop = false;
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVersion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.Location = new System.Drawing.Point(146, 156);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(167, 14);
            this.lblVersion.TabIndex = 7;
            this.lblVersion.Text = "Version: 1.0.0.0000";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lnkiScalableWeb
            // 
            this.lnkiScalableWeb.AutoSize = true;
            this.lnkiScalableWeb.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkiScalableWeb.Location = new System.Drawing.Point(118, 173);
            this.lnkiScalableWeb.Name = "lnkiScalableWeb";
            this.lnkiScalableWeb.Size = new System.Drawing.Size(210, 14);
            this.lnkiScalableWeb.TabIndex = 8;
            this.lnkiScalableWeb.TabStop = true;
            this.lnkiScalableWeb.Text = "Visit us at: http://www.iscalable.com";
            this.lnkiScalableWeb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkiScalableWeb_LinkClicked);
            // 
            // lnkEMail
            // 
            this.lnkEMail.AutoSize = true;
            this.lnkEMail.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkEMail.Location = new System.Drawing.Point(136, 193);
            this.lnkEMail.Name = "lnkEMail";
            this.lnkEMail.Size = new System.Drawing.Size(159, 14);
            this.lnkEMail.TabIndex = 9;
            this.lnkEMail.TabStop = true;
            this.lnkEMail.Text = "Email us: info@iscalable.com";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(161, 213);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(125, 14);
            this.linkLabel1.TabIndex = 10;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Call us: 09989923219";
            // 
            // lblCopyright
            // 
            this.lblCopyright.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCopyright.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCopyright.Location = new System.Drawing.Point(12, 297);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(423, 13);
            this.lblCopyright.TabIndex = 11;
            this.lblCopyright.Text = "© 2012 Scalable Software Solutions Private Limited. All rights reserved.";
            this.lblCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(447, 313);
            this.ControlBox = false;
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.lnkEMail);
            this.Controls.Add(this.lnkiScalableWeb);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.picLogo);
            this.Controls.Add(this.picTitle);
            this.Controls.Add(this.pnlCopyrightNotice);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FAbout";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About Foresight";
            this.Load += new System.EventHandler(this.FAbout_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FAbout_KeyDown);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FAbout_MouseClick);
            this.pnlCopyrightNotice.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlCopyrightNotice;
        private System.Windows.Forms.Label lblCopyrightNotice;
        private System.Windows.Forms.PictureBox picTitle;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.LinkLabel lnkiScalableWeb;
        private System.Windows.Forms.LinkLabel lnkEMail;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label lblCopyright;
    }
}