using System.Text;
using System.IO;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Properties;

namespace ScalableApps.Foresight.Logic.Connection
{
    internal class SqlCeConnectionInfo : IDbConnectionInfo
    {
        #region Internal Declrations

        private readonly string _path;
        private readonly string _password;

        #endregion

        #region Constructors

        public SqlCeConnectionInfo(string path, string password)
        {
            _path = path;
            _password = password;
        }

        #endregion

        #region Public Members

        public string GetConnectionStringForCreate()
        {
            return getConnectionStringInternal();
        }

        public string GetConnectionString()
        {
            checkFileExits();
            makeDataFileWriteable();
            return getConnectionStringInternal();
        }

        #endregion

        #region Internal Members

        private string getConnectionStringInternal()
        {
            var result = new StringBuilder("Data Source=" + _path);

            if (!string.IsNullOrEmpty(_password))
                result.Append(";Password=" + _password);

            return result.ToString();
        }

        private void checkFileExits()
        {
            if (!File.Exists(_path))
                throw new ValidationException(string.Format(Resources.DatabaseFileNotFound, _path));
        }

        private void makeDataFileWriteable()
        {
            var fi = new FileInfo(_path);

            if ((fi.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                fi.Attributes = FileAttributes.Normal;
        }

        #endregion
    }
}
