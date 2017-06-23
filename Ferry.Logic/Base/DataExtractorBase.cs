using System;
using System.Collections.Generic;
using System.Linq;
using Ferry.Logic.Model;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.DataAccess;
using ExtractingEventArgs = Ferry.Logic.Common.ExtractingEventArgs;
using ExtractingEventHandler = Ferry.Logic.Common.ExtractingEventHandler;

namespace Ferry.Logic.Base
{
    internal abstract class DataExtractorBase
    {
        #region Declarations

        #region Common

        public event ExtractingEventHandler ExtractingHandler;
        protected readonly Database sourceDatabase;
        private delegate void ExecuteCacheDelegate();
        protected IList<KeyValuePair<string[], DaybookType>> daybookCodes;

        #endregion

        #region Source Masters

        protected IList<ChartOfAccountMapper> ChartOfAccountsMapper;
        protected IList<SourceChartOfAccount> SourceChartOfAccounts;
        public IList<SourceAccount> SourceAccounts;
        protected IList<SourceDaybook> SourceDaybooks;
        protected IList<SourceItemCategory> SourceItemCategories;
        protected IList<SourceItemGroup> SourceItemGroups;
        protected IList<SourceItem> SourceItems;
        protected IList<RowColumn> RowColumns;

        #endregion

        #region Source Transactions

        public IList<SourceLineItem> SourceLineItems;
        public IList<SourceLineItemTerm> SourceLineItemTerms;
        public IList<SourceTransaction> SourceTransactions;
        public IList<SourceLineItem> SourceItemLots;
        public IList<SourceInventoryIssue> SourceInventoryIssues;
        public IList<SourceInventoryReceive> SourceInventoryReceives;
        public IList<SourceMiscInventoryIssue> SourceMiscInventoryIssues;

        #endregion

        #region Masters

        public IList<ChartOfAccount> ChartOfAccounts;
        public IList<Daybook> Daybooks;
        public IList<Account> Accounts;
        public IList<Item> Items;
        public IList<ItemCategory> ItemCategories;
        public IList<ItemGroup> ItemGroups;

        #endregion

        #endregion

        #region Constructor

        protected DataExtractorBase(Database sourceDatabase)
        {
            this.sourceDatabase = sourceDatabase;
            initSourceDataLists();
        }

        private void initSourceDataLists()
        {
            ChartOfAccountsMapper = new List<ChartOfAccountMapper>();
            SourceChartOfAccounts = new List<SourceChartOfAccount>();
            SourceAccounts = new List<SourceAccount>();
            SourceDaybooks = new List<SourceDaybook>();
            SourceItemCategories = new List<SourceItemCategory>();
            SourceItemGroups = new List<SourceItemGroup>();
            SourceItems = new List<SourceItem>();
            RowColumns = new List<RowColumn>();

            SourceLineItems = new List<SourceLineItem>();
            SourceLineItemTerms = new List<SourceLineItemTerm>();
            SourceTransactions = new List<SourceTransaction>();
            SourceInventoryIssues = new List<SourceInventoryIssue>();
            SourceInventoryReceives = new List<SourceInventoryReceive>();
            SourceMiscInventoryIssues = new List<SourceMiscInventoryIssue>();
        }

        #endregion

        #region Public Methods

        public void Extract()
        {
            extractMasters();
            extractTransactions();
            transformMasters();
        }

        #endregion

        #region Private Methods

        private void extractMasters()
        {
            OnExtracting(new ExtractingEventArgs("Loading chart of accounts mapper"));
            executeCache(loadChartOfAccountsMapper);

            OnExtracting(new ExtractingEventArgs("Loading chart of accounts"));
            executeCache(LoadSourceChartOfAccounts);

            OnExtracting(new ExtractingEventArgs("Loading all accounts"));
            executeCache(LoadSourceAccounts);

            OnExtracting(new ExtractingEventArgs("Loading all daybooks"));
            executeCache(LoadSourceDaybooks);

            OnExtracting(new ExtractingEventArgs("Loading item categories"));
            executeCache(LoadSourceItemCategories);

            OnExtracting(new ExtractingEventArgs("Loading item groups"));
            executeCache(LoadSourceItemGroups);

            OnExtracting(new ExtractingEventArgs("Loading items"));
            executeCache(LoadSourceItems);

            OnExtracting(new ExtractingEventArgs("Loading row columns"));
            executeCache(LoadRowColumns);
        }

