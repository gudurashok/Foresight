using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Connection;
using ScalableApps.Foresight.Logic.Sql;

namespace ScalableApps.Foresight.Logic.DataAccess
{
    public class CoGroupSqlServerDatabase : CoGroupDatabase
    {
        #region Declarations

        #endregion

        #region Constructors

        public CoGroupSqlServerDatabase()
        {
            groupDb = DatabaseFactory.GetForesightDatabase(TargetDbConnInfoFactory.GetSqlConnectionInfo());
            changeDatabaseContextToMasterDb();
        }

        #endregion

        #region Public Methods

        protected override void deleteCompanyGroupDatabase(CompanyGroup coGroup)
        {
            changeDatabaseContextToMasterDb();
            groupDb.ExecuteNonQuery(string.Format(SqlQueries.DropDatabase, coGroup.FilePath));
        }

        #endregion

        #region Internal Methods

        protected override void createCompanyGroupDatabase()
        {
            groupDb.ExecuteNonQuery(string.Format(SqlQueries.CreateDatabase, companyGroup.FilePath));
            groupDb.ChangeDatabase(companyGroup.FilePath);
        }

        protected override string getCompanyGroupDatabaseName()
        {
            return companyGroup.CreateDatabaseName();
        }

        protected override void setDatabaseContext()
        {
            groupDb.ChangeDatabase(companyGroup.FilePath);
        }

        private void changeDatabaseContextToMasterDb()
        {
            groupDb.ChangeDatabase("master");
        }

        #endregion
    }
}
