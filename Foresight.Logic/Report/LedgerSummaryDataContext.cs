using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Sql;

namespace ScalableApps.Foresight.Logic.Report
{
    public class LedgerSummaryDataContext : ReportDataContext
    {
        public Account Account { private get; set; }
        public Daybook Daybook { private get; set; }
        public DatePeriod Period { private get; set; }
        public LedgerView View { private get; set; }
        private IList<LedgerSummary> _report;

        public override ReportData GetReportData()
        {
            if (Account == null)
                loadJvData();
            else
                loadData();
            return new ReportData(_report);
        }

        private void loadJvData()
        {
            if (View == LedgerView.Yearly)
                _report = loadYearlyData(readJvYearlyLedgerData());
            else
            {
                _report = loadMonthlyData(readJvMonthlyLedgerData());
                reorderMontlyLedgerResult();
            }
        }

        private void loadData()
        {
            if (View == LedgerView.Yearly)
                _report = loadYearlyData(readYearlyLedgerData());
            else
            {
                _report = loadMonthlyData(readMonthlyLedgerData());
                reorderMontlyLedgerResult();
                calculateMontlyClosingBalances();
            }
        }

        #region Yearly Ledger

        private IDataReader readJvYearlyLedgerData()
        {
            return db.ExecuteReader(ReportQueries.SelectJvYearlyLedger,
                                    "@daybookId", Daybook.Id);
        }

        private IDataReader readYearlyLedgerData()
        {
            return db.ExecuteReader(
                      string.Format(ReportQueries.SelectYearlyLedger,
                                    AccountTransTables.GetCreditAmountExpr(),
                                    AccountTransTables.GetDebitAmountExpr()), "@accountId", Account.Id);
        }

        private IList<LedgerSummary> loadYearlyData(IDataReader rdr)
        {
            var result = new List<LedgerSummary>();
            while (rdr.Read())
                result.Add(readYearlyLedger(rdr));

            rdr.Close();
            return result;
        }

        private LedgerSummary readYearlyLedger(IDataReader rdr)
        {
            var result = new LedgerSummary { CompanyPeriod = new CompanyPeriod() };
            result.CompanyPeriod.Period = Session.Dbc.GetDatePeriodById(Convert.ToInt32(rdr["PeriodId"]));
            result.OpeningAmount = Convert.ToDecimal(rdr["OpAmount"]);
            readTransAmount(rdr, result);
            result.BalanceAmount = result.OpeningAmount + result.CreditAmount - result.DebitAmount;
            return result;
        }

        #endregion

        #region Common

        private void readTransAmount(IDataReader rdr, LedgerSummary ls)
        {
            ls.CreditAmount = Convert.ToDecimal(rdr["CrAmount"]);
            ls.DebitAmount = Convert.ToDecimal(rdr["DbAmount"]);
        }

        #endregion

        #region Monthly Ledger

        private IList<LedgerSummary> loadMonthlyData(IDataReader rdr)
        {
            var result = new List<LedgerSummary>();
            while (rdr.Read())
                result.Add(readMonthlyLedger(rdr));

            rdr.Close();
            return result;
        }

        private IDataReader readMonthlyLedgerData()
        {
            var cmd = db.CreateCommand(
                string.Format(ReportQueries.SelectMonthlyLedger,
                              AccountTransTables.GetCreditAmountExpr(),
                              AccountTransTables.GetDebitAmountExpr()));
            db.AddParameterWithValue(cmd, "@accountId", Account.Id);
            db.AddParameterWithValue(cmd, "@periodId", Period.Id);
            return cmd.ExecuteReader();
        }

        private void calculateMontlyClosingBalances()
        {
            if (_report.Count == 0)
                return;

            _report[0].OpeningAmount = Session.Dbc.GetAccountOpeningBalance(Account.Id, Period.Id);
            for (var i = 0; i < _report.Count; i++)
            {
                _report[i].BalanceAmount = _report[i].OpeningAmount + _report[i].CreditAmount - _report[i].DebitAmount;

                if (i < _report.Count - 1)
                    _report[i + 1].OpeningAmount = _report[i].BalanceAmount;
            }
        }

        private IDataReader readJvMonthlyLedgerData()
        {
            var cmd = db.CreateCommand(ReportQueries.SelectJvMonthlyLedger);
            db.AddParameterWithValue(cmd, "@daybookId", Daybook.Id);
            db.AddParameterWithValue(cmd, "@periodId", Period.Id);
            return cmd.ExecuteReader();
        }

        private void reorderMontlyLedgerResult()
        {
            var oldResult = (from r in _report orderby r.Month select r).ToList();
            foreach (var ls in oldResult.Where(ls => ls.Month > 12))
                ls.Month = ls.Month % 12;

            _report = oldResult;
        }

        private LedgerSummary readMonthlyLedger(IDataReader rdr)
        {
            var result = new LedgerSummary { CompanyPeriod = new CompanyPeriod() };
            result.CompanyPeriod.Period = Period;
            var month = Convert.ToInt32(rdr["Month"]);
            result.Month = month < 4 ? month + 12 : month;
            readTransAmount(rdr, result);
            return result;
        }

        #endregion
    }
}
