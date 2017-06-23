namespace ScalableApps.Foresight.Logic.Report
{
    public class NewPartyDataContext : PartyAssociationDataContext
    {
        protected override string getInnerQueryFilterOpr()
        {
            return "<";
        }

        protected override string getOuterQueryFilterOpr()
        {
            return ">=";
        }
    }
}
