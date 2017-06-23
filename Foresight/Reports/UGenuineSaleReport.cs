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
    public partial class UGenuineSaleReport : UReportBase
    {
        #region Internal Declarations

        private IList<GenuineSale> _report;
        private const int rankColumnIndex = 0;
        private const int customerNameColumnIndex = 1;
        private const int saleAmountColumnIndex = 2;
        private const int receiptAmtColumnIndex = 3;
        private const int balanceAmtColumnIndex = 4;
        private const int genSalePctColumnIndex = 5;
        private const int fudgeSize = 21;
        private bool _isAscending = true;

        #endregion

        #region Constructor

        public UGenuineSaleReport(Command command)
            : base(command)
        {
            InitializeComponent();
            loadAmountFormatList();
        }

        #endregion

        #region Event Handlers

        private void UGenuineSaleReport_Load(object sender, EventArgs e)
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

        private void lvwReport_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                lvwReport.ListViewItemSorter = new ListViewItemComparer(e.Column, sortDirection());
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

        private bool sortDirection()
        {
            _isAscending = !_isAscending;
            return _isAscending;
        }

        private void showLedger()
        {
            var genuineSale = lvwReport.SelectedItems[0].Tag as GenuineSale;
            if (genuineSale == null)
                return;
            var account = Session.Dbc.GetAccountById(genuineSale.AccountId);

            if (account == null)
                return;

            FReportViewer.ShowLedger(FindForm(), account);
        }

        private void loadAmountFormatList()
        {
            Utilities.LoadEnumListItems(cmbAmtFormat, typeof(ReportsAmountFormat),
                                    (int)ReportsAmountFormat.Thousands);
        }

        private void showReport()
        {
            getReportData();
            renderReport();
        }

        private void getReportData()
        {
            var sdc = rdc as GenuineSaleDataContext;
            if (sdc == null)
                return;

            sdc.PartyGrouping = chkPartyGrouping.Checked;
            _report = sdc.GetReportData().Result as IList<GenuineSale>;
        }

        private void buildReportViewColumns()
        {
            lvwReport.Columns.Clear();
            lvwReport.Items.Clear();

            lvwReport.Columns.Add("Rank", 50, HorizontalAlignment.Right);
            lvwReport.Columns.Add(getColumnName(), 90, HorizontalAlignment.Left);
            lvwReport.Columns.Add("Sale", 90, HorizontalAlignment.Right);
            lvwReport.Columns.Add("Receipt", 90, HorizontalAlignment.Right);
            lvwReport.Columns.Add("Balance", 90, HorizontalAlignment.Right);
            lvwReport.Columns.Add("Genuine Sale %", 70, HorizontalAlignment.Right);
            autoResize();
        }

        private string getColumnName()
        {
            const string a = "Name";
            return chkPartyGrouping.Checked ? a + " (Groupwise)" : a;
        }

        private void autoResize()
        {
            lvwReport.Columns[customerNameColumnIndex].Width =
                    lvwReport.Width -
                    (lvwReport.Columns[rankColumnIndex].Width +
                     lvwReport.Columns[saleAmountColumnIndex].Width +
                     lvwReport.Columns[receiptAmtColumnIndex].Width +
                     lvwReport.Columns[balanceAmtColumnIndex].Width +
                     lvwReport.Columns[genSalePctColumnIndex].Width +
                     fudgeSize);
        }

        private void renderReport()
        {
            buildReportViewColumns();
            addReportViewRows();
        }

        private void addReportViewRows()
        {
            var rank = 1;
            foreach (var gsr in _report.OrderBy(r => r.GenuineSalePct))
            {
                var lvi = new ListViewItem(rank.ToString());
                lvi.Font = new Font(lvwReport.Font, FontStyle.Regular);
                lvi.Tag = gsr;
                lvi.SubItems.Add(gsr.Name);
                lvi.SubItems.Add(formatAmount(gsr.SaleAmount, cmbAmtFormat));
                lvi.SubItems.Add(formatAmount(gsr.ReceiptAmount, cmbAmtFormat));
                lvi.SubItems.Add(formatAmount(gsr.BalanceAmount, cmbAmtFormat, true));
                lvi.SubItems.Add(gsr.GenuineSalePct.ToString(Constants.AmountFormat));
                lvwReport.Items.Add(lvi);
                rank++;
            }
        }

        #endregion

    }
}
