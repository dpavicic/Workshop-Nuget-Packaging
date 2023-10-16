namespace My.Awesome.Library.Tests;

public class FibonacciTest
{
	[Fact]
	public void ShouldProduceCorrectFibonacciResult()
	{
		var f = new Fibonacci();
		
		// Zero-based Fibonacci sequence for 3th iteration is 2
		Assert.Equal(2, f.Calculate(3));
	}
}