using System.Collections.Generic;

namespace ScalableApps.Foresight.Logic.Business
{
    public class DebitNote
    {
        public DebitNoteHeader Header { get; set; }
        public IList<DebitNoteLine> Lines { get; set; }

        public DebitNote()
        {
            Header = new DebitNoteHeader();
            Lines = new List<DebitNoteLine>();
        }

        public void SetIdentityValue(int id)
        {
            Header.Id = id;

            foreach (var line in Lines)
                line.NoteId = id;
        }
    }
}
