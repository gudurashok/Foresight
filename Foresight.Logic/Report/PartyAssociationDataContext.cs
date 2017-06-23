using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Sql;

namespace ScalableApps.Foresight.Logic.Report
{
    public abstract class PartyAssociationDataContext : ReportDataContext
    {
        public int TopNCount { private get; set; }
        public int DaysSince { private get; set; }

        public override ReportData GetReportData()
        {
            return new ReportData(loadData(readData()));
        }

        private IDataReader readData()
        {
            var cmd = db.CreateCommand(getQuery());
            db.AddParameterWithValue(cmd, "@days", DaysSince);
            return cmd.ExecuteReader();
        }

        private string getQuery()
        {
            string coaIds = ChartOfAccount.GetChartOfAccountIds(AccountTypes.Customers);
            return string.Format(ReportQueries.SelectTopNSalePartyAssociations,
                                TopNCount, getPartyIdentityColumn(),
                                getOuterQueryFilterOpr(),
                                getInnerQueryFilterOpr(), coaIds);
        }

        protected abstract string getInnerQueryFilterOpr();
        protected abstract string getOuterQueryFilterOpr();

        private IList<NewLostPartyValue> loadData(IDataReader rdr)
        {
            var result = new List<NewLostPartyValue>();
            while (rdr.Read())
            {
                var lc = new NewLostPartyValue();
                lc.GroupId = Convert.ToInt32(rdr["Id"]);
                lc.Name = rdr["Name"].ToString();
                lc.FirstDate = Convert.ToDateTime(rdr["FirstDate"]);
                lc.LastDate = Convert.ToDateTime(rdr["LastDate"]);
                lc.Amount = Convert.ToDecimal(rdr["TotalAmount"]);
                lc.TransCount = Convert.ToInt32(rdr["TransCount"]);

                result.Add(lc);
            }

            rdr.Close();
            return result.OrderByDescending(r => r.Amount).ToList();
        }
    }
}
