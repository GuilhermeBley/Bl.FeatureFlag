﻿using Bl.FeatureFlag.Domain.Extensions;
using Bl.FeatureFlag.Domain.Primitive;

namespace Bl.FeatureFlag.Domain.Entities.Flag;

public class CompleteFlagAccess
    : FlagAccess
{
    public string? Description { get; private set; }
    public string? Obs { get; private set; }

    private CompleteFlagAccess() { }

    public override bool Equals(object? obj)
    {
        return obj is CompleteFlagAccess access &&
               base.Equals(obj) &&
               EntityId.Equals(access.EntityId) &&
               RoleName == access.RoleName &&
               NormalizedRoleName == access.NormalizedRoleName &&
               Active == access.Active &&
               ExpiresAt == access.ExpiresAt &&
               CreatedAt == access.CreatedAt &&
               Description == access.Description &&
               Obs == access.Obs;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(base.GetHashCode());
        hash.Add(EntityId);
        hash.Add(RoleName);
        hash.Add(NormalizedRoleName);
        hash.Add(Active);
        hash.Add(ExpiresAt);
        hash.Add(CreatedAt);
        hash.Add(Description);
        hash.Add(Obs);
        return hash.ToHashCode();
    }

    public static Result<CompleteFlagAccess> Create(
        string roleName,
        string description,
        string obs,
        bool active,
        DateTime? expiresAt,
        DateTime createdAt)
    {
        ResultBuilder<CompleteFlagAccess> builder = new();

        roleName = roleName ?? string.Empty;
        description = description?.Trim() ?? string.Empty;
        obs = obs?.Trim() ?? string.Empty;

        builder.AddIf(
            e => e.RoleName,
            roleName.Length < 3 || roleName.Length > 255,
            CoreExceptionCode.InvalidStringLength);

        builder.AddIf(
            e => e.Description,
            roleName.Length > 2000,
            CoreExceptionCode.InvalidStringLength);

        builder.AddIf(
            e => e.Obs,
            roleName.Length > 1000,
            CoreExceptionCode.InvalidStringLength);

        return builder.CreateResult(() =>
            new CompleteFlagAccess
            {
                Active = active,
                CreatedAt = createdAt,
                ExpiresAt = expiresAt,
                NormalizedRoleName = roleName.RemoveAccents(),
                RoleName = roleName,
                Description = description,
                Obs = obs,
            }
        );
    }
}
