using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Res;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class ButtonImage : ImageButton
	{
		private readonly List<(Rectangle bounds, float alpha)> _frameBounds = new List<(Rectangle, float)>();

		private Rectangle _textureBounds;

		private Point? _textureSize;

		private AsyncTexture2D _buttonImage;

		private AsyncTexture2D _hoveredButton;

		public Point? TextureSize
		{
			get
			{
				return _textureSize;
			}
			set
			{
				Common.SetProperty(ref _textureSize, value, new Action(RecalculateLayout));
			}
		}

		public ButtonImage()
		{
			_buttonImage = (AsyncTexture2D)TexturesService.GetTextureFromRef(textures_common.ImageButtonBackground, "ImageButtonBackground");
			_hoveredButton = (AsyncTexture2D)TexturesService.GetTextureFromRef(textures_common.ImageButtonBackground_Hovered, "ImageButtonBackground_Hovered");
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (_buttonImage != null)
			{
				spriteBatch.DrawOnCtrl(this, (base.MouseOver && base.Enabled) ? _hoveredButton : _buttonImage, bounds, _buttonImage.Bounds, Color.White);
			}
			if (base.MouseOver && base.Enabled)
			{
				for (int i = 0; i < _frameBounds.Count; i++)
				{
					Rectangle b = _frameBounds[i].bounds;
					float alpha = _frameBounds[i].alpha;
					spriteBatch.DrawFrame(this, b, ContentService.Colors.ColonialWhite * alpha);
				}
			}
			base.Paint(spriteBatch, _textureBounds);
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			Point size = TextureSize ?? base.Size;
			Point padding = new Point((base.Width - size.X) / 2, (base.Height - size.Y) / 2);
			int xOffset = (int)((double)base.Width * 0.15);
			int yOffset = (int)((double)base.Height * 0.15);
			_textureBounds = new Rectangle(xOffset / 2 + padding.X, yOffset / 2 + padding.Y, size.X - xOffset, size.Y - yOffset);
			_frameBounds.Clear();
			int frameWidth = Math.Max(2, (int)((double)base.Width * 0.05));
			float stepSize = 0.75f / (float)(frameWidth - 1);
			for (int i = 0; i < frameWidth; i++)
			{
				_frameBounds.Add((new Rectangle(i, i, base.Width - i * 2, base.Height - i * 2), (float)i * stepSize));
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			_hoveredButton = null;
			_buttonImage = null;
		}
	}
}
