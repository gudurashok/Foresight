using System;
using System.Windows.Forms;
using System.Collections;

namespace ScalableApps.Foresight.Win.Common
{
    public class ListViewItemComparer : IComparer
    {
        private readonly int _columnIndex;
        private readonly bool _isAscending;
        private ListViewItem _xLvi;
        private ListViewItem _yLvi;

        public ListViewItemComparer(int columnIndex, bool isAscending)
        {
            _columnIndex = columnIndex;
            _isAscending = isAscending;
        }

        public int Compare(object x, object y)
        {
            _xLvi = x as ListViewItem;
            _yLvi = y as ListViewItem;

            if (_isAscending)
                return String.Compare(getSubItemText(_xLvi), getSubItemText(_yLvi), true);
            
            return String.Compare(getSubItemText(_yLvi), getSubItemText(_xLvi), true);
        }

        private string getSubItemText(ListViewItem lvi)
        {
            string text = lvi.SubItems[_columnIndex].Text;

            if (lvi.ListView.Columns[_columnIndex].TextAlign == HorizontalAlignment.Right)
                return text.PadLeft(15, '0');

            return text;
        }
    }
}
