namespace ScalableApps.Foresight.Logic.Report
{
    public class CompanyExpenseDataContext : CompanyAccountBaseDataContext
    {
        protected override CompanyTopperBaseDataContext getTopNDataContext()
        {
            return new CompanyTopExpensesDataContext();
        }
    }
}
