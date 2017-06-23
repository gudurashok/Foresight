using System;
using System.Windows.Forms;
using Ferry.Win.Common;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.DataAccess;

namespace Ferry.Win.Forms
{
    public partial class FLogin : FFormBase
    {
        #region Contructors

        public FLogin()
        {
            InitializeComponent();
            fillLoginNames();
        }

        private void fillLoginNames()
        {
            Utilities.LoadEnumListItems(cmbLogin, typeof(UserRole));
        }

        #endregion

        #region Event Handlers

        private void FLogin_Load(object sender, EventArgs e)
        {
            try
            {
                lnkServer.Visible = Util.GetGenus() == Genus.Lion;
            }
            catch (ValidationException ex)
            {
                Utilities.ShowMessage(ex.Message);
            }
            catch (Exception ex)
            {
                Utilities.ShowError(ex);
            }
        }

        private void lnkServer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                showServerConfig();
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (Util.GetGenus() == Genus.Lion && !isServerConfigValid())
                {
                    showServerConfig();
                    return;
                }

                processLogin();
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

        private bool isServerConfigValid()
        {
            try
            {
                Util.TestDatabaseConnection();
                return true;
            }
            catch (Exception ex)
            {
                Utilities.ShowMessage(string.Format("Cannot connect to server.\n\n {0}", ex.Message));
            }

            return false;
        }

        private void showServerConfig()
        {
            new FServer().ShowDialog();
        }

        private void processLogin()
        {
            var role = (UserRole)cmbLogin.SelectedValue;
            ForesightDatabaseFactory.GetInstance().CheckPassword((int)role, txtPassword.Text);
            Session.Login = new Login { Role = role };
            clearPassword();
            showMainForm();
            Hide();
        }

        private void showMainForm()
        {
            var coGroups = new FCompanyGroups(this);
            coGroups.Show();
        }

        private void clearPassword()
        {
            txtPassword.Text = "";
        }

        #endregion
    }
}
