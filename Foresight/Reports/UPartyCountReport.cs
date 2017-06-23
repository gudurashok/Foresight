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
    public partial class UPartyCountReport : UReportBase
    {
        #region Internal Declarations

        private IList<NewLostPartyCount> _report;

        #endregion

        #region Constructor

        public UPartyCountReport(Command command)
            : base(command)
        {
            InitializeComponent();
            loadAmountFormatList();
        }

        #endregion

        #region Event Handlers

        private void UPartyCountReport_Load(object sender, EventArgs e)
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

        #region Private Methods

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
            var adc = rdc as PartyCountBaseDataContext;
            if (adc == null)
                return;

            adc.PartyGrouping = chkPartyGrouping.Checked;
            adc.SelectedCoPeriods = SelectedCoPeriods;
            _report = rdc.GetReportData().Result as IList<NewLostPartyCount>;
        }

        private void buildReportViewColumns()
        {
            clearList();
            lvwReport.Columns.Add("Year", 60);
            lvwReport.Columns.Add("Total Amount", 120, HorizontalAlignment.Right);
            lvwReport.Columns.Add(getColumnName(), 220, HorizontalAlignment.Right);
        }

        private string getColumnName()
        {
            const string a = "No. of Customers";
            return chkPartyGrouping.Checked ? a + " (Groupwise)" : a;
        }

        private void clearList()
        {
            lvwReport.Columns.Clear();
            lvwReport.Items.Clear();
        }

        private void renderReport()
        {
            buildReportViewColumns();
            addReportViewRows();
            addTotalsRow();
        }

        private void addReportViewRows()
        {
            foreach (var p in _report.OrderByDescending(r => r.CompanyPeriod.Period.FinancialTo.Year))
            {
                var lvi = new ListViewItem(p.CompanyPeriod.Period.FinancialTo.Year.ToString());
                lvi.Font = new Font(lvwReport.Font, FontStyle.Regular);
                lvi.SubItems.Add(formatAmount(p.Amount, cmbAmtFormat));
                lvi.SubItems.Add(p.Count.ToString());
                lvwReport.Items.Add(lvi);
            }
        }

        private void addTotalsRow()
        {
            var lvi = new ListViewItem("TOTAL:");
            lvi.UseItemStyleForSubItems = true;
            lvi.ForeColor = Color.Maroon;
            lvi.SubItems.Add(formatAmount(_report.Sum(r => r.Amount), cmbAmtFormat));
            lvi.SubItems.Add(_report.Sum(r => r.Count).ToString());
            lvwReport.Items.Add(lvi);
        }

        #endregion
    }
}
