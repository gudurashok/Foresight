using System;
using System.Collections.Generic;
using System.Linq;
using Ferry.Logic.Common;
using Ferry.Logic.Model;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.DataAccess;
using ExtractingEventArgs = Ferry.Logic.Common.ExtractingEventArgs;
using ImportingEventArgs = Ferry.Logic.Common.ImportingEventArgs;
using ImportingEventHandler = Ferry.Logic.Common.ImportingEventHandler;

namespace Ferry.Logic.Base
{
    public class DataImporter
    {
        #region Declarations

        protected readonly Database sourceDatabase;
        protected readonly DataContext targetDbContext;
        protected readonly CompanyPeriod companyPeriod;
        internal DataExtractorBase extractor;
        internal readonly ImportingEventArgsFactory ieaFactory;
        private delegate void ExecuteImportDelegate();
        public event ImportingEventHandler Importing;
        public bool IsCancelled;

        #endregion

        #region Constructor

        public DataImporter(Database sourceDatabase, DataContext targetDbContext, CompanyPeriod companyPeriod)
        {
            this.sourceDatabase = sourceDatabase;
            this.targetDbContext = targetDbContext;
            this.companyPeriod = companyPeriod;
            ieaFactory = new ImportingEventArgsFactory(this);
        }

        #endregion

        #region Progress Indicator

        private void onImporting(ImportingEventArgs e)
        {
            if (Importing != null)
                Importing(this, e);
        }

        internal void RaiseImportingEvent(ImportingEventArgs e)
        {
            onImporting(e);
        }

        protected void ReportProgress(ImportingEventArgs e)
        {
            ieaFactory.RaiseImportingEvent(e);
        }

        #endregion

        #region Txn Type

        protected int GetTxnType(string dbCr)
        {
            return dbCr == "C" ? 0 : 1;
        }

        protected string GetTxnTypeFullName(int txnType)
        {
            return txnType == 0 ? "Receipt" : "Payment";
        }

        #endregion

        #region Public Members

        public void Execute()
        {
            extract();
            importTransactions();
            updateCompanyPeriod();
            completeImport();
        }

        public void CancelImport()
        {
            IsCancelled = true;
        }

        #endregion

        #region Composed Main Methods

        private void extract()
        {
            ieaFactory.RaiseImportingEvent("Caching Data");
            extractor = DataExtractorFactory.GetInstance(
                    companyPeriod.SourceDataProvider, sourceDatabase, targetDbContext);

            extractor.ExtractingHandler += sourceDataExtractor_Caching;
            extractor.Extract();
            extractor.ExtractingHandler -= sourceDataExtractor_Caching;
        }

        private void sourceDataExtractor_Caching(object sender, ExtractingEventArgs e)
        {
            ieaFactory.RaiseImportingEvent(e.CurrentText);
        }

        private void importTransactions()
        {
            ieaFactory.RaiseImportingEvent("Transactions");

            executeImport(importAccountOpeningBalances);
            executeImport(importSaleInvoices);
            executeImport(importPurchaseInvoices);
            executeImport(importCashTransactions);
            executeImport(importBankTransactions);
            executeImport(importCreditNotes);
            executeImport(importDebitNotes);
            executeImport(importJournalVoucher);
            executeImport(importItemLots);
            executeImport(importInventoryIssue);
            executeImport(importInventoryReceives);
            executeImport(importMiscInventoryIssues);
        }

        private void executeImport(ExecuteImportDelegate executeImportMethod)
        {
            if (IsCancelled) return;
            executeImportMethod();
        }

        private void completeImport()
        {
            ieaFactory.RaiseImportingEvent("Completing import");
            targetDbContext.SetCompanyPeriodIsImported(companyPeriod, true);
        }

        private void updateCompanyPeriod()
        {
            ieaFactory.RaiseImportingEvent("Updating company period");
            targetDbContext.SetCompanyPeriodColumnValue(companyPeriod);
        }

        #endregion

        #region Transaction Import Methods

        #region Opening Balances

        private void importAccountOpeningBalances()
        {
            ieaFactory.RaiseImportingEvent("Opening Account Balances");
            loadAccountOpeningBalances(readAccountOpeningBalances());
        }

        private IEnumerable<SourceAccount> readAccountOpeningBalances()
        {
            return extractor.SourceAccounts.Where(a => a.OpeningBalance != 0);
        }

