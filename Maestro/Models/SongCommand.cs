using Microsoft.Xna.Framework.Input;

namespace Maestro.Models
{
	public class SongCommand
	{
		public CommandType Type { get; set; }

		public Keys Key { get; set; }

		public int Duration { get; set; }

		public static SongCommand KeyDownCmd(Keys key)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			return new SongCommand
			{
				Type = CommandType.KeyDown,
				Key = key
			};
		}

		public static SongCommand KeyUpCmd(Keys key)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			return new SongCommand
			{
				Type = CommandType.KeyUp,
				Key = key
			};
		}

		public static SongCommand WaitCmd(int ms)
		{
			return new SongCommand
			{
				Type = CommandType.Wait,
				Duration = ms
			};
		}
	}
}
