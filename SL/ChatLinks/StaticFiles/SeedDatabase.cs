using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

namespace SL.ChatLinks.StaticFiles
{
	[System.Runtime.CompilerServices.RequiredMember]
	[JsonConverter(typeof(SeedDatabaseJsonConverter))]
	public sealed record SeedDatabase
	{
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(SeedDatabase);
			}
		}

		[System.Runtime.CompilerServices.RequiredMember]
		public int SchemaVersion { get; init; }

		[System.Runtime.CompilerServices.RequiredMember]
		public string Language { get; init; }

		[System.Runtime.CompilerServices.RequiredMember]
		public string Name { get; init; }

		[System.Runtime.CompilerServices.RequiredMember]
		public string Url { get; init; }

		[System.Runtime.CompilerServices.RequiredMember]
		public string SHA256 { get; init; }

		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SeedDatabase");
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
			builder.Append("SchemaVersion = ");
			builder.Append(SchemaVersion.ToString());
			builder.Append(", Language = ");
			builder.Append((object)Language);
			builder.Append(", Name = ");
			builder.Append((object)Name);
			builder.Append(", Url = ");
			builder.Append((object)Url);
			builder.Append(", SHA256 = ");
			builder.Append((object)SHA256);
			return true;
		}

		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((((EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(SchemaVersion)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Language)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Url)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SHA256);
		}

		[CompilerGenerated]
		public bool Equals(SeedDatabase? other)
		{
			if ((object)this != other)
			{
				if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<int>.Default.Equals(SchemaVersion, other!.SchemaVersion) && EqualityComparer<string>.Default.Equals(Language, other!.Language) && EqualityComparer<string>.Default.Equals(Name, other!.Name) && EqualityComparer<string>.Default.Equals(Url, other!.Url))
				{
					return EqualityComparer<string>.Default.Equals(SHA256, other!.SHA256);
				}
				return false;
			}
			return true;
		}

		[CompilerGenerated]
		[System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
		private SeedDatabase(SeedDatabase original)
		{
			SchemaVersion = original.SchemaVersion;
			Language = original.Language;
			Name = original.Name;
			Url = original.Url;
			SHA256 = original.SHA256;
		}

		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[System.Runtime.CompilerServices.CompilerFeatureRequired("RequiredMembers")]
		public SeedDatabase()
		{
		}
	}
}
