using System;
using System.Linq;
using System.Windows.Forms;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.DataAccess;
using ScalableApps.Foresight.Win.Reports;
using System.Reflection;
using ScalableApps.Foresight.Win.Common;

namespace ScalableApps.Foresight.Win.Forms
{
    public partial class FMain : FFormBase
    {
        #region Declarations

        private FCompanyGroups companyGroupsForm { get; set; }

        #endregion

        #region Constructors

        public FMain(FCompanyGroups groupForm)
        {
            InitializeComponent();
            companyGroupsForm = groupForm;
        }

        #endregion

        #region Event Handlers

        private void FMain_Load(object sender, EventArgs e)
        {
            try
            {
                lblUserName.Text = Utilities.GetEnumDescription(Session.Login.Role);
                populateReportsCommandBar();
                Utilities.SelectListItem(lvwCommandBar, 0);
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void FMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (e.CloseReason != CloseReason.UserClosing)
                    return;

                e.Cancel = Utilities.GetConfirmationYesNo("Want to exit Foresight?") == DialogResult.No;
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void FMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                companyGroupsForm.Close();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void FMain_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                closeActiveTabIfControlF4(e);
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void lnkAbout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                new FAbout().ShowDialog(this);
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void updateLedgersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                new FLedgerUpdater(Session.Dbc.GetCompanyPeriods()).ShowDialog(this);
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void companyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                companyGroupsForm.Show();
                Hide();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void pnlCommandBar_Resize(object sender, EventArgs e)
        {
            try
            {
                lvwCommandBar.Columns[0].Width = lvwCommandBar.Width - 5;
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void lvwCommandBar_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                executeCommand();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        private void lvwCommandBar_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (lvwCommandBar.SelectedItems.Count == 0)
                    return;

                if (e.Modifiers != Keys.None)
                    return;

                if (e.KeyCode == Keys.Enter)
                    executeCommand();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
        }

        #endregion

        #region Public Methods

        public void InitializeForm(bool isGroupChanged)
        {
            if (isGroupChanged)
                tabMain.TabPages.Clear();

            setFormTitle();
            Utilities.SelectListItem(lvwCommandBar, 0, true);
        }

        #endregion

        #region Internal Methods

        private void populateReportsCommandBar()
        {
            var reports = ForesightDatabaseFactory.GetInstance().GetAllCommands();

            foreach (var report in (from r in reports where r.IsActive select r))
                lvwCommandBar.Items.Add(report.Name).Tag = report;
        }

        private void closeActiveTabIfControlF4(KeyEventArgs e)
        {
            if (e.KeyCode != Keys.F4 || tabMain.TabCount == 0 || !e.Control || e.Shift || e.Alt)
                return;

            tabMain.TabPages.Remove(tabMain.SelectedTab);
        }

        private void setFormTitle()
        {
            Text = string.Format("{0} - {1}", Session.CompanyGroup.Name, Constants.AppName);
            lblTitle.Text = Session.CompanyGroup.Name;
        }

        private Command getSelectedCommand()
        {
            return lvwCommandBar.SelectedItems[0].Tag as Command;
        }

        private void executeCommand()
        {
            var command = getSelectedCommand();

            if (command.Type == CommandType.Report)
            {
                if (isReportAlreadyOpened(command))
                    tabMain.SelectedTab = tabMain.TabPages[command.Nr.ToString()];
                else
                    showReport(createReportTabPage(command), getReportInstance(command));
            }
            else
            {
                var form = getUIInstanceOf(command);
                form.Show(this);
            }
        }

        private bool isReportAlreadyOpened(Command command)
        {
            return tabMain.TabPages.ContainsKey(command.Nr.ToString());
        }

        private TabPage createReportTabPage(Command command)
        {
            tabMain.TabPages.Add(command.Nr.ToString(), command.Name.PadRight(command.Name.Length + 10));
            return tabMain.TabPages[command.Nr.ToString()];
        }

        private void showReport(TabPage tab, UReportBase report)
        {
            tab.Controls.Add(report);
            report.BringToFront();
            report.Dock = DockStyle.Fill;
            report.Show();
            tabMain.SelectedTab = tab;
        }

        private UReportBase getReportInstance(Command command)
        {
            if (string.IsNullOrEmpty(command.UIControlName))
                throw new ValidationException("Under construction");

            return UReportBase.CreateInstance(command);
        }

        private Form getUIInstanceOf(Command command)
        {
            if (string.IsNullOrEmpty(command.UIControlName))
                throw new ValidationException("Under construction");

            var asm = Assembly.GetExecutingAssembly();
            return asm.CreateInstance("ScalableApps.Foresight.Win.Reports." + command.UIControlName, true) as Form;
        }

        #endregion
    }
}
