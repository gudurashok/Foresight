using ScalableApps.Foresight.Logic.Common;

namespace ScalableApps.Foresight.Logic.Business
{
    public class CompanyPeriod
    {
        public int Id { get; set; }
        public Company Company { get; set; }
        public DatePeriod Period { get; set; }
        public SourceDataProvider SourceDataProvider { get; set; }
        public string DataPath { get; set; }
        public bool IsImported { get; set; }

        public bool IsNew()
        {
            return Id == 0;
        }
    }
}
