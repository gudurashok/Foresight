using System.Linq;
using System.Text;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.DataAccess;

namespace ScalableApps.Foresight.Logic.Business
{
    public class ChartOfAccountMapper
    {
        #region Properties

        public int Id { get; set; }
        public int ChartOfAccountId { get; set; }
        public string ChartOfAccountName { get; set; }
        public AccountTypes Type { get; set; }
        public string EasyCode { get; set; }
        public string McsCode { get; set; }
        public string TcsCode { get; set; }

        #endregion
    }
}