        private void loadAccountOpeningBalances(IEnumerable<SourceAccount> sourceAccounts)
        {
            foreach (var aob in sourceAccounts.Select(getAccountOpeningBalance))
            {
                targetDbContext.SaveAccountOpeningBalance(aob);
                ReportProgress(ieaFactory.ForAccountOpeningBalance(aob.Account));
                if (IsCancelled) break;
            }
        }

        private AccountOpeningBalance getAccountOpeningBalance(SourceAccount sourceAccount)
        {
            var aob = new AccountOpeningBalance();
            aob.Account = extractor.loadAccount(extractor.getAccount(sourceAccount.Code));
            aob.Date = companyPeriod.Period.FinancialFrom;
            aob.Amount = sourceAccount.OpeningBalance;
            return aob;
        }

        #endregion

        #region Sales Invoice

        private void importSaleInvoices()
        {
            ieaFactory.RaiseImportingEvent("Sales");
            foreach (var daybook in readDaybooksByType(DaybookType.Sale))
                loadSaleInvoices(readTransactionsByDaybook(daybook), daybook);
        }

        private void loadSaleInvoices(IEnumerable<SourceTransaction> sourceTransactions,
                                        Daybook daybook)
        {
            foreach (var sourceTransaction in sourceTransactions)
            {
                var invoice = new SaleInvoice();
                invoice.Header = getSaleInvoiceHeader(sourceTransaction, daybook);
                if (isShippingInfoExist(sourceTransaction))
                    invoice.HeaderEx = getSaleInvoiceHeaderEx(sourceTransaction);

                invoice.Lines = loadSaleInvoiceLines(readInvoiceLines(sourceTransaction));
                invoice.Terms = loadSaleInvoiceBillTerms(readInvoiceBillTerms(sourceTransaction));

                targetDbContext.SaveSaleInvoice(invoice);
                ReportProgress(ieaFactory.ForSaleInvoice(invoice.Header.Daybook.Code, invoice.Header.DocumentNr));
                if (IsCancelled) break;
            }
        }

        private SaleInvoiceHeader getSaleInvoiceHeader(SourceTransaction sourceTransaction,
                                                       Daybook daybook)
        {
            var sale = new SaleInvoiceHeader();
            fillTransactionHeader(sourceTransaction, daybook, sale);

            sale.BrokerageAmount = 0; // sourceTransaction.BrokerageAmount;
            sale.Through = sourceTransaction.Through;
            sale.VehicleId = 0;
            sale.Transport = sourceTransaction.Transport;
            sale.ReferenceDocNr = sourceTransaction.ReferenceDocNr;
            sale.OrderId = 0;
            sale.DiscountPct = sourceTransaction.DiscountPct;
            sale.SaleTaxColumnId = 0;
            return sale;
        }

        private bool isShippingInfoExist(SourceTransaction sourceTransaction)
        {
            if (string.IsNullOrWhiteSpace(sourceTransaction.ShipToName)) return true;
            if (string.IsNullOrWhiteSpace(sourceTransaction.ShipToAddressLine1)) return true;
            if (string.IsNullOrWhiteSpace(sourceTransaction.ShipToAddressLine2)) return true;
            if (string.IsNullOrWhiteSpace(sourceTransaction.ShipToCity)) return true;

            return false;
        }

        private SaleInvoiceHeaderEx getSaleInvoiceHeaderEx(SourceTransaction sourceTransaction)
        {
            var headerEx = new SaleInvoiceHeaderEx();
            headerEx.ShipToName = sourceTransaction.ShipToName;
            headerEx.ShipToAddressLine1 = sourceTransaction.ShipToAddressLine1;
            headerEx.ShipToAddressLine2 = sourceTransaction.ShipToAddressLine2;
            headerEx.ShipToCity = sourceTransaction.ShipToCity;
            return headerEx;
        }

        private IList<SaleInvoiceLine> loadSaleInvoiceLines(IEnumerable<SourceLineItem> sourceLineItems)
        {
            return sourceLineItems.Select(getSaleInvoiceLineItem).ToList();
        }

