using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Sql;

namespace ScalableApps.Foresight.Logic.Report
{
    public class TopNVendorPurchaseItemsDataContext : TopNAccountItemsBaseDataContext
    {
        private const string columnName = "Purchase";

        protected override string getTopNQuery()
        {
            return string.Format(ReportQueries.SelectTopNAccountItems,
                                TopNCount, getPartyIdentityColumn(), getItemIdentityColumn(), getTableName(),
                                columnName, ChartOfAccount.GetChartOfAccountIds(AccountTypes.Vendors));
        }
    }
}
