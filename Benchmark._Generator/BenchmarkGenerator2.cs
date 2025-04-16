using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Benchmark._Generator;

[Generator]
public class ComponentsGenerator : ISourceGenerator {
    public void Initialize(GeneratorInitializationContext ctx) {
        // no op
    }
    public void Execute(GeneratorExecutionContext ctx) {
        var source = CompilationUnit()
            .AddUsings(
                UsingDirective(ParseName("MorpehComponent = Scellecs.Morpeh.IComponent")),
                UsingDirective(ParseName("DragonComponent = DCFApixels.DragonECS.IEcsComponent")),
                UsingDirective(ParseName("XenoComponent = Xeno.IComponent")),
                UsingDirective(ParseName("FrifloComponent = Friflo.Engine.ECS.IComponent")),
                UsingDirective(ParseName("StaticEcsComponent = FFS.Libraries.StaticEcs.IComponent")))
            .AddMembers(NamespaceDeclaration(ParseName("Benchmark"))
                .AddMembers(GetComponentTypes().ToArray())
                .AddMembers(GetPaddingTypes().ToArray()))
            .NormalizeWhitespace()
            .ToFullString();

        ctx.AddSource("BenchmarksGenerator/Components.g.cs", SourceText.From($"{source}", Encoding.UTF8));
    }

    private IEnumerable<MemberDeclarationSyntax> GetComponentTypes() {
        for (int i = 1; i <= 100; i++) {
            yield return ParseMemberDeclaration($"public struct Component{i} : MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent {{ public int Value; }}");
        }
    }

    private IEnumerable<MemberDeclarationSyntax> GetPaddingTypes() {
        for (int i = 1; i <= 100; i++) {
            yield return ParseMemberDeclaration($"public struct Padding{i} : MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent {{ public long Value1; public long Value2; public long Value3; public long Value4; }}");
        }
    }
}

[Generator]
public class BenchmarksGenerator : ISourceGenerator {
    private static readonly StringBuilder log = new();
    public void Initialize(GeneratorInitializationContext context) {
        // No initialization required
    }

    public void Execute(GeneratorExecutionContext context) {
        log.Clear();
        foreach (var reference in context.Compilation.References) {
            log.AppendLine($"Reference: {reference.Display}");
        }

        try {

            // Get the existing compilation
            var compilation = context.Compilation;

            // Combine the existing references with additional ones
            var allReferences = compilation.References;

            // Create a new compilation with all references
            compilation = CSharpCompilation.Create(
                compilation.AssemblyName,
                compilation.SyntaxTrees,
                allReferences,
                (CSharpCompilationOptions)compilation.Options
            );


            var parseOptions = (CSharpParseOptions)context.Compilation.SyntaxTrees.First().Options;
            var additionalSyntaxTrees = new List<SyntaxTree>();
            foreach (var additionalFile in context.AdditionalFiles)
            {
                var text = additionalFile.GetText(context.CancellationToken);
                if (text == null) continue;

                var syntaxTree = CSharpSyntaxTree.ParseText(text.ToString(), parseOptions);
                additionalSyntaxTrees.Add(syntaxTree);
            }
            compilation = compilation.AddSyntaxTrees(additionalSyntaxTrees);

            log.AppendLine($"Compilation ParseOptions: {context.Compilation.SyntaxTrees.First().Options}");
            log.AppendLine($"AdditionalFile ParseOptions: {parseOptions}");

            GenerateBenchmarks(compilation, context);
        } catch (Exception ex) {
            context.AddSource("BenchmarksGenerator/_ErrorLog.g.cs", SourceText.From($"/* {ex} */", Encoding.UTF8));
        } finally {
            context.AddSource("BenchmarksGenerator/_Log.g.cs", SourceText.From($"/* {log} */", Encoding.UTF8));
        }
    }

