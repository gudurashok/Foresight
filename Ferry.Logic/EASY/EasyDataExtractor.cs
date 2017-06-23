using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Ferry.Logic.Base;
using Ferry.Logic.Model;
using Ferry.Logic.Sql;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.DataAccess;

namespace Ferry.Logic.EASY
{
    internal class EasyDataExtractor : DataExtractorBase
    {
        private readonly DataContext targetDbContext;

        #region Constructor

        public EasyDataExtractor(Database sourceDatabase, DataContext targetDbContext)
            : base(sourceDatabase)
        {
            this.targetDbContext = targetDbContext;
            cacheDaybookTypes();
        }

        private void cacheDaybookTypes()
        {
            daybookCodes = new List<KeyValuePair<string[], DaybookType>>();

            var daybookCode = new KeyValuePair<string[], DaybookType>(new[] { "S" }, DaybookType.Sale);
            daybookCodes.Add(daybookCode);
            daybookCode = new KeyValuePair<string[], DaybookType>(new[] { "P" }, DaybookType.Purchase);
            daybookCodes.Add(daybookCode);
            daybookCode = new KeyValuePair<string[], DaybookType>(new[] { "C" }, DaybookType.Cash);
            daybookCodes.Add(daybookCode);
            daybookCode = new KeyValuePair<string[], DaybookType>(new[] { "B" }, DaybookType.Bank);
            daybookCodes.Add(daybookCode);
            daybookCode = new KeyValuePair<string[], DaybookType>(new[] { "R" }, DaybookType.CreditNote);
            daybookCodes.Add(daybookCode);
            daybookCode = new KeyValuePair<string[], DaybookType>(new[] { "D" }, DaybookType.DebitNote);
            daybookCodes.Add(daybookCode);
            daybookCode = new KeyValuePair<string[], DaybookType>(new[] { "J" }, DaybookType.JournalVoucher);
            daybookCodes.Add(daybookCode);
            daybookCode = new KeyValuePair<string[], DaybookType>(new[] { "I" }, DaybookType.InventoryIssue);
            daybookCodes.Add(daybookCode);
            daybookCode = new KeyValuePair<string[], DaybookType>(new[] { "M" }, DaybookType.MiscInventoryIssue);
            daybookCodes.Add(daybookCode);
        }

        #endregion

        #region Overridden Methods

        #region Masters

        protected override void LoadSourceChartOfAccounts()
        {
            loadSourceChartOfAccounts(readSourceChartOfAccounts());
        }

        protected override void LoadSourceAccounts()
        {
            loadSourceAccounts(readSourceAccounts());
        }

        protected override void LoadSourceDaybooks()
        {
            loadSourceDaybooks(readSourceDaybooks());
        }

        protected override void LoadSourceItemCategories()
        {
            loadSourceItemCategories(readSourceItemCategories());
        }

        protected override void LoadSourceItemGroups()
        {
            loadSourceItemGroups(readSourceItemGroups());
        }

        protected override void LoadSourceItems()
        {
            loadSourceItems(readSourceItems());
        }

        #endregion

        #region Transactions

        protected override void LoadSourceLineItems()
        {
            loadLineItems(readLineItems());
        }

        protected override void LoadSourceLineItemTerms()
        {
            loadLineItemTerms(readLineItemTerms());
        }

        protected override void LoadSourceTransactions()
        {
            loadTransactions(readTransactions());
        }

        protected override void LoadItemLots()
        {
            loadItemLots();
        }

        protected override void LoadInventoryIssue()
        {
            loadInventoryIssues(readInventoryIssues());
        }

        protected override void LoadInventoryReceives()
        {
            loadInventoryReceives(readInventoryReceives());
        }

        protected override void LoadMiscInventoryIssues()
        {
            loadMiscInventoryIssues(readMiscInventoryIssues());
        }

        #endregion

        #region New Masters

        protected override void LoadAccounts()
        {
            Accounts = new List<Account>();
            loadAccounts(readAccountGroups(), false);
            loadAccounts(readAccounts(), true);
        }

        protected override void LoadDaybooks()
        {
            loadDaybooks();
        }

        protected override void LoadItemCategories()
        {
            loadItemCategories();
        }

        protected override void LoadItemGroups()
        {
            loadItemGroups();
        }

        protected override void LoadItems()
        {
            loadItems();
        }

        #endregion

        #endregion

        #region Source Chart Of Accounts

