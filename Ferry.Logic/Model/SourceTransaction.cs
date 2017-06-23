using System;

namespace Ferry.Logic.Model
{
    public class SourceTransaction
    {
        public string DaybookCode { get; set; }
        public string TransactionType { get; set; }
        public string AccountCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string DocumentNr { get; set; }
        public decimal Amount { get; set; }
        public decimal BrokerageAmount { get; set; }
        public string Notes { get; set; }
        public string ChequeNr { get; set; }
        public string BankBranchName { get; set; }
        public string ChequeDate { get; set; }
        public string RowColumnCode { get; set; }
        public string Through { get; set; }
        public string Transport { get; set; }
        public string ReferenceDocNr { get; set; }
        public double DiscountPct { get; set; }
        public bool IsAdjusted { get; set; }

        public string ShipToName { get; set; }
        public string ShipToAddressLine1 { get; set; }
        public string ShipToAddressLine2 { get; set; }
        public string ShipToCity { get; set; }

    }
}
