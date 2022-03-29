using System;
using System.IO;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using Nekres.Screenshot_Manager.UI.Controls;

namespace Nekres.Screenshot_Manager_Module.Controls
{
	public class ScreenshotNotification : Panel
	{
		private static int _visibleNotifications;

		private static readonly BitmapFont Font = GameService.Content.GetFont((FontFace)0, (FontSize)24, (FontStyle)0);

		private static readonly BitmapFont TitleFont = GameService.Content.GetFont((FontFace)0, (FontSize)22, (FontStyle)0);

		private readonly ThumbnailBase _thumbnail;

		private readonly string _message;

		private ScreenshotNotification(AsyncTexture2D texture, string fileName, string message)
			: this()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			ThumbnailBase thumbnailBase = new ThumbnailBase(texture, fileName);
			((Control)thumbnailBase).set_Parent((Container)(object)this);
			((Control)thumbnailBase).set_Location(new Point(0, 36));
			((Control)thumbnailBase).set_Size(new Point(256, 144));
			_thumbnail = thumbnailBase;
			_message = message;
			((Control)this).set_Opacity(0f);
			((Control)this).set_Size(new Point(256, 180));
			((Control)this).set_Location(new Point(60, 60 + ((Control)this).get_Height() * _visibleNotifications));
			((Panel)this).set_ShowBorder(true);
			((Panel)this).set_ShowTint(true);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)4;
		}

		public override void RecalculateLayout()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Location(new Point(60, 60 + ((Control)this).get_Height() * _visibleNotifications));
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bound)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			((Panel)this).PaintBeforeChildren(spriteBatch, bound);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bound, Color.get_Black() * 0.4f);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _message, Font, new Rectangle(0, 0, ((Control)this).get_Width(), 36), Color.get_White(), false, true, 2, (HorizontalAlignment)1, (VerticalAlignment)1);
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bound)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).PaintAfterChildren(spriteBatch, bound);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "“" + Path.GetFileNameWithoutExtension(_thumbnail.FileName) + "”", TitleFont, new Rectangle(0, 38, ((Control)this).get_Width(), 36), Color.get_White(), false, true, 1, (HorizontalAlignment)1, (VerticalAlignment)1);
		}

		private void Show(float duration)
		{
			Control.get_Content().PlaySoundEffectByName("audio/color-change");
			((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<ScreenshotNotification>(this, (object)new
			{
				Opacity = 1f
			}, 0.2f, 0f, true).Repeat(1).RepeatDelay(duration)
				.Reflect()
				.OnComplete((Action)((Control)this).Dispose);
		}

		public static void ShowNotification(AsyncTexture2D texture, string fileName, string message, float duration, Action clickCallback)
		{
			ScreenshotNotification screenshotNotification = new ScreenshotNotification(texture, fileName, message);
			((Control)screenshotNotification).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
			((Control)screenshotNotification).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				clickCallback();
			});
			screenshotNotification.Show(duration);
			_visibleNotifications++;
		}

		protected override void DisposeControl()
		{
			_visibleNotifications--;
			((Panel)this).DisposeControl();
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((Panel)this).OnClick(e);
			((Control)this).Dispose();
		}
	}
}
