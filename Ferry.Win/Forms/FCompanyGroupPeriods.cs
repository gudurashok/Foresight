using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Ferry.Logic.Base;
using Ferry.Win.Common;
using Ferry.Win.Properties;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using System.IO;

namespace Ferry.Win.Forms
{
    public partial class FCompanyGroupPeriods : FFormBase
    {
        #region Declaratrions

        private IList<CompanyPeriod> _companyPeriods;
        private const int fudgeSize = 21;
        private readonly CompanyGroup _companyGroup;
        private IList<CompanyGroup> _companyGroups;
        private string _folderPath;
        private SourceCompanyDbContext sdbc;
        private readonly FCompany _listForm;
        #endregion

        #region Constructor

        public FCompanyGroupPeriods(FCompany listForm, CompanyGroup companyGroup)
        {
            InitializeComponent();
            _listForm = listForm;
            _companyGroup = companyGroup;
        }

        #endregion

        #region Event Handlers

        private void FCompanyGroup_Resize(object sender, EventArgs e)
        {
            try
            {
                autoResizeGroupList();
                autoResizeCoList();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void btnImportFrom_Click(object sender, EventArgs e)
        {
            try
            {
                _folderPath = getImportFromPath();

                if (_folderPath == "")
                    return;

                Cursor = Cursors.WaitCursor;
                processProviderDataPath(getProviderType());
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

        private void lvwCoGroups_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (e.Item.Checked)
                    loadCompanyPeriodsOfGroup(e.Item.Tag as CompanyGroup);

                fillSelectedGroupPeriods();
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

        private void loadCompanyPeriodsOfGroup(CompanyGroup coGroup)
        {
            var result = sdbc.GetCompanyPeriodsFor(coGroup);
            if (result.Count == 0)
                return;

            _companyPeriods = result;
        }

        private void lvwCoPeriods_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                if (e.NewValue == CheckState.Unchecked)
                    return;

                validateCheckedCompanyPeriod(e);
            }
            catch (Exception ex)
            {
                e.NewValue = CheckState.Unchecked;
                Utilities.ProcessException(ex);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                _listForm.RefreshList();
                Close();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                saveSelectedCompanyPeriods();
                clearSavedCompanyPeriods();
                Utilities.ShowMessage(Resources.CoGroupSavedSuccessfully);
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

        #region Internal Methods

        private string getImportFromPath()
        {
            var fpd = new FolderBrowserDialog();
            fpd.Description = Resources.SelectImportDataFolder;
            fpd.SelectedPath = _folderPath;

            if (fpd.ShowDialog(this) == DialogResult.OK)
                return fpd.SelectedPath;

            return "";
        }

        private SourceDataProvider getProviderType()
        {
            if (isProviderFilesExists(SourceCompanyDbContext.GetProviderCompanyFileNames(SourceDataProvider.Easy)))
                return SourceDataProvider.Easy;

            if (isProviderFilesExists(SourceCompanyDbContext.GetProviderCompanyFileNames(SourceDataProvider.Mcs)))
                return SourceDataProvider.Mcs;

            if (isProviderFilesExists(SourceCompanyDbContext.GetProviderCompanyFileNames(SourceDataProvider.Tcs)))
                return SourceDataProvider.Tcs;

            throw new ValidationException(Resources.ImportProviderFilesNotFound);
        }

        private bool isProviderFilesExists(IEnumerable<string> files)
        {
            return files.All(file => File.Exists(_folderPath + @"\" + file));
        }

        private void autoResizeCoList()
        {
            if (lvwCoPeriods.Columns.Count == 0)
                return;

            lvwCoPeriods.Columns[0].Width = ClientSize.Width -
                                    (lvwCoPeriods.Columns[1].Width + fudgeSize);
        }

        private void autoResizeGroupList()
        {
            if (lvwCoGroups.Columns.Count == 0)
                return;

            lvwCoGroups.Columns[0].Width = ClientSize.Width - fudgeSize;
        }

        private void processProviderDataPath(SourceDataProvider provider)
        {
            sdbc = SourceCompanyDbcFactory.GetInstance(provider, _folderPath);
            _companyGroups = sdbc.GetAllCompanyGroups();

            if (_companyGroups.Count == 0)
                return;

            createCoGroupListColumns();
            fillCompanyGroups();
            createCoPeriodListColumns();
            displaySourceProviderInfo(provider);
            _companyPeriods = new List<CompanyPeriod>();
        }

        private void displaySourceProviderInfo(SourceDataProvider provider)
        {
            txtImportFrom.Text = _folderPath;
            lblProviderName.Text = Utilities.GetEnumDescription(provider);
        }

        private void fillCompanyGroups()
        {
            lvwCoGroups.Items.Clear();

            foreach (var group in _companyGroups)
            {
                var lvi = lvwCoGroups.Items.Add(group.Name);
                lvi.Tag = group;
            }

            Utilities.SelectListItem(lvwCoGroups, 0);
        }

        private void createCoGroupListColumns()
        {
            lvwCoGroups.Columns.Clear();
            lvwCoGroups.Columns.Add("Company group name");
            autoResizeGroupList();
        }

        private void createCoPeriodListColumns()
        {
            lvwCoPeriods.Columns.Clear();
            lvwCoPeriods.Columns.Add("Company name", 219);
            lvwCoPeriods.Columns.Add("Date period", 100);
        }

        private string getSelectedGroupCode(ListViewItem lvi)
        {
            return ((CompanyGroup)lvi.Tag).Code;
        }

        private void saveSelectedCompanyPeriods()
        {
            foreach (var cp in (from ListViewItem lvi in lvwCoPeriods.CheckedItems select lvi.Tag).OfType<CompanyPeriod>())
            {
                _companyGroup.Code = cp.Company.Group.Code;
                cp.Company.Group = _companyGroup;
                var period = Session.Dbc.GetDatePeriodByFinPeriod(cp.Period);
                if (period == null)
                    Session.Dbc.AddDatePeriod(cp.Period);
                else
                    cp.Period = period;

                Session.Dbc.SaveCompanyPeriod(cp);
            }
        }

        private void clearSavedCompanyPeriods()
        {
            foreach (ListViewItem lvi in lvwCoPeriods.CheckedItems)
            {
                lvi.Checked = false;
                disableCompanyPeriod(lvi, Color.DarkGray);
            }
        }

        private void fillSelectedGroupPeriods()
        {
            var selectedPeriods = getCurrentSelectedCompanies();
            fillSelectedGroupCompanies();
            recheckSelectedPeriods(selectedPeriods);
        }

        private void recheckSelectedPeriods(IList<ListViewItem> selectedPeriods)
        {
            foreach (ListViewItem lvi in lvwCoPeriods.Items)
                lvi.Checked = shouldCheckCoPeriodItem(selectedPeriods, lvi);
        }

        private bool shouldCheckCoPeriodItem(IEnumerable<ListViewItem> selectedPeriods, ListViewItem lvi)
        {
            return selectedPeriods.Any(selectedItem => selectedItem.Text == lvi.Text
                            && selectedItem.SubItems[1].Text == lvi.SubItems[1].Text);
        }

        private IList<ListViewItem> getCurrentSelectedCompanies()
        {
            return lvwCoPeriods.CheckedItems.Cast<ListViewItem>().ToList();
        }

        private void fillSelectedGroupCompanies()
        {
            lvwCoPeriods.Items.Clear();

            foreach (ListViewItem lvi in lvwCoGroups.CheckedItems)
                fillCompanyPeriods((from cp in _companyPeriods
                                    where cp.Company.Group.Code == getSelectedGroupCode(lvi)
                                    select cp).ToList());
        }

        private void fillCompanyPeriods(IEnumerable<CompanyPeriod> periods)
        {
            foreach (var cp in periods)
            {
                findCompanyPeriodId(cp);
                var lvi = lvwCoPeriods.Items.Add(cp.Company.Name);
                lvi.Tag = cp;
                lvi.SubItems.Add(cp.Period.GetNameFromFinancialPeriod());
                if (!cp.IsNew()) disableCompanyPeriod(lvi, Color.DarkGray);
                if (!hasDataFolder(cp)) disableCompanyPeriod(lvi, Color.LightGray);
            }
        }

        private void findCompanyPeriodId(CompanyPeriod cp)
        {
            if (_companyGroup.IsNew() || !cp.IsNew())
                return;

            cp.Id = Session.Dbc.GetCompanyPeriodByNameAndFinPeriod(cp.Company.Name, cp.Period);
        }

        private void disableCompanyPeriod(ListViewItem lvi, Color backColor)
        {
            lvi.UseItemStyleForSubItems = false;
            lvi.BackColor = backColor;
            lvi.ForeColor = Color.White;

            foreach (ListViewItem.ListViewSubItem lvsi in lvi.SubItems)
            {
                lvsi.BackColor = backColor;
                lvsi.ForeColor = Color.White;
            }
        }

        private bool hasDataFolder(CompanyPeriod cp)
        {
            return Directory.Exists(cp.DataPath);
        }

        private void validateCheckedCompanyPeriod(ItemCheckEventArgs e)
        {
            var cp = lvwCoPeriods.Items[e.Index].Tag as CompanyPeriod;

            if (cp != null && !cp.IsNew())
                throw new ValidationException("This company period is already taken. Cannot add again");

            if (cp != null && !Directory.Exists(cp.DataPath))
                throw new ValidationException(string.Format("Company data folder {0} not found", cp.DataPath));
        }

        #endregion
    }
}
