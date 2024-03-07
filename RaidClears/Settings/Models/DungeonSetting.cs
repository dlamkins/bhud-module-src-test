using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace RaidClears.Settings.Models
{
	public record DungeonSetting<T> : Setting<T>
	{
		[CompilerGenerated]
		protected override Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(DungeonSetting<T>);
			}
		}

		public DungeonSetting(string Key, T DefaultValue, Func<string>? Name = null)
		{
			Func<string> Name2 = Name;
			base._002Ector(Key, DefaultValue, Name2, (Func<string>?)(() => "Enable " + Name2?.Invoke() + " on the dungeon overlay"));
		}

		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("DungeonSetting");
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
		public virtual bool Equals(DungeonSetting<T>? other)
		{
			if ((object)this != other)
			{
				return base.Equals(other);
			}
			return true;
		}

		[CompilerGenerated]
		protected DungeonSetting(DungeonSetting<T> original)
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
