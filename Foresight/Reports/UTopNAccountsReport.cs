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
    public partial class UTopNAccountsReport : UReportBase
    {
        #region Declarations

        private IList<AccountValue> _report;
        private const int rankColumnIndex = 0;
        private const int accountNameColumnIndex = 1;
        private const int totalAmountColumnIndex = 2;
        private const int pctColumnIndex = 3;
        private const int fudgeSize = 21;

        #endregion

        #region Constructor

        public UTopNAccountsReport(Command command)
            : base(command)
        {
            InitializeComponent();
            loadAmountFormatList();
        }

        #endregion

        #region Event Handlers

        private void UTopNAccountsReport_Load(object sender, EventArgs e)
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
                lvwReport.Resize -= lvwReport_Resize;
                if (!selectCompanyPeriods(CompanyPeriodType.Company))
                    return;

                Cursor = Cursors.WaitCursor;
                showReport();
                lvwReport.Resize += lvwReport_Resize;
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
                lvwReport.Resize -= lvwReport_Resize;
                Cursor = Cursors.WaitCursor;
                showReport();
                lvwReport.Resize += lvwReport_Resize;
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
                lvwReport.Resize -= lvwReport_Resize;
                Cursor = Cursors.WaitCursor;
                SelectedCoPeriods = new List<CompanyPeriod>();
                showReport();
                lvwReport.Resize += lvwReport_Resize;
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

        private void lvwReport_Resize(object sender, EventArgs e)
        {
            try
            {
                if (lvwReport.Columns.Count > 0)
                    autoResize();
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

        #region Private Methods

        private void showLedger()
        {
            var accountValue = lvwReport.SelectedItems[0].Tag as AccountValue;
            
            if(accountValue == null)
                return;

            var account = Session.Dbc.GetAccountById(accountValue.Id);

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
            getReportData(SelectedCoPeriods);
            renderReport();
        }

        private void getReportData(IList<CompanyPeriod> selectedCoPeriods)
        {
            var adc = rdc as TopNAccountsBaseDataContext;
            if (adc == null)
                return;

            adc.PartyGrouping = chkPartyGrouping.Checked;
            adc.TopNCount = Convert.ToInt32(nudTopNCount.Value);
            adc.SelectedCoPeriods = selectedCoPeriods;
            _report = rdc.GetReportData().Result as IList<AccountValue>;
        }

        private void buildReportViewColumns()
        {
            clearList();
            lvwReport.Columns.Add("Rank", 45);
            lvwReport.Columns.Add(getColumnName(), 150);
            lvwReport.Columns.Add("Total Amount", 100, HorizontalAlignment.Right);
            lvwReport.Columns.Add("Total %", 70, HorizontalAlignment.Right);

            autoResize();
        }

        private void clearList()
        {
            lvwReport.Columns.Clear();
            lvwReport.Items.Clear();
        }

        private string getColumnName()
        {
            const string a = "Name";
            return chkPartyGrouping.Checked ? a + " (Groupwise)" : a;
        }

        private void autoResize()
        {
            lvwReport.Columns[accountNameColumnIndex].Width =
                    lvwReport.Width -
                    (lvwReport.Columns[rankColumnIndex].Width +
                     lvwReport.Columns[totalAmountColumnIndex].Width +
                     lvwReport.Columns[pctColumnIndex].Width +
                     fudgeSize);
        }

        private void renderReport()
        {
            buildReportViewColumns();
            addReportViewRows();
            addTotalsRow();
        }

        private void addReportViewRows()
        {
            var rank = 1;
            foreach (var av in _report)
            {
                var lvi = new ListViewItem(rank.ToString());
                lvi.Font = new Font(lvwReport.Font, FontStyle.Regular);
                lvi.Tag = av;
                lvi.SubItems.Add(av.Name);
                var value = av.Amount ?? 0;
                lvi.SubItems.Add(formatAmount(value, cmbAmtFormat));
                lvi.SubItems.Add(av.Percentage.ToString("0.00"));
                lvwReport.Items.Add(lvi);
                rank++;
            }
        }

        private void addTotalsRow()
        {
            var lvi = new ListViewItem("");
            lvi.UseItemStyleForSubItems = true;
            lvi.ForeColor = Color.Maroon;
            lvi.SubItems.Add("TOTAL:");
            lvi.SubItems.Add(formatAmount(_report.Sum(r => r.Amount ?? 0), cmbAmtFormat));
            lvi.SubItems.Add(_report.Sum(r => r.Percentage).ToString("0.00"));
            lvwReport.Items.Add(lvi);
        }

        #endregion
    }
}
