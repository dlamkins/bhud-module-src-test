using System;
using System.Text;

namespace RaidClears.Settings.Models
{
	public record DungeonSetting<T> : Setting<T>
	{
		public DungeonSetting(string Key, T DefaultValue, Func<string>? Name = null)
		{
			Func<string> Name2 = Name;
			base._002Ector(Key, DefaultValue, Name2, (Func<string>?)(() => "Enable " + Name2?.Invoke() + " on the dungeon overlay"));
		}

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

		public void Deconstruct(out string Key, out T DefaultValue, out Func<string>? Name)
		{
			Key = base.Key;
			DefaultValue = base.DefaultValue;
			Name = base.Name;
		}
	}
}
