using System.Collections.Generic;

namespace ScalableApps.Foresight.Logic.Business
{
    public class SaleInvoice
    {
        public SaleInvoiceHeader Header { get; set; }
        public SaleInvoiceHeaderEx HeaderEx { get; set; }
        public IList<SaleInvoiceLine> Lines { get; set; }
        public IList<SaleInvoiceTerm> Terms { get; set; }

        public SaleInvoice()
        {
            Lines = new List<SaleInvoiceLine>();
            Terms = new List<SaleInvoiceTerm>();
        }

        public void SetIdentityValue(int id)
        {
            Header.Id = id;

            if (HeaderEx != null)
                HeaderEx.InvoiceId = id;

            foreach (var line in Lines)
                line.InvoiceId = id;

            foreach (var term in Terms)
                term.InvoiceId = id;
        }
    }
}
