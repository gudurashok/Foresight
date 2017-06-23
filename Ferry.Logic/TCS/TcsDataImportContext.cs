using Ferry.Logic.MCS;
using ScalableApps.Foresight.Logic.Business;

namespace Ferry.Logic.TCS
{
    public class TcsDataImportContext : McsDataImportContext
    {
        public TcsDataImportContext(CompanyPeriod companyPeriod)
            : base(companyPeriod)
        {
        }

        protected override string getChartOfAccountsGroupFileName()
        {
            return "RPGRP.DBF";
        }
    }
}
