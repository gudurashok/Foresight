using System;
using System.Windows.Forms;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.DataAccess;
using ScalableApps.Foresight.Win.Properties;
using ScalableApps.Foresight.Win.Common;

namespace ScalableApps.Foresight.Win.Forms
{
    public partial class FCreateCompanyGroup : FFormBase
    {
        #region Declaratrions

        private readonly FCompanyGroups _listForm;
        private readonly CompanyGroup _companyGroup;

        #endregion

        #region Constructor

        public FCreateCompanyGroup(FCompanyGroups listForm, CompanyGroup companyGroup)
        {
            InitializeComponent();
            _listForm = listForm;
            _companyGroup = companyGroup;
        }

        #endregion

        #region Event Handlers

        private void FCompanyGroup_Load(object sender, EventArgs e)
        {
            try
            {
                initializeForm();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void FCompanyGroup_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                closeForm();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void btnDataPath_Click(object sender, EventArgs e)
        {
            try
            {
                var dataPath = getDataPath();

                if (dataPath == "")
                    return;

                Cursor = Cursors.WaitCursor;
                processDataPath(dataPath);
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
                closeForm();
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
                saveCompanyGroup();
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

        private void initializeForm()
        {
            if (Util.GetGenus() == Genus.Lion)
            {
                txtDataPath.ReadOnly = false;
                btnDataPath.Visible = false;
            }

            fillForm();
        }

        private void fillForm()
        {
            txtCoGroup.Text = _companyGroup.Name;
            txtDataPath.Text = _companyGroup.FilePath;
            createDataContext();
        }

        private void createDataContext()
        {
            if (!_companyGroup.IsNew())
                Session.OpenCompanyGroup(_companyGroup);
        }

        private string getDataPath()
        {
            var ofd = new OpenFileDialog();
            ofd.Title = Resources.SelectOrEnterCompanyGroupDbFileName;
            ofd.DefaultExt = ".fsd";
            ofd.Filter = @"Foresight files (*.fsd)|*.fsd";
            if (ofd.ShowDialog(this) == DialogResult.OK)
                return ofd.FileName;

            return "";
        }

        private void processDataPath(string dataPath)
        {
            try
            {
                var group = CompanyGroup.CreateNewGroup();
                group.FilePath = dataPath;
                group = new DataContext(group).GetCompanyGroupById(1);
                group.FilePath = dataPath;
                txtDataPath.Text = dataPath;
                txtCoGroup.Text = group.Name;
            }
            catch (Exception)
            {
                throw new ValidationException("Not a Foresight database");
            }
        }

        private void saveCompanyGroup()
        {
            _companyGroup.Name = txtCoGroup.Text.Trim();
            _companyGroup.FilePath = txtDataPath.Text;
            var db = CoGroupDatabaseFactory.GetInstance();
            db.SaveCompanyGroup(_companyGroup);
            txtDataPath.Text = _companyGroup.FilePath;
            db.Close();
            createDataContext();
        }

        private void closeForm()
        {
            Hide();
            _listForm.RefreshList();
            _listForm.Show();
        }

        #endregion
    }
}
