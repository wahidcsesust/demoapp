using System.ComponentModel;
using System.Runtime.Serialization;

namespace HealthCare.Web.Models
{
    public enum EnumOrderStatus
    {
        [EnumMember, Description("Outstanding")]
        Outstanding = 1,
        [EnumMember, Description("InProgress")]
        InProgress = 2,
        [EnumMember, Description("Completed")]
        Completed = 3
    }
}
