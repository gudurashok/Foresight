using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Scalable.Win.Controls
{
    public partial class NumTextBox : UserControl
    {
        public static bool isPnlHidden = true;
        private NumKeypad _numKeypad;

        public NumTextBox()
        {
            InitializeComponent();
            Initialize();
        }

        protected void Initialize()
        {
            txtDisplay.Location = new Point(0, 0);
            btnDropDown.Location = new Point(txtDisplay.Width, 0);
            btnDropDown.Height = txtDisplay.Height;
            txtDisplay.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            btnDropDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Height = txtDisplay.Height;
        }

        private void AddNewNumPad()
        {
            if (!Parent.Controls.Contains(_numKeypad))
            {
                _numKeypad = new NumKeypad(txtDisplay);
                var parent = Parent as Form;
                if (parent != null) parent.Controls.Add(_numKeypad);
                _numKeypad.Location = new Point(Location.X + Width - 66, Location.Y + Height);
            }
        }

        private void btnDropDown_Click(object sender, EventArgs e)
        {
            try
            {
                AddNewNumPad();
                TogglePanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void TogglePanel()
        {
            if (isPnlHidden)
            {
                _numKeypad.Show();
                _numKeypad.BringToFront();
            }
            else
                _numKeypad.Hide();

            isPnlHidden = !isPnlHidden;
        }

        private void txtDisplay_KeyPress(object sender, KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            e.Handled = isValidForCurrency(e);
        }

        private bool isValidForCurrency(KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
                return false;

            if (e.KeyChar == '-' && !txtDisplay.Text.Contains('-'))
            {
                txtDisplay.Text = @"-" + txtDisplay.Text;
                txtDisplay.Select(txtDisplay.TextLength, 0);
                return true;
            }

            if (e.KeyChar == '.' && !txtDisplay.Text.Contains('.'))
            {
                if (String.IsNullOrEmpty(txtDisplay.Text) || txtDisplay.Text == @"-")
                {
                    txtDisplay.Text += @"0.";
                    txtDisplay.Select(txtDisplay.TextLength, 0);
                    return true;
                }
                return false;
            }

            if (!NumLength.IsIntegerLong(txtDisplay))
            {
                if (Char.IsDigit(e.KeyChar))
                {
                    if (txtDisplay.Text == @"0")
                        txtDisplay.Text = "";

                    return false;
                }
            }
            return true;
        }

        public double Value
        {
            get { return Convert.ToDouble(txtDisplay.Text); }
        }

        public override string Text
        {
            get { return txtDisplay.Text; }
            set { txtDisplay.Text = value; }
        }

        private void txtDisplay_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtDisplay.Text) && isPnlHidden)
                base.OnLostFocus(e);

            hidePanelIfOtherControl();
        }

        private void txtDisplay_Enter(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtDisplay.Text))
            {
                base.OnEnter(e);
                //txtDisplay.SelectAll();
            }
        }

        private void txtDisplay_TextChanged(object sender, EventArgs e)
        {
            base.OnTextChanged(e);
        }

        private void btnDropDown_Enter(object sender, EventArgs e)
        {
            if (!isPnlHidden)
                base.OnEnter(e);
        }

        private void btnDropDown_Leave(object sender, EventArgs e)
        {
            if (isPnlHidden)
                base.OnLostFocus(e);

            hidePanelIfOtherControl();
        }

        private bool CheckNumPadFocus()
        {
            return _numKeypad.Controls.Cast<Control>().Any(control => control.Focused);
        }

        private void hidePanelIfOtherControl()
        {
            if (txtDisplay.Focused || btnDropDown.Focused || CheckNumPadFocus()) 
                return;
            _numKeypad.Hide();
            isPnlHidden = true;
        }
    }
}
