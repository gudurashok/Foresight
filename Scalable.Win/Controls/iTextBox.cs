using System;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using Scalable.Shared.Enums;
using Scalable.Win.Events;

namespace Scalable.Win.Controls
{
    public class iTextBox : TextBox
    {
        #region Properties

        [DefaultValue(TextCaseStyle.Normal)]
        public TextCaseStyle AutoCasing { get; set; }

        public event SearchEventHandler Search;

        #endregion

        #region Declarations

        private TextCaseStyle _currentCase;
        private string _originalText;

        #endregion

        #region Event Handlers

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            SelectAll();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (e.Handled)
                return;

            if (ModifierKeys == Keys.Control)
            {
                e.Handled = true;
                return;
            }

            if (AutoCompleteMode == AutoCompleteMode.None)
                performAutoCasing(e);
            else
                performAutoCompletion(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Back && e.Control)
            {
                Text = "";
                return;
            }

            if (Text.Length == 0 || e.Alt)
                return;

            if (e.KeyCode == Keys.F2 && e.Modifiers == Keys.None)
            {
                toggleTextSelection();
                return;
            }

            if (e.KeyCode == Keys.F3)
            {
                if (AutoCasing != TextCaseStyle.Normal)
                    return;

                if (e.Modifiers == Keys.None)
                {
                    Text = Text.ToUpper();
                    return;
                }

                if (string.IsNullOrWhiteSpace(_originalText))
                    _originalText = Text;

                setNextCaseStyle(e.Control);
                switchCase(getSwitchedCaseText());
            }
        }

        #endregion

        #region Switch Character Case

        private void switchCase(string switchedText)
        {
            Text = switchedText;
            SelectionStart = Text.Length;
        }

        private void toggleTextSelection()
        {
            if (SelectionLength != Text.Length)
                SelectAll();
            else
                SelectionLength = 0;
        }

        private string getSwitchedCaseText()
        {
            switch (_currentCase)
            {
                case TextCaseStyle.Normal:
                    return _originalText;
                case TextCaseStyle.Upper:
                    return Text.ToUpper();
                case TextCaseStyle.Lower:
                    return Text.ToLower();
                case TextCaseStyle.Proper:
                    return getProperCase();
                case TextCaseStyle.Title:
                    return string.Format("{0}{1}", Text.Substring(0, 1), Text.Substring(1).ToLower());
                default:
                    return Text;
            }
        }

        private string getProperCase()
        {
            var sb = new StringBuilder();

            foreach (var word in Text.Split(' '))
            {
                sb.Append(word.Length > 1
                              ? string.Format("{0}{1}", word.Substring(0, 1).ToUpper(), word.Substring(1))
                              : word.ToUpper());

                sb.Append(' ');
            }

            return sb.Remove(sb.Length - 1, 1).ToString();
        }

        private void setNextCaseStyle(bool backwards)
        {
            var style = (int)_currentCase;
            style += backwards ? -1 : +1;
            if (style > (int)TextCaseStyle.Title)
                style = 0;
            else if (style < 0)
                style = (int)TextCaseStyle.Title;

            _currentCase = (TextCaseStyle)style;
        }

        #endregion

        #region Auto Charater Casing

        private void performAutoCasing(KeyPressEventArgs e)
        {
            if (AutoCasing == TextCaseStyle.Normal)
            {
                _originalText = null;
                return;
            }

            e.KeyChar = changeCase(e.KeyChar);
        }

        private char changeCase(char c)
        {
            if (getCaseStyle() == CharacterCasing.Upper)
                return char.ToUpper(c);

            return char.ToLower(c);
        }

        private CharacterCasing getCaseStyle()
        {
            switch (AutoCasing)
            {
                case TextCaseStyle.Proper:
                    return CharacterCasing.Upper; // TODO: implement proper case
                //{
                //    if (SelectionStart != 0)
                //        return TextCaseStyle.Lower;
                //    else
                //        return TextCaseStyle.Lower;
                //}
                case TextCaseStyle.Title:
                    if (SelectionStart == 0)
                        return CharacterCasing.Upper;

                    return CharacterCasing.Lower;
                default:
                    return CharacterCasing.Normal;
            }
        }

        #endregion

        #region Auto Completion

        private void performAutoCompletion(KeyPressEventArgs e)
        {
            var oldString = getOldInputText();

            if (e.KeyChar == (char)Keys.Escape)
                e.KeyChar = (char)Keys.None;

            if (string.IsNullOrEmpty(oldString) && e.KeyChar == (char)Keys.Back)
                e.KeyChar = (char)Keys.None;

            e.KeyChar = Char.ToUpper(e.KeyChar);
            var inputText = appendTypedKeyChar(e.KeyChar, oldString);

            if (inputText.Length == 0)
            {
                Text = "";
                return;
            }

            var args = new SearchEventArgs(inputText);
            OnSearch(args);

            if (args.Result == null)
                return;

            Text = args.Result.Text.ToUpper();
            highlightTextFound(inputText);
            e.Handled = true;
        }

        protected void OnSearch(SearchEventArgs e)
        {
            if (Search != null)
                Search(this, e);
        }

        private string appendTypedKeyChar(char keyChar, string text)
        {
            if ((keyChar == (char)Keys.Back) && text.Length > 0)
                return text.Remove(text.Length - 1, 1);

            return string.Format("{0}{1}", text, keyChar);
        }

        private string getOldInputText()
        {
            return Text.Substring(0, SelectionStart);
        }

        private void highlightTextFound(string inputText)
        {
            Select(inputText.Length, Text.Length - inputText.Length);
        }

        #endregion
    }
}
