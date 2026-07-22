using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Characters.Controls
{
	public class TesseractFailedNotification : BaseNotification
	{
		private Rectangle _textRectangle;

		private DetailedTexture _settingsCog = new DetailedTexture(155052, 157110);

		private DetailedTexture _dismiss = new DetailedTexture(156012, 156011)
		{
			TextureRegion = new Rectangle(4, 4, 24, 24)
		};

		public Action ClickAction { get; set; }

		public string PathToEngine { get; internal set; }

		public TesseractFailedNotification()
		{
			base.NotificationType = NotificationType.Tesseract;
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			int height = GameService.Content.DefaultFont14.LineHeight + 4;
			_dismiss.Bounds = new Rectangle(0, 0, height, height);
			_settingsCog.Bounds = new Rectangle(_dismiss.Bounds.Right + 2, 0, height, height);
			int width = base.Width - _settingsCog.Bounds.Right - 6;
			string wrappedText = TextUtil.WrapText(GameService.Content.DefaultFont14, string.Format(strings.TesseractFailedNotification, PathToEngine), width);
			_textRectangle = new Rectangle(height: (int)GameService.Content.DefaultFont14.GetStringRectangle(wrappedText).Height, x: _settingsCog.Bounds.Right + 6, y: 0, width: width);
			base.Height = Math.Max(height, _textRectangle.Height);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			string txt = string.Empty;
			_dismiss.Draw(this, spriteBatch, base.RelativeMousePosition);
			_settingsCog.Draw(this, spriteBatch, base.RelativeMousePosition);
			spriteBatch.DrawStringOnCtrl(this, string.Format(strings.TesseractFailedNotification, PathToEngine), GameService.Content.DefaultFont14, _textRectangle, Color.White, wrap: true, HorizontalAlignment.Left, VerticalAlignment.Top);
			base.BasicTooltipText = txt;
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			if (_settingsCog.Hovered)
			{
				ClickAction?.Invoke();
			}
			if (_dismiss.Hovered)
			{
				Container parent = base.Parent;
				Dispose();
				parent?.Invalidate();
			}
		}
	}
}
