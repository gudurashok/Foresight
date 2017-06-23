using System.ComponentModel;

namespace Scalable.Shared.Enums
{
    public enum LoginRole
    {
        [Description("Administrator")]
        Admin,
        [Description("User")]
        User,
        [Description("Guest")]
        Guest
    }
}
