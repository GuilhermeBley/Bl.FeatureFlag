using System.Collections.Immutable;

namespace Bl.FeatureFlag.Domain.Primitive.Exceptions;

public class AggregateCoreException
    : CoreException
{
    private readonly IReadOnlyList<ICoreException> _exceptions;
    public IReadOnlyList<ICoreException> InnerExceptions => _exceptions;

    public AggregateCoreException(IEnumerable<ICoreException> coreExceptions)
        : this(coreExceptions.ToArray()) { }

    public AggregateCoreException(params ICoreException[] coreExceptions) 
        : base(CoreExceptionCode.BadRequest)
    {
        ArgumentOutOfRangeException.ThrowIfZero(coreExceptions.Length, nameof(coreExceptions));

        _exceptions = coreExceptions.ToImmutableArray();
    }
}
