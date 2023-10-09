using CommandLine;

namespace Packaging.Tool;

internal class Program
{
	private class Options
	{
		[Option('i', "iterations", Required = false, HelpText = "Number of iterations (nth Fibonacci number). If not specified, the default value is 1.")]
		public int Iterations { get; set; } = 1;
	}
	
	static void Main(string[] args)
	{
		Parser.Default.ParseArguments<Options>(args)
			.WithParsed(o =>
			{
				if (o is { Iterations: > 0 })
				{
					Console.WriteLine($"Fibonacci number for {o.Iterations} iterations is {CalculateFibonacci(o.Iterations)}");
				}
			});
	}
	
	/// <summary>
	/// Calculates the nth Fibonacci number.
	/// </summary>
	/// <param name="n">Number of iterations.</param>
	/// <returns>Returns n-th Fibonacci number as a long</returns>
	/// <exception cref="ArgumentException">The method throws an ArgumentException if the input is negative.</exception>
	private static long CalculateFibonacci(int n)
	{
		if (n < 0)
		{
			throw new ArgumentException("Input should be a non-negative integer.");
		}
        
		if (n == 0) return 0;
		if (n == 1) return 1;

		long a = 0, b = 1;

		for (int i = 2; i <= n; i++)
		{
			long temp = a + b;
			a = b;
			b = temp;
		}

		return b;
	}
}