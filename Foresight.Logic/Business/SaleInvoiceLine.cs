namespace ScalableApps.Foresight.Logic.Business
{
    public class SaleInvoiceLine : LineItem
    {
        public int InvoiceId { get; set; }
        public decimal Price { get; set; }
    }
}
