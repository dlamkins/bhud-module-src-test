using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Discovery.ItemList.Controls
{
	public class PanelHeaderButton : Container
	{
		private AsyncTexture2D DefaultTexture = ServiceContainer.TextureRepository.Textures.PanelHeaderBg;

		private AsyncTexture2D ActiveTexture = ServiceContainer.TextureRepository.Textures.PanelHeaderBgActive;

		public BitmapFont TextFont { get; set; } = GameService.Content.get_DefaultFont18();


		public string Text { get; set; }

		public IList<string> Breadcrumbs { get; set; }

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_MouseOver())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(ActiveTexture), new Rectangle(0, 0, ((Control)this).get_Width(), ((Control)this).get_Height()));
			}
			else
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(DefaultTexture), new Rectangle(0, 0, ((Control)this).get_Width(), ((Control)this).get_Height()));
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(ServiceContainer.TextureRepository.Textures.BackButton), new Rectangle(10, 0, 35, 35));
			if (!string.IsNullOrWhiteSpace(Text))
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Text, TextFont, new Rectangle(60, 0, 150, 35), Color.get_White(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			int xPosition = 60;
			foreach (string breadcrumb in Breadcrumbs)
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, breadcrumb, TextFont, new Rectangle(xPosition, 0, 150, 35), Color.get_White(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				int textWidth = (int)Math.Ceiling(TextFont.MeasureString(breadcrumb).Width);
				xPosition += textWidth + 10;
				if (Breadcrumbs.IndexOf(breadcrumb) + 1 != Breadcrumbs.Count())
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(ServiceContainer.TextureRepository.Textures.BreadcrumbArrow), new Rectangle(xPosition, 19, 25, 25), (Rectangle?)null, Color.get_White(), (float)Math.PI, new Vector2(16f, 16f), (SpriteEffects)0);
					xPosition += 15;
				}
			}
			((Container)this).PaintBeforeChildren(spriteBatch, bounds);
		}

		public PanelHeaderButton()
			: this()
		{
		}
	}
}
