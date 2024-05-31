namespace GodotUtils.InstanceResolver.Generators.Components;

internal sealed record PropertyInfo(
    string TypeNameWithNullabilityAnnotations,
    string FieldName,
    string WrapperName,
    string PropertyName,
    ParameterInfo AttributeInfo
);
