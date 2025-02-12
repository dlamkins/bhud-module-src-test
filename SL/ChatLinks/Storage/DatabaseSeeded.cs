using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using GuildWars2;

namespace SL.ChatLinks.Storage
{
	public sealed record DatabaseSeeded(Language Language, IReadOnlyDictionary<string, int> Updated)
	{
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(DatabaseSeeded);
			}
		}

		public DatabaseSeeded(Language Language, IReadOnlyDictionary<string, int> Updated)
		{
			this.Language = ;
			this.Updated = Updated;
			base._002Ector();
		}

		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("DatabaseSeeded");
			stringBuilder.Append(" { ");
			if (PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Language = ");
			builder.Append(Language);
			builder.Append(", Updated = ");
			builder.Append(Updated);
			return true;
		}

		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<GuildWars2.Language>.Default.GetHashCode(Language)) * -1521134295 + EqualityComparer<IReadOnlyDictionary<string, int>>.Default.GetHashCode(Updated);
		}

		[CompilerGenerated]
		public bool Equals(DatabaseSeeded? other)
		{
			if ((object)this != other)
			{
				if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<GuildWars2.Language>.Default.Equals(Language, other!.Language))
				{
					return EqualityComparer<IReadOnlyDictionary<string, int>>.Default.Equals(Updated, other!.Updated);
				}
				return false;
			}
			return true;
		}

		[CompilerGenerated]
		private DatabaseSeeded(DatabaseSeeded original)
		{
			Language = original.Language;
			Updated = original.Updated;
		}
	}
}
