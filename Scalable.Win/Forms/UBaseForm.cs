using System.Windows.Forms;
using Scalable.Win.Controls;

namespace Scalable.Win.Forms
{
    public partial class UBaseForm : UserControl
    {
        public UBaseForm()
        {
            InitializeComponent();
        }

        #region Keyboard Navigation

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            MoveBetweenFields(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void MoveBetweenFields(Keys keyData)
        {
            if (ActiveControl is UBaseForm)
                return;

            if (isListControl(ActiveControl))
                return;

            if (shouldMoveBetweenControls(keyData))
                SelectNextControl(ActiveControl, moveForward(keyData), true, true, true);
        }

        private bool isListControl(Control control)
        {
            return control.GetType() == typeof(iListView);
        }

        private bool shouldMoveBetweenControls(Keys keyData)
        {
            if (isAltKey(keyData) || isControlKey(keyData))
                return false;

            return isEnterOrArrowKey(keyData);
        }

        private bool isEnterOrArrowKey(Keys keyData)
        {
            if (isEnterKey(keyData) || isShiftEnterKey(keyData))
                return true;

            return keyData == Keys.Down ||
                   keyData == Keys.Up;
        }

        private bool moveForward(Keys keyData)
        {
            return keyData == Keys.Down ||
                    isEnterKey(keyData);
        }

        private bool isEnterKey(Keys keyData)
        {
            return keyData == Keys.Enter;
        }

        private bool isShiftEnterKey(Keys keyData)
        {
            return (keyData & Keys.Shift) == Keys.Shift &&
                    (keyData & Keys.Enter) == Keys.Enter;
        }

        private bool isAltKey(Keys keyData)
        {
            return (keyData & Keys.Alt) == Keys.Alt;
        }

        private bool isControlKey(Keys keyData)
        {
            return (keyData & Keys.Control) == Keys.Control;
        }

        #endregion
    }
}
