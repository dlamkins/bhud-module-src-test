using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Res;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class ButtonImage : ImageButton
	{
		private readonly AsyncTexture2D _buttonImage;

		private readonly AsyncTexture2D _hoveredButton;

		private readonly List<(Rectangle bounds, float alpha)> _frameBounds = new List<(Rectangle, float)>();

		private Rectangle _textureBounds;

		private Point? _textureSize;

		public Point? TextureSize
		{
			get
			{
				return _textureSize;
			}
			set
			{
				Common.SetProperty(ref _textureSize, value, ((Control)this).RecalculateLayout);
			}
		}

		public ButtonImage()
		{
			_buttonImage = AsyncTexture2D.op_Implicit(textures_common.ImageButtonBackground.CreateTexture2D());
			_hoveredButton = AsyncTexture2D.op_Implicit(textures_common.ImageButtonBackground_Hovered.CreateTexture2D());
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			if (_buttonImage != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(((Control)this).get_MouseOver() ? _hoveredButton : _buttonImage), bounds, (Rectangle?)_buttonImage.get_Bounds(), Color.get_White());
			}
			if (((Control)this).get_MouseOver())
			{
				for (int i = 0; i < _frameBounds.Count; i++)
				{
					Rectangle b = _frameBounds[i].bounds;
					float alpha = _frameBounds[i].alpha;
					spriteBatch.DrawFrame((Control)(object)this, b, Colors.ColonialWhite * alpha);
				}
			}
			base.Paint(spriteBatch, _textureBounds);
		}

		public override void RecalculateLayout()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			Point size = (Point)(((_003F?)TextureSize) ?? ((Control)this).get_Size());
			Point padding = default(Point);
			((Point)(ref padding))._002Ector((((Control)this).get_Width() - size.X) / 2, (((Control)this).get_Height() - size.Y) / 2);
			int xOffset = (int)((double)((Control)this).get_Width() * 0.15);
			int yOffset = (int)((double)((Control)this).get_Height() * 0.15);
			_textureBounds = new Rectangle(xOffset / 2 + padding.X, yOffset / 2 + padding.Y, size.X - xOffset, size.Y - yOffset);
			_frameBounds.Clear();
			int frameWidth = Math.Max(2, (int)((double)((Control)this).get_Width() * 0.05));
			float stepSize = 0.75f / (float)(frameWidth - 1);
			for (int i = 0; i < frameWidth; i++)
			{
				_frameBounds.Add((new Rectangle(i, i, ((Control)this).get_Width() - i * 2, ((Control)this).get_Height() - i * 2), (float)i * stepSize));
			}
		}
	}
}
