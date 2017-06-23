using ScalableApps.Foresight.Logic.Common;

namespace ScalableApps.Foresight.Logic.Business
{
    public class Daybook
    {
        public int Id { get; set; }
        public DaybookType Type { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Account Account { get; set; }
    }
}
