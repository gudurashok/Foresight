using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.DataAccess;

namespace ScalableApps.Foresight.Logic.Common
{
    public static class Session
    {
        public static Login Login { get; set; }
        public static CompanyGroup CompanyGroup { get; set; }
        public static DataContext Dbc { get; private set; }

        public static void OpenCompanyGroup(CompanyGroup group)
        {
            CompanyGroup = group;
            Dbc = new DataContext(group);
        }
    }
}
