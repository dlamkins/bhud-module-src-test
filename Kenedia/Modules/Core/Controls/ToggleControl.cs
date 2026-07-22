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
			RecalculateLayout();
			spriteBatch.DrawStringOnCtrl(this, TextLeft, Font, TextLeftBounds, FontColor * ((State == ToggleState.Option1) ? 1f : 0.6f), wrap: false, stroke: true);
			spriteBatch.DrawStringOnCtrl(this, TextRight, Font, TextRightBounds, FontColor * ((State == ToggleState.Option2) ? 1f : 0.6f), wrap: false, stroke: true, 1, HorizontalAlignment.Right);
			spriteBatch.DrawOnCtrl(this, ToggleAreaLeft, ToggleBoundsLeft, Color.Black * 0.6f);
			spriteBatch.DrawOnCtrl(this, ToggleAreaMid, ToggleBounds, Color.Black * 0.6f);
			spriteBatch.DrawOnCtrl(this, ToggleAreaRight, ToggleBoundsRight, Color.Black * 0.6f);
			spriteBatch.DrawOnCtrl(this, ToggleDot, ToggleDotBounds, ToggleDotDrawBounds);
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			ToggleDotDrawBounds = new Rectangle(6, 6, 26, 26);
			ToggleSize = new Point(ToggleDotSize.X * 2, ToggleDotSize.Y + TogglePadding * 2);
			int textWidth = (base.Width - ToggleSize.X) / 2 - 10;
			ToggleBoundsLeft = new Rectangle(textWidth + 10, 0, base.Height / 2, base.Height);
			ToggleBoundsRight = new Rectangle(base.Right - textWidth - 10, 0, base.Height / 2, base.Height);
			ToggleBounds = new Rectangle(ToggleBoundsLeft.Right, ToggleBoundsLeft.Top, ToggleBoundsRight.Left - ToggleBoundsLeft.Right, ToggleBoundsLeft.Height);
			SetToggleButtonBounds();
			TextLeftBounds = new Rectangle(0, 0, textWidth, base.Height);
			TextRightBounds = new Rectangle(base.Width - textWidth, 0, textWidth, base.Height);
		}

		private void SetToggleButtonBounds()
		{
			ToggleDotBounds = ((State == ToggleState.Option1) ? new Rectangle(ToggleBounds.X + TogglePadding, ToggleBounds.Y + TogglePadding, ToggleDotSize.X, ToggleDotSize.Y) : ((State == ToggleState.Option2) ? new Rectangle(ToggleBounds.X + ToggleSize.X - ToggleDotSize.X - TogglePadding, ToggleBounds.Y + TogglePadding, ToggleDotSize.X, ToggleDotSize.Y) : new Rectangle(ToggleBounds.Center.X - ToggleSize.X / 2, ToggleBounds.Y + TogglePadding, ToggleDotSize.X, ToggleDotSize.Y)));
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			State = ((State != ToggleState.Option1) ? ToggleState.Option1 : ToggleState.Option2);
			SetToggleButtonBounds();
		}
	}
}
