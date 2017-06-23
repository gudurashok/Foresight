namespace ScalableApps.Foresight.Logic.Business
{
    public class ItemMonthlyLedger
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int AccountId { get; set; }
        public int ChartOfAccountId { get; set; }
        public int Month { get; set; }
        public decimal Sale { get; set; }
        public decimal Purchase { get; set; }
        public CompanyPeriod CompanyPeriod { get; set; }
    }
}
