using System;

namespace Ferry.Logic.Common
{
    public delegate void ExtractingEventHandler(object sender, ExtractingEventArgs e);

    public class ExtractingEventArgs : EventArgs
    {
        public string CurrentText { get; private set; }

        public ExtractingEventArgs(string currentText)
        {
            CurrentText = currentText;
        }
    }
}
