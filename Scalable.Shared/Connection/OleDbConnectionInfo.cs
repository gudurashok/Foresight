namespace Scalable.Shared.Connection
{
    public class OleDbConnectionInfo : DbConnectionInfoBase, IDbConnectionInfo
    {
        #region Public Members

        public OleDbConnectionInfo(string path)
            : base(path)
        {
        }

        public string GetConnectionString()
        {
            checkPathExits();

            return "Provider=Microsoft.Jet.OLEDB.4.0" +
                   ";Data Source=" + _path +
                   ";Extended Properties=dBASE IV" +
                   ";User ID=Admin;Password=";
        }

        #endregion
    }
}