        private IDataReader readSourceChartOfAccounts()
        {
            return sourceDatabase.ExecuteReader(EasySqlQueries.SelectAllGeneralLedgerGroups);
        }

        private void loadSourceChartOfAccounts(IDataReader reader)
        {
            while (reader.Read())
                SourceChartOfAccounts.Add(readSourceChartOfAccount(reader));

            reader.Close();
        }

        private SourceChartOfAccount readSourceChartOfAccount(IDataReader reader)
        {
            var scoa = new SourceChartOfAccount();
            scoa.Nr = reader["GLG_CODE"].ToString();
            scoa.Name = reader["GLG_NAME"].ToString();
            return scoa;
        }

        #endregion

        #region Source Accounts

        private IDataReader readSourceAccounts()
        {
            return sourceDatabase.ExecuteReader(EasySqlQueries.SelectAllAccounts);
        }

        private void loadSourceAccounts(IDataReader reader)
        {
            while (reader.Read())
                SourceAccounts.Add(readSourceAccount(reader));

            reader.Close();
        }

        private SourceAccount readSourceAccount(IDataReader reader)
        {
            var account = new SourceAccount();
            account.GroupCode = reader["GRP_CODE"].ToString();
            account.Code = reader["ACC_CODE"].ToString();
            account.Name = reader["ACC_NAME"].ToString();

            var openBalanceType = Util.ConvertDbNullToString(reader["OPB_CR_DR"]);
            if (!string.IsNullOrWhiteSpace(openBalanceType) && openBalanceType == "D")
                account.OpeningBalance = Convert.ToDecimal(Util.ConvertDbNull(reader["OP_BAL"])) * -1;
            else
                account.OpeningBalance = Convert.ToDecimal(Util.ConvertDbNull(reader["OP_BAL"]));

            account.AddressLine1 = reader["OFF_ADDR1"].ToString();
            account.AddressLine2 = reader["OFF_ADDR2"].ToString();
            account.City = reader["CITY"].ToString();
            account.State = reader["STATE"].ToString();
            account.Pin = reader["PIN"].ToString();
            account.ChartOfAccountCode = reader["GLG_CODE"].ToString();
            account.IsActive = Convert.ToBoolean(reader["ISACTIVE"]);
            return account;
        }

        #endregion

        #region Source Daybooks

        private IDataReader readSourceDaybooks()
        {
            return sourceDatabase.ExecuteReader(EasySqlQueries.SelectAllDaybooks);
        }

        private void loadSourceDaybooks(IDataReader reader)
        {
            while (reader.Read())
                SourceDaybooks.Add(readSourceDaybook(reader));

            reader.Close();
        }

        private SourceDaybook readSourceDaybook(IDataReader reader)
        {
            var daybook = new SourceDaybook();

            daybook.Type = reader["DBK_TYPE"].ToString();
            daybook.Code = reader["DBK_CODE"].ToString();
            daybook.Name = reader["DBK_NAME"].ToString();

            if (daybook.Type != "J")
                daybook.AccountCode = reader["ACC_CODE"].ToString();

            return daybook;
        }

        #endregion

        #region Source Item Categories

        private IDataReader readSourceItemCategories()
        {
            return sourceDatabase.ExecuteReader(EasySqlQueries.SelectAllItemCategories);
        }

        private void loadSourceItemCategories(IDataReader reader)
        {
            while (reader.Read())
                SourceItemCategories.Add(readSourceItemCategory(reader));

            reader.Close();
        }

        private SourceItemCategory readSourceItemCategory(IDataReader reader)
        {
            var category = new SourceItemCategory();
            category.Code = reader["CATAGORY"].ToString();
            category.Name = reader["ICAT_DESC"].ToString();
            return category;
        }

        #endregion

        #region Source Item Groups

        private IDataReader readSourceItemGroups()
        {
            return sourceDatabase.ExecuteReader(EasySqlQueries.SelectAllItemGroups);
        }

        private void loadSourceItemGroups(IDataReader reader)
        {
            while (reader.Read())
                SourceItemGroups.Add(readSourceItemGroup(reader));

            reader.Close();
        }

        private SourceItemGroup readSourceItemGroup(IDataReader reader)
        {
            var group = new SourceItemGroup();
            group.Code = reader["ITM_GRP"].ToString();
            group.Name = reader["GRP_NAME"].ToString();
            return group;
        }

        #endregion

        #region Source Items

