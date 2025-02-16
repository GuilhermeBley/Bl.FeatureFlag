﻿namespace Bl.FeatureFlag.Domain.Primitive;

public class DateTimeProvider
    : IDateTimeProvider
{
    public readonly static IDateTimeProvider Default = new DateTimeProvider();

    public DateTime UtcNow => DateTime.UtcNow;

    private DateTimeProvider() { }
}

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}
