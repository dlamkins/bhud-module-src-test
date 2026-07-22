using System;
using System.Runtime.CompilerServices;
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

		private Rectangle _movementBounds = Rectangle.Empty;

		private Point _startPoint = Point.Zero;

		private bool _choyaTargeted;

		private int ChoyaSize => Math.Min(base.Width, base.Height);

		public int Steps { get; set; } = 360;


		public Vector2 TravelDistance { get; set; } = new Vector2(4f, 4f);


		public bool ChoyaHunt { get; set; }

		public bool CaptureInput { get; set; } = true;


		public Color TextureColor { get; set; } = Color.White;


		public Point StartPoint
		{
			get
			{
				return _startPoint;
			}
			set
			{
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
			[CompilerGenerated]
			get
			{
				return _003CCanMove_003Ek__BackingField;
			}
			set
			{
				if (_003CCanMove_003Ek__BackingField != value)
				{
					_003CCanMove_003Ek__BackingField = value;
					ResetPosition();
				}
			}
		}

		public AsyncTexture2D ChoyaTexture { get; set; }

		public Rectangle MovementBounds
		{
			get
			{
				return _movementBounds;
			}
			set
			{
				_movementBounds = value;
			}
		}

		public InputDetectionService InputDetectionService { get; }

		public event EventHandler ChoyaLeftBounds;

		public RollingChoya()
		{
			_003CCanMove_003Ek__BackingField = true;
			base._002Ector();
		}

		public RollingChoya(InputDetectionService inputDetectionService)
		{
			_003CCanMove_003Ek__BackingField = true;
			base._002Ector();
			InputDetectionService = inputDetectionService;
			inputDetectionService.MouseClicked += new EventHandler<double>(InputDetectionService_MouseClicked);
		}

		private void InputDetectionService_MouseClicked(object sender, double e)
		{
			if (ChoyaHunt && _choyaTargeted)
			{
				OnClick(null);
			}
		}

		protected override CaptureType CapturesInput()
		{
			if (!CaptureInput)
			{
				return CaptureType.None;
			}
			return base.CapturesInput();
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
		}

		public void ResetPosition()
		{
			if (CanMove)
			{
				_xOffset = ChoyaSize / 2;
				_yOffset = 0f;
			}
			else
			{
				int size = Math.Min(base.Width, base.Height);
				Rectangle movementBounds = ((base.Parent != null) ? base.Parent.AbsoluteBounds : Rectangle.Empty);
				base.Location = new Point((movementBounds.Width - size) / 2, (movementBounds.Height - size) / 2);
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (ChoyaTexture != null)
			{
				float rotation = (float)((GameService.Overlay.CurrentGameTime.TotalGameTime.TotalMilliseconds - _start) / (double)Steps);
				_choyaTargeted = ChoyaHunt && base.AbsoluteBounds.Contains(Control.Input.Mouse.Position);
				Rectangle movementBounds = ((base.Parent != null) ? base.Parent.ContentRegion : Rectangle.Empty);
				int size = Math.Min(base.Width, base.Height);
				int choyaSize = Math.Min(ChoyaTexture.Bounds.Width, ChoyaTexture.Bounds.Height);
				_xOffset += (CanMove ? TravelDistance.X : 0f);
				_yOffset += (CanMove ? TravelDistance.Y : 0f);
				Rectangle choyaRect = new Rectangle(new Point(size / 2), new Point(size));
				if (CanMove)
				{
					base.Location = new Point(movementBounds.X + (int)(CanMove ? _xOffset : 0f), movementBounds.Y + (int)(CanMove ? _yOffset : 0f));
				}
				base.Size = new Point(size);
				if (ChoyaTexture != null)
				{
					spriteBatch.DrawOnCtrl(this, ChoyaTexture, choyaRect, ChoyaTexture.Bounds, _choyaTargeted ? Color.Red : TextureColor, rotation, new Vector2(choyaSize / 2));
				}
				if ((float)movementBounds.Width < (float)base.Location.X + TravelDistance.X + (float)(choyaSize / 20))
				{
					this.ChoyaLeftBounds?.Invoke(this, null);
					_xOffset = -(int)((double)choyaSize * 0.7);
				}
				else if ((float)base.Location.X + TravelDistance.X < (float)(-(int)((double)choyaSize * 0.7)))
				{
					this.ChoyaLeftBounds?.Invoke(this, null);
					_xOffset = movementBounds.Width - (int)((double)choyaSize * 0.05);
				}
				if ((float)movementBounds.Height < (float)base.Location.Y + TravelDistance.Y + (float)(choyaSize / 20))
				{
					this.ChoyaLeftBounds?.Invoke(this, null);
					_yOffset = -(int)((double)choyaSize * 0.7);
				}
				else if ((float)base.Location.Y + TravelDistance.Y < (float)(-(int)((double)choyaSize * 0.7)))
				{
					this.ChoyaLeftBounds?.Invoke(this, null);
					_yOffset = movementBounds.Height - (int)((double)choyaSize * 0.05);
				}
			}
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			_start = GameService.Overlay.CurrentGameTime.TotalGameTime.TotalMilliseconds;
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			ChoyaTexture = null;
			if (InputDetectionService != null)
			{
				InputDetectionService.MouseClicked -= new EventHandler<double>(InputDetectionService_MouseClicked);
			}
		}
	}
}
