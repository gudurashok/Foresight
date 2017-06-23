namespace ScalableApps.Foresight.Logic.Report
{
    public class GenuineSale
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public decimal SaleAmount { get; set; }
        public decimal ReceiptAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal GenuineSalePct { get; set; }
    }
}
