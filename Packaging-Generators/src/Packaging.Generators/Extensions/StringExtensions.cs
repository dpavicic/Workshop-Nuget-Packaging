namespace Six.Generator.Extensions;

public static class StringExtensions
{
	/// <summary>
	/// Sometimes the attribute name is not ending with "Attribute" so we need to add it.
	/// </summary>
	/// <param name="attributeName">Name of the attribute</param>
	/// <returns>Returns the fully qualified attribute name (with "Attribute" suffix).</returns>
	public static string SanitizeAttributeName(this string attributeName)
	{
		if(!attributeName.ToLowerInvariant().Contains("attribute"))
		{
			attributeName += "Attribute";
		}
		return attributeName;
	}

	/// <summary>
	/// Removes the namespace from a string.
	/// </summary>
	/// <param name="source">Source string (probably) with namespace.</param>
	/// <param name="namespace">String without the namespace.</param>
	/// <returns>Returns cleaned string or same string if namespace don't match or exists in the source string.</returns>
	public static string RemoveNamespace(this string source, string @namespace)
	{
		if(source.Contains(@namespace))
		{
			return source.Replace(@namespace, "").Trim('.');
		}
		return source;
	}
	
	
	
}