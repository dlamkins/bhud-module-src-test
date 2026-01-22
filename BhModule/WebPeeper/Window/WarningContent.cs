using BhModule.WebPeeper.Strings;
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
		private readonly string _originText = BhModule.WebPeeper.Strings.UIService.Warning;

		private string _text = "";

		private BitmapFont _fontSize = Control.get_Content().get_DefaultFont32();

		private Rectangle _textRect = Rectangle.get_Empty();

		private const int _gap = 10;

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnResized(e);
			_fontSize = ((((Control)this).get_Width() > 500) ? Control.get_Content().get_DefaultFont32() : Control.get_Content().get_DefaultFont18());
			_text = CalculateText(_originText, ref _textRect, Rectangle.get_Empty());
			int num = ((Rectangle)(ref _textRect)).get_Bottom() + 30;
			if (((Control)this).get_Height() < num)
			{
				((Control)this).set_Height(num);
			}
			else if (((Control)this).get_Height() > num)
			{
				int num2 = ((Control)this).get_Height() / 3 - ((Rectangle)(ref _textRect)).get_Bottom() / 2;
				if (num2 > 0)
				{
					_textRect.Y += num2;
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
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _text, _fontSize, _textRect, Color.get_Red(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
		}

		private string CalculateText(string text, ref Rectangle rect, Rectangle prevTextRect)
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			int num = ((_fontSize == Control.get_Content().get_DefaultFont32()) ? 80 : 0);
			rect.Width = ((Control)this).get_Width();
			rect.Y = 10 + ((Rectangle)(ref prevTextRect)).get_Bottom();
			string text2 = DrawUtil.WrapText(_fontSize, text, (float)(rect.Width - num)).Trim();
			Size2 val = _fontSize.MeasureString(text);
			Size2 val2 = _fontSize.MeasureString(text2);
			rect.Height = (int)val2.Height;
			if (val == val2)
			{
				rect.X = ((Control)this).get_Width() / 2 - (int)(val2.Width / 2f);
			}
			else
			{
				rect.X = 0;
			}
			return text2;
		}

		public WarningContent()
			: this()
		{
		}//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)

	}
}
