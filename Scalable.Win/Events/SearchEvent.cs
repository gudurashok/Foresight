using System;

namespace Scalable.Win.Events
{
    public delegate void SearchEventHandler(object sender, SearchEventArgs e);

    public class SearchEventArgs : EventArgs
    {
        public SearchResult Result { get; set; }
        public string SearchText { get; private set; }

        public SearchEventArgs(string searchText)
        {
            SearchText = searchText;
        }
    }
}
