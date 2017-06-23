using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Report;
using ScalableApps.Foresight.Win.Common;

namespace ScalableApps.Foresight.Win.Reports
{
    public partial class ULedgerSummaryReport : UReportBase
    {
        #region Declarations

        private LedgerSummary _yearTotal;
        private IList<LedgerSummary> _report;
        private readonly Account _account;
        private readonly Daybook _daybook;
        private LedgerView _currentView = LedgerView.Yearly;
        public ULedgerDetailReport DetailControl { private get; set; }

        #endregion

        #region Constructor

        public ULedgerSummaryReport(Command command, Daybook jvDaybook)
            : base(command)
        {
            _daybook = jvDaybook;
            initializeReport();
        }

        public ULedgerSummaryReport(Command command, Account account)
            : base(command)
        {
            _account = account;
            initializeReport();
        }

        private void initializeReport()
        {
            InitializeComponent();
            rdc = new LedgerSummaryDataContext();
            loadAmountFormatList();
        }

        #endregion

        #region Event Handlers

        private void UCompanyLedgerReport_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                setReportTitle();
                lblDescription.Text = _account == null ? "" : _account.GetAddressString();
                showReport();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void cmbAmtFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (_report == null)
                    return;

                Cursor = Cursors.WaitCursor;
                renderReport();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void lvwReport_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                processLedgerView();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (keyData == Keys.Enter)
                {
                    processLedgerView();
                    return true;
                }

