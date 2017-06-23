namespace Scalable.Shared.Connection
{
    public class OdbcConnectionInfo : DbConnectionInfoBase, IDbConnectionInfo
    {
        #region Public Members

        public OdbcConnectionInfo(string path)
            : base(path)
        {
        }

        public string GetConnectionString()
        {
            checkPathExits();
            return "Driver={Microsoft dBASE Driver (*.dbf)};DriverID=277;Dbq=" + _path;
        }

        #endregion
    }
}
