namespace ScalableApps.Foresight.Logic.Business
{
    public class AccountMonthlyLedger
    {
        public int Id { get; set; }
        public int DaybookId { get; set; }
        public int AccountId { get; set; }
        public int ChartOfAccountId { get; set; }
        //public int ChartOfAccountType { get; set; }
        public int Month { get; set; }
        public decimal Opening { get; set; }
        public decimal Sale { get; set; }
        public decimal CashPayment { get; set; }
        public decimal BankPayment { get; set; }
        public decimal DebitNote { get; set; }
        public decimal DebitJV { get; set; }
        public decimal Purchase { get; set; }
        public decimal CashReceipt { get; set; }
        public decimal BankReceipt { get; set; }
        public decimal CreditNote { get; set; }
        public decimal CreditJV { get; set; }
        public CompanyPeriod CompanyPeriod { get; set; }
    }
}