        private SaleInvoiceLine getSaleInvoiceLineItem(SourceLineItem sourceLineItem)
        {
            var line = new SaleInvoiceLine();
            line.LineNr = sourceLineItem.LineNr;
            line.Item = extractor.loadItem(extractor.Items.SingleOrDefault(i => i.Code == sourceLineItem.ItemCode));
            line.ItemDescription = sourceLineItem.ItemName;
            line.Quantity1 = Convert.ToDouble(Util.ConvertDbNull(sourceLineItem.Quantity1));
            line.Packing = Convert.ToDouble(Util.ConvertDbNull(sourceLineItem.Packing));
            line.Quantity2 = Convert.ToDouble(Util.ConvertDbNull(sourceLineItem.Quantity2));
            line.Price = Convert.ToDecimal(Util.ConvertDbNull(sourceLineItem.Price));
            line.DiscountPct = Convert.ToDouble(Util.ConvertDbNull(sourceLineItem.DiscountPct));
            line.Amount = Convert.ToDecimal(Util.ConvertDbNull(sourceLineItem.LineItemAmount));
            return line;
        }

        private IList<SaleInvoiceTerm> loadSaleInvoiceBillTerms(IEnumerable<SourceLineItemTerm> sourceLineItemTerms)
        {
            var terms = new List<SaleInvoiceTerm>();
            foreach (var sourceLineItemTerm in sourceLineItemTerms)
            {
                var term = new SaleInvoiceTerm();
                readInvoiceTerm(sourceLineItemTerm, term);
                terms.Add(term);
            }
            return terms;
        }

        #endregion

        #region Purchase Invoice

        private void importPurchaseInvoices()
        {
            ieaFactory.RaiseImportingEvent("Purchase Invoices");
            foreach (var daybook in readDaybooksByType(DaybookType.Purchase))
                loadPurchaseInvoices(readTransactionsByDaybook(daybook), daybook);
        }

        private void loadPurchaseInvoices(IEnumerable<SourceTransaction> sourceTransactions,
                                          Daybook daybook)
        {
            foreach (var sourceTransaction in sourceTransactions)
            {
                var invoice = new PurchaseInvoice();
                invoice.Header = getPurchaseInvoiceHeader(sourceTransaction, daybook);
                invoice.Lines = loadPurchaseInvoiceLines(readInvoiceLines(sourceTransaction));
                invoice.Terms = loadPurchaseInvoiceBillTerms(readInvoiceBillTerms(sourceTransaction));

                targetDbContext.SavePurchaseInvoice(invoice);
                ReportProgress(ieaFactory.ForPurchaseInvoice(invoice.Header.Daybook.Code, invoice.Header.DocumentNr));
                if (IsCancelled) break;
            }
        }

        private PurchaseInvoiceHeader getPurchaseInvoiceHeader(SourceTransaction sourceTransaction,
                                                               Daybook daybook)
        {
            var purchase = new PurchaseInvoiceHeader();
            fillTransactionHeader(sourceTransaction, daybook, purchase);

            purchase.BrokerageAmount = 0; // sourceTransaction.BrokerageAmount;
            purchase.Through = sourceTransaction.Through;
            purchase.Transport = sourceTransaction.Transport;
            purchase.ReferenceDocNr = sourceTransaction.ReferenceDocNr;
            purchase.OrderId = 0;
            purchase.DiscountPct = sourceTransaction.DiscountPct;
            purchase.SaleTaxColumnId = 0;
            return purchase;
        }

        private IList<PurchaseInvoiceLine> loadPurchaseInvoiceLines(IEnumerable<SourceLineItem> sourceLineItems)
        {
            return sourceLineItems.Select(getPurchaseInvoiceLineItem).ToList();
        }

        private PurchaseInvoiceLine getPurchaseInvoiceLineItem(SourceLineItem sourceLineItem)
        {
            var line = new PurchaseInvoiceLine();
            line.Item = extractor.loadItem(extractor.Items.SingleOrDefault(i => i.Code == sourceLineItem.ItemCode));
            line.LineNr = sourceLineItem.LineNr;
            line.ItemDescription = sourceLineItem.ItemName;
            line.Quantity1 = sourceLineItem.Quantity1;
            line.Packing = sourceLineItem.Packing;
            line.Quantity2 = sourceLineItem.Quantity2;
            line.Cost = sourceLineItem.Price;
            line.DiscountPct = sourceLineItem.DiscountPct;
            line.Amount = sourceLineItem.LineItemAmount;
            return line;
        }

        private IList<PurchaseInvoiceTerm> loadPurchaseInvoiceBillTerms(
            IEnumerable<SourceLineItemTerm> sourceLineItemTerms)
        {
            var terms = new List<PurchaseInvoiceTerm>();
            foreach (var sourceLineItemTerm in sourceLineItemTerms)
            {
                var term = new PurchaseInvoiceTerm();
                readInvoiceTerm(sourceLineItemTerm, term);
                terms.Add(term);
            }

            return terms;
        }

