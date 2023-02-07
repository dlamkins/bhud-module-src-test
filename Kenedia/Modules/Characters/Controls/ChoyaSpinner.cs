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

		private double _start;

		private int xOffset;

		public ChoyaSpinner(TextureManager textureManager)
			: this()
		{
			_textureManager = textureManager;
			_choyaTexture = AsyncTexture2D.op_Implicit(_textureManager.GetControlTexture(TextureManager.ControlTextures.Choya));
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			float rotation = (float)((GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds - _start) / 0.75 / 360.0);
			int size = Math.Min(((Control)this).get_Width(), ((Control)this).get_Height());
			int choyaSize = Math.Min(_choyaTexture.get_Bounds().Width, _choyaTexture.get_Bounds().Height);
			xOffset += 4;
			if (xOffset >= ((Control)this).get_Width() + choyaSize / 4)
			{
				xOffset = -choyaSize / 5;
			}
			if (_choyaTexture != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_choyaTexture), new Rectangle(new Point(xOffset, ((Control)this).get_Height() / 2), new Point(size)), (Rectangle?)_choyaTexture.get_Bounds(), Color.get_White(), rotation, new Vector2((float)(choyaSize / 2)), (SpriteEffects)0);
			}
		}

		protected override void OnShown(EventArgs e)
		{
			((Control)this).OnShown(e);
			_start = GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds;
		}
	}
}
