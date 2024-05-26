namespace GodotUtils.InstanceResolver.Generators.Components;

internal sealed record PropertyInfo(
    string TypeNameWithNullabilityAnnotations,
    string FieldName,
    string PropertyName,
    ParameterInfo AttributeInfo
);
