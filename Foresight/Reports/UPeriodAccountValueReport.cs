using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Report;
using ScalableApps.Foresight.Win.Common;

namespace ScalableApps.Foresight.Win.Reports
{
    public partial class UPeriodAccountValueReport : UReportBase
    {
        #region Internal Declarations

        private IList<CompanyPeriodAccountValue> _report;

        #endregion

        #region Constructor

        public UPeriodAccountValueReport(Command command)
            : base(command)
        {
            InitializeComponent();
            loadAmountFormatList();
        }

        #endregion

        #region Event Handlers

        private void UPeriodAccountValueReport_Load(object sender, EventArgs e)
        {
            try
            {
                btnShow.Focus();
                btnShow.PerformClick();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void btnSelectCompany_Click(object sender, EventArgs e)
        {
            try
            {
                if (!selectCompanyPeriods(CompanyPeriodType.Both))
                    return;

                Cursor = Cursors.WaitCursor;
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

        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
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

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                SelectedCoPeriods = new List<CompanyPeriod>();
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
                showLedger();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void lvwReport_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar != (char)Keys.Enter)
                    return;

                showLedger();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        #endregion

        #region Private methods

        private void showLedger()
        {
            var companyPeriodAccountValue = lvwReport.SelectedItems[0].Tag as CompanyPeriodAccountValue;

            if (companyPeriodAccountValue == null)
                return;

            var account = Session.Dbc.GetAccountById(companyPeriodAccountValue.Topper.Account.Id);

            if (account == null)
                return;

            FReportViewer.ShowLedger(FindForm(), account);
        }

        private void loadAmountFormatList()
        {
            Utilities.LoadEnumListItems(cmbAmtFormat, typeof(ReportsAmountFormat),
                                    (int)AmountFormat);
        }

        private void showReport()
        {
            getReportData();
            renderReport();
        }

        private void getReportData()
        {
            var adc = rdc as PeriodAccountBaseDataContext;
            if (adc == null)
                return;

            adc.PartyGrouping = chkPartyGrouping.Checked;
            adc.SelectedCoPeriods = SelectedCoPeriods;
            adc.TopNCount = Convert.ToInt32(nudTopNCount.Value);
            _report = adc.GetReportData().Result as IList<CompanyPeriodAccountValue>;
        }

        private void renderReport()
        {
            if (_report.Count == 0)
                return;

            buildReportViewColumns();
            addReportViewRows();
            formatReportView();
        }

        private void formatReportView()
        {
            formatCompanyColumn();
            formatTotalRow();
        }

        private void formatCompanyColumn()
        {
            foreach (ListViewItem item in lvwReport.Items)
                item.SubItems[0].ForeColor = Color.Blue;

            lvwReport.Items[lvwReport.Items.Count - 1].SubItems[0].ForeColor = Color.Brown;
        }

        private void buildReportViewColumns()
        {
            lvwReport.Columns.Clear();
            lvwReport.Items.Clear();

            lvwReport.Columns.Add(getColumnName(), 150);

            var periodIds = getDistinctPeriods();
            if (periodIds == null)
                return;

            var count = periodIds.Count();
            foreach (var period in periodIds.Select(getPeriodOf))
            {
                addListPeriodColumn(period);

                if (count != 1)
                    addDiffPctListColumn();

                count--;
            }
        }

        private string getColumnName()
        {
            var a = Command.GetPropertyValue("ListColumnTitle");
            return chkPartyGrouping.Checked ? a + " (Groupwise)" : a;
        }

        private void formatTotalRow()
        {
            var lvi = lvwReport.Items[lvwReport.Items.Count - 1];
            foreach (ListViewItem.ListViewSubItem lvs in lvi.SubItems)
                lvs.Font = new Font(Font, FontStyle.Bold);
        }

        private void addListPeriodColumn(CompanyPeriodAccountValue cpv)
        {
            var year = lvwReport.Columns.Add(cpv.Topper.CompanyPeriod.Period.FinancialTo.Year.ToString(), 100);
            year.Tag = cpv.Topper.CompanyPeriod.Period.Id;
            year.TextAlign = HorizontalAlignment.Right;
        }

        private void addDiffPctListColumn()
        {
            var diffPct = lvwReport.Columns.Add("Diff.%", 65);
            diffPct.TextAlign = HorizontalAlignment.Right;
        }

        private IEnumerable<int> getDistinctPeriods()
        {
            return (from r in _report
                    orderby r.Topper.CompanyPeriod.Period.Id descending
                    select r.Topper.CompanyPeriod.Period.Id).Distinct();
        }

        private CompanyPeriodAccountValue getPeriodOf(int periodId)
        {
            return (_report.First(r => r.Topper.CompanyPeriod.Period.Id == periodId));
        }

        private void addReportViewRows()
        {
            foreach (var accountId in (_report.Select(r => r.Topper.Account.Id).Distinct()))
            {
                var accounts = getAccountsOf(accountId);
                var lvi = new ListViewItem(accounts[0].Topper.Account.Name);
                lvi.UseItemStyleForSubItems = false;

                for (var i = 1; i < lvwReport.Columns.Count; i = i + 2)
                    addSubItem(lvi, getAccountValueOfPeriod(accounts, accountId, getColumnPeriodId(i)));

                lvwReport.Items.Add(lvi);
            }
        }

        private int getColumnPeriodId(int i)
        {
            var ch = lvwReport.Columns[i];
            return Convert.ToInt32(ch.Tag);
        }

        private CompanyPeriodAccountValue getAccountValueOfPeriod(
                            IEnumerable<CompanyPeriodAccountValue> accounts,
                            int accountId, int periodId)
        {
            var apv = accounts.SingleOrDefault(ap => ap.Topper.CompanyPeriod.Period.Id == periodId);
            if (apv == null)
            {
                var value = getPeriodAccountValue(accountId, periodId);
                return new CompanyPeriodAccountValue
                {
                    Topper = new CompanyPeriodTopperValue
                    {
                        Account = new AccountValue { Id = accountId, Amount = value }
                    }
                };
            }

            return apv;
        }

        private decimal? getPeriodAccountValue(int accountId, int periodId)
        {
            var adc = rdc as PeriodAccountBaseDataContext;
            return adc != null ? adc.GetPeriodAccountValue(accountId, periodId) : null;
        }

        private void addSubItem(ListViewItem lvi, CompanyPeriodAccountValue apv)
        {
            var lvs = lvi.SubItems.Add(getAccountValue(apv));

            if (lvi.Tag == null)
                lvi.Tag = apv;

            formatSubItem(lvs);
            setColorOfNotInTopN(apv, lvs);
            addDiffPctSubItem(apv, lvi);
        }

        private string getAccountValue(CompanyPeriodAccountValue apv)
        {
            var value = apv.Topper.Account.Amount ?? 0;

            if (isNotInTopN(apv))
                if (apv.Topper.Account.Amount == null)
                    return "<N.A>";
                else
                    return string.Format("<{0}>", formatAmount(value, cmbAmtFormat));

            return formatAmount(value, cmbAmtFormat);
        }

        private void setColorOfNotInTopN(CompanyPeriodAccountValue apv, ListViewItem.ListViewSubItem lvs)
        {
            if (isNotInTopN(apv))
                lvs.ForeColor = Color.DarkGray;
        }

        private bool isNotInTopN(CompanyPeriodAccountValue apv)
        {
            return (apv.Topper.Account.Id == 0) &&
                    string.IsNullOrEmpty(apv.Topper.Account.Name);
        }

        private void formatSubItem(ListViewItem.ListViewSubItem listViewSubItem)
        {
            listViewSubItem.Font = new Font(lvwReport.Font, FontStyle.Regular);
        }

        private void addDiffPctSubItem(CompanyPeriodAccountValue account, ListViewItem lvi)
        {
            var pct = account.DifferencePct;
            var lvs = lvi.SubItems.Add((pct ?? 0).ToString("0.00"));
            lvs.ForeColor = getDifferencePctColor(account.DifferencePct);
            lvs.Font = new Font(lvwReport.Font, FontStyle.Regular);
        }

        private IList<CompanyPeriodAccountValue> getAccountsOf(int accountId)
        {
            return (from r in _report
                    where r.Topper.Account.Id == accountId
                    orderby r.Topper.CompanyPeriod.Period.Id descending
                    select r).ToList();
        }

        private Color getDifferencePctColor(decimal? value)
        {
            if (value < 0)
                return Color.FromName(Command.GetPropertyValue("NegativePctColor"));
            if (value > 0)
                return Color.FromName(Command.GetPropertyValue("PositivePctColor"));

            return Color.Black;
        }

        #endregion

    }
}
