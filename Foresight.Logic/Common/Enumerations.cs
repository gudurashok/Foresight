using System;
using System.ComponentModel;

namespace ScalableApps.Foresight.Logic.Common
{
    public enum SourceDataProvider
    {
        [Description("EASY")]
        Easy,
        [Description("MCS")]
        Mcs,
        [Description("TCS")]
        Tcs
    }

    public enum Genus
    {
        Cheetah, Lion
    }

    public enum UserRole
    {
        [Description("Administrator")]
        Admin,
        [Description("Finance")]
        Finance,
        [Description("Marketing")]
        Marketing,
        [Description("Purchase")]
        Purchase,
        [Description("A/c Receivable")]
        AR
    }

    public enum DatabaseProvider
    {
        SqlServer, OleDb, SqlCe, Odbc
    }

    public enum TxnType
    {
        Credit = 0,
        Debit = 1
    }

    //public enum ChartOfAccountType
    //{
    //    None = 0,
    //    Liability = 1,
    //    Asset = 2,
    //    Income = 3,
    //    Expense = 4
    //}

    public enum CommandType
    {
        Normal = 0,
        Report = 1
    }

    public enum LedgerView
    {
        Yearly = 0,
        Monthly = 1
    }

    [Flags]
    public enum AccountTypes
    {
        [Description("All")]
        All = 0,
        [Description("Customers")]
        Customers = 21, //TODO: Need to analyze more
        [Description("Vendors")]
        Vendors = 10 //TODO: Need to analyze more
    }

    [Flags]
    public enum CompanyPeriodType
    {
        Company = 1,
        Period = 2,
        Both = 3
    }

    public enum DaybookType
    {
        Unknown = 0,
        Sale = 1,
        Purchase = 2,
        Cash = 3,
        Bank = 4,
        CreditNote = 5,
        DebitNote = 6,
        JournalVoucher = 7,
        InventoryIssue = 8,
        MiscInventoryIssue = 9
    }
}

