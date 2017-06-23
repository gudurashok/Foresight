using Ferry.Logic.MCS;
using Ferry.Logic.Sql;
using ScalableApps.Foresight.Logic.Common;

namespace Ferry.Logic.TCS
{
    public class TcsCompanyDbContext: McsCompanyDbContext
    {
        #region Constructor

        public TcsCompanyDbContext(string sourceDataPath)
            : base(sourceDataPath)
        {
        }

        #endregion

        #region Internal Methods

        protected override string GetCoFileName()
        {
            return TcsSqlQueries.CoFileName;
        }

        protected override SourceDataProvider getProvider()
        {
            return SourceDataProvider.Tcs;
        }

        #endregion
    }
}
