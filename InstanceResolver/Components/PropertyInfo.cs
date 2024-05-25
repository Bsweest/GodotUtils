namespace GodotUtils.InstanceResolver.Generator.Components;

internal sealed record PropertyInfo(
    string TypeNameWithNullabilityAnnotations,
    string FieldName,
    string PropertyName
);
