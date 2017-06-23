namespace Ferry.Logic.Model
{
    public class SourceItem
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string GroupCode { get; set; }
        public string CategoryCode { get; set; }
        public bool HasBom { get; set; }
        public bool IsActive { get; set; }
    }
}
