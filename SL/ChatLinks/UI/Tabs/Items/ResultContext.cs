using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public sealed record ResultContext
	{
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(ResultContext);
			}
		}

		public int ResultTotal { get; set; }

		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ResultContext");
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
			builder.Append("ResultTotal = ");
			builder.Append(ResultTotal.ToString());
			return true;
		}

		[CompilerGenerated]
		public override int GetHashCode()
		{
			return EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(ResultTotal);
		}

		[CompilerGenerated]
		public bool Equals(ResultContext? other)
		{
			if ((object)this != other)
			{
				if ((object)other != null && EqualityContract == other!.EqualityContract)
				{
					return EqualityComparer<int>.Default.Equals(ResultTotal, other!.ResultTotal);
				}
				return false;
			}
			return true;
		}

		[CompilerGenerated]
		private ResultContext(ResultContext original)
		{
			ResultTotal = original.ResultTotal;
		}

		public ResultContext()
		{
		}
	}
}
