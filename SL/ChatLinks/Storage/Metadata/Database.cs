using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace SL.ChatLinks.Storage.Metadata
{
	[System.Runtime.CompilerServices.RequiredMember]
	public sealed record Database
	{
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(Database);
			}
		}

		[System.Runtime.CompilerServices.RequiredMember]
		public string Name { get; set; }

		[System.Runtime.CompilerServices.RequiredMember]
		public int SchemaVersion { get; set; }

		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Database");
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
			builder.Append("Name = ");
			builder.Append((object)Name);
			builder.Append(", SchemaVersion = ");
			builder.Append(SchemaVersion.ToString());
			return true;
		}

		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(SchemaVersion);
		}

		[CompilerGenerated]
		public bool Equals(Database? other)
		{
			if ((object)this != other)
			{
				if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<string>.Default.Equals(Name, other!.Name))
				{
					return EqualityComparer<int>.Default.Equals(SchemaVersion, other!.SchemaVersion);
				}
				return false;
			}
			return true;
		}

		[CompilerGenerated]
		[System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
		private Database(Database original)
		{
			Name = original.Name;
			SchemaVersion = original.SchemaVersion;
		}

		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[System.Runtime.CompilerServices.CompilerFeatureRequired("RequiredMembers")]
		public Database()
		{
		}
	}
}
