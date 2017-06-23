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
    public partial class UFindAccountsReport : UReportBase
    {
        #region Internal Declarations

        private IList<NewLostPartyValue> _report;
        private const int rankColumnIndex = 0;
        private const int customerNameColumnIndex = 1;
        private const int firstDateColumnIndex = 2;
        private const int lastDateColumnIndex = 3;
        private const int totalAmountColumnIndex = 4;
        private const int transCountColumnIndex = 5;
        private const int fudgeSize = 21;

        #endregion

        #region Constructor

        public UFindAccountsReport(Command command)
            : base(command)
        {
            InitializeComponent();
            loadAmountFormatList();
        }

        #endregion

        #region Event Handlers

        private void UFindAccountsReport_Load(object sender, EventArgs e)
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
            var newLostPartyValue = lvwReport.SelectedItems[0].Tag as NewLostPartyValue;
            if (newLostPartyValue == null)
                return;
            var account = Session.Dbc.GetAccountById(newLostPartyValue.GroupId);

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
            var adc = rdc as PartyAssociationDataContext;
            rdc.PartyGrouping = chkPartyGrouping.Checked;
            if (adc == null)
                return;

            adc.TopNCount = Convert.ToInt32(nudTopNCount.Value);
            adc.DaysSince = Convert.ToInt32(nudSince.Value);
            _report = rdc.GetReportData().Result as IList<NewLostPartyValue>;
        }

        private void buildReportViewColumns()
        {
            lvwReport.Columns.Clear();
            lvwReport.Items.Clear();

            lvwReport.Columns.Add("Rank", 50);
            lvwReport.Columns.Add(getColumnName(), 150);
            lvwReport.Columns.Add("First Date", 80);
            lvwReport.Columns.Add("Last Date", 80);
            lvwReport.Columns.Add("Total Amount", 100, HorizontalAlignment.Right);
            lvwReport.Columns.Add("Txn Count", 80, HorizontalAlignment.Right);
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
                     lvwReport.Columns[firstDateColumnIndex].Width +
                     lvwReport.Columns[lastDateColumnIndex].Width +
                     lvwReport.Columns[totalAmountColumnIndex].Width +
                     lvwReport.Columns[transCountColumnIndex].Width +
                     fudgeSize);
        }

        private void renderReport()
        {
            buildReportViewColumns();
            addReportViewRows();
            addTotalsRow();
        }

        private void addTotalsRow()
        {
            var lvi = new ListViewItem("");
            lvi.UseItemStyleForSubItems = true;
            lvi.ForeColor = Color.Maroon;
            lvi.SubItems.Add("TOTAL:");
            lvi.SubItems.Add("");
            lvi.SubItems.Add("");
            lvi.SubItems.Add(formatAmount(_report.Sum(t => t.Amount), cmbAmtFormat));
            lvi.SubItems.Add(_report.Sum(t => t.TransCount).ToString());
            lvwReport.Items.Add(lvi);
        }

        private void addReportViewRows()
        {
            var rank = 1;
            foreach (var lc in _report)
            {
                var lvi = new ListViewItem(rank.ToString());
                lvi.Font = new Font(lvwReport.Font, FontStyle.Regular);
                lvi.Tag = lc;
                lvi.SubItems.Add(lc.Name);
                lvi.SubItems.Add(lc.FirstDate.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(lc.LastDate.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(formatAmount(lc.Amount, cmbAmtFormat));
                lvi.SubItems.Add(lc.TransCount.ToString());

                lvwReport.Items.Add(lvi);
                rank++;
            }
        }

        #endregion

    }
}
