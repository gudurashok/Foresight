using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Ferry.Logic.Connection;
using Ferry.Logic.Sql;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.DataAccess;

namespace Ferry.Logic.Base
{
    public abstract class SourceCompanyDbContext
    {
        #region Declarations

        protected readonly string sourceDataPath;
        protected Database dbc;
        protected readonly IList<CompanyPeriod> companyPeriods;

        #endregion

        protected SourceCompanyDbContext(string sourceDataPath)
        {
            this.sourceDataPath = sourceDataPath;
            getSourceDataContext();
            companyPeriods = new List<CompanyPeriod>();
        }

        public abstract IList<CompanyPeriod> GetCompanyPeriodsFor(CompanyGroup coGroup);
        public abstract IList<CompanyGroup> GetAllCompanyGroups();

        private void getSourceDataContext()
        {
            var connInfo = SourceDbConnInfoFactory.GetConnectionInfo(sourceDataPath);
            dbc = DatabaseFactory.GetSourceDatabase(connInfo);
        }

        public static string[] GetProviderCompanyFileNames(SourceDataProvider provider)
        {
            switch (provider)
            {
                case SourceDataProvider.Easy:
                    return new[] { EasySqlQueries.EasyCoGroupFileName, EasySqlQueries.EasyCoFileName };
                case SourceDataProvider.Mcs:
                    return new[] { McsSqlQueries.CoFileName, McsSqlQueries.GlGroupFileName };
                case SourceDataProvider.Tcs:
                    return new[] { TcsSqlQueries.CoFileName, TcsSqlQueries.GlGroupFileName };
                default:
                    throw new NotSupportedException();
            }
        }

        public static string[] GetProviderCoPeriodDataFileNames(SourceDataProvider provider)
        {
            switch (provider)
            {
                case SourceDataProvider.Easy:
                    return new[] { EasySqlQueries.AccountMasterFileName, EasySqlQueries.TransactionFileName };
                case SourceDataProvider.Mcs:
                    return new[] { McsSqlQueries.AccountMasterFileName, McsSqlQueries.TransactionFileName };
                case SourceDataProvider.Tcs:
                    return new[] { TcsSqlQueries.AccountMasterFileName, TcsSqlQueries.TransactionFileName };
                default:
                    throw new NotSupportedException();
            }
        }

        public static bool IsGlGroupFileExists(SourceDataProvider provider, string folderPath)
        {
            var di = new DirectoryInfo(folderPath);
            return di.Parent != null && di.Parent.GetFiles(getGlGroupFileName(provider)).Length > 0;
        }

        private static string getGlGroupFileName(SourceDataProvider provider)
        {
            if (provider == SourceDataProvider.Mcs)
                return McsSqlQueries.GlGroupFileName;

            return TcsSqlQueries.GlGroupFileName;
        }

        protected bool IsGroupAlreadyExist(CompanyGroup coGroup)
        {
            return companyPeriods != null && companyPeriods
                                .Where(c => c.Company.Group.Code == coGroup.Code)
                                .Distinct().Count() > 0;
        }

        public CompanyPeriod GetCompanyPeriod()
        {
            return createCompanyPeriod(new DirectoryInfo(sourceDataPath));
        }

        protected CompanyPeriod createCompanyPeriod(DirectoryInfo dir)
        {
            return createCompanyPeriod(createDummyGroup(), dir);
        }

        private CompanyGroup createDummyGroup()
        {
            return new CompanyGroup("EASY company group", "") { Code = "NA" };
        }

        private CompanyPeriod createCompanyPeriod(CompanyGroup group, DirectoryInfo dir)
        {
            try
            {
                var cp = new CompanyPeriod();
                cp.Company = getCompany(createCompanyName(dir.Name), createCompanyName(dir.Name), group);
                cp.Period = createPeriod(dir.Name);
                cp.DataPath = dir.FullName;
                cp.SourceDataProvider = getProvider();
                return cp;
            }
            catch { }

            return null;
        }

        protected Company getCompany(string coCode, string coName, CompanyGroup group)
        {
            IList<CompanyPeriod> result = (from p in companyPeriods
                                           where p.Company.Name == coName 
                                              && p.Company.Group.Code == @group.Code
                                           select p).ToList();

            if (result.Count == 0)
                return new Company { Code = coCode, Name = coName, Group = group };

            return result[0].Company;
        }

        protected abstract SourceDataProvider getProvider();
        protected abstract string createCompanyName(string coName);
        protected abstract void makeFinancialPeriod(DatePeriod dp, int year);

        private DatePeriod createPeriod(string coName)
        {
            var dp = new DatePeriod();
            makeFinancialPeriod(dp, getFinancialYear(coName));
            dp.Name = dp.GetNameFromFinancialPeriod();
            return dp;
        }

        private int getFinancialYear(string coName)
        {
            int year = Convert.ToInt32(coName.Substring(coName.Length - 2));
            year = addCentury(year);
            return year;
        }

        private int addCentury(int year)
        {
            return year + (year <= 70 ? 2000 : 1900);
        }
    }
}
