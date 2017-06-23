using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Report;
using ScalableApps.Foresight.Win.Common;

namespace ScalableApps.Foresight.Win.Reports
{
    public partial class UYearlyTopperReport : UReportBase
    {
        #region Declarations

        private const int periodColumnIndex = 0;
        private const int accountNameColumnIndex = 1;
        private const int totalAmountColumnIndex = 2;
        private const int pctColumnIndex = 3;
        private const int fudgeSize = 21;
        private IList<CompanyPeriodTopperValue> _report;

        #endregion

        #region Constructor

        public UYearlyTopperReport(Command command)
            : base(command)
        {
            InitializeComponent();
            buildReportViewColumns();
            loadAmountFormatList();
        }

        #endregion

        #region Event Handlers

        private void UYearlyTopperReport_Load(object sender, EventArgs e)
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
                if (!selectCompanyPeriods(CompanyPeriodType.Both))
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
            var adc = rdc as CompanyTopperBaseDataContext;
            if (adc == null)
                return;

            adc.TopNCount = Convert.ToInt32(nudTopNCount.Value);
            adc.SelectedCoPeriods = SelectedCoPeriods;
            _report = rdc.GetReportData().Result as IList<CompanyPeriodTopperValue>;

        }

        private void buildReportViewColumns()
        {
            clearList();
            lvwReport.Columns.Add("Year", 55);
            lvwReport.Columns.Add("Name", 150);
            lvwReport.Columns.Add("Total Amount", 100, HorizontalAlignment.Right);
            lvwReport.Columns.Add("Total %", 70, HorizontalAlignment.Right);

            autoResize();
        }

        private void clearList()
        {
            lvwReport.Columns.Clear();
            lvwReport.Items.Clear();
        }

        private void autoResize()
        {
            lvwReport.Columns[accountNameColumnIndex].Width =
                    lvwReport.Width -
                    (lvwReport.Columns[periodColumnIndex].Width +
                     lvwReport.Columns[totalAmountColumnIndex].Width +
                     lvwReport.Columns[pctColumnIndex].Width +
                     fudgeSize);
        }

        private void renderReport()
        {
            lvwReport.Items.Clear();
            addReportViewRows();
        }

        private void addReportViewRows()
        {
            foreach (var v in _report)
            {
                var lvi = new ListViewItem(v.CompanyPeriod.Period.FinancialTo.Year.ToString());
                lvi.SubItems.Add(v.Account.Name);
                var value = v.Account.Amount ?? 0;
                lvi.SubItems.Add(formatAmount(value, cmbAmtFormat));
                lvi.SubItems.Add(v.Account.Percentage.ToString("0.00"));
                lvwReport.Items.Add(lvi);
            }
        }

        #endregion
    }
}
