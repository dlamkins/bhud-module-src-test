using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Nekres.Screenshot_Manager.UI.Controls
{
	internal sealed class InspectPanel : Panel
	{
		private const int BORDER_WIDTH = 10;

		private static readonly BitmapFont _font = GameService.Content.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size36, ContentService.FontStyle.Regular);

		private readonly AsyncTexture2D _texture;

		private Rectangle _textureBounds;

		private Rectangle _borderBounds;

		private readonly string _label;

		public InspectPanel(AsyncTexture2D texture, string label)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			_texture = texture;
			_label = label;
			base.Parent = GameService.Graphics.SpriteScreen;
			base.Size = GameService.Graphics.SpriteScreen.Size;
			ZIndex = 120;
			base.ShowTint = true;
			_texture.TextureSwapped += OnTextureSwapped;
			GameService.Graphics.SpriteScreen.Resized += OnSpriteScreenResized;
		}

		private void OnSpriteScreenResized(object sender, ResizedEventArgs e)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			base.Size = e.CurrentSize;
			if (_texture.HasTexture)
			{
				_textureBounds = _texture.Texture.get_Bounds().ScaleTo(base.LocalBounds, 0.8f, center: true);
				_borderBounds = new Rectangle(_textureBounds.X, _textureBounds.Y, _textureBounds.Width + 10, _textureBounds.Height + 10);
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			Dispose();
		}

		protected override void DisposeControl()
		{
			GameService.Graphics.SpriteScreen.Resized -= OnSpriteScreenResized;
			_texture.Dispose();
			base.DisposeControl();
		}

		public void OnTextureSwapped(object o, EventArgs e)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			_textureBounds = _texture.Texture.get_Bounds().ScaleTo(base.LocalBounds, 0.8f, center: true);
			_borderBounds = new Rectangle(_textureBounds.X, _textureBounds.Y, _textureBounds.Width + 10, _textureBounds.Height + 10);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, bounds, Color.get_Black() * 0.5f);
			if (_texture.HasTexture)
			{
				spriteBatch.DrawOnCtrl(this, _texture.Texture, _textureBounds);
				spriteBatch.DrawRectangleOnCtrl(this, _borderBounds, 10, Color.get_Black());
			}
			else
			{
				LoadingSpinnerUtil.DrawLoadingSpinner(this, spriteBatch, bounds);
			}
			spriteBatch.DrawStringOnCtrl(this, "“" + _label + "”", _font, new Rectangle(0, 36, base.Width, 36), Color.get_White(), wrap: false, stroke: true, 3, HorizontalAlignment.Center);
		}
	}
}
