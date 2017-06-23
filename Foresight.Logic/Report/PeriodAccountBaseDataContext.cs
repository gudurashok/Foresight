using System;
using System.Collections.Generic;
using System.Linq;
using ScalableApps.Foresight.Logic.Business;

namespace ScalableApps.Foresight.Logic.Report
{
    public abstract class PeriodAccountBaseDataContext : ReportDataContext
    {
        public int TopNCount { private get; set; }
        private IList<CompanyPeriodAccountValue> _result;

        public override ReportData GetReportData()
        {
            _result = new List<CompanyPeriodAccountValue>();
            loadData();
            calculatePeriodTotals();
            calculateDifferencePct();
            return new ReportData(_result);
        }

        public decimal? GetPeriodAccountValue(int accountId, int periodId)
        {
            var rdc = getTopNDataContext();
            rdc.PartyGrouping = PartyGrouping;
            return rdc.GetAccountValue(accountId, periodId);
        }

        protected abstract PeriodTopperBaseDataContext getTopNDataContext();

        private void loadData()
        {
            var rdc = getTopNDataContext();
            rdc.PartyGrouping = PartyGrouping;
            rdc.SelectedCoPeriods = SelectedCoPeriods;
            rdc.TopNCount = TopNCount;
            var report = rdc.GetReportData();

            foreach (var tv in (IList<CompanyPeriodTopperValue>)report.Result)
                _result.Add(new CompanyPeriodAccountValue { Topper = tv });
        }

        private void calculatePeriodTotals()
        {
            foreach (var periodId in (getDistinctPeriods()))
            {
                var cp = new CompanyPeriod
                             {
                                 Period = new DatePeriod { Id = periodId, Name = "TOTAL:" },
                             };

                var totalValue = (decimal)(from r in _result
                                           where r.Topper.CompanyPeriod.Period.Id == periodId
                                           select r.Topper.Account.Amount).Sum();

                var av = new AccountValue { Name = "TOTAL:", Amount = totalValue };

                var tv = new CompanyPeriodTopperValue { CompanyPeriod = cp, Account = av };
                var totalRow = new CompanyPeriodAccountValue { Topper = tv };
                _result.Add(totalRow);
            }
        }

        private IEnumerable<int> getDistinctPeriods()
        {
            return (from r in _result
                    orderby r.Topper.CompanyPeriod.Period.Id descending
                    select r.Topper.CompanyPeriod.Period.Id).Distinct();
        }

        private void calculateDifferencePct()
        {
            foreach (var accountId in (_result.Select(r => r.Topper.Account.Id).Distinct()))
            {
                var accounts = getAccountsOf(accountId);

                if (accounts.Count <= 1)
                    continue;

                var previousValue = accounts[0].Topper.Account.Amount ?? 0;

                for (var i = 1; i < accounts.Count; i++)
                {
                    var currentValue = accounts[i].Topper.Account.Amount ?? 0;
                    calculateDifferencePct(accounts[i], previousValue, currentValue);
                    previousValue = currentValue;
                }
            }
        }

        private IList<CompanyPeriodAccountValue> getAccountsOf(int accountId)
        {
            return (from r in _result
                    where r.Topper.Account.Id == accountId
                    orderby r.Topper.CompanyPeriod.Period.Id
                    select r).ToList();
        }

        private void calculateDifferencePct(CompanyPeriodAccountValue account,
                                            decimal previousValue,
                                            decimal currentValue)
        {
            if (previousValue == 0)
                return;

            var diffValue = currentValue - previousValue;
            var diffPct = Math.Round((diffValue / previousValue) * 100, 2);
            account.DifferencePct = diffPct;
        }
    }
}
