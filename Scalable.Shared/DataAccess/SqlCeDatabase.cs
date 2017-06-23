using Scalable.Shared.Common;
using Scalable.Shared.Properties;
using System.IO;

namespace Scalable.Shared.DataAccess
{
    public class SqlCeDatabase
    {
        #region Declarations

        private Database _database;

        #endregion

        #region Constructor

        public SqlCeDatabase(Database database)
        {
            _database = database;
        }

        #endregion

        #region Public Methods

        public void Create()
        {
            var engine = ScalableUtil.GetSqlCeAssembly().CreateInstance("System.Data.SqlServerCe.SqlCeEngine");
            if (engine == null)
                return;

            var type = engine.GetType();
            var pi = type.GetProperty("LocalConnectionString");
            pi.SetValue(engine, getConnectionStringForCreate(), null);
            var mi = type.GetMethod("CreateDatabase");
            mi.Invoke(engine, null);
        }

        public void Delete()
        {
            var fi = new FileInfo(_database.Name);
            if (fi.Directory != null && !fi.Directory.Exists)
                return;

            fi.Delete();
        }

        #endregion

        #region Internal Methods

        private string getConnectionStringForCreate()
        {
            checkDatabaseFileNotExists();
            return _database.GetSqlCeConnectionString();
        }

        private void checkDatabaseFileNotExists()
        {
            if (File.Exists(_database.Name))
                throw new ValidationException(string.Format(Resources.DatabaseFileAlreadyExist, _database.Name));
        }

        #endregion
    }
}
