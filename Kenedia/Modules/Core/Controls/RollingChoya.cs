using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Core.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class RollingChoya : Control
	{
		private double _start;

		private float _xOffset;

		private float _yOffset;

		private bool _canMove = true;

		private Rectangle _movementBounds = Rectangle.get_Empty();

		private Point _startPoint = Point.get_Zero();

		private bool _choyaTargeted;

		private int ChoyaSize => Math.Min(((Control)this).get_Width(), ((Control)this).get_Height());

		public int Steps { get; set; } = 360;


		public Vector2 TravelDistance { get; set; } = new Vector2(4f, 4f);


		public bool ChoyaHunt { get; set; }

		public bool CaptureInput { get; set; } = true;


		public Color TextureColor { get; set; } = Color.get_White();


		public Point StartPoint
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _startPoint;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				if (_startPoint != value)
				{
					_startPoint = value;
					_xOffset = _startPoint.X;
					_yOffset = _startPoint.Y;
				}
			}
		}

		public bool CanMove
		{
			get
			{
				return _canMove;
			}
			set
			{
				if (_canMove != value)
				{
					_canMove = value;
					ResetPosition();
				}
			}
		}

		public AsyncTexture2D ChoyaTexture { get; set; }

		public Rectangle MovementBounds
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _movementBounds;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				_movementBounds = value;
			}
		}

		public InputDetectionService InputDetectionService { get; }

		public event EventHandler ChoyaLeftBounds;

		public RollingChoya()
			: this()
		{
		}//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)


		public RollingChoya(InputDetectionService inputDetectionService)
			: this()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			InputDetectionService = inputDetectionService;
			inputDetectionService.MouseClicked += InputDetectionService_MouseClicked;
		}

		private void InputDetectionService_MouseClicked(object sender, double e)
		{
			if (ChoyaHunt && _choyaTargeted)
			{
				((Control)this).OnClick((MouseEventArgs)null);
			}
		}

		protected override CaptureType CapturesInput()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (!CaptureInput)
			{
				return (CaptureType)0;
			}
			return ((Control)this).CapturesInput();
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((Control)this).OnClick(e);
		}

		public void ResetPosition()
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			if (CanMove)
			{
				_xOffset = ChoyaSize / 2;
				_yOffset = 0f;
			}
			else
			{
				int size = Math.Min(((Control)this).get_Width(), ((Control)this).get_Height());
				Rectangle movementBounds = ((((Control)this).get_Parent() != null) ? ((Control)((Control)this).get_Parent()).get_AbsoluteBounds() : Rectangle.get_Empty());
				((Control)this).set_Location(new Point((movementBounds.Width - size) / 2, (movementBounds.Height - size) / 2));
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			if (ChoyaTexture != null)
			{
				float rotation = (float)((GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds - _start) / (double)Steps);
				int choyaTargeted;
				if (ChoyaHunt)
				{
					Rectangle absoluteBounds = ((Control)this).get_AbsoluteBounds();
					choyaTargeted = (((Rectangle)(ref absoluteBounds)).Contains(Control.get_Input().get_Mouse().get_Position()) ? 1 : 0);
				}
				else
				{
					choyaTargeted = 0;
				}
				_choyaTargeted = (byte)choyaTargeted != 0;
				Rectangle movementBounds = ((((Control)this).get_Parent() != null) ? ((Control)this).get_Parent().get_ContentRegion() : Rectangle.get_Empty());
				int size = Math.Min(((Control)this).get_Width(), ((Control)this).get_Height());
				int choyaSize = Math.Min(ChoyaTexture.get_Bounds().Width, ChoyaTexture.get_Bounds().Height);
				_xOffset += (CanMove ? TravelDistance.X : 0f);
				_yOffset += (CanMove ? TravelDistance.Y : 0f);
				Rectangle choyaRect = default(Rectangle);
				((Rectangle)(ref choyaRect))._002Ector(new Point(size / 2), new Point(size));
				if (CanMove)
				{
					((Control)this).set_Location(new Point(movementBounds.X + (int)(CanMove ? _xOffset : 0f), movementBounds.Y + (int)(CanMove ? _yOffset : 0f)));
				}
				((Control)this).set_Size(new Point(size));
				if (ChoyaTexture != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(ChoyaTexture), choyaRect, (Rectangle?)ChoyaTexture.get_Bounds(), _choyaTargeted ? Color.get_Red() : TextureColor, rotation, new Vector2((float)(choyaSize / 2)), (SpriteEffects)0);
				}
				if ((float)movementBounds.Width < (float)((Control)this).get_Location().X + TravelDistance.X + (float)(choyaSize / 20))
				{
					this.ChoyaLeftBounds?.Invoke(this, null);
					_xOffset = -(int)((double)choyaSize * 0.7);
				}
				else if ((float)((Control)this).get_Location().X + TravelDistance.X < (float)(-(int)((double)choyaSize * 0.7)))
				{
					this.ChoyaLeftBounds?.Invoke(this, null);
					_xOffset = movementBounds.Width - (int)((double)choyaSize * 0.05);
				}
				if ((float)movementBounds.Height < (float)((Control)this).get_Location().Y + TravelDistance.Y + (float)(choyaSize / 20))
				{
					this.ChoyaLeftBounds?.Invoke(this, null);
					_yOffset = -(int)((double)choyaSize * 0.7);
				}
				else if ((float)((Control)this).get_Location().Y + TravelDistance.Y < (float)(-(int)((double)choyaSize * 0.7)))
				{
					this.ChoyaLeftBounds?.Invoke(this, null);
					_yOffset = movementBounds.Height - (int)((double)choyaSize * 0.05);
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
			if (InputDetectionService != null)
			{
				InputDetectionService.MouseClicked -= InputDetectionService_MouseClicked;
			}
		}
	}
}
