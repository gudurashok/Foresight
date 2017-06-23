using System;
using System.Configuration;
using System.Windows.Forms;
using Ferry.Win.Common;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Connection;

namespace Ferry.Win.Forms
{
    public partial class FServer : FFormBase
    {
        #region Contructors

        public FServer()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                validateData();
                Cursor.Current = Cursors.WaitCursor;
                testSqlConnection();
            }
            catch (Exception ex)
            {
                Utilities.ProcessException(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void chkUseSqlCredentials_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                grbSqlCredentials.Enabled = chkUseSqlCredentials.Checked;

                if (chkUseSqlCredentials.Checked)
                    txtUserId.Focus();

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
                validateData();
                Cursor.Current = Cursors.WaitCursor;
                saveConfig();
                Close();
            }
            catch (ValidationException ex)
            {
                Utilities.ShowMessage(ex.Message);
            }
            catch (Exception ex)
            {
                Utilities.ShowError(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region Internal Methods

        private void testSqlConnection()
        {
            try
            {
                Util.TestDatabaseConnection(connectionInfo());
                Utilities.ShowMessage("Test connection succeeded.");
            }
            catch (Exception ex)
            {
                throw new ValidationException(
                    string.Format("Cannot connect to server [{0}]\n\n {1}",
                                    txtServer.Text, ex.Message));
            }
        }

        private void validateData()
        {
            if (string.IsNullOrEmpty(txtServer.Text))
                throw new ValidationException("Server name cannot be empty");

            validateCredentials();
        }

        private void validateCredentials()
        {
            if (!chkUseSqlCredentials.Checked)
                return;

            if (string.IsNullOrEmpty(txtUserId.Text))
                throw new ValidationException("UserID cannot be empty");
        }

        private SqlServerConnectionInfo connectionInfo()
        {
            if (chkUseSqlCredentials.Checked)
                return new SqlServerConnectionInfo(txtServer.Text, "", txtUserId.Text, txtPassword.Text);

            return new SqlServerConnectionInfo(txtServer.Text, "");
        }

        private void saveConfig()
        {
            ConfigurationManager.AppSettings.Set("Lion", connectionInfo().GetConnectionString());
        }

        #endregion
    }
}
