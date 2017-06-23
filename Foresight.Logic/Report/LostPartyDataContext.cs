namespace ScalableApps.Foresight.Logic.Report
{
    public class LostPartyDataContext : PartyAssociationDataContext
    {
        protected override string getInnerQueryFilterOpr()
        {
            return ">=";
        }

        protected override string getOuterQueryFilterOpr()
        {
            return "<";
        }
    }
}
