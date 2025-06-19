using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Core.Res;
using Kenedia.Modules.Core.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Core.Controls
{
	public class ToggleControl : Control
	{
		public enum ToggleState
		{
			OptionNone,
			Option1,
			Option2
		}

		protected AsyncTexture2D ToggleDot { get; set; } = AsyncTexture2D.FromAssetId(157336);


		protected Texture2D ToggleAreaLeft { get; set; } = TexturesService.GetTextureFromRef(textures_common.ToggleAreaLeft, "ToggleAreaLeft");


		protected Texture2D ToggleAreaMid { get; set; } = TexturesService.GetTextureFromRef(textures_common.ToggleAreaMid, "ToggleAreaMid");


		protected Texture2D ToggleAreaRight { get; set; } = TexturesService.GetTextureFromRef(textures_common.ToggleAreaRight, "ToggleAreaRight");


		protected Rectangle ToggleDotBounds { get; set; }

		protected Rectangle ToggleDotDrawBounds { get; set; } = new Rectangle(4, 4, 24, 24);


		protected Rectangle ToggleBounds { get; set; }

		protected Rectangle ToggleBoundsLeft { get; set; }

		protected Rectangle ToggleBoundsRight { get; set; }

		public string TextLeft { get; set; } = "Option 1";


		protected Rectangle TextLeftBounds { get; set; }

		public string TextRight { get; set; } = "Option 2";


		protected Rectangle TextRightBounds { get; set; }

		public BitmapFont Font { get; set; } = Control.Content.DefaultFont16;


		public Color FontColor { get; set; } = ContentService.Colors.OldLace;


		public ToggleState State { get; set; } = ToggleState.Option1;


		public int TogglePadding { get; set; } = 2;


		public Point ToggleDotSize { get; private set; } = new Point(25, 25);


		public Point ToggleSize { get; private set; }

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			RecalculateLayout();
			spriteBatch.DrawStringOnCtrl(this, TextLeft, Font, TextLeftBounds, FontColor * ((State == ToggleState.Option1) ? 1f : 0.6f), wrap: false, stroke: true);
			spriteBatch.DrawStringOnCtrl(this, TextRight, Font, TextRightBounds, FontColor * ((State == ToggleState.Option2) ? 1f : 0.6f), wrap: false, stroke: true, 1, HorizontalAlignment.Right);
			spriteBatch.DrawOnCtrl(this, ToggleAreaLeft, ToggleBoundsLeft, Color.get_Black() * 0.6f);
			spriteBatch.DrawOnCtrl(this, ToggleAreaMid, ToggleBounds, Color.get_Black() * 0.6f);
			spriteBatch.DrawOnCtrl(this, ToggleAreaRight, ToggleBoundsRight, Color.get_Black() * 0.6f);
			spriteBatch.DrawOnCtrl((Control)this, (Texture2D)ToggleDot, ToggleDotBounds, (Rectangle?)ToggleDotDrawBounds);
		}

		public override void RecalculateLayout()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			ToggleDotDrawBounds = new Rectangle(6, 6, 26, 26);
			ToggleSize = new Point(ToggleDotSize.X * 2, ToggleDotSize.Y + TogglePadding * 2);
			int textWidth = (base.Width - ToggleSize.X) / 2 - 10;
			ToggleBoundsLeft = new Rectangle(textWidth + 10, 0, base.Height / 2, base.Height);
			ToggleBoundsRight = new Rectangle(base.Right - textWidth - 10, 0, base.Height / 2, base.Height);
			Rectangle val = ToggleBoundsLeft;
			int right = ((Rectangle)(ref val)).get_Right();
			val = ToggleBoundsLeft;
			int top = ((Rectangle)(ref val)).get_Top();
			val = ToggleBoundsRight;
			int left = ((Rectangle)(ref val)).get_Left();
			val = ToggleBoundsLeft;
			ToggleBounds = new Rectangle(right, top, left - ((Rectangle)(ref val)).get_Right(), ToggleBoundsLeft.Height);
			SetToggleButtonBounds();
			TextLeftBounds = new Rectangle(0, 0, textWidth, base.Height);
			TextRightBounds = new Rectangle(base.Width - textWidth, 0, textWidth, base.Height);
		}

		private void SetToggleButtonBounds()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			Rectangle toggleDotBounds;
			if (State != ToggleState.Option1)
			{
				if (State != ToggleState.Option2)
				{
					Rectangle toggleBounds = ToggleBounds;
					toggleDotBounds = new Rectangle(((Rectangle)(ref toggleBounds)).get_Center().X - ToggleSize.X / 2, ToggleBounds.Y + TogglePadding, ToggleDotSize.X, ToggleDotSize.Y);
				}
				else
				{
					toggleDotBounds = new Rectangle(ToggleBounds.X + ToggleSize.X - ToggleDotSize.X - TogglePadding, ToggleBounds.Y + TogglePadding, ToggleDotSize.X, ToggleDotSize.Y);
				}
			}
			else
			{
				toggleDotBounds = new Rectangle(ToggleBounds.X + TogglePadding, ToggleBounds.Y + TogglePadding, ToggleDotSize.X, ToggleDotSize.Y);
			}
			ToggleDotBounds = toggleDotBounds;
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			State = ((State != ToggleState.Option1) ? ToggleState.Option1 : ToggleState.Option2);
			SetToggleButtonBounds();
		}
	}
}
