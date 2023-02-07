using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Res;
using Kenedia.Modules.Core.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class RollingChoya : Control
	{
		private readonly TexturesService _textureService;

		private AsyncTexture2D _choyaTexture;

		private double _start;

		private int _xOffset;

		public int Steps { get; set; } = 360;


		public int TravelDistance { get; set; } = 4;


		public bool CaptureInput { get; set; } = true;


		public Color TextureColor { get; set; } = Color.get_White();


		public AsyncTexture2D ChoyaTexture
		{
			get
			{
				return _choyaTexture;
			}
			set
			{
				_choyaTexture = value;
			}
		}

		public event EventHandler ChoyaLeftBounds;

		public RollingChoya(TexturesService textureManager)
			: this()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			_textureService = textureManager;
			_choyaTexture = AsyncTexture2D.op_Implicit(_textureService.GetTexture(textures_common.RollingChoya, "RollingChoya"));
		}

		protected override CaptureType CapturesInput()
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (CaptureInput)
			{
				return ((Control)this).CapturesInput();
			}
			return (CaptureType)0;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			float rotation = (float)((GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds - _start) / (double)Steps);
			int size = Math.Min(((Control)this).get_Width(), ((Control)this).get_Height());
			int choyaSize = Math.Min(_choyaTexture.get_Bounds().Width, _choyaTexture.get_Bounds().Height);
			_xOffset += TravelDistance;
			if (_xOffset >= ((Control)this).get_Width() + choyaSize / 4)
			{
				_xOffset = -choyaSize / 5;
			}
			Rectangle choyaRect = default(Rectangle);
			((Rectangle)(ref choyaRect))._002Ector(new Point(_xOffset, ((Control)this).get_Height() / 2), new Point(size));
			if (_choyaTexture != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_choyaTexture), choyaRect, (Rectangle?)_choyaTexture.get_Bounds(), TextureColor, rotation, new Vector2((float)(choyaSize / 2)), (SpriteEffects)0);
			}
			if (!((Rectangle)(ref bounds)).Contains(((Rectangle)(ref choyaRect)).get_Location()))
			{
				this.ChoyaLeftBounds?.Invoke(this, null);
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
