using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.DataAccess;
using ScalableApps.Foresight.Win.Common;

namespace ScalableApps.Foresight.Win.Forms
{
    public partial class FCompanyGroups : FFormBase
    {
        #region Declarations

        private FLogin _login { get; set; }
        private const int fudgeSize = 21;
        private FMain mainForm { get; set; }

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
            setCommandButtonStates();
        }

        #endregion

        #region Internal Methods

        private void setCommandButtonStates()
        {
            btnOpen.Enabled = lvwList.SelectedItems.Count == 1;
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

        private void processOpen()
        {
            Cursor = Cursors.WaitCursor;
            var isGroupChanged = Session.CompanyGroup != setSelectedCompanyGroup();
            Session.OpenCompanyGroup(setSelectedCompanyGroup());
            showMainForm(isGroupChanged);
            Hide();
        }

        private void showMainForm(bool isGroupChanged)
        {
            if (mainForm == null)
                mainForm = new FMain(this);

            mainForm.InitializeForm(isGroupChanged);
            mainForm.Show();
        }

        private CompanyGroup setSelectedCompanyGroup()
        {
            return (CompanyGroup)lvwList.SelectedItems[0].Tag;
        }

        private void autoResizeList()
        {
            lvwList.Columns[0].Width = lvwList.Width - fudgeSize;
        }

        #endregion
    }
}
