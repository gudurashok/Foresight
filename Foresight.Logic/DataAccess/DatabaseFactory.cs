using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Connection;

namespace ScalableApps.Foresight.Logic.DataAccess
{
    public static class DatabaseFactory
    {
        public static Database GetForesightDatabase(CompanyGroup companyGroup)
        {
            var db = GetForesightDatabase(TargetDbConnInfoFactory.GetConnectionInfo(companyGroup));
            db.ChangeDatabase(companyGroup.FilePath);
            return db;
        }

        public static Database GetForesightDatabase(IDbConnectionInfo connInfo)
        {
            return new Database(
                    DbConnectionFactory.GetConnection(
                                Util.GetForesightDatabaseProvider(),
                                connInfo.GetConnectionString()));
        }

        public static Database GetSourceDatabase(IDbConnectionInfo connInfo)
        {
            return new Database(
                    DbConnectionFactory.GetConnection(
                                Util.GetSourceProvider(),
                                connInfo.GetConnectionString()));
        }
    }
}
