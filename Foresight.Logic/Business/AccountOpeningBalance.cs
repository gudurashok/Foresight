using System;

namespace ScalableApps.Foresight.Logic.Business
{
    public class AccountOpeningBalance
    {
        public int Id { get; set; }
        public Account Account { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}
