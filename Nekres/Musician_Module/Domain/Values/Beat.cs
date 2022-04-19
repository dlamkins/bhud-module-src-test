namespace Nekres.Musician_Module.Domain.Values
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
