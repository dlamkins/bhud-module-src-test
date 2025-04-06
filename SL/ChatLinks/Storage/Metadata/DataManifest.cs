using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

namespace SL.ChatLinks.Storage.Metadata
{
	[System.Runtime.CompilerServices.RequiredMember]
	[JsonConverter(typeof(DataManifestJsonConverter))]
	public sealed record DataManifest
	{
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(DataManifest);
			}
		}

		[System.Runtime.CompilerServices.RequiredMember]
		public int Version { get; set; }

		[System.Runtime.CompilerServices.RequiredMember]
		public Dictionary<string, Database> Databases { get; set; }

		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("DataManifest");
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
			builder.Append("Version = ");
			builder.Append(Version.ToString());
			builder.Append(", Databases = ");
			builder.Append(Databases);
			return true;
		}

		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(Version)) * -1521134295 + EqualityComparer<Dictionary<string, Database>>.Default.GetHashCode(Databases);
		}

		[CompilerGenerated]
		public bool Equals(DataManifest? other)
		{
			if ((object)this != other)
			{
				if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<int>.Default.Equals(Version, other!.Version))
				{
					return EqualityComparer<Dictionary<string, Database>>.Default.Equals(Databases, other!.Databases);
				}
				return false;
			}
			return true;
		}

		[CompilerGenerated]
		[System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
		private DataManifest(DataManifest original)
		{
			Version = original.Version;
			Databases = original.Databases;
		}

		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[System.Runtime.CompilerServices.CompilerFeatureRequired("RequiredMembers")]
		public DataManifest()
		{
		}
	}
}
