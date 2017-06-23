using System;

namespace ScalableApps.Foresight.Logic.Business
{
    public class InventoryReceive
    {
        public int Id { get; set; }
        public Daybook Daybook { get; set; }
        public string DocumentNr { get; set; }
        public DateTime Date { get; set; }
        public InventoryIssue Issue { get; set; }
        public int LineNr { get; set; }
        public Item Item { get; set; }
        public double Quantity1 { get; set; }
        public double Packing { get; set; }
        public double Quantity2 { get; set; }
        public double Quantity3 { get; set; }
        public bool IsClosed { get; set; }
    }
}
