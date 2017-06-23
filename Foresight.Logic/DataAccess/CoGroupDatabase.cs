using System;
using System.IO;
using System.Text;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Sql;

namespace ScalableApps.Foresight.Logic.DataAccess
{
    public abstract class CoGroupDatabase : IDisposable
    {
        #region Declarations

        private const string databaseScriptFileName = "ForesightData.sqlce";
        protected Database groupDb;
        protected CompanyGroup companyGroup;
        private readonly ForesightDatabase _foresightDb;

        #endregion

        #region Public Members

        protected CoGroupDatabase()
        {
            _foresightDb = ForesightDatabaseFactory.GetInstance();
        }

        public void Dispose()
        {
            Close();
        }

        public void Close()
        {
            if (groupDb != null)
                groupDb.Close();
        }

        public void SaveCompanyGroup(CompanyGroup coGroup)
        {
            companyGroup = coGroup;

            if (coGroup.IsNew())
                if (shouldCreateDataFile(coGroup))
                    createCompanyGroup();
                else
                    saveCompanyGroupInForesight();
            else
                updateCompanyGroup();
        }

        private bool shouldCreateDataFile(CompanyGroup coGroup)
        {
            try
            {
                new DataContext(coGroup).GetCompanyGroupById(1);
                return false;
            }
            catch { }

            return true;
        }

        private void createCompanyGroup()
        {
            companyGroup.FilePath = getCompanyGroupDatabaseName();
            new SaveCompanyGroupRules().Check(_foresightDb, companyGroup);
#if DEBUG
            checkScriptFileExists();
#endif
            createCompanyGroupDatabase();
            createDatabaseSchema(getDatabaseSchemaScript());
            saveCompanyGroup(SqlQueries.InsertCompanyGroup);
            saveCompanyGroupInForesight();
        }

        private void saveCompanyGroupInForesight()
        {
            _foresightDb.SaveCompanyGroup(companyGroup);
        }

        protected abstract string getCompanyGroupDatabaseName();
        protected abstract void createCompanyGroupDatabase();
        protected abstract void deleteCompanyGroupDatabase(CompanyGroup coGroup);

        public void DeleteCompanyGroup(CompanyGroup coGroup)
        {
            deleteCompanyGroupDatabase(coGroup);
            _foresightDb.DeleteCompanyGroup(coGroup);
        }

        #endregion

        #region Internal Members

        private string getDatabaseSchemaScript()
        {

#if DEBUG
            return File.ReadAllText(getDatabaseScriptFileName());
#else
            return _foresightDb.GetDbScript();
#endif
        }

#if DEBUG
        private void checkScriptFileExists()
        {
            if (!File.Exists(getDatabaseScriptFileName()))
                throw new ValidationException(
                    string.Format("Script file not found at {0}", getDatabaseScriptFileName()));

        }
#endif

        private string getDatabaseScriptFileName()
        {
            return Util.GetAppPath() + @"\" + databaseScriptFileName;
        }

        private void createDatabaseSchema(string script)
        {
            var cmd = groupDb.CreateCommand();
            var builder = new StringBuilder(0x2710);

            using (var reader = new StringReader(script))
            {
                string scriptLine;

                while ((scriptLine = reader.ReadLine()) != null)
                {
                    if (scriptLine.Equals("GO", StringComparison.OrdinalIgnoreCase))
                    {
                        cmd.CommandText = builder.ToString();
                        cmd.ExecuteNonQuery();
                        builder.Remove(0, builder.Length);
                    }
                    else if (!scriptLine.StartsWith("-- "))
                    {
                        builder.Append(scriptLine);
                        builder.Append(Environment.NewLine);
                    }
                }
            }
        }

        private void updateCompanyGroup()
        {
            setDatabaseContext();
            new SaveCompanyGroupRules().Check(_foresightDb, companyGroup);
            saveCompanyGroup(SqlQueries.UpdateCompanyGroup);
            updateCompanyGroupInForesight();
        }

        protected abstract void setDatabaseContext();

        private void saveCompanyGroup(string commandText)
        {
            var cmd = groupDb.CreateCommand(commandText);
            groupDb.AddParameterWithValue(cmd, "@Id", 1);
            groupDb.AddParameterWithValue(cmd, "@Name", companyGroup.Name);
            cmd.ExecuteNonQuery();
        }

        private void updateCompanyGroupInForesight()
        {
            _foresightDb.UpdateCompanyGroup(companyGroup);
        }

        #endregion
    }
}
