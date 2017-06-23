using System;

namespace Scalable.Shared.Domain
{
    public class DatePeriod
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public override string ToString()
        {
            return string.Format("({0}-{1})", From.ToShortDateString(), To.ToShortDateString()); 
        }

        public string ToYearString()
        {
            return string.Format("{0}-{1}", From.Year, To.Year);
        }
    }
}
