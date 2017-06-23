using Ferry.Logic.Base;
using ScalableApps.Foresight.Logic.Business;

namespace Ferry.Logic.Common
{
    internal class ImportingEventArgsFactory
    {
        private readonly DataImporter _importer;

        public ImportingEventArgsFactory(DataImporter importer)
        {
            _importer = importer;
        }

        internal ImportingEventArgs ForAccountOpeningBalance(Account account)
        {
            return new ImportingEventArgs(string.Format("Done account: {0}", account.Name));
        }

        public ImportingEventArgs ForSaleInvoice(string daybookCode, string docNr)
        {
            return new ImportingEventArgs(string.Format("Done sale invoice: daybook:{0}, #{1}", daybookCode, docNr));
        }

        public ImportingEventArgs ForPurchaseInvoice(string daybookCode, string docNr)
        {
            return new ImportingEventArgs(string.Format("Done purchase invoice: daybook:{0}, #{1}", daybookCode, docNr));
        }

        public ImportingEventArgs ForCashTransaction(string daybookCode, string txnType, string docNr)
        {
            return new ImportingEventArgs(string.Format("Done cash: daybook:{0}, {1}, #{2}", daybookCode, txnType, docNr));
        }

        public ImportingEventArgs ForBankTransaction(string daybookCode, string txnType, string docNr)
        {
            return new ImportingEventArgs(string.Format("Done bank: daybook:{0}, {1}, #{2}", daybookCode, txnType, docNr));
        }

        public ImportingEventArgs ForCreditNote(string daybookCode, string docNr)
        {
            return new ImportingEventArgs(string.Format("Done credit note: daybook:{0}, #{1}", daybookCode, docNr));
        }

        public ImportingEventArgs ForDebitNote(string daybookCode, string docNr)
        {
            return new ImportingEventArgs(string.Format("Done debit note: daybook:{0}, #{1}", daybookCode, docNr));
        }

        public ImportingEventArgs ForJournalVoucher(string daybookCode, string docNr)
        {
            return new ImportingEventArgs(string.Format("Done JV: daybook:{0}, #{1}", daybookCode, docNr));
        }

        public ImportingEventArgs ForItemLot(string docNr)
        {
            return new ImportingEventArgs(string.Format("Done item-lot: #{0}", docNr));
        }

        public ImportingEventArgs ForInventoryIssue(string docNr)
        {
            return new ImportingEventArgs(string.Format("Done inventory issue: #{0}", docNr));
        }

        public ImportingEventArgs ForInventoryReceive(string daybookCode, string docNr)
        {
            return new ImportingEventArgs(string.Format("Done inventory receive: daybook:{0}, #{1}", daybookCode, docNr));
        }

        public ImportingEventArgs ForMiscInventoryIssue(string daybookCode, string docNr)
        {
            return new ImportingEventArgs(string.Format("Done misc. inventory issue: daybook:{0}, #{1}", daybookCode, docNr));
        }

        public void RaiseImportingEvent(string text)
        {
            RaiseImportingEvent(new ImportingEventArgs(text));
        }

        public void RaiseImportingEvent(ImportingEventArgs e)
        {
            _importer.RaiseImportingEvent(e);
        }
    }
}
