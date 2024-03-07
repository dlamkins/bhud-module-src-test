using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace RaidClears.Settings.Models
{
	public record Setting<T>(string Key, T DefaultValue, Func<string>? Name = null, Func<string>? Description = null)
	{
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(Setting<T>);
			}
		}

		public Setting(string Key, T DefaultValue, Func<string>? Name = null, Func<string>? Description = null)
		{
			this.Key = ;
			this.DefaultValue = DefaultValue;
			this.Name = Name;
			this.Description = Description;
			base._002Ector();
		}

		[CompilerGenerated]
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

		[CompilerGenerated]
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

		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (((EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Key)) * -1521134295 + EqualityComparer<T>.Default.GetHashCode(DefaultValue)) * -1521134295 + EqualityComparer<Func<string>>.Default.GetHashCode(Name)) * -1521134295 + EqualityComparer<Func<string>>.Default.GetHashCode(Description);
		}

		[CompilerGenerated]
		public virtual bool Equals(Setting<T>? other)
		{
			if ((object)this != other)
			{
				if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<string>.Default.Equals(Key, other!.Key) && EqualityComparer<T>.Default.Equals(DefaultValue, other!.DefaultValue) && EqualityComparer<Func<string>>.Default.Equals(Name, other!.Name))
				{
					return EqualityComparer<Func<string>>.Default.Equals(Description, other!.Description);
				}
				return false;
			}
			return true;
		}

		[CompilerGenerated]
		protected Setting(Setting<T> original)
		{
			Key = original.Key;
			DefaultValue = original.DefaultValue;
			Name = original.Name;
			Description = original.Description;
		}
	}
}
