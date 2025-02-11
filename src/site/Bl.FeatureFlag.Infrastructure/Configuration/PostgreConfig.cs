using System.ComponentModel.DataAnnotations;

namespace Bl.FeatureFlag.Infrastructure.Configuration;

public class PostgreConfig
{
    public const string SECTION = nameof(PostgreConfig);

    [Required]
    [MinLength(10)]
    public string ConnectionString { get; set; } = string.Empty;
}
