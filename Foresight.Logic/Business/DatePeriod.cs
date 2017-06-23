using System;

namespace ScalableApps.Foresight.Logic.Business
{
    public class DatePeriod
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime FinancialFrom { get; set; }
        public DateTime FinancialTo { get; set; }
        public DateTime AssesmentFrom { get; set; }
        public DateTime AssesmentTo { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1} to {2})", 
                                Name, 
                                FinancialFrom.ToShortDateString(), 
                                FinancialTo.ToShortDateString()); 
        }

        public string GetNameFromFinancialPeriod()
        {
            return string.Format("{0}-{1}", FinancialFrom.Year, FinancialTo.Year);
        }

        public void SetAssessmentPeriodFromFinancialPeriod()
        {
            AssesmentFrom = FinancialFrom.AddYears(1);
            AssesmentTo = FinancialTo.AddYears(1);
        }
    }
}
