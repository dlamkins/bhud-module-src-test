using System;
using System.Diagnostics;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.Community.Pathing.UI.Controls
{
	internal class DonateHero : Container
	{
		private AsyncTexture2D _backgroundTexture = AsyncTexture2D.FromAssetId(1234872);

		private AsyncTexture2D _heartTexture = AsyncTexture2D.FromAssetId(156127);

		private BlueButton _donateButton;

		public DonateHero()
			: this()
		{
			BlueButton obj = new BlueButton
			{
				Text = "Donate"
			};
			((Control)obj).set_Parent((Container)(object)this);
			_donateButton = obj;
			((Control)_donateButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Process.Start("https://ko-fi.com/freesnow");
			});
		}

		public override void RecalculateLayout()
		{
			if (_donateButton != null)
			{
				((Control)_donateButton).set_Top(((Control)this).get_Height() / 2 - 12);
				((Control)_donateButton).set_Right(((Control)this).get_Width() - 12);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			if (!((Control)this).get_MouseOver())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, Color.get_Black() * 0.75f);
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_backgroundTexture), bounds, Color.get_Pink() * 0.8f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_heartTexture), new Rectangle(-8, -8, 64, 64));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_heartTexture), new Rectangle(24, -8, 64, 64));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_heartTexture), new Rectangle(56, -8, 64, 64));
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "Consider  helping  Freesnöw  with  server  expenses:", GameService.Content.get_DefaultFont18(), bounds, Color.get_White(), false, (HorizontalAlignment)1, (VerticalAlignment)1);
		}
	}
}
