namespace ScalableApps.Foresight.Logic.Sql
{
    internal static class SqlQueries
    {
        #region Common

        public const string ReadGeneratedIdentityValue =
                            "SELECT @@identity";

        #endregion

        #region Company Group

        public const string DropDatabase =
                            "DROP DATABASE [{0}]";

        public const string CreateDatabase =
                            "CREATE DATABASE [{0}]";

        public const string SelectCompanyGroupIdByName =
                            "SELECT Id FROM CompanyGroup" +
                            " WHERE Name = @name";

        public const string SelectCompanyGroupById =
                            "SELECT * FROM CompanyGroup" +
                            " WHERE Id = @Id";

        public const string SelectCountByCompanyGroupName =
                            "SELECT COUNT(*) FROM CompanyGroup " +
                            " WHERE Name = @name AND Id <> @id";

        public const string InsertCompanyGroup =
                            "INSERT INTO CompanyGroup (Id, [Name]) " +
                            "VALUES (@Id, @Name)";

        public const string UpdateCompanyGroup =
                            "UPDATE CompanyGroup SET [Name]=@Name " +
                            " WHERE Id = @Id";

        #endregion

        #region Company

        public const string SelectCompanyByName =
                            "SELECT COUNT(*) FROM Company" +
                            " WHERE [Name] = @Name";

        public const string SelectCompanyById =
                            "SELECT * FROM Company" +
                            " WHERE Id = @Id";

        public const string SelectCompaniesByGroupId =
                            "SELECT * FROM Company" +
                            " WHERE GroupId = @GroupId";

        public const string InsertCompany =
                            "INSERT INTO Company (Code, [Name], GroupId) " +
                            "VALUES (@Code, @Name, @GroupId)";

        public const string DeleteCompany =
                            "DELETE Company WHERE Id = @Id";

        #endregion

        #region Company Period

        public const string InsertCompanyPeriod =
                            "INSERT INTO CompanyPeriod (" +
                            " CompanyId, PeriodId, DataPath, SourceDataProvider) " +
                            "VALUES (@CompanyId, @PeriodId, @DataPath, @SourceDataProvider)";

        public const string UpdateCompanyPeriod =
                            "UPDATE CompanyPeriod " +
                            " SET CompanyId=@NewCompanyId, PeriodId=@NewPeriodId " +
                            " WHERE CompanyId=@CompanyId AND PeriodId=@PeriodId";

        public const string UpdateCompanyPeriodSetIsImportedFlagValue =
                            "UPDATE CompanyPeriod SET IsImported = @isImported " +
                            "WHERE CompanyId=@companyId AND PeriodId=@periodId";

        public const string UpdateCompanyPeriodIsImporting =
                            "UPDATE CompanyPeriod SET" +
                            " IsImporting = @isImporting, ProcessId = @processId" +
                            " WHERE CompanyId=@companyId AND PeriodId=@periodId";

        public const string ClearUnfinishedCompanyPeriodImports =
                            "UPDATE CompanyPeriod SET IsImporting = 0, ProcessId = 0" +
                            " WHERE ";

        public const string SelectCountOfCompanyPeriod =
                            "SELECT COUNT(*) FROM CompanyPeriod " +
                            "WHERE CompanyId = @CompanyId AND PeriodId = @PeriodId";

        public const string SelectCompanyPeriodIDByNameAndFinPeriod =
                            "SELECT cp.Id FROM CompanyPeriod cp" +
                            " INNER JOIN Company c ON c.Id = cp.CompanyId " +
                            " INNER JOIN DatePeriod dp ON dp.Id = cp.PeriodId " +
                            " WHERE c.Name = @companyName AND " +
                            " dp.FinancialFrom = @FinancialFrom AND " +
                            " dp.FinancialTo = @FinancialTo";

        public const string SelectCompanyPeriodByCompanyAndPeriodId =
                            "SELECT Id FROM CompanyPeriod " +
                            " WHERE CompanyId = @CompanyId AND PeriodId = @PeriodId";

        public const string SelectCompanyPeriodsBy =
                            "SELECT cp.Id, cp.CompanyId, cp.PeriodId, dp.Name, " +
                            "cp.DataPath, cp.SourceDataProvider, " +
                            "dp.FinancialFrom, dp.FinancialTo, dp.AssesmentFrom, " +
                            "dp.AssesmentTo, cp.IsImported FROM CompanyPeriod cp " +
                            " INNER JOIN DatePeriod dp ON dp.Id = cp.PeriodId " +
                            " {0}" +
                            " ORDER BY cp.PeriodId";

        public const string SelectCompanyCountFromCompanyPeriod =
                            "SELECT COUNT(*) FROM CompanyPeriod " +
                            "WHERE CompanyId = @CompanyId";

        public const string SelectCompanyIsImported =
                            "SELECT COUNT(*) FROM CompanyPeriod " +
                            "WHERE CompanyId = @CompanyId " +
                            "AND PeriodId = @PeriodId " +
                            "AND IsImported = 1";

        public const string SelectCompanyIsImporting =
                            "SELECT COUNT(*) FROM CompanyPeriod " +
                            "WHERE CompanyId = @CompanyId " +
                            "AND PeriodId = @PeriodId " +
                            "AND IsImporting = 1";

        public const string SelectDatePeriodByFinPeriod =
                            "SELECT * FROM [DatePeriod] " +
                            " WHERE FinancialFrom=@FinFrom " +
                            " AND FinancialTo=@FinTo ";

        public const string InsertDatePeriod =
                            "INSERT INTO [DatePeriod] " +
                            "(Name, FinancialFrom, FinancialTo, " +
                            "AssesmentFrom, AssesmentTo) " +
                            " VALUES (@PeriodName, @FinFrom, @FinTo, " +
                            "@AssesmentFrom, @AssesmentTo)";

        public const string SelectDatePeriodById =
                            "SELECT * FROM DatePeriod WHERE Id = @Id";

        public const string SelectDatePeriods =
                            "SELECT * FROM [DatePeriod]";

        public const string DeleteCompanyPeriod =
                            "DELETE FROM [CompanyPeriod] " +
                            "WHERE Id = @Id";

        public const string SelectTransTables =
                            "SELECT * FROM [TransTable]";

        public const string DeleteTransTablePeriodData =
                            "DELETE FROM {0} " +
                            "WHERE CompanyId = @CompanyId AND PeriodId = @PeriodId";

        public const string UpdateTransTablesCompanyPeriod =
                            "UPDATE {0} SET CompanyPeriodId=@CompanyPeriodId, " +
                            "CompanyId=@CompanyId, PeriodId=@PeriodId " +
                            "WHERE CompanyId IS NULL AND PeriodId IS NULL";

        public const string ChangeTransTablesCompanyPeriod =
                            "UPDATE {0} SET CompanyId=@NewCompanyId, PeriodId=@NewPeriodId " +
                            "WHERE CompanyId=@CompanyId AND PeriodId=@PeriodId ";

        #endregion

        #region Business

        public const string SelectAllAccountBalances =
                            "SELECT a.{0} AccountId, SUM(Opening) " +
                            " + SUM(Purchase) + SUM(CashReceipt) + SUM(BankReceipt) + SUM(CreditNote) + SUM(CreditJV) " +
                            " - SUM(Sale) - SUM(CashPayment) - SUM(BankPayment) - SUM(DebitNote) - SUM(DebitJV) Amount " +
                            " FROM AccountLedger al " +
                            " INNER JOIN Account a ON a.Id = al.AccountId " +
                            " GROUP BY a.{0} ";

        public const string SelectAllAccounts =
                            "SELECT * FROM Account a" +
                            " INNER JOIN ChartOfAccount ca ON ca.Id = a.ChartOfAccountId " +
                            " {0}";

        public const string SelectAccountById =
                            "SELECT * FROM Account WHERE id = @id";

        public const string SelectAccountByNameAndAddress =
                            "SELECT * FROM Account " +
                            "WHERE name = @name AND " +
                            "(addressLine1 = @addressLine1 OR addressLine1 IS NULL) AND " +
                            "(addressLine2 = @addressLine2 OR addressLine2 IS NULL) AND " +
                            "(city = @city OR city IS NULL) AND " +
                            "(state = @state OR state IS NULL) AND " +
                            "(pin = @pin OR pin IS NULL) ";

        public const string InsertAccount =
                            "INSERT INTO Account (" +
                            " groupId, chartOfAccountId, name, addressLine1, addressLine2," +
                            " city, state, pin, contactId, isActive) " +
                            "VALUES (@groupId, @chartOfAccountId, @name, @addressLine1, @addressLine2," +
                            " @city, @state, @pin, @contactId, @isActive)";

        public const string InsertAccountOpeningBalance =
                            "INSERT INTO OpeningBalance (" +
                            "AccountId, [Date], Amount) " +
                            "VALUES (@accountId, @date, @amount)";

        public const string UpdateAccountGroupId =
                            "UPDATE Account SET GroupId = @Id" +
                            " WHERE Id = @Id";

        public const string SelectAllChartOfAccounts =
                            "SELECT * FROM ChartOfAccount";

        public const string SelectChartOfAccountByNr =
                            "SELECT * FROM ChartOfAccount WHERE Nr = @nr";

        public const string SelectChartOfAccountById =
                            "SELECT * FROM ChartOfAccount WHERE Id = @Id";

        public const string SelectChartOfAccountByParentId =
                            "SELECT * FROM ChartOfAccount WHERE Id = @parentId";

        public const string SelectChartOfAccountIdsByParentId = 
                            "SELECT DISTINCT Id FROM ChartOfAccount " +
                            " WHERE ParentId = @parentId";

        public const string InsertChartOfAccount =
                            "INSERT INTO ChartOfAccount (" +
                            "Nr, parentId, name) " +
                            "VALUES (@nr, @parentId, @name)";

        public const string InsertSaleInvoiceHeader =
                            "INSERT INTO SaleInvoiceHeader (" +
                            " daybookId, documentNr, date, accountId, brokerId, brokerageAmount," +
                            " through, vehicleId, transport, referenceDocNr, orderId, discountPct," +
                            " amount, isAdjusted, saleTaxColumnId, notes) " +
                            "VALUES (@daybookId, @documentNr, @date, @accountId, @brokerId, @brokerageAmount," +
                            " @through, @vehicleId, @transport, @referenceDocNr, @orderId, @discountPct," +
                            " @amount, @isAdjusted, @saleTaxColumnId, @notes)";

        public const string InsertSaleInvoiceHeaderEx =
                            "INSERT INTO SaleInvoiceHeaderEx (" +
                            " InvoiceId, ShipToName, ShipToAddressLine1, ShipToAddressLine2, ShipToCity)" +
                            "VALUES (@InvoiceId, @ShipToName, @ShipToAddressLine1, @ShipToAddressLine2, @ShipToCity)";

        public const string InsertSaleInvoiceLine =
                            "INSERT INTO SaleInvoiceLine (" +
                            " InvoiceId, LineNr, ItemId, ItemDescription," +
                            " Quantity1, Packing, Quantity2, Quantity3, Price, DiscountPct, Amount)" +
                            "VALUES (@InvoiceId, @LineNr, @ItemId, @ItemDescription," +
                            " @Quantity1, @Packing, @Quantity2, @Quantity3, @Price, @DiscountPct, @Amount)";

        public const string InsertSaleInvoiceTerm =
                            "INSERT INTO SaleInvoiceTerm (" +
                            " InvoiceId, TermId, Description, Percentage, Amount, AccountId)" +
                            "VALUES (@InvoiceId, @TermId, @Description, @Percentage, @Amount, @AccountId)";

        public const string InsertPurchaseInvoiceHeader =
                            "INSERT INTO PurchaseInvoiceHeader (" +
                            " daybookId, documentNr, date, accountId, brokerId, brokerageAmount," +
                            " through, transport, referenceDocNr, orderId, discountPct," +
                            " amount, isAdjusted, saleTaxColumnId, notes) " +
                            "VALUES (@daybookId, @documentNr, @date, @accountId, @brokerId, @brokerageAmount," +
                            " @through, @transport, @referenceDocNr, @orderId, @discountPct," +
                            " @amount, @isAdjusted, @saleTaxColumnId, @notes)";

        public const string InsertPurchaseInvoiceLine =
                            "INSERT INTO PurchaseInvoiceLine (" +
                            " InvoiceId, LineNr, ItemId, ItemDescription," +
                            " Quantity1, Packing, Quantity2, Quantity3, Cost, DiscountPct, Amount)" +
                            "VALUES (@InvoiceId, @LineNr, @ItemId, @ItemDescription," +
                            " @Quantity1, @Packing, @Quantity2, @Quantity3, @Cost, @DiscountPct, @Amount)";

        public const string InsertPurchaseInvoiceTerm =
                            "INSERT INTO PurchaseInvoiceTerm (" +
                            " InvoiceId, TermId, Description, Percentage, Amount, AccountId)" +
                            "VALUES (@InvoiceId, @TermId, @Description, @Percentage, @Amount, @AccountId)";

        public const string InsertJournalVoucher =
                            "INSERT INTO JournalVoucher (" +
                            " daybookId, documentNr, accountId, date," +
                            " amount, txnType, isAdjusted, notes) " +
                            "VALUES (@daybookId, @documentNr, @accountId, @date," +
                            " @amount, @txnType, @isAdjusted, @notes)";

        public const string InsertCashTransaction =
                            "INSERT INTO CashTransaction (" +
                            " daybookId, documentNr, date, accountId, brokerId," +
                            " amount, txnType, isAdjusted, notes) " +
                            "VALUES (@daybookId, @documentNr, @date, @accountId, @brokerId," +
                            " @amount, @txnType, @isAdjusted, @notes)";

        public const string InsertBankPayment =
                            "INSERT INTO BankPayment (" +
                            " daybookId, documentNr, date, accountId, brokerId," +
                            " chequeNr, amount, isAdjusted, notes, isRealised) " +
                            "VALUES (@daybookId, @documentNr, @date, @accountId, @brokerId," +
                            " @chequeNr, @amount, @isAdjusted, @notes, @isRealised)";

        public const string InsertBankReceipt =
                            "INSERT INTO BankReceipt (" +
                            " daybookId, documentNr, date, accountId, brokerId," +
                            " chequeNr, bankBranchName, amount, isAdjusted, notes, isRealised) " +
                            "VALUES (@daybookId, @documentNr, @date, @accountId, @brokerId," +
                            " @chequeNr, @bankBranchName, @amount, @isAdjusted, @notes, @isRealised)";

        public const string InsertCreditNoteHeader =
                            "INSERT INTO CreditNoteHeader (" +
                            " daybookId, documentNr, date, accountId, brokerId," +
                            " amount, isAdjusted, notes) " +
                            "VALUES (@daybookId, @documentNr, @date, @accountId, @brokerId," +
                            " @amount, @isAdjusted, @notes)";

        public const string InsertCreditNoteLine =
                            "INSERT INTO CreditNoteLine (" +
                            " NoteId, LineNr, ItemId, ItemDescription," +
                            " Quantity1, Packing, Quantity2, Quantity3, Cost, DiscountPct, Amount)" +
                            "VALUES (@NoteId, @LineNr, @ItemId, @ItemDescription," +
                            " @Quantity1, @Packing, @Quantity2, @Quantity3, @Cost, @DiscountPct, @Amount)";

        public const string InsertDebitNoteHeader =
                            "INSERT INTO DebitNoteHeader (" +
                            " daybookId, documentNr, date, accountId, brokerId," +
                            " amount, isAdjusted, notes) " +
                            "VALUES (@daybookId, @documentNr, @date, @accountId, @brokerId," +
                            " @amount, @isAdjusted, @notes)";

        public const string InsertDebitNoteLine =
                            "INSERT INTO DebitNoteLine (" +
                            " NoteId, LineNr, ItemId, ItemDescription," +
                            " Quantity1, Packing, Quantity2, Quantity3, Price, DiscountPct, Amount)" +
                            "VALUES (@NoteId, @LineNr, @ItemId, @ItemDescription," +
                            " @Quantity1, @Packing, @Quantity2, @Quantity3, @Price, @DiscountPct, @Amount)";

        public const string InsertItemLot =
                            "INSERT INTO ItemLot (" +
                            " daybookId, LotNr, Date, AccountId, LineNr, ItemId, " +
                            " Quantity1, Packing, Quantity2, Quantity3)" +
                            "VALUES (@daybookId, @LotNr, @Date, @AccountId, @LineNr, @ItemId, " +
                            " @Quantity1, @Packing, @Quantity2, @Quantity3)";

        public const string SelectLotByLotNrAndLineNr =
                            "SELECT * FROM ItemLot " +
                            "WHERE LotNr=@lotNr AND LineNr=@lineNr";

        public const string InsertInventoryIssue =
                            "INSERT INTO InventoryIssue (" +
                            " daybookId, documentNr, Date, LotId, AccountId, " +
                            " Quantity1, Quantity2, Quantity3)" +
                            "VALUES (@daybookId, @documentNr, @Date, @LotId, @AccountId," +
                            " @Quantity1, @Quantity2, @Quantity3)";

        public const string InsertMiscInventoryIssue =
                            "INSERT INTO MiscMaterialIssue (" +
                            " daybookId, documentNr, Date, LineNr, ItemId, " +
                            " Quantity1, Quantity2, Quantity3)" +
                            "VALUES (@daybookId, @documentNr, @Date, @LineNr, @ItemId," +
                            " @Quantity1, @Quantity2, @Quantity3)";

        public const string InsertInventoryReceive =
                            "INSERT INTO InventoryReceive (" +
                            " daybookId, documentNr, Date, IssueId, LineNr, ItemId, " +
                            " Quantity1, Packing, Quantity2, Quantity3)" +
                            "VALUES (@daybookId, @documentNr, @Date, @IssueId, @LineNr, @ItemId," +
                            " @Quantity1, @Packing, @Quantity2, @Quantity3)";

        public const string SelectInventoryIssueByDocNr =
                            "SELECT * FROM InventoryIssue " +
                            "WHERE documentNr = @documentNr";

        public const string SelectItemCategoryById =
                            "SELECT * FROM ItemCategory WHERE id = @id";

        public const string SelectItemCategoryByName =
                            "SELECT * FROM ItemCategory WHERE name = @name";

        public const string SelectItemGroupById =
                            "SELECT * FROM ItemGroup WHERE id = @id";

        public const string SelectItemGroupByName =
                            "SELECT * FROM ItemGroup WHERE name = @name";

        public const string SelectItemById =
                            "SELECT * FROM Item WHERE id = @id";

        public const string SelectItemByName =
                            "SELECT * FROM Item WHERE name = @name";

        public const string InsertItem =
                            "INSERT INTO Item (" +
                            " groupId, name, shortName, categoryId, hasBom, isActive) " +
                            "VALUES (@groupId, @name, @shortName, @categoryId, @hasBom, @isActive)";

        public const string InsertItemGroup =
                            "INSERT INTO ItemGroup (" +
                            " parentId, name)" +
                            "VALUES (@parentId, @name)";

        public const string InsertItemCategory =
                            "INSERT INTO ItemCategory (" +
                            " name)" +
                            "VALUES (@name)";

        public const string SelectBillTermIdByDescription =
                            "SELECT id FROM BillTerm WHERE description = @description";

        public const string InsertBillTerm =
                            "INSERT INTO BillTerm (" +
                            " name)" +
                            "VALUES (@name)";

        public const string SelectAllDaybooks =
                            "SELECT * FROM Daybook ORDER BY Id";

        public const string SelectDaybookByCode =
                            "SELECT * FROM Daybook " +
                            "WHERE code = @code";

        public const string SelectDaybookIdOfAccount =
                            "SELECT Id FROM Daybook WHERE AccountId = @accountId";

        public const string SelectDaybookOfAccount =
                            "SELECT * FROM Daybook WHERE AccountId = @accountId";

        public const string InsertDaybook =
                            "INSERT INTO Daybook (Type, Code, Name, AccountId) " +
                            "VALUES (@type, @code, @name, @accountId)";

        #endregion

        #region Error Log

        public const string InsertError =
                            "INSERT INTO ErrorLog([DateTime], [Text]) " +
                            " VALUES(@DateTime, @Text) ";

        #endregion

        #region Foresight Db

        public const string SelectLoginPassword =
                            "SELECT [Password] FROM [Login] " +
                            " WHERE [Role] = @Role";

        public const string SelectAllCompanyGroups =
                            "SELECT * FROM CompanyGroup";

        public const string ForesightInsertCompanyGroup =
                            "INSERT INTO CompanyGroup ([Name], DatabaseName) " +
                            "VALUES (@Name, @DatabaseName)";

        public const string ForesightUpdateCompanyGroup =
                            "UPDATE CompanyGroup SET [Name] = @Name, " +
                            " DatabaseName = @DatabaseName " +
                            " WHERE Id = @Id";

        public const string ForesightDeleteCompanyGroup =
                            "DELETE CompanyGroup " +
                            " WHERE Id = @Id";

        public const string SelectAllCommands =
                            "SELECT * FROM Command ";

        public const string SelectCommandByNr =
                            "SELECT * FROM Command " +
                            " WHERE Nr = @nr";

        public const string SelectCommandPropsByCommandId =
                            "SELECT * FROM CommandProp" +
                            " WHERE CommandId = @commandId";

        public const string SelectChartOfAccountsMapper =
                            "SELECT * FROM ChartOfAccountMapper ";

