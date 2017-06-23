using System.Windows.Forms;
using Scalable.Shared.Enums;

namespace Scalable.Win.Controls
{
    public class iColumnHeader : ColumnHeader
    {
        //public int MinWidth { get; set; }
        //public int MaxWidth { get; set; }
        public bool AutoResizable { get; set; }
        //public string Format { get; set; }
        public DataType DataType { get; set; }

        public iColumnHeader(string name)
        {
            DataType = DataType.Text;
            Name = name;
            Text = name;
            TextAlign = getTextAlignment();
        }

        public iColumnHeader(string name, bool autoResizable)
            : this(name)
        {
            AutoResizable = autoResizable;
        }

        public iColumnHeader(string name, bool autoResizable, int width)
            : this(name, autoResizable)
        {
            Width = width;
        }

        private HorizontalAlignment getTextAlignment()
        {
            return DataType == DataType.Number
                                ? HorizontalAlignment.Right
                                : HorizontalAlignment.Left;
        }
    }
}
