using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics
{
	internal readonly struct ActivityLink : IEquatable<ActivityLink>
	{
		private readonly Activity.TagsLinkedList _tags;

		public ActivityContext Context { get; }

		public IEnumerable<KeyValuePair<string, object?>>? Tags => _tags;

		public ActivityLink(ActivityContext context, ActivityTagsCollection? tags = null)
		{
			Context = context;
			_tags = ((tags != null && tags!.Count > 0) ? new Activity.TagsLinkedList(tags) : null);
		}

		public override bool Equals([_003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003ENotNullWhen(true)] object? obj)
		{
			if (obj is ActivityLink)
			{
				ActivityLink value = (ActivityLink)obj;
				return Equals(value);
			}
			return false;
		}

		public bool Equals(ActivityLink value)
		{
			if (Context == value.Context)
			{
				return value.Tags == Tags;
			}
			return false;
		}

		public static bool operator ==(ActivityLink left, ActivityLink right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(ActivityLink left, ActivityLink right)
		{
			return !left.Equals(right);
		}

		public Activity.Enumerator<KeyValuePair<string, object?>> EnumerateTagObjects()
		{
			return new Activity.Enumerator<KeyValuePair<string, object>>(_tags?.First);
		}

		public override int GetHashCode()
		{
			if (this == default(ActivityLink))
			{
				return 0;
			}
			int num = 5381;
			num = (num << 5) + num + Context.GetHashCode();
			if (Tags != null)
			{
				foreach (KeyValuePair<string, object> item in Tags!)
				{
					num = (num << 5) + num + item.Key.GetHashCode();
					if (item.Value != null)
					{
						num = (num << 5) + num + item.Value.GetHashCode();
					}
				}
				return num;
			}
			return num;
		}
	}
}
