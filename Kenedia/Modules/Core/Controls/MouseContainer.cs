using System;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class MouseContainer : Panel
	{
		public DetailedTexture Background { get; set; }

		public RectangleDimensions TexturePadding { get; set; } = new RectangleDimensions(50);


		public Point MouseOffset { get; set; } = new Point(15);


		public override void RecalculateLayout()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			if (Background != null)
			{
				Background.Bounds = new Rectangle(0, 0, ((Control)this).get_Width(), ((Control)this).get_Height());
				Background.TextureRegion = new Rectangle(TexturePadding.Left, TexturePadding.Top, Math.Min(Background.Texture.get_Width(), ((Control)this).get_Width()) - TexturePadding.Horizontal, Math.Min(Background.Texture.get_Height(), ((Control)this).get_Height()) - TexturePadding.Vertical);
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).UpdateContainer(gameTime);
			((Control)this).set_Location(Control.get_Input().get_Mouse().get_Position()
				.Add(MouseOffset));
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			Background?.Draw((Control)(object)this, spriteBatch);
			base.PaintBeforeChildren(spriteBatch, bounds);
		}
	}
}