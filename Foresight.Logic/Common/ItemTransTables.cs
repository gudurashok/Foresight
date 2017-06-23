using System.Collections.Generic;

namespace ScalableApps.Foresight.Logic.Common
{
    public class ItemTransTables
    {
        public string TransName { get; private set; }
        public string HeaderTableName { get; private set; }
        public string DetailTableName { get; private set; }

        public static IEnumerable<ItemTransTables> GetTransactionTables()
        {
            var result = new List<ItemTransTables>();
            result.Add(new ItemTransTables
            {
                TransName = "Sale",
                HeaderTableName = "SaleInvoiceHeader",
                DetailTableName = "SaleInvoiceLine"
            });
            
            result.Add(new ItemTransTables
            {
                TransName = "Purchase",
                HeaderTableName = "PurchaseInvoiceHeader",
                DetailTableName = "PurchaseInvoiceLine"
            });
            return result;
        }
    }
}
