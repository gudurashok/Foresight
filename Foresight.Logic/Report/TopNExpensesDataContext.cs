using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Sql;

namespace ScalableApps.Foresight.Logic.Report
{
    public class TopNExpensesDataContext : TopNAccountsBaseDataContext
    {
        private const int expensesChartOfAccountId = 4;

        private const string queryPrefix = "AND CompanyPeriodId IN ";

        protected override string getTotalValueQuery()
        {
            string coaIds = ChartOfAccount.GetChartOfAccountIds(expensesChartOfAccountId);
            return string.Format(ReportQueries.SelectTotalOfExpense,
                                createFilterExprFrom(queryPrefix, getCoPeriodIds()), coaIds);
        }

        protected override string getTopNQuery()
        {
            string coaIds = ChartOfAccount.GetChartOfAccountIds(expensesChartOfAccountId);
            return string.Format(ReportQueries.SelectTopNExpenses,
                                TopNCount, getPartyIdentityColumn(),
                                createFilterExprFrom(queryPrefix, getCoPeriodIds()), coaIds);
        }
    }
}
