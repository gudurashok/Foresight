using System;

namespace ScalableApps.Foresight.Logic.Report
{
    public class JVAccountDetail
    {
        public DateTime Date { get; set; }
        public string DocumentNr { get; set; }
        public string AccountName { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
    }
}
