using System;

using Microsoft.CodeAnalysis;

// https://www.variablenotfound.com/2021/03/c-source-generators-un-ejemplo-sencillo.html
// https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview
// https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.md
//
// testing generators: https://github.com/dotnet/roslyn/issues/49249#issuecomment-999351046

// not generating sources: https://github.com/dotnet/roslyn/issues/49249

namespace InteropTypes.Graphics.Bitmaps
{
    [Generator]
    public class ExplicitConstructorsGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            // TODO
        }

        public void Execute(GeneratorExecutionContext context)
        {
            // Find the main method
            var mainMethod = context.Compilation.GetEntryPoint(context.CancellationToken);
            var nameSpace = mainMethod.ContainingNamespace.ToDisplayString();

            // Build up the source code
            string source = $@" // Auto-generated code
using System;

namespace InteropTypes.Graphics.Bitmaps
{{
    partial class Pixel
    {{
        partial struct BGRA32
        {{

            // public static explicit operator BGRP32(BGRA32 other) {{ return new BGRP32(other); }}

        }}
    }}
}}";
            

            // Add the source code to the compilation
            context.AddSource("Code1.g.cs", source);
        }
    }
}
