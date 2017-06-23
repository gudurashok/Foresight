namespace ScalableApps.Foresight.Logic.Sql
{
    internal static class TcsSqlQueries
    {
        public const string CoFileName = "RPCOM.dbf";
        public const string GlGroupFileName = "RPGRP.dbf";

        public const string AccountMasterFileName = "PRT*.dbf";
        public const string TransactionFileName = "TRN*.dbf";
        
        public const string SelectAllGeneralLedgerGroups =
                                "select GR_CD, GRHEAD from " + GlGroupFileName +
                                " order by GR_CD";
    }
}

