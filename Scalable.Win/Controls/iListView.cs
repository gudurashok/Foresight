using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using Scalable.Shared.Common;

namespace Scalable.Win.Controls
{
    public class iListView : ListView
    {
        private bool isListPopulating;

        #region Data Filling

        public void FillData(IEnumerable<dynamic> data)
        {
            Items.Clear();
            
            if (data == null)
                return;

            isListPopulating = true;

            foreach (var item in data)
                Items.Add(createListItem(item));

            isListPopulating = false;
            ScalableUtil.SelectListItem(this, 0);
            resizeColumns();
        }

        private ListViewItem createListItem(object data)
        {
            var lvi = new ListViewItem(getColumnValue(data, Columns[0] as iColumnHeader));
            lvi.Tag = data;

            for (var i = 1; i < Columns.Count; i++)
                lvi.SubItems.Add(getColumnValue(data, Columns[i] as iColumnHeader));

            return lvi;
        }

        private string getColumnValue(object data, iColumnHeader col)
        {
            var pi = data.GetType().GetProperty(col.Name);
            var value = pi.GetValue(data, null);
            if (value == null)
                return "";

            return value.ToString(); //TODO: Apply column format
        }

        #endregion

        #region Search Item

        public ListViewItem SearchItem(string searchText)
        {
            var lvi = FindItemWithText(searchText, false, 0, true);
            if (lvi != null)
                lvi.Selected = true;

            return lvi;
        }

        protected override void OnItemSelectionChanged(ListViewItemSelectionChangedEventArgs e)
        {
            base.OnItemSelectionChanged(e);

            if (e.IsSelected)
                highlight(e.Item);
            else
                clearHighlight(e.Item);
        }

        private void clearHighlight(ListViewItem lvi)
        {
            if (lvi == null)
                return;

            lvi.ForeColor = Color.Black;
            lvi.BackColor = Color.White;
            lvi.Selected = false;
        }

        private void highlight(ListViewItem lvi)
        {
            lvi.ForeColor = SystemColors.HighlightText;
            lvi.BackColor = SystemColors.Highlight;
            lvi.EnsureVisible();
        }

        #endregion

        #region Column Auto Resizing

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (isListPopulating)
                return;

            resizeColumns();
        }

        private void resizeColumns()
        {
            var resizableColumnWidth = getResizableColumnCount();

            if (resizableColumnWidth == 0)
                return;

            var resizableWidth = getResizableWidth() / resizableColumnWidth;
            var adjustableWidth = resizableWidth;
            var remainderWidth = resizableWidth - adjustableWidth;

            foreach (var column in (from c in Columns.Cast<iColumnHeader>() where c.AutoResizable select c))
                Columns[column.Name].Width += adjustableWidth;

            if (remainderWidth > 0)
                adjustRemainingWidth(remainderWidth);
        }

        private void adjustRemainingWidth(int remainderWidth)
        {
            var column = Columns.Cast<iColumnHeader>().FirstOrDefault(c => c.AutoResizable);
            if (column != null)
                Columns[column.Name].Width += remainderWidth;
        }

        private int getResizableWidth()
        {
            return ClientSize.Width - Columns.Cast<iColumnHeader>().Sum(c => c.Width);
        }

        private int getResizableColumnCount()
        {
            return (from c in Columns.Cast<iColumnHeader>()
                    where c.AutoResizable
                    select c.Width).Count();
        }

        #endregion
    }
}
