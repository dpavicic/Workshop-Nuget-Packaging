namespace My.Awesome.Library;

public class Fibonacci
{
	/// <summary>
	/// Calculates the nth Fibonacci number.
	/// </summary>
	/// <param name="n">Number of iterations.</param>
	/// <returns>Returns n-th Fibonacci number as a long</returns>
	/// <exception cref="ArgumentException">The method throws an ArgumentException if the input is negative.</exception>
	public long Calculate(int n)
	{
		if (n < 0)
		{
			throw new ArgumentException("Input should be a non-negative integer.");
		}
        
		if (n == 0) return 0;
		if (n == 1) return 1;

		long a = 0, b = 1, temp;

		for (int i = 2; i <= n; i++)
		{
			temp = a + b;
			a = b;
			b = temp;
		}

		return b;
	}
}