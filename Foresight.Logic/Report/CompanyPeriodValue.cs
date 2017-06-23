using ScalableApps.Foresight.Logic.Business;

namespace ScalableApps.Foresight.Logic.Report
{
    public class CompanyPeriodValue
    {
        public CompanyPeriod CompanyPeriod { get; set; }
        public decimal? Value { get; set; }
        public decimal? DifferencePct { get; set; }
    }
}
