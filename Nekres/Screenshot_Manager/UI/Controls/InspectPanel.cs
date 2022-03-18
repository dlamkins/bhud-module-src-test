using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Nekres.Screenshot_Manager.UI.Controls
{
	internal sealed class InspectPanel : Panel
	{
		private static readonly BitmapFont Font = GameService.Content.GetFont((FontFace)0, (FontSize)36, (FontStyle)0);

		private readonly AsyncTexture2D _texture;

		private Rectangle _textureBoundsFitted;

		private readonly string _label;

		public InspectPanel(AsyncTexture2D texture, string label)
			: this()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			_texture = texture;
			_label = label;
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Size(new Point(1, 1));
			((Control)this).set_Location(new Point(((Control)((Control)this).get_Parent()).get_Width() / 2, ((Control)((Control)this).get_Parent()).get_Height() / 2));
			((Control)this).set_BackgroundColor(Color.get_Black());
			((Control)this).set_ZIndex(9999);
			((Panel)this).set_ShowTint(true);
			_texture.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)OnTextureSwapped);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((Panel)this).OnClick(e);
			((Control)this).Dispose();
		}

		protected override void DisposeControl()
		{
			_texture.Dispose();
			((Panel)this).DisposeControl();
		}

		public void OnTextureSwapped(object o, EventArgs e)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(_texture.get_Texture().get_Width() + 10, _texture.get_Texture().get_Height() + 10));
			((Control)this).set_Location(new Point((((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - ((Control)this).get_Width()) / 2, (((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - ((Control)this).get_Height()) / 2));
			_textureBoundsFitted = _texture.get_Texture().get_Bounds().Fit(((Control)this).get_LocalBounds());
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			((Panel)this).PaintBeforeChildren(spriteBatch, bounds);
			if (_texture.get_HasTexture())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _texture.get_Texture(), _textureBoundsFitted);
			}
			else
			{
				LoadingSpinnerUtil.DrawLoadingSpinner((Control)(object)this, spriteBatch, bounds);
			}
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "“" + _label + "”", Font, new Rectangle(0, 36, ((Control)this).get_Width(), 36), Color.get_White(), false, true, 3, (HorizontalAlignment)1, (VerticalAlignment)1);
		}
	}
}
