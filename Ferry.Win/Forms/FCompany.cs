using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ferry.Win.Common;
using Ferry.Win.Properties;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.DataAccess;

namespace Ferry.Win.Forms
{
    public partial class FCompany : FFormBase
    {
        #region Internal Declarations

        private DataContext _dbc;
        private CompanyViewProcessor _cpvp;
        private FCompanyGroups _fCoGroups;

        #endregion

        #region Constructors

        public FCompany(FCompanyGroups fCoGroups)
        {
            InitializeComponent();
            _fCoGroups = fCoGroups;
        }

        #endregion

        #region Event Handlers

        private void FCompany_Load(object sender, EventArgs e)
        {
            try
            {
                setFormTitle();
                _dbc = Session.Dbc;
                _cpvp = new CompanyViewProcessor(lvwList);
                fillCompanyPeriods();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void FCompany_FormClosed(object sender, FormClosedEventArgs e)
        {
            _fCoGroups.Close();
        }

        private void btnAddPeriods_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                var fCoGroupPeriods = new FCompanyGroupPeriods(this, Session.CompanyGroup);
                fCoGroupPeriods.ShowDialog();
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

        private void btnAddPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                saveCompanyPeriod(createNewCompanyPeriod());
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

        private void btnEditPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                saveCompanyPeriod(_dbc.GetCompanyPeriodById(getSelectedCompanyPeriod().Id));
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

        private void btnDeletePeriod_Click(object sender, EventArgs e)
        {
            try
            {
                if (!shouldDeleteCompanyPeriod())
                    return;

                Cursor = Cursors.WaitCursor;
                _dbc.DeleteCompanyPeriod(getSelectedCompanyPeriod());
                fillCompanyPeriods();
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

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                importSelectedCompanyPeriods();
                refreshCompanyPeriodList();
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

        private void lvwList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            try
            {
                displayPeriodDataPath();
                setPeriodButtonsState();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void lvwList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                _dbc.ClearUnfinishedImports(getExecutingProcessesExpr());

                if (e.NewValue != CheckState.Checked)
                    return;

                if (!shouldImportCompanyPeriod(getSelectedCompanyPeriod(lvwList.Items[e.Index])))
                    e.NewValue = CheckState.Unchecked;
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void lvwList_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            try
            {
                setImportButtonState();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        #endregion

        #region Internal Methods

        private void setFormTitle()
        {
            Text = string.Format("{0} - {1}", Session.CompanyGroup.Name, Constants.AppName);
        }

        private void saveCompanyPeriod(CompanyPeriod cp)
        {
            var result = getCompanyPeriodInfo(cp);

            if (result == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;

                if (cp.IsNew())
                    _dbc.SaveCompanyPeriod(cp);
                else
                    _dbc.SaveCompanyPeriod(getSelectedCompanyPeriod(), cp);

                fillCompanyPeriods();
            }
        }

        private void refreshCompanyPeriodList()
        {
            fillCompanyPeriods();
        }

        private void fillCompanyPeriods()
        {
            _cpvp.LoadCompanyPeriods(readCompanyPeriods());
            setCompanyPeriodViewState();
        }

        private CompanyPeriod createNewCompanyPeriod()
        {
            var cp = new CompanyPeriod();
            cp.Period = new DatePeriod();
            return cp;
        }

        private DialogResult getCompanyPeriodInfo(CompanyPeriod cp)
        {
            var coPeriodForm = new FCompanyPeriod(_dbc, cp);
            coPeriodForm.Companies = _dbc.GetCompaniesByGroupId(Session.CompanyGroup);
            return coPeriodForm.ShowDialog();
        }

        private IEnumerable<CompanyPeriod> readCompanyPeriods()
        {
            var companyPeriods = _dbc.GetCompanyPeriods();
            return companyPeriods;
        }

        private void setCompanyPeriodViewState()
        {
            if (AreCompanyPeriodsExist())
            {
                Utilities.SelectListItem(lvwList, 0);
                AcceptButton = btnImport;
            }
            else
                disableCompanyPeriodButtons();
        }

        private bool AreCompanyPeriodsExist()
        {
            return lvwList.Items.Count > 0;
        }

        private CompanyPeriod getSelectedCompanyPeriod()
        {
            return getSelectedCompanyPeriod(lvwList.SelectedItems[0]);
        }

        private CompanyPeriod getSelectedCompanyPeriod(ListViewItem lvi)
        {
            return lvi.Tag as CompanyPeriod;
        }

        private bool shouldDeleteCompanyPeriod()
        {
            var cp = getSelectedCompanyPeriod();

            if (_dbc.IsCompanyPeriodImporting(cp))
            {
                Utilities.ShowMessage(Resources.CompanyPeriodAlreadyImportingCannotDelete);
                return false;
            }

            _dbc.CheckIsCompanyPeriodImported(cp);

            if (cp.IsImported)
                return Utilities.GetConfirmationYesNo(Resources.DeleteImportedCompanyPeriod) == DialogResult.Yes;

            return Utilities.GetConfirmationYesNo(Resources.DeleteCompanyPeriod) == DialogResult.Yes;
        }

        private void enableCompanyPeriodButtons()
        {
            btnEditPeriod.Enabled = true;
            btnDeletePeriod.Enabled = true;
            AcceptButton = btnImport;
        }

        private void disableCompanyPeriodButtons()
        {
            btnEditPeriod.Enabled = false;
            btnDeletePeriod.Enabled = false;
            AcceptButton = btnAddPeriod;
        }

        private bool shouldImportCompanyPeriod(CompanyPeriod cp)
        {
            if (_dbc.IsCompanyPeriodImporting(cp))
            {
                Utilities.ShowMessage(Resources.CompanyPeriodAlreadyImportingCannotImport);
                return false;
            }

            _dbc.CheckIsCompanyPeriodImported(cp);

            if (cp.IsImported)
                return Utilities.GetConfirmationYesNo(Resources.PeriodIsAlreadyImported) == DialogResult.Yes;

            return true;
        }

        private string getExecutingProcessesExpr()
        {
            var sb = new StringBuilder();
            foreach (var p in Utilities.GetExecutingProcesses())
                sb.Append("ProcessId <> ").Append(p.Id.ToString()).Append(" AND ");

            sb.Replace(" AND ", "", (sb.Length - 5), 5);
            return sb.ToString();
        }

        private void setPeriodButtonsState()
        {
            if (lvwList.SelectedItems.Count == 1)
                enableCompanyPeriodButtons();
            else
                disableCompanyPeriodButtons();
        }

        private void displayPeriodDataPath()
        {
            txtPeriodDataPath.Text = "";
            if (lvwList.SelectedItems.Count == 0) return;
            txtPeriodDataPath.Text = getSelectedCompanyPeriod().DataPath;
        }

        private void setImportButtonState()
        {
            btnImport.Enabled = lvwList.CheckedItems.Count > 0;
        }

        private void importSelectedCompanyPeriods()
        {
            var pi = new FImporter(getSelectedCompanyPeriods(), _dbc);
            pi.ShowDialog();
        }

        private IList<CompanyPeriod> getSelectedCompanyPeriods()
        {
            return (from ListViewItem lvi in lvwList.CheckedItems select getSelectedCompanyPeriod(lvi)).ToList();
        }

        internal void RefreshList()
        {
            fillCompanyPeriods();
        }

        internal void InitializeForm(bool isGroupChanged)
        {
            if(!isGroupChanged)
                return;

            Text = string.Format("{0} - {1}", Session.CompanyGroup.Name, Constants.AppName);
            lblTitle.Text = Session.CompanyGroup.Name;
        }

        #endregion
    }
}
