using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

var domainDocs = Project.Analysis.Documents.Where(x => x.FilePath.Contains("LH.Forcas\\Domain"));

foreach (var document in domainDocs)
{
    var syntaxRoot = document.GetSyntaxRootAsync().Result;
    var semantic = document.GetSemanticModelAsync().Result;

    var clSyntaxNodes = syntaxRoot.DescendantNodes().Where(x => x is ClassDeclarationSyntax);
    var classes = syntaxRoot.DescendantNodes().OfType<ClassDeclarationSyntax>();

    Output.WriteLine($"// {document.FilePath}");
    Output.WriteLine($"// L1: {clSyntaxNodes.Count()}, L2: {classes.Count()}");

    foreach (var cl in classes)
    {
        Output.WriteLine($"// Class: {cl.Identifier.ValueText}");

        var properties = cl.Members.OfType<PropertyDeclarationSyntax>();

        foreach (var property in properties)
        {
            Output.WriteLine($"//\tProperty: {property.Identifier.ValueText}");
        }

        // var typeInfo = semantic.GetTypeInfo((SyntaxNode)cl);

        //Output.WriteLine($"// Class: {cl.Identifier.ValueText}");
        //Output.WriteLine($"// Namespace: {typeInfo.Type.ContainingNamespace}");
    }
}