using System;

namespace ScalableApps.Foresight.Logic.Report
{
    public class LedgerDetail
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string DaybookName { get; set; }
        public string DocumentNr { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
    }
}
