namespace Nekres.Musician_Module.Domain.Values
{
	public class Fraction
	{
		public int Nominator { get; }

		public int Denominator { get; }

		public Fraction(int nominator, int denominator)
		{
			Nominator = nominator;
			Denominator = denominator;
		}

		public override string ToString()
		{
			return $"{Nominator}/{Denominator}";
		}

		public static Fraction operator *(Fraction a, Fraction b)
		{
			return new Fraction(a.Nominator * b.Nominator, a.Denominator * b.Denominator);
		}

		public override bool Equals(object obj)
		{
			return Equals((Fraction)obj);
		}

		protected bool Equals(Fraction other)
		{
			if (Nominator == other.Nominator)
			{
				return Denominator == other.Denominator;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (Nominator * 397) ^ Denominator;
		}

		public static implicit operator decimal(Fraction fraction)
		{
			return (decimal)fraction.Nominator / (decimal)fraction.Denominator;
		}
	}
}
