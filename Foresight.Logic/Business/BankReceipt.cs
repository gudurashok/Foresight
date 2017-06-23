namespace ScalableApps.Foresight.Logic.Business
{
    public class BankReceipt : TransactionHeader
    {
        public string ChequeNr { get; set; }
        public string BankBranchName { get; set; }
        public bool IsRealised { get; set; }
    }
}
