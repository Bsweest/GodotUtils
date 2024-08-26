namespace GodotUtils.InstanceResolver.SourceGenerators.Components;

internal sealed record PropertyInfo(
    string TypeNameWithNullabilityAnnotations,
    string FieldName,
    string WrapperName,
    string PropertyName,
    ParameterInfo AttributeInfo
);
