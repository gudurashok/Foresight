using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Win.Common;

namespace ScalableApps.Foresight.Win.Reports
{
    public partial class FCompanyPeriods : Form
    {
        #region Internal Declarations

        private IList<CompanyPeriod> _selectedCoPeriods;
        private readonly CompanyPeriodType _listType;

        #endregion
        
        #region Constructor

        public FCompanyPeriods(IList<CompanyPeriod> selectedCoPeriods)
            : this(selectedCoPeriods, CompanyPeriodType.Both)
        {
        }

        public FCompanyPeriods(IList<CompanyPeriod> selectedCoPeriods, CompanyPeriodType listType)
        {
            InitializeComponent();
            _selectedCoPeriods = selectedCoPeriods;
            _listType = listType;
        }

        #endregion

        #region Event Handlers

        private void FCompanyPeriods_Load(object sender, EventArgs e)
        {
            try
            {
                buildColumns();
                fillListItems(readCompanyPeriods());
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void FCompanyPeriods_Activated(object sender, EventArgs e)
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

        private void FCompanyPeriods_Resize(object sender, EventArgs e)
        {
            try
            {
                listAutoResize();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void FCompanyPeriods_FormClosing(object sender, FormClosingEventArgs e)
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
                projectSelectedItems();
                DialogResult = DialogResult.OK;
                Hide();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void btnInvertSelection_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ListViewItem lvi in lvw.Items)
                    lvi.Checked = !lvi.Checked;
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void FCompanyPeriods_KeyDown(object sender, KeyEventArgs e)
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

        #endregion

        #region Private Methods

        private void buildColumns()
        {
            lvw.Columns.Clear();

            if (showCompany())
                lvw.Columns.Add("Company", 110);

            if (showPeriod())
                lvw.Columns.Add("Period", 65);

            listAutoResize();
        }

        private bool showPeriod()
        {
            return (_listType & CompanyPeriodType.Period) == CompanyPeriodType.Period;
        }

        private bool showCompany()
        {
            return (_listType & CompanyPeriodType.Company) == CompanyPeriodType.Company;
        }

        private void listAutoResize()
        {
            var colTotalWidth = 0;

            if (_listType == CompanyPeriodType.Both)
                colTotalWidth += 65;

            var findForm = lvw.FindForm();
            if (findForm != null) lvw.Columns[0].Width = findForm.Width - colTotalWidth - 35;
        }

        private IList<CompanyPeriod> readCompanyPeriods()
        {
            var companyPeriods = Session.Dbc.GetCompanyPeriods(true);
            return companyPeriods;
        }

        private void fillListItems(IList<CompanyPeriod> listItems)
        {
            lvw.Items.Clear();

            if (_listType == CompanyPeriodType.Both)
            {
                foreach (var cp in listItems)
                    lvw.Items.Add(createListItem(cp));

                return;
            }

            if (_listType == CompanyPeriodType.Company)
            {
                foreach (var cp in listItems.Select(c => c.Company.Id)
                                            .Distinct()
                                            .Select(companyId => listItems.First(c => c.Company.Id == companyId)))
                    lvw.Items.Add(createListItem(cp));

                return;
            }

            if (_listType == CompanyPeriodType.Period)
                foreach (var cp in listItems.Select(c => c.Period.Id)
                                            .Distinct()
                                            .Select(periodId => listItems.First(c => c.Period.Id == periodId)))
                    lvw.Items.Add(createListItem(cp));
        }

        private ListViewItem createListItem(CompanyPeriod cp)
        {
            var lvi = new ListViewItem();
            lvi.Tag = cp;

            if (showCompany())
                lvi.Text = cp.Company.Name;

            if (showPeriod())
                if (showCompany())
                    lvi.SubItems.Add(cp.Period.Name);
                else
                    lvi.Text = cp.Period.FinancialTo.Year.ToString();

            lvi.Checked = shouldCheck(cp);
            return lvi;
        }

        private bool shouldCheck(CompanyPeriod cp)
        {
            return _selectedCoPeriods.SingleOrDefault(scp => scp.Id == cp.Id) != null;
        }

        private void projectSelectedItems()
        {
            _selectedCoPeriods = new List<CompanyPeriod>();

            for (var index = 0; index < lvw.CheckedItems.Count; index++)
                _selectedCoPeriods.Add(lvw.CheckedItems[index].Tag as CompanyPeriod);
        }

        #endregion

        #region Public Methods

        public IList<CompanyPeriod> GetSelectedCoPeriods()
        {
            return _selectedCoPeriods;
        }

        #endregion
    }
}
