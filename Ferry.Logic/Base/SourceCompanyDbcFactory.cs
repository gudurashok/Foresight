using Ferry.Logic.EASY;
using Ferry.Logic.MCS;
using Ferry.Logic.Properties;
using Ferry.Logic.TCS;
using ScalableApps.Foresight.Logic.Common;

namespace Ferry.Logic.Base
{
    public static class SourceCompanyDbcFactory
    {
        public static SourceCompanyDbContext GetInstance(SourceDataProvider provider, 
                                                                string sourceDataPath)
        {
            switch (provider)
            {
                case SourceDataProvider.Easy:
                    return new EasyCompanyDbContext(sourceDataPath);
                case SourceDataProvider.Mcs:
                    return new McsCompanyDbContext(sourceDataPath);
                case SourceDataProvider.Tcs:
                    return new TcsCompanyDbContext(sourceDataPath);
                default:
                    throw new ValidationException(string.Format(Resources.SourceDatabaseProviderNotSupported, provider));
            }
        }
    }
}
