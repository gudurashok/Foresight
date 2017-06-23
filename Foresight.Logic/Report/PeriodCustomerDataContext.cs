namespace ScalableApps.Foresight.Logic.Report
{
    public class PeriodCustomerDataContext : PeriodAccountBaseDataContext
    {
        protected override PeriodTopperBaseDataContext getTopNDataContext()
        {
            return new PeriodTopCustomersDataContext();
        }
    }
}
