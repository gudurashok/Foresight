namespace ScalableApps.Foresight.Logic.Business
{
    public class BankPayment : TransactionHeader
    {
        public string ChequeNr { get; set; }
        public bool IsRealised { get; set; }
    }
}
