using System;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Scriban;

// ReSharper disable StringLiteralTypo
// ReSharper disable CommentTypo

namespace Six.Modular.Generator.Shared;

public static class TemplatingExtensions
{
	/// <summary>
	///   Renders the code template with Scriban and adds it to the source production context.
	/// </summary>
	/// <param name="context">
	///   <see cref="SourceProductionContext" />
	/// </param>
	/// <param name="templateResourceAssembly">
	///   Assembly where templates are embedded.<br></br>
	///   Use: <code>var assembly = Assembly.GetExecutingAssembly();</code>
	///   since the templates are usually embedded in the same assembly as the generator.
	///   <br></br>
	///   <b>NOTE: Template files must be set as Embedded Resources in the project file.</b>
	/// </param>
	/// <param name="templatePath">Full solution path to the template.</param>
	/// <param name="hintName">File name of the generated code. For example: <code>MyClass.g.cs</code></param>
	/// <param name="model">Optional data model to pass to the template.</param>
	public static void RenderTemplate(
		this SourceProductionContext context,
		Assembly templateResourceAssembly,
		string templatePath,
		string hintName,
		object? model = null
	)
	{
		string templateResource = GetEmbeddedResource(templatePath, templateResourceAssembly);

		// Parse the template with Scriban
		try
		{
			Template? template = Template.Parse(templateResource);
			string? renderedCode = template.Render(model);
			context.AddSource(hintName, SourceText.From(renderedCode, Encoding.UTF8));
		}
		catch(Exception e)
		{
			context.ReportDiagnostic(
				Diagnostic.Create(
					new DiagnosticDescriptor(
						"MGDBR001",
						"Error while rendering Scriban template",
						e.Message, "Error",
						DiagnosticSeverity.Error,
						true),
					Location.None));
		}
	}
	
	/// <summary>
	///   Gets the content of the embedded resource.
	/// </summary>
	/// <param name="resourceName">Full path of the resource (as project folder namespaced path)</param>
	/// <param name="assembly">Assembly where resource is embedded</param>
	/// <returns>Resource as a string</returns>
	/// <exception cref="InvalidOperationException"></exception>
	private static string GetEmbeddedResource(string resourceName, Assembly assembly)
	{
		using Stream? stream = assembly.GetManifestResourceStream(resourceName);
		if(stream == null)
		{
			throw new InvalidOperationException(
				$"Resource {resourceName} not found in templateResourceAssembly {assembly.FullName}.");
		}

		using StreamReader reader = new(stream);
		return reader.ReadToEnd();
	}
}