namespace ScalableApps.Foresight.Logic.Business
{
    public class Account
    {
        public string Code { get; set; }
        public int Id { get; set; }
        public Account Group { get; set; }
        public ChartOfAccount ChartOfAccount { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pin { get; set; }
        public int ContactId { get; set; }
        public bool IsActive { get; set; }
        public decimal BalanceAmount { get; set; }

        public string GetAddressString()
        {
            return string.Format("{0} {1} {2} {3} {4}", 
                        AddressLine1, AddressLine2, City, State, Pin);
        }
    }
}
