namespace ForesightDataUpdateGUI
{
    partial class ForesightDataUpdate
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
            this.txtForesightSdfFilePath = new System.Windows.Forms.TextBox();
            this.btnFindForesightFile = new System.Windows.Forms.Button();
            this.ofdForesightFile = new System.Windows.Forms.OpenFileDialog();
            this.txtForesightDataScriptFilePath = new System.Windows.Forms.TextBox();
            this.btnFindForesightDataFile = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lblRootDb = new System.Windows.Forms.Label();
            this.lblScriptFilePath = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtForesightSdfFilePath
            // 
            this.txtForesightSdfFilePath.Location = new System.Drawing.Point(12, 24);
            this.txtForesightSdfFilePath.Name = "txtForesightSdfFilePath";
            this.txtForesightSdfFilePath.ReadOnly = true;
            this.txtForesightSdfFilePath.Size = new System.Drawing.Size(343, 20);
            this.txtForesightSdfFilePath.TabIndex = 0;
            // 
            // btnFindForesightFile
            // 
            this.btnFindForesightFile.Location = new System.Drawing.Point(361, 24);
            this.btnFindForesightFile.Name = "btnFindForesightFile";
            this.btnFindForesightFile.Size = new System.Drawing.Size(22, 20);
            this.btnFindForesightFile.TabIndex = 0;
            this.btnFindForesightFile.Text = "...";
            this.btnFindForesightFile.UseVisualStyleBackColor = true;
            this.btnFindForesightFile.Click += new System.EventHandler(this.btnFindForesightFile_Click);
            // 
            // ofdForesightFile
            // 
            this.ofdForesightFile.Filter = "sdf|*.sdf|isd|*.isd";
            // 
            // txtForesightDataScriptFilePath
            // 
            this.txtForesightDataScriptFilePath.Location = new System.Drawing.Point(12, 65);
            this.txtForesightDataScriptFilePath.Name = "txtForesightDataScriptFilePath";
            this.txtForesightDataScriptFilePath.ReadOnly = true;
            this.txtForesightDataScriptFilePath.Size = new System.Drawing.Size(343, 20);
            this.txtForesightDataScriptFilePath.TabIndex = 0;
            // 
            // btnFindForesightDataFile
            // 
            this.btnFindForesightDataFile.Location = new System.Drawing.Point(361, 65);
            this.btnFindForesightDataFile.Name = "btnFindForesightDataFile";
            this.btnFindForesightDataFile.Size = new System.Drawing.Size(22, 20);
            this.btnFindForesightDataFile.TabIndex = 1;
            this.btnFindForesightDataFile.Text = "...";
            this.btnFindForesightDataFile.UseVisualStyleBackColor = true;
            this.btnFindForesightDataFile.Click += new System.EventHandler(this.btnFindForesightDataFile_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(160, 91);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lblRootDb
            // 
            this.lblRootDb.AutoSize = true;
            this.lblRootDb.Location = new System.Drawing.Point(12, 8);
            this.lblRootDb.Name = "lblRootDb";
            this.lblRootDb.Size = new System.Drawing.Size(104, 13);
            this.lblRootDb.TabIndex = 3;
            this.lblRootDb.Text = "Root database path:";
            // 
            // lblScriptFilePath
            // 
            this.lblScriptFilePath.AutoSize = true;
            this.lblScriptFilePath.Location = new System.Drawing.Point(12, 49);
            this.lblScriptFilePath.Name = "lblScriptFilePath";
            this.lblScriptFilePath.Size = new System.Drawing.Size(77, 13);
            this.lblScriptFilePath.TabIndex = 4;
            this.lblScriptFilePath.Text = "Script file path:";
            // 
            // ForesightDataUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 131);
            this.Controls.Add(this.lblScriptFilePath);
            this.Controls.Add(this.lblRootDb);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnFindForesightDataFile);
            this.Controls.Add(this.btnFindForesightFile);
            this.Controls.Add(this.txtForesightDataScriptFilePath);
            this.Controls.Add(this.txtForesightSdfFilePath);
            this.Name = "ForesightDataUpdate";
            this.Text = "ForesightData Update";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtForesightSdfFilePath;
        private System.Windows.Forms.Button btnFindForesightFile;
        private System.Windows.Forms.OpenFileDialog ofdForesightFile;
        private System.Windows.Forms.TextBox txtForesightDataScriptFilePath;
        private System.Windows.Forms.Button btnFindForesightDataFile;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label lblRootDb;
        private System.Windows.Forms.Label lblScriptFilePath;
    }
}

