namespace Nekres.Musician.Core.Domain
{
	public class Beat
	{
		public decimal Value { get; }

		public Beat(decimal value)
		{
			Value = value;
		}

		public static implicit operator decimal(Beat beat)
		{
			return beat.Value;
		}
	}
}
