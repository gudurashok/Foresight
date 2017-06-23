using System.Collections.Generic;
using System.Linq;
using ScalableApps.Foresight.Logic.Business;

namespace ScalableApps.Foresight.Logic.Report
{
    public abstract class PeriodTopperBaseDataContext : TopperBaseDataContext
    {
        protected override IEnumerable<int> getDistinctIds(IEnumerable<CompanyPeriod> coPeriods)
        {
            return coPeriods.Select(cp => cp.Period.Id).Distinct();
        }

        protected override CompanyPeriod getCompanyPeriodOf(IEnumerable<CompanyPeriod> coPeriods, int periodId)
        {
            return coPeriods.First(cp => cp.Period.Id == periodId);
        }

        protected override string getCompanyPeriodFilter(int id)
        {
            return string.Format(" AND PeriodId={0}", id);
        }
    }
}
