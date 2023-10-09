using System.Collections.Immutable;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Six.Generator.Extensions;
using Six.Modular.Generator.Shared;

namespace Packaging.Generators;

internal record ClassData
{
	public string ClassName { get; set; } = default!;
	public string NamespaceName { get; set; } = default!;
}


[Generator]
public class HelloThereGenerator : IIncrementalGenerator
{
	private const string HelloThereAttributeName = "HelloThere";

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		var modularBuilderProvider = context.SyntaxProvider.CreateSyntaxProvider(
			predicate: (syntaxNode, _) =>
			{
				// Quick checks for validity; accept only classes with the HelloThere attribute
				if(syntaxNode is not ClassDeclarationSyntax classDeclarationSyntax) return false;
				if(!classDeclarationSyntax.HasAttribute(HelloThereAttributeName)) return false;
				return true;
			},
			transform: (transformContext, cancellationToken) =>
			{
				cancellationToken.ThrowIfCancellationRequested();

				ClassDeclarationSyntax classDeclarationSyntax = (ClassDeclarationSyntax)transformContext.Node;
				SemanticModel semanticModel = transformContext.SemanticModel;
				var symbol = semanticModel.GetDeclaredSymbol(transformContext.Node) as INamedTypeSymbol;
				if(symbol == null) return null;

				// Extract class data
				return new ClassData()
				{
					ClassName = classDeclarationSyntax.Identifier.Text, 
					NamespaceName = symbol.ContainingNamespace.ToString()
				};

			});

		var compilation = context.CompilationProvider.Combine(modularBuilderProvider.Collect());

		context.RegisterSourceOutput(
			compilation,
			(sourceProductionContext, data) => GenerateSource(sourceProductionContext, data.Left, data.Right)
		);
	}
	
	private static void GenerateSource(
		SourceProductionContext context,
		Compilation compilation,
		ImmutableArray<ClassData?> compilationData
	)
	{
		if(compilationData == null || compilationData.Length == 0) return;

		// ACTUAL CODE GENERATION
		Assembly assembly = Assembly.GetExecutingAssembly();
		
		// Generate HelloThere attribute
		context.RenderTemplate(
			assembly,
			"Packaging.Generators.Templates.HelloThereAttr.tpl",
			"HelloThereAttribute.g.cs",
			null
		);
		
		// Generate all classes with the HelloThere attribute
		foreach(var data in compilationData)
		{
			if(data == null) continue;
			
			string fileName = $"{data.ClassName}.g.cs";
			context.RenderTemplate(
				assembly,
				"Packaging.Generators.Templates.HelloThereClass.tpl",
				fileName,
				data
			);
		}
	}
	
}