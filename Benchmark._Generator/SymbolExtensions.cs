using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public static class SymbolExtensions
{

    public static BlockSyntax AsBlock(this StatementSyntax statement)
    {
        return statement as BlockSyntax ?? SyntaxFactory.Block(statement);
    }
}
