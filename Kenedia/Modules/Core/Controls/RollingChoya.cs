using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class RollingChoya : Control
	{
		private double _start;

		private int _xOffset;

		public int Steps { get; set; } = 360;


		public int TravelDistance { get; set; } = 4;


		public bool CaptureInput { get; set; } = true;


		public Color TextureColor { get; set; } = Color.get_White();


		public bool CanMove { get; set; } = true;


		public AsyncTexture2D ChoyaTexture { get; set; }

		public event EventHandler ChoyaLeftBounds;

		public RollingChoya()
			: this()
		{
		}//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)


		protected override CaptureType CapturesInput()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (!CaptureInput)
			{
				return (CaptureType)0;
			}
			return ((Control)this).CapturesInput();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			if (ChoyaTexture != null)
			{
				float rotation = (float)((GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds - _start) / (double)Steps);
				int size = Math.Min(((Control)this).get_Width(), ((Control)this).get_Height());
				int choyaSize = Math.Min(ChoyaTexture.get_Bounds().Width, ChoyaTexture.get_Bounds().Height);
				_xOffset += (CanMove ? TravelDistance : 0);
				if ((double)_xOffset >= (double)((Control)this).get_Width() + ((double)(choyaSize / 2) - (double)choyaSize * 0.15))
				{
					_xOffset = -(int)((double)(choyaSize / 2) - (double)choyaSize * 0.15);
				}
				Rectangle choyaRect = default(Rectangle);
				((Rectangle)(ref choyaRect))._002Ector(new Point(CanMove ? _xOffset : (((Control)this).get_Width() / 2), ((Control)this).get_Height() / 2), new Point(size));
				if (ChoyaTexture != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(ChoyaTexture), choyaRect, (Rectangle?)ChoyaTexture.get_Bounds(), TextureColor, rotation, new Vector2((float)(choyaSize / 2)), (SpriteEffects)0);
				}
				if (!((Rectangle)(ref bounds)).Contains(((Rectangle)(ref choyaRect)).get_Location()))
				{
					this.ChoyaLeftBounds?.Invoke(this, null);
				}
			}
		}

		protected override void OnShown(EventArgs e)
		{
			((Control)this).OnShown(e);
			_start = GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds;
		}

		protected override void DisposeControl()
		{
			((Control)this).DisposeControl();
		}
	}
}
