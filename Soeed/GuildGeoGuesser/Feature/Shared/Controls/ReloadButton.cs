using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Controls
{
	public class ReloadButton : Control, IDisposable
	{
		protected int IntervalCount;

		protected Rectangle BOUNDS = new Rectangle(8, 8, 32, 32);

		protected AsyncTexture2D _icon_base = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(784346);

		protected AsyncTexture2D _icon_top = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(156330);

		public ReloadButton()
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Width(48);
			((Control)this).set_Height(48);
		}

		protected override void DisposeControl()
		{
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_icon_base), BOUNDS);
			if (!base._mouseOver)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_icon_top), BOUNDS);
			}
		}
	}
}
