namespace ScalableApps.Foresight.Logic.Report
{
    public class CompanyCustomerDataContext : CompanyAccountBaseDataContext
    {
        protected override CompanyTopperBaseDataContext getTopNDataContext()
        {
            return new CompanyTopCustomersDataContext();
        }
    }
}
