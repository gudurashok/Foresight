namespace ScalableApps.Foresight.Logic.Business
{
    public abstract class InvoiceTerm
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int TermId { get; set; }
        public string Description { get; set; }
        public double Percentage { get; set; }
        public decimal Amount { get; set; }
        public Account Account { get; set; }
    }
}
