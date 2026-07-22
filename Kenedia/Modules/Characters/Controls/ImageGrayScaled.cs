using System.Runtime.CompilerServices;
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

		private Rectangle _textureRectangle = Rectangle.Empty;

		private Texture2D _grayScaleTexture;

		public bool UseGrayScale { get; set; } = true;


		public AsyncTexture2D Texture
		{
			[CompilerGenerated]
			get
			{
				return _003CTexture_003Ek__BackingField;
			}
			set
			{
				AsyncTexture2D temp = _003CTexture_003Ek__BackingField;
				if (Common.SetProperty(ref _003CTexture_003Ek__BackingField, value))
				{
					if (temp != null)
					{
						temp.TextureSwapped -= Texture_TextureSwapped;
					}
					if (_003CTexture_003Ek__BackingField != null)
					{
						_003CTexture_003Ek__BackingField.TextureSwapped += Texture_TextureSwapped;
					}
					if (_003CTexture_003Ek__BackingField != null)
					{
						_grayScaleTexture = _003CTexture_003Ek__BackingField.Texture.ToGrayScaledPalettable();
					}
				}
			}
		}

		public Rectangle SizeRectangle { get; set; }

		public Rectangle TextureRectangle
		{
			get
			{
				return _textureRectangle;
			}
			set
			{
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
			ColorHovered = s_defaultColorHovered;
			ColorActive = s_defaultColorActive;
			ColorInActive = s_defaultColorInActive;
		}

		public Texture2D ToGrayScaledPalettable(Texture2D original)
		{
			Color[] colors = new Color[original.Width * original.Height];
			original.GetData(colors);
			Color[] destColors = new Color[original.Width * original.Height];
			Texture2D newTexture;
			using (GraphicsDeviceContext device = GameService.Graphics.LendGraphicsDeviceContext())
			{
				newTexture = new Texture2D(device.GraphicsDevice, original.Width, original.Height);
			}
			for (int i = 0; i < original.Width; i++)
			{
				for (int j = 0; j < original.Height; j++)
				{
					int index = i + j * original.Width;
					Color originalColor = colors[index];
					float maxval = 1.79f;
					float grayScale = (float)(int)originalColor.R / 255f * 0.3f + (float)(int)originalColor.G / 255f * 0.59f + (float)(int)originalColor.B / 255f * 0.11f + (float)(int)originalColor.A / 255f * 0.79f;
					grayScale /= maxval;
					destColors[index] = new Color(grayScale, grayScale, grayScale, (int)originalColor.A);
				}
			}
			newTexture.SetData(destColors);
			return newTexture;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (Texture != null)
			{
				spriteBatch.DrawOnCtrl(this, (UseGrayScale && !Active && !base.MouseOver) ? _grayScaleTexture : ((Texture2D)Texture), (SizeRectangle != Rectangle.Empty) ? SizeRectangle : bounds, (_textureRectangle == Rectangle.Empty) ? Texture.Bounds : _textureRectangle, Active ? ColorActive : (base.MouseOver ? ColorHovered : (ColorInActive * (UseGrayScale ? 0.5f : Alpha))), 0f, default(Vector2));
			}
		}

		private void Texture_TextureSwapped(object sender, ValueChangedEventArgs<Texture2D> e)
		{
			_grayScaleTexture = Texture.Texture.ToGrayScaledPalettable();
			Texture.TextureSwapped -= Texture_TextureSwapped;
		}
	}
}
