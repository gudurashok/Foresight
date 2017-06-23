using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScalableApps.Foresight.Logic.Report
{
   public class TrialBalance
    {
       public int AccountId { get; set; }
       public string ChartOfAccountName { get; set; }
       public string AccountName { get; set; }
       public decimal Opening { get; set; }
       public decimal TransactionCredit { get; set; }
       public decimal TransactionDebit{ get; set; }
       public decimal Closing { get; set; }
    }
}
