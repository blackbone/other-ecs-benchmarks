using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

public static class MethodInliner
{
    public static MethodDeclarationSyntax InlineMethodCall(this MethodDeclarationSyntax targetMethod, INamedTypeSymbol contextType)
    {
        var statements = targetMethod.Body?.Statements ?? Enumerable.Empty<StatementSyntax>();
        var newStatements = new List<StatementSyntax>();

        foreach (var statement in statements)
        {
            ProcessStatement(statement);
        }

        return targetMethod.WithBody(SyntaxFactory.Block(newStatements));

        void ProcessStatement(StatementSyntax statement)
        {
            switch (statement)
            {
                case ExpressionStatementSyntax expressionStatement:
                    HandleExpression(expressionStatement.Expression);
                    break;

                case LocalDeclarationStatementSyntax localDeclarationStatement:
                    HandleVariable(localDeclarationStatement);
                    break;

                default:
                    // Recurse for nested blocks (e.g., loops or conditionals)
                    newStatements.Add(statement);
                    foreach (var child in statement.ChildNodes().OfType<StatementSyntax>())
                    {
                        ProcessStatement(child);
                    }
                    break;
            }
        }

        void HandleVariable(LocalDeclarationStatementSyntax declaration)
        {
            var variable = declaration.Declaration.Variables.First();
            var initializer = variable.Initializer?.Value;

            if (initializer is InvocationExpressionSyntax invocation)
            {
                InlineInvocation(variable.Identifier.ToString(), invocation);
            }
            else
            {
                newStatements.Add(declaration);
            }
        }

        void HandleExpression(ExpressionSyntax expression)
        {
            if (expression is InvocationExpressionSyntax invocation)
            {
                InlineInvocation(null, invocation);
            }
            else
            {
                newStatements.Add(SyntaxFactory.ExpressionStatement(expression));
            }
        }

        void InlineInvocation(string variableName, InvocationExpressionSyntax invocation)
        {
            var methodName = GetMethodName(invocation, out var genericParameters);
            var invocationArguments = invocation.ArgumentList.Arguments.ToArray();
            var (methodSymbol, methodSyntax) = GetMethodDeclaration(methodName, genericParameters, invocationArguments, contextType);

            if (methodSymbol == null || methodSyntax == null)
            {
                // If method not found, add the original invocation
                newStatements.Add(SyntaxFactory.ExpressionStatement(invocation));
                return;
            }

            // Recursively inline the method body
            var inlinedBody = InlineMethodCall(methodSyntax, contextType).Body ?? SyntaxFactory.Block();
            var body = SyntaxFactory.Block(inlinedBody.Statements);

            // Replace parameters with actual arguments
            body = ReplaceArguments(body, methodSymbol, invocationArguments);

            // Replace return statement with assignment if applicable
            if (!string.IsNullOrEmpty(variableName))
            {
                var returnStatement = body.Statements.LastOrDefault(s => s is ReturnStatementSyntax);
                if (returnStatement != null)
                {
                    body = body.ReplaceNode(returnStatement,
                        SyntaxFactory.ParseStatement($"var {variableName} = {returnStatement.ToFullString().Replace("return", "").Trim()};"));
                }
            }

            // Add the expanded method body
            newStatements.AddRange(body.Statements);
        }
    }

    private static string GetMethodName(InvocationExpressionSyntax invocation, out TypeSyntax[] genericParameters)
    {
        if (invocation.Expression is MemberAccessExpressionSyntax memberAccess)
        {
            genericParameters = memberAccess.Name is GenericNameSyntax genericName
                ? genericName.TypeArgumentList.Arguments.ToArray()
                : System.Array.Empty<TypeSyntax>();

            return memberAccess.Name.Identifier.Text;
        }

        genericParameters = System.Array.Empty<TypeSyntax>();
        return string.Empty;
    }

    private static (IMethodSymbol methodSymbol, MethodDeclarationSyntax methodSyntax) GetMethodDeclaration(
        string methodName,
        TypeSyntax[] genericParameters,
        ArgumentSyntax[] invocationArguments,
        INamedTypeSymbol contextType)
    {
        var method = contextType.GetMembers()
            .OfType<IMethodSymbol>()
            .FirstOrDefault(m => m.Name == methodName
                                 && IsMatchGeneric(m, genericParameters)
                                 && IsMatchArguments(m, invocationArguments));

        if (method == null)
            return (null, null);

        var syntaxRef = method.DeclaringSyntaxReferences.FirstOrDefault();
        var syntax = syntaxRef?.GetSyntax() as MethodDeclarationSyntax;
        return (method, syntax);

        static bool IsMatchGeneric(IMethodSymbol methodSymbol, TypeSyntax[] genericParameters)
            => methodSymbol.TypeParameters.Length >= genericParameters.Length;

        static bool IsMatchArguments(IMethodSymbol methodSymbol, ArgumentSyntax[] argumentSyntaxes)
            => methodSymbol.Parameters.Length == argumentSyntaxes.Length;
    }

    private static BlockSyntax ReplaceArguments(BlockSyntax body, IMethodSymbol methodSymbol, ArgumentSyntax[] invocationArguments)
    {
        var updatedStatements = body.Statements.ToList();

        for (var i = 0; i < invocationArguments.Length; i++)
        {
            var argument = invocationArguments[i].ToFullString();
            var parameter = methodSymbol.Parameters[i].Name;

            // Prepend parameter assignment to the method body
            updatedStatements.Insert(i, SyntaxFactory.ParseStatement($"var {parameter} = {argument};"));
        }

        return SyntaxFactory.Block(updatedStatements);
    }
}
