using System;

namespace Ferry.Logic.Model
{
    public class SourceInventoryIssue
    {
        public string DaybookCode { get; set; }
        public string DocumentNr { get; set; }
        public DateTime Date { get; set; }
        public string LotNr { get; set; }
        public int LineNr { get; set; }
        public string AccountCode { get; set; }
        public double Quantity1 { get; set; }
        public double Quantity2 { get; set; }
    }
}
