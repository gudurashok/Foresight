using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ScalableApps.Foresight.Logic.Report
{
    public abstract class TopNItemsBaseDataContext : ReportDataContext
    {
        public int TopNCount { protected get; set; }
        private const string tableName = "Item";

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

        protected IList<ItemValue> loadData(IDataReader rdr)
        {
            var total = getTotalValue();
            var result = new List<ItemValue>();
            while (rdr.Read())
            {
                var itemValue = new ItemValue();
                itemValue.Id = Convert.ToInt32(rdr["Id"]);
                itemValue.Name = rdr["Name"].ToString();
                itemValue.Amount = Convert.ToDecimal(rdr["TotalAmount"]);
                
                if (total != 0)
                    itemValue.Percentage = (Convert.ToDecimal(rdr["TotalAmount"]) / total * 100);

                result.Add(itemValue);
            }
            rdr.Close();
            return result.OrderByDescending(r => r.Amount).ToList();
        }

        protected string getTableName()
        {
            return ItemGrouping ? tableName + "Group" : tableName;
        }
    }
}
