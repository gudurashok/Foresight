using System;

namespace ScalableApps.Foresight.Logic.Business
{
    public abstract class TransactionHeader
    {
        public int Id { get; set; }
        public Daybook Daybook { get; set; }
        public string DocumentNr { get; set; }
        public DateTime Date { get; set; }
        public Account Account { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
        public int BrokerId { get; set; }
        public bool IsAdjusted { get; set; }
    }
}
