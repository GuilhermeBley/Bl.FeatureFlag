using System.Security.Claims;

namespace Bl.FeatureFlag.Domain.Security;

public static class UserClaim
{
    public const string DEFAULT_ROLE = "role";
    public const string DEFAULT_USER_NAME = "name";
    public const string DEFAULT_USER_ID = "id";
    public const string DEFAULT_USER_EMAIL = "email";

    public static Claim SeeFlags => new(DEFAULT_ROLE, "SeeFlags");
    public static Claim ManageFlags => new(DEFAULT_ROLE, "ManageFlags");

    public static Claim CreateUserNameClaim(string userName)
        => new(DEFAULT_USER_NAME, userName);
    public static Claim CreateUserIdClaim(Guid userId)
        => new(DEFAULT_USER_ID, userId.ToString());
    public static Claim CreateUserEmailClaim(string email)
        => new(DEFAULT_USER_EMAIL, email);
}
