using System.Windows.Forms;
using System.Drawing;

namespace ScalableApps.Foresight.Win.Common
{
    public static class ListViewLineColorProcessor
    {
        public static void SetInvertedColorsToItems(ListView lvw, int count)
        {
            bool swap = false;
            int i = 1;

            foreach (ListViewItem lvi in lvw.Items)
            {
                lvi.UseItemStyleForSubItems = true;
                if (swap)
                    lvi.BackColor = Color.PowderBlue;

                if (i < count)
                    i++;
                else
                {
                    i = 1;
                    swap = !swap;
                }
            }
        }
    }
}
