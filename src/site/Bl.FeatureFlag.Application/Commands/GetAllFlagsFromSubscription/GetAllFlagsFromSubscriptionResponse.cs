using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.FeatureFlag.Application.Commands.GetAllFlagsFromSubscription
{
    public class GetAllFlagsFromSubscriptionSubscriptionInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }


    public record GetAllFlagsFromSubscriptionResponse
    {
        public Guid FlagId { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string NormalizedGroupName { get; set; } = string.Empty;
        public bool Active { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public DateTime FlagCreatedAt { get; set; }
    }
}
