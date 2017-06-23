using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ScalableApps.Foresight.Logic.Report
{
    public abstract class TopNAccountsBaseDataContext : ReportDataContext
    {
        public int TopNCount { protected get; set; }

        public override ReportData GetReportData()
        {
            return new ReportData(loadData(readData()));
        }

        private decimal getTotalValue()
        {
            var value = db.ExecuteScalar(getTotalValueQuery());
            return value == DBNull.Value ? 1 : Convert.ToDecimal(value);
        }

        protected abstract string getTotalValueQuery();

        protected IDataReader readData()
        {
            var cmd = db.CreateCommand(getTopNQuery());
            return cmd.ExecuteReader();
        }

        protected abstract string getTopNQuery();

        protected IList<AccountValue> loadData(IDataReader rdr)
        {
            var total = getTotalValue();
            var result = new List<AccountValue>();
            while (rdr.Read())
            {
                var accValue = new AccountValue();
                accValue.Id = Convert.ToInt32(rdr["Id"]);
                accValue.Name = rdr["Name"].ToString();
                accValue.Amount = Convert.ToDecimal(rdr["TotalAmount"]);

                if (total != 0)
                    accValue.Percentage = (Convert.ToDecimal(rdr["TotalAmount"]) / total * 100);

                result.Add(accValue);
            }
            rdr.Close();
            return result.OrderByDescending(r => r.Amount).ToList();
        }
    }
}
