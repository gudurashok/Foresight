using System;

namespace ScalableApps.Foresight.Logic.Common
{
    public delegate void UpdatingEventHandler(object sender, UpdatingEventArgs e);

    public class UpdatingEventArgs : EventArgs
    {
        public string CurrentItem { get; private set; }
        public bool Cancel { get; set; }

        public UpdatingEventArgs(string currentItem)
        {
            CurrentItem = currentItem;
        }
    }
}
