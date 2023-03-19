using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Characters.Controls
{
	public class ImageGrayScaled : Control
	{
		private static Color s_defaultColorHovered = new Color(255, 255, 255, 255);

		private static Color s_defaultColorActive = new Color(200, 200, 200, 200);

		private static Color s_defaultColorInActive = new Color(175, 175, 175, 255);

		private Rectangle _textureRectangle = Rectangle.get_Empty();

		private Texture2D _grayScaleTexture;

		private AsyncTexture2D _texture;

		public bool UseGrayScale { get; set; } = true;


		public AsyncTexture2D Texture
		{
			get
			{
				return _texture;
			}
			set
			{
				AsyncTexture2D temp = _texture;
				if (Common.SetProperty(ref _texture, value))
				{
					if (temp != null)
					{
						temp.remove_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)Texture_TextureSwapped);
					}
					if (_texture != null)
					{
						_texture.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)Texture_TextureSwapped);
					}
					if (_texture != null)
					{
						_grayScaleTexture = _texture.get_Texture().ToGrayScaledPalettable();
					}
				}
			}
		}

		public Rectangle SizeRectangle { get; set; }

		public Rectangle TextureRectangle
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _textureRectangle;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				_textureRectangle = value;
			}
		}

		public bool Active { get; set; }

		public Color ColorHovered { get; set; } = new Color(255, 255, 255, 255);


		public Color ColorActive { get; set; } = new Color(200, 200, 200, 200);


		public Color ColorInActive { get; set; } = new Color(175, 175, 175, 255);


		public float Alpha { get; set; } = 0.25f;


		public void ResetColors()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			ColorHovered = s_defaultColorHovered;
			ColorActive = s_defaultColorActive;
			ColorInActive = s_defaultColorInActive;
		}

		public Texture2D ToGrayScaledPalettable(Texture2D original)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			Color[] colors = (Color[])(object)new Color[original.get_Width() * original.get_Height()];
			original.GetData<Color>(colors);
			Color[] destColors = (Color[])(object)new Color[original.get_Width() * original.get_Height()];
			GraphicsDeviceContext device = GameService.Graphics.LendGraphicsDeviceContext();
			Texture2D newTexture;
			try
			{
				newTexture = new Texture2D(((GraphicsDeviceContext)(ref device)).get_GraphicsDevice(), original.get_Width(), original.get_Height());
			}
			finally
			{
				((GraphicsDeviceContext)(ref device)).Dispose();
			}
			for (int i = 0; i < original.get_Width(); i++)
			{
				for (int j = 0; j < original.get_Height(); j++)
				{
					int index = i + j * original.get_Width();
					Color originalColor = colors[index];
					float maxval = 1.79f;
					float grayScale = (float)(int)((Color)(ref originalColor)).get_R() / 255f * 0.3f + (float)(int)((Color)(ref originalColor)).get_G() / 255f * 0.59f + (float)(int)((Color)(ref originalColor)).get_B() / 255f * 0.11f + (float)(int)((Color)(ref originalColor)).get_A() / 255f * 0.79f;
					grayScale /= maxval;
					destColors[index] = new Color(grayScale, grayScale, grayScale, (float)(int)((Color)(ref originalColor)).get_A());
				}
			}
			newTexture.SetData<Color>(destColors);
			return newTexture;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			if (_texture != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, (UseGrayScale && !Active && !((Control)this).get_MouseOver()) ? _grayScaleTexture : AsyncTexture2D.op_Implicit(_texture), (SizeRectangle != Rectangle.get_Empty()) ? SizeRectangle : bounds, (Rectangle?)((_textureRectangle == Rectangle.get_Empty()) ? _texture.get_Bounds() : _textureRectangle), Active ? ColorActive : (((Control)this).get_MouseOver() ? ColorHovered : (ColorInActive * (UseGrayScale ? 0.5f : Alpha))), 0f, default(Vector2), (SpriteEffects)0);
			}
		}

		private void Texture_TextureSwapped(object sender, ValueChangedEventArgs<Texture2D> e)
		{
			_grayScaleTexture = _texture.get_Texture().ToGrayScaledPalettable();
			_texture.remove_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)Texture_TextureSwapped);
		}

		public ImageGrayScaled()
			: this()
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)

	}
}
