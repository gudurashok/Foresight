namespace ScalableApps.Foresight.Logic.Business
{
    public class PurchaseInvoiceLine : LineItem
    {
        public int InvoiceId { get; set; }
        public decimal Cost { get; set; }
    }
}
