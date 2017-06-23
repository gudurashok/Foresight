using System;
using System.Collections.Generic;
using System.Data;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Sql;

namespace ScalableApps.Foresight.Logic.Report
{
    public class LedgerDetailDataContext : ReportDataContext
    {
        public Account Account { private get; set; }
        public Daybook Daybook { private get; set; }
        public DatePeriod Period { private get; set; }
        public int Month { private get; set; }
        private IList<LedgerDetail> _result;
        private int _id;
        private bool _isDaybook;

        public override ReportData GetReportData()
        {
            _result = new List<LedgerDetail>();
            getDaybookIdOfAccount();

            if (Account != null)
                foreach (var trans in AccountTransTables.GetCreditDebitTables())
                    loadData(readData(trans));
            else
                loadData(readData(new AccountTransTables()));


            return new ReportData(_result);
        }

        private void getDaybookIdOfAccount()
        {
            if (Account != null)
            {
                var daybook = Session.Dbc.GetDaybookOfAccount(Account.Id);
                if (daybook == null || daybook.Id == 0)
                {
                    _id = Account.Id;
                    _isDaybook = false;
                }
                else
                {
                    Daybook = daybook;
                    setDaybook();
                }
            }
            else
                setDaybook();
        }

        private void setDaybook()
        {
            _id = Daybook.Id;
            _isDaybook = true;
        }

        private IDataReader readData(AccountTransTables trans)
        {
            var cmd = db.CreateCommand(getQuery(trans));
            addQueryParameters(cmd);
            return cmd.ExecuteReader();
        }

        private void addQueryParameters(IDbCommand cmd)
        {
            db.AddParameterWithValue(cmd, "@id1", _id);

            if (Account != null)
                db.AddParameterWithValue(cmd, "@id2", Account.Id);

            db.AddParameterWithValue(cmd, "@periodId", Period.Id);
            db.AddParameterWithValue(cmd, "@month", Month);
        }

        private string getQuery(AccountTransTables trans)
        {
            if (Account == null)
                return string.Format(ReportQueries.SelectJvDetailLedger, getMonthFilter());

            return string.Format(ReportQueries.SelectDetailLedger,
                                trans.TransAmount, trans.TableName, getJoiningTable(),
                                getComparionColumn(), getFilterExpr(trans));
        }

        private string getJoiningTable()
        {
            return _isDaybook ? "Account" : "Daybook";
        }

        private string getComparionColumn()
        {
            return _isDaybook ? "DaybookId = @id1 OR t.AccountId = @id2" : "AccountId = @id1";
        }

        private string getFilterExpr(AccountTransTables trans)
        {
            return getTxnTypeFilter(trans) + getMonthFilter();
        }

        private static string getTxnTypeFilter(AccountTransTables trans)
        {
            return String.IsNullOrEmpty(trans.Filter) ? "" : " AND " + trans.Filter;
        }

        private string getMonthFilter()
        {
            return Month < 13 ? " AND DATEPART(MONTH, [Date]) = @month " : "";
        }

        private void loadData(IDataReader rdr)
        {
            while (rdr.Read())
            {
                var ld = new LedgerDetail();
                ld.Id = Convert.ToInt32(rdr["Id"]);
                ld.CompanyName = rdr["CompanyName"].ToString();
                ld.DaybookName = rdr["Name"].ToString();
                ld.DocumentNr = rdr["DocumentNr"].ToString();
                ld.Date = Convert.ToDateTime(rdr["Date"]);
                ld.Amount = Convert.ToDecimal(rdr["Amount"]);
                ld.Notes = rdr["Notes"].ToString();
                _result.Add(ld);
            }

            rdr.Close();
        }
    }
}
