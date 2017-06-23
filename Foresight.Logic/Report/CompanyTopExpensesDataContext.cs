using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Sql;

namespace ScalableApps.Foresight.Logic.Report
{
    public class CompanyTopExpensesDataContext : CompanyTopperBaseDataContext
    {
        private const int expensesChartOfAccountId = 4;

        protected override string getCompanyPeriodTotalValueQuery(string filterExpr)
        {
            var coaIds = ChartOfAccount.GetChartOfAccountIds(expensesChartOfAccountId);
            return string.Format(ReportQueries.SelectTotalOfExpense, filterExpr, coaIds);
        }

        protected override string getAccountsQuery(string filterExpr)
        {
            var coaIds = ChartOfAccount.GetChartOfAccountIds(expensesChartOfAccountId);
            return string.Format(ReportQueries.SelectTopNExpenses, TopNCount,
                            getPartyIdentityColumn(), filterExpr, coaIds);
        }
    }
}