                if (keyData == Keys.Back)
                {
                    processBackButton();
                    return true;
                }

            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void processLedgerView()
        {
            if (lvwReport.SelectedItems.Count == 0)
                return;

            switchLedgerView();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                processBackButton();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion

        #region Private Methods

        private void switchLedgerView()
        {
            Cursor = Cursors.WaitCursor;

            if (_currentView == LedgerView.Yearly)
                showMonthlyLedger();
            else
                showDetailLedger();
        }

        private void processBackButton()
        {
            Cursor = Cursors.WaitCursor;

            if (_currentView == LedgerView.Yearly)
            {
                var form = FindForm();
                if (form != null) form.Close();
            }
            else
            {
                _currentView = LedgerView.Yearly;
                setViewTypeColumnText();
                setLedgerPeriod();

                Cursor = Cursors.Default;
                showReport();
            }
        }

        private void loadAmountFormatList()
        {
            Utilities.LoadEnumListItems(cmbAmtFormat, typeof(ReportsAmountFormat),
                                    (int)ReportsAmountFormat.Thousands);
        }

        private void buildReportViewColumns()
        {
            lvwReport.Columns.Clear();
            lvwReport.Items.Clear();

            lvwReport.Columns.Add("Year", 80);

            if (_currentView == LedgerView.Yearly)
                lvwReport.Columns.Add("Opening", 120, HorizontalAlignment.Right);

            lvwReport.Columns.Add("Credit", 120, HorizontalAlignment.Right);
            lvwReport.Columns.Add("Debit", 120, HorizontalAlignment.Right);
            lvwReport.Columns.Add("Balance", 120, HorizontalAlignment.Right);
        }

        private void setViewTypeColumnText()
        {
            lvwReport.Columns[0].Text = _currentView == LedgerView.Yearly ? "Year" : "Month";
        }

        private void setReportTitle()
        {
            lblTitle.Text = _account == null ? _daybook.Name : _account.Name;
        }

        private void showReport()
        {
            getReportData();
            renderReport();
            Utilities.SelectListItem(lvwReport, 0, true);
        }

        private void getReportData()
        {
            var ldc = rdc as LedgerSummaryDataContext;
            if (ldc == null)
                return;

            ldc.Account = _account;
            ldc.Daybook = _daybook;
            ldc.View = _currentView;

            if (_currentView == LedgerView.Monthly)
                ldc.Period = getSelectedPeriod().CompanyPeriod.Period;

            _report = ldc.GetReportData().Result as IList<LedgerSummary>;
        }

        private LedgerSummary getSelectedPeriod()
        {
            return lvwReport.SelectedItems[0].Tag as LedgerSummary;
        }

        private void renderReport()
        {
            buildReportViewColumns();

            if (_currentView == LedgerView.Monthly)
                addMonthlyOpeningRow();

            addReportViewRows();

            if (_currentView == LedgerView.Monthly)
                addMonthlyTotalsRow();
        }

        private void addMonthlyOpeningRow()
        {
            var lvi = new ListViewItem();
            lvi.Text = @"Opening:";
            lvi.ForeColor = Color.Maroon;

            if (_yearTotal.OpeningAmount > 0)
            {
                lvi.SubItems.Add(formatAmount(_yearTotal.OpeningAmount, cmbAmtFormat));
                lvi.SubItems.Add("");
            }
            else
            {
                lvi.SubItems.Add("");
                lvi.SubItems.Add(formatAmount(Math.Abs(_yearTotal.OpeningAmount), cmbAmtFormat));
            }

            lvi.SubItems.Add(formatAmount(_yearTotal.OpeningAmount, cmbAmtFormat, withDbCr: true));
            lvwReport.Items.Add(lvi);
        }

        private void addReportViewRows()
        {
            foreach (var ls in _report)
            {
                var lvi = new ListViewItem();
                lvi.UseItemStyleForSubItems = true;
                lvi.Font = new Font(lvi.Font, FontStyle.Regular);
                lvi.Tag = ls;
                lvi.Text = getViewTypeColumnValue(ls);

                if (_currentView == LedgerView.Yearly)
                    lvi.SubItems.Add(formatAmount(ls.OpeningAmount, cmbAmtFormat, withDbCr: true));

                lvi.SubItems.Add(formatAmount(ls.CreditAmount, cmbAmtFormat));
                lvi.SubItems.Add(formatAmount(ls.DebitAmount, cmbAmtFormat));
                lvi.SubItems.Add(formatAmount(ls.BalanceAmount, cmbAmtFormat, withDbCr: true));
                lvwReport.Items.Add(lvi);
            }
        }

        private void addMonthlyTotalsRow()
        {
            _yearTotal.Month = 13;
            var lvi = new ListViewItem();
            lvi.ForeColor = Color.Maroon;
            lvi.Tag = _yearTotal;
            lvi.Text = @"TOTAL:";
            lvi.SubItems.Add(formatAmount(_yearTotal.CreditAmount, cmbAmtFormat));
            lvi.SubItems.Add(formatAmount(_yearTotal.DebitAmount, cmbAmtFormat));
            lvi.SubItems.Add(formatAmount(_yearTotal.BalanceAmount, cmbAmtFormat, withDbCr: true));
            lvwReport.Items.Add(lvi);
        }

        private string getViewTypeColumnValue(LedgerSummary ls)
        {
            if (_currentView == LedgerView.Yearly)
                return ls.CompanyPeriod.Period.FinancialTo.Year.ToString();

            return getMonthName(ls.Month);
        }

        private void showMonthlyLedger()
        {
            _yearTotal = getSelectedPeriod();
            _currentView = LedgerView.Monthly;
            setViewTypeColumnText();
            setLedgerPeriod();
            showReport();
        }

        private void setLedgerPeriod()
        {
            if (_currentView == LedgerView.Monthly)
            {
                lblYear.Visible = true;
                lblYearValue.Text = getSelectedPeriod().CompanyPeriod.Period.FinancialTo.Year.ToString();
            }
            else
            {
                lblYear.Visible = false;
                lblYearValue.Text = "";
            }
        }

        private void showDetailLedger()
        {
            if (isOpeningRow())
                return;

            DetailControl.Summary = getSelectedPeriod();
            DetailControl.LoadData();
            Hide();
            Enabled = false;
            DetailControl.Enabled = true;
            DetailControl.Show();
            DetailControl.Focus();
        }

        private bool isOpeningRow()
        {
            return getSelectedPeriod() == null;
        }

        #endregion
    }
}
