using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.DataAccess;
using ScalableApps.Foresight.Win.Common;

namespace ScalableApps.Foresight.Win.Reports
{
    public partial class FAccounts : Form
    {
        #region Declarations

        private UAccountListReport _list;
        public bool IsPartyGroupingSelected { get; private set; }

        #endregion

        #region Constructor

        public FAccounts()
            : this(new List<int>()) { }

        public FAccounts(IList<int> selectedAccountIds)
            : this(selectedAccountIds, false) { }

        public FAccounts(IList<int> selectedAccountIds, bool multiSelect)
            : this(selectedAccountIds, multiSelect, AccountTypes.All) { }

        public FAccounts(IList<int> selectedAccountIds,
            bool multiSelect, AccountTypes accountTypes)
            : this(selectedAccountIds, multiSelect, accountTypes, false) { }

        public FAccounts(IList<int> selectedAccountIds , bool multiSelect,
                         AccountTypes accountTypes, bool partyGrouping)
        {
            InitializeComponent();
            loadAccountList();

            _list.SelectedAccountIds = selectedAccountIds;
            _list.MultiSelect = multiSelect;
            _list.AccountTypes = accountTypes;
            if (partyGrouping)
                _list.ShowPartyGroups();
        }

        #endregion

        #region Event Handlers

        private void FAccounts_Activated(object sender, EventArgs e)
        {
            try
            {
                DialogResult = DialogResult.None;
            }

            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void FAccounts_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                    Close();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void FAccounts_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (e.CloseReason == CloseReason.UserClosing)
                    DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                processListSelection();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                _list.SelectedAccountIds = new List<int>();
                DialogResult = DialogResult.OK;
                Hide();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        #endregion

        #region Public Methods

        public IList<int> GetSelectedAccountIds()
        {
            return _list.SelectedAccountIds;
        }

        #endregion

        #region Private Methods

        private void loadAccountList()
        {
            var command = ForesightDatabaseFactory.GetInstance()
                                .GetCommandByNr(Constants.LedgerReportNr);
            _list = new UAccountListReport(command);
            Controls.Add(_list);
            _list.BringToFront();
            _list.Dock = DockStyle.Fill;
            _list.Show();
        }

        private void processListSelection()
        {
            _list.ProjectSelectedAccounts();
            IsPartyGroupingSelected = _list.IsPartyGroupingSelected();

            DialogResult = _list.SelectedAccountIds.Count == 0 ? DialogResult.Cancel : DialogResult.OK;
            Hide();
        }

        #endregion
    }
}