        #endregion

        #region Sale and Purchase invoice common

        private IEnumerable<SourceLineItemTerm> readInvoiceBillTerms(SourceTransaction sourceTransaction)
        {
            return extractor.SourceLineItemTerms.Where(it => it.DocumentNr == sourceTransaction.DocumentNr);
        }

        private void readInvoiceTerm(SourceLineItemTerm sourceLineItemTerm, InvoiceTerm term)
        {
            term.TermId = sourceLineItemTerm.TermId;
            term.Description = sourceLineItemTerm.Description;
            term.Percentage = sourceLineItemTerm.Percentage;
            term.Amount = sourceLineItemTerm.Amount;
            term.Account = extractor.loadAccount(extractor.getAccount(sourceLineItemTerm.AccountCode));
        }

        private IEnumerable<SourceLineItem> readInvoiceLines(SourceTransaction sourceTransaction)
        {
            return extractor.SourceLineItems.Where(li => li.AccountCode == sourceTransaction.AccountCode &&
                                                    li.DaybookCode == sourceTransaction.DaybookCode &&
                                                    li.DocumentNr == sourceTransaction.DocumentNr);
        }

        #endregion

        #region Cash

        private void importCashTransactions()
        {
            ieaFactory.RaiseImportingEvent("Cash");

            foreach (var daybook in readDaybooksByType(DaybookType.Cash))
                loadCashTransactions(readTransactionsByDaybook(daybook), daybook);
        }

        private void loadCashTransactions(IEnumerable<SourceTransaction> sourceTransactions,
                                          Daybook daybook)
        {
            foreach (var sourceTransaction in sourceTransactions)
            {
                var cash = getCashTransaction(sourceTransaction, daybook);
                targetDbContext.SaveCashTransaction(cash);
                ReportProgress(ieaFactory.ForCashTransaction(cash.Daybook.Code,
                                                    GetTxnTypeFullName(cash.TxnType),
                                                    cash.DocumentNr));
                if (IsCancelled) break;
            }
        }

        private CashTransaction getCashTransaction(SourceTransaction sourceTransaction, Daybook daybook)
        {
            var cash = new CashTransaction();
            fillTransactionHeader(sourceTransaction, daybook, cash);
            cash.TxnType = GetTxnType(sourceTransaction.TransactionType);
            return cash;
        }

        #endregion

        #region Bank

        private void importBankTransactions()
        {
            ieaFactory.RaiseImportingEvent("Bank");

            foreach (var daybook in readDaybooksByType(DaybookType.Bank))
                loadBankTransactions(readTransactionsByDaybook(daybook), daybook);
        }

        private void loadBankTransactions(IEnumerable<SourceTransaction> sourceTransactions,
                                          Daybook daybook)
        {
            foreach (var sourceTransaction in sourceTransactions)
            {
                insertBankTransaction(sourceTransaction, daybook);
                ReportProgress(ieaFactory.ForBankTransaction(daybook.Code,
                                    GetTxnTypeFullName(GetTxnType(sourceTransaction.TransactionType)),
                                                                    sourceTransaction.DocumentNr));
                if (IsCancelled) break;
            }
        }

        private void insertBankTransaction(SourceTransaction sourceTransaction, Daybook daybook)
        {
            if (sourceTransaction.TransactionType == "C")
                targetDbContext.SaveBankReceipt(getBankReceipt(sourceTransaction, daybook));
            else
                targetDbContext.SaveBankPayment(getBankPayment(sourceTransaction, daybook));
        }

        private BankReceipt getBankReceipt(SourceTransaction sourceTransaction, Daybook daybook)
        {
            var receipt = new BankReceipt();
            fillTransactionHeader(sourceTransaction, daybook, receipt);

            receipt.ChequeNr = sourceTransaction.ChequeNr;
            receipt.BankBranchName = sourceTransaction.BankBranchName;
            receipt.IsRealised = isChequeRealised(sourceTransaction.ChequeDate);
            return receipt;
        }

        private BankPayment getBankPayment(SourceTransaction sourceTransaction, Daybook daybook)
        {
            var payment = new BankPayment();
            fillTransactionHeader(sourceTransaction, daybook, payment);

            payment.ChequeNr = sourceTransaction.ChequeNr;
            payment.IsRealised = isChequeRealised(sourceTransaction.ChequeDate);
            return payment;
        }

