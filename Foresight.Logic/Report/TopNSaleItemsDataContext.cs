using ScalableApps.Foresight.Logic.Sql;

namespace ScalableApps.Foresight.Logic.Report
{
    public class TopNSaleItemsDataContext : TopNItemsBaseDataContext
    {
        private const string queryPrefix = " WHERE CompanyPeriodId IN ";
        private const string columnName = "Sale";

        protected override string getTotalValueQuery()
        {
            return string.Format(ReportQueries.SelectTotalOfItems, columnName,
                                createFilterExprFrom(queryPrefix, getCoPeriodIds()));
        }

        protected override string getTopNQuery()
        {
            return string.Format(ReportQueries.SelectTopNItems,
                                TopNCount, getItemIdentityColumn(), columnName, getTableName(),
                                createFilterExprFrom(queryPrefix, getCoPeriodIds()));
        }
    }
}
