using System;
using System.Linq;
using System.Windows.Forms;

namespace Scalable.Win.Controls
{
    internal partial class NumKeypad : UserControl
    {
        private readonly TextBox _textBox;
        private int cursorPosition;
        private string stringBeforeCursor = "";
        private string stringAfterCursor = "";

        public NumKeypad(TextBox textBox)
        {
            InitializeComponent();
            _textBox = textBox;
            textBox.Focus();
        }

        private void concatButtonString(object sender, EventArgs e)
        {
            var b = sender as Button;
            if (b != null) AppendChar(Convert.ToChar(b.Tag));
            _textBox.Focus();
        }

        private void AppendChar(char text)
        {
            cursorPosition = _textBox.SelectionStart;

            if (isBackSpace(text))
                return;

            if (isDecimal(text))
                return;

            if (isMinus(text))
                return;

            if (!NumLength.IsIntegerLong(_textBox) && Char.IsDigit(text))
            {
                AddCharToTextBox(text);
                return;
            }
        }

        private bool isMinus(char text)
        {
            if (text == '-')
            {
                if (!_textBox.Text.Contains(text))
                {
                    _textBox.Text = text + _textBox.Text;
                    _textBox.SelectionStart = cursorPosition + 1;
                }
                return true;
            }
            return false;
        }

        private bool isDecimal(char text)
        {
            if (text == '.')
            {
                if (_textBox.Text.Contains(text))
                    return true;

                if (String.IsNullOrEmpty(_textBox.Text))
                {
                    _textBox.Text = @"0.";
                    _textBox.SelectionStart = 2;
                    return true;
                }

                if (_textBox.Text == @"-")
                {
                    _textBox.Text += @"0.";
                    _textBox.SelectionStart = 3;
                    return true;
                }

                AddCharToTextBox(text);
                return true;
            }
            return false;
        }

        private bool isBackSpace(char text)
        {
            if (text == 'B')
            {
                EraseChar();
                return true;
            }
            return false;
        }

        private void AddCharToTextBox(char text)
        {
            stringBeforeCursor = _textBox.Text.Substring(0, cursorPosition);
            stringAfterCursor = _textBox.Text.Substring(cursorPosition, _textBox.TextLength - cursorPosition);
            _textBox.Text = stringBeforeCursor + text + stringAfterCursor;
            _textBox.SelectionStart = cursorPosition + 1;
        }

        private void EraseChar()
        {
            if (_textBox.SelectionStart != 0)
            {
                stringBeforeCursor = _textBox.Text.Substring(0, cursorPosition - 1);
                stringAfterCursor = _textBox.Text.Substring(cursorPosition, _textBox.TextLength - cursorPosition);
                _textBox.Text = stringBeforeCursor + stringAfterCursor;
                _textBox.SelectionStart = cursorPosition - 1;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _textBox.Text = "";
        }

        private void buttons_Leave(object sender, EventArgs e)
        {
            //foreach (Control control in this.Controls)
            //{
            //    if (!control.Focused && !_textBox.Focused)
            //        this.Hide();
            //    else
            //    {
            //        this.Show();
            //        break;
            //    }
            //}
        }
    }
}
