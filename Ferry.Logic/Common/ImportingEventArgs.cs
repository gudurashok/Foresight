using System;

namespace Ferry.Logic.Common
{
    public delegate void ImportingEventHandler(object sender, ImportingEventArgs e);

    public class ImportingEventArgs : EventArgs
    {
        public string CurrentItem { get; private set; }
        public bool Cancel { get; set; }

        public ImportingEventArgs(string currentItem)
        {
            CurrentItem = currentItem;
        }
    }
}
