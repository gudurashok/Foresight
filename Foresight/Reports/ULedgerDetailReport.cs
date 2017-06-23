using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Report;
using ScalableApps.Foresight.Win.Common;
using ScalableApps.Foresight.Logic.Common;
using System.Linq;

namespace ScalableApps.Foresight.Win.Reports
{
    public partial class ULedgerDetailReport : UReportBase
    {
        #region Declarations

        private IList<LedgerDetail> _report;
        private IList<JVAccountDetail> _reportJournalVoucher;

        private const int jvDateColumnIndex = 0;
        private const int jvDocumentNrColumnIndex = 1;
        private const int jvAccountNameColumnIndex = 2;
        private const int jvAmountColumnIndex = 3;
        private const int jvNotesColumnIndex = 4;

        private const int dateColumnIndex = 0;
        private const int companyColumnIndex = 1;
        private const int documentNrColumnIndex = 2;
        private const int accountNameColumnIndex = 3;
        private const int creditColumnIndex = 4;
        private const int debitColumnIndex = 5;
        private const int balanceColumnIndex = 6;
        private const int notesColumnIndex = 7;
        private const int fudgeSize = 21;
        private readonly Account _account;
        private readonly Daybook _daybook;
        public LedgerSummary Summary { private get; set; }
        public ULedgerSummaryReport SummaryControl { get; set; }

        #endregion

        #region Constructor

        public ULedgerDetailReport(Command command, Daybook jvDaybook)
            : this(command, jvDaybook, null) { }

        public ULedgerDetailReport(Command command, Daybook jvDaybook, LedgerSummary summary)
            : base(command)
        {
            _daybook = jvDaybook;
            initializeReport(summary);
        }

        public ULedgerDetailReport(Command command, Account account)
            : this(command, account, null) { }

        public ULedgerDetailReport(Command command, Account account, LedgerSummary summary)
            : base(command)
        {
            _account = account;
            initializeReport(summary);
        }

        private void initializeReport(LedgerSummary summary)
        {
            InitializeComponent();
            rdc = new LedgerDetailDataContext();
            Summary = summary;
        }

        #endregion

        #region Event Handlers

