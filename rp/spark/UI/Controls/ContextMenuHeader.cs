using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace rp.spark.UI.Controls
{
	internal sealed class ContextMenuHeader : ContextMenuStripItem
	{
		private static readonly Color HeaderTextColor = new Color(255, 233, 180);

		private static readonly Color LineColor = new Color(255, 233, 180) * 0.35f;

		public ContextMenuHeader(string text)
			: this()
		{
			((ContextMenuStripItem)this).set_Text(text);
			((Control)this).set_Enabled(false);
			((Control)this).set_EffectBehind((ControlEffect)null);
		}

		public override void DoUpdate(GameTime gameTime)
		{
			((Control)this).DoUpdate(gameTime);
			((Control)this).set_Height(20);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			int textWidth = (int)Math.Ceiling(Control.get_Content().get_DefaultFont14().MeasureString(((ContextMenuStripItem)this).get_Text())
				.Width);
				int textX = Math.Max(26, (((Control)this).get_Width() - textWidth) / 2);
				int lineY = ((Control)this).get_Height() / 2 + 1;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(8, lineY, Math.Max(0, textX - 16), 1), LineColor);
				int rightLineX = textX + textWidth + 8;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(rightLineX, lineY, Math.Max(0, ((Control)this).get_Width() - rightLineX - 8), 1), LineColor);
				Rectangle textBounds = default(Rectangle);
				((Rectangle)(ref textBounds))._002Ector(textX, 0, textWidth + 4, ((Control)this).get_Height());
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, ((ContextMenuStripItem)this).get_Text(), Control.get_Content().get_DefaultFont14(), new Rectangle(textBounds.X + 1, textBounds.Y + 1, textBounds.Width, textBounds.Height), StandardColors.get_Shadow(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, ((ContextMenuStripItem)this).get_Text(), Control.get_Content().get_DefaultFont14(), textBounds, HeaderTextColor * 0.9f, false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
		}
	}
