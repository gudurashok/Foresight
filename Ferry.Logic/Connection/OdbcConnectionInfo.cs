using System.IO;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Connection;

namespace Ferry.Logic.Connection
{
    internal class OdbcConnectionInfo : IDbConnectionInfo
    {
        private readonly string _path;

        #region Public Members

        public OdbcConnectionInfo(string path)
        {
            _path = path;
        }

        public string GetConnectionString()
        {
            checkPathExits();
            return "Driver={Microsoft dBASE Driver (*.dbf)};DriverID=277;Dbq=" + _path;
        }

        #endregion

        #region Internal Members

        private void checkPathExits()
        {
            if (!Directory.Exists(_path))
                throw new ValidationException(string.Format("Data folder {0} doesn't exist.", _path));
        }

        #endregion
    }
}
