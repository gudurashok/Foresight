using System.ComponentModel;

namespace Scalable.Shared.Enums
{
    public enum AmountFormatStyle
    {
        [Description("None")]
        None,
        [Description("Thousands")]
        Thousands,
        [Description("Lacs")]
        Lacs,
        [Description("Crores")]
        Crores
    }
}
