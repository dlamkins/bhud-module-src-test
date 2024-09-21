using System;

namespace MysticCrafting.Module.Menu
{
	public class EnumFilterMenuItem<T> : FilterMenuItem where T : Enum
	{
		public T Value { get; set; }

		public EnumFilterMenuItem(T value, string text)
			: base(text)
		{
			Value = value;
		}
	}
}
