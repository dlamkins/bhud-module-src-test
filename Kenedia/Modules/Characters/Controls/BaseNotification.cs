using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Characters.Controls
{
	public class BaseNotification : Control
	{
		private static int s_counter;

		public NotificationType NotificationType { get; protected set; }

		public int Id { get; protected set; }

		public BaseNotification()
		{
			s_counter++;
			Id = s_counter;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
		}
	}
}
