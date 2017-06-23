using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Ferry.Logic.Base;
using Ferry.Logic.Common;
using Ferry.Win.Common;
using Ferry.Win.Properties;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.DataAccess;

namespace Ferry.Win.Forms
{
    public partial class FImporter : FFormBase
    {
        #region Internal Declarations

        private StopWatch _timer;
        private readonly DataContext _dbc;
        private DataImportContext _idc;
        private IList<CompanyPeriod> _companyPeriods { get; set; }
        private bool _isImportInProgress;
        private CompanyViewProcessor _cpvp;
        private bool _isImportSuccess = true;

        #endregion

        #region Constructor

        public FImporter(IList<CompanyPeriod> companyPeriods, DataContext dbc)
        {
            InitializeComponent();
            _companyPeriods = companyPeriods;
            _dbc = dbc;
            lblTimeElapsed.Text = "";
        }

        #endregion

        #region Event Handlers

        private void FImporter_Load(object sender, EventArgs e)
        {
            try
            {
                fillCompanyPeriods();
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
                var lvsi = lvwList.SelectedItems[0].SubItems[0];
                if (lvsi.Tag == null)
                    return;

                Utilities.ShowError(lvsi.Tag as Exception, true);
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Timer elapsedTimeTimer = null;

            try
            {
                elapsedTimeTimer = new Timer();
                elapsedTimeTimer.Interval = 1000;
                elapsedTimeTimer.Enabled = true;
                elapsedTimeTimer.Tick += elapsedTimeTimer_Tick;
                startImport();
                AcceptButton = btnOK;
            }
            catch (ImportAbortException)
            {
                abortedImport();
            }
            finally
            {
                if (elapsedTimeTimer != null)
                    elapsedTimeTimer.Tick -= elapsedTimeTimer_Tick;

                Cursor = Cursors.Default;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_isImportInProgress)
                {
                    cancelImport();
                    Close();
                    return;
                }

                if (Utilities.GetConfirmationYesNo(Resources.AreYouSureToAbortImport) != DialogResult.Yes)
                    return;

                cancelImport();
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
                Close();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        #endregion

        #region Internal Methods

        private void startImport()
        {
            Cursor = Cursors.WaitCursor;
            _timer = new StopWatch();
            setPreImportState();
            performImport();
            _timer.Stop();
            showPostImportInfo();
        }

        void elapsedTimeTimer_Tick(object sender, EventArgs e)
        {
            var ts = DateTime.Now.Subtract(_timer.StartTime);
            lblTimeElapsed.Text = string.Format("Time elapsed: {0}", getTimeSpanDisplayText(ts));
        }

        private string getTimeSpanDisplayText(TimeSpan ts)
        {
            return string.Format("{0}:{1}:{2} hrs",
                                    ts.Hours.ToString("00"),
                                    ts.Minutes.ToString("00"),
                                    ts.Seconds.ToString("00"));
        }

        private void performImport()
        {
            foreach (ListViewItem lvi in lvwList.Items)
            {
                try
                {
                    var cp = lvi.Tag as CompanyPeriod;
                    lblCompany.Text = getTitleText(cp);
                    setImportInProgress(lvi);
                    performImport(cp);
                    setImportSuccess(lvi);
                }
                catch (Exception ex)
                {
                    _dbc.UpdateIsCompanyPeriodImporting(lvi.Tag as CompanyPeriod, false, 0);
                    _isImportSuccess = false;

                    if (ex is ImportAbortException)
                        throw;

                    Utilities.LogError(ex);
                    setImportFailed(lvi, ex);
                }
            }
        }

        private void setImportInProgress(ListViewItem lvi)
        {
            lvi.ForeColor = Color.Blue;
            lvi.Font = new Font(lvwList.Font, FontStyle.Italic);
        }

        private void setImportSuccess(ListViewItem lvi)
        {
            lvi.ForeColor = Color.DarkGreen;
            lvi.Font = new Font(lvwList.Font, FontStyle.Bold);
        }

        private void setImportFailed(ListViewItem lvi, Exception ex)
        {
            lvi.ForeColor = Color.Red;
            lvi.Font = new Font(lvwList.Font, FontStyle.Strikeout);
            lvi.SubItems[0].Tag = ex;
        }

        private string getTitleText(CompanyPeriod cp)
        {
            return string.Format("{0}: {1}", cp.Company.Name, cp.Period.Name);
        }

        private void setPreImportState()
        {
            Text = Resources.EtlInProgress;
            lblStartTime.Text = string.Format(Resources.ImportStartedAt, DateTime.Now.ToLongTimeString());
            picWaitIndicator.Visible = true;
            picWaitIndicator.Refresh();
            btnStart.Visible = false;
            pnlProgress.Visible = true;
            pnlStart.Visible = false;
            btnStart.Visible = false;
            lblProgress.Text = Resources.StartingImporting;
            btnCancel.Cursor = Cursors.Hand;
            pnlProgress.Refresh();
            _isImportInProgress = true;
        }

        private void performImport(CompanyPeriod cp)
        {
            _dbc.UpdateIsCompanyPeriodImporting(cp, true, Utilities.GetCurrentProcessId());
            _idc = DataImporterFactory.GetDataImporter(cp);
            _idc.Importer.Importing += Importer_Importing;
            _idc.PerformImport();
            _dbc.UpdateIsCompanyPeriodImporting(cp, false, 0);
            _idc.Importer.Importing -= Importer_Importing;
        }

        private void Importer_Importing(object sender, ImportingEventArgs e)
        {
            showProgress(e.CurrentItem);
        }

        private void showProgress(string currentItem)
        {
            Application.DoEvents();
            setLabelText(lblProgress, currentItem);
        }

        private void showPostImportInfo()
        {
            displayPostImportStatusMessage();
            setLabelText(lblProgress, "");
            _isImportInProgress = false;
            picWaitIndicator.Visible = false;
            btnCancel.Visible = false;
            btnOK.Visible = true;
        }

        private void displayPostImportStatusMessage()
        {
            if (_isImportSuccess)
            {
                lblStatus.Text = string.Format(Resources.ImportDataSuccessfull);
                lblStatus.ForeColor = Color.Blue;
            }
            else
            {
                lblStatus.Text = string.Format(Resources.ImportDataFailed);
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void abortedImport()
        {
            setLabelText(lblStatus, Resources.ImportAborted);
            setLabelText(lblProgress, "");
            Close();
        }

        private void cancelImport()
        {
            if (_isImportInProgress)
            {
                _idc.Importer.Importing -= Importer_Importing;
                setLabelText(lblProgress, "");
                setLabelText(lblStatus, Resources.AbortingImport);
                Application.DoEvents();
                _idc.Importer.CancelImport();
            }
        }

        private void setLabelText(Label lbl, string text)
        {
            lbl.Text = text;
            lbl.Refresh();
        }

        private void fillCompanyPeriods()
        {
            _cpvp = new CompanyViewProcessor(lvwList, true);
            _cpvp.LoadCompanyPeriods(_companyPeriods);
        }

        #endregion
    }
}
