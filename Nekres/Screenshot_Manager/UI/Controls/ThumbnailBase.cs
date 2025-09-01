using System;
using System.IO;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Nekres.Screenshot_Manager.UI.Controls
{
	public class ThumbnailBase : Container
	{
		private static BitmapFont _font = GameService.Content.GetFont((FontFace)0, (FontSize)36, (FontStyle)0);

		private AsyncTexture2D _texture;

		private string _fileName;

		public AsyncTexture2D Texture
		{
			get
			{
				return _texture;
			}
			set
			{
				AsyncTexture2D texture = _texture;
				if (texture != null)
				{
					texture.Dispose();
				}
				((Control)this).SetProperty<AsyncTexture2D>(ref _texture, value, false, "Texture");
			}
		}

		public string FileName
		{
			get
			{
				return _fileName;
			}
			set
			{
				((Control)this).SetProperty<string>(ref _fileName, value ?? string.Empty, false, "FileName");
			}
		}

		public ThumbnailBase(AsyncTexture2D texture, string fileName)
			: this()
		{
			_texture = texture;
			FileName = fileName;
		}

		protected override void DisposeControl()
		{
			AsyncTexture2D texture = _texture;
			if (texture != null)
			{
				texture.Dispose();
			}
			((Container)this).DisposeControl();
		}

		public bool TryLoadImage(out Texture2D texture)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				using (FileStream stream = File.OpenRead(FileName))
				{
					GraphicsDeviceContext ctx = GameService.Graphics.LendGraphicsDeviceContext();
					try
					{
						texture = Texture2D.FromStream(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice(), (Stream)stream);
					}
					finally
					{
						((GraphicsDeviceContext)(ref ctx)).Dispose();
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				ScreenshotManagerModule.Logger.Warn(ex, "Failed to copy image to clipboard: " + FileName);
				texture = null;
				return false;
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, Color.get_Black());
			if (_texture.get_HasTexture())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _texture.get_Texture(), _texture.get_Texture().get_Bounds().ScaleTo(bounds, 1f, center: true));
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Path.GetExtension(FileName).TrimStart('.').ToUpperInvariant(), _font, new Rectangle(bounds.X + 10, bounds.Y + 3, bounds.Width - 20, bounds.Height - 6), new Color(Color.get_Gray(), 0.5f), false, false, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			}
			else
			{
				LoadingSpinnerUtil.DrawLoadingSpinner((Control)(object)this, spriteBatch, bounds);
			}
		}
	}
}
