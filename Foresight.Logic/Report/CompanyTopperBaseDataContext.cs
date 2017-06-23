using System.Collections.Generic;
using System.Linq;
using ScalableApps.Foresight.Logic.Business;

namespace ScalableApps.Foresight.Logic.Report
{
    public abstract class CompanyTopperBaseDataContext : TopperBaseDataContext
    {
        protected override IEnumerable<int> getDistinctIds(IEnumerable<CompanyPeriod> coPeriods)
        {
            return coPeriods.Select(cp => cp.Company.Id).Distinct();
        }

        protected override CompanyPeriod getCompanyPeriodOf(IEnumerable<CompanyPeriod> coPeriods, int companyId)
        {
            return coPeriods.First(cp => cp.Company.Id == companyId);
        }

        protected override string getCompanyPeriodFilter(int id)
        {
            return string.Format(" AND CompanyId={0}", id);
        }
    }
}
