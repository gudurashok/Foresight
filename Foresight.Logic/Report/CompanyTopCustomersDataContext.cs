using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Sql;

namespace ScalableApps.Foresight.Logic.Report
{
    public class CompanyTopCustomersDataContext : CompanyTopperBaseDataContext
    {
        protected override string getCompanyPeriodTotalValueQuery(string filterExpr)
        {
            return string.Format(ReportQueries.SelectTotalOfSale, 
                                filterExpr, getTypeCodes());
        }

        protected override string getAccountsQuery(string filterExpr)
        {
            return string.Format(ReportQueries.SelectTopNCustomers, TopNCount, 
                                getPartyIdentityColumn(), getTypeCodes(),
                                filterExpr);
        }

        private string getTypeCodes()
        {
            return ChartOfAccount.GetChartOfAccountIds(AccountTypes.Customers);
        }
    }
}
