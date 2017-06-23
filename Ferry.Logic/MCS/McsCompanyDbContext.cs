using System;
using System.Collections.Generic;
using System.Linq;
using Ferry.Logic.Base;
using Ferry.Logic.Sql;
using System.Data;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;

namespace Ferry.Logic.MCS
{
    public class McsCompanyDbContext : SourceCompanyDbContext
    {
        private readonly IList<SourceCompanyPeriod> _sourceCoPeriods;
        private readonly IList<CompanyGroup> _coGroups;

        #region Constructor

        public McsCompanyDbContext(string sourceDataPath)
            : base(sourceDataPath)
        {
            _sourceCoPeriods = new List<SourceCompanyPeriod>();
            _coGroups = new List<CompanyGroup>();
        }

        #endregion

        #region Public Methods

        public override IList<CompanyGroup> GetAllCompanyGroups()
        {
            loadCompanyPeriods();
            return getAllCoGroups();
        }

        public override IList<CompanyPeriod> GetCompanyPeriodsFor(CompanyGroup coGroup)
        {
            if (IsGroupAlreadyExist(coGroup))
                return new List<CompanyPeriod>();

            loadCompanyPeriodsOfGroup(coGroup);
            return companyPeriods;
        }

        #endregion

        #region Internal Methods

        protected virtual string GetCoFileName()
        {
            return McsSqlQueries.CoFileName;
        }

        private void loadCompanyPeriods()
        {
            var reader = dbc.ExecuteReader(
                            string.Format(McsSqlQueries.SelectAllCompanyPeriods, GetCoFileName()));

            while (reader.Read())
                _sourceCoPeriods.Add(getSourceCoPeriod(reader));

            reader.Close();
        }

        private SourceCompanyPeriod getSourceCoPeriod(IDataReader reader)
        {
            var result = new SourceCompanyPeriod();
            result.CoCode = reader["COM_CD"].ToString();
            result.CoName = reader["COM_NM"].ToString();
            result.GroupCode = reader["GROUP"].ToString();
            result.DateFrom = Convert.ToDateTime(reader["YDT1"]);
            result.DateTo = Convert.ToDateTime(reader["YDT2"]);
            return result;
        }

        private IList<CompanyGroup> getAllCoGroups()
        {
            foreach (var groupCode in getDistinctGroupCodes())
            {
                var coGroup = new CompanyGroup(getCoGroupName(groupCode), "");
                coGroup.Code = groupCode;
                _coGroups.Add(coGroup);
            }

            return _coGroups.OrderBy(c => c.Name).ToList();
        }

        private string getCoGroupName(string groupCode)
        {
            if (string.IsNullOrEmpty(groupCode))
                return "(UNGROUPED)";

            return getGroupNameIfExist(groupCode);
        }

        private string getGroupNameIfExist(string groupCode)
        {
            const int groupCodeLength = 3;

            var result = _sourceCoPeriods
                            .Where(c => groupCode.Length == groupCodeLength
                                        && groupCode == c.CoCode.Substring(0, groupCodeLength))
                            .OrderByDescending(c => c.CoCode)
                            .Select(c => c.CoName).FirstOrDefault();

            return result ?? groupCode;
        }

        private IEnumerable<string> getDistinctGroupCodes()
        {
            return _sourceCoPeriods.Select(c => c.GroupCode).Distinct();
        }

        private void loadCompanyPeriodsOfGroup(CompanyGroup group)
        {
            foreach (var coCode in getCompanyCodesFor(group))
                readCompanyPeriod(group, coCode);
        }

        private IEnumerable<string> getCompanyCodesFor(CompanyGroup group)
        {
            return _sourceCoPeriods.Where(c => c.GroupCode == group.Code)
                                            .OrderBy(c => c.CoCode)
                                            .Select(c => c.CoCode)
                                            .Distinct().ToList();
        }

        private void readCompanyPeriod(CompanyGroup group, string coCode)
        {
            var coPeriod = getCompanyPeriodFor(coCode, group.Code);
            var cp = new CompanyPeriod();
            cp.Company = getCompany(coCode, coPeriod.CoName, group);
            cp.Period = new DatePeriod();
            cp.Period.FinancialFrom = Convert.ToDateTime(coPeriod.DateFrom);
            cp.Period.FinancialTo = Convert.ToDateTime(coPeriod.DateTo);
            cp.DataPath = sourceDataPath + @"\" + coCode;
            cp.SourceDataProvider = getProvider();
            companyPeriods.Add(cp);
        }

        private SourceCompanyPeriod getCompanyPeriodFor(string coCode, string groupCode)
        {
            return _sourceCoPeriods.FirstOrDefault(c => c.CoCode == coCode && c.GroupCode == groupCode);
        }

        protected override SourceDataProvider getProvider()
        {
            return SourceDataProvider.Mcs;
        }

        protected override string createCompanyName(string coName)
        {
            return coName.Substring(0, 3);
        }

        protected override void makeFinancialPeriod(DatePeriod dp, int year)
        {
            dp.FinancialFrom = new DateTime(year - 1, 4, 1);
            dp.FinancialTo = new DateTime(year, 3, 31);
        }

        #endregion
    }
}
