using System.Text;
using System.IO;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;

namespace ScalableApps.Foresight.Logic.DataAccess
{
    public class CoGroupSqlCeDatabase : CoGroupDatabase
    {
        #region Declarations

        private const string dataFileExtension = ".fsd";
        private const string allForesightDataFiles = "*" + dataFileExtension;
        private const string dataPath = @"\Data";

        #endregion

        #region Save Company Group

        protected override void createCompanyGroupDatabase()
        {
            new SqlCeSaveCompanyGroupRules().Check(companyGroup);
            createCompanyGroupWithEngine();
            groupDb = DatabaseFactory.GetForesightDatabase(companyGroup);
        }

        protected override string getCompanyGroupDatabaseName()
        {
            return getCompanyGroupFileName();
        }

        private void createCompanyGroupWithEngine()
        {
            var engine = Util.GetSqlCeAssembly().CreateInstance("System.Data.SqlServerCe.SqlCeEngine");
            if (engine == null) 
                return;

            var type = engine.GetType();
            var pi = type.GetProperty("LocalConnectionString");
            pi.SetValue(engine, getNewDbConnectionString(), null);
            var mi = type.GetMethod("CreateDatabase");
            mi.Invoke(engine, null);
        }

        private string getNewDbConnectionString()
        {
            var result = new StringBuilder("Data Source=" + companyGroup.FilePath);

            if (!string.IsNullOrEmpty(CompanyGroup.Password))
                result.Append(";Password=" + CompanyGroup.Password);

            return result.ToString();
        }

        private string getCompanyGroupFileName()
        {
            return getDataPath() + @"\" + companyGroup.CreateDatabaseName() + dataFileExtension;
        }

        protected override void setDatabaseContext()
        {
            groupDb = DatabaseFactory.GetForesightDatabase(companyGroup);
        }

        #endregion

        #region Delete Company Group

        protected override void deleteCompanyGroupDatabase(CompanyGroup coGroup)
        {
            var fi = new FileInfo(coGroup.FilePath);
            if (fi.Directory != null && !fi.Directory.Exists)
                return;

            fi.Delete();
        }

        #endregion

        #region Internal Methods

        private string getDataPath()
        {
            //TODO: get from the config file
            //string path = Common.GetDataPath();
            //if (path == null) path = DataPath;

            //return Common.GetAppPath();
            var path = Util.GetAppPath() + dataPath;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        #endregion
    }
}
