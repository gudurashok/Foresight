using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Ferry.Win.Common;
using Ferry.Win.Properties;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.DataAccess;

namespace Ferry.Win.Forms
{
    public partial class FCompanyGroups : FFormBase
    {
        #region Declarations

        private FLogin _login { get; set; }
        private CompanyGroup _companyGroup;
        private const int fudgeSize = 21;
        private FCompany fCompany { get; set; }

        #endregion

        #region Constructor

        public FCompanyGroups(FLogin login)
        {
            InitializeComponent();
            _login = login;
        }

        #endregion

        #region Event Handlers

        private void FCompanyGroups_Load(object sender, EventArgs e)
        {
            try
            {
                buildListColumns();
                fillCompanyGroups();
                applySecurityRights();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void FCompanyGroups_Activated(object sender, EventArgs e)
        {
            try
            {
                lvwList.Select();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void FCompanyGroups_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                _login.Close();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void FCompanyGroups_Resize(object sender, EventArgs e)
        {
            try
            {
                autoResizeList();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void lvwCompany_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            try
            {
                setCommandButtonStates();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void lvwList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                processOpen();
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

        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                _companyGroup = CompanyGroup.CreateNewGroup();
                initializeEntry();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                _companyGroup = getSelectedCompanyGroup();
                initializeEntry();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (Utilities.GetConfirmationYesNo(Resources.DeleteCompanyGroup) == DialogResult.No)
                    return;

                CoGroupDatabaseFactory.GetInstance().DeleteCompanyGroup(getSelectedCompanyGroup());
                _companyGroup = null;
                RefreshList();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                processOpen();
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

        #region Public Methods

        public void RefreshList()
        {
            fillCompanyGroups();
            selectCurrentCompanyGroup();
            setCommandButtonStates();
        }

        #endregion

        #region Internal Methods

        private void applySecurityRights()
        {
            var visible = (Session.Login.Role == UserRole.Admin);

            btnNew.Visible = visible;
            btnEdit.Visible = visible;
            btnDelete.Visible = visible;
        }

        private void setCommandButtonStates()
        {
            if (lvwList.SelectedItems.Count == 1)
                enableCommandButtons();
            else
                disableCommandButtons();
        }

        private void enableCommandButtons()
        {
            btnOpen.Enabled = true;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void disableCommandButtons()
        {
            btnOpen.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void fillCompanyGroups()
        {
            lvwList.Items.Clear();

            foreach (var group in getCompanyGroups())
                createListItem(group);

            Utilities.SelectListItem(lvwList, 0);
        }

        private IEnumerable<CompanyGroup> getCompanyGroups()
        {
            return ForesightDatabaseFactory.GetInstance().GetCompanyGroups();
        }

        private void createListItem(CompanyGroup @group)
        {
            var lvi = lvwList.Items.Add(group.Name);
            lvi.Tag = group;
        }

        private void buildListColumns()
        {
            lvwList.Columns.Add("Name");
            autoResizeList();
        }

        private void initializeEntry()
        {
            var entryForm = new FCompanyGroup(this, _companyGroup);
            entryForm.Show();
            Hide();
        }

        private void processOpen()
        {
            Cursor = Cursors.WaitCursor;
            var isGroupChanged = Session.CompanyGroup != getSelectedCompanyGroup();
            Session.OpenCompanyGroup(getSelectedCompanyGroup());
            showMainForm(isGroupChanged);
            Hide();
        }

        private void showMainForm(bool isGroupChanged)
        {
            if (fCompany == null)
                fCompany = new FCompany(this);

            fCompany.InitializeForm(isGroupChanged);
            fCompany.Show();
        }

        private CompanyGroup getSelectedCompanyGroup()
        {
            return (CompanyGroup)lvwList.SelectedItems[0].Tag;
        }

        private void selectCurrentCompanyGroup()
        {
            if (_companyGroup == null)
                return;

            var lvi = lvwList.FindItemWithText(_companyGroup.Name);
            if (lvi == null)
            {
                Utilities.SelectListItem(lvwList, 0, true);
            }
            else
            {
                lvi.Selected = true;
                lvi.EnsureVisible();
            }
        }

        private void autoResizeList()
        {
            lvwList.Columns[0].Width = lvwList.Width - fudgeSize;
        }

        #endregion
    }
}
