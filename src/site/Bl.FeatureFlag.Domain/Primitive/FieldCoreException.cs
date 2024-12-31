using System.Linq.Expressions;
using System.Reflection;

namespace Bl.FeatureFlag.Domain.Primitive;

public class FieldCoreException
    : IFieldCoreException
{
    public Type ClassType { get; } = null!;
    public Type PropertyType { get; } = null!;
    public string FieldName { get; } = string.Empty;
    public CoreExceptionCode StatusCode { get; }
    public string? Message { get; }

    internal FieldCoreException(Type classType, Type propertyType, string fieldName, CoreExceptionCode statusCode, string? message)
    {
        ClassType = classType;
        PropertyType = propertyType;
        FieldName = fieldName;
        StatusCode = statusCode;
        Message = message;
    }

    public static FieldCoreException Create<T, TProperty>(
        Expression<Func<T, TProperty>> fieldError,
        CoreExceptionCode code)
        => Create(fieldError, code.ToString(), code);

    public static FieldCoreException Create<T, TProperty>(
        Expression<Func<T, TProperty>> fieldError,
        string errorDescription)
        => Create(fieldError, errorDescription, CoreExceptionCode.BadRequest);

    public static FieldCoreException Create<T, TProperty>(
        Expression<Func<T, TProperty>> fieldError,
        string errorDescription,
        CoreExceptionCode code)
    {
        var result = GetPropertyDetails(fieldError);

        return new FieldCoreException
        (
            fieldName: result.PropertyName,
            message: errorDescription,
            statusCode: code,
            classType: result.ClassType,
            propertyType: result.PropertyType
        );
    }

    private static (Type ClassType, string PropertyName, Type PropertyType) GetPropertyDetails<T, TProperty>(
        Expression<Func<T, TProperty>> propertyExpression)
    {
        if (propertyExpression.Body is MemberExpression memberExpression)
        {
            if (memberExpression.Member is PropertyInfo propertyInfo)
            {
                return (typeof(T), propertyInfo.Name, propertyInfo.PropertyType);
            }
        }
        else if (propertyExpression.Body is UnaryExpression unaryExpression &&
                 unaryExpression.Operand is MemberExpression operandMember)
        {
            if (operandMember.Member is PropertyInfo propertyInfo)
            {
                return (typeof(T), propertyInfo.Name, propertyInfo.PropertyType);
            }
        }

        throw new ArgumentException("The expression does not refer to a valid property.", nameof(propertyExpression));
    }
}
