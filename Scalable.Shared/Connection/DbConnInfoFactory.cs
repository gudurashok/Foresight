using Scalable.Shared.Common;
using Scalable.Shared.DataAccess;
using Scalable.Shared.Enums;
using Scalable.Shared.Properties;

namespace Scalable.Shared.Connection
{
    public static class DbConnInfoFactory
    {
        public static IDbConnectionInfo GetConnectionInfo(Database database)
        {
            var genus = ScalableUtil.GetAppGenus();

            switch (genus)
            {
                case Genus.Cheetah:
                    return new SqlCeConnectionInfo(database);
                case Genus.Lion:
                    return GetSqlConnectionInfo();
                default:
                    throw new ValidationException(string.Format(Resources.GenusNotSupported, genus));
            }
        }

        public static IDbConnectionInfo GetSqlConnectionInfo()
        {
            return new SqlServerConnectionInfo(ScalableUtil.GetAppGenusLionValue());
        }
    }
}
