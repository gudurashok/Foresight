using Scalable.Shared.Common;
using Scalable.Shared.Connection;
using Scalable.Shared.Enums;

namespace Scalable.Shared.DataAccess
{
    public static class DbManagerFactory
    {
        public static SqlDbManager GetInstance(Database database)
        {
            var db = GetInstance(DbConnInfoFactory.GetConnectionInfo(database));
            db.ChangeDatabase(database);
            return db;
        }

        public static SqlDbManager GetInstance(IDbConnectionInfo connInfo)
        {
            return GetInstance(connInfo, ScalableUtil.GetAppDatabaseProvider());
        }

        public static SqlDbManager GetInstance(IDbConnectionInfo connInfo, DatabaseProvider provider)
        {
            return new SqlDbManager(
                    DbConnectionFactory.GetConnection(provider, connInfo.GetConnectionString()));
        }

        //TODO: has to merged into... GetInstance like as above. bcz only source and app dataprovider is different
        //public static SqlDbManager GetSourceDbManager(IDbConnectionInfo connInfo)
        //{
        //    return new SqlDbManager(
        //            DbConnectionFactory.GetConnection(
        //                        ScalableUtil.GetSourceDatabaseProvider(),
        //                        connInfo.GetConnectionString()));
        //}
    }
}
