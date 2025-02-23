using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public static class SymbolExtensions
{
     public static List<INamedTypeSymbol> CollectImplementations(this Compilation compilation, INamedTypeSymbol interfaceType,
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

    public static TypeDeclarationSyntax GetTypeDeclarationSyntax(this INamedTypeSymbol typeSymbol) {
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

    public static BlockSyntax AsBlock(this StatementSyntax statement)
    {
        return statement as BlockSyntax ?? SyntaxFactory.Block(statement);
    }
}
