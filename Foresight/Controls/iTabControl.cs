using System.Windows.Forms;
using System.Drawing;

namespace ScalableApps.Foresight.Win.Controls
{
    public class iTabControl : TabControl
    {
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            for (var index = 0; index < TabPages.Count; index++)
            {
                var r = GetTabRect(index);
                var closeButton = new Rectangle(r.Right - 15, r.Top + 4, 12, 10);

                if (!closeButton.Contains(e.Location)) 
                    continue;

                TabPages.RemoveAt(index);
                break;
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            if (SelectedTab == null)
                return;

            var isActiveTab = TabPages.IndexOf(SelectedTab) == e.Index;
            var font = new Font(e.Font.FontFamily, 12.00F, FontStyle.Bold);

            if (isActiveTab)
            {
                e.Graphics.FillRectangle(Brushes.Teal, e.Bounds);
                e.Graphics.DrawString("x", font, Brushes.Red, e.Bounds.Right - 18, e.Bounds.Top);
                e.Graphics.DrawString(TabPages[e.Index].Text, e.Font, Brushes.Yellow, e.Bounds.Left + 5, e.Bounds.Top + 4);
            }
            else
            {
                e.Graphics.DrawString("x", font, Brushes.Red, e.Bounds.Right - 15, e.Bounds.Top - 2);
                e.Graphics.DrawString(TabPages[e.Index].Text, e.Font, Brushes.Black, e.Bounds.Left + 5, e.Bounds.Top + 4);
            }
        }
    }
}
