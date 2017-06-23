namespace ScalableApps.Foresight.Logic.Business
{
    public class Item
    {
        public string Code { get; set; }
        public int Id { get; set; }
        public ItemGroup Group { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public ItemCategory Category { get; set; }
        public bool HasBom { get; set; }
        public bool IsActive { get; set; }
    }
}
