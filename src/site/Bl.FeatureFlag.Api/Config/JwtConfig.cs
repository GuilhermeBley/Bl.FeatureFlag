using System.ComponentModel.DataAnnotations;

namespace Bl.FeatureFlag.Api.Config;

public class JwtConfig
{
    [Required, StringLength(int.MaxValue, MinimumLength = 10)]
    public string Key { get; set; } = string.Empty;
}
