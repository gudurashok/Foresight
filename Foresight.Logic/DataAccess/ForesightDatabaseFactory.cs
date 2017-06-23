using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Properties;

namespace ScalableApps.Foresight.Logic.DataAccess
{
    public static class ForesightDatabaseFactory
    {
        public static ForesightDatabase GetInstance()
        {
            var genus = Util.GetGenus();

            switch (genus)
            {
                case Genus.Cheetah:
                    return new ForesightSqlCeDatabase();
                case Genus.Lion:
                    return new ForesightSqlServerDatabase();
                default:
                    throw new ValidationException(
                        string.Format(Resources.GenusNotSupported, genus));
            }
        }
    }
}
