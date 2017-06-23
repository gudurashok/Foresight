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
    public partial class UTopNAccountWiseItemsReport : UReportBase
    {
        private IList<AccountItemValue> _report;

        #region Constructor

        public UTopNAccountWiseItemsReport(Command command)
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

        private void btnShow_Click(object sender, EventArgs e)
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
                if (lvwReport.SelectedItems[0].Tag == null)
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
            var idc = rdc as TopNAccountItemsBaseDataContext;
            if (idc == null)
                return;

            idc.ItemGrouping = chkItemGrouping.Checked;
            idc.TopNCount = Convert.ToInt32(nudTopNCount.Value);
            idc.SelectedCoPeriods = selectedCoPeriods;
            _report = rdc.GetReportData().Result as IList<AccountItemValue>;
        }

        private void buildReportViewColumns()
        {
            clearList();
            lvwReport.Columns.Add("Rank", 45);
            lvwReport.Columns.Add(getAccountColumnName(), 150);
            lvwReport.Columns.Add(getItemColumnName(), 150);
            lvwReport.Columns.Add("Total Amount", 100, HorizontalAlignment.Right);
        }

        private void clearList()
        {
            lvwReport.Columns.Clear();
            lvwReport.Items.Clear();
        }

        private string getAccountColumnName()
        {
            const string a = "Account";
            return chkPartyGrouping.Checked ? a + " (Groupwise)" : a;
        }

        private string getItemColumnName()
        {
            const string a = "Items";
            return chkItemGrouping.Checked ? a + " (Groupwise)" : a;
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
            foreach (var iv in _report)
            {
                var lvi = new ListViewItem(rank.ToString());
                lvi.Font = new Font(lvwReport.Font, FontStyle.Regular);
                lvi.Tag = iv;
                lvi.SubItems.Add(iv.AccountName);
                lvi.SubItems.Add(iv.ItemName);
                lvi.SubItems.Add(formatAmount(iv.Amount, cmbAmtFormat));
                lvwReport.Items.Add(lvi);
                rank++;
            }
        }

        private void addTotalsRow()
        {
            var lvi = new ListViewItem("");
            lvi.UseItemStyleForSubItems = true;
            lvi.ForeColor = Color.Maroon;
            lvi.SubItems.Add("");
            lvi.SubItems.Add("TOTAL:");
            lvi.SubItems.Add(formatAmount(_report.Sum(r => r.Amount), cmbAmtFormat));
            lvwReport.Items.Add(lvi);
        }

        private void showLedger()
        {
            var account = Session.Dbc.GetAccountById(((AccountItemValue)lvwReport.SelectedItems[0].Tag).AccountId);

            if (account == null)
                return;

            FReportViewer.ShowLedger(FindForm(), account);
        }

        #endregion
    }
}
