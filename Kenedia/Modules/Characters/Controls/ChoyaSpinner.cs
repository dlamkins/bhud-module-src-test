using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Kenedia.Modules.Characters.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Characters.Controls
{
	public class ChoyaSpinner : Control
	{
		private readonly TextureManager _textureManager;

		private readonly AsyncTexture2D _choyaTexture;

		private readonly int _choyaSize;

		private double _start;

		private double _lastTick;

		private int _xOffset;

		private float _rotation;

		public ChoyaSpinner(TextureManager textureManager)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			_textureManager = textureManager;
			_choyaTexture = (AsyncTexture2D)_textureManager.GetControlTexture(TextureManager.ControlTextures.Choya);
			_choyaSize = Math.Min(_choyaTexture.Bounds.Width, _choyaTexture.Bounds.Height);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			double now = GameService.Overlay.CurrentGameTime.get_TotalGameTime().TotalMilliseconds;
			double duration = now - _start;
			_rotation = (float)(duration / 0.75 / 360.0);
			if (now - _lastTick > 18.0)
			{
				_lastTick = now;
				_xOffset += 5;
				if (_xOffset >= base.Width + _choyaSize / 4)
				{
					_xOffset = -_choyaSize / 5;
				}
			}
			int size = Math.Min(base.Width, base.Height);
			if (_choyaTexture != null)
			{
				spriteBatch.DrawOnCtrl(this, _choyaTexture, new Rectangle(new Point(_xOffset, base.Height / 2), new Point(size)), _choyaTexture.Bounds, Color.get_White(), _rotation, new Vector2((float)(_choyaSize / 2)), (SpriteEffects)0);
			}
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			_start = GameService.Overlay.CurrentGameTime.get_TotalGameTime().TotalMilliseconds;
		}
	}
}
