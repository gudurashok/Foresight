using System;

namespace ScalableApps.Foresight.Logic.Business
{
    public class InventoryIssue
    {
        public int Id { get; set; }
        public Daybook Daybook { get; set; }
        public string DocumentNr { get; set; }
        public DateTime Date { get; set; }
        public int LotId { get; set; }
        public Account Account { get; set; }
        public double Quantity1 { get; set; }
        public double Quantity2 { get; set; }
        public double Quantity3 { get; set; }
    }
}