        private IDataReader readSourceItems()
        {
            return sourceDatabase.ExecuteReader(EasySqlQueries.SelectAllItems);
        }

        private void loadSourceItems(IDataReader reader)
        {
            while (reader.Read())
                SourceItems.Add(readSourceItem(reader));

            reader.Close();
        }

        private SourceItem readSourceItem(IDataReader reader)
        {
            var item = new SourceItem();
            item.Code = reader["ITM_CODE"].ToString();
            item.CategoryCode = reader["CATAGORY"].ToString();
            item.GroupCode = reader["ITM_GRP"].ToString();
            item.Name = reader["ITM_NAME"].ToString();
            item.ShortName = reader["SHORT_NAME"].ToString();
            item.HasBom = Convert.ToBoolean(reader["BOM"]);
            item.IsActive = Convert.ToBoolean(reader["ISACTIVE"]);
            return item;
        }

        #endregion

        #region Line Items

        private IDataReader readLineItems()
        {
            return sourceDatabase.ExecuteReader(EasySqlQueries.SelectAllInvoiceLines);
        }

        private void loadLineItems(IDataReader reader)
        {
            while (reader.Read())
                SourceLineItems.Add(readSourceLineItem(reader));

            reader.Close();
        }

        private SourceLineItem readSourceLineItem(IDataReader reader)
        {
            var lineItem = new SourceLineItem();
            lineItem.LineNr = Convert.ToInt32(reader["SRNO"]);
            lineItem.ItemCode = reader["ITM_CODE"].ToString();
            lineItem.ItemName = reader["ITM_NAME"].ToString();
            lineItem.DaybookCode = reader["BRK_CODE"].ToString();
            lineItem.AccountCode = reader["ACC_CODE"].ToString();
            lineItem.DaybookCode = reader["BRK_CODE"].ToString();
            lineItem.DocumentNr = reader["DOC_NO"].ToString();
            lineItem.Date = Convert.ToDateTime(reader["DATE"]);
            lineItem.Quantity1 = Convert.ToDouble(Util.ConvertDbNull(reader["PCS"]));
            lineItem.Quantity2 = Convert.ToDouble(Util.ConvertDbNull(reader["QTY"]));
            lineItem.Packing = Convert.ToDouble(Util.ConvertDbNull(reader["BHARTI"]));
            lineItem.Price = Convert.ToDecimal(Util.ConvertDbNull(reader["RATE"]));
            lineItem.LineItemAmount = Convert.ToDecimal(Util.ConvertDbNull(reader["AMOUNT"]));
            lineItem.DiscountPct = Convert.ToDouble(Util.ConvertDbNull(reader["DISCOUNT"]));
            return lineItem;
        }

        #endregion

        #region Line Item Terms

        private IDataReader readLineItemTerms()
        {
            return sourceDatabase.ExecuteReader(EasySqlQueries.SelectAllInvoiceTerms);
        }

        private void loadLineItemTerms(IDataReader reader)
        {
            while (reader.Read())
                SourceLineItemTerms.Add(readSourceLineItemTerm(reader));

            reader.Close();
        }

        private static SourceLineItemTerm readSourceLineItemTerm(IDataReader reader)
        {
            var itemTerm = new SourceLineItemTerm();
            itemTerm.DaybookCode = reader["DBK_CODE"].ToString().Substring(0, 4);
            itemTerm.DocumentNr = reader["DOC_NO"].ToString();
            itemTerm.TermId = Convert.ToInt32(Util.ConvertDbNull(reader["CODE"]));
            itemTerm.Description = reader["DESCRIPT"].ToString();
            itemTerm.Percentage = Convert.ToDouble(Util.ConvertDbNull(reader["PERCENTAGE"]));
            itemTerm.Amount = Convert.ToDecimal(Util.ConvertDbNull(reader["AMOUNT"]));
            itemTerm.AccountCode = reader["ACC_CODE"].ToString();
            return itemTerm;
        }

        #endregion

        #region Source Transactions

        private IDataReader readTransactions()
        {
            return sourceDatabase.ExecuteReader(EasySqlQueries.SelectAllTransactions);
        }

        private void loadTransactions(IDataReader reader)
        {
            while (reader.Read())
                SourceTransactions.Add(readSourceTransaction(reader));

            reader.Close();
        }

