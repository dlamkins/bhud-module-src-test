using System;

namespace Frtal.LorebookReader
{
	public static class Palette
	{
		public static readonly (string Name, int R, int G, int B)[] Colors = new(string, int, int, int)[8]
		{
			("None", 180, 180, 180),
			("Red", 200, 70, 70),
			("Orange", 210, 130, 50),
			("Yellow", 210, 195, 80),
			("Green", 90, 180, 90),
			("Blue", 80, 140, 210),
			("Purple", 160, 100, 200),
			("Teal", 70, 180, 180)
		};

		public static (int R, int G, int B) Resolve(string name)
		{
			(string, int, int, int)[] colors = Colors;
			for (int i = 0; i < colors.Length; i++)
			{
				(string, int, int, int) c = colors[i];
				if (string.Equals(c.Item1, name, StringComparison.OrdinalIgnoreCase))
				{
					return (c.Item2, c.Item3, c.Item4);
				}
			}
			return (180, 180, 180);
		}
	}
}
