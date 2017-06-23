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
    public partial class UAccountListReport : UReportBase
    {
        #region Declarations

        private IList<TrialBalance> _report;
        private ListViewItem _selectedAccountItem;
        private const int fudgeSize = 21;
        private bool _isAscending = true;
        private const int accountNameColumnIndex = 0;
        private const int openingCreditColumnIndex = 1;
        private const int openingDebitColumnIndex = 2;
        private const int txnCreditColumnIndex = 3;
        private const int txnDebitColumnIndex = 4;
        private const int closingCreditColumnIndex = 5;
        private const int closingDebitColumnIndex = 6;

        #endregion

        #region Properties

        public IList<int> SelectedAccountIds { get; set; }
        public bool MultiSelect { private get; set; }
        public AccountTypes AccountTypes { private get; set; }

        #endregion

        #region Constructor

        public UAccountListReport(Command command)
            : base(command)
        {
            InitializeComponent();
            SelectedAccountIds = new List<int>();
            loadAmountFormatList();
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
                btnInvertSelection.Visible = MultiSelect;
                btnClose.Visible = !(Parent is Form);
                showReport();
                btnShowLedger.Enabled = false;
                btnShowLedger.Focus();
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

                var selectedIndex = 0;
                if (lvwReport.SelectedIndices.Count > 0)
                    selectedIndex = lvwReport.SelectedIndices[0];

                addReportViewRows();
                Utilities.SelectListItem(lvwReport, selectedIndex, true);
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

        private void chkPartyGrouping_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lvwReport.Resize -= lvwReport_Resize;
                showReport();
                lvwReport.Resize += lvwReport_Resize;
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

        private void lvwReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnShowLedger.Enabled = (lvwReport.SelectedItems.Count > 0 &&
                                        lvwReport.SelectedItems[0].Tag as TrialBalance != null);
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
                if (!lvwReport.CheckBoxes)
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
                if (!lvwReport.CheckBoxes)
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
                Cursor = Cursors.WaitCursor;

                if (lvwReport.SelectedItems[0].Tag == null)
                    return;

                showLedger();
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

        private void btnInvertSelection_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                foreach (ListViewItem lvi in lvwReport.Items)
                    lvi.Checked = !lvi.Checked;
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

        private bool sortDirection()
        {
            _isAscending = !_isAscending;
            return _isAscending;
        }

        private void autoResize()
        {
            lvwReport.Columns[accountNameColumnIndex].Width =
                                         lvwReport.Width - lvwReport.Columns[openingCreditColumnIndex].Width
                                         - lvwReport.Columns[openingDebitColumnIndex].Width
                                         - lvwReport.Columns[txnCreditColumnIndex].Width
                                         - lvwReport.Columns[txnDebitColumnIndex].Width
                                         - lvwReport.Columns[closingCreditColumnIndex].Width
                                         - lvwReport.Columns[openingDebitColumnIndex].Width
                                         - fudgeSize;
        }

        private void showReport()
        {
            getReportData();
            renderReport();
        }

        private void getReportData()
        {
            var adc = rdc as AccountDataContext;
            rdc.PartyGrouping = chkPartyGrouping.Checked;
            if (adc == null)
                return;

            _report = adc.GetReportData().Result as IList<TrialBalance>;
        }

        private void renderReport()
        {
            buildReportViewColumns();
            addReportViewRows();
        }

        private void buildReportViewColumns()
        {
            lvwReport.Columns.Clear();
            lvwReport.Columns.Add("Account Name", 150);
            lvwReport.Columns.Add("Opening Credit", 100, HorizontalAlignment.Right);
            lvwReport.Columns.Add("Opening Debit", 100, HorizontalAlignment.Right);
            lvwReport.Columns.Add("Txn Credit", 100, HorizontalAlignment.Right);
            lvwReport.Columns.Add("Txn Debit", 100, HorizontalAlignment.Right);
            lvwReport.Columns.Add("Closing Credit", 100, HorizontalAlignment.Right);
            lvwReport.Columns.Add("Closing Debit", 100, HorizontalAlignment.Right);

            createChartOfAccountGroups();
        }

        private void createChartOfAccountGroups()
        {
            foreach (var coaName in _report.Select(tb => tb.ChartOfAccountName).Distinct())
            {
                var group = lvwReport.Groups.Add(coaName, coaName);
                group.Name = coaName;
                lvwReport.Groups.Add(group);
            }
        }

        private void addTotalChartOfAccountGroups(ListViewGroup group)
        {
            var openingCr = _report.Where(r => r.ChartOfAccountName == group.Name && r.Opening > 0).Sum(r => r.Opening);
            var openingDb = Math.Abs(_report.Where(r => r.ChartOfAccountName == group.Name && r.Opening < 0).Sum(r => r.Opening));
            var transCr = _report.Where(r => r.ChartOfAccountName == group.Name).Sum(r => r.TransactionCredit);
            var transDb = Math.Abs(_report.Where(r => r.ChartOfAccountName == group.Name).Sum(r => r.TransactionDebit));
            var closingCr = _report.Where(r => r.ChartOfAccountName == group.Name && r.Closing > 0).Sum(r => r.Closing);
            var closingDb = Math.Abs(_report.Where(r => r.ChartOfAccountName == group.Name && r.Closing < 0).Sum(r => r.Closing));

            var lvi = new ListViewItem("TOTAL:", group);
            lvi.SubItems.Add(formatAmountWithBlank(openingCr, cmbAmtFormat));
            lvi.SubItems.Add(formatAmountWithBlank(openingDb, cmbAmtFormat));
            lvi.SubItems.Add(formatAmountWithBlank(transCr, cmbAmtFormat));
            lvi.SubItems.Add(formatAmountWithBlank(transDb, cmbAmtFormat));
            lvi.SubItems.Add(formatAmountWithBlank(closingCr, cmbAmtFormat));
            lvi.SubItems.Add(formatAmountWithBlank(closingDb, cmbAmtFormat));

            lvwReport.Items.Add(lvi);
        }

        private void addReportViewRows()
        {
            lvwReport.Items.Clear();

            foreach (ListViewGroup group in lvwReport.Groups)
            {
                addReportViewRows(group, _report.Where(r => r.ChartOfAccountName == group.Name));
                addTotalChartOfAccountGroups(group);
            }
        }

        private void addReportViewRows(ListViewGroup group, IEnumerable<TrialBalance> data)
        {
            foreach (var trialBalance in data)
            {
                var lvi = new ListViewItem(trialBalance.AccountName, group);
                lvi.Font = new Font(lvwReport.Font, FontStyle.Regular);
                lvi.Tag = trialBalance;

                addOpeningBalanceSubItems(trialBalance, lvi);

                addTransactionSubItems(trialBalance, lvi);

                addClosingBalanceSubItems(trialBalance, lvi);

                lvwReport.Items.Add(lvi);
            }
        }

        private void addTransactionSubItems(TrialBalance trialBalance, ListViewItem lvi)
        {
            lvi.SubItems.Add(formatAmountWithBlank(Math.Abs(trialBalance.TransactionCredit), cmbAmtFormat));
            lvi.SubItems.Add(formatAmountWithBlank(Math.Abs(trialBalance.TransactionDebit), cmbAmtFormat));
        }

        private void addClosingBalanceSubItems(TrialBalance trialBalance, ListViewItem lvi)
        {
            if (trialBalance.Closing >= 0)
            {
                lvi.SubItems.Add(formatAmountWithBlank(Math.Abs(trialBalance.Closing), cmbAmtFormat));
                lvi.SubItems.Add("");
            }
            else
            {
                lvi.SubItems.Add("");
                lvi.SubItems.Add(formatAmountWithBlank(Math.Abs(trialBalance.Closing), cmbAmtFormat));
            }
        }

        private void addOpeningBalanceSubItems(TrialBalance trialBalance, ListViewItem lvi)
        {
            if (trialBalance.Opening >= 0)
            {
                lvi.SubItems.Add(formatAmountWithBlank(Math.Abs(trialBalance.Opening), cmbAmtFormat));
                lvi.SubItems.Add("");
            }
            else
            {
                lvi.SubItems.Add("");
                lvi.SubItems.Add(formatAmountWithBlank(Math.Abs(trialBalance.Opening), cmbAmtFormat));
            }
        }

        private ListViewItem createListItem(Account account)
        {
            var lvi = new ListViewItem();
            lvi.Font = new Font(lvwReport.Font, FontStyle.Regular);
            lvi.Tag = account;
            lvi.Text = account.Name;

            if (MultiSelect)
                lvi.Checked = shouldCheck(account);
            else if (_selectedAccountItem == null && shouldCheck(account))
                _selectedAccountItem = lvi;

            return lvi;
        }

        private bool shouldCheck(Account account)
        {
            return SelectedAccountIds.Contains(account.Id);
        }

        private void populateSelectedItems()
        {
            for (var index = 0; index < lvwReport.CheckedItems.Count; index++)
                SelectedAccountIds.Add((lvwReport.CheckedItems[index].Tag as TrialBalance).AccountId);
        }

        private void populateSelectedItem()
        {
            if (lvwReport.SelectedItems.Count == 0)
                return;

            SelectedAccountIds.Add((lvwReport.CheckedItems[0].Tag as TrialBalance).AccountId);
        }


        private void loadAmountFormatList()
        {
            Utilities.LoadEnumListItems(cmbAmtFormat, typeof(ReportsAmountFormat),
                                    (int)ReportsAmountFormat.Thousands);
        }
        private void showLedger()
        {
            var trialBalance = lvwReport.SelectedItems[0].Tag as TrialBalance;
            if (trialBalance == null)
                return;

            var account = Session.Dbc.GetAccountById(trialBalance.AccountId);

            FReportViewer.ShowLedger(FindForm(), account);
        }

        #endregion

        #region Public Methods

        public void ShowPartyGroups()
        {
            chkPartyGrouping.Visible = true;
        }

        public bool IsPartyGroupingSelected()
        {
            return chkPartyGrouping.Checked;
        }

        public void ProjectSelectedAccounts()
        {
            SelectedAccountIds = new List<int>();

            if (MultiSelect)
                populateSelectedItems();
            else
                populateSelectedItem();
        }

        #endregion
    }
}
