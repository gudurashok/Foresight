using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.IO;
using Ferry.Logic.Base;
using Ferry.Logic.Sql;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;

namespace Ferry.Logic.EASY
{
    public class EasyCompanyDbContext : SourceCompanyDbContext
    {
        #region Constructor

        public EasyCompanyDbContext(string sourceDataPath)
            : base(sourceDataPath)
        {
        }

        #endregion

        #region Public Methods

        public override IList<CompanyPeriod> GetCompanyPeriodsFor(CompanyGroup coGroup)
        {
            if (IsGroupAlreadyExist(coGroup))
                return new List<CompanyPeriod>();

            if (shouldUseCompanyFile())
                loadAllCompanyPeriods(coGroup);
            else
                findAllCompanyPeriods();

            return companyPeriods;
        }

        public override IList<CompanyGroup> GetAllCompanyGroups()
        {
            return loadAllCompanyGroups(readAllCompanyGroups());
        }

        #endregion

        #region Internal Methods

        private bool shouldUseCompanyFile()
        {
            try
            {
                if (!isConvertedCompanyFilesExists())
                    return false;

                dbc.ExecuteScalar(EasySqlQueries.SelectAllCompanyGroups);
                return true;
            }
            catch { }

            return false;
        }

        private bool isConvertedCompanyFilesExists()
        {
            if (!File.Exists(sourceDataPath + @"\" + EasySqlQueries.FsEasyCoGroupFileName))
                return false;

            return File.Exists(sourceDataPath + @"\" + EasySqlQueries.FsEasyCoFileName);
        }

        private void findAllCompanyPeriods()
        {
            foreach (var dir in getProviderDirectories().Where(isProviderDirectoryValid))
                createCompanyPeriodInternal(dir);
        }

        private void createCompanyPeriodInternal(DirectoryInfo dir)
        {
            var cp = createCompanyPeriod(dir);
            if (cp != null) companyPeriods.Add(cp);
        }

        private bool isProviderDirectoryValid(DirectoryInfo dir)
        {
            var files = dir.GetFiles("*.ASK");
            if (files.Length == 0) return false;
            var fi = files.SingleOrDefault(f => f.Name == "TXN_FILE.ASK");
            return fi != null;
        }

        private IEnumerable<DirectoryInfo> getProviderDirectories()
        {
            var di = new DirectoryInfo(sourceDataPath);
            return di.GetDirectories("*_*");
        }

        protected override string createCompanyName(string coName)
        {
            return coName.Substring(0, coName.IndexOf('_'));
        }

        protected override SourceDataProvider getProvider()
        {
            return SourceDataProvider.Easy;
        }

        protected override void makeFinancialPeriod(DatePeriod dp, int year)
        {
            dp.FinancialFrom = new DateTime(year, 4, 1);
            dp.FinancialTo = new DateTime(year + 1, 3, 31);
        }

        private string GetCoFileName()
        {
            return McsSqlQueries.CoFileName;
        }

        private IDataReader readAllCompanyGroups()
        {
            return dbc.ExecuteReader(EasySqlQueries.SelectAllCompanyGroups);
        }

        private IList<CompanyGroup> loadAllCompanyGroups(IDataReader groupData)
        {
            var groups = new List<CompanyGroup>();

            while (groupData.Read())
                groups.Add(readCompanyGroup(groupData));

            groupData.Close();
            return groups;
        }

        private CompanyGroup readCompanyGroup(IDataReader reader)
        {
            var group = new CompanyGroup(reader["CO_GNAME"].ToString(), "");
            group.Code = reader["CO_GRP"].ToString();
            return group;
        }

        private void loadAllCompanyPeriods(CompanyGroup coGroup)
        {
            loadAllCompanyPeriodsOfGroup(coGroup, readCompaniesOfGroup(coGroup));
        }

        private IDataReader readCompaniesOfGroup(CompanyGroup group)
        {
            return dbc.ExecuteReader(EasySqlQueries.SelectAllCompaniesOfGroupCode, "@groupCode", group.Code);
        }

        private void loadAllCompanyPeriodsOfGroup(CompanyGroup group, IDataReader coData)
        {
            while (coData.Read())
                companyPeriods.Add(readCompanyPeriod(group, coData));

            coData.Close();
        }

        private CompanyPeriod readCompanyPeriod(CompanyGroup group, IDataReader coData)
        {
            var cp = new CompanyPeriod();
            cp.Company = getCompany(coData["CO_CODE"].ToString(), coData["CO_NAME"].ToString(), group);
            cp.Period = new DatePeriod();
            cp.Period.FinancialFrom = Convert.ToDateTime(coData["FINYEAR_BD"]);
            cp.Period.FinancialTo = Convert.ToDateTime(coData["FINYEAR_ED"]);
            cp.DataPath = coData["DIR_NAME"].ToString();
            cp.SourceDataProvider = SourceDataProvider.Easy;
            return cp;
        }

        #endregion
    }
}