        private void UAccountLedgerReport_Load(object sender, EventArgs e)
        {
            try
            {
                if (Summary == null)
                    return;

                LoadData();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void lvwReport_Resize(object sender, EventArgs e)
        {
            try
            {
                if (lvwReport.Columns.Count != 0)
                    autoResize();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData != Keys.Back)
                return base.ProcessCmdKey(ref msg, keyData);

            try
            {
                showSummary();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }

            return true;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                showSummary();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        #endregion

        #region Private Methods

        private void showSummary()
        {
            Hide();
            Enabled = false;
            SummaryControl.Enabled = true;
            SummaryControl.Show();
            SummaryControl.Focus();
        }

        public void LoadData()
        {
            Cursor = Cursors.WaitCursor;

            lvwReport.Resize -= lvwReport_Resize;
            lblTitle.Text = _account == null ? _daybook.Name : _account.Name;

            if (Summary.Month < 13)
                lblPeriodValue.Text = string.Format("{0}, {1}", getMonthName(Summary.Month),
                                        Summary.CompanyPeriod.Period.FinancialTo.Year);
            else
                lblPeriodValue.Text = string.Format("{0}",
                                        Summary.CompanyPeriod.Period.FinancialTo.Year);


            showReport();
            lvwReport.Resize += lvwReport_Resize;

            Cursor = Cursors.Default;
        }

        private void showReport()
        {
            getReportData();
            renderReport();
            Utilities.SelectListItem(lvwReport, 0, true);
        }

        private void getReportData()
        {
            var ldc = new LedgerDetailDataContext();
            ldc.Account = _account;
            ldc.Daybook = _daybook;
            ldc.Month = Summary.Month;
            ldc.Period = Summary.CompanyPeriod.Period;
            _report = ldc.GetReportData().Result as IList<LedgerDetail>;

            if (isJVDaybook())
                _reportJournalVoucher = transformLedgerDetails()
                                        .OrderBy(r => r.Amount.ToString(Constants.AmountFormatWithCrDb))
                                        .OrderBy(r => r.DocumentNr).OrderBy(r => r.Date).ToList();

        }

        private IEnumerable<JVAccountDetail> transformLedgerDetails()
        {
            var jvDetails = new List<JVAccountDetail>();
            foreach (var ld in _report)
            {
                var jv = new JVAccountDetail();
                jv.Date = ld.Date;
                jv.DocumentNr = ld.DocumentNr;
                jv.AccountName = ld.DaybookName;
                jv.Amount = ld.Amount;
                jv.Notes = ld.Notes;
                jvDetails.Add(jv);
            }

            return jvDetails;
        }

        private void renderReport()
        {
            if (isJVDaybook())
            {
                buildJVColumns();
                addJVViewRows();
            }
            else
            {
                buildReportViewColumns();
                addOpeningBalanceRow();
                addReportViewRows();
                addTotalsRow();
            }
        }

        private void addJVViewRows()
        {
            foreach (var JournalVoucher in _reportJournalVoucher)
            {
                var lvi = new ListViewItem(JournalVoucher.Date.ToString("dd/MM/yyyy"));
                lvi.UseItemStyleForSubItems = true;
                lvi.Font = new Font(lvi.Font, FontStyle.Regular);
                lvi.SubItems.Add(string.Format("{0, 9}", JournalVoucher.DocumentNr));
                lvi.SubItems.Add(JournalVoucher.AccountName);
                addRunningBalanceColumn(lvi, JournalVoucher.Amount);
                lvi.SubItems.Add(JournalVoucher.Notes);
                lvwReport.Items.Add(lvi);
            }
        }

        private void buildJVColumns()
        {
            lvwReport.Columns.Clear();
            lvwReport.Items.Clear();

            lvwReport.Columns.Add("Date", 70);
            lvwReport.Columns.Add("Doc. Nr.", 70, HorizontalAlignment.Right);
            lvwReport.Columns.Add("Account Name", 120);
            lvwReport.Columns.Add("Amount", 150, HorizontalAlignment.Right);
            lvwReport.Columns.Add("Notes", 150);
            autoResize();
        }

        private void addReportViewRows()
        {
            var balance = Summary.OpeningAmount;

            foreach (var ld in _report)
            {
                balance += ld.Amount;
                var lvi = new ListViewItem(ld.Date.ToString("dd/MM/yyyy"));
                lvi.UseItemStyleForSubItems = true;
                lvi.Font = new Font(lvi.Font, FontStyle.Regular);
                lvi.Tag = ld;
                lvi.SubItems.Add(ld.DocumentNr);
                lvi.SubItems.Add(ld.CompanyName);
                lvi.SubItems.Add(ld.DaybookName);

                addTransactionColumnValues(lvi, ld.Amount);
                addRunningBalanceColumn(lvi, balance);
                lvi.SubItems.Add(ld.Notes);
                lvwReport.Items.Add(lvi);
            }
        }

        private void addOpeningBalanceRow()
        {
            var lvi = new ListViewItem("");
            lvi.ForeColor = Color.Maroon;
            lvi.SubItems.Add("");
            lvi.SubItems.Add("");
            lvi.SubItems.Add("Opening:");

            addTransactionColumnValues(lvi, Summary.OpeningAmount);
            addRunningBalanceColumn(lvi, Summary.OpeningAmount);
            lvwReport.Items.Add(lvi);
        }

        private void addTransactionColumnValues(ListViewItem lvi, decimal amount)
        {
            if (amount > 0)
            {
                lvi.SubItems.Add(amount.ToString(Constants.AmountFormat));
                lvi.SubItems.Add("");
            }
            else
            {
                lvi.SubItems.Add("");
                lvi.SubItems.Add(Math.Abs(amount).ToString(Constants.AmountFormat));
            }
        }

        private void addRunningBalanceColumn(ListViewItem lvi, decimal amount)
        {
            lvi.SubItems.Add(amount.ToString(Constants.AmountFormatWithCrDb));
        }

        private void addTotalsRow()
        {
            var lvi = new ListViewItem("");
            lvi.ForeColor = Color.Maroon;
            lvi.SubItems.Add("");
            lvi.SubItems.Add("");
            lvi.SubItems.Add("TOTAL:");
            lvi.SubItems.Add(Summary.CreditAmount.ToString(Constants.AmountFormat));
            lvi.SubItems.Add(Summary.DebitAmount.ToString(Constants.AmountFormat));
            addRunningBalanceColumn(lvi, Summary.BalanceAmount);
            lvwReport.Items.Add(lvi);
        }

        private void buildReportViewColumns()
        {
            lvwReport.Columns.Clear();
            lvwReport.Items.Clear();

            lvwReport.Columns.Add("Date", 70);
            lvwReport.Columns.Add("Doc. Nr.", 70);
            lvwReport.Columns.Add("Company", 70);
            lvwReport.Columns.Add("Account Name", 120,HorizontalAlignment.Left);
            lvwReport.Columns.Add("Credit", 120, HorizontalAlignment.Right);
            lvwReport.Columns.Add("Debit", 120, HorizontalAlignment.Right);
            lvwReport.Columns.Add("Balance", 150, HorizontalAlignment.Right);
            lvwReport.Columns.Add("Notes", 0);
            autoResize();
        }

        private void autoResize()
        {
            int adjWidth;
            if (isJVDaybook())
            {
                adjWidth = lvwReport.Width -
                            (lvwReport.Columns[jvDateColumnIndex].Width +
                            lvwReport.Columns[jvDocumentNrColumnIndex].Width +
                            lvwReport.Columns[jvAmountColumnIndex].Width +
                            fudgeSize);

                lvwReport.Columns[jvNotesColumnIndex].Width = adjWidth;
            }
            else
            {
                adjWidth = lvwReport.Width -
                        (lvwReport.Columns[dateColumnIndex].Width +
                         lvwReport.Columns[companyColumnIndex].Width +
                         lvwReport.Columns[documentNrColumnIndex].Width +
                         lvwReport.Columns[creditColumnIndex].Width +
                         lvwReport.Columns[debitColumnIndex].Width +
                         lvwReport.Columns[balanceColumnIndex].Width +
                         fudgeSize);

                lvwReport.Columns[notesColumnIndex].Width = adjWidth;
            }
        }

        private bool isJVDaybook()
        {
            return _daybook != null && _daybook.Type == DaybookType.JournalVoucher;
        }

        #endregion
    }
}
