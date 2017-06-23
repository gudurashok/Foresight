namespace ScalableApps.Foresight.Logic.Business
{
    public abstract class LineItem
    {
        public int Id { get; set; }
        public int LineNr { get; set; }
        public Item Item { get; set; }
        public string ItemDescription { get; set; }
        public double Quantity1 { get; set; }
        public double Packing { get; set; }
        public double Quantity2 { get; set; }
        public double Quantity3 { get; set; }
        public double DiscountPct { get; set; }
        public decimal Amount { get; set; }
    }
}
