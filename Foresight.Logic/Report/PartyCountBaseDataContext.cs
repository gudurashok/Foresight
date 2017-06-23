using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Sql;

namespace ScalableApps.Foresight.Logic.Report
{
    public abstract class PartyCountBaseDataContext : ReportDataContext
    {
        public override ReportData GetReportData()
        {
            return new ReportData(loadData(getSelectedCoPeriods()));
        }

        private IList<NewLostPartyCount> loadData(IList<CompanyPeriod> coPeriods)
        {
            var result = new List<NewLostPartyCount>();
            int inPeriodId = 0;

            foreach (var periodId in getDistinctPeriods(coPeriods))
            {
                var rdr = readData(inPeriodId, periodId);
                if (rdr.Read())
                    result.Add(readValue(rdr, coPeriods.First(cp => cp.Period.Id == periodId)));

                rdr.Close();
                inPeriodId = periodId;
            }

            return result;
        }

        protected abstract IEnumerable<int> getDistinctPeriods(IEnumerable<CompanyPeriod> coPeriods);

        private IDataReader readData(int inPeriodId, int outPeriodId)
        {
            var cmd = db.CreateCommand(getQuery());
            db.AddParameterWithValue(cmd, "@inPeriodId", inPeriodId);
            db.AddParameterWithValue(cmd, "@outPeriodId", outPeriodId);
            return cmd.ExecuteReader();
        }

        private string getQuery()
        {
            string coaIds = ChartOfAccount.GetChartOfAccountIds(AccountTypes.Customers);
            return string.Format(ReportQueries.SelectCountOfSalePartyAssociationsOfYear, 
                                getPartyIdentityColumn(), coaIds);
        }

        private NewLostPartyCount readValue(IDataReader rdr, CompanyPeriod cp)
        {
            var pc = new NewLostPartyCount();
            pc.CompanyPeriod = cp;
            pc.Count = Convert.ToInt32(rdr["TCount"]);
            pc.Amount = rdr["TAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(rdr["TAmount"]);
            return pc;
        }
    }
}