        private static SourceTransaction readSourceTransaction(IDataReader reader)
        {
            var transaction = new SourceTransaction();

            var docNr = Util.ConvertDbNullToString(reader["DOC_NO"]);
            transaction.DocumentNr = docNr;
            transaction.DaybookCode = docNr.Length >= 4 ? docNr.Substring(0, 4) : docNr;
            transaction.TransactionType = Util.ConvertDbNullToString(reader["DB_CR"]);
            transaction.AccountCode = reader["ACC_CODE"].ToString();
            transaction.TransactionDate = Convert.ToDateTime(Util.ConvertDbNull(reader["DATE"]));
            transaction.Amount = Convert.ToDecimal(Util.ConvertDbNull(reader["NET_AMT"]));
            //transaction.BrokerageAmount = Convert.ToDecimal(Util.ConvertDbNull(reader["BRK_AMT"]));
            transaction.ChequeNr = reader["CHEQUE"].ToString();
            transaction.BankBranchName = reader["CHQ_FROM"].ToString();
            transaction.ChequeDate = Util.ConvertDbNullToString(reader["PASSED"]);
            transaction.Notes = reader["PARTICULAR"].ToString();
            transaction.Through = reader["THROUGH"].ToString();
            transaction.Transport = reader["TRANSPORT"].ToString();
            transaction.ReferenceDocNr = reader["REF_DOC"].ToString();
            transaction.DiscountPct = Convert.ToDouble(Util.ConvertDbNull(reader["DISC"]));
            transaction.IsAdjusted = Convert.ToBoolean(Util.ConvertDbNull(reader["ADJUSTED"]));

            transaction.ShipToName = reader["SHP_NAME"].ToString();
            transaction.ShipToAddressLine1 = reader["SHP_ADDR1"].ToString();
            transaction.ShipToAddressLine2 = reader["SHP_ADDR2"].ToString();
            transaction.ShipToCity = reader["SHP_CITY"].ToString();

            return transaction;
        }

        #endregion

        #region Item Lots

        private void loadItemLots()
        {
            SourceItemLots = SourceLineItems.OrderBy(l => l.LineNr).ToList();
        }

        #endregion

        #region Inventory Issues

        private IDataReader readInventoryIssues()
        {
            return sourceDatabase.ExecuteReader(EasySqlQueries.SelectAllInventoryIssues);
        }

        private void loadInventoryIssues(IDataReader reader)
        {
            while (reader.Read())
                SourceInventoryIssues.Add(readInventoryIssue(reader));

            reader.Close();
        }

        private SourceInventoryIssue readInventoryIssue(IDataReader reader)
        {
            var invIssue = new SourceInventoryIssue();
            invIssue.DaybookCode = reader["DOC_NO"].ToString().Substring(0, 4);
            invIssue.DocumentNr = reader["DOC_NO"].ToString();
            invIssue.Date = Convert.ToDateTime(reader["DATE"]);
            invIssue.LineNr = Convert.ToInt32(Util.ConvertDbNull(reader["DTL_LNO"]));
            invIssue.LotNr = reader["REF_DOC"].ToString();
            invIssue.AccountCode = reader["ACC_CODE"].ToString();
            invIssue.Quantity1 = Convert.ToDouble(Util.ConvertDbNull(reader["IPCS"]));
            invIssue.Quantity2 = Convert.ToDouble(Util.ConvertDbNull(reader["IQTY"]));

            return invIssue;
        }

        #endregion

        #region Inventory Receives

        private IDataReader readInventoryReceives()
        {
            return sourceDatabase.ExecuteReader(EasySqlQueries.SelectAllInventoryReceives);
        }

        private void loadInventoryReceives(IDataReader reader)
        {
            while (reader.Read())
                SourceInventoryReceives.Add(readInventoryReceive(reader));

            reader.Close();
        }

        private SourceInventoryReceive readInventoryReceive(IDataReader reader)
        {
            var invReceive = new SourceInventoryReceive();
            invReceive.DocumentNr = reader["DOC_NO"].ToString();
            invReceive.Date = Convert.ToDateTime(reader["DATE"]);
            invReceive.IssueDocNr = "IP01" + invReceive.DocumentNr.Substring(4);
            invReceive.LineNr = Convert.ToInt32(Util.ConvertDbNull(reader["SRNO"]));
            invReceive.ItemCode = reader["ITM_CODE"].ToString();
            invReceive.Quantity1 = Convert.ToDouble(Util.ConvertDbNull(reader["PCS"]));
            invReceive.Quantity2 = Convert.ToDouble(Util.ConvertDbNull(reader["QTY"]));
            invReceive.Packing = Convert.ToDouble(Util.ConvertDbNull(reader["BHARTI"]));
            return invReceive;
        }

