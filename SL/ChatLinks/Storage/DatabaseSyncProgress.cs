using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using GuildWars2;

namespace SL.ChatLinks.Storage
{
	public record DatabaseSyncProgress(string Step, BulkProgress Report)
	{
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(DatabaseSyncProgress);
			}
		}

		public DatabaseSyncProgress(string Step, BulkProgress Report)
		{
			this.Step = ;
			this.Report = Report;
			base._002Ector();
		}

		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("DatabaseSyncProgress");
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
			builder.Append("Step = ");
			builder.Append((object)Step);
			builder.Append(", Report = ");
			builder.Append(Report);
			return true;
		}

		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Step)) * -1521134295 + EqualityComparer<BulkProgress>.Default.GetHashCode(Report);
		}

		[CompilerGenerated]
		public virtual bool Equals(DatabaseSyncProgress? other)
		{
			if ((object)this != other)
			{
				if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<string>.Default.Equals(Step, other!.Step))
				{
					return EqualityComparer<BulkProgress>.Default.Equals(Report, other!.Report);
				}
				return false;
			}
			return true;
		}

		[CompilerGenerated]
		protected DatabaseSyncProgress(DatabaseSyncProgress original)
		{
			Step = original.Step;
			Report = original.Report;
		}
	}
}
