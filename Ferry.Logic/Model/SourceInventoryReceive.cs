using System;

namespace Ferry.Logic.Model
{
    public class SourceInventoryReceive
    {
        public string DocumentNr { get; set; }
        public DateTime Date { get; set; }
        public string IssueDocNr { get; set; }
        public int LineNr { get; set; }
        public string ItemCode { get; set; }
        public double Quantity1 { get; set; }
        public double Quantity2 { get; set; }
        public double Packing { get; set; }
    }
}
