using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ScalableApps.Foresight.Logic.Business;

namespace ScalableApps.Foresight.Win.Common
{
    public class CompanyViewProcessor
    {
        #region Internal Declarations

        private const int companyNameCoulmnIndex = 0;
        private const int companyPeriodCoulmnIndex = 1;
        private const int dataPathCoulmnIndex = 2;
        private const int dataProviderCoulmnIndex = 3;
        private const int isImportedCoulmnIndex = 4;
        private const int fudgeSize = 21;
        private readonly ListView _lvw;
        private CheckState _checkState;
        private readonly bool _forImportor;

        #endregion

        #region Constructors

        public CompanyViewProcessor(ListView lvw)
            : this(lvw, false)
        {
        }

        public CompanyViewProcessor(ListView lvw, bool forImportor)
        {
            _lvw = lvw;
            _forImportor = forImportor;
            _lvw.Resize += lvw_Resize;
            buildColumns();
            autoResizeColumn();
        }

        #endregion

        #region Event Handler

        private void lvw_Resize(object sender, EventArgs e)
        {
            autoResizeColumn();
        }

        #endregion

        #region Internal Methods

        private void buildColumns()
        {
            _lvw.Columns.Add("Company", 100);
            _lvw.Columns.Add("Period", 75);

            if (!_forImportor)
            {
                _lvw.Columns.Add("Data Path", 131);
                _lvw.Columns.Add("Provider", 55);
                _lvw.Columns.Add("Imported", 55);
            }
        }

        private int remainingColumnsWidth()
        {
            var result = _lvw.Columns[companyPeriodCoulmnIndex].Width;

            if (!_forImportor)
            {
                result +=
                _lvw.Columns[dataPathCoulmnIndex].Width +
                _lvw.Columns[dataProviderCoulmnIndex].Width +
                _lvw.Columns[isImportedCoulmnIndex].Width;
            }

            return result;
        }

        public void LoadCompanyPeriods(IEnumerable<CompanyPeriod> companyPeriods)
        {
            LoadCompanyPeriods(companyPeriods, CheckState.Unchecked);
        }

        public void LoadCompanyPeriods(IEnumerable<CompanyPeriod> companyPeriods, CheckState checkState)
        {
            _checkState = checkState;
            _lvw.Items.Clear();
            fillCompanyPeriods(companyPeriods);
        }

        private void fillCompanyPeriods(IEnumerable<CompanyPeriod> companyPeriods)
        {
            foreach (var cp in companyPeriods)
                _lvw.Items.Add(createCompanyPeriodListItem(cp));
        }

        private ListViewItem createCompanyPeriodListItem(CompanyPeriod cp)
        {
            var lvi = new ListViewItem();
            lvi.Checked = _checkState == CheckState.Checked;
            lvi.Tag = cp;
            lvi.Text = cp.Company.Name;
            lvi.SubItems.Add(cp.Period.Name);

            if (!_forImportor)
            {
                lvi.SubItems.Add(cp.DataPath);
                lvi.SubItems.Add(cp.SourceDataProvider.ToString());
                lvi.SubItems.Add(formatIsImportedValue(cp.IsImported));
            }

            return lvi;
        }

        private string formatIsImportedValue(bool value)
        {
            return value ? "Yes" : "No";
        } 

        #endregion

        #region Public Methods

        private void autoResizeColumn()
        {
            _lvw.Columns[companyNameCoulmnIndex].Width =
                    _lvw.Width - remainingColumnsWidth() - fudgeSize;
        }
 
        #endregion
    }
}
