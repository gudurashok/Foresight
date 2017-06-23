namespace ScalableApps.Foresight.Logic.Business
{
    public class SaleInvoiceHeaderEx
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string ShipToName { get; set; }
        public string ShipToAddressLine1 { get; set; }
        public string ShipToAddressLine2 { get; set; }
        public string ShipToCity { get; set; }
    }
}
