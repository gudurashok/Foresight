using System.Windows.Forms;
using System.Drawing;

namespace ScalableApps.Foresight.Win.Controls
{
    public class iListView : ListView
    {
        public void SetAlternateColor()
        {
            var alternate = false;

            foreach (ListViewItem lvi in Items)
            {
                if (alternate)
                    setAlternateColor(lvi);

                alternate = !alternate;
            }
        }

        private void setAlternateColor(ListViewItem lvi)
        {
            lvi.UseItemStyleForSubItems = true;
            lvi.BackColor = Color.LightYellow;
        }

        //public void AutoResizeColumns()
    }
}
