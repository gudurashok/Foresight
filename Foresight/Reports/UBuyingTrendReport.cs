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
    public partial class UBuyingTrendReport : UReportBase
    {
        #region Internal Declarations

        private const int yearColumnIndex = 0;
        private IList<BuyingTrendValue> _report;
        private IList<int> _selectedAccountIds;
        private bool _partyGrouping;

        #endregion

        #region Constructor

        public UBuyingTrendReport(Command command)
            : base(command)
        {
            InitializeComponent();
            loadAmountFormatList();
            _selectedAccountIds = new List<int>();
            displaySelectedParty();
        }

        #endregion

        #region Event Handlers

        private void UBuyingTrendReport_Load(object sender, EventArgs e)
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

        private void btnParty_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (!selectAccountGroups())
                    return;

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

        private void btnCompany_Click(object sender, EventArgs e)
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

        #endregion

        #region Private methods

        private void loadAmountFormatList()
        {
            Utilities.LoadEnumListItems(cmbAmtFormat, typeof(ReportsAmountFormat),
                                    (int)AmountFormat);
        }

        private void showReport()
        {
            Cursor = Cursors.WaitCursor;
            getReportData();
            renderReport();
        }

        private void renderReport()
        {
            buildReportViewColumns();
            addReportViewRows();
            addMonthTotalsRow();
            formatYearColumn();
        }

        private void getReportData()
        {
            var bdc = rdc as BuyingTrendDataContext;
            rdc.PartyGrouping = _partyGrouping;
            if (bdc == null)
                return;

            bdc.SelectedCoPeriods = SelectedCoPeriods;
            bdc.SelectedAccountIds = _selectedAccountIds;
            _report = rdc.GetReportData().Result as IList<BuyingTrendValue>;
        }

        private bool selectAccountGroups()
        {
            var accountsForm = new FAccounts(_selectedAccountIds, true, AccountTypes.Customers, true);

            var result = accountsForm.ShowDialog();

            if (result == DialogResult.Cancel)
                return false;

            _selectedAccountIds = accountsForm.GetSelectedAccountIds();
            _partyGrouping = accountsForm.IsPartyGroupingSelected;
            displaySelectedParty();
            SelectedCoPeriods = new List<CompanyPeriod>();
            return true;
        }

        private void displaySelectedParty()
        {
            if (_selectedAccountIds.Count == 0)
                txtParty.Text = @"(All)";
            else if (_selectedAccountIds.Count > 1)
                txtParty.Text = @"(Multiple)";
            else
            {
                var account = Session.Dbc.GetAccountById(_selectedAccountIds[0]);
                txtParty.Text = string.Format("{0}, {1}", account.Name, account.GetAddressString());
            }
        }

        private void formatYearColumn()
        {
            var yearTotalColumnIndex = lvwReport.Columns.Count - 1;
            var monthTotalColumnIndex = lvwReport.Items.Count - 1;

            foreach (ListViewItem item in lvwReport.Items)
            {
                item.SubItems[yearColumnIndex].ForeColor = Color.Blue;
                item.SubItems[yearTotalColumnIndex].ForeColor = Color.Brown;
                item.SubItems[yearTotalColumnIndex].Font = lvwReport.Font;
            }

            lvwReport.Items[monthTotalColumnIndex].SubItems[0].ForeColor = Color.Brown;
        }

        private void buildReportViewColumns()
        {
            lvwReport.Columns.Clear();
            lvwReport.Items.Clear();

            lvwReport.Columns.Add("Year", 70);
            buildMonthColumns();
            buildYearTotalColumn();
        }

        private void buildYearTotalColumn()
        {
            var yearTotal = lvwReport.Columns.Add("TOTAL", 100);
            yearTotal.Tag = 13;
            yearTotal.TextAlign = HorizontalAlignment.Right;
        }

        private void buildMonthColumns()
        {
            foreach (var month in getReorderedDistinctMonths())
                addMonthColumn(month);
        }

        private void addMonthColumn(int month)
        {
            var year = lvwReport.Columns.Add(getMonthName(calculateMonth(month)), 80);
            year.Tag = calculateMonth(month);
            year.TextAlign = HorizontalAlignment.Right;
        }

        private int calculateMonth(int month)
        {
            if (month > 12)
                month = month % 12;

            return month;
        }

        private IEnumerable<int> getReorderedDistinctMonths()
        {
            var months = (from r in _report
                          orderby r.Month
                          select r.Month).Distinct().ToArray();

            for (var i = 0; i <= months.Length - 1; i++)
                if (months[i] < 4) months[i] += 12;

            return months.OrderBy(i => i).ToArray();
        }

        private void addReportViewRows()
        {
            foreach (var periodId in (_report.Select(r => r.Period.Id).Distinct()))
            {
                IList<BuyingTrendValue> monthValues = getMonthValuesOf(periodId);
                var lvi = new ListViewItem(monthValues[0].Period.FinancialTo.Year.ToString());
                lvi.UseItemStyleForSubItems = false;

                for (var i = 1; i < lvwReport.Columns.Count - 1; i++)
                    addSubItem(lvi, getBuyingTrendValue(monthValues, periodId, getMonthNumber(i)));

                addSubItem(lvi, getYearTotal(periodId));
                lvwReport.Items.Add(lvi);
            }
        }

        private int getMonthNumber(int i)
        {
            var ch = lvwReport.Columns[i];
            var month = Convert.ToInt32(ch.Tag);
            return month;
        }

        private BuyingTrendValue getBuyingTrendValue(IEnumerable<BuyingTrendValue> monthValues,
                                                    int periodId, int month)
        {
            var value = monthValues.SingleOrDefault(r => r.Period.Id == periodId &&
                                                        r.Month == month);
            if (value == null)
                return new BuyingTrendValue();

            return value;
        }

        private void addSubItem(ListViewItem lvi, BuyingTrendValue btv)
        {
            var lvs = lvi.SubItems.Add(getMonthValue(btv));
            formatSubItem(lvs);
            setColorOfNonAvailable(btv, lvs);
        }

        private string getMonthValue(BuyingTrendValue btv)
        {
            if (isNotAvailable(btv))
                return "<N.A>";

            return formatAmount(btv.Amount, cmbAmtFormat);
        }

        private void setColorOfNonAvailable(BuyingTrendValue btv, ListViewItem.ListViewSubItem lvs)
        {
            if (isNotAvailable(btv))
                lvs.ForeColor = Color.DarkGray;
        }

        private bool isNotAvailable(BuyingTrendValue btv)
        {
            return (btv.Month == 0);
        }

        private void formatSubItem(ListViewItem.ListViewSubItem listViewSubItem)
        {
            listViewSubItem.Font = new Font(lvwReport.Font, FontStyle.Regular);
        }

        private List<BuyingTrendValue> getMonthValuesOf(int periodId)
        {
            return (from r in _report
                    where r.Period.Id == periodId
                    orderby r.Month
                    select r).ToList();
        }

        private void addMonthTotalsRow()
        {
            var lvi = new ListViewItem("TOTAL:");

            foreach (var month in getReorderedDistinctMonths())
                addSubItem(lvi, getMonthTotal(calculateMonth(month)));

            addSubItem(lvi, getGrandTotal());
            lvwReport.Items.Add(lvi);
        }

        private BuyingTrendValue getGrandTotal()
        {
            return new BuyingTrendValue { Month = 13, Amount = _report.Sum(r => r.Amount) };
        }

        private BuyingTrendValue getYearTotal(int periodId)
        {
            var total = (from r in _report
                         where r.Period.Id == periodId
                         select r.Amount).Sum();

            return new BuyingTrendValue { Period = new DatePeriod { Id = periodId }, Month = 13, Amount = total };
        }

        private BuyingTrendValue getMonthTotal(int month)
        {
            var total = (from r in _report
                         where r.Month == month
                         select r.Amount).Sum();

            return new BuyingTrendValue { Month = month, Amount = total };
        }

        #endregion
    }
}
