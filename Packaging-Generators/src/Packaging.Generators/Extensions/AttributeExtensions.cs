
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Six.Generator.Extensions;

/// <summary>
///   Utility extensions for easier handling of attributes in Roslyn
/// </summary>
public static class AttributeExtensions
{
	
	// Using TypeDeclarationSyntax
// -------------------------------------------------------------------------------------------------------

	/// <summary>
	///   Checks if a type has a specific attribute
	/// </summary>
	/// <param name="typeDeclaration"></param>
	/// <param name="name">Attribute name to check</param>
	/// <returns>Returns true if type has specified attribute</returns>
	public static bool HasAttribute(this TypeDeclarationSyntax typeDeclaration, string name)
	{
		if(typeDeclaration.AttributeLists.Count == 0)
		{
			return false;
		}

		bool hasAttribute = typeDeclaration.AttributeLists
			.SelectMany(attributeListSyntax => attributeListSyntax.Attributes)
			.Select(attributeSyntax => attributeSyntax.Name.ToString())
			.Contains(name);
		return hasAttribute;
	}

	public static bool HasAnyAttribute(this TypeDeclarationSyntax typeDeclaration, IEnumerable<string> attributeNames)
	{
		if (typeDeclaration.AttributeLists.Count == 0)
		{
			return false;
		}

		var declaredAttributes = typeDeclaration.AttributeLists
			.SelectMany(attributeListSyntax => attributeListSyntax.Attributes)
			.Select(attributeSyntax => attributeSyntax.Name.ToString());

		// Generate all possible names for the attributes (both with and without the "Attribute" suffix)
		var allNamesToCheck = new List<string>();
		foreach (var name in attributeNames)
		{
			allNamesToCheck.Add(name);
			if (name.EndsWith("Attribute"))
			{
				allNamesToCheck.Add(name.Substring(0, name.Length - 9)); // without "Attribute" suffix
			}
			else
			{
				allNamesToCheck.Add($"{name}Attribute"); // with "Attribute" suffix
			}
		}

		return declaredAttributes.Intersect(allNamesToCheck).Any();
	}

	/// <summary>
	///   Gets all arguments and their values of a specific attribute
	/// </summary>
	/// <param name="typeDeclaration"></param>
	/// <param name="semanticModel"></param>
	/// <param name="attributeName">Name of the attribute from which to extract arguments and their values.</param>
	/// <returns>Returns a Dictionary with string => objects (argument name => value)</returns>
	public static Dictionary<string, object>? GetAttributeArguments(this TypeDeclarationSyntax typeDeclaration,
		SemanticModel semanticModel, string attributeName)
	{
		if(typeDeclaration.AttributeLists.Count == 0)
		{
			return null;
		}

		ISymbol? classSymbol = semanticModel.GetDeclaredSymbol(typeDeclaration);
		if(classSymbol == null)
		{
			return null;
		}

		AttributeData? attributeData = classSymbol.GetAttributes().FirstOrDefault(ad =>
			ad.AttributeClass?.Name == attributeName || ad.AttributeClass?.Name == attributeName.Replace("Attribute", ""));
		if(attributeData == null)
		{
			return null;
		}

		Dictionary<string, object> arguments = new Dictionary<string, object>();

		// Named arguments
		foreach(KeyValuePair<string, TypedConstant> namedArg in attributeData.NamedArguments)
		{
			if(namedArg.Value.Value != null)
			{
				arguments[namedArg.Key] = namedArg.Value.Value;
			}
		}

		// Constructor arguments
		for(int i = 0; i < attributeData.ConstructorArguments.Length; i++)
		{
			object? argumentValue = attributeData.ConstructorArguments[i].Value;
			if(argumentValue != null)
			{
				// Here we use "Arg{i}" as a placeholder for the argument's name, since the actual names are not retrievable from this data.
				arguments[$"Arg{i}"] = argumentValue;
			}
		}

		return arguments;
	}

	
	
	// Using ISymbol
	// -------------------------------------------------------------------------------------------------------
	
	/// <summary>
	/// Checks if ISymbol has a specific attribute.
	/// </summary>
	/// <remarks>
	/// It will check both the attribute name and the attribute name with "Attribute" suffix.
	/// </remarks>
	/// <param name="symbol"><see cref="ISymbol"/> to check if attribute is attached.</param>
	/// <param name="attributeName">Name of the attribute</param>
	/// <returns></returns>
	public static bool HasAttribute(this ISymbol symbol, string attributeName)
	{
		bool hasAttribute = symbol.GetAttributes().Any(ad => ad.AttributeClass?.Name == attributeName);
		if(hasAttribute) return true;
		
		string sanitizedName = attributeName.SanitizeAttributeName();
		return symbol.GetAttributes().Any(ad => ad.AttributeClass?.Name == sanitizedName);
	}