        private bool isChequeRealised(string passed)
        {
            return !string.IsNullOrWhiteSpace(passed);
        }

        #endregion

        #region Credit Note

        private void importCreditNotes()
        {
            ieaFactory.RaiseImportingEvent("Credit Notes");

            foreach (var daybook in readDaybooksByType(DaybookType.CreditNote))
                loadCreditNotes(readTransactionsByDaybook(daybook), daybook);
        }

        private void loadCreditNotes(IEnumerable<SourceTransaction> sourceTransactions,
                                     Daybook daybook)
        {
            foreach (var sourceTransaction in sourceTransactions)
            {
                var note = new CreditNote();
                note.Header = getCreditNoteHeader(sourceTransaction, daybook);
                note.Lines = getLineItems(sourceTransaction).Select(getCreditNoteLineItem).ToList();

                targetDbContext.SaveCreditNote(note);
                ReportProgress(ieaFactory.ForCreditNote(note.Header.Daybook.Code, note.Header.DocumentNr));
                if (IsCancelled) break;
            }
        }

        private CreditNoteHeader getCreditNoteHeader(SourceTransaction sourceTransaction, Daybook daybook)
        {
            var note = new CreditNoteHeader();
            fillTransactionHeader(sourceTransaction, daybook, note);
            return note;
        }

        private CreditNoteLine getCreditNoteLineItem(SourceLineItem sourceLineItem)
        {
            var line = new CreditNoteLine();
            line.LineNr = 0; // Convert.ToInt32(sourceData["ITSR"]);
            line.Item = extractor.loadItem(extractor.getItem(sourceLineItem.ItemCode));
            line.Quantity1 = sourceLineItem.Quantity1;
            line.Quantity2 = sourceLineItem.Quantity2;
            line.Quantity3 = sourceLineItem.Quantity3;
            line.Cost = sourceLineItem.Price;
            line.Amount = sourceLineItem.LineItemAmount;
            return line;
        }

        #endregion

        #region Debit Note

        private void importDebitNotes()
        {
            ieaFactory.RaiseImportingEvent("Debit Notes");

            foreach (var daybook in readDaybooksByType(DaybookType.DebitNote))
                loadDebitNotes(readTransactionsByDaybook(daybook), daybook);
        }

        private void loadDebitNotes(IEnumerable<SourceTransaction> sourceTransactions,
                                    Daybook daybook)
        {
            foreach (var sourceTransaction in sourceTransactions)
            {
                var note = new DebitNote();
                note.Header = getDebitNoteHeader(sourceTransaction, daybook);
                note.Lines = getLineItems(sourceTransaction).Select(getDebitNoteLineItem).ToList();

                targetDbContext.SaveDebitNote(note);
                ReportProgress(ieaFactory.ForDebitNote(note.Header.Daybook.Code, note.Header.DocumentNr));
                if (IsCancelled) break;
            }
        }

        private DebitNoteHeader getDebitNoteHeader(SourceTransaction sourceTransaction, Daybook daybook)
        {
            var note = new DebitNoteHeader();
            fillTransactionHeader(sourceTransaction, daybook, note);
            return note;
        }

        private DebitNoteLine getDebitNoteLineItem(SourceLineItem sourceLineItem)
        {
            var line = new DebitNoteLine();
            line.LineNr = 0; // Convert.ToInt32(sourceData["ITSR"]);
            line.Item = extractor.loadItem(extractor.getItem(sourceLineItem.ItemCode));
            line.Quantity1 = sourceLineItem.Quantity1;
            line.Quantity2 = sourceLineItem.Quantity2;
            line.Quantity3 = sourceLineItem.Quantity3;
            line.Price = sourceLineItem.Price;
            line.Amount = sourceLineItem.LineItemAmount;
            return line;
        }

        #endregion

        #region Journal Voucher

        private void importJournalVoucher()
        {
            ieaFactory.RaiseImportingEvent("Journal Vouchers");
            foreach (var daybook in readDaybooksByType(DaybookType.JournalVoucher))
                loadJournalVoucher(readTransactionsByDaybook(daybook), daybook);
        }

        private void loadJournalVoucher(IEnumerable<SourceTransaction> sourceTransactions,
                                        Daybook daybook)
        {
            foreach (var sourceTransaction in sourceTransactions)
            {
                var jv = getJournalVoucher(sourceTransaction, daybook);
                targetDbContext.SaveJournalVoucher(jv);
                ReportProgress(ieaFactory.ForJournalVoucher(daybook.Code, jv.DocumentNr));
                if (IsCancelled) break;
            }
        }

        private JournalVoucher getJournalVoucher(SourceTransaction sourceTransaction, Daybook daybook)
        {
            var jv = new JournalVoucher();
            fillTransactionHeader(sourceTransaction, daybook, jv);
            jv.TxnType = GetTxnType(sourceTransaction.TransactionType);
            return jv;
        }

        #endregion

        #region Item Lots

        private IEnumerable<SourceLineItem> readPurchaseLineItemsAsItemLots()
        {
            return extractor.SourceLineItems.Where(il => il.DocumentNr.StartsWith("P"));
        }

        private void importItemLots()
        {
            ieaFactory.RaiseImportingEvent("Item Lots");
            foreach (var lot in readPurchaseLineItemsAsItemLots().Select(getItemLot))
            {
                targetDbContext.SaveItemLot(lot);
                ReportProgress(ieaFactory.ForItemLot(lot.LotNr));
                if (IsCancelled) break;
            }
        }

        private ItemLot getItemLot(SourceLineItem sourceLineItem)
        {
            var lot = new ItemLot();
            lot.LotNr = sourceLineItem.DocumentNr;
            lot.Date = sourceLineItem.Date;
            lot.Account = extractor.loadAccount(extractor.getAccount(sourceLineItem.AccountCode));
            lot.LineNr = sourceLineItem.LineNr;
            lot.Item = extractor.loadItem(extractor.Items.SingleOrDefault(i => i.Code == sourceLineItem.ItemCode));
            lot.Quantity1 = sourceLineItem.Quantity1;
            lot.Packing = sourceLineItem.Packing;
            lot.Quantity2 = sourceLineItem.Quantity2;
            return lot;
        }

        #endregion

        #region Inventory Issue

        private void importInventoryIssue()
        {
            ieaFactory.RaiseImportingEvent("Inventory Issues");
            foreach (var daybook in readDaybooksByType(DaybookType.InventoryIssue))
                loadInventoryIssue(readInventoryIssue(daybook), daybook);
        }

        private IEnumerable<SourceInventoryIssue> readInventoryIssue(Daybook daybook)
        {
            return extractor.SourceInventoryIssues.Where(si => si.DaybookCode == daybook.Code);
        }

        private void loadInventoryIssue(IEnumerable<SourceInventoryIssue> sourceInventoryIssues,
                                        Daybook daybook)
        {
            foreach (var sourceInventoryIssue in sourceInventoryIssues)
            {
                var issue = getInventoryIssue(sourceInventoryIssue, daybook);
                targetDbContext.SaveInventoryIssue(issue);
                ReportProgress(ieaFactory.ForItemLot(issue.DocumentNr));
                if (IsCancelled) break;
            }
        }

        private InventoryIssue getInventoryIssue(SourceInventoryIssue sourceInventoryIssue,
                                                 Daybook daybook)
        {
            var issue = new InventoryIssue();
            issue.Daybook = daybook;
            issue.DocumentNr = sourceInventoryIssue.DocumentNr;
            issue.Date = sourceInventoryIssue.Date;
            issue.LotId = (getItemLotByLotNrLineNr(sourceInventoryIssue)).Id;
            issue.Account = extractor.loadAccount(extractor.getAccount(sourceInventoryIssue.AccountCode));
            issue.Quantity1 = sourceInventoryIssue.Quantity1;
            issue.Quantity2 = sourceInventoryIssue.Quantity2;
            return issue;
        }

        private ItemLot getItemLotByLotNrLineNr(SourceInventoryIssue sourceInventoryIssue)
        {
            return targetDbContext.GetItemLotByLotNrLineNr(sourceInventoryIssue.LotNr,
                                                           sourceInventoryIssue.LineNr);
        }

        #endregion

        #region Inventory Receive

        private void importInventoryReceives()
        {
            ieaFactory.RaiseImportingEvent("Inventory Receives");
            foreach (var receive in extractor.SourceInventoryReceives.Select(getInventoryReceive))
            {
                targetDbContext.SaveInventoryReceive(receive);
                ReportProgress(ieaFactory.ForInventoryIssue(receive.DocumentNr));
                if (IsCancelled) break;
            }
        }

        private InventoryReceive getInventoryReceive(SourceInventoryReceive sourceInventoryReceive)
        {
            var receive = new InventoryReceive();
            receive.DocumentNr = sourceInventoryReceive.DocumentNr;
            receive.Date = sourceInventoryReceive.Date;
            receive.Issue = getInventoryIssueByDocNr(sourceInventoryReceive.IssueDocNr);
            receive.LineNr = sourceInventoryReceive.LineNr;
            receive.Item = extractor.loadItem(extractor.Items.SingleOrDefault(i => i.Code == sourceInventoryReceive.ItemCode));
            receive.Quantity1 = sourceInventoryReceive.Quantity1;
            receive.Packing = sourceInventoryReceive.Packing;
            receive.Quantity2 = sourceInventoryReceive.Quantity2;
            return receive;
        }

        private InventoryIssue getInventoryIssueByDocNr(string issueDocNr)
        {
            return targetDbContext.GetInventoryIssueByDocNr(issueDocNr);
        }

        #endregion

        #region Misc Inventory Issue

        private void importMiscInventoryIssues()
        {
            ieaFactory.RaiseImportingEvent("Misc. Inventory Issues");
            foreach (var daybook in readDaybooksByType(DaybookType.MiscInventoryIssue))
                loadMiscInventoryIssue(readMiscInventoryIssue(daybook), daybook);
        }

        private IEnumerable<SourceMiscInventoryIssue> readMiscInventoryIssue(Daybook daybook)
        {
            return extractor.SourceMiscInventoryIssues.Where(mi => mi.DaybookCode == daybook.Code);
        }

        private void loadMiscInventoryIssue(IEnumerable<SourceMiscInventoryIssue> sourceMiscInventoryIssues,
                                            Daybook daybook)
        {
            foreach (var sourceMiscInventoryIssue in sourceMiscInventoryIssues)
            {
                var issue = getMiscInventoryIssue(sourceMiscInventoryIssue, daybook);
                targetDbContext.SaveMiscMaterialIssue(issue);
                ReportProgress(ieaFactory.ForMiscInventoryIssue(issue.Daybook.Code, issue.DocumentNr));
                if (IsCancelled) break;
            }
        }

        private MiscMaterialIssue getMiscInventoryIssue(SourceMiscInventoryIssue sourceMiscInventoryIssue,
                                                        Daybook daybook)
        {
            var issue = new MiscMaterialIssue();
            issue.Daybook = daybook;
            issue.DocumentNr = sourceMiscInventoryIssue.DocumentNr;
            issue.Date = sourceMiscInventoryIssue.Date;
            issue.LineNr = sourceMiscInventoryIssue.LineNr;
            issue.Item = extractor.loadItem(extractor.Items.SingleOrDefault(i => i.Code == sourceMiscInventoryIssue.ItemCode));
            issue.Quantity1 = sourceMiscInventoryIssue.Quantity1;
            issue.Quantity2 = sourceMiscInventoryIssue.Quantity2;
            return issue;
        }

        #endregion

        #region Common

        private IEnumerable<Daybook> readDaybooksByType(DaybookType type)
        {
            return extractor.Daybooks.Where(d => d.Type == type);
        }

        private IEnumerable<SourceTransaction> readTransactionsByDaybook(Daybook daybook)
        {
            return extractor.SourceTransactions.Where(trans => trans.DaybookCode == daybook.Code);
        }

        private void fillTransactionHeader(SourceTransaction sourceTransaction,
                                           Daybook daybook, TransactionHeader t)
        {
            t.Daybook = daybook;
            t.DocumentNr = sourceTransaction.DocumentNr;
            t.Date = sourceTransaction.TransactionDate;
            t.Account = extractor.loadAccount(extractor.getAccount(sourceTransaction.AccountCode));
            t.BrokerId = 0;
            t.Amount = sourceTransaction.Amount;
            t.IsAdjusted = sourceTransaction.IsAdjusted;
            t.Notes = sourceTransaction.Notes;
        }

        private IEnumerable<SourceLineItem> getLineItems(SourceTransaction sourceTransaction)
        {
            return extractor.SourceLineItems.Where(l => l.DaybookCode == sourceTransaction.DaybookCode &&
                                        l.AccountCode == sourceTransaction.AccountCode &&
                                        l.DocumentNr == sourceTransaction.DocumentNr).ToList();
        }

        #endregion

        #endregion
    }
}