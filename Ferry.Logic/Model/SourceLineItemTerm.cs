namespace Ferry.Logic.Model
{
    public class SourceLineItemTerm
    {
        public string DaybookCode { get; set; }
        public string DocumentNr { get; set; }
        public int TermId { get; set; }
        public string Description { get; set; }
        public double Percentage { get; set; }
        public decimal Amount { get; set; }
        public string AccountCode { get; set; }
    }
}
