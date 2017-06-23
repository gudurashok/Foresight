using System.Collections.Generic;

namespace ScalableApps.Foresight.Logic.Business
{
    public class PurchaseInvoice
    {
        public PurchaseInvoiceHeader Header { get; set; }
        public IList<PurchaseInvoiceLine> Lines { get; set; }
        public IList<PurchaseInvoiceTerm> Terms { get; set; }

        public PurchaseInvoice()
        {
            Lines = new List<PurchaseInvoiceLine>();
            Terms = new List<PurchaseInvoiceTerm>();
        }

        public void SetIdentityValue(int id)
        {
            Header.Id = id;

            foreach (var line in Lines)
                line.InvoiceId = id;

            foreach (var term in Terms)
                term.InvoiceId = id;
        }
    }
}
