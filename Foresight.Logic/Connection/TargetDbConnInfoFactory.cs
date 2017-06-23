using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Properties;

namespace ScalableApps.Foresight.Logic.Connection
{
    internal static class TargetDbConnInfoFactory
    {
        public static IDbConnectionInfo GetConnectionInfo(CompanyGroup companyGroup)
        {
            var genus = Util.GetGenus();

            switch (genus)
            {
                case Genus.Cheetah:
                    return new SqlCeConnectionInfo(companyGroup.FilePath, CompanyGroup.Password);
                case Genus.Lion:
                    return GetSqlConnectionInfo();
                default:
                    throw new ValidationException(string.Format(Resources.GenusNotSupported, genus));
            }
        }

        public static IDbConnectionInfo GetSqlConnectionInfo()
        {
            return new SqlServerConnectionInfo(Util.GetLionValue());
        }
    }
}
