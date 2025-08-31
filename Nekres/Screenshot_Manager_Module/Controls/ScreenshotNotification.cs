using System;
using System.IO;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using Nekres.Screenshot_Manager.UI.Controls;

namespace Nekres.Screenshot_Manager_Module.Controls
{
	public class ScreenshotNotification : Panel
	{
		private static int _visibleNotifications;

		private static readonly BitmapFont _font = GameService.Content.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size24, ContentService.FontStyle.Regular);

		private static readonly BitmapFont _titleFont = GameService.Content.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size22, ContentService.FontStyle.Regular);

		private readonly ThumbnailBase _thumbnail;

		private readonly string _message;

		private ScreenshotNotification(AsyncTexture2D texture, string fileName, string message)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			_thumbnail = new ThumbnailBase(texture, fileName)
			{
				Parent = this,
				Location = new Point(0, 36),
				Size = new Point(256, 144)
			};
			_message = message;
			base.Opacity = 0f;
			base.Size = new Point(256, 180);
			base.Location = new Point(60, 60 + base.Height * _visibleNotifications);
			base.ShowBorder = true;
			base.ShowTint = true;
		}

		protected override CaptureType CapturesInput()
		{
			return CaptureType.Mouse;
		}

		public override void RecalculateLayout()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			base.Location = new Point(60, 60 + base.Height * _visibleNotifications);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bound)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bound);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, bound, Color.get_Black() * 0.4f);
			spriteBatch.DrawStringOnCtrl(this, _message, _font, new Rectangle(0, 0, base.Width, 36), Color.get_White(), wrap: false, stroke: true, 2, HorizontalAlignment.Center);
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bound)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bound);
			spriteBatch.DrawStringOnCtrl(this, "“" + Path.GetFileNameWithoutExtension(_thumbnail.FileName) + "”", _titleFont, new Rectangle(0, 38, base.Width, 36), Color.get_White(), wrap: false, stroke: true, 1, HorizontalAlignment.Center);
		}

		private void Show(float duration)
		{
			Control.Content.PlaySoundEffectByName("audio/color-change");
			Control.Animation.Tweener.Tween(this, new
			{
				Opacity = 1f
			}, 0.2f).Repeat(1).RepeatDelay(duration)
				.Reflect()
				.OnComplete(Dispose);
		}

		public static void ShowNotification(AsyncTexture2D texture, string fileName, string message, float duration, Action clickCallback)
		{
			ScreenshotNotification screenshotNotification = new ScreenshotNotification(texture, fileName, message);
			screenshotNotification.Parent = Control.Graphics.SpriteScreen;
			screenshotNotification.Click += delegate
			{
				clickCallback();
			};
			screenshotNotification.Show(duration);
			_visibleNotifications++;
		}

		protected override void DisposeControl()
		{
			_visibleNotifications--;
			base.DisposeControl();
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			Dispose();
		}
	}
}
