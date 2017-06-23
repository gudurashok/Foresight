using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Scalable.Win.Controls
{
    public class EnumPanel : Panel
    {
        [DefaultValue(false)]
        public bool IsFlagged { get; set; }

        public int Value
        {
            get { return getValue(); }
            set { setValue(value); }
        }

        private void setValue(int value)
        {
            if (IsFlagged)
            {
                foreach (var ctl in Controls.OfType<CheckBox>().Where(ctl => ctl.Tag != null))
                    ctl.Checked = Convert.ToBoolean(Convert.ToInt32(ctl.Tag) & value);
            }
            else
            {
                foreach (var ctl in Controls.OfType<RadioButton>().Where(ctl => Convert.ToInt32(ctl.Tag) == value))
                {
                    ctl.Checked = true;
                    return;
                }
            }
        }

        private int getValue()
        {
            var result = 0;

            if (IsFlagged)
                result = (from CheckBox ctl in Controls.OfType<CheckBox>()
                          where ctl.Tag != null && ctl.Checked
                          select ctl).Aggregate(result, (current, ctl) => current | Convert.ToInt32(ctl.Tag));
            else
                foreach (var ctl in Controls.OfType<RadioButton>().Where(ctl => ctl.Checked && ctl.Tag != null))
                    return Convert.ToInt32(ctl.Tag);

            return result;
        }
    }
}
