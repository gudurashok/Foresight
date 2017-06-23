namespace ScalableApps.Foresight.Logic.Sql
{
    internal static class EasySqlQueries
    {
        public const string PlantAndMachineryGlgCode = "20102";
        public const string FactoryAccountGlgCode = "99999";

        public const string FsEasyCoGroupFileName = "EF_COGRP.dbf";
        public const string FsEasyCoFileName = "EF_CO.dbf";

        public const string EasyCoGroupFileName = "CO_GRP.dbf";
        public const string EasyCoFileName = "CO_FILE.dbf";

        public const string AccountMasterFileName = "ACC_MAST.ASK";
        public const string TransactionFileName = "TXN_FILE.ASK";

        public const string SelectAllCompanyGroups =
                                "select CO_GRP, CO_GNAME from " + FsEasyCoGroupFileName;

        public const string SelectAllCompaniesOfGroupCode =
                                "select CO_CODE, CO_NAME, FINYEAR_BD, " +
                                " FINYEAR_ED, DIR_NAME from " + FsEasyCoFileName +
                                " where ([CO_GRP]=?)";

        public const string SelectAllTransactions =
                                "select DOC_NO, DATE, ACC_CODE, DB_CR, NET_AMT, " +
                                "THROUGH, TRANSPORT, REF_DOC, DISC, " +
                                "ADJUSTED, PARTICULAR, CHEQUE, CHQ_FROM, PASSED, " +
                                "SHP_NAME, SHP_ADDR1, SHP_ADDR2, SHP_CITY " +
                                " from TXN_FILE.dbf" +
                                " order by DATE, DB_CR, DOC_NO";

        public const string SelectAllInvoiceTerms =
                                "select M.DBK_CODE, T.DOC_NO, T.CODE, T.PERCENTAGE, T.AMOUNT," +
                                " T.ACC_CODE, M.DESCRIPT from BILLTERM.dbf T" +
                                " inner join SP_TERMS.dbf M" +
                                " on M.DBK_CODE = left(T.DOC_NO,4) and M.CODE = T.CODE" +
                                " order by T.CODE";

        public const string SelectAllInventoryIssues =
                                "select H.DATE, H.ACC_CODE, T.* from OIP_HEAD.dbf H" +
                                " inner join TRN_OIP.dbf T on H.DOC_NO = T.DOC_NO" +
                                " order by H.DATE, H.DOC_NO";

        public const string SelectAllInventoryReceives =
                                "select DOC_NO, DATE, " +
                                "SRNO, ITM_CODE, PCS, BHARTI, QTY " +
                                "from TRN_IRP.dbf" +
                                " order by DOC_NO";

        public const string SelectAllMiscInventoryIssues =
                                "select DOC_NO, SRNO, DATE, " +
                                "ITM_CODE, PCS, QTY " +
                                "from TRN_MIP.dbf" +
                                " order by DATE, DOC_NO";

        public const string SelectAllInvoiceLines =
                                " select SRNO, ITM_CODE, ITM_NAME, " +
                                "  PCS, BHARTI, QTY, RATE, DISCOUNT, AMOUNT, " +
                                "  DOC_NO, DATE, ACC_CODE " +
                                "  from TRN_IPP.dbf " +
                                "union all " +
                                " select SRNO, ITM_CODE, ITM_NAME, " +
                                "  PCS, BHARTI, QTY, RATE, DISCOUNT, AMOUNT, " +
                                "  DOC_NO, DATE, ACC_CODE " +
                                "  from TRN_OFS.dbf ";

        public const string SelectAllAccountOpeningBalances =
                                "select ACC_CODE, OP_BAL, OPB_CR_DR " +
                                " from ACC_MAST.dbf " +
                                " WHERE OP_BAL <> 0";

        public const string SelectAllDaybooks =
                                "select DBK_TYPE, DBK_CODE, DBK_NAME," +
                                " ACC_CODE from DBK_MAST.dbf" +
                                " where DBK_TYPE <> 'Y' AND DBK_TYPE <> 'Z'" +
                                " order by DBK_TYPE, DBK_CODE";

        public const string SelectAllAccounts =
                                "select GLG_CODE, GRP_CODE, ACC_CODE, ACC_NAME, " +
                                " OP_BAL, OPB_CR_DR, OFF_ADDR1, OFF_ADDR2, " +
                                " CITY, STATE, PIN, ISACTIVE " +
                                " from ACC_MAST.dbf" +
                                " order by GLG_CODE";

        public const string SelectAllGeneralLedgerGroups =
                                "select GLG_CODE, GLG_NAME from GLG_MAST.dbf";

        public const string SelectAllItemCategories =
                                "select CATAGORY, ICAT_DESC from ITEMCAT.dbf";

        public const string SelectAllItemGroups =
                                "select ITM_GRP, GRP_NAME from ITM_GRP.dbf";

        public const string SelectAllItems =
                                "select ITM_CODE, CATAGORY, ITM_GRP, " +
                                "ITM_NAME, SHORT_NAME, BOM, ISACTIVE " +
                                "from ITM_MAST.dbf";
    }
}