        #endregion

        #region Misc Inventory Issues

        private IDataReader readMiscInventoryIssues()
        {
            return sourceDatabase.ExecuteReader(EasySqlQueries.SelectAllMiscInventoryIssues);
        }

        private void loadMiscInventoryIssues(IDataReader reader)
        {
            while (reader.Read())
                SourceMiscInventoryIssues.Add(readMiscInventoryIssue(reader));

            reader.Close();
        }

        private SourceMiscInventoryIssue readMiscInventoryIssue(IDataReader reader)
        {
            var miscIssue = new SourceMiscInventoryIssue();
            miscIssue.DaybookCode = reader["DOC_NO"].ToString().Substring(0, 4);
            miscIssue.DocumentNr = reader["DOC_NO"].ToString();
            miscIssue.Date = Convert.ToDateTime(reader["DATE"]);
            miscIssue.LineNr = Convert.ToInt32(Util.ConvertDbNull(reader["SRNO"]));
            miscIssue.ItemCode = reader["ITM_CODE"].ToString();
            miscIssue.Quantity1 = Convert.ToDouble(Util.ConvertDbNull(reader["PCS"]));
            miscIssue.Quantity2 = Convert.ToDouble(Util.ConvertDbNull(reader["QTY"]));
            return miscIssue;
        }

        #endregion

        #region Account

        private IEnumerable<SourceAccount> readAccounts()
        {
            return SourceAccounts.Where(
                        a => string.IsNullOrWhiteSpace(a.GroupCode) &&
                        a.Code != a.GroupCode);
        }

        private IEnumerable<SourceAccount> readAccountGroups()
        {
            return SourceAccounts.Where(a => a.Code == a.GroupCode);
        }

        private void loadAccounts(IEnumerable<SourceAccount> sourceAccounts, bool checkGroup)
        {
            foreach (var sourceAccount in sourceAccounts)
                Accounts.Add(readAccount(sourceAccount, checkGroup));
        }

        private Account readAccount(SourceAccount sourceAccount, bool checkGroup)
        {
            var account = new Account();

            if (checkGroup && !string.IsNullOrWhiteSpace(sourceAccount.GroupCode))
                account.Group = loadAccount(getAccount(sourceAccount.GroupCode));

            account.Code = sourceAccount.Code;
            account.Name = sourceAccount.Name;
            account.AddressLine1 = sourceAccount.AddressLine1;
            account.AddressLine2 = sourceAccount.AddressLine2;
            account.City = sourceAccount.City;
            account.State = sourceAccount.State;
            account.Pin = sourceAccount.Pin;
            account.ChartOfAccount = loadChartOfAccount(getChartOfAccount(sourceAccount.ChartOfAccountCode));
            account.IsActive = sourceAccount.IsActive;
            return account;
        }

        private Account readAccountByCode(string accountCode)
        {
            return Accounts.SingleOrDefault(a => a.Code == accountCode);
        }

        private string getChartOfAccount(string glgCode)
        {
            if (glgCode == EasySqlQueries.FactoryAccountGlgCode)
                return EasySqlQueries.PlantAndMachineryGlgCode;

            return glgCode;
        }

        #endregion

        #region Daybooks

        private void loadDaybooks()
        {
            Daybooks = new List<Daybook>();
            foreach (var sourceDaybook in SourceDaybooks)
                Daybooks.Add(loadDaybook(readDaybook(sourceDaybook)));
        }

        private Daybook readDaybook(SourceDaybook sourceDaybook)
        {
            var book = new Daybook();
            transformDaybookType(sourceDaybook);
            book.Type = (DaybookType)Convert.ToInt32(sourceDaybook.Type);
            book.Code = sourceDaybook.Code;
            book.Name = sourceDaybook.Name;

            if (book.Type != DaybookType.JournalVoucher)
                book.Account = loadAccount(getAccount(sourceDaybook.AccountCode));

            return book;
        }

        private Daybook loadDaybook(Daybook book)
        {
            var result = targetDbContext.GetDaybookByCode(book.Code);
            if (result == null)
            {
                targetDbContext.SaveDaybook(book);
                return book;
            }

            return result;
        }

        #endregion

        #region Item Category

