using ScalableApps.Foresight.Logic.Business;

namespace ScalableApps.Foresight.Logic.Report
{
    public class LedgerSummary
    {
        public CompanyPeriod CompanyPeriod { get; set; }
        public int Month { get; set; }
        public decimal OpeningAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal BalanceAmount { get; set; }
    }
}
