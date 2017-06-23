using System;
using System.Collections.Generic;
using System.Linq;
using ScalableApps.Foresight.Logic.Business;

namespace ScalableApps.Foresight.Logic.Report
{
    public abstract class CompanyAccountBaseDataContext : ReportDataContext
    {
        public int TopNCount { private get; set; }
        private IList<CompanyPeriodAccountValue> _result;

        public override ReportData GetReportData()
        {
            _result = new List<CompanyPeriodAccountValue>();
            loadData();
            calculateCompanyTotals();
            calculateDifferencePct();
            return new ReportData(_result);
        }

        public decimal? GetCompanyAccountValue(int accountId, int companyId)
        {
            var rdc = getTopNDataContext();
            rdc.PartyGrouping = PartyGrouping;
            return rdc.GetAccountValue(accountId, companyId);
        }

        protected abstract CompanyTopperBaseDataContext getTopNDataContext();

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

        private void calculateCompanyTotals()
        {
            foreach (var companyId in (getDistinctCompanies()))
            {
                var cp = new CompanyPeriod
                             {
                                 Company = new Company { Id = companyId, Name = "TOTAL:" },
                             };

                var totalValue = (decimal) (from r in _result
                                            where r.Topper.CompanyPeriod.Company.Id == companyId
                                            select r.Topper.Account.Amount).Sum();

                var av = new AccountValue { Name = "TOTAL:", Amount = totalValue };
                var tv = new CompanyPeriodTopperValue { CompanyPeriod = cp, Account = av };
                var totalRow = new CompanyPeriodAccountValue { Topper = tv };
                _result.Add(totalRow);
            }
        }

        private IEnumerable<int> getDistinctCompanies()
        {
            return (from r in _result
                    orderby r.Topper.CompanyPeriod.Company.Id descending
                    select r.Topper.CompanyPeriod.Company.Id).Distinct();
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
                    orderby r.Topper.CompanyPeriod.Company.Id
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