    private static void GenerateBenchmarks(Compilation compilation, GeneratorExecutionContext context) {
        // Get the benchmark and context interfaces
        var contextInterface = compilation.GetTypeByMetadataName("Benchmark.Context.IBenchmarkContext");
        var benchmarkInterface = compilation.GetTypeByMetadataName("Benchmark.IBenchmark");

        if (contextInterface == null || benchmarkInterface == null)
            return;

        // Collect all contexts and benchmarks
        var contexts = compilation.CollectImplementations(contextInterface,
            t => !t.IsAbstract && !t.IsGenericType && t.TypeKind == TypeKind.Class);
        var benchmarks = compilation.CollectImplementations(benchmarkInterface,
            t => t.IsAbstract && t.IsGenericType && t.TypeKind != TypeKind.Interface);

        var byBenchmark = new Dictionary<string, List<string>>();
        var byContext = new Dictionary<string, List<string>>();

        foreach (var benchmarkType in benchmarks) {
            foreach (var contextType in contexts) {
                try {
                    var fullName = GenerateBenchmark(benchmarkType, contextType, context);

                    var benchmarkFullName = $"{benchmarkType.ContainingNamespace}.{benchmarkType.Name}";
                    var contextFullName = $"{contextType.ContainingNamespace}.{contextType.Name}";

                    byBenchmark.TryAdd(benchmarkFullName, []);
                    byBenchmark[benchmarkFullName].Add(fullName);

                    byContext.TryAdd(contextFullName, []);
                    byContext[contextFullName].Add(fullName);
                } catch (Exception ex) {
                    context.AddSource($"BenchmarksGenerator/Error_{benchmarkType.Name}_{contextType.Name}.g.cs",
                        SourceText.From($"/*\n{ex}\n*/", Encoding.UTF8));
                }
            }
        }

        {
            var typeSyntax = ClassDeclaration("BenchMap")
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)))
                .AddMembers(GenerateBenchmarkMapping(byBenchmark, byContext));

            var source = CompilationUnit()
                .AddUsings(
                    UsingDirective(ParseName("System")),
                    UsingDirective(ParseName("System.Collections.Generic")))
                .AddMembers(NamespaceDeclaration(ParseName("Benchmark"))
                    .AddMembers(typeSyntax))
                .NormalizeWhitespace()
                .ToFullString();

            context.AddSource("BenchmarksGenerator/BenchMap.g.cs", SourceText.From(source, Encoding.UTF8));
        }
    }

    private static MemberDeclarationSyntax[] GenerateBenchmarkMapping(Dictionary<string, List<string>> byBenchmark, Dictionary<string, List<string>> byContext) {
        var sb = new StringBuilder();
        var members = new MemberDeclarationSyntax[2];
        sb.AppendLine("public static Dictionary<Type, Type[]> Runs = new (){");
        foreach (var (bench, impls) in byBenchmark.OrderBy(kv => kv.Key))
            sb.AppendLine($"{{typeof({bench}<,>), [{string.Join(", ", impls.Select(i => $"typeof({i})"))}]}},");
        sb.AppendLine("};");
        var member = ParseMemberDeclaration(sb.ToString());
        if (member != null)
            members[0] = member.NormalizeWhitespace();

        sb.Clear();
        sb.AppendLine("public static Dictionary<Type, Type[]> Contexts = new (){");
        foreach (var (ctx, impls) in byContext.OrderBy(kv => kv.Key).ToArray())
            sb.AppendLine($"{{typeof({ctx}), [{string.Join(", ", impls.Select(i => $"typeof({i})"))}]}},");
        sb.AppendLine("};");

        member = ParseMemberDeclaration(sb.ToString());
        if (member != null)
            members[1] = member.NormalizeWhitespace();

        return members;
    }

    private static string GenerateBenchmark(INamedTypeSymbol benchmark, INamedTypeSymbol contextType, GeneratorExecutionContext context) {
        var className = $"{benchmark.Name}_{contextType.Name}";
        log.AppendLine($"Generating: {className}...");

        var namespaceName = "Benchmark";
        var artifactsPathArgument = $".benchmark_results/{benchmark.Name}";

        // Get original usings
        var originalUsings = GetOriginalUsings(benchmark);
        var contextUsings = GetOriginalUsings(contextType);
        var mergedUsings = MergeUsings(originalUsings, contextUsings)
            .Concat([UsingDirective(ParseName(contextType.ContainingNamespace.ToDisplayString()))]);

        // Generate the benchmark class
        var benchmarkClass = GetBenchmarkClassDeclaration(className, benchmark, contextType)
            .WithAttributeLists(ReplaceArtifactsPathAttribute(benchmark, artifactsPathArgument));

        var compilationUnit = CompilationUnit()
            .AddUsings(mergedUsings.ToArray())
            .AddMembers(NamespaceDeclaration(ParseName(namespaceName))
                .AddMembers(benchmarkClass))
            .NormalizeWhitespace();

        var code = compilationUnit.ToFullString();
        code = StringReplacements(code);

        // Add the source to the generator context
        context.AddSource($"BenchmarksGenerator/{className}.g.cs", SourceText.From(code, Encoding.UTF8));
        log.AppendLine($"Generating: {className} done\n");
        return className;
    }

    private static SyntaxList<AttributeListSyntax> ReplaceArtifactsPathAttribute(
        INamedTypeSymbol benchmarkType,
        string artifactsPathValue
    ) {
        var originalAttributes = benchmarkType.GetTypeDeclarationSyntax()?.AttributeLists ?? default;

        // Remove existing ArtifactsPathAttribute
        var updatedAttributes = originalAttributes.Where(attrList =>
            !attrList.Attributes.Any(attr =>
                attr.Name.ToString().Contains("ArtifactsPath")));

        // Add the updated ArtifactsPath attribute
        var newArtifactsPathAttribute = AttributeList(
            SingletonSeparatedList(
                Attribute(IdentifierName("ArtifactsPath"))
                    .WithArgumentList(AttributeArgumentList(
                        SingletonSeparatedList(
                            AttributeArgument(LiteralExpression(
                                SyntaxKind.StringLiteralExpression,
                                Literal(artifactsPathValue))))))));

        return List(updatedAttributes.Concat([newArtifactsPathAttribute]));
    }

    private static IEnumerable<UsingDirectiveSyntax> GetOriginalUsings(INamedTypeSymbol symbol) {
        var location = symbol.Locations.FirstOrDefault();
        if (location == null)
            return [];

        var root = location.SourceTree?.GetCompilationUnitRoot();
        return root?.Usings ?? Enumerable.Empty<UsingDirectiveSyntax>();
    }

    private static IEnumerable<UsingDirectiveSyntax> MergeUsings(IEnumerable<UsingDirectiveSyntax> benchmarkUsings, IEnumerable<UsingDirectiveSyntax> contextUsings) {
        var uniqueUsings = new HashSet<string>();
        var mergedUsings = new List<UsingDirectiveSyntax>();

        foreach (var u in benchmarkUsings.Concat(contextUsings)) {
            var usingText = u.ToString();
            if (uniqueUsings.Add(usingText)) // Add only if not already present
                mergedUsings.Add(u);
        }

        return mergedUsings.OrderBy(u => u.Name?.ToString() ?? "null"); // Optional: sort for readability
    }

    private static ClassDeclarationSyntax GetBenchmarkClassDeclaration(string className, INamedTypeSymbol benchmarkType, INamedTypeSymbol contextType) {
        // Extract fields, properties, and methods
        var classAttributes = CopyClassAttributes(benchmarkType);
        var entityTypeName = GetEntityTypeName(contextType);

        var contextFields = GetFields(contextType, entityTypeName);
        var contextProperties = GetProperties(contextType);
        var contextMethods = GetContextMethods(contextType);
        var contextInnerTypes = GetInnerTypes(contextType);

        var benchmarkFields = GetFields(benchmarkType, entityTypeName);
        var benchmarkProperties = GetProperties(benchmarkType);
        var benchmarkMethods = InlineBenchmarkMethods(benchmarkType, contextType);

        // Generate the benchmark class
        return ClassDeclaration(className)
            .AddBaseListTypes(SimpleBaseType(ParseTypeName("IBenchmark")))
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
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

    private static bool NotIgnored(ISymbol member) {
        var attrs = member.GetAttributes();
        if (attrs.Length == 0) return true;
        return attrs.All(a => a.AttributeClass!.Name != "IgnoreAttribute");
    }

    private static bool NotIgnored(MemberDeclarationSyntax syntax) {
        if (syntax.AttributeLists.Count == 0)
            return true;

        var attrs = syntax.AttributeLists.SelectMany(list => list.Attributes).ToImmutableArray();
        if (attrs.Length == 0) return true;
        return attrs.All(a => a.Name.ToString() != "Ignore" && a.Name.ToString() != "IgnoreAttribute");
    }

    private static string GetEntityTypeName(INamedTypeSymbol contextType) {
        var iface = contextType.Interfaces.FirstOrDefault();
        var type = iface?.TypeArguments.ElementAtOrDefault(0);
        if (type == null)
            return null;

        return type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
    }

    private static IEnumerable<AttributeListSyntax> CopyClassAttributes(INamedTypeSymbol benchmarkType) {
        var benchmarkSyntax = benchmarkType.GetTypeDeclarationSyntax();
        return benchmarkSyntax?.AttributeLists ?? Enumerable.Empty<AttributeListSyntax>();
    }

    private static IEnumerable<MemberDeclarationSyntax> GetProperties(INamedTypeSymbol type) {
        var benchmarkSyntax = type.GetTypeDeclarationSyntax();
        return benchmarkSyntax?.Members
            .OfType<PropertyDeclarationSyntax>()
            .Where(NotIgnored)
            .Where(p => p.Identifier.Text != "Context")
            .ToArray() ?? Enumerable.Empty<MemberDeclarationSyntax>();
    }

    private static IEnumerable<MemberDeclarationSyntax> GetFields(INamedTypeSymbol contextType, string entityTypeName) {
        foreach (var field in contextType.GetMembers().OfType<IFieldSymbol>()
                     .Where(f => NotIgnored(f))
                     .Where(f => !f.Name.Contains("k__"))) {

            var type = field.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            type = type switch {
                "TE" => entityTypeName,
                "TE[]" => $"{entityTypeName}[]",
                _ => type
            };

            yield return FieldDeclaration(
                    VariableDeclaration(ParseTypeName(type))
                        .AddVariables(
                            VariableDeclarator(field.Name)
                                .WithInitializer(GetInitializer(field))
                            ))
                .AddModifiers(Token(SyntaxKind.PrivateKeyword));
        }
    }


    private static EqualsValueClauseSyntax GetInitializer(IFieldSymbol fieldSymbol)
    {
        var fieldDeclaration = fieldSymbol.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as VariableDeclaratorSyntax;
        return fieldDeclaration?.Initializer;
    }

    private static IEnumerable<MemberDeclarationSyntax> InlineBenchmarkMethods(INamedTypeSymbol benchmarkType, INamedTypeSymbol contextType) {
        return benchmarkType.GetTypeDeclarationSyntax()?.Members
                .OfType<MethodDeclarationSyntax>()
                .Select(method => InlineRemainingMemberAccess(InlineMethodCall(method, contextType)))
            ?? [];
    }

    private static IEnumerable<MemberDeclarationSyntax> GetContextMethods(INamedTypeSymbol contextType) {
        var benchmarkInterface = contextType.AllInterfaces.FirstOrDefault();

        return contextType.GetMembers()
            .OfType<IMethodSymbol>()
            .Where(method => !method.IsStatic && method.DeclaringSyntaxReferences.Any() && benchmarkInterface.GetMembers(method.Name).Length == 0)
            .Select(method => method.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as MethodDeclarationSyntax)
            .Where(method => method != null);
    }

    private static IEnumerable<TypeDeclarationSyntax> GetInnerTypes(INamedTypeSymbol type) {
        return type.GetTypeMembers()
            .OfType<INamedTypeSymbol>()
            .Select(symbol => symbol.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as TypeDeclarationSyntax)
            .Where(syntax => syntax != null);
    }

    private static T InlineRemainingMemberAccess<T>(T node)
        where T : SyntaxNode
    {
        // replace Context.* calls to * calls
        node = node.ReplaceNodes(node.DescendantNodes().OfType<MemberAccessExpressionSyntax>(),
            (original, _)
                => original.Expression is IdentifierNameSyntax { Identifier.Text: "Context" }
                    ? IdentifierName(original.Name.Identifier.Text)
                    : original);

        return node;
    }

    private static MethodDeclarationSyntax InlineMethodCall(MethodDeclarationSyntax method, INamedTypeSymbol contextType) {
        if (method.Body == null)
            return method;

        // log.AppendLine($"mod \"{method.WithBody(null).WithAttributeLists(SyntaxFactory.List<AttributeListSyntax>())}\"");
        return method.WithBody(Block(ModifyStatementsRecursive(contextType, method.Body.Statements)));
    }

    // ReSharper disable once CognitiveComplexity
    private static List<StatementSyntax> ModifyStatementsRecursive(INamedTypeSymbol contextType, IEnumerable<StatementSyntax> statements) {
        var newStatements = new List<StatementSyntax>();
        foreach (var statement in statements) {
            switch (statement) {
                // Remove Context assignments
                case ExpressionStatementSyntax { Expression: AssignmentExpressionSyntax assignment } when
                    assignment.Left.ToString() == "Context":
                    continue; // Skip the Context assignment
                // Recursively process inner blocks
                case BlockSyntax block:
                    newStatements.Add(Block(ModifyStatementsRecursive(contextType, block.Statements)));
                    continue;
                // Handle if statements with optional else clause
                case IfStatementSyntax ifStatement: {
                    // Replace property access in condition
                    var updatedCondition = ReplaceContextPropertyAccess(ifStatement.Condition);
                    var updatedIf =
                        ifStatement
                            .WithCondition(updatedCondition)
                            .WithStatement(Block(ModifyStatementsRecursive(contextType, ifStatement.Statement.AsBlock().Statements)))
                            .WithElse(ifStatement.Else != null
                                ? ElseClause(Block(ModifyStatementsRecursive(contextType, ifStatement.Else.Statement.AsBlock().Statements)))
                                : null);

                    newStatements.Add(updatedIf);
                    continue;
                }
                // Handle switch statements
                case SwitchStatementSyntax switchStatement: {
                    var updatedSections = switchStatement.Sections.Select(section => {
                        var updatedLabels = section.Labels.Select(label
                            => label is CaseSwitchLabelSyntax caseLabel
                                ? caseLabel.WithValue(ReplaceContextPropertyAccess(caseLabel.Value))
                                : label);

                        var updatedStatements = ModifyStatementsRecursive(contextType, section.Statements);
                        return section
                            .WithLabels(List(updatedLabels))
                            .WithStatements(SingletonList<StatementSyntax>(Block(updatedStatements)));
                    });

                    newStatements.Add(switchStatement.WithSections(List(updatedSections)));
                    continue;
                }
                // Handle loops (while, for, foreach, do-while)
                case WhileStatementSyntax whileStatement: {
                    // Replace property access in condition
                    var updatedCondition = ReplaceContextPropertyAccess(whileStatement.Condition);
                    var updatedWhile = whileStatement
                        .WithCondition(updatedCondition)
                        .WithStatement(Block(ModifyStatementsRecursive(contextType, whileStatement.Statement.AsBlock().Statements)));
                    newStatements.Add(updatedWhile);
                    continue;
                }
                case ForStatementSyntax forStatement: {
                    // Replace property access in condition, initializers, and incrementors
                    var updatedCondition = ReplaceContextPropertyAccess(forStatement.Condition);
                    var updatedInitializers = forStatement.Initializers
                        .Select(ReplaceContextPropertyAccess)
                        .ToList();
                    var updatedIncrementors = forStatement.Incrementors
                        .Select(ReplaceContextPropertyAccess)
                        .ToList();

                    // Recursively process the inner statements
                    var updatedFor = forStatement
                        .WithCondition(updatedCondition)
                        .WithInitializers(SeparatedList(updatedInitializers))
                        .WithIncrementors(SeparatedList(updatedIncrementors))
                        .WithStatement(Block(ModifyStatementsRecursive(contextType, forStatement.Statement.AsBlock().Statements)));

                    newStatements.Add(updatedFor);
                    continue;
                }
                case ForEachStatementSyntax foreachStatement: {
                    // Replace property access in the expression
                    var updatedExpression = ReplaceContextPropertyAccess(foreachStatement.Expression);

                    // Recursively process the inner statements
                    var updatedForEach = foreachStatement
                        .WithExpression(updatedExpression)
                        .WithStatement(Block(ModifyStatementsRecursive(contextType, foreachStatement.Statement.AsBlock().Statements)));

                    newStatements.Add(updatedForEach);
                    continue;
                }
                case DoStatementSyntax doStatement: {
                    // Replace property access in condition
                    var updatedCondition = ReplaceContextPropertyAccess(doStatement.Condition);

                    var updatedDo = doStatement
                        .WithCondition(updatedCondition)
                        .WithStatement(Block(ModifyStatementsRecursive(contextType, doStatement.Statement.AsBlock().Statements)));
                    newStatements.Add(updatedDo);
                    continue;
                }
                case UnsafeStatementSyntax unsafeStatement: {
                    var updatedUnsafe = unsafeStatement.WithBlock(
                        Block(ModifyStatementsRecursive(contextType, unsafeStatement.Block.Statements)));
                    newStatements.Add(updatedUnsafe);
                    continue;
                }
                // Handle local variable declarations with method calls
                case LocalDeclarationStatementSyntax localDeclaration when
                    localDeclaration.Declaration.Variables.FirstOrDefault()?.Initializer?.Value is InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax memberAccess } invocation &&
                    memberAccess.Expression.ToString() == "Context": {

                    var methodName = memberAccess.Name.Identifier.Text;
                    var typeParameterCount = (memberAccess.Name as GenericNameSyntax)?.TypeArgumentList.Arguments.Count ?? 0;
                    var returnVariable = "var " + localDeclaration.Declaration.Variables.First().Identifier.Text;
                    var inlinedMethod = GetContextMethod(contextType, methodName, typeParameterCount, out var symbol);

                    if (inlinedMethod != null) {
                        var genericSubstitutions = GetGenericSubstitutions(invocation, symbol);
                        var substitutions = GetArgumentSubstitutions(inlinedMethod, invocation);
                        var inlinedStatements = InlineMethodBodyWithArgumentsAndGenerics(inlinedMethod, substitutions, genericSubstitutions, returnVariable).ToArray();
                        newStatements.AddRange(inlinedStatements);
                        continue;
                    }

                    break;
                }
                // Handle field/property assignments with method calls
                case ExpressionStatementSyntax { Expression: AssignmentExpressionSyntax { Right: InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax fieldMemberAccess } fieldInvocation } fieldAssignmentExpr } when
                    fieldMemberAccess.Expression.ToString() == "Context": {

                    var methodName = fieldMemberAccess.Name.Identifier.Text;
                    var typeParameterCount = (fieldMemberAccess.Name as GenericNameSyntax)?.TypeArgumentList.Arguments.Count ?? 0;
                    var returnVariable = fieldAssignmentExpr.Left.ToString();
                    var inlinedMethod = GetContextMethod(contextType, methodName, typeParameterCount, out var symbol);

                    if (inlinedMethod != null) {
                        var genericSubstitutions = GetGenericSubstitutions(fieldInvocation, symbol);
                        var substitutions = GetArgumentSubstitutions(inlinedMethod, fieldInvocation);
                        var inlinedStatements = InlineMethodBodyWithArgumentsAndGenerics(inlinedMethod, substitutions, genericSubstitutions, returnVariable).ToArray();
                        newStatements.AddRange(inlinedStatements);
                        continue;
                    }
                    break;
                }
            }

            // Handle assignments to existing local variables
            if (statement is ExpressionStatementSyntax { Expression: AssignmentExpressionSyntax { Right: InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax localVarMemberAccess } localVarInvocation } localVarAssignmentExpr } &&
                localVarMemberAccess.Expression.ToString() == "Context") {
                // log.Append(" exist local var member access ");
                var methodName = localVarMemberAccess.Name.Identifier.Text;
                var typeParameterCount = (localVarMemberAccess.Name as GenericNameSyntax)?.TypeArgumentList.Arguments.Count ?? 0;
                var returnVariable = localVarAssignmentExpr.Left.ToString();
                var inlinedMethod = GetContextMethod(contextType, methodName, typeParameterCount, out var symbol);

                if (inlinedMethod != null) {
                    var genericSubstitutions = GetGenericSubstitutions(localVarInvocation, symbol);
                    var substitutions = GetArgumentSubstitutions(inlinedMethod, localVarInvocation);
                    var inlinedStatements = InlineMethodBodyWithArgumentsAndGenerics(inlinedMethod, substitutions, genericSubstitutions, returnVariable).ToArray();
                    newStatements.AddRange(inlinedStatements);
                    continue;
                }
            }

            // Handle non-return method calls with MemberAccessExpressionSyntax
            if (statement is ExpressionStatementSyntax { Expression: InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax invocationMemberAccess } methodInvocation } &&
                invocationMemberAccess.Expression.ToString() == "Context") {

                var methodName = invocationMemberAccess.Name.Identifier.Text;
                var typeParameterCount = (invocationMemberAccess.Name as GenericNameSyntax)?.TypeArgumentList.Arguments.Count ?? 0;
                var inlinedMethod = GetContextMethod(contextType, methodName, typeParameterCount, out var symbol);

                if (inlinedMethod != null) {
                    var genericSubstitutions = GetGenericSubstitutions(methodInvocation, symbol);
                    var substitutions = GetArgumentSubstitutions(inlinedMethod, methodInvocation);
                    var inlinedStatements = InlineMethodBodyWithArgumentsAndGenerics(inlinedMethod, substitutions, genericSubstitutions).ToArray();
                    newStatements.AddRange(inlinedStatements);
                    continue;
                }
            }

            // Handle non-return generic method calls (e.g., Context.AddComponent<>())
            if (statement is ExpressionStatementSyntax { Expression: InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax genericMemberAccess } genericMethodInvocation } &&
                genericMemberAccess.Expression.ToString() == "Context") {

                var methodName = genericMemberAccess.Name.Identifier.Text;
                var typeParameterCount = (genericMemberAccess.Name as GenericNameSyntax)?.TypeArgumentList.Arguments.Count ?? 0;
                var inlinedMethod = GetContextMethod(contextType, methodName, typeParameterCount, out var symbol);

                if (inlinedMethod != null) {
                    var genericSubstitutions = GetGenericSubstitutions(genericMethodInvocation, symbol);
                    var substitutions = GetArgumentSubstitutions(inlinedMethod, genericMethodInvocation);
                    var inlinedStatements = InlineMethodBodyWithArgumentsAndGenerics(inlinedMethod, substitutions, genericSubstitutions).ToArray();
                    newStatements.AddRange(inlinedStatements);
                    continue;
                }
            }

            // Handle member access to properties
            if (statement is ExpressionStatementSyntax { Expression: MemberAccessExpressionSyntax propertyAccess } &&
                propertyAccess.Expression.ToString() == "Context") {
                var updatedExpression = IdentifierName(propertyAccess.Name.Identifier.Text);
                newStatements.Add(ExpressionStatement(updatedExpression));
                continue;
            }

            // Retain all other statements
            newStatements.Add(statement);
        }

        return newStatements;
    }

    private static Dictionary<string, string> GetArgumentSubstitutions(MethodDeclarationSyntax methodSyntax, InvocationExpressionSyntax invocation) {
        var parameters = methodSyntax.ParameterList.Parameters;
        var arguments = invocation.ArgumentList.Arguments;

        return parameters.Zip(arguments, (param, arg) =>
                new KeyValuePair<string, string>(param.Identifier.Text, arg.ToString()))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    private static Dictionary<string, string> GetGenericSubstitutions(InvocationExpressionSyntax invocation, IMethodSymbol methodSymbol) {
        var substitutions = new Dictionary<string, string>();

        // Get the generic type parameters from the method symbol
        var typeParameters = methodSymbol.TypeParameters;

        // Check if generic arguments are explicitly provided
        var genericName = invocation.Expression switch {
            MemberAccessExpressionSyntax { Name: GenericNameSyntax memberGenericName } => memberGenericName,
            GenericNameSyntax invocationGenericName => invocationGenericName,
            _ => null
        };

        // If explicit type arguments are provided
        if (genericName != null) {
            return GetGenericName(genericName, typeParameters, substitutions);
        }

        // Attempt to infer generic arguments from the invocation arguments
        HandleArguments(invocation, typeParameters, substitutions);

        return substitutions;
    }
    private static void HandleArguments(InvocationExpressionSyntax invocation, ImmutableArray<ITypeParameterSymbol> typeParameters, Dictionary<string, string> substitutions) {
        var arguments = invocation.ArgumentList.Arguments;

        if (typeParameters.Length > 0 && arguments.Count > 0) {
            for (int i = 0; i < typeParameters.Length; i++) {
                var typeParameter = typeParameters[i];
                var argumentExpression = arguments.ElementAtOrDefault(i)?.Expression;

                if (argumentExpression is LiteralExpressionSyntax literalExpression &&
                    literalExpression.Kind() == SyntaxKind.DefaultLiteralExpression) {
                    // Handle `default` without a specified type
                    substitutions[typeParameter.Name] = "default";
                } else if (argumentExpression is DefaultExpressionSyntax defaultExpression) {
                    // Handle `default(Type)`
                    substitutions[typeParameter.Name] = defaultExpression.Type.ToString();
                } else {
                    // Use the argument's expression type if no explicit substitution is provided
                    substitutions[typeParameter.Name] = argumentExpression?.ToString() ?? "unknown";
                }
            }
        }
    }
    private static Dictionary<string, string> GetGenericName(GenericNameSyntax genericName, ImmutableArray<ITypeParameterSymbol> typeParameters, Dictionary<string, string> substitutions) {
        var typeArguments = genericName.TypeArgumentList.Arguments.ToArray();

        if (typeParameters.Length != typeArguments.Length) {
            throw new InvalidOperationException(
                $"Mismatch in the number of generic arguments. Expected {typeParameters.Length}, but got {typeArguments.Length}.");
        }

        for (int i = 0; i < typeParameters.Length; i++) {
            substitutions[typeParameters[i].Name] = typeArguments[i].ToString();
        }

        return substitutions;
    }

    private static IEnumerable<StatementSyntax> InlineMethodBodyWithArgumentsAndGenerics(MethodDeclarationSyntax methodSyntax, Dictionary<string, string> substitutions, Dictionary<string, string> genericSubstitutions, string returnVariable = null) {
        if (methodSyntax.Body == null && methodSyntax.ExpressionBody == null) {
            return [];
        }

        var inlinedStatements = new List<StatementSyntax>();

        // Handle block body
        if (methodSyntax.Body != null)
            HandleMethodBody(methodSyntax, substitutions, genericSubstitutions, returnVariable, inlinedStatements);

        // Handle expression body
        if (methodSyntax.ExpressionBody != null) {
            HandleExpressionBody(methodSyntax, substitutions, genericSubstitutions, returnVariable, inlinedStatements);
        }

        // log.AppendLine("ok");
        return inlinedStatements;
    }
    private static void HandleExpressionBody(MethodDeclarationSyntax methodSyntax, Dictionary<string, string> substitutions, Dictionary<string, string> genericSubstitutions, string returnVariable, List<StatementSyntax> inlinedStatements) {
        // ReSharper disable once PossibleNullReferenceException
        var expression = methodSyntax.ExpressionBody.Expression.ReplaceNodes(
            methodSyntax.ExpressionBody.Expression.DescendantNodes().OfType<IdentifierNameSyntax>(),
            (original, _) => substitutions.TryGetValue(original.Identifier.Text, out var replacement)
                ? IdentifierName(replacement)
                : genericSubstitutions.TryGetValue(original.Identifier.Text, out var genericReplacement)
                    ? IdentifierName(genericReplacement)
                    : original);

        if (returnVariable != null)
        {
            inlinedStatements.Add(ExpressionStatement(
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(returnVariable),
                    expression)));
        }
        else
        {
            inlinedStatements.Add(ExpressionStatement(expression));
        }
    }
    private static void HandleMethodBody(MethodDeclarationSyntax methodSyntax, Dictionary<string, string> substitutions, Dictionary<string, string> genericSubstitutions, string returnVariable, List<StatementSyntax> inlinedStatements) {
        // ReSharper disable once PossibleNullReferenceException
        foreach (var statement in methodSyntax.Body.Statements) {
            var updatedStatement = statement.ReplaceNodes(
                statement.DescendantNodes().OfType<IdentifierNameSyntax>(),
                (original, _)
                    => substitutions.TryGetValue(original.Identifier.Text, out var replacement)
                        ? IdentifierName(replacement)
                        : genericSubstitutions.TryGetValue(original.Identifier.Text, out var genericReplacement)
                            ? IdentifierName(genericReplacement)
                            : original);

            if (updatedStatement is ReturnStatementSyntax returnStatement && returnVariable != null) {
                updatedStatement = ExpressionStatement(
                    AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        IdentifierName(returnVariable),
                        returnStatement.Expression!));
            }

            inlinedStatements.Add(updatedStatement);
        }
    }

    private static MethodDeclarationSyntax GetContextMethod(INamedTypeSymbol contextType, string methodName, int typeParameterCount, out IMethodSymbol methodSymbol) {
        methodSymbol = contextType.GetMembers()
            .OfType<IMethodSymbol>()
            .FirstOrDefault(m => m.Name == methodName && m.TypeParameters.Length == typeParameterCount);

        if (methodSymbol == null) {
            log.AppendLine($"Method {methodName} with {typeParameterCount} type parameters not found in {contextType.Name}.");
            return null;
        }

        // If the method is in metadata, log and return null
        if (methodSymbol.Locations.All(loc => loc.IsInMetadata)) {
            log.AppendLine($"Method {methodName} is in metadata: {methodSymbol.ContainingAssembly.Name}");
            return null;
        }

        // Get the syntax if available
        var reference = methodSymbol.DeclaringSyntaxReferences
            .FirstOrDefault()?.GetSyntax() as MethodDeclarationSyntax;

        if (reference == null) {
            log.AppendLine($"Failed to locate syntax for {methodName}.");
        }

        return reference;
    }

    private static ExpressionSyntax ReplaceContextPropertyAccess(ExpressionSyntax expression) {
        var origin = expression.ToFullString();
        expression = expression.ReplaceNodes(
            expression.DescendantNodes().OfType<MemberAccessExpressionSyntax>(),
            (original, _)
                => original.Expression.ToString() == "Context"
                    ? IdentifierName(original.Name.Identifier.Text)
                    : original);
        var updated = expression.ToFullString();
        if (updated != origin)
            log.AppendLine($"{origin} -> {updated}");

        return expression;
    }

    private static string StringReplacements(string code) {
        code = ReplaceAddressOfInvocations(code);
        return code;

        static string ReplaceAddressOfInvocations(string code) {
            return code.Replace("&Update(", "Update(");
        }
    }
}
