using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace BhModule.WebPeeper.Window
{
	public class WarningContent : Control
	{
		private string _line1 = "You are using an OUTDATED Chromium Version.";

		private string _line2 = "DO NOT Browse Untrusted or Security-Sensitive Websites.";

		private string _line3 = "You could BE ATTACKED through known Vulnerabilities.";

		private BitmapFont _fontSize = Control.get_Content().get_DefaultFont32();

		private Rectangle _line1Rect = Rectangle.get_Empty();

		private Rectangle _line2Rect = Rectangle.get_Empty();

		private Rectangle _line3Rect = Rectangle.get_Empty();

		private const int _gap = 10;

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnResized(e);
			_fontSize = ((((Control)this).get_Width() > 300) ? Control.get_Content().get_DefaultFont32() : Control.get_Content().get_DefaultFont18());
			SetLineRect(ref _line1, ref _line1Rect, Rectangle.get_Empty());
			SetLineRect(ref _line2, ref _line2Rect, _line1Rect);
			SetLineRect(ref _line3, ref _line3Rect, _line2Rect);
			if (((Control)this).get_Height() < ((Rectangle)(ref _line3Rect)).get_Bottom())
			{
				((Control)this).set_Height(((Rectangle)(ref _line3Rect)).get_Bottom());
			}
			else if (((Control)this).get_Height() > ((Rectangle)(ref _line3Rect)).get_Bottom())
			{
				int num = ((Control)this).get_Height() / 3 - ((Rectangle)(ref _line3Rect)).get_Bottom() / 2;
				if (num > 0)
				{
					_line1Rect.Y += num;
					_line2Rect.Y += num;
					_line3Rect.Y += num;
				}
			}
		}

		public override void DoUpdate(GameTime gameTime)
		{
			((Control)this).DoUpdate(gameTime);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _line1, _fontSize, _line1Rect, Color.get_Red(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _line2, _fontSize, _line2Rect, Color.get_Red(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _line3, _fontSize, _line3Rect, Color.get_Red(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
		}

		private void SetLineRect(ref string text, ref Rectangle rect, Rectangle prevTextRect)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			rect.Width = ((Control)this).get_Width();
			rect.Y = 10 + ((Rectangle)(ref prevTextRect)).get_Bottom();
			text = DrawUtil.WrapText(_fontSize, text.Replace("\n", ""), (float)(rect.Width - 20)).Trim();
			Size2 val = _fontSize.MeasureString(text);
			rect.Height = (int)val.Height;
			if (!text.Contains("\n"))
			{
				rect.X = ((Control)this).get_Width() / 2 - (int)(val.Width / 2f);
			}
			else
			{
				rect.X = 0;
			}
		}

		public WarningContent()
			: this()
		{
		}//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)

	}
}
