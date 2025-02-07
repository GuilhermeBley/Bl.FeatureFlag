namespace Bl.FeatureFlag.Domain.Primitive.Exceptions;

public class CoreException
    : Exception,
    ICoreException
{
    public CoreExceptionCode StatusCode { get; }
    public CoreException(CoreExceptionCode statusCode)
    {
        StatusCode = statusCode;
    }
}
