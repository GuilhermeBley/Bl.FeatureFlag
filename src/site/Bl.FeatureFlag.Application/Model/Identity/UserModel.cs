using Microsoft.AspNetCore.Identity;

namespace Bl.FeatureFlag.Application.Model.Identity;

public class UserModel : IdentityUser<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? NickName { get; set; }
}
