using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Benchmark._Generator;

[Generator]
public sealed class BenchGenerator3 : IIncrementalGenerator {
    public void Initialize(IncrementalGeneratorInitializationContext context) {
        var additionalTexts = context.AdditionalTextsProvider
            .Select((text, ct) => text.GetText(ct)?.ToString())
            .Collect();

        var combined = context.CompilationProvider.Combine(additionalTexts);

        context.RegisterSourceOutput(combined, static (spc, source) => {
            var compilation = source.Left;
            var texts = source.Right;

            ComponentsGen.Generate(spc);
            BenchmarksGen.Generate(compilation, texts, spc);
        });
    }

    private static class ComponentsGen {
        private const int Count = 100;

        public static void Generate(SourceProductionContext ctx) {
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

            ctx.AddSource("BenchmarksGenerator/Components.g.cs", SourceText.From(source, Encoding.UTF8));
        }

        private static IEnumerable<MemberDeclarationSyntax> GetComponentTypes() {
            for (var i = 1; i <= Count; i++) {
                yield return ParseMemberDeclaration($"public struct Component{i} : MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent {{ public int Value; }}");
            }
        }

        private static IEnumerable<MemberDeclarationSyntax> GetPaddingTypes() {
            for (var i = 1; i <= Count; i++) {
                yield return ParseMemberDeclaration($"public struct Padding{i} : MorpehComponent, DragonComponent, XenoComponent, FrifloComponent, StaticEcsComponent {{ public long Value1; public long Value2; public long Value3; public long Value4; }}");
            }
        }
    }

    private static class BenchmarksGen {
        private static readonly StringBuilder log = new();
        private const string NamespaceName = "Benchmark";

        public static void Generate(Compilation compilation, ImmutableArray<string?> additionalTexts, SourceProductionContext context) {
            log.Clear();
            try {
                var allReferences = compilation.References;
                compilation = CSharpCompilation.Create(
                    compilation.AssemblyName,
                    compilation.SyntaxTrees,
                    allReferences,
                    (CSharpCompilationOptions)compilation.Options
                );

                var parseOptions = (CSharpParseOptions)compilation.SyntaxTrees.First().Options;
                var additionalSyntaxTrees = additionalTexts
                    .Where(t => !string.IsNullOrEmpty(t))
                    .Select(t => CSharpSyntaxTree.ParseText(t!, parseOptions));
                compilation = compilation.AddSyntaxTrees(additionalSyntaxTrees);

                GenerateBenchmarks(compilation, context);
            }
            catch (Exception ex) {
                context.AddSource("BenchmarksGenerator/_ErrorLog.g.cs", SourceText.From($"/* {ex} */", Encoding.UTF8));
            }
            finally {
                context.AddSource("BenchmarksGenerator/_Log.g.cs", SourceText.From($"/* {log} */", Encoding.UTF8));
            }
        }

