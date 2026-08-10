using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.Logging
{
	internal readonly struct EventId : IEquatable<EventId>
	{
		public int Id { get; }

		public string? Name { get; }

		public static implicit operator EventId(int i)
		{
			return new EventId(i);
		}

		public static bool operator ==(EventId left, EventId right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(EventId left, EventId right)
		{
			return !left.Equals(right);
		}

		public EventId(int id, string? name = null)
		{
			Id = id;
			Name = name;
		}

		public override string ToString()
		{
			return Name ?? Id.ToString();
		}

		public bool Equals(EventId other)
		{
			return Id == other.Id;
		}

		public override bool Equals([_003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003ENotNullWhen(true)] object? obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is EventId)
			{
				EventId other = (EventId)obj;
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Id;
		}
	}
}
