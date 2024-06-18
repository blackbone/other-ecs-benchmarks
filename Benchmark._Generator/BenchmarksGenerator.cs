using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Benchmark._Generator;

[Generator]
public class BenchmarksGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var log = new StringBuilder();
        try
        {
            ExecuteInner(context, log);
        }
        catch (Exception e)
        {
            log.AppendLine();
            log.AppendLine(e.ToString());
        }

        context.AddSource("Log.g.cs", $"/*\n{log}*/");
    }

    public void ExecuteInner(GeneratorExecutionContext context, StringBuilder log)
    {
        var compilation = context.Compilation;

        // get base context type
        var contextInterfaceType = compilation.GetTypeByMetadataName("Benchmark._Context.IBenchmarkContext");
        log.AppendLine(contextInterfaceType!.ContainingNamespace + "." + contextInterfaceType!.Name);

        // get base benchmark type
        var benchmarkInterfaceType = compilation.GetTypeByMetadataName("Benchmark.IBenchmark");
        log.AppendLine(benchmarkInterfaceType!.ContainingNamespace + "." + benchmarkInterfaceType!.Name);

        // get all context implementation types
        var contexts = CollectImplementations(compilation, contextInterfaceType);

        log.AppendLine("CONTEXTS:");
        log.AppendLine(string.Join("\n",
            contexts.Select(ctx =>
                $"\t{ctx.ContainingNamespace}.{ctx.Name} :: {ctx.Locations[0].GetLineSpan().Path}")));

        // get all benchmark types
        var benchmarks = CollectImplementations(compilation, benchmarkInterfaceType,
            t => t.IsAbstract && t.IsGenericType && t.TypeKind != TypeKind.Interface);

        log.AppendLine("BENCHMARKS:");
        log.AppendLine(string.Join("\n",
            benchmarks.Select(b => $"\t{b.ContainingNamespace}.{b.Name} :: {b.Locations[0].GetLineSpan().Path}")));

        log.AppendLine("CONSTRUCTS:");
        var byBenchmark = new Dictionary<string, List<string>>();
        var byContext = new Dictionary<string, List<string>>();

        // construct Benchmark_Context : IBenchmark<TContext>
        foreach (var benchmarkType in benchmarks)
        {
            var benchmarkFullName = $"{benchmarkType.ContainingNamespace}.{benchmarkType.Name}";
            foreach (var contextType in contexts)
            {
                var contextFullName = $"{contextType.ContainingNamespace}.{contextType.Name}";
                var name = benchmarkType.Name + "_" + contextType.Name;
                log.AppendLine($"\t{name}");

                var typeSyntax = ClassDeclaration(name)
                        .AddAttributeLists(benchmarkType.GetTypeDeclarationSyntax().AttributeLists
                            .ToArray()) // copy attribute lists
                        .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                        .AddMembers(benchmarkType.GetTypeDeclarationSyntax().Members.ToArray()) // copy contents
                        .WithBaseList(BaseList(SeparatedList<BaseTypeSyntax>(new[]
                            { SimpleBaseType(ParseTypeName($"IBenchmark<{contextFullName}>")) })));

                var namespaceSyntax = NamespaceDeclaration(ParseName("Benchmark.Generated"))
                    .AddMembers(typeSyntax);

                var source = CompilationUnit()
                    .AddUsings(
                        UsingDirective(ParseName("System")),
                        UsingDirective(ParseName("Benchmark._Context")),
                        UsingDirective(ParseName("BenchmarkDotNet.Attributes")),
                        UsingDirective(ParseName("Benchmark.Utils")))
                    .AddMembers(namespaceSyntax)
                    .NormalizeWhitespace()
                    .ToFullString();

                if (contextType.Name == "ArchContext")
                    source = source.Replace("[Benchmark]", "[Benchmark(Baseline = true)]");

                source = source
                    .Replace($"\" + nameof({benchmarkType.Name}<T>)", $"{benchmarkType.Name}\"")
                    .Replace("public T Context { get; set; }", $"public {contextFullName} Context {{ get; set; }}")
                    .Replace("Context = BenchmarkContext.Create<T>(EntityCount);",
                        $"Context = new {contextFullName}(EntityCount);");

                context.AddSource($"{name}.g.cs", SourceText.From(source, Encoding.UTF8));
                var caseFullName = $"Benchmark.Generated.{name}";

                byBenchmark.TryAdd(benchmarkFullName, []);
                byBenchmark[benchmarkFullName].Add(caseFullName);

                byContext.TryAdd(contextFullName, []);
                byContext[contextFullName].Add(caseFullName);
            }
        }

        {
            var typeSyntax = ClassDeclaration("BenchMap")
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)))
                .AddMembers(GenerateBenchmarkMapping(byBenchmark, byContext));

            var namespaceSyntax = NamespaceDeclaration(ParseName("Benchmark"))
                .AddMembers(typeSyntax);

            var source = CompilationUnit()
                .AddUsings(
                    UsingDirective(ParseName("System")),
                    UsingDirective(ParseName("System.Collections.Generic")))
                .AddMembers(namespaceSyntax)
                .NormalizeWhitespace()
                .ToFullString();

            context.AddSource("BenchMap.g.cs", SourceText.From(source, Encoding.UTF8));
        }
    }

    private static MemberDeclarationSyntax[] GenerateBenchmarkMapping(
        Dictionary<string, List<string>> byBenchmark, Dictionary<string, List<string>> byContext)
    {
        var sb = new StringBuilder();
        var members = new MemberDeclarationSyntax[2];
        sb.AppendLine("public static Dictionary<Type, Type[]> Runs = new (){");
        foreach (var (bench, impls) in byBenchmark.OrderBy(kv => kv.Key))
            sb.AppendLine($"{{typeof({bench}<>), [{string.Join(", ", impls.Select(i => $"typeof({i})"))}]}},");
        sb.AppendLine("};");
        var member = ParseMemberDeclaration(sb.ToString());
        if (member != null) members[0] = member.NormalizeWhitespace();

        sb.Clear();
        sb.AppendLine("public static Dictionary<Type, Type[]> Contexts = new (){");
        foreach (var (ctx, impls) in byContext?.OrderBy(kv => kv.Key).ToArray() ?? [])
            sb.AppendLine($"{{typeof({ctx}), [{string.Join(", ", impls.Select(i => $"typeof({i})"))}]}},");
        sb.AppendLine("};");

        member = ParseMemberDeclaration(sb.ToString());
        if (member != null) members[1] = member.NormalizeWhitespace();

        return members;
    }

    private List<INamedTypeSymbol> CollectImplementations(Compilation compilation, INamedTypeSymbol interfaceType,
        Func<INamedTypeSymbol, bool> filter = null)
    {
        var implementations = new List<INamedTypeSymbol>();

        // Check the current compilation
        CollectImplementationsInternal(compilation.Assembly);

        // Check referenced assemblies
        foreach (var referencedAssembly in compilation.ReferencedAssemblyNames)
        {
            var referencedCompilation = compilation.References
                .Select(compilation.GetAssemblyOrModuleSymbol)
                .OfType<IAssemblySymbol>()
                .FirstOrDefault(assembly => assembly.Identity.Equals(referencedAssembly));

            if (referencedCompilation != null) CollectImplementationsInternal(referencedCompilation);
        }

        return implementations;

        void CollectImplementationsInternal(IAssemblySymbol assembly)
        {
            var values = assembly.GlobalNamespace.GetNamespaceTypes()
                .Where(type => type.AllInterfaces.Contains(interfaceType));
            if (filter != null) values = values.Where(filter);
            implementations.AddRange(values);
        }
    }
}

public static class SymbolExtensions
{
    public static TypeDeclarationSyntax GetTypeDeclarationSyntax(this INamedTypeSymbol typeSymbol)
    {
        var syntaxReference = typeSymbol.DeclaringSyntaxReferences.FirstOrDefault();
        if (syntaxReference == null) return null;

        var syntaxNode = syntaxReference.GetSyntax();
        return syntaxNode as TypeDeclarationSyntax;
    }

    public static IEnumerable<INamedTypeSymbol> GetNamespaceTypes(this INamespaceSymbol namespaceSymbol)
    {
        foreach (var member in namespaceSymbol.GetMembers())
            if (member is INamespaceSymbol nestedNamespace)
            {
                foreach (var nestedType in nestedNamespace.GetNamespaceTypes())
                    yield return nestedType;
            }
            else if (member is INamedTypeSymbol namedType)
                yield return namedType;
    }
}