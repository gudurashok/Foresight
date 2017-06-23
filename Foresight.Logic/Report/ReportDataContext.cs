using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.DataAccess;

namespace ScalableApps.Foresight.Logic.Report
{
    public abstract class ReportDataContext : IDisposable
    {
        #region Declarations

        protected readonly Database db;
        public IList<CompanyPeriod> SelectedCoPeriods { protected get; set; }
        public bool PartyGrouping { protected get; set; }
        public bool ItemGrouping { protected get; set; }

        #endregion

        #region Constructor

        protected ReportDataContext()
        {
            db = DatabaseFactory.GetForesightDatabase(Session.CompanyGroup);
        }

        #endregion

        #region Factory Method

        public static ReportDataContext CreateInstance(Command command)
        {
            var asm = Assembly.GetExecutingAssembly();
            return asm.CreateInstance("ScalableApps.Foresight.Logic.Report." +
                                        command.GetPropertyValue("DataContextName"),
                                            true) as ReportDataContext;
        }

        #endregion

        #region Public Members

        public abstract ReportData GetReportData();

        public void Dispose()
        {
            db.Close();
        }

        #endregion

        #region Internal Members

        protected IList<CompanyPeriod> getSelectedCoPeriods()
        {
            if (SelectedCoPeriods.Count > 0)
                return SelectedCoPeriods;

            return Session.Dbc.GetCompanyPeriods();
        }

        protected int[] getCompanyIds()
        {
            return getSelectedCoPeriods().Select(p => p.Company.Id).Distinct().ToArray();
        }

        protected int[] getPeriodIds()
        {
            return getSelectedCoPeriods().Select(p => p.Period.Id).Distinct().ToArray();
        }

        protected int[] getCoPeriodIds()
        {
            return getSelectedCoPeriods().Select(p => p.Id).ToArray();
        }

        protected string createFilterExprFrom(IList<int> selectedIds)
        {
            return createFilterExprFrom("", selectedIds);
        }

        protected string createFilterExprFrom(string prefix, IList<int> selectedIds)
        {
            if (selectedIds.Count() == 0)
                return "";

            var result = new StringBuilder(prefix).Append("(");
            foreach (var id in selectedIds)
                result.Append(string.Format("{0},", id));

            return result.Remove(result.Length - 1, 1).Append(")").ToString();
        }

        protected string getPartyIdentityColumn()
        {
            return PartyGrouping ? "GroupId" : "Id";
        }

        protected string getItemIdentityColumn()
        {
            return ItemGrouping ? "GroupId" : "Id";
        }

        #endregion
    }
}
