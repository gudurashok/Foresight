using System.Collections.Generic;
using System.Text;

namespace ScalableApps.Foresight.Logic.Common
{
    public class AccountTransTables
    {
        public string TransAmount { get; private set; }
        public string TransName { get; private set; }
        public string TableName { get; private set; }
        public string Filter { get; private set; }

        public static IEnumerable<AccountTransTables> GetAllTransactionTables()
        {
            var result = new List<AccountTransTables>();
            result.Add(getOpeningTable());
            result.AddRange(GetCreditDebitTables());
            return result;
        }

        private static AccountTransTables getOpeningTable()
        {
            return new AccountTransTables { TransAmount = "Amount", TransName = "Opening", TableName = "OpeningBalance" };
        }

        public static IEnumerable<AccountTransTables> GetCreditDebitTables()
        {
            var result = new List<AccountTransTables>();
            result.AddRange(getCreditTables());
            result.AddRange(getDebitTables());
            return result;
        }

        private static IEnumerable<AccountTransTables> getCreditTables()
        {
            var result = new List<AccountTransTables>();
            result.Add(new AccountTransTables { TransAmount = "Amount", TransName = "Purchase", TableName = "PurchaseInvoiceHeader" });
            result.Add(new AccountTransTables { TransAmount = "Amount", TransName = "CashPayment", TableName = "CashTransaction", Filter = "TxnType = 0" });
            result.Add(new AccountTransTables { TransAmount = "Amount", TransName = "BankReceipt", TableName = "BankReceipt" });
            result.Add(new AccountTransTables { TransAmount = "Amount", TransName = "CreditJV", TableName = "JournalVoucher", Filter = "TxnType = 0" });
            return result;
        }

        private static IEnumerable<AccountTransTables> getDebitTables()
        {
            var result = new List<AccountTransTables>();
            result.Add(new AccountTransTables { TransAmount = "-Amount", TransName = "Sale", TableName = "SaleInvoiceHeader" });
            result.Add(new AccountTransTables { TransAmount = "-Amount", TransName = "CashReceipt", TableName = "CashTransaction", Filter = "TxnType = 1" });
            result.Add(new AccountTransTables { TransAmount = "-Amount", TransName = "BankPayment", TableName = "BankPayment" });
            result.Add(new AccountTransTables { TransAmount = "-Amount", TransName = "CreditNote", TableName = "CreditNoteHeader" });
            result.Add(new AccountTransTables { TransAmount = "-Amount", TransName = "DebitNote", TableName = "DebitNoteHeader" });
            result.Add(new AccountTransTables { TransAmount = "-Amount", TransName = "DebitJV", TableName = "JournalVoucher", Filter = "TxnType = 1" });
            return result;
        }

        public static string GetCreditAmountExpr()
        {
            return createAmountExpr(getCreditTables());
        }

        public static string GetDebitAmountExpr()
        {
            return createAmountExpr(getDebitTables());
        }

        private static string createAmountExpr(IEnumerable<AccountTransTables> transTables)
        {
            var sb = new StringBuilder("(");

            foreach (var table in transTables)
                sb.Append("SUM(").Append(table.TransName).Append(") + ");

            sb.Remove(sb.Length - 3, 3).Append(")");

            return sb.ToString();
        }
    }
}
