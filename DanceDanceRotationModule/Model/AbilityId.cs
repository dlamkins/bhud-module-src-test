using System;

namespace DanceDanceRotationModule.Model
{
	public readonly struct AbilityId : IEquatable<AbilityId>
	{
		public static readonly AbilityId Unknown = new AbilityId(-9999999);

		public int Raw { get; }

		public AbilityId(int raw)
		{
			Raw = raw;
		}

		public bool Equals(AbilityId other)
		{
			return Raw == other.Raw;
		}

		public override bool Equals(object obj)
		{
			if (obj is AbilityId)
			{
				AbilityId other = (AbilityId)obj;
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Raw.GetHashCode();
		}
	}
}
