namespace Ferry.Logic.Model
{
    public class SourceAccount
    {
        public string GroupCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ChartOfAccountCode { get; set; }
        public decimal OpeningBalance { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pin { get; set; }
        public bool IsActive { get; set; }
    }
}
