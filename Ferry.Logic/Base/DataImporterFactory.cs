using Ferry.Logic.EASY;
using Ferry.Logic.MCS;
using Ferry.Logic.Properties;
using Ferry.Logic.TCS;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;

namespace Ferry.Logic.Base
{
    public static class DataImporterFactory
    {
        public static DataImportContext GetDataImporter(CompanyPeriod companyPeriod)
        {
            switch (companyPeriod.SourceDataProvider)
            {
                case SourceDataProvider.Easy:
                    return new EasyDataImportContext(companyPeriod);
                case SourceDataProvider.Mcs:
                    return new McsDataImportContext(companyPeriod);
                case SourceDataProvider.Tcs:
                    return new TcsDataImportContext(companyPeriod);
                default:
                    throw new ValidationException(string.Format(Resources.ImporterNotSupported, companyPeriod.SourceDataProvider));
            }
        }
    }
}
