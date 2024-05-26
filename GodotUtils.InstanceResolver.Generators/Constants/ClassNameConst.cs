namespace GodotUtils.InstanceResolver.Generators.Constants;

internal static class ClassNameConst
{
    private const string internalResolverNamespace = $"GodotUtils.InstanceResolver.Internal";
    private const string resolvedNodeName = "IResolveNode";
    private const string hasParamsName = "IHasParams";
    private const string paramsName = "IParameters";

    public const string BuildParametersClassName = "BuildParameters";
    public const string PassingObj = "parameters";

    public static string RequiredResolveInterface(string className)
    {
        return $"{internalResolverNamespace}.{resolvedNodeName}<{className}>";
    }

    public static string RequiredHasParamInterface(string className)
    {
        return $"{internalResolverNamespace}.{hasParamsName}<{className}.{BuildParametersClassName}>";
    }

    public static string RequiredParamsInterface(string className)
    {
        return $"{internalResolverNamespace}.{paramsName}<{className}>";
    }
}
