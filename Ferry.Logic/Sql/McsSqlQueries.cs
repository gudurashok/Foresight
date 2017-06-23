namespace Ferry.Logic.Sql
{
    internal static class McsSqlQueries
    {
        public const string CoFileName = "MNCOM.dbf";
        public const string GlGroupFileName = "MNGRP.dbf";

        public const string AccountMasterFileName = "PRT?????.dbf";
        public const string TransactionFileName = "TRN?????.dbf";

        public const string SelectAllCompanyPeriods =
                                "select COM_CD, COM_NM, [GROUP], " +
                                " YDT1, YDT2 from {0} " +
                                " where COM_CD IS NOT NULL " +
                                " order by [GROUP] desc";

        public const string SelectAllGeneralLedgerGroups =
                                "select GR_CD, GRHEAD from " + GlGroupFileName +
                                " order by GR_CD";

        public const string SelectAllAccounts =
                                "select SB_GR, PRT_CD, PRT_NM, GR_CD," +
                                " ADD1, ADD2, ADD2A, ADD3, PIN, O_K, OPBAL" +
                                " from PRT.dbf" +
                                " order by GR_CD";

        public const string SelectAllRowColumns =
                                "select R_O, BK_CD, CD_PRT from PRS.dbf";

        public const string SelectAllItems =
                                "select QC, QN from QLY.dbf" +
                                " order by QC";

        public const string SelectAllTransactions =
                                "select BK_CD, CR_DR, PRT_CD, TR_DATE, " +
                                " VNO, PRT_BN, AMOUNT, REM, CHNO, " +
                                " CHEX, CHDT, R_O " +
                                " from TRN.dbf";

        public const string SelectAllLineItems =
                                "select BKCD6, PRTCD6, VNO6, QC6, " +
                                " QT1, QT2, QT3, RATE, AMNT " +
                                " from INM.dbf";
    }
}

