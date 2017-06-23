using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.DataAccess;

namespace ScalableApps.Foresight.Logic.Import
{
    public class LedgerBuilder
    {
        protected readonly DataContext targetDbContext;
        protected readonly CompanyPeriod companyPeriod;
        private delegate void ExecuteUpdateDelegate();
        public event UpdatingEventHandler Updating;
        public bool IsCancelled;

        public LedgerBuilder(DataContext targetDbContext, CompanyPeriod companyPeriod)
        {
            this.targetDbContext = targetDbContext;
            this.companyPeriod = companyPeriod;
        }

        #region Build Dimension Tables

        public void BuildDimensionTables()
        {
            try
            {
                targetDbContext.BeginTransaction();
                targetDbContext.DeleteCompanyPeriodLedgerData(companyPeriod);
                OnUpdating(new UpdatingEventArgs("Building dimension tables"));
                executeImport(populateAccountLedger);
                executeImport(populateItemLedger);
                targetDbContext.Commit();
            }
            catch (Exception)
            {
                targetDbContext.Rollback();
                throw;
            }
        }

        #endregion

        private void executeImport(ExecuteUpdateDelegate executeUpdateDelegate)
        {
            if (IsCancelled) return;
            executeUpdateDelegate();
        }


        public void CancelUpdation()
        {
            IsCancelled = true;
        }

        #region Build Account Ledger

        private void populateAccountLedger()
        {
            var ledger = new List<AccountMonthlyLedger>();

            var trans = AccountTransTables.GetAllTransactionTables();
            foreach (var tran in trans)
            {
                populateAccountTransDimension(ledger, targetDbContext.ReadLedgerData(tran, companyPeriod.Id), tran);
                OnUpdating(new UpdatingEventArgs(string.Format("Building account ledger {0}", tran.TransName)));
                if (IsCancelled) break;
            }

            foreach (var account in ledger)
            {
                targetDbContext.InsertAccountLedger(account);
                OnUpdating(new UpdatingEventArgs(string.Format("Building account ledger {0}", account.AccountId)));
                if (IsCancelled) break;
            }
        }

        private void populateAccountTransDimension(IList<AccountMonthlyLedger> ledger, IDataReader rdr, AccountTransTables tran)
        {
            while (rdr.Read())
            {
                var accountId = Convert.ToInt32(rdr["AccountId"]);
                var month = Convert.ToInt32(rdr["Month"]);
                var al = ledger.SingleOrDefault(a => a.AccountId == accountId && a.Month == month);
                if (al == null)
                {
                    al = new AccountMonthlyLedger
                    {
                        AccountId = accountId,
                        ChartOfAccountId = Convert.ToInt32(rdr["ChartOfAccountId"]),
                        Month = month,
                        CompanyPeriod = companyPeriod,

                    };
                    al.GetType().GetProperty(tran.TransName).SetValue(al, Convert.ToDecimal(rdr["Amount"]), null);
                    ledger.Add(al);
                }
                else
                {
                    var amount = Convert.ToDecimal(al.GetType().GetProperty(tran.TransName).GetValue(al, null));
                    al.GetType().GetProperty(tran.TransName).SetValue(al, amount + Convert.ToDecimal(rdr["Amount"]), null);
                }
            }

            rdr.Close();
        }

        #endregion

        #region Build Item Ledger

        private void populateItemLedger()
        {
            var items = new List<ItemMonthlyLedger>();
            var trans = ItemTransTables.GetTransactionTables();

            foreach (var tran in trans)
            {
                populateItemLedgerDimension(items, tran);
                OnUpdating(new UpdatingEventArgs(string.Format("Building item ledger {0}", tran.TransName)));
                if (IsCancelled) break;
            }

            foreach (var item in items)
            {
                targetDbContext.InsertItemLedger(item);
                OnUpdating(new UpdatingEventArgs(string.Format("Building item ledger {0}", item.ItemId)));
                if (IsCancelled) break;
            }
        }

        private void populateItemLedgerDimension(IList<ItemMonthlyLedger> items, ItemTransTables tran)
        {
            var rdr = targetDbContext.ReadItemLedger(tran, companyPeriod.Id);

            while (rdr.Read())
            {
                var itemId = Convert.ToInt32(rdr["ItemId"]);
                var accountId = Convert.ToInt32(rdr["AccountId"]);
                var month = Convert.ToInt32(rdr["Month"]);
                var il = items.SingleOrDefault(i => i.ItemId == itemId
                                && i.AccountId == accountId && i.Month == month);
                if (il == null)
                {
                    il = new ItemMonthlyLedger
                    {
                        ItemId = itemId,
                        AccountId = accountId,
                        ChartOfAccountId = Convert.ToInt32(rdr["chartOfAccountId"]),
                        Month = month,
                        CompanyPeriod = companyPeriod
                    };
                    il.GetType().GetProperty(tran.TransName).SetValue(il, Convert.ToDecimal(rdr["Amount"]), null);
                    items.Add(il);
                }
                else
                {
                    var amount = Convert.ToDecimal(il.GetType().GetProperty(tran.TransName).GetValue(il, null));
                    il.GetType().GetProperty(tran.TransName).SetValue(il, amount + Convert.ToDecimal(rdr["Amount"]), null);
                }
            }

            rdr.Close();
        }

        #endregion

        public void OnUpdating(UpdatingEventArgs e)
        {
            if (Updating != null)
                Updating(this, e);
        }
    }
}
