using System;
using System.Data.SqlServerCe;
using System.IO;
using System.Windows.Forms;

namespace ForesightDataUpdateGUI
{
    public partial class ForesightDataUpdate : Form
    {
        public ForesightDataUpdate()
        {
            InitializeComponent();
        }

        private void btnFindForesightFile_Click(object sender, EventArgs e)
        {
            ofdForesightFile.Filter = @"sdf|*.sdf|isd|*.isd";
            ofdForesightFile.ShowDialog();

            if (ofdForesightFile.FileName != "")
                txtForesightSdfFilePath.Text = ofdForesightFile.FileName;
        }

        private void btnFindForesightDataFile_Click(object sender, EventArgs e)
        {
            ofdForesightFile.Filter = @"sqlce|*.sqlce";
            ofdForesightFile.ShowDialog();

            if (ofdForesightFile.FileName != "")
                txtForesightDataScriptFilePath.Text = ofdForesightFile.FileName;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtForesightSdfFilePath.Text == "")
                {
                    MessageBox.Show(@"Select Valid Foresight file");
                    return;
                }

                if (txtForesightDataScriptFilePath.Text == "")
                {
                    MessageBox.Show(@"Select Valid ForesightData file");
                    return;
                }

                Cursor = Cursors.WaitCursor;
                UpdateData(GetConnection(), GetQuery());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private SqlCeConnection GetConnection()
        {
            var conn = new SqlCeConnection();
            conn.ConnectionString = @"DataSource = " + txtForesightSdfFilePath.Text + "; Password = iScalable@2011";
            conn.Open();
            return conn;
        }

        private string GetQuery()
        {
            return @"UPDATE [DbScript] SET [Script] = N'" + File.ReadAllText(txtForesightDataScriptFilePath.Text).Replace("'", "''") + "'";
        }

        private void UpdateData(SqlCeConnection conn, string query)
        {
            var comm = conn.CreateCommand();
            comm.CommandText = query;
            comm.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show(@"Success in " + conn.DataSource);
        }
    }
}
