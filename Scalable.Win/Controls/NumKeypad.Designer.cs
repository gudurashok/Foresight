namespace Scalable.Win.Controls
{
    partial class NumKeypad
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
            this.btnMinus = new System.Windows.Forms.Button();
            this.btnDecimal = new System.Windows.Forms.Button();
            this.btnThree = new System.Windows.Forms.Button();
            this.btnTwo = new System.Windows.Forms.Button();
            this.btnSix = new System.Windows.Forms.Button();
            this.btnZero = new System.Windows.Forms.Button();
            this.btnFive = new System.Windows.Forms.Button();
            this.btnOne = new System.Windows.Forms.Button();
            this.btnNine = new System.Windows.Forms.Button();
            this.btnFour = new System.Windows.Forms.Button();
            this.btnEight = new System.Windows.Forms.Button();
            this.btnSeven = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnBackSpace = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnMinus
            // 
            this.btnMinus.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnMinus.Location = new System.Drawing.Point(48, 86);
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size(22, 22);
            this.btnMinus.TabIndex = 69;
            this.btnMinus.Tag = "-";
            this.btnMinus.Text = "-";
            this.btnMinus.Click += new System.EventHandler(this.concatButtonString);
            this.btnMinus.Leave += new System.EventHandler(this.buttons_Leave);
            // 
            // btnDecimal
            // 
            this.btnDecimal.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnDecimal.Location = new System.Drawing.Point(25, 86);
            this.btnDecimal.Name = "btnDecimal";
            this.btnDecimal.Size = new System.Drawing.Size(22, 22);
            this.btnDecimal.TabIndex = 68;
            this.btnDecimal.Tag = ".";
            this.btnDecimal.Text = ".";
            this.btnDecimal.Click += new System.EventHandler(this.concatButtonString);
            this.btnDecimal.Leave += new System.EventHandler(this.buttons_Leave);
            // 
            // btnThree
            // 
            this.btnThree.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnThree.Location = new System.Drawing.Point(48, 65);
            this.btnThree.Name = "btnThree";
            this.btnThree.Size = new System.Drawing.Size(22, 22);
            this.btnThree.TabIndex = 66;
            this.btnThree.Tag = "3";
            this.btnThree.Text = "3";
            this.btnThree.Click += new System.EventHandler(this.concatButtonString);
            this.btnThree.Leave += new System.EventHandler(this.buttons_Leave);
            // 
            // btnTwo
            // 
            this.btnTwo.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnTwo.Location = new System.Drawing.Point(25, 65);
            this.btnTwo.Name = "btnTwo";
            this.btnTwo.Size = new System.Drawing.Size(22, 22);
            this.btnTwo.TabIndex = 65;
            this.btnTwo.Tag = "2";
            this.btnTwo.Text = "2";
            this.btnTwo.Click += new System.EventHandler(this.concatButtonString);
            this.btnTwo.Leave += new System.EventHandler(this.buttons_Leave);
            // 
            // btnSix
            // 
            this.btnSix.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnSix.Location = new System.Drawing.Point(48, 44);
            this.btnSix.Name = "btnSix";
            this.btnSix.Size = new System.Drawing.Size(22, 22);
            this.btnSix.TabIndex = 63;
            this.btnSix.Tag = "6";
            this.btnSix.Text = "6";
            this.btnSix.Click += new System.EventHandler(this.concatButtonString);
            this.btnSix.Leave += new System.EventHandler(this.buttons_Leave);
            // 
            // btnZero
            // 
            this.btnZero.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnZero.Location = new System.Drawing.Point(2, 86);
            this.btnZero.Name = "btnZero";
            this.btnZero.Size = new System.Drawing.Size(22, 22);
            this.btnZero.TabIndex = 67;
            this.btnZero.Tag = "0";
            this.btnZero.Text = "0";
            this.btnZero.Click += new System.EventHandler(this.concatButtonString);
            this.btnZero.Leave += new System.EventHandler(this.buttons_Leave);
            // 
            // btnFive
            // 
            this.btnFive.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnFive.Location = new System.Drawing.Point(25, 44);
            this.btnFive.Name = "btnFive";
            this.btnFive.Size = new System.Drawing.Size(22, 22);
            this.btnFive.TabIndex = 62;
            this.btnFive.Tag = "5";
            this.btnFive.Text = "5";
            this.btnFive.Click += new System.EventHandler(this.concatButtonString);
            this.btnFive.Leave += new System.EventHandler(this.buttons_Leave);
            // 
            // btnOne
            // 
            this.btnOne.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnOne.Location = new System.Drawing.Point(2, 65);
            this.btnOne.Name = "btnOne";
            this.btnOne.Size = new System.Drawing.Size(22, 22);
            this.btnOne.TabIndex = 64;
            this.btnOne.Tag = "1";
            this.btnOne.Text = "1";
            this.btnOne.Click += new System.EventHandler(this.concatButtonString);
            this.btnOne.Leave += new System.EventHandler(this.buttons_Leave);
            // 
            // btnNine
            // 
            this.btnNine.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnNine.Location = new System.Drawing.Point(48, 23);
            this.btnNine.Name = "btnNine";
            this.btnNine.Size = new System.Drawing.Size(22, 22);
            this.btnNine.TabIndex = 60;
            this.btnNine.Tag = "9";
            this.btnNine.Text = "9";
            this.btnNine.Click += new System.EventHandler(this.concatButtonString);
            this.btnNine.Leave += new System.EventHandler(this.buttons_Leave);
            // 
            // btnFour
            // 
            this.btnFour.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnFour.Location = new System.Drawing.Point(2, 44);
            this.btnFour.Name = "btnFour";
            this.btnFour.Size = new System.Drawing.Size(22, 22);
            this.btnFour.TabIndex = 61;
            this.btnFour.Tag = "4";
            this.btnFour.Text = "4";
            this.btnFour.Click += new System.EventHandler(this.concatButtonString);
            this.btnFour.Leave += new System.EventHandler(this.buttons_Leave);
            // 
            // btnEight
            // 
            this.btnEight.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnEight.Location = new System.Drawing.Point(25, 23);
            this.btnEight.Name = "btnEight";
            this.btnEight.Size = new System.Drawing.Size(22, 22);
            this.btnEight.TabIndex = 59;
            this.btnEight.Tag = "8";
            this.btnEight.Text = "8";
            this.btnEight.Click += new System.EventHandler(this.concatButtonString);
            this.btnEight.Leave += new System.EventHandler(this.buttons_Leave);
            // 
            // btnSeven
            // 
            this.btnSeven.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnSeven.Location = new System.Drawing.Point(2, 23);
            this.btnSeven.Name = "btnSeven";
            this.btnSeven.Size = new System.Drawing.Size(22, 22);
            this.btnSeven.TabIndex = 58;
            this.btnSeven.Tag = "7";
            this.btnSeven.Text = "7";
            this.btnSeven.Click += new System.EventHandler(this.concatButtonString);
            this.btnSeven.Leave += new System.EventHandler(this.buttons_Leave);
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnClear.Location = new System.Drawing.Point(48, 2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(22, 22);
            this.btnClear.TabIndex = 57;
            this.btnClear.Text = "C";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            this.btnClear.Leave += new System.EventHandler(this.buttons_Leave);
            // 
            // btnBackSpace
            // 
            this.btnBackSpace.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold);
            this.btnBackSpace.Location = new System.Drawing.Point(2, 2);
            this.btnBackSpace.Name = "btnBackSpace";
            this.btnBackSpace.Size = new System.Drawing.Size(45, 22);
            this.btnBackSpace.TabIndex = 56;
            this.btnBackSpace.Tag = "B";
            this.btnBackSpace.Text = "←";
            this.btnBackSpace.Click += new System.EventHandler(this.concatButtonString);
            this.btnBackSpace.Leave += new System.EventHandler(this.buttons_Leave);
            // 
            // NumKeypad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnMinus);
            this.Controls.Add(this.btnDecimal);
            this.Controls.Add(this.btnThree);
            this.Controls.Add(this.btnTwo);
            this.Controls.Add(this.btnSix);
            this.Controls.Add(this.btnZero);
            this.Controls.Add(this.btnFive);
            this.Controls.Add(this.btnOne);
            this.Controls.Add(this.btnNine);
            this.Controls.Add(this.btnFour);
            this.Controls.Add(this.btnEight);
            this.Controls.Add(this.btnSeven);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnBackSpace);
            this.Name = "NumKeypad";
            this.Size = new System.Drawing.Size(72, 110);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnMinus;
        private System.Windows.Forms.Button btnDecimal;
        private System.Windows.Forms.Button btnThree;
        private System.Windows.Forms.Button btnTwo;
        private System.Windows.Forms.Button btnSix;
        private System.Windows.Forms.Button btnZero;
        private System.Windows.Forms.Button btnFive;
        private System.Windows.Forms.Button btnOne;
        private System.Windows.Forms.Button btnNine;
        private System.Windows.Forms.Button btnFour;
        private System.Windows.Forms.Button btnEight;
        private System.Windows.Forms.Button btnSeven;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnBackSpace;

    }
}
