using Blish_HUD;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class BorderedImage : Image
	{
		public RectangleDimensions BorderWidth { get; set; } = new RectangleDimensions(2);


		public Color BorderColor { get; set; } = Colors.ColonialWhite;


		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			((Image)this).Paint(spriteBatch, bounds);
			spriteBatch.DrawFrame((Control)(object)this, bounds, BorderColor, 2);
		}
	}
}
