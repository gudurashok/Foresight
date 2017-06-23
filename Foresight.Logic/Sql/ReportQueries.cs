namespace ScalableApps.Foresight.Logic.Sql
{
    internal static class ReportQueries
    {
        public const string SelectTotalOfItems =
                            "SELECT SUM({0}) " +
                            " FROM ItemLedger " +
                            " {1} ";

        public const string SelectTopNItems =
                            "SELECT r.Id, Name, TotalAmount " +
                            " FROM ( " +
                            "   SELECT TOP {0} i.{1} Id, SUM({2}) TotalAmount " +
                            "   FROM ItemLedger il " +
                            "   INNER JOIN Item i ON i.Id = il.ItemId " +
                            "   {4} " +
                            "   GROUP BY i.{1} " +
                            "   ORDER BY TotalAmount DESC ) r " +
                            " INNER JOIN {3} im ON im.Id = r.Id ";

        public const string SelectTotalSaleValueByCompanyPeriodId =
                            "SELECT SUM(Sale)" +
                            " FROM AccountLedger" +
                            " WHERE ChartOfAccountId {0} AND " +
                            " CompanyPeriodId = @companyPeriodId ";

        public const string SelectTopNCustomers =
                            "SELECT ac.Id, ac.Name, t.TotalAmount " +
                            " FROM [Account] ac" +
                            " INNER JOIN (" +
                            "   SELECT TOP {0} a.{1} Id, SUM(Sale) AS TotalAmount" +
                            "   FROM AccountLedger al" +
                            "   INNER JOIN Account a ON a.Id = al.AccountId" +
                            "   WHERE al.ChartOfAccountId {2} {3} " +
                            "   GROUP BY a.{1}" +
                            "   ORDER BY TotalAmount DESC ) AS t " +
                            " ON t.Id = ac.Id";

        public const string SelectTotalOfSale =
                            "SELECT SUM(Sale) " +
                            " FROM AccountLedger al" +
                            " WHERE al.ChartOfAccountId {1} {0} ";

        public const string SelectTopNExpenses =
                            "SELECT r.Id, ac.Name, TotalAmount " +
                            " FROM ( " +
                            " 	SELECT TOP {0} a.{1} Id, SUM(CashPayment) + SUM(BankPayment) TotalAmount " +
                            " 	FROM AccountLedger al " +
                            " 	INNER JOIN Account a ON al.AccountId = a.Id " +
                            " 	WHERE al.ChartOfAccountId {3} {2} " +
                            "   GROUP BY a.{1}" +
                            "   ORDER BY TotalAmount DESC ) AS r " +
                            " INNER JOIN Account ac ON r.Id = ac.Id ";

        public const string SelectTotalOfExpense =
                            "SELECT SUM(CashPayment) + SUM(BankPayment) TotalAmount " +
                            " FROM AccountLedger al " +
                            " WHERE al.ChartOfAccountId {1} {0} ";

        public const string SelectBuyingTrend =
                            "SELECT PeriodId, Month, SUM(Amount) Amount " +
                            "FROM (" +
                            "  SELECT a.{0}, al.PeriodId, al.Month, SUM(al.Sale) Amount " +
                            "  FROM AccountLedger al " +
                            "  INNER JOIN Account a ON a.Id = al.AccountId " +
                            "  WHERE al.ChartOfAccountId {1} {2} " +
                            "  GROUP BY a.{0}, al.PeriodId, al.Month ) r " +
                            "GROUP BY PeriodId, Month ";

        public const string SelectTopNSalePartyAssociations =
                            "SELECT r.Id, acc.Name, FirstDate, LastDate, TotalAmount, TransCount " +
                            " FROM ( " +
                            "   SELECT TOP {0} a.{1} Id, " +
                            "    MIN(s.[Date]) FirstDate,  " +
                            "    MAX(s.[Date]) LastDate,  " +
                            "    SUM(s.Amount) TotalAmount, COUNT(*) TransCount " +
                            "    FROM SaleInvoiceHeader s " +
                            "    INNER JOIN Account a ON s.AccountId = a.Id " +
                            "    INNER JOIN ChartOfAccount ca ON ca.Id = a.ChartOfAccountId " +
                            "    WHERE s.[Date] {2} (CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, GETDATE()))) - @days) " +
                            "    AND ca.Nr {4} " +
                            "    AND a.Id NOT IN (" +
                            "      SELECT DISTINCT AccountId " +
                            "      FROM SaleInvoiceHeader sh " +
                            "      WHERE sh.[Date] {3} (CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, GETDATE()))) - @days) ) " +
                            "    GROUP BY a.{1}" +
                            "    ORDER BY TotalAmount DESC ) r " +
                            " INNER JOIN Account acc ON acc.Id = r.Id ";

        public const string SelectCountOfSalePartyAssociationsOfYear =
                            "SELECT COUNT(*) TCount, SUM(TotalAmount) TAmount " +
                            "FROM " +
                            "(SELECT a.{0}, SUM(Sale) TotalAmount " +
                            " FROM AccountLedger al " +
                            " INNER JOIN Account a ON al.AccountId = a.Id " +
                            " WHERE al.ChartOfAccountId {1} " + 
                            " AND PeriodId = @outPeriodId" +
                            " AND Sale <> 0 " +
                            " AND a.Id NOT IN " +
                            "   (SELECT DISTINCT AccountId " +
                            "   FROM AccountLedger " +
                            "   WHERE PeriodId = @inPeriodId " +
                            "   AND Sale <> 0 ) " +
                            " GROUP BY a.{0} ) AS t ";

        public const string SelectGenuineSaleRatio =
                            "SELECT AccountId, acc.Name, SaleAmount, ReceiptAmount " +
                            " FROM ( " +
                            "   SELECT a.{0} AccountId, SUM(STAmount) SaleAmount, SUM(RTAmount) ReceiptAmount " +
                            "   FROM ( " +
                            "      SELECT AccountId, SUM(Sale) STAmount, 0 RTAmount " +
                            "      FROM AccountLedger a GROUP BY AccountId HAVING SUM(Sale) > 0 " +
                            "    UNION ALL " +
                            "      SELECT AccountId, ABS(SUM(Amount)) STAmount, 0 RTAmount " +
                            "      FROM OpeningBalance " +
                            "      WHERE AccountId IN (SELECT DISTINCT AccountId FROM SaleInvoiceHeader) " +
                            "      GROUP BY AccountId HAVING SUM(Amount) < 0 " +
                            "    UNION ALL " +
                            "      SELECT AccountId, 0 STAmount, SUM(Amount) RTAmount " +
                            "      FROM ( " +
                            "        SELECT AccountId, SUM(CashReceipt) - SUM(CashPayment) " +
                            "                        + SUM(BankReceipt) - SUM(BankPayment) Amount " +
                            "        FROM AccountLedger GROUP BY AccountId " +
                            "       UNION ALL " +
                            "        SELECT AccountId, SUM(Amount) Amount " +
                            "        FROM OpeningBalance GROUP BY AccountId HAVING SUM(Amount) > 0 ) r " +
                            "      WHERE AccountId IN (SELECT DISTINCT AccountId FROM SaleInvoiceHeader) " +
                            "      GROUP BY AccountId HAVING SUM(Amount) <> 0 " +
                            "    ) AS t " +
                            "   INNER JOIN Account a ON a.Id = t.AccountId " +
                            "   INNER JOIN ChartOfAccount ca ON ca.Id = a.ChartOfAccountId " +
                            "   WHERE Nr {1} " +
                            "   GROUP BY a.{0} ) r " +
                            "INNER JOIN Account acc ON acc.Id = r.AccountId ";

        public const string SelectAllTrailBalances =
                            "SELECT ChartOfAccountName, Id AccountId, AccountName, " +
                            " Opening, CrAmt, DbAmt, (Opening + CrAmt - DbAmt) Closing " +
                            " FROM (SELECT coa.Name AS ChartOfAccountName, a.Id, a.Name AS AccountName, " +
                            " SUM(al.Opening) AS Opening, {0} AS CrAmt, {1} AS DbAmt " +
                            " FROM Account AS a LEFT OUTER JOIN " +
                            " AccountLedger AS al ON a.Id = al.AccountId LEFT OUTER JOIN " +
                            " ChartOfAccount AS coa ON coa.Id = a.ChartOfAccountId " +
                            " GROUP BY a.Id, a.Name, coa.Name) Result " +
                            " ORDER BY ChartOfAccountName ";

        public const string SelectAccountOpeningBalance =
                            "SELECT SUM(Amount) Amount" +
                            " FROM OpeningBalance " +
                            " WHERE AccountId = @accountId {0} ";

        public const string SelectYearlyLedger =
                            "SELECT PeriodId, SUM(Opening) OpAmount, {0} CrAmount, {1} DbAmount " +
                            " FROM AccountLedger " +
                            " WHERE AccountId = @accountId " +
                            " GROUP BY PeriodId " +
                            " ORDER BY PeriodId DESC ";

        public const string SelectMonthlyLedger =
                            "SELECT [Month], {0} CrAmount, {1} DbAmount " +
                            " FROM AccountLedger " +
                            " WHERE AccountId = @accountId AND PeriodId = @periodId " +
                            " GROUP BY [Month] ";

        public const string SelectDetailLedger =
                            " SELECT t.Id, c.Name CompanyName, a.Name Name, DocumentNr, [Date], {0} Amount, Notes " +
                            " FROM {1} t " +
                            " INNER JOIN {2} a ON a.Id = t.{2}Id " +
                            " INNER JOIN Company c on c.Id = t.CompanyId " +
                            " WHERE (t.{3}) AND PeriodId = @periodId {4} " +
                            " ORDER BY [Date], t.Id, DocumentNr ";

        public const string SelectJvDetailLedger =
                            " SELECT Id, CompanyName, Name, DocumentNr, [Date], Amount, Notes " +
                            " FROM ( " +
                            "  SELECT jv.Id, '' CompanyName, a.Name, DocumentNr, [Date], -Amount Amount, Notes " +
                            "  FROM JournalVoucher jv " +
                            "  INNER JOIN Account a ON a.Id = jv.AccountId " +
                            "  WHERE jv.DaybookId = @id1 AND PeriodId = @periodId AND TxnType = 0 {0} " +
                            " UNION ALL " +
                            "  SELECT jv.Id, '' CompanyName, a.Name, DocumentNr, [Date], Amount, Notes " +
                            "  FROM JournalVoucher jv " +
                            "  INNER JOIN Account a ON a.Id = jv.AccountId " +
                            "  WHERE jv.DaybookId = @id1 AND PeriodId = @periodId AND TxnType = 1 {0} " +
                            " ) r" +
                            " ORDER BY r.[Date], r.Id, r.DocumentNr ";

        public const string SelectJvYearlyLedger =
                            " SELECT 0 OpAmount, SUM(CrAmt) CrAmount, SUM(DbAmt) DbAmount, r.PeriodId " +
                            " FROM ( " +
                            "   SELECT SUM(Amount) CrAmt, 0 DbAmt, PeriodId " +
                            "   FROM JournalVoucher jv " +
                            "   INNER JOIN Account a ON a.Id = jv.AccountId " +
                            "   WHERE jv.DaybookId = @daybookId AND TxnType = 0 " +
                            "   GROUP BY PeriodId " +
                            " UNION ALL " +
                            "   SELECT 0 CrAmt, SUM(AMOUNT) DbAmt, PeriodId " +
                            "   FROM JournalVoucher jv " +
                            "   INNER JOIN Account a ON a.Id = jv.AccountId " +
                            "   WHERE jv.DaybookId = @daybookId AND TxnType = 1 " +
                            "   GROUP BY PeriodId " +
                            "      ) r " +
                            " GROUP BY r.PeriodId ";

        public const string SelectJvMonthlyLedger =
                            " SELECT SUM(CrAmt) CrAmount, SUM(DbAmt) DbAmount, r.[Month] " +
                            " FROM ( " +
                            "  SELECT SUM(Amount) CrAmt, 0 DbAmt, DATEPART(MONTH, jv.[Date]) [Month] " +
                            "  FROM JournalVoucher jv " +
                            "  INNER JOIN Account a ON a.Id = jv.AccountId " +
                            "  WHERE jv.DaybookId = @daybookId AND jv.TxnType = 0 AND PeriodId = @periodId " +
                            "  GROUP BY DATEPART(MONTH, jv.[Date]) " +
                            " UNION ALL " +
                            "  SELECT 0 CrAmt, SUM(AMOUNT) DbAmt, DATEPART(MONTH, jv.[Date]) [Month] " +
                            "  FROM JournalVoucher jv " +
                            "  INNER JOIN Account a ON a.Id = jv.AccountId " +
                            "  WHERE jv.DaybookId = @daybookId AND jv.TxnType = 1 AND PeriodId = @periodId " +
                            "  GROUP BY DATEPART(MONTH, jv.[Date]) " +
                            "    ) r " +
                            " GROUP BY r.[Month] ";

        public const string SelectTopNAccountItems =
                            "SELECT ac.Id AccountNr, ac.Name AccountName, im.Name ItemName, TotalAmount " +
                            "FROM ( " +
                            "  SELECT TOP {0} a.{1} AccId, i.{2} ItemId, SUM({4}) TotalAmount " +
                            "  FROM ItemLedger il " +
                            "  INNER JOIN Account a ON a.Id = il.AccountId " +
                            "  INNER JOIN Item i ON i.Id = il.ItemId " +
                            "  WHERE il.ChartOfAccountId {5} " +
                            "  AND a.Id NOT IN (SELECT DISTINCT AccountId FROM Daybook) " +
                            "  GROUP BY a.{1}, i.{2} " +
                            "  ORDER BY TotalAmount DESC " +
                            "  ) r " +
                            "INNER JOIN Account ac ON ac.Id = r.AccId " +
                            "INNER JOIN {3} im ON im.Id = r.ItemId ";
    }
}
