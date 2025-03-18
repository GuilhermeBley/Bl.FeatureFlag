using System.ComponentModel.DataAnnotations;

namespace Bl.FeatureFlag.Api.Model;

public class LoginRequestViewModel
{
    [Required, EmailAddress]
    public string Login { get; set; } = string.Empty;
    [Required, Range(8,45)]
    public string Password { get; set; } = string.Empty;
}