        private void loadItemCategories()
        {
            ItemCategories = new List<ItemCategory>();
            foreach (var sourceItemCategory in SourceItemCategories)
                ItemCategories.Add(readItemCategory(sourceItemCategory));
        }

        private ItemCategory readItemCategory(SourceItemCategory sourceItemCategory)
        {
            var result = new ItemCategory();
            result.Code = sourceItemCategory.Code;
            result.Name = sourceItemCategory.Name;
            return result;
        }

        private ItemCategory loadItemCategory(ItemCategory ic)
        {
            var result = targetDbContext.GetItemCategoryByName(ic.Name);
            if (result == null)
            {
                targetDbContext.SaveItemCategory(ic);
                return ic;
            }

            return result;
        }

        #endregion

        #region Item Group

        private void loadItemGroups()
        {
            ItemGroups = new List<ItemGroup>();
            foreach (var sourceItemGroup in SourceItemGroups)
                ItemGroups.Add(readItemGroup(sourceItemGroup));
        }

        private ItemGroup readItemGroup(SourceItemGroup sourceItemGroup)
        {
            var result = new ItemGroup();
            result.Code = sourceItemGroup.Code;
            result.Name = sourceItemGroup.Name;
            return result;
        }

        private ItemGroup loadItemGroup(ItemGroup ig)
        {
            var result = targetDbContext.GetItemGroupByName(ig.Name);
            if (result == null)
            {
                targetDbContext.SaveItemGroup(ig);
                return ig;
            }

            return result;
        }

        #endregion

        #region Item

        private void loadItems()
        {
            Items = new List<Item>();
            foreach (var sourceItem in SourceItems)
                Items.Add(readItem(sourceItem));
        }

        private Item readItem(SourceItem sourceItem)
        {
            var item = new Item();
            item.Code = sourceItem.Code;
            item.Category = loadItemCategory(ItemCategories.SingleOrDefault(c => c.Code == sourceItem.CategoryCode));
            item.Group = loadItemGroup(ItemGroups.SingleOrDefault(g => g.Code == sourceItem.GroupCode));
            item.Name = sourceItem.Name;
            item.ShortName = sourceItem.ShortName;
            item.HasBom = sourceItem.HasBom;
            item.IsActive = sourceItem.IsActive;
            return item;
        }

        #endregion

        #region Common

        internal override Account loadAccount(Account account)
        {
            if (account.Id != 0)
                return account;

            var result = targetDbContext.GetAccountByNameAndAddress(account);
            if (result == null)
            {
                targetDbContext.SaveAccount(account);
                return account;
            }

            return result;
        }

        internal override Account getAccount(string accountCode)
        {
            var account = Accounts.SingleOrDefault(a => a.Code == accountCode) ?? createDummyAccount(accountCode);
            return account;
        }

        private Account createDummyAccount(string accountCode)
        {
            return new Account
            {
                Code = accountCode,
                Name = getDummyName(accountCode),
                ChartOfAccount = loadChartOfAccount("99")
            };
        }

        internal override ChartOfAccount loadChartOfAccount(string glgCode)
        {
            var coaId = ChartOfAccountsMapper.Where(c => c.EasyCode == glgCode)
                            .Select(c => c.ChartOfAccountId).SingleOrDefault();

            var coa = ChartOfAccounts
                            .Where(c => c.Id == coaId ||
                                    c.Name == getDummyName(glgCode))
                            .SingleOrDefault();
            return coa ?? createChartOfAccount(glgCode);
        }

        private ChartOfAccount createChartOfAccount(string glgCode)
        {
            ChartOfAccount parent = null;

            if (glgCode.Length == 5)
                parent = loadChartOfAccount(glgCode.Substring(0, 3));

            return insertChartOfAccount(glgCode, parent);
        }

        private ChartOfAccount insertChartOfAccount(string glgCode, ChartOfAccount parent)
        {
            var coa = new ChartOfAccount();
            coa.Name = getDummyName(glgCode);
            coa.Nr = ChartOfAccounts.Max(c => c.Nr) + 1;
            coa.Parent = parent;
            ChartOfAccounts.Add(coa);
            targetDbContext.SaveChartOfAccount(coa);
            return coa;
        }

        internal override Item loadItem(Item item)
        {
            if (item.Id != 0)
                return item;

            var result = targetDbContext.GetItemByName(item.Name);
            if (result == null)
            {
                targetDbContext.SaveItem(item);
                return item;
            }

            return result;
        }

        #endregion
    }
}
