// BenchmarkGenerator3.cs
// Incremental Source Generator that replicates BenchmarkGenerator2 output
// (Components.g.cs, <Bench>_<Context>.g.cs, BenchMap.g.cs)
// but uses an incremental pipeline and avoids mutating the user compilation.
// Target: netstandard2.1

#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Benchmark._Generator;

[Generator(LanguageNames.CSharp)]
public sealed class BenchmarkGenerator3 : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Combine the current compilation with all additional files (collected for optional fallback analysis only)
        var input = context.CompilationProvider.Combine(context.AdditionalTextsProvider.Collect());

        context.RegisterSourceOutput(input, static (spc, pair) =>
        {
            var (compilation, additional) = pair;

            // 1) Emit Components.g.cs (identical to v2)
            EmitComponents(spc);

            // 2) Discover contexts & benchmarks from the main compilation
            var analysis = compilation;
            var ctxIface = analysis.GetTypeByMetadataName("Benchmark.Context.IBenchmarkContext");
            var benchIface = analysis.GetTypeByMetadataName("Benchmark.IBenchmark");

            // If interfaces not found in the main graph, try a lightweight fallback
            if (ctxIface == null || benchIface == null)
            {
                var firstTree = compilation.SyntaxTrees.FirstOrDefault();
                var parseOptions = firstTree is null
                    ? new CSharpParseOptions(LanguageVersion.CSharp9)
                    : (CSharpParseOptions)firstTree.Options;

                var trees = new List<SyntaxTree>();
                foreach (var add in additional)
                {
                    if (!add.Path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                        continue;
                    var text = add.GetText(spc.CancellationToken);
                    if (text is null) continue;
                    trees.Add(CSharpSyntaxTree.ParseText(text, parseOptions));
                }

                if (trees.Count != 0)
                {
                    analysis = CSharpCompilation.Create(
                        compilation.AssemblyName + ".BenchmarkGenerator3.Analysis",
                        compilation.SyntaxTrees.Concat(trees),
                        compilation.References,
                        (CSharpCompilationOptions)compilation.Options);
                    ctxIface = analysis.GetTypeByMetadataName("Benchmark.Context.IBenchmarkContext");
                    benchIface = analysis.GetTypeByMetadataName("Benchmark.IBenchmark");
                }
            }

            if (ctxIface == null || benchIface == null)
            {
                // Emit a tiny log to help debugging generator setup
                spc.AddSource("BenchmarksGenerator/_Log.g.cs", SourceText.From("/* BenchmarkGenerator3: interfaces not found */", Encoding.UTF8));
                return;
            }

            // 3) Collect
            var contexts = CollectImplementations(analysis, ctxIface, t => !t.IsAbstract && !t.IsGenericType && t.TypeKind == TypeKind.Class);
            var benchmarks = CollectImplementations(analysis, benchIface, t => t.IsAbstract && t.IsGenericType && t.TypeKind != TypeKind.Interface);

            var byBenchmark = new Dictionary<string, List<string>>();
            var byContext = new Dictionary<string, List<string>>();

            foreach (var bench in benchmarks)
            {
                foreach (var ctx in contexts)
                {
                    try
                    {
                        var fullName = GenerateBenchmark(spc, bench, ctx);

                        var benchFull = bench.ContainingNamespace + "." + bench.Name;
                        var ctxFull = ctx.ContainingNamespace + "." + ctx.Name;

                        if (!byBenchmark.TryGetValue(benchFull, out var listB)) { listB = new List<string>(); byBenchmark[benchFull] = listB; }
                        listB.Add(fullName);

                        if (!byContext.TryGetValue(ctxFull, out var listC)) { listC = new List<string>(); byContext[ctxFull] = listC; }
                        listC.Add(fullName);
                    }
                    catch (Exception ex)
                    {
                        spc.AddSource($"BenchmarksGenerator/Error_{bench.Name}_{ctx.Name}.g.cs", SourceText.From($"/*\n{ex}\n*/", Encoding.UTF8));
                    }
                }
            }

            EmitBenchMap(spc, byBenchmark, byContext);
        });
    }

    // Helper to filter out duplicate types by their fully qualified name
    private static IEnumerable<INamedTypeSymbol> DistinctByFullName(IEnumerable<INamedTypeSymbol> types)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var t in types)
        {
            var name = t.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            if (string.IsNullOrEmpty(name))
                continue;
            if (seen.Add(name))
                yield return t;
        }
    }

    // ---------------- Components.g.cs ----------------
    private static void EmitComponents(SourceProductionContext spc)
    {
        var cu = SyntaxFactory.CompilationUnit()
            .AddUsings(
                SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("MorpehComponent = Scellecs.Morpeh.IComponent")),
                SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("DragonComponent = DCFApixels.DragonECS.IEcsComponent")),
                SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("XenoComponent = Xeno.IComponent")),
                SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("FrifloComponent = Friflo.Engine.ECS.IComponent")),
                SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("StaticEcsComponent = FFS.Libraries.StaticEcs.IComponent")))
            .AddMembers(
                SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("Benchmark"))
                    .AddMembers(GetComponentTypes().Concat(GetPaddingTypes()).ToArray()));

        var src = cu.NormalizeWhitespace().ToFullString();
        spc.AddSource("BenchmarksGenerator/Components.g.cs", SourceText.From(src, Encoding.UTF8));
    }

    private static IEnumerable<MemberDeclarationSyntax> GetComponentTypes()
    {
        for (int i = 1; i <= 100; i++)
        {
            yield return SyntaxFactory.ParseMemberDeclaration($"public struct Component{i} : MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent {{ public int Value; }}");
        }
    }

    private static IEnumerable<MemberDeclarationSyntax> GetPaddingTypes()
    {
        for (int i = 1; i <= 100; i++)
        {
            yield return SyntaxFactory.ParseMemberDeclaration($"public struct Padding{i} : MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent {{ public long Value1; public long Value2; public long Value3; public long Value4; }}");
        }
    }

    // ---------------- Wrapper generation ----------------

    private static ImmutableArray<INamedTypeSymbol> CollectImplementations(
        Compilation compilation,
        INamedTypeSymbol iface,
        Func<INamedTypeSymbol, bool> predicate)
    {
        var builder = ImmutableArray.CreateBuilder<INamedTypeSymbol>();
        void Walk(INamespaceSymbol ns)
        {
            foreach (var member in ns.GetMembers())
            {
                if (member is INamespaceSymbol ns2) { Walk(ns2); continue; }
                if (member is INamedTypeSymbol t)
                {
                    try
                    {
                        if (t.TypeKind == TypeKind.Class || t.TypeKind == TypeKind.Struct || t.TypeKind == TypeKind.Interface)
                        {
                            if (t.AllInterfaces.Any(i => SymbolEqualityComparer.Default.Equals(i, iface)) && predicate(t))
                                builder.Add(t);
                        }

                        foreach (var nt in t.GetTypeMembers())
                        {
                            if (nt is INamedTypeSymbol nt2)
                            {
                                if (nt2.AllInterfaces.Any(i => SymbolEqualityComparer.Default.Equals(i, iface)) && predicate(nt2))
                                    builder.Add(nt2);
                            }
                        }
                    }
                    catch { /* best effort */ }
                }
            }
        }
        Walk(compilation.Assembly.GlobalNamespace);
        foreach (var ra in compilation.References)
        {
            var asm = compilation.GetAssemblyOrModuleSymbol(ra) as IAssemblySymbol;
            if (asm != null)
                Walk(asm.GlobalNamespace);
        }
        return builder.ToImmutable();
    }

    private static string GenerateBenchmark(SourceProductionContext spc, INamedTypeSymbol benchmark, INamedTypeSymbol contextType)
    {
        var className = $"{benchmark.Name}_{contextType.Name}";
        var namespaceName = "Benchmark";
        var artifactsPathArgument = $".benchmark_results/{benchmark.Name}";

        // Usings from both types
        var mergedUsings = MergeUsings(GetOriginalUsings(benchmark), GetOriginalUsings(contextType))
            .Concat(new[] { SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(contextType.ContainingNamespace.ToDisplayString())) });

        var benchmarkClass = GetBenchmarkClassDeclaration(className, benchmark, contextType)
            .WithAttributeLists(ReplaceArtifactsPathAttribute(benchmark, artifactsPathArgument));

        var cu = SyntaxFactory.CompilationUnit()
            .AddUsings(mergedUsings.ToArray())
            .AddMembers(SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(namespaceName))
                .AddMembers(benchmarkClass))
            .NormalizeWhitespace();

        var code = cu.ToFullString();
        code = StringReplacements(code);

        spc.AddSource($"BenchmarksGenerator/{className}.g.cs", SourceText.From(code, Encoding.UTF8));
        return className;
    }

    private static SyntaxList<AttributeListSyntax> ReplaceArtifactsPathAttribute(INamedTypeSymbol benchmarkType, string artifactsPathValue)
    {
        var originalAttributes = GetTypeDeclarationSyntax(benchmarkType)?.AttributeLists ?? default;

        // Remove existing ArtifactsPathAttribute (by name)
        var updatedAttributes = originalAttributes.Where(attrList =>
            !attrList.Attributes.Any(attr => attr.Name.ToString().Contains("ArtifactsPath")));

        var newArtifactsPathAttribute = SyntaxFactory.AttributeList(
            SyntaxFactory.SingletonSeparatedList(
                SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("ArtifactsPath"))
                    .WithArgumentList(SyntaxFactory.AttributeArgumentList(
                        SyntaxFactory.SingletonSeparatedList(
                            SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(
                                SyntaxKind.StringLiteralExpression,
                                SyntaxFactory.Literal(artifactsPathValue))))))));

        return SyntaxFactory.List(updatedAttributes.Concat(new[] { newArtifactsPathAttribute }));
    }

    private static IEnumerable<UsingDirectiveSyntax> GetOriginalUsings(INamedTypeSymbol symbol)
    {
        var location = symbol.Locations.FirstOrDefault();
        var root = location?.SourceTree?.GetCompilationUnitRoot();
        return root?.Usings ?? Enumerable.Empty<UsingDirectiveSyntax>();
    }

    private static IEnumerable<UsingDirectiveSyntax> MergeUsings(IEnumerable<UsingDirectiveSyntax> a, IEnumerable<UsingDirectiveSyntax> b)
    {
        var set = new HashSet<string>(StringComparer.Ordinal);
        var list = new List<UsingDirectiveSyntax>();
        foreach (var u in a.Concat(b))
        {
            var t = u.ToString();
            if (set.Add(t)) list.Add(u);
        }
        return list.OrderBy(u => u.Name?.ToString() ?? string.Empty);
    }

    private static ClassDeclarationSyntax GetBenchmarkClassDeclaration(string className, INamedTypeSymbol benchmarkType, INamedTypeSymbol contextType)
    {
        var entityTypeName = GetEntityTypeName(contextType);
        var classAttributes = CopyClassAttributes(benchmarkType);

        var contextFields = GetFields(contextType, entityTypeName);
        var contextProperties = GetProperties(contextType);
        var contextMethods = GetContextMethods(contextType);
        var contextInnerTypes = GetInnerTypes(contextType);

        var benchmarkFields = GetFields(benchmarkType, entityTypeName);
        var benchmarkProperties = GetProperties(benchmarkType);
        var benchmarkMethods = InlineBenchmarkMethods(benchmarkType, contextType);

        return SyntaxFactory.ClassDeclaration(className)
            .AddBaseListTypes(SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName("IBenchmark")))
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
            .AddAttributeLists(classAttributes.ToArray())
            .AddMembers(
                contextFields
                    .Concat(contextProperties)
                    .Concat(benchmarkFields)
                    .Concat(benchmarkProperties)
                    .Concat(benchmarkMethods)
                    .Concat(contextMethods)
                    .Concat(contextInnerTypes)
                    .ToArray());
    }

    private static IEnumerable<AttributeListSyntax> CopyClassAttributes(INamedTypeSymbol t)
    {
        return GetTypeDeclarationSyntax(t)?.AttributeLists ?? Enumerable.Empty<AttributeListSyntax>();
    }

    private static string GetEntityTypeName(INamedTypeSymbol contextType)
    {
        var iface = contextType.Interfaces.FirstOrDefault();
        var type = iface?.TypeArguments.ElementAtOrDefault(0);
        return type?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
    }

    private static IEnumerable<MemberDeclarationSyntax> GetProperties(INamedTypeSymbol type)
    {
        var syntax = GetTypeDeclarationSyntax(type);
        return syntax?.Members
            .OfType<PropertyDeclarationSyntax>()
            .Where(p => NotIgnored(p))
            .Where(p => p.Identifier.Text != "Context")
            .ToArray() ?? Enumerable.Empty<MemberDeclarationSyntax>();
    }

    private static IEnumerable<MemberDeclarationSyntax> GetFields(INamedTypeSymbol type, string entityTypeName)
    {
        foreach (var field in type.GetMembers().OfType<IFieldSymbol>().Where(f => NotIgnored(f)).Where(f => !f.Name.Contains("k__")))
        {
            var t = field.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            t = t switch
            {
                "TE" => entityTypeName,
                "TE[]" => entityTypeName + "[]",
                _ => t
            };

            yield return SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(SyntaxFactory.ParseTypeName(t))
                        .AddVariables(
                            SyntaxFactory.VariableDeclarator(field.Name)
                                .WithInitializer(GetInitializer(field))))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
        }
    }

    private static EqualsValueClauseSyntax GetInitializer(IFieldSymbol fieldSymbol)
    {
        var decl = fieldSymbol.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as VariableDeclaratorSyntax;
        return decl?.Initializer;
    }

    private static IEnumerable<MemberDeclarationSyntax> InlineBenchmarkMethods(INamedTypeSymbol benchmarkType, INamedTypeSymbol contextType)
    {
        var syntax = GetTypeDeclarationSyntax(benchmarkType);
        return syntax?.Members
            .OfType<MethodDeclarationSyntax>()
            .Where(NotIgnored)
            .Select(m => InlineRemainingMemberAccess(InlineMethodCall(m, contextType)))
            ?? Enumerable.Empty<MemberDeclarationSyntax>();
    }

    private static IEnumerable<MemberDeclarationSyntax> GetContextMethods(INamedTypeSymbol contextType)
    {
        var benchmarkInterface = contextType.AllInterfaces.FirstOrDefault();
        return contextType.GetMembers()
            .OfType<IMethodSymbol>()
            .Where(method => !method.IsStatic && method.DeclaringSyntaxReferences.Any() && benchmarkInterface.GetMembers(method.Name).Length == 0)
            .Select(method => method.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as MethodDeclarationSyntax)
            .Where(m => m != null)
            .Cast<MemberDeclarationSyntax>();
    }

    private static IEnumerable<TypeDeclarationSyntax> GetInnerTypes(INamedTypeSymbol type)
    {
        return type.GetTypeMembers()
            .Select(symbol => symbol.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as TypeDeclarationSyntax)
            .Where(s => s != null)!;
    }

    private static bool NotIgnored(ISymbol member)
    {
        var attrs = member.GetAttributes();
        if (attrs.Length == 0) return true;
        return attrs.All(a => a.AttributeClass!.Name != "IgnoreAttribute");
    }

    private static bool NotIgnored(MemberDeclarationSyntax syntax)
    {
        if (syntax.AttributeLists.Count == 0) return true;
        var attrs = syntax.AttributeLists.SelectMany(l => l.Attributes).ToImmutableArray();
        if (attrs.Length == 0) return true;
        return attrs.All(a => a.Name.ToString() != "Ignore" && a.Name.ToString() != "IgnoreAttribute");
    }

    private static MethodDeclarationSyntax InlineMethodCall(MethodDeclarationSyntax method, INamedTypeSymbol contextType)
    {
        if (method.Body == null)
            return method;
        return method.WithBody(SyntaxFactory.Block(ModifyStatementsRecursive(contextType, method.Body.Statements)));
    }

    private static IEnumerable<StatementSyntax> ModifyStatementsRecursive(INamedTypeSymbol contextType, IEnumerable<StatementSyntax> statements)
    {
        var result = new List<StatementSyntax>();
        foreach (var st in statements)
        {
            switch (st)
            {
                // Remove direct Context assignments
                case ExpressionStatementSyntax { Expression: AssignmentExpressionSyntax assign } when assign.Left.ToString() == "Context":
                    continue;
                case BlockSyntax block:
                    result.Add(SyntaxFactory.Block(ModifyStatementsRecursive(contextType, block.Statements)));
                    continue;
                case IfStatementSyntax ifs:
                {
                    var cond = ReplaceContextPropertyAccess(ifs.Condition);
                    var stmt = SyntaxFactory.Block(ModifyStatementsRecursive(contextType, AsBlock(ifs.Statement).Statements));
                    var els = ifs.Else != null ? SyntaxFactory.ElseClause(SyntaxFactory.Block(ModifyStatementsRecursive(contextType, AsBlock(ifs.Else.Statement).Statements))) : null;
                    result.Add(ifs.WithCondition(cond).WithStatement(stmt).WithElse(els));
                    continue;
                }
                case SwitchStatementSyntax sw:
                {
                    var updatedSections = sw.Sections.Select(section =>
                    {
                        var updatedLabels = section.Labels.Select(label =>
                            label is CaseSwitchLabelSyntax caseLabel
                                ? caseLabel.WithValue(ReplaceContextPropertyAccess(caseLabel.Value))
                                : label);

                        var updatedStatements = ModifyStatementsRecursive(contextType, section.Statements);
                        return section
                            .WithLabels(SyntaxFactory.List(updatedLabels))
                            .WithStatements(SyntaxFactory.SingletonList<StatementSyntax>(SyntaxFactory.Block(updatedStatements)));
                    });
                    result.Add(sw.WithSections(SyntaxFactory.List(updatedSections)));
                    continue;
                }
                case WhileStatementSyntax wh:
                    result.Add(wh.WithCondition(ReplaceContextPropertyAccess(wh.Condition))
                        .WithStatement(SyntaxFactory.Block(ModifyStatementsRecursive(contextType, AsBlock(wh.Statement).Statements))));
                    continue;
                case ForStatementSyntax fs:
                {
                    var cond = ReplaceContextPropertyAccess(fs.Condition);
                    var init = SyntaxFactory.SeparatedList(fs.Initializers.Select(ReplaceContextPropertyAccess));
                    var inc = SyntaxFactory.SeparatedList(fs.Incrementors.Select(ReplaceContextPropertyAccess));
                    result.Add(fs.WithCondition(cond).WithInitializers(init).WithIncrementors(inc)
                        .WithStatement(SyntaxFactory.Block(ModifyStatementsRecursive(contextType, AsBlock(fs.Statement).Statements))));
                    continue;
                }
                case ForEachStatementSyntax fe:
                    result.Add(fe.WithExpression(ReplaceContextPropertyAccess(fe.Expression))
                        .WithStatement(SyntaxFactory.Block(ModifyStatementsRecursive(contextType, AsBlock(fe.Statement).Statements))));
                    continue;
                case DoStatementSyntax dos:
                    result.Add(dos.WithCondition(ReplaceContextPropertyAccess(dos.Condition))
                        .WithStatement(SyntaxFactory.Block(ModifyStatementsRecursive(contextType, AsBlock(dos.Statement).Statements))));
                    continue;
                case UnsafeStatementSyntax us:
                    result.Add(us.WithBlock(SyntaxFactory.Block(ModifyStatementsRecursive(contextType, us.Block.Statements))));
                    continue;
            }

            // Local declaration with method call: var x = Context.Foo(...)
            if (st is LocalDeclarationStatementSyntax lds &&
                lds.Declaration.Variables.FirstOrDefault()?.Initializer?.Value is InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax mea } &&
                mea.Expression.ToString() == "Context")
            {
                var methodName = mea.Name.Identifier.Text;
                var typeParameterCount = (mea.Name as GenericNameSyntax)?.TypeArgumentList.Arguments.Count ?? 0;
                var returnVar = "var " + lds.Declaration.Variables.First().Identifier.Text;
                var inlined = GetContextMethod(contextType, methodName, typeParameterCount, out var symbol);
                if (inlined != null)
                {
                    var genericSubs = GetGenericSubstitutions((InvocationExpressionSyntax)lds.Declaration.Variables.First().Initializer!.Value, symbol);
                    var subs = GetArgumentSubstitutions(inlined, (InvocationExpressionSyntax)lds.Declaration.Variables.First().Initializer!.Value);
                    var stmts = InlineMethodBodyWithArgumentsAndGenerics(inlined, subs, genericSubs, returnVar).ToArray();
                    result.AddRange(stmts);
                    continue;
                }
            }

            // Field/property assignment: X = Context.Foo(...)
            if (st is ExpressionStatementSyntax { Expression: AssignmentExpressionSyntax { Right: InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax mea2 } inv2 } assignExpr } &&
                mea2.Expression.ToString() == "Context")
            {
                var methodName = mea2.Name.Identifier.Text;
                var typeParameterCount = (mea2.Name as GenericNameSyntax)?.TypeArgumentList.Arguments.Count ?? 0;
                var returnVar = assignExpr.Left.ToString();
                var inlined = GetContextMethod(contextType, methodName, typeParameterCount, out var symbol);
                if (inlined != null)
                {
                    var genericSubs = GetGenericSubstitutions(inv2, symbol);
                    var subs = GetArgumentSubstitutions(inlined, inv2);
                    var stmts = InlineMethodBodyWithArgumentsAndGenerics(inlined, subs, genericSubs, returnVar).ToArray();
                    result.AddRange(stmts);
                    continue;
                }
            }

            // Assignment to existing local variable: x = Context.Foo(...)
            if (st is ExpressionStatementSyntax { Expression: AssignmentExpressionSyntax { Right: InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax mea3 } inv3 } assignExpr2 } &&
                mea3.Expression.ToString() == "Context")
            {
                var methodName = mea3.Name.Identifier.Text;
                var typeParameterCount = (mea3.Name as GenericNameSyntax)?.TypeArgumentList.Arguments.Count ?? 0;
                var returnVar = assignExpr2.Left.ToString();
                var inlined = GetContextMethod(contextType, methodName, typeParameterCount, out var symbol);
                if (inlined != null)
                {
                    var genericSubs = GetGenericSubstitutions(inv3, symbol);
                    var subs = GetArgumentSubstitutions(inlined, inv3);
                    var stmts = InlineMethodBodyWithArgumentsAndGenerics(inlined, subs, genericSubs, returnVar).ToArray();
                    result.AddRange(stmts);
                    continue;
                }
            }

            // Non-return method calls: Context.Foo(...);
            if (st is ExpressionStatementSyntax { Expression: InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax mea4 } inv4 } &&
                mea4.Expression.ToString() == "Context")
            {
                var methodName = mea4.Name.Identifier.Text;
                var typeParameterCount = (mea4.Name as GenericNameSyntax)?.TypeArgumentList.Arguments.Count ?? 0;
                var inlined = GetContextMethod(contextType, methodName, typeParameterCount, out var symbol);
                if (inlined != null)
                {
                    var genericSubs = GetGenericSubstitutions(inv4, symbol);
                    var subs = GetArgumentSubstitutions(inlined, inv4);
                    var stmts = InlineMethodBodyWithArgumentsAndGenerics(inlined, subs, genericSubs).ToArray();
                    result.AddRange(stmts);
                    continue;
                }
            }

            // Property/member access on Context -> drop the prefix
            if (st is ExpressionStatementSyntax { Expression: MemberAccessExpressionSyntax propAccess } &&
                propAccess.Expression.ToString() == "Context")
            {
                var updated = SyntaxFactory.IdentifierName(propAccess.Name.Identifier.Text);
                result.Add(SyntaxFactory.ExpressionStatement(updated));
                continue;
            }

            // Fallbacks: strip `Context.` where still present in arbitrary nodes
            if (st is LocalDeclarationStatementSyntax || st is ExpressionStatementSyntax)
            {
                var updated = InlineRemainingMemberAccess(st);
                result.Add(updated);
                continue;
            }

            result.Add(st);
        }
        return result;
    }

    // --- Helper methods for inlining Context methods ---
    private static Dictionary<string, string> GetArgumentSubstitutions(MethodDeclarationSyntax methodSyntax, InvocationExpressionSyntax invocation)
    {
        var parameters = methodSyntax.ParameterList.Parameters;
        var arguments = invocation.ArgumentList.Arguments;
        return parameters.Zip(arguments, (param, arg) =>
                new KeyValuePair<string, string>(param.Identifier.Text, arg.ToString()))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    private static Dictionary<string, string> GetGenericSubstitutions(InvocationExpressionSyntax invocation, IMethodSymbol methodSymbol)
    {
        var substitutions = new Dictionary<string, string>();
        var typeParameters = methodSymbol.TypeParameters;

        SyntaxNode? expr = invocation.Expression;
        GenericNameSyntax? genericName = expr switch
        {
            MemberAccessExpressionSyntax { Name: GenericNameSyntax g } => g,
            GenericNameSyntax g => g,
            _ => null
        };

        if (genericName != null)
        {
            return GetGenericName(genericName, typeParameters, substitutions);
        }

        HandleArguments(invocation, typeParameters, substitutions);
        return substitutions;
    }

    private static void HandleArguments(InvocationExpressionSyntax invocation, ImmutableArray<ITypeParameterSymbol> typeParameters, Dictionary<string, string> substitutions)
    {
        var arguments = invocation.ArgumentList.Arguments;
        if (typeParameters.Length > 0 && arguments.Count > 0)
        {
            for (int i = 0; i < typeParameters.Length; i++)
            {
                var tp = typeParameters[i];
                var argExpr = arguments.ElementAtOrDefault(i)?.Expression;
                if (argExpr is LiteralExpressionSyntax lit && lit.Kind() == SyntaxKind.DefaultLiteralExpression)
                {
                    substitutions[tp.Name] = "default";
                }
                else if (argExpr is DefaultExpressionSyntax def)
                {
                    substitutions[tp.Name] = def.Type.ToString();
                }
                else
                {
                    substitutions[tp.Name] = argExpr?.ToString() ?? "unknown";
                }
            }
        }
    }

    private static Dictionary<string, string> GetGenericName(GenericNameSyntax genericName, ImmutableArray<ITypeParameterSymbol> typeParameters, Dictionary<string, string> substitutions)
    {
        var typeArguments = genericName.TypeArgumentList.Arguments.ToArray();
        if (typeParameters.Length != typeArguments.Length)
            throw new InvalidOperationException($"Mismatch in the number of generic arguments. Expected {typeParameters.Length}, but got {typeArguments.Length}.");

        for (int i = 0; i < typeParameters.Length; i++)
            substitutions[typeParameters[i].Name] = typeArguments[i].ToString();

        return substitutions;
    }

    private static IEnumerable<StatementSyntax> InlineMethodBodyWithArgumentsAndGenerics(MethodDeclarationSyntax methodSyntax, Dictionary<string, string> substitutions, Dictionary<string, string> genericSubstitutions, string? returnVariable = null)
    {
        if (methodSyntax.Body == null && methodSyntax.ExpressionBody == null)
            return Enumerable.Empty<StatementSyntax>();

        var list = new List<StatementSyntax>();
        if (methodSyntax.Body != null)
            HandleMethodBody(methodSyntax, substitutions, genericSubstitutions, returnVariable, list);
        if (methodSyntax.ExpressionBody != null)
            HandleExpressionBody(methodSyntax, substitutions, genericSubstitutions, returnVariable, list);
        return list;
    }

    private static void HandleExpressionBody(MethodDeclarationSyntax methodSyntax, Dictionary<string, string> substitutions, Dictionary<string, string> genericSubstitutions, string? returnVariable, List<StatementSyntax> output)
    {
        var expr = methodSyntax.ExpressionBody!.Expression.ReplaceNodes(
            methodSyntax.ExpressionBody.Expression.DescendantNodes().OfType<IdentifierNameSyntax>(),
            (orig, _) => substitutions.TryGetValue(orig.Identifier.Text, out var repl)
                ? SyntaxFactory.IdentifierName(repl)
                : genericSubstitutions.TryGetValue(orig.Identifier.Text, out var gen)
                    ? SyntaxFactory.IdentifierName(gen)
                    : orig);

        if (returnVariable != null)
        {
            output.Add(SyntaxFactory.ExpressionStatement(
                SyntaxFactory.AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    SyntaxFactory.IdentifierName(returnVariable),
                    expr)));
        }
        else
        {
            output.Add(SyntaxFactory.ExpressionStatement(expr));
        }
    }

    private static void HandleMethodBody(MethodDeclarationSyntax methodSyntax, Dictionary<string, string> substitutions, Dictionary<string, string> genericSubstitutions, string? returnVariable, List<StatementSyntax> output)
    {
        foreach (var s in methodSyntax.Body!.Statements)
        {
            var updated = s.ReplaceNodes(
                s.DescendantNodes().OfType<IdentifierNameSyntax>(),
                (orig, _) => substitutions.TryGetValue(orig.Identifier.Text, out var repl)
                    ? SyntaxFactory.IdentifierName(repl)
                    : genericSubstitutions.TryGetValue(orig.Identifier.Text, out var gen)
                        ? SyntaxFactory.IdentifierName(gen)
                        : orig);

            if (updated is ReturnStatementSyntax ret && returnVariable != null)
            {
                updated = SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.IdentifierName(returnVariable),
                        ret.Expression!));
            }

            output.Add(updated);
        }
    }

    private static MethodDeclarationSyntax? GetContextMethod(INamedTypeSymbol contextType, string methodName, int typeParameterCount, out IMethodSymbol? methodSymbol)
    {
        methodSymbol = contextType.GetMembers()
            .OfType<IMethodSymbol>()
            .FirstOrDefault(m => m.Name == methodName && m.TypeParameters.Length == typeParameterCount);

        if (methodSymbol == null)
            return null;

        if (methodSymbol.Locations.All(loc => loc.IsInMetadata))
            return null;

        var reference = methodSymbol.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as MethodDeclarationSyntax;
        return reference;
    }

    private static T InlineRemainingMemberAccess<T>(T node) where T : SyntaxNode
    {
        // replace Context.* → *
        return node.ReplaceNodes(node.DescendantNodes().OfType<MemberAccessExpressionSyntax>(), (orig, _) =>
            orig.Expression is IdentifierNameSyntax { Identifier.Text: "Context" }
                ? (ExpressionSyntax)SyntaxFactory.IdentifierName(orig.Name.Identifier.Text)
                : orig);
    }

    private static ExpressionSyntax ReplaceContextPropertyAccess(ExpressionSyntax expr)
    {
        return expr.ReplaceNodes(expr.DescendantNodesAndSelf().OfType<MemberAccessExpressionSyntax>(), (orig, _) =>
            orig.Expression is IdentifierNameSyntax { Identifier.Text: "Context" }
                ? (ExpressionSyntax)SyntaxFactory.IdentifierName(orig.Name.Identifier.Text)
                : orig);
    }

    private static BlockSyntax AsBlock(StatementSyntax s) => s as BlockSyntax ?? SyntaxFactory.Block(s);

    private static TypeDeclarationSyntax? GetTypeDeclarationSyntax(INamedTypeSymbol symbol)
        => symbol.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as TypeDeclarationSyntax;

    private static void EmitBenchMap(SourceProductionContext spc, Dictionary<string, List<string>> byBenchmark, Dictionary<string, List<string>> byContext)
    {
        var typeSyntax = SyntaxFactory.ClassDeclaration("BenchMap")
            .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.StaticKeyword)))
            .AddMembers(GenerateBenchmarkMapping(byBenchmark, byContext));

        var source = SyntaxFactory.CompilationUnit()
            .AddUsings(
                SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")),
                SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Collections.Generic")))
            .AddMembers(SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("Benchmark"))
                .AddMembers(typeSyntax))
            .NormalizeWhitespace()
            .ToFullString();

        spc.AddSource("BenchmarksGenerator/BenchMap.g.cs", SourceText.From(source, Encoding.UTF8));
    }

    private static MemberDeclarationSyntax[] GenerateBenchmarkMapping(Dictionary<string, List<string>> byBenchmark, Dictionary<string, List<string>> byContext)
    {
        var members = new MemberDeclarationSyntax[2];
        var sb = new StringBuilder();

        sb.AppendLine("public static Dictionary<Type, Type[]> Runs = new (){");
        foreach (var (bench, impls) in byBenchmark.OrderBy(kv => kv.Key))
            sb.AppendLine($"{{typeof({bench}<,>), [{string.Join(", ", impls.Select(i => $"typeof({i})"))}]}},");
        sb.AppendLine("};");
        var member = SyntaxFactory.ParseMemberDeclaration(sb.ToString());
        if (member != null) members[0] = member.NormalizeWhitespace();

        sb.Clear();
        sb.AppendLine("public static Dictionary<Type, Type[]> Contexts = new (){");
        foreach (var (ctx, impls) in byContext.OrderBy(kv => kv.Key))
            sb.AppendLine($"{{typeof({ctx}), [{string.Join(", ", impls.Select(i => $"typeof({i})"))}]}},");
        sb.AppendLine("};");
        member = SyntaxFactory.ParseMemberDeclaration(sb.ToString());
        if (member != null) members[1] = member.NormalizeWhitespace();

        return members;
    }

    private static string StringReplacements(string code)
    {
        // Keep the historical hack from v2 (safe no-op if not present)
        return code.Replace("&Update(", "Update(");
    }
}
