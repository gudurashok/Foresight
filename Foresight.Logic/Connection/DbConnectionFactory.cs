using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Properties;

namespace ScalableApps.Foresight.Logic.Connection
{
    public static class DbConnectionFactory
    {
        public static IDbConnection GetConnection(
                                    DatabaseProvider provider,
                                    string connectionString)
        {
            switch (provider)
            {
                case DatabaseProvider.SqlServer:
                    return new SqlConnection(connectionString);
                case DatabaseProvider.OleDb:
                    return new OleDbConnection(connectionString);
                case DatabaseProvider.SqlCe:
                    return getSqlCeConnection(connectionString);
                case DatabaseProvider.Odbc:
                    return new OdbcConnection(connectionString);
                default:
                    throw new ValidationException(
                            string.Format(Resources.TargetDatabaseProviderNotSupported, provider));
            }
        }

        private static IDbConnection getSqlCeConnection(string connectionString)
        {
            var conn = Util.GetSqlCeAssembly().CreateInstance("System.Data.SqlServerCe.SqlCeConnection") as IDbConnection;
            if (conn == null) return null;
            conn.ConnectionString = connectionString;
            return conn;
        }
    }
}
