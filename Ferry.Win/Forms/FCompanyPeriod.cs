using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Ferry.Win.Common;
using Ferry.Win.Properties;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.DataAccess;
using SourceCompanyDbContext = Ferry.Logic.Base.SourceCompanyDbContext;
using SourceCompanyDbcFactory = Ferry.Logic.Base.SourceCompanyDbcFactory;

namespace Ferry.Win.Forms
{
    public partial class FCompanyPeriod : FFormBase
    {
        #region Declarations

        private readonly DataContext _dbc;
        private readonly CompanyPeriod _cp;
        private CompanyPeriod _selectedCp;
        private string _folderPath;
        public IList<Company> Companies { private get; set; }

        #endregion

        #region Constructors

        public FCompanyPeriod(DataContext dbc, CompanyPeriod cp)
        {
            InitializeComponent();
            _dbc = dbc;
            _cp = cp;
            _selectedCp = cp;
        }

        #endregion

        #region Event Handlers

        private void FCompanyPeriod_Load(object sender, EventArgs e)
        {
            try
            {
                fillCompanies();
                btnDataSourceBrowser.Enabled = _cp.IsNew();

                if (!_cp.IsNew())
                    fillForm(_cp);
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
                deleteCompany();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void btnDataSourceBrowser_Click(object sender, EventArgs e)
        {
            try
            {
                _folderPath = getDataSourcePath();

                if (_folderPath == "")
                    return;

                processProviderDataPath(getProviderType());
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (!isDataValid())
                    return;

                fillObject();
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        #endregion

        #region Internal Methods

        private void fillCompanies()
        {
            cmbCompany.DataSource = Companies;
        }

        private SourceDataProvider getProviderType()
        {
            if (isProviderFilesExists(SourceCompanyDbContext.GetProviderCoPeriodDataFileNames(SourceDataProvider.Easy)))
                return SourceDataProvider.Easy;

            if (isProviderFilesExists(SourceCompanyDbContext.GetProviderCoPeriodDataFileNames(SourceDataProvider.Mcs)) &&
                SourceCompanyDbContext.IsGlGroupFileExists(SourceDataProvider.Mcs, _folderPath))
                return SourceDataProvider.Mcs;

            if (isProviderFilesExists(SourceCompanyDbContext.GetProviderCoPeriodDataFileNames(SourceDataProvider.Tcs)) &&
                SourceCompanyDbContext.IsGlGroupFileExists(SourceDataProvider.Tcs, _folderPath))
                return SourceDataProvider.Tcs;

            throw new ValidationException(Resources.ImportProviderFilesNotFound);
        }

        private bool isProviderFilesExists(IEnumerable<string> files)
        {
            return !(from file in files let di = new DirectoryInfo(_folderPath) where di.GetFiles(file).Length == 0 select file).Any();
        }

        private void processProviderDataPath(SourceDataProvider provider)
        {
            var sdbc = SourceCompanyDbcFactory.GetInstance(provider, _folderPath);
            _selectedCp = sdbc.GetCompanyPeriod();

            fillForm(_selectedCp);
        }

        private string getDataSourcePath()
        {
            var fbd = new FolderBrowserDialog();
            fbd.Description = Resources.SelectImportCompanyDataFolder;
            fbd.SelectedPath = _folderPath;
            
            if (fbd.ShowDialog(this) == DialogResult.OK)
                return fbd.SelectedPath;

            return "";
        }

        private bool isDataValid()
        {
            if (string.IsNullOrEmpty(txtDataPath.Text))
            {
                Utilities.ShowMessage(Resources.SelectDataPath);
                return false;
            }

            return true;
        }

        private void fillForm(CompanyPeriod coPeriod)
        {
            txtDataPath.Text = coPeriod.DataPath;
            cmbCompany.Text = coPeriod.Company.Name;
            lblDatePeriod.Text = coPeriod.Period.ToString();
            lblProvider.Text = Utilities.GetEnumDescription(coPeriod.SourceDataProvider);
        }

        private void fillObject()
        {
            _cp.Company = fillCompany();

            if (!_cp.IsNew())
                return;

            _cp.DataPath = txtDataPath.Text;
            _cp.Period = _dbc.GetDatePeriodByFinPeriod(_selectedCp.Period);
            _cp.SourceDataProvider = _selectedCp.SourceDataProvider;
        }

        private Company fillCompany()
        {
            var enteredCompanyName = cmbCompany.Text.Trim();
            findEnteredCompanyName();

            if (cmbCompany.SelectedIndex == -1)
                return new Company
                           {
                               Code = _selectedCp.Company.Code,
                               Name = enteredCompanyName,
                               Group = Session.CompanyGroup
                           };

            return cmbCompany.SelectedItem as Company;
        }

        private void findEnteredCompanyName()
        {
            cmbCompany.SelectedIndex = cmbCompany.FindStringExact(cmbCompany.Text.Trim(), -1);
        }

        private void deleteCompany()
        {
            if (cmbCompany.SelectedIndex == -1)
                cmbCompany.Text = "";
            else
            {
                _dbc.DeleteCompany(cmbCompany.SelectedItem as Company);
                Companies.Remove(cmbCompany.SelectedItem as Company);
                clearCompanies();
                fillCompanies();
            }
        }

        private void clearCompanies()
        {
            cmbCompany.DataSource = null;
        }

        #endregion
    }
}
