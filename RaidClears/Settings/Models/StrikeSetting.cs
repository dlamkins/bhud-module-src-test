using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace RaidClears.Settings.Models
{
	public record StrikeSetting<T> : Setting<T>
	{
		[CompilerGenerated]
		protected override Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(StrikeSetting<T>);
			}
		}

		public StrikeSetting(string Key, T DefaultValue, Func<string>? Name = null)
		{
			Func<string> Name2 = Name;
			base._002Ector(Key, DefaultValue, Name2, (Func<string>?)(() => "Enable " + Name2?.Invoke() + " on the strike overlay"));
		}

		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("StrikeSetting");
			stringBuilder.Append(" { ");
			if (PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		[CompilerGenerated]
		protected override bool PrintMembers(StringBuilder builder)
		{
			return base.PrintMembers(builder);
		}

		[CompilerGenerated]
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		[CompilerGenerated]
		public virtual bool Equals(StrikeSetting<T>? other)
		{
			if ((object)this != other)
			{
				return base.Equals(other);
			}
			return true;
		}

		[CompilerGenerated]
		protected StrikeSetting(StrikeSetting<T> original)
			: base((Setting<T>)original)
		{
		}

		[CompilerGenerated]
		public void Deconstruct(out string Key, out T DefaultValue, out Func<string>? Name)
		{
			Key = base.Key;
			DefaultValue = base.DefaultValue;
			Name = base.Name;
		}
	}
}
