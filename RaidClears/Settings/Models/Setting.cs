using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace RaidClears.Settings.Models
{
	public record Setting<T>(string Key, T DefaultValue, Func<string>? Name = null, Func<string>? Description = null)
	{
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Setting");
			stringBuilder.Append(" { ");
			if (PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		protected virtual bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Key = ");
			builder.Append((object)Key);
			builder.Append(", DefaultValue = ");
			builder.Append(DefaultValue);
			builder.Append(", Name = ");
			builder.Append(Name);
			builder.Append(", Description = ");
			builder.Append(Description);
			return true;
		}
	}
}