        #endregion

        #region DbScript

        public const string SelectDbScript =
                            "SELECT Script FROM DbScript " +
                            " WHERE Name = 'Foresight'";

        #endregion

        #region Account Ledger

        public const string InsertAccountLedger =
                            "INSERT INTO AccountLedger " +
                            " (AccountId, ChartOfAccountId, [Month], " +
                            " Opening, Sale, CashPayment, BankPayment, DebitNote, DebitJV, " +
                            " Purchase, CashReceipt, BankReceipt, CreditNote, CreditJV, " +
                            " PeriodId, CompanyId, CompanyPeriodId) " +
                            " VALUES(@accountId, @chartOfAccountId, @month, " +
                            " @opening, @sale, @cashPayment, @bankPayment, @debitNote, @debitJV, " +
                            " @purchase, @cashReceipt, @bankReceipt, @creditNote, @creditJV, " +
                            " @periodId, @companyId, @companyPeriodId) ";

        public const string SelectMonthlyOpeningBalanceByAccount =
                            " SELECT AccountId, ca.Nr ChartOfAccountId, " +
                            " DATEPART(MONTH, [Date]) [Month], SUM(Amount) Amount " +
                            " FROM OpeningBalance " +
                            " INNER JOIN Account a ON a.Id = AccountId " +
                            " INNER JOIN ChartOfAccount ca ON ca.Id = a.ChartOfAccountId " +
                            " WHERE CompanyPeriodId = @companyPeriodId " +
                            " GROUP BY AccountId, ca.Nr, DATEPART(MONTH, [Date]) ";

        public const string SelectMonthlySumOfTransByAccount =
                            "SELECT AccountId, ChartOfAccountId, [Month], Amount " +
                            "FROM ( " +
                            "  SELECT t.AccountId AccountId, ca.Nr ChartOfAccountId, " +
                            "  DATEPART(MONTH, [Date]) [Month], SUM(Amount) Amount " +
                            "  FROM {0} t " +
                            "  INNER JOIN Account a ON a.Id = t.AccountId " +
                            "  INNER JOIN ChartOfAccount ca ON ca.Id = a.ChartOfAccountId " +
                            "  WHERE CompanyPeriodId = @companyPeriodId {1} " +
                            "  GROUP BY t.AccountId, ca.Nr, DATEPART(MONTH, [Date]) " +
                            " UNION ALL " +
                            "  SELECT d.AccountId AccountId, ca.Nr ChartOfAccountId, " +
                            "  DATEPART(MONTH, [Date]) [Month], SUM(Amount) Amount " +
                            "  FROM {0} t " +
                            "  INNER JOIN Daybook d ON d.Id = t.DaybookId " +
                            "  INNER JOIN Account a ON a.Id = d.AccountId " +
                            "  INNER JOIN ChartOfAccount ca ON ca.Id = a.ChartOfAccountId " +
                            "  WHERE CompanyPeriodId = @companyPeriodId {1} " +
                            "  GROUP BY d.AccountId, ca.Nr, DATEPART(MONTH, [Date]) " +
                            " ) Result " +
                            "ORDER BY AccountId, [Month] ";

        #endregion

        #region Item Ledger

        public const string InsertItemLedger =
                            "INSERT INTO ItemLedger " +
                            " (ItemId, AccountId, ChartOfAccountId, [Month], Sale, Purchase, " +
                            " PeriodId, CompanyId, CompanyPeriodId) " +
                            " VALUES(@itemId, @accountId, @chartOfAccountId, @month, @sale, @purchase, " +
                            " @periodId, @companyId, @companyPeriodId) ";

        public const string SelectMonthlySumOfTransByItemAndAccount =
                            "SELECT l.ItemId, h.AccountId, ca.Nr chartOfAccountId, " +
                            " DATEPART(MONTH, [Date]) [Month], SUM(l.Amount) Amount " +
                            " FROM {0} h " +
                            " INNER JOIN {1} l ON l.InvoiceId = h.Id " +
                            " INNER JOIN Account a ON a.Id = h.AccountId " +
                            " INNER JOIN ChartOfAccount ca ON ca.Id = a.ChartOfAccountId " +
                            " WHERE l.CompanyPeriodId = @companyPeriodId " +
                            " GROUP BY l.ItemId, h.AccountId, ca.Nr, DATEPART(MONTH, [Date]) ";

        #endregion
    }
}
