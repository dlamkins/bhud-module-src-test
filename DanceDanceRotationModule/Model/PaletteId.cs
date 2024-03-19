using System;

namespace DanceDanceRotationModule.Model
{
	public readonly struct PaletteId : IEquatable<PaletteId>
	{
		private int Raw { get; }

		public PaletteId(int raw)
		{
			Raw = raw;
		}

		public bool Equals(PaletteId other)
		{
			return Raw == other.Raw;
		}

		public override bool Equals(object obj)
		{
			if (obj is PaletteId)
			{
				PaletteId other = (PaletteId)obj;
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Raw;
		}
	}
}
