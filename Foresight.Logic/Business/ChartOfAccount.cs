using System;
using System.Text;
using ScalableApps.Foresight.Logic.Common;

namespace ScalableApps.Foresight.Logic.Business
{
    public class ChartOfAccount
    {
        public int Id { get; set; }
        public int Nr { get; set; }
        public ChartOfAccount Parent { get; set; }
        public string Sorting { get; set; }
        public string Name { get; set; }

        #region Factory Method

        public static string GetChartOfAccountIds(AccountTypes accountTypes)
        {
            return GetChartOfAccountIds((int) accountTypes);
        }

        public static string GetChartOfAccountIds(int coaId)
        {
            var coPeriods = Session.Dbc.GetCompanyPeriods();
            if (coPeriods.Count == 0 || coaId == 0)
                return " NOT IN ('') ";

            var sb = new StringBuilder(" IN (");
            var ids = Session.Dbc.GetChartOfAccountIDsFor(coaId);
            foreach (var id in ids)
                sb.Append(String.Format("'{0}',", id));

            return sb.Remove(sb.Length - 1, 1).Append(") ").ToString();
        }

        #endregion
    }
}
