namespace GodotUtils.InstanceResolver.SourceGenerators.Constants;

internal static class ClassNameConst
{
    private const string internalNs = $"GodotUtils.InstanceResolver.Internal";
    private const string resolvedNodeName = "IResolvableNode";
    private const string paramsName = "IParametersBuilder";
    private const string optionalValue = "Models.OptionalValue";

    public const string BuildParametersClassName = "ParametersBuilder";
    public const string HasParamsInterface = $"{internalNs}.IHasParametersInstance";
    public const string NoParamsInterface = $"{internalNs}.INoParametersInstance";

    public static class BuildFunctionConst
    {
        public const string PassingObj = "parameters";
        public const string GetValue = "Value";
        public const string SetValue = "Set";
        public const string ValueKeyword = "value";
        public const string IsInitialized = "IsInitialized";
        public const string Name = "Build";
    }

    public static string OptionalValueField(string type)
    {
        return $"{internalNs}.{optionalValue}<{type}>";
    }

    public static string RequiredResolveInterface(string className)
    {
        return $"{internalNs}.{resolvedNodeName}<{className}, {className}.{BuildParametersClassName}>";
    }

    public static string RequiredParamsInterface(string className)
    {
        return $"{internalNs}.{paramsName}<{className}>";
    }
}
