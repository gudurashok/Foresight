using Ferry.Logic.Properties;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Connection;

namespace Ferry.Logic.Connection
{
    internal static class SourceDbConnInfoFactory
    {
        public static IDbConnectionInfo GetConnectionInfo(string path)
        {
            var provider = Util.GetSourceProvider();

            switch (provider)
            {
                case DatabaseProvider.Odbc:
                    return new OdbcConnectionInfo(path);
                case DatabaseProvider.OleDb:
                    return new OleDbConnectionInfo(path);
                default:
                    throw new ValidationException(
                        string.Format(Resources.SourceDatabaseProviderNotSupported, provider));
            }
        }
    }
}
