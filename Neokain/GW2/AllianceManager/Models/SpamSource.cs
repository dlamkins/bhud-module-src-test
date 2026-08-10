using System;
using Neokain.GW2.WebClient.Models.Enums;

namespace Neokain.GW2.AllianceManager.Models
{
	public readonly struct SpamSource : IEquatable<SpamSource>
	{
		public SpamSourceType Type { get; }

		public Guid Id { get; }

		private SpamSource(SpamSourceType type, Guid id)
		{
			Type = type;
			Id = id;
		}

		public static SpamSource Account(Guid id)
		{
			return new SpamSource(SpamSourceType.Account, id);
		}

		public static SpamSource Alliance(Guid id)
		{
			return new SpamSource(SpamSourceType.Alliance, id);
		}

		public static SpamSource Guild(Guid id)
		{
			return new SpamSource(SpamSourceType.Guild, id);
		}

		public bool Equals(SpamSource other)
		{
			if (Type == other.Type)
			{
				return Id == other.Id;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is SpamSource)
			{
				SpamSource o = (SpamSource)obj;
				return Equals(o);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return ((int)Type * 397) ^ Id.GetHashCode();
		}
	}
}
