using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Properties;

namespace ScalableApps.Foresight.Logic.DataAccess
{
    public static class CoGroupDatabaseFactory
    {
        public static CoGroupDatabase GetInstance()
        {
            var genus = Util.GetGenus();

            switch (genus)
            {
                case Genus.Cheetah:
                    return new CoGroupSqlCeDatabase();
                case Genus.Lion:
                    return new CoGroupSqlServerDatabase();
                default:
                    throw new ValidationException(
                        string.Format(Resources.GenusNotSupported, genus));
            }
        }
    }
}
