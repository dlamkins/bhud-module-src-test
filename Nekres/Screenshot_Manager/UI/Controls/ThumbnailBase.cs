using System.IO;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Nekres.Screenshot_Manager.UI.Controls
{
	public class ThumbnailBase : Container
	{
		private static BitmapFont _font = GameService.Content.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size36, ContentService.FontStyle.Regular);

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
				_texture?.Dispose();
				SetProperty(ref _texture, value, invalidateLayout: false, "Texture");
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
				SetProperty(ref _fileName, value ?? string.Empty, invalidateLayout: false, "FileName");
			}
		}

		public ThumbnailBase(AsyncTexture2D texture, string fileName)
		{
			_texture = texture;
			FileName = fileName;
		}

		protected override void DisposeControl()
		{
			_texture?.Dispose();
			base.DisposeControl();
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
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, bounds, Color.get_Black());
			if (_texture.HasTexture)
			{
				spriteBatch.DrawOnCtrl(this, _texture.Texture, _texture.Texture.get_Bounds().ScaleTo(bounds, 1f, center: true));
				spriteBatch.DrawStringOnCtrl(this, Path.GetExtension(FileName).TrimStart('.').ToUpperInvariant(), _font, new Rectangle(bounds.X + 10, bounds.Y + 3, bounds.Width - 20, bounds.Height - 6), new Color(Color.get_Gray(), 0.5f), wrap: false, stroke: false, 1, HorizontalAlignment.Left, VerticalAlignment.Top);
			}
			else
			{
				LoadingSpinnerUtil.DrawLoadingSpinner(this, spriteBatch, bounds);
			}
		}
	}
}
