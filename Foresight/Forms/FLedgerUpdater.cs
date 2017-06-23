using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Import;
using ScalableApps.Foresight.Win.Properties;
using ScalableApps.Foresight.Win.Common;

namespace ScalableApps.Foresight.Win.Forms
{
    public partial class FLedgerUpdater : FFormBase
    {
        #region Internal Declarations

        private StopWatch _timer;
        private LedgerBuilder _lb;
        private IList<CompanyPeriod> _companyPeriods { get; set; }
        private bool _isUpdateInProgress;
        private CompanyViewProcessor _cpvp;
        private bool _isUpdateSuccess = true;

        #endregion

        #region Constructor

        public FLedgerUpdater(IList<CompanyPeriod> companyPeriods)
        {
            InitializeComponent();
            _companyPeriods = companyPeriods;
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
                startUpdating();
                AcceptButton = btnOK;
            }
            catch (LedgerUpdateAbortException)
            {
                abortedUpdation();
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
                if (!_isUpdateInProgress)
                {
                    cancelUpdation();
                    Close();
                    return;
                }

                if (Utilities.GetConfirmationYesNo(Resources.AreYouSureToAbortUpdation) != DialogResult.Yes)
                    return;

                cancelUpdation();
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

        private void startUpdating()
        {
            Cursor = Cursors.WaitCursor;
            _timer = new StopWatch();
            setPreUpdateState();
            performUpdate();
            _timer.Stop();
            showPostUpdateInfo();
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

        private void performUpdate()
        {
            foreach (ListViewItem lvi in lvwList.Items)
            {
                try
                {
                    var cp = lvi.Tag as CompanyPeriod;
                    lblCompany.Text = getTitleText(cp);
                    setUpdateInProgress(lvi);
                    performUpdate(cp);
                    setUpdationSuccess(lvi);
                }
                catch (Exception ex)
                {
                    _isUpdateSuccess = false;

                    if (ex is LedgerUpdateAbortException)
                        throw;

                    Utilities.LogError(ex);
                    setUpdationFailed(lvi, ex);
                }
            }
        }

        private void setUpdateInProgress(ListViewItem lvi)
        {
            lvi.ForeColor = Color.Blue;
            lvi.Font = new Font(lvwList.Font, FontStyle.Italic);
        }

        private void setUpdationSuccess(ListViewItem lvi)
        {
            lvi.ForeColor = Color.DarkGreen;
            lvi.Font = new Font(lvwList.Font, FontStyle.Bold);
        }

        private void setUpdationFailed(ListViewItem lvi, Exception ex)
        {
            lvi.ForeColor = Color.Red;
            lvi.Font = new Font(lvwList.Font, FontStyle.Strikeout);
            lvi.SubItems[0].Tag = ex;
        }

        private string getTitleText(CompanyPeriod cp)
        {
            return string.Format("{0}: {1}", cp.Company.Name, cp.Period.Name);
        }

        private void setPreUpdateState()
        {
            Text = Resources.UpdateInProgress;
            lblStartTime.Text = string.Format(Resources.UpdateStartedAt, DateTime.Now.ToLongTimeString());
            picWaitIndicator.Visible = true;
            picWaitIndicator.Refresh();
            btnStart.Visible = false;
            pnlProgress.Visible = true;
            pnlStart.Visible = false;
            btnStart.Visible = false;
            lblProgress.Text = Resources.StartingUpdating;
            btnCancel.Cursor = Cursors.Hand;
            pnlProgress.Refresh();
            _isUpdateInProgress = true;
        }

        private void performUpdate(CompanyPeriod cp)
        {
            _lb = new LedgerBuilder(Session.Dbc, cp);
            _lb.Updating += Updater_Updating;
            _lb.BuildDimensionTables();
            _lb.Updating -= Updater_Updating;
        }

        private void Updater_Updating(object sender, UpdatingEventArgs e)
        {
            showProgress(e.CurrentItem);
        }

        private void showProgress(string currentItem)
        {
            Application.DoEvents();
            setLabelText(lblProgress, currentItem);
        }

        private void showPostUpdateInfo()
        {
            displayPostUpdateStatusMessage();
            setLabelText(lblProgress, "");
            _isUpdateInProgress = false;
            picWaitIndicator.Visible = false;
            btnCancel.Visible = false;
            btnOK.Visible = true;
        }

        private void displayPostUpdateStatusMessage()
        {
            if (_isUpdateSuccess)
            {
                lblStatus.Text = string.Format(Resources.UpdateDataSuccessfull);
                lblStatus.ForeColor = Color.Blue;
            }
            else
            {
                lblStatus.Text = string.Format(Resources.UpdateDataFailed);
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void abortedUpdation()
        {
            setLabelText(lblStatus, Resources.UpdateAborted);
            setLabelText(lblProgress, "");
            Close();
        }

        private void cancelUpdation()
        {
            if (_isUpdateInProgress)
            {
                _lb.Updating -= Updater_Updating;
                setLabelText(lblProgress, "");
                setLabelText(lblStatus, Resources.AbortingUpdation);
                Application.DoEvents();
                _lb.CancelUpdation();
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