	/// <summary>
	/// Gets all attributes of a specific attribute name attached to a <see cref="ISymbol"/>
	/// </summary>
	/// <param name="symbol"><see cref="ISymbol"/> tp check for attribute data.</param>
	/// <param name="attributeName">Name of the attribute (must end with "Attribute" keyword)</param>
	/// <returns>Returns a list of <see cref="AttributeData"/> objects or empty list.</returns>
	public static List<AttributeData> GetAttributeData(this ISymbol symbol, string attributeName)
	{
		List<AttributeData> data = symbol.GetAttributes().Where(attr => attr.AttributeClass?.Name == attributeName).ToList();
		if(data.Count > 0) return data;
		
		string sanitizedName = attributeName.SanitizeAttributeName();
		return symbol.GetAttributes().Where(attr => attr.AttributeClass?.Name == sanitizedName).ToList();
	}

	/// <summary>
	/// Find if any of the attribute names in the passed list are applied to the <see cref="ISymbol"/>
	/// </summary>
	/// <param name="symbol"><see cref="ISymbol"/> instance to check for attributes.</param>
	/// <param name="attributeNames">List of attribute names to check for.</param>
	/// <returns>Returns an attribute name which is applied on the class or null.</returns>
	public static string? FindMatchingAttribute(this ISymbol symbol, List<string> attributeNames)
	{
		var attrs = symbol.GetAttributes();
		foreach(var attr in attributeNames)
		{
			if(attrs.Any(ad => ad.AttributeClass?.Name.Contains(attr) ?? false))
			{
				return attr;
			}
		}
		return null;
	}
	
	/// <summary>
	///   Gets all arguments and their values of a specific attribute from a <see cref="ISymbol"/>
	/// </summary>
	/// <param name="symbol">
	///   Given a SyntaxNode, you can retrieve its corresponding ISymbol using the SemanticModel.
	///   Obtain the SemanticModel: If you're inside a method where you're processing syntax nodes in your generator, you might
	///   have a SyntaxNode and the current Compilation. You can use the Compilation to get the SemanticModel for any syntax
	///   tree:
	///   <br></br>
	///   <code>SemanticModel semanticModel = compilation.GetSemanticModel(syntaxTree);</code>
	///   <br></br>
	///   Get the Symbol: Once you have the SemanticModel and a SyntaxNode, you can get the ISymbol for that node:
	///   <br></br>
	///   <code>ISymbol symbol = semanticModel.GetDeclaredSymbol(syntaxNode);</code>
	///   <br></br>
	///   For <see cref="TypeDeclarationSyntax" /> (classes, structs, enums, etc.), GetDeclaredSymbol will return a
	///   <see cref="INamedTypeSymbol" />.
	///   <br></br>
	///   For <see cref="MethodDeclarationSyntax" />, it will return an <see cref="IMethodSymbol" />.
	///   <br></br>
	///   For <see cref="PropertyDeclarationSyntax" />, it will return an <see cref="IPropertySymbol" />.
	///   <br></br>
	///   ...and so on.
	/// </param>
	/// <param name="attributeName">Name of the attribute</param>
	/// <returns>Returns the Dictionary of argumentName->Data or null.</returns>
	public static Dictionary<string, object>? GetAttributeArguments(this ISymbol symbol, string attributeName)
	{
		Dictionary<string, object>? arguments = _GetAttributeArguments(symbol, attributeName);
		if(arguments != null) return arguments;
		
		string sanitizedName = attributeName.SanitizeAttributeName();
		return _GetAttributeArguments(symbol, sanitizedName);
	}
	
	
	// PRIVATE
	// -------------------------------------------------------------------------------------------------------
	
	private static Dictionary<string, object>? _GetAttributeArguments(ISymbol symbol, string attributeName)
	{
		AttributeData? attributeData =
			symbol.GetAttributes().FirstOrDefault(ad => ad.AttributeClass?.Name == attributeName);
		
		if(attributeData == null) return null;
		
		Dictionary<string, object> arguments = new();

		// Named arguments
		foreach(KeyValuePair<string, TypedConstant> namedArg in attributeData.NamedArguments)
		{
			if(namedArg.Value.Value != null)
			{
				arguments[namedArg.Key] = namedArg.Value.Value;
			}
		}

		// Constructor arguments
		ImmutableArray<IParameterSymbol>? constructorParameters = attributeData.AttributeConstructor?.Parameters;
		if(!constructorParameters.HasValue || constructorParameters.Value.IsDefault)
		{
			return null; // or you can continue with other logic if needed
		}

		for(int i = 0; i < constructorParameters.Value.Length; i++)
		{
			string parameterName = constructorParameters.Value[i].Name;
			object? parameterValue = attributeData.ConstructorArguments[i].Value;
			if(parameterValue != null)
			{
				arguments[parameterName] = parameterValue;
			}
		}

		return arguments;
	}

	
	
	
	
}