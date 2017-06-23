using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Sql;

namespace ScalableApps.Foresight.Logic.Report
{
    public class BuyingTrendDataContext : ReportDataContext
    {
        public IList<int> SelectedAccountIds { private get; set; }

        public override ReportData GetReportData()
        {
            return new ReportData(loadData(readData()));
        }

        private IDataReader readData()
        {
            var coaIDs = ChartOfAccount.GetChartOfAccountIds(AccountTypes.Customers);
            var cmd = db.CreateCommand(string.Format(ReportQueries.SelectBuyingTrend,
                            getPartyIdentityColumn(), coaIDs, getFilterExpr()));
            return cmd.ExecuteReader();
        }

        private string getFilterExpr()
        {
            var sb = new StringBuilder();
            sb.Append(createFilterExprFrom(" AND al.CompanyPeriodId IN ", getCoPeriodIds()));

            if (SelectedAccountIds.Count > 0)
                sb.Append(string.Format(" AND {0} IN {1} ", getPartyGroupingExpr(),
                            createFilterExprFrom(SelectedAccountIds)));

            return sb.ToString();
        }

        private string getPartyGroupingExpr()
        {
            return PartyGrouping ? "a.GroupId" : "a.Id";
        }

        private IList<BuyingTrendValue> loadData(IDataReader rdr)
        {
            var result = new List<BuyingTrendValue>();
            while (rdr.Read())
                result.Add(new BuyingTrendValue
                               {
                                   Period = Session.Dbc.GetDatePeriodById(Convert.ToInt32(rdr["PeriodId"])),
                                   Month = Convert.ToInt32(rdr["Month"]),
                                   Amount = Convert.ToDecimal(rdr["Amount"])
                               });

            rdr.Close();
            return result;
        }
    }
}
