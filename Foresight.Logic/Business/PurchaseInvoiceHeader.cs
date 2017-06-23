namespace ScalableApps.Foresight.Logic.Business
{
    public class PurchaseInvoiceHeader : TransactionHeader
    {
        public decimal BrokerageAmount { get; set; }
        public string Through { get; set; }
        public string Transport { get; set; }
        public string ReferenceDocNr { get; set; }
        public int OrderId { get; set; }
        public double DiscountPct { get; set; }
        public int SaleTaxColumnId { get; set; }
    }
}
