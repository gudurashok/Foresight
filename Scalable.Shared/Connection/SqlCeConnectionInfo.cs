using System.IO;
using Scalable.Shared.Common;
using Scalable.Shared.DataAccess;
using Scalable.Shared.Properties;

namespace Scalable.Shared.Connection
{
    public class SqlCeConnectionInfo : IDbConnectionInfo
    {
        #region Internal Declrations

        private readonly Database _database;

        #endregion

        #region Constructors

        public SqlCeConnectionInfo(Database database)
        {
            _database = database;
        }

        #endregion

        #region Public Members

        public string GetConnectionString()
        {
            checkDatabaseFileExists();
            makeDataFileWriteable();
            return _database.GetSqlCeConnectionString();
        }

        #endregion

        #region Internal Members

        private void checkDatabaseFileExists()
        {
            if (!File.Exists(_database.Name))
                throw new ValidationException(string.Format(Resources.DatabaseFileNotFound, _database.Name));
        }

        private void makeDataFileWriteable()
        {
            var fi = new FileInfo(_database.Name);

            if ((fi.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                fi.Attributes = FileAttributes.Normal;
        }

        #endregion
    }
}
