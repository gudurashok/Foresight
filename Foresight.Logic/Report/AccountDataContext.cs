using System;
using System.Collections.Generic;
using System.Linq;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Sql;

namespace ScalableApps.Foresight.Logic.Report
{
    public class AccountDataContext : ReportDataContext
    {
        public AccountTypes AccountTypes { private get; set; }
        public bool OnlyDaybooks { private get; set; }

        public override ReportData GetReportData()
        {
            if (OnlyDaybooks)
                return new ReportData(Session.Dbc.GetDaybooks());

            return new ReportData(Session.Dbc.GetTrialBalances());
        }
    }
}
