using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using Scalable.Shared.Common;
using Scalable.Shared.Enums;
using Scalable.Shared.Properties;

namespace Scalable.Shared.Connection
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
                            string.Format(Resources.DatabaseProviderNotSupported, provider));
            }
        }

        //TODO: Should be moved
        private static IDbConnection getSqlCeConnection(string connectionString)
        {
            var asm = ScalableUtil.GetSqlCeAssembly(); 
            var conn = asm.CreateInstance("System.Data.SqlServerCe.SqlCeConnection") as IDbConnection;
            if (conn == null) return null;
            conn.ConnectionString = connectionString;
            return conn;
        }
    }
}
