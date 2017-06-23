using System.Collections.Generic;
using System.Linq;
using ScalableApps.Foresight.Logic.Business;

namespace ScalableApps.Foresight.Logic.Report
{
    public class LostPartyCountDataContext : PartyCountBaseDataContext
    {
        protected override IEnumerable<int> getDistinctPeriods(IEnumerable<CompanyPeriod> coPeriods)
        {
            return coPeriods.Select(cp => cp.Period.Id).Distinct().OrderByDescending(p => p);
        }
    }
}
