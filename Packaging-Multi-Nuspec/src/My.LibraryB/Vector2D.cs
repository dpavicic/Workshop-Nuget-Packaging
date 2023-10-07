namespace My.LibraryB;

/// <summary>
/// This class is a basic but robust starting point for any 2D vector operations
/// you might need in game development, computer graphics, or simulation.
/// overloads for syntactic sugar.
/// </summary>
public class Vector2D
{
	private double X { get; set; }
	private double Y { get; set; }

	public Vector2D(double x, double y)
	{
		X = x;
		Y = y;
	}

	/// <summary>
	/// Takes another Vector2D object and returns a new Vector2D that is the sum of the two vectors.
	/// </summary>
	/// <param name="v"></param>
	/// <returns></returns>
	public Vector2D Add(Vector2D v)
	{
		return new Vector2D(X + v.X, Y + v.Y);
	}

	/// <summary>
	/// Takes another Vector2D object and returns a new Vector2D that is the difference between the two vectors.
	/// </summary>
	/// <param name="v"></param>
	/// <returns></returns>
	public Vector2D Subtract(Vector2D v)
	{
		return new Vector2D(X - v.X, Y - v.Y);
	}

	/// <summary>
	/// Takes another Vector2D object and returns the dot product as a double.
	/// </summary>
	/// <param name="v"></param>
	/// <returns></returns>
	public double Dot(Vector2D v)
	{
		return X * v.X + Y * v.Y;
	}

	/// <summary>
	/// Takes a scalar and returns a new Vector2D that is the result of scaling the original vector.
	/// </summary>
	/// <param name="scalar"></param>
	/// <returns></returns>
	public Vector2D Multiply(double scalar)
	{
		return new Vector2D(X * scalar, Y * scalar);
	}

	/// <summary>
	/// Returns the magnitude of the vector as a double.
	/// </summary>
	/// <returns></returns>
	public double Magnitude()
	{
		return Math.Sqrt(X * X + Y * Y);
	}

	/// <summary>
	/// Returns a new Vector2D that is the normalized version of the original vector.
	/// </summary>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException">
	/// The Normalize method throws an InvalidOperationException if you attempt to normalize a zero vector.
	/// </exception>
	public Vector2D Normalize()
	{
		double magnitude = Magnitude();
		if (magnitude == 0)
		{
			throw new InvalidOperationException("Cannot normalize a zero vector.");
		}
		return Multiply(1.0 / magnitude);
	}

	public override string ToString()
	{
		return $"({X}, {Y})";
	}
}