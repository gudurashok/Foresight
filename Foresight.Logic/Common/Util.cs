using System;
using System.IO;
using System.Reflection;
using System.Configuration;
using ScalableApps.Foresight.Logic.Connection;
using ScalableApps.Foresight.Logic.DataAccess;
using ScalableApps.Foresight.Logic.Report;
using ScalableApps.Foresight.Logic.Properties;

namespace ScalableApps.Foresight.Logic.Common
{
    public static class Util
    {
        public static string GetAppPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public static void TestDatabaseConnection()
        {
            TestDatabaseConnection(TargetDbConnInfoFactory.GetSqlConnectionInfo());
        }

        public static void TestDatabaseConnection(IDbConnectionInfo sqlConnInfo)
        {
            new Database(DbConnectionFactory.GetConnection(
                DatabaseProvider.SqlServer, sqlConnInfo.GetConnectionString()));
        }

        public static DatabaseProvider GetForesightDatabaseProvider()
        {
            var genus = GetGenus();

            switch (genus)
            {
                case Genus.Cheetah:
                    return DatabaseProvider.SqlCe;
                case Genus.Lion:
                    return DatabaseProvider.SqlServer;
                default:
                    throw new ValidationException(string.Format(Resources.GenusNotSupported, genus));
            }
        }

        public static DatabaseProvider GetSourceProvider()
        {
            var sourceProvider = ConfigurationManager.AppSettings.Get("SourceProvider");

            switch (sourceProvider)
            {
                case "SqlServer":
                    return DatabaseProvider.SqlServer;
                case "OleDb":
                    return DatabaseProvider.OleDb;
                case "SqlCe":
                    return DatabaseProvider.SqlCe;
                case "Odbc":
                    return DatabaseProvider.Odbc;
                default:
                    throw new ValidationException(
                        string.Format(Resources.SourceDatabaseProviderNotSupported, sourceProvider));
            }
        }

        public static Genus GetGenus()
        {
            var genus = ConfigurationManager.AppSettings.Get("Genus");

            switch (genus)
            {
                case "Cheetah":
                    return Genus.Cheetah;
                case "Lion":
                    return Genus.Lion;
                default:
                    throw new ValidationException(string.Format("Incorrect Genus {0}", genus));
            }
        }

        public static string GetLionValue()
        {
            return ConfigurationManager.AppSettings.Get("Lion");
        }

        public static string GetDataPath()
        {
            return ConfigurationManager.AppSettings.Get("DataPath");
        }

        public static object ConvertToDbValue(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return DBNull.Value;
            
            return value;
        }

        public static object ConvertDbNull(object value)
        {
            return DBNull.Value == value ? null : value;
        }

        public static string ConvertDbNullToString(object value)
        {
            return DBNull.Value == value ? string.Empty : value.ToString();
        }

        public static decimal GetAmountFormatValue(ReportsAmountFormat format)
        {
            if (format == ReportsAmountFormat.Crores) return 10000000;
            if (format == ReportsAmountFormat.Lacs) return 100000;
            if (format == ReportsAmountFormat.Thousands) return 1000;
            return 1;
        }

        public static Assembly GetSqlCeAssembly()
        {
#if DEBUG
            return Assembly.LoadFrom(getSqlCeAssemblyPath());
#else
            return Assembly.LoadFrom(string.Format(@"{0}\Private\System.Data.SqlServerCe.dll", GetAppPath()));
#endif
        }

#if DEBUG

        private static string getSqlCeAssemblyPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) +
                    @"\Microsoft SQL Server Compact Edition\v4.0\Desktop\System.Data.SqlServerCe.dll";

        }
#endif

    }
}
