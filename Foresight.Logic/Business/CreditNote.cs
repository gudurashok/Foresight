using System.Collections.Generic;

namespace ScalableApps.Foresight.Logic.Business
{
    public class CreditNote
    {
        public CreditNoteHeader Header { get; set; }
        public IList<CreditNoteLine> Lines { get; set; }

        public CreditNote()
        {
            Header = new CreditNoteHeader();
            Lines = new List<CreditNoteLine>();
        }

        public void SetIdentityValue(int id)
        {
            Header.Id = id;

            foreach (var line in Lines)
                line.NoteId = id;
        }
    }
}
