using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SL.ChatLinks.Storage
{
	public record DatabaseMigrated
	{
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(DatabaseMigrated);
			}
		}

		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("DatabaseMigrated");
			stringBuilder.Append(" { ");
			if (PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		[CompilerGenerated]
		protected virtual bool PrintMembers(StringBuilder builder)
		{
			return false;
		}

		[CompilerGenerated]
		public override int GetHashCode()
		{
			return EqualityComparer<Type>.Default.GetHashCode(EqualityContract);
		}

		[CompilerGenerated]
		public virtual bool Equals(DatabaseMigrated? other)
		{
			if ((object)this != other)
			{
				if ((object)other != null)
				{
					return EqualityContract == other!.EqualityContract;
				}
				return false;
			}
			return true;
		}

		[CompilerGenerated]
		protected DatabaseMigrated(DatabaseMigrated original)
		{
		}

		public DatabaseMigrated()
		{
		}
	}
}
