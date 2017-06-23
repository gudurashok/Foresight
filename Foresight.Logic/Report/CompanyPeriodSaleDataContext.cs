using System;
using System.Collections.Generic;
using System.Linq;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Sql;
using ScalableApps.Foresight.Logic.Common;

namespace ScalableApps.Foresight.Logic.Report
{
    public class CompanyPeriodSaleDataContext : ReportDataContext
    {
        private IList<CompanyPeriodValue> _result;

        public override ReportData GetReportData()
        {
            _result = new List<CompanyPeriodValue>();

            foreach (var cp in getSelectedCoPeriods())
                _result.Add(readCompanyPeriodValue(cp));

            calculateTotals();
            calculateDifferencePct();

            var rd = new ReportData(_result);
            return rd;
        }

        private void calculateTotals()
        {
            foreach (var periodId in (getDistinctPeriods()))
            {
                var total = new CompanyPeriodValue();

                total.CompanyPeriod = new CompanyPeriod
                                          {
                                              Company = new Company { Name = "TOTAL:" },
                                              Period = new DatePeriod { Id = periodId }
                                          };

                total.Value = getPeriodTotal(periodId);
                _result.Add(total);
            }
        }

        private decimal? getPeriodTotal(int periodId)
        {
            return (from r in _result
                    where r.CompanyPeriod.Period.Id == periodId
                    select r.Value).Sum();
        }

        private IEnumerable<int> getDistinctPeriods()
        {
            return (from r in _result
                    orderby r.CompanyPeriod.Period.Id descending
                    select r.CompanyPeriod.Period.Id).Distinct();
        }

        private void calculateDifferencePct()
        {
            foreach (var companyId in (_result.Select(r => r.CompanyPeriod.Company.Id).Distinct()))
            {
                var companies = getCompaniesOf(companyId);

                if (companies.Count <= 1)
                    continue;

                var previousValue = companies[0].Value ?? 0;

                for (var i = 1; i < companies.Count; i++)
                {
                    var currentValue = companies[i].Value ?? 0;
                    calculateDifferencePct(companies[i], previousValue, currentValue);
                    previousValue = currentValue;
                }
            }
        }

        private IList<CompanyPeriodValue> getCompaniesOf(int companyId)
        {
            return (from r in _result
                    where r.CompanyPeriod.Company.Id == companyId
                    orderby r.CompanyPeriod.Period.Id
                    select r).ToList();
        }

        private void calculateDifferencePct(CompanyPeriodValue company,
                                            decimal previousValue,
                                            decimal currentValue)
        {
            if (previousValue == 0)
                return;

            var diffValue = currentValue - previousValue;
            var diffPct = Math.Round((diffValue / previousValue) * 100, 2);
            company.DifferencePct = diffPct;
        }

        private CompanyPeriodValue readCompanyPeriodValue(CompanyPeriod cp)
        {
            var cpv = new CompanyPeriodValue();
            cpv.CompanyPeriod = cp;
            cpv.DifferencePct = null;
            readCompanyPeriodValue(cpv);
            return cpv;
        }

        private void readCompanyPeriodValue(CompanyPeriodValue cpv)
        {
            var cmd = db.CreateCommand(getSelectTotalSaleValueByCompanyPeriodIdQuery());
            db.AddParameterWithValue(cmd, "@companyPeriodId", cpv.CompanyPeriod.Id);
            var value = cmd.ExecuteScalar();
            cpv.Value = value == DBNull.Value ? 0 : Convert.ToDecimal(value);
        }

        public decimal? GetCompanyPeriodValue(CompanyPeriod cp)
        {
            var cmd = db.CreateCommand(getSelectTotalSaleValueByCompanyPeriodIdQuery());
            db.AddParameterWithValue(cmd, "@companyPeriodId", cp.Id);
            var value = cmd.ExecuteScalar();

            if (value == DBNull.Value)
                return null;
            
            return Convert.ToDecimal(value);
        }

        private string getSelectTotalSaleValueByCompanyPeriodIdQuery()
        {
            return string.Format(ReportQueries.SelectTotalSaleValueByCompanyPeriodId, getTypeCodes());
        }

        private string getTypeCodes()
        {
            return ChartOfAccount.GetChartOfAccountIds(AccountTypes.Customers);
        }
    }
}
