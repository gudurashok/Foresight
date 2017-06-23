using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Report;
using ScalableApps.Foresight.Win.Common;

namespace ScalableApps.Foresight.Win.Reports
{
    public partial class UDaybookListReport : UReportBase
    {
        #region Declarations

        private IList<Daybook> _report;
        private ListViewItem _selectedAccountItem;
        private const int fudgeSize = 21;
        private bool _isAscending = true;

        #endregion

        #region Properties

        public IList<Daybook> SelectedDaybooks { get; set; }
        public bool MultiSelect { get; set; }

        #endregion

        #region Constructor

        public UDaybookListReport(Command command)
            : base(command)
        {
            InitializeComponent();
            SelectedDaybooks = new List<Daybook>();
            buildReportViewColumns();
        }

        #endregion

        #region Event Handlers

        private void UAccountListReport_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                lvwReport.Resize -= lvwReport_Resize;
                lvwReport.CheckBoxes = MultiSelect;
                btnClose.Visible = !(Parent is Form);
                showReport();
                btnShowLedger.Enabled = lvwReport.Items.Count > 0;
                lvwReport.Focus();
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

        private void lvwReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnShowLedger.Enabled = lvwReport.SelectedItems.Count > 0;
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData != Keys.Enter)
                return base.ProcessCmdKey(ref msg, keyData);

            try
            {
                showLedger();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }

            return true;
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

        private void btnShowLedger_Click(object sender, EventArgs e)
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

        private bool sortDirection()
        {
            _isAscending = !_isAscending;
            return _isAscending;
        }

        private void showReport()
        {
            getReportData();
            renderReport();
        }

        private void getReportData()
        {
            var adc = rdc as AccountDataContext;
            if (adc == null)
                return;

            adc.OnlyDaybooks = true;
            _report = adc.GetReportData().Result as IList<Daybook>;
        }

        private void buildReportViewColumns()
        {
            lvwReport.Columns.Clear();
            lvwReport.Items.Clear();

            lvwReport.Columns.Clear();
            lvwReport.Columns.Add("Name", 280);
            autoResize();
        }

        private void autoResize()
        {
            lvwReport.Columns[0].Width = lvwReport.Width - fudgeSize;
        }

        private void renderReport()
        {
            lvwReport.Items.Clear();
            addReportViewRows();
        }

        private void addReportViewRows()
        {
            foreach (var daybook in _report)
                lvwReport.Items.Add(createListItem(daybook));

            Utilities.SelectListItem(lvwReport, 0, true);
        }

        private ListViewItem createListItem(Daybook daybook)
        {
            var lvi = new ListViewItem();
            lvi.Font = new Font(lvwReport.Font, FontStyle.Regular);
            lvi.Tag = daybook;
            lvi.Text = daybook.Name;

            if (MultiSelect)
                lvi.Checked = shouldCheck(daybook);
            else if (_selectedAccountItem == null && shouldCheck(daybook))
                _selectedAccountItem = lvi;

            return lvi;
        }

        private bool shouldCheck(Daybook daybook)
        {
            return SelectedDaybooks.SingleOrDefault(g => g.Id == daybook.Id) != null;
        }

        private void showLedger()
        {
            var daybook = (lvwReport.SelectedItems[0].Tag as Daybook);

            if (daybook.Account == null)
                FReportViewer.ShowLedger(FindForm(), daybook);
            else
                FReportViewer.ShowLedger(FindForm(), daybook.Account);
        }

        #endregion
    }
}
