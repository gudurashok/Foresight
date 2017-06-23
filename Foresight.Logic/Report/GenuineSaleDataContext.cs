using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Sql;

namespace ScalableApps.Foresight.Logic.Report
{
    public class GenuineSaleDataContext : ReportDataContext
    {
        private IList<KeyValuePair<int, decimal>> _balances;

        public override ReportData GetReportData()
        {
            _balances = Session.Dbc.GetAccountBalances(PartyGrouping);
            var report = loadData(readData());
            return new ReportData(report);
        }

        private IDataReader readData()
        {
            var cmd = db.CreateCommand(getSelectGenuineSaleQuery());
            return cmd.ExecuteReader();
        }

        private string getSelectGenuineSaleQuery()
        {
            var coaIds = ChartOfAccount.GetChartOfAccountIds(AccountTypes.Customers);
            return string.Format(ReportQueries.SelectGenuineSaleRatio,
                                        getPartyIdentityColumn(), coaIds);
        }

        private IList<GenuineSale> loadData(IDataReader rdr)
        {
            var result = new List<GenuineSale>();
            while (rdr.Read())
            {
                var gs = new GenuineSale();
                gs.AccountId = Convert.ToInt32(rdr["AccountId"]);
                gs.Name = rdr["Name"].ToString();
                gs.SaleAmount = Convert.ToDecimal(rdr["SaleAmount"]);
                gs.ReceiptAmount = Convert.ToDecimal(rdr["ReceiptAmount"]);
                gs.BalanceAmount = _balances.SingleOrDefault(b => b.Key == gs.AccountId).Value;
                calculateGenuineSalePct(gs);
                result.Add(gs);
            }

            rdr.Close();
            return result;
        }

        private void calculateGenuineSalePct(GenuineSale gsr)
        {
            if (gsr.BalanceAmount < 0)
                gsr.ReceiptAmount += Math.Abs(gsr.BalanceAmount);
            else
                gsr.ReceiptAmount -= Math.Abs(gsr.BalanceAmount);

            if (gsr.SaleAmount != 0)
                gsr.GenuineSalePct = Math.Round(gsr.ReceiptAmount / gsr.SaleAmount * 100, 2);
        }
    }
}
