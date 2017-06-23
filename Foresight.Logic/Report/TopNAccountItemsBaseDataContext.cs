using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ScalableApps.Foresight.Logic.Report
{
    public abstract class TopNAccountItemsBaseDataContext : ReportDataContext
    {
        public int TopNCount { protected get; set; }
        private const string tableName = "Item";

        public override ReportData GetReportData()
        {
            return new ReportData(loadData(readData()));
        }

        protected IDataReader readData()
        {
            var cmd = db.CreateCommand(getTopNQuery());
            return cmd.ExecuteReader();
        }

        protected abstract string getTopNQuery();

        protected IList<AccountItemValue> loadData(IDataReader rdr)
        {
            var result = new List<AccountItemValue>();
            while (rdr.Read())
                result.Add(new AccountItemValue
                               {
                                   AccountId = Convert.ToInt32(rdr["AccountNr"]),
                                   AccountName = rdr["AccountName"].ToString(),
                                   ItemName = rdr["ItemName"].ToString(),
                                   Amount = Convert.ToDecimal(rdr["TotalAmount"]),
                               });

            rdr.Close();
            return result.OrderByDescending(r => r.Amount).ToList();
        }

        protected string getTableName()
        {
            return ItemGrouping ? tableName + "Group" : tableName;
        }
    }
}
