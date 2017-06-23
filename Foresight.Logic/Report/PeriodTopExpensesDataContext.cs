using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Sql;

namespace ScalableApps.Foresight.Logic.Report
{
    public class PeriodTopExpensesDataContext : PeriodTopperBaseDataContext
    {
        private const int expensesChartOfAccountId = 4;

        protected override string getCompanyPeriodTotalValueQuery(string filterExpr)
        {
            string coaIds = ChartOfAccount.GetChartOfAccountIds(expensesChartOfAccountId);
            return string.Format(ReportQueries.SelectTotalOfExpense, filterExpr, coaIds);
        }

        protected override string getAccountsQuery(string filterExpr)
        {
            string coaIds = ChartOfAccount.GetChartOfAccountIds(expensesChartOfAccountId);
            return string.Format(ReportQueries.SelectTopNExpenses, TopNCount,
                                getPartyIdentityColumn(), filterExpr, coaIds);
        }
    }
}