        private void extractTransactions()
        {
            OnExtracting(new ExtractingEventArgs("Loading line items"));
            executeCache(LoadSourceLineItems);

            OnExtracting(new ExtractingEventArgs("Loading line item terms"));
            executeCache(LoadSourceLineItemTerms);

            OnExtracting(new ExtractingEventArgs("Loading transactions"));
            executeCache(LoadSourceTransactions);

            OnExtracting(new ExtractingEventArgs("Loading item lots"));
            executeCache(LoadItemLots);

            OnExtracting(new ExtractingEventArgs("Loading inventory issue"));
            executeCache(LoadInventoryIssue);

            OnExtracting(new ExtractingEventArgs("Loading inventory receives"));
            executeCache(LoadInventoryReceives);

            OnExtracting(new ExtractingEventArgs("Loading misc inventory issue"));
            executeCache(LoadMiscInventoryIssues);
        }

        private void transformMasters()
        {
            OnExtracting(new ExtractingEventArgs("Transforming Masters"));

            executeCache(loadChartOfAccounts);
            executeCache(LoadAccounts);
            executeCache(LoadDaybooks);
            executeCache(LoadItemCategories);
            executeCache(LoadItemGroups);
            executeCache(LoadItems);
        }

        private void executeCache(ExecuteCacheDelegate executeCacheMethod)
        {
            executeCacheMethod();
        }

        #endregion

        #region Source Data Caching Abstract Methods

        #region Masters

        private void loadChartOfAccountsMapper()
        {
            ChartOfAccountsMapper = ForesightDatabaseFactory.GetInstance().GetChartOfAccountsMapper();
        }

        protected abstract void LoadSourceChartOfAccounts();
        protected abstract void LoadSourceAccounts();
        protected abstract void LoadSourceDaybooks();
        protected virtual void LoadSourceItemCategories() { }
        protected virtual void LoadSourceItemGroups() { }
        protected abstract void LoadSourceItems();
        protected virtual void LoadRowColumns() { }

        #endregion

        #region Transactions

        protected abstract void LoadSourceLineItems();
        protected virtual void LoadSourceLineItemTerms() { }
        protected abstract void LoadSourceTransactions();
        protected virtual void LoadItemLots() { }
        protected virtual void LoadInventoryIssue() { }
        protected virtual void LoadInventoryReceives() { }
        protected virtual void LoadMiscInventoryIssues() { }

        #endregion

        #endregion

        #region Transforming Masters Abstract Methods

        private void loadChartOfAccounts()
        {
            ChartOfAccounts = Session.Dbc.GetChartOfAccounts();
        }

        protected abstract void LoadAccounts();
        protected abstract void LoadDaybooks();
        protected virtual void LoadItemCategories() { }
        protected virtual void LoadItemGroups() { }
        protected abstract void LoadItems();

        #endregion

        #region Transforming masters helping methods

        internal abstract Account loadAccount(Account account);
        internal abstract Account getAccount(string accountCode);
        internal abstract ChartOfAccount loadChartOfAccount(string glgCode);
        internal abstract Item loadItem(Item item);
        internal virtual Item getItem(string itemCode) { return null; }

        #endregion

        #region Common

        protected void transformDaybookType(SourceDaybook sourceDaybook)
        {
            foreach (var daybookCode in daybookCodes)
            {
                var codeLen = daybookCode.Key.FirstOrDefault().Length;

                if (isDaybook(daybookCode.Key, codeLen, sourceDaybook.Type) ||
                        isDaybook(daybookCode.Key, codeLen, sourceDaybook.Code))
                {
                    sourceDaybook.Type = ((int)daybookCode.Value).ToString();
                    return;
                }
            }

            if (!char.IsDigit(Convert.ToChar(sourceDaybook.Type)))
                sourceDaybook.Type = ((int)DaybookType.Unknown).ToString();
        }

        private bool isDaybook(IEnumerable<string> daybookKeys, int codeLen, string field)
        {
            return field.Length >= codeLen && daybookKeys.Contains(field.Substring(0, codeLen));
        }

        protected string getDummyName(string name)
        {
            return string.Format("{0} not found", name);
        }

        protected virtual void OnExtracting(ExtractingEventArgs e)
        {
            if (ExtractingHandler != null)
                ExtractingHandler(this, e);
        }

        #endregion
    }
}
