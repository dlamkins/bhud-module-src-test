using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

namespace SL.ChatLinks.StaticFiles
{
	[JsonConverter(typeof(SeedIndexJsonConverter))]
	public sealed record SeedIndex
	{
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(SeedIndex);
			}
		}

		public IReadOnlyList<SeedDatabase> Databases { get; init; }

		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SeedIndex");
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
			builder.Append("Databases = ");
			builder.Append(Databases);
			return true;
		}

		[CompilerGenerated]
		public override int GetHashCode()
		{
			return EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<IReadOnlyList<SeedDatabase>>.Default.GetHashCode(Databases);
		}

		[CompilerGenerated]
		public bool Equals(SeedIndex? other)
		{
			if ((object)this != other)
			{
				if ((object)other != null && EqualityContract == other!.EqualityContract)
				{
					return EqualityComparer<IReadOnlyList<SeedDatabase>>.Default.Equals(Databases, other!.Databases);
				}
				return false;
			}
			return true;
		}

		[CompilerGenerated]
		private SeedIndex(SeedIndex original)
		{
			Databases = original.Databases;
		}

		public SeedIndex()
		{
			Databases = Array.Empty<SeedDatabase>();
			base._002Ector();
		}
	}
}
