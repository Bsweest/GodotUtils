using GodotUtils.InstanceResolver.Generator.Components;
using GodotUtils.InstanceResolver.Generator.Helper;
using Microsoft.CodeAnalysis;

namespace GodotUtils.InstanceResolver.Generator.Extensions;

internal static class IncrementalContextExtensions
{
    public static void ReportDiagnostics(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<EquatableArray<DiagnosticInfo>> diagnostics
    )
    {
        context.RegisterSourceOutput(
            diagnostics,
            static (context, diagnostics) =>
            {
                foreach (DiagnosticInfo diagnostic in diagnostics)
                {
                    context.ReportDiagnostic(diagnostic.ToDiagnostic());
                }
            }
        );
    }

    public static void ReportDiagnostics(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<Diagnostic> diagnostics
    )
    {
        context.RegisterSourceOutput(
            diagnostics,
            static (context, diagnostic) => context.ReportDiagnostic(diagnostic)
        );
    }
}
