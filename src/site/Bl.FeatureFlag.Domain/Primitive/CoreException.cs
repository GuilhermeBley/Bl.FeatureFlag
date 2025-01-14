namespace Bl.FeatureFlag.Domain.Primitive;

public class CoreException
    : Exception,
    ICoreException
{
    public CoreExceptionCode StatusCode {  get; private set; }

    public CoreException(CoreExceptionCode status) : this(status, status.ToString())
    {

    }

    public CoreException(CoreExceptionCode status, string? message) : this(message)
    {
        StatusCode = status;
    }

    public CoreException(string? message) : base(message)
    {
    }
}
