namespace Scalable.Win.Controls
{
    partial class NumTextBox
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
            this.btnDropDown = new System.Windows.Forms.Button();
            this.txtDisplay = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnDropDown
            // 
            this.btnDropDown.Font = new System.Drawing.Font("Tahoma", 6F);
            this.btnDropDown.Location = new System.Drawing.Point(85, 0);
            this.btnDropDown.Name = "btnDropDown";
            this.btnDropDown.Size = new System.Drawing.Size(26, 20);
            this.btnDropDown.TabIndex = 3;
            this.btnDropDown.Text = "123";
            this.btnDropDown.Click += new System.EventHandler(this.btnDropDown_Click);
            this.btnDropDown.Enter += new System.EventHandler(this.btnDropDown_Enter);
            this.btnDropDown.Leave += new System.EventHandler(this.btnDropDown_Leave);
            // 
            // txtDisplay
            // 
            this.txtDisplay.Location = new System.Drawing.Point(0, 0);
            this.txtDisplay.Name = "txtDisplay";
            this.txtDisplay.Size = new System.Drawing.Size(86, 20);
            this.txtDisplay.TabIndex = 2;
            this.txtDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDisplay.TextChanged += new System.EventHandler(this.txtDisplay_TextChanged);
            this.txtDisplay.Enter += new System.EventHandler(this.txtDisplay_Enter);
            this.txtDisplay.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDisplay_KeyPress);
            this.txtDisplay.Leave += new System.EventHandler(this.txtDisplay_Leave);
            // 
            // NumTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDropDown);
            this.Controls.Add(this.txtDisplay);
            this.Name = "NumTextBox";
            this.Size = new System.Drawing.Size(111, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDropDown;
        private System.Windows.Forms.TextBox txtDisplay;
    }
}
