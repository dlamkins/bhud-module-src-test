using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SL.ChatLinks.Storage
{
	public record DatabaseSyncCompleted(IReadOnlyDictionary<string, int> Updated)
	{
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(DatabaseSyncCompleted);
			}
		}

		public DatabaseSyncCompleted(IReadOnlyDictionary<string, int> Updated)
		{
			this.Updated = ;
			base._002Ector();
		}

		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("DatabaseSyncCompleted");
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
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Updated = ");
			builder.Append(Updated);
			return true;
		}

		[CompilerGenerated]
		public override int GetHashCode()
		{
			return EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<IReadOnlyDictionary<string, int>>.Default.GetHashCode(Updated);
		}

		[CompilerGenerated]
		public virtual bool Equals(DatabaseSyncCompleted? other)
		{
			if ((object)this != other)
			{
				if ((object)other != null && EqualityContract == other!.EqualityContract)
				{
					return EqualityComparer<IReadOnlyDictionary<string, int>>.Default.Equals(Updated, other!.Updated);
				}
				return false;
			}
			return true;
		}

		[CompilerGenerated]
		protected DatabaseSyncCompleted(DatabaseSyncCompleted original)
		{
			Updated = original.Updated;
		}
	}
}
