using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NexusShim
{
	internal class LeftOffsetTrick : Control
	{
		public uint IconCount { get; set; }

		public LeftOffsetTrick()
			: this()
		{
			((Control)this).set_ZIndex(int.MinValue);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			CornerIcon.set_LeftOffset((int)(35.3 * (double)(IconCount + 1)) + 2);
		}
	}
}
