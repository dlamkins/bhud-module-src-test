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

		private static readonly BitmapFont _font = GameService.Content.GetFont((FontFace)0, (FontSize)36, (FontStyle)0);

		private readonly AsyncTexture2D _texture;

		private Rectangle _textureBounds;

		private Rectangle _borderBounds;

		private readonly string _label;

		public InspectPanel(AsyncTexture2D texture, string label)
			: this()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			_texture = texture;
			_label = label;
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Size(((Control)GameService.Graphics.get_SpriteScreen()).get_Size());
			((Control)this).set_ZIndex(120);
			((Panel)this).set_ShowTint(true);
			_texture.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)OnTextureSwapped);
			((Control)GameService.Graphics.get_SpriteScreen()).add_Resized((EventHandler<ResizedEventArgs>)OnSpriteScreenResized);
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
			((Control)this).set_Size(e.get_CurrentSize());
			if (_texture.get_HasTexture())
			{
				_textureBounds = _texture.get_Texture().get_Bounds().ScaleTo(((Control)this).get_LocalBounds(), 0.8f, center: true);
				_borderBounds = new Rectangle(_textureBounds.X, _textureBounds.Y, _textureBounds.Width + 10, _textureBounds.Height + 10);
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((Panel)this).OnClick(e);
			((Control)this).Dispose();
		}

		protected override void DisposeControl()
		{
			((Control)GameService.Graphics.get_SpriteScreen()).remove_Resized((EventHandler<ResizedEventArgs>)OnSpriteScreenResized);
			_texture.Dispose();
			((Panel)this).DisposeControl();
		}

		public void OnTextureSwapped(object o, EventArgs e)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			_textureBounds = _texture.get_Texture().get_Bounds().ScaleTo(((Control)this).get_LocalBounds(), 0.8f, center: true);
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
			((Panel)this).PaintBeforeChildren(spriteBatch, bounds);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, Color.get_Black() * 0.5f);
			if (_texture.get_HasTexture())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _texture.get_Texture(), _textureBounds);
				spriteBatch.DrawRectangleOnCtrl((Control)(object)this, _borderBounds, 10, Color.get_Black());
			}
			else
			{
				LoadingSpinnerUtil.DrawLoadingSpinner((Control)(object)this, spriteBatch, bounds);
			}
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "“" + _label + "”", _font, new Rectangle(0, 36, ((Control)this).get_Width(), 36), Color.get_White(), false, true, 3, (HorizontalAlignment)1, (VerticalAlignment)1);
		}
	}
}
