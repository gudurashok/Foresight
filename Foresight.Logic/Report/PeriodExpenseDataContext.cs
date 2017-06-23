namespace ScalableApps.Foresight.Logic.Report
{
    public class PeriodExpenseDataContext : PeriodAccountBaseDataContext
    {
        protected override PeriodTopperBaseDataContext getTopNDataContext()
        {
            return new PeriodTopExpensesDataContext();
        }
    }
}
