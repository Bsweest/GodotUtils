namespace GodotUtils.InstanceResolver.Generators.Constants;

internal static class ClassNameConst
{
    private const string internalResolverNamespace = $"GodotUtils.InstanceResolver.Internal";
    private const string resolvedNodeName = "IResolvedNode";
    private const string paramsName = "IParameters";
    private const string optionalValue = "Models.OptionalValue";

    public const string BuildParametersClassName = "BuildParameters";
    public const string PassingObj = "parameters";
    public const string GetValue = "Value";
    public const string SetValue = "Set";
    public const string ValueKeyword = "value";
    public const string IsInitialized = "IsInitialized";

    public static string OptionalValueField(string type)
    {
        return $"{internalResolverNamespace}.{optionalValue}<{type}>";
    }

    public static string RequiredResolveInterface(string className)
    {
        return $"{internalResolverNamespace}.{resolvedNodeName}<{className}, {className}.{BuildParametersClassName}>";
    }

    public static string RequiredParamsInterface(string className)
    {
        return $"{internalResolverNamespace}.{paramsName}<{className}>";
    }
}
