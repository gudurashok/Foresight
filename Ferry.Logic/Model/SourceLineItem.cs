using System;

namespace Ferry.Logic.Model
{
    public class SourceLineItem
    {
        public int LineNr { get; set; }
        public string DaybookCode { get; set; }
        public string AccountCode { get; set; }
        public string DocumentNr { get; set; }
        public DateTime Date { get; set; }        
        public string ItemCode { get; set; } 
        public string ItemName { get; set; } 
        public double Quantity1 { get; set; }
        public double Quantity2 { get; set; }
        public double Quantity3 { get; set; }
        public double Packing { get; set; }
        public decimal Price { get; set; }
        public decimal LineItemAmount { get; set; }
        public double DiscountPct { get; set; }
    }
}