        private static void GenerateBenchmarks(Compilation compilation, SourceProductionContext context) {
            var contextInterface = compilation.GetTypeByMetadataName("Benchmark.Context.IBenchmarkContext");
            var benchmarkInterface = compilation.GetTypeByMetadataName("Benchmark.IBenchmark");

            if (contextInterface == null || benchmarkInterface == null)
                return;

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
                    }
                    catch (Exception ex) {
                        context.AddSource($"BenchmarksGenerator/Error_{benchmarkType.Name}_{contextType.Name}.g.cs",
                            SourceText.From($"/*\n{ex}\n*/", Encoding.UTF8));
                    }
                }
            }

            var typeSyntax = ClassDeclaration("BenchMap")
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)))
                .AddMembers(GenerateBenchmarkMapping(byBenchmark, byContext));

            var source = CompilationUnit()
                .AddUsings(
                    UsingDirective(ParseName("System")),
                    UsingDirective(ParseName("System.Collections.Generic")))
                .AddMembers(NamespaceDeclaration(ParseName(NamespaceName))
                    .AddMembers(typeSyntax))
                .NormalizeWhitespace()
                .ToFullString();

            context.AddSource("BenchmarksGenerator/BenchMap.g.cs", SourceText.From(source, Encoding.UTF8));
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

        private static string GenerateBenchmark(INamedTypeSymbol benchmark, INamedTypeSymbol contextType, SourceProductionContext context) {
            var className = $"{benchmark.Name}_{contextType.Name}";
            log.AppendLine($"Generating: {className}...");

            var artifactsPathArgument = $".benchmark_results/{benchmark.Name}";

            var originalUsings = GetOriginalUsings(benchmark);
            var contextUsings = GetOriginalUsings(contextType);
            var mergedUsings = MergeUsings(originalUsings, contextUsings)
                .Concat([UsingDirective(ParseName(contextType.ContainingNamespace.ToDisplayString()))]);

            var benchmarkClass = GetBenchmarkClassDeclaration(className, benchmark, contextType)
                .WithAttributeLists(ReplaceArtifactsPathAttribute(benchmark, artifactsPathArgument));

            var compilationUnit = CompilationUnit()
                .AddUsings(mergedUsings.ToArray())
                .AddMembers(NamespaceDeclaration(ParseName(NamespaceName))
                    .AddMembers(benchmarkClass))
                .NormalizeWhitespace();

            var code = compilationUnit.ToFullString();
            code = StringReplacements(code, GetEntityTypeName(contextType));

            context.AddSource($"BenchmarksGenerator/{className}.g.cs", SourceText.From(code, Encoding.UTF8));
            log.AppendLine($"Generating: {className} done\n");
            return className;
        }

        private static SyntaxList<AttributeListSyntax> ReplaceArtifactsPathAttribute(
            INamedTypeSymbol benchmarkType,
            string artifactsPathValue
        ) {
            var originalAttributes = benchmarkType.GetTypeDeclarationSyntax()?.AttributeLists ?? default;

            var updatedAttributes = originalAttributes.Where(attrList =>
                !attrList.Attributes.Any(attr =>
                    attr.Name.ToString().Contains("ArtifactsPath")));

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
                if (uniqueUsings.Add(usingText))
                    mergedUsings.Add(u);
            }

            return mergedUsings.OrderBy(u => u.Name?.ToString() ?? "null");
        }

        private static ClassDeclarationSyntax GetBenchmarkClassDeclaration(string className, INamedTypeSymbol benchmarkType, INamedTypeSymbol contextType) {
            var classAttributes = CopyClassAttributes(benchmarkType);
            var entityTypeName = GetEntityTypeName(contextType);

            var contextFields = GetFields(contextType, entityTypeName);
            var contextProperties = GetProperties(contextType);
            var contextMethods = GetContextMethods(contextType);
            var contextInnerTypes = GetInnerTypes(contextType);

            var benchmarkFields = GetFields(benchmarkType, entityTypeName);
            var benchmarkProperties = GetProperties(benchmarkType);
            var benchmarkMethods = InlineBenchmarkMethods(benchmarkType, contextType);

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
                yield return FieldDeclaration(
                        VariableDeclaration(ParseTypeName(type))
                            .AddVariables(
                                VariableDeclarator(field.Name)
                                    .WithInitializer(GetInitializer(field))
                                ))
                    .AddModifiers(Token(SyntaxKind.PrivateKeyword));
            }
        }

        private static EqualsValueClauseSyntax GetInitializer(IFieldSymbol fieldSymbol) {
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
            where T : SyntaxNode {
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

            return method.WithBody(Block(ModifyStatementsRecursive(contextType, method.Body.Statements)));
        }

        private static List<StatementSyntax> ModifyStatementsRecursive(INamedTypeSymbol contextType, IEnumerable<StatementSyntax> statements) {
            var newStatements = new List<StatementSyntax>();
            foreach (var statement in statements) {
                switch (statement) {
                    case ExpressionStatementSyntax { Expression: AssignmentExpressionSyntax assignment } when
                        assignment.Left.ToString() == "Context":
                        continue;
                    case BlockSyntax block:
                        newStatements.Add(Block(ModifyStatementsRecursive(contextType, block.Statements)));
                        continue;
                    case IfStatementSyntax ifStatement: {
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
                    case WhileStatementSyntax whileStatement: {
                        var updatedCondition = ReplaceContextPropertyAccess(whileStatement.Condition);
                        var updatedWhile = whileStatement
                            .WithCondition(updatedCondition)
                            .WithStatement(Block(ModifyStatementsRecursive(contextType, whileStatement.Statement.AsBlock().Statements)));
                        newStatements.Add(updatedWhile);
                        continue;
                    }
                    case ForStatementSyntax forStatement: {
                        var updatedCondition = ReplaceContextPropertyAccess(forStatement.Condition);
                        var updatedInitializers = forStatement.Initializers
                            .Select(ReplaceContextPropertyAccess)
                            .ToList();
                        var updatedIncrementors = forStatement.Incrementors
                            .Select(ReplaceContextPropertyAccess)
                            .ToList();

                        var updatedFor = forStatement
                            .WithCondition(updatedCondition)
                            .WithInitializers(SeparatedList(updatedInitializers))
                            .WithIncrementors(SeparatedList(updatedIncrementors))
                            .WithStatement(Block(ModifyStatementsRecursive(contextType, forStatement.Statement.AsBlock().Statements)));

                        newStatements.Add(updatedFor);
                        continue;
                    }
                    case ForEachStatementSyntax foreachStatement: {
                        var updatedExpression = ReplaceContextPropertyAccess(foreachStatement.Expression);

                        var updatedForEach = foreachStatement
                            .WithExpression(updatedExpression)
                            .WithStatement(Block(ModifyStatementsRecursive(contextType, foreachStatement.Statement.AsBlock().Statements)));

                        newStatements.Add(updatedForEach);
                        continue;
                    }
                    case DoStatementSyntax doStatement: {
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
                    case LocalDeclarationStatementSyntax localDeclaration when
                        localDeclaration.Declaration.Variables.FirstOrDefault()?.Initializer?.Value is InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax memberAccess } invocation &&
                        memberAccess.Expression.ToString() == "Context": {

                        var methodName = memberAccess.Name.Identifier.Text;
                        var typeParameterCount = (memberAccess.Name as GenericNameSyntax)?.TypeArgumentList.Arguments.Count ?? 0;
                        var returnVariable = "var " + localDeclaration.Declaration.Variables.First().Identifier.Text;
                        var inlinedMethod = GetContextMethod(contextType, methodName, typeParameterCount, out var symbol);

                        if (inlinedMethod != null) {
                            var substitutions = GetInvocationSubstitutions(invocation, inlinedMethod, symbol);
                            var genericSubstitutions = GetGenericTypeSubstitutions(invocation, symbol);

                            var inlinedStatements = InlineMethodBodyWithArgumentsAndGenerics(inlinedMethod, substitutions, genericSubstitutions, returnVariable);

                            newStatements.Add(Block(inlinedStatements));
                        }

                        continue;
                    }
                    case ExpressionStatementSyntax { Expression: AssignmentExpressionSyntax { Right: InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax fieldMemberAccess } fieldInvocation } assignment } when
                        fieldMemberAccess.Expression.ToString() == "Context": {

                        var methodName = fieldMemberAccess.Name.Identifier.Text;
                        var typeParameterCount = (fieldMemberAccess.Name as GenericNameSyntax)?.TypeArgumentList.Arguments.Count ?? 0;
                        var returnVariable = assignment.Left.ToString();
                        var inlinedMethod = GetContextMethod(contextType, methodName, typeParameterCount, out var symbol);

                        if (inlinedMethod != null) {
                            var substitutions = GetInvocationSubstitutions(fieldInvocation, inlinedMethod, symbol);
                            var genericSubstitutions = GetGenericTypeSubstitutions(fieldInvocation, symbol);
                            var inlinedStatements = InlineMethodBodyWithArgumentsAndGenerics(inlinedMethod, substitutions, genericSubstitutions, returnVariable);
                            newStatements.Add(Block(inlinedStatements));
                            continue;
                        }
                        break;
                    }
                    case ExpressionStatementSyntax { Expression: InvocationExpressionSyntax invocation } when
                        invocation.Expression is MemberAccessExpressionSyntax { Expression: IdentifierNameSyntax name } memberAccess &&
                        name.Identifier.Text == "Context": {
                        var methodName = memberAccess.Name.Identifier.Text;
                        var typeParameterCount = (memberAccess.Name as GenericNameSyntax)?.TypeArgumentList.Arguments.Count ?? 0;
                        var inlinedMethod = GetContextMethod(contextType, methodName, typeParameterCount, out var symbol);

                        if (inlinedMethod != null) {
                            var substitutions = GetInvocationSubstitutions(invocation, inlinedMethod, symbol);
                            var genericSubstitutions = GetGenericTypeSubstitutions(invocation, symbol);

                            var inlinedStatements = InlineMethodBodyWithArgumentsAndGenerics(inlinedMethod, substitutions, genericSubstitutions);

                            newStatements.Add(Block(inlinedStatements));
                        }

                        continue;
                    }
                }

                if (statement is ExpressionStatementSyntax { Expression: MemberAccessExpressionSyntax propertyAccess } &&
                    propertyAccess.Expression.ToString() == "Context") {
                    var updatedExpression = IdentifierName(propertyAccess.Name.Identifier.Text);
                    newStatements.Add(ExpressionStatement(updatedExpression));
                    continue;
                }

                newStatements.Add(statement);
            }

            return newStatements;
        }

        private static Dictionary<string, string> GetInvocationSubstitutions(InvocationExpressionSyntax invocation, MethodDeclarationSyntax method, IMethodSymbol methodSymbol) {
            var substitutions = new Dictionary<string, string>();

            var parameters = method.ParameterList.Parameters.ToList();
            var arguments = invocation.ArgumentList.Arguments;

            for (var i = 0; i < parameters.Count; i++) {
                var parameter = parameters[i];
                if (arguments.Count > i) {
                    var argumentExpression = arguments[i].Expression;
                    substitutions[parameter.Identifier.Text] = argumentExpression.ToString();
                }
                else if (parameter.Default != null) {
                    substitutions[parameter.Identifier.Text] = parameter.Default.Value.ToString();
                }
                else if (methodSymbol.Parameters[i].HasExplicitDefaultValue) {
                    substitutions[parameter.Identifier.Text] = methodSymbol.Parameters[i].ExplicitDefaultValue?.ToString();
                }
            }

            return substitutions;
        }

        private static Dictionary<string, string> GetGenericTypeSubstitutions(InvocationExpressionSyntax invocation, IMethodSymbol methodSymbol) {
            var substitutions = new Dictionary<string, string>();

            if (invocation.Expression is not MemberAccessExpressionSyntax { Name: GenericNameSyntax genericName })
                return substitutions;

            var typeArguments = genericName.TypeArgumentList.Arguments;
            var typeParameters = methodSymbol.TypeParameters;

            for (var i = 0; i < typeParameters.Length; i++) {
                if (typeArguments.Count > i)
                    substitutions[typeParameters[i].Name] = typeArguments[i].ToString();
            }

            return substitutions;
        }

        private static IEnumerable<StatementSyntax> InlineMethodBodyWithArgumentsAndGenerics(MethodDeclarationSyntax methodSyntax, Dictionary<string, string> substitutions, Dictionary<string, string> genericSubstitutions, string returnVariable = null) {
            if (methodSyntax.Body == null && methodSyntax.ExpressionBody == null) {
                return [];
            }

            var inlinedStatements = new List<StatementSyntax>();

            if (methodSyntax.Body != null)
                HandleMethodBody(methodSyntax, substitutions, genericSubstitutions, returnVariable, inlinedStatements);

            if (methodSyntax.ExpressionBody != null) {
                HandleExpressionBody(methodSyntax, substitutions, genericSubstitutions, returnVariable, inlinedStatements);
            }

            return inlinedStatements;
        }

        private static void HandleExpressionBody(MethodDeclarationSyntax methodSyntax, Dictionary<string, string> substitutions, Dictionary<string, string> genericSubstitutions, string returnVariable, List<StatementSyntax> inlinedStatements) {
            var expression = methodSyntax.ExpressionBody!.Expression.ReplaceNodes(
                methodSyntax.ExpressionBody.Expression.DescendantNodes().OfType<IdentifierNameSyntax>(),
                (original, _)
                    => substitutions.TryGetValue(original.Identifier.Text, out var replacement)
                        ? IdentifierName(replacement)
                        : genericSubstitutions.TryGetValue(original.Identifier.Text, out var genericReplacement)
                            ? IdentifierName(genericReplacement)
                            : original);

            if (returnVariable != null) {
                inlinedStatements.Add(ExpressionStatement(
                    AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        IdentifierName(returnVariable),
                        expression)));
            }
            else {
                inlinedStatements.Add(ExpressionStatement(expression));
            }
        }

        private static void HandleMethodBody(MethodDeclarationSyntax methodSyntax, Dictionary<string, string> substitutions, Dictionary<string, string> genericSubstitutions, string returnVariable, List<StatementSyntax> inlinedStatements) {
            foreach (var statement in methodSyntax.Body!.Statements) {
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

            if (methodSymbol.Locations.All(loc => loc.IsInMetadata)) {
                log.AppendLine($"Method {methodName} is in metadata: {methodSymbol.ContainingAssembly.Name}");
                return null;
            }

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

        private static string StringReplacements(string code, string entityType) {
            code = ReplaceAddressOfInvocations(code);
            code = ReplaceEntityTypes(code, entityType);

            return code;

            static string ReplaceAddressOfInvocations(string code) {
                // Remove any & directly preceding a method name followed by (
                return Regex.Replace(code, @"&(?=\s*[A-Za-z_][A-Za-z0-9_]*\s*\()", string.Empty);
            }

            static string ReplaceEntityTypes(string code, string entityType) {
                return code.Replace("TE", entityType);
            }
        }
    }
}
