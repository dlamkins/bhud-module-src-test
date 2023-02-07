using System;
using Blish_HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.Core.Services
{
	public class InputDetectionService
	{
		private double _lastInteraction;

		private double _lastKeyInteraction;

		private double _lastMouseInteraction;

		private double _lastMouseMove;

		private double _lastMouseClick;

		private Point _lastMousePosition;

		public double LastInteraction
		{
			get
			{
				return _lastInteraction;
			}
			set
			{
				if (_lastInteraction != value)
				{
					_lastInteraction = value;
					this.Interacted?.Invoke(this, value);
				}
			}
		}

		public double LastKeyInteraction
		{
			get
			{
				return _lastKeyInteraction;
			}
			set
			{
				if (_lastKeyInteraction != value)
				{
					_lastKeyInteraction = value;
					this.KeyInteracted?.Invoke(this, value);
				}
			}
		}

		public double LastMouseInteraction
		{
			get
			{
				return _lastMouseInteraction;
			}
			set
			{
				if (_lastMouseInteraction != value)
				{
					_lastMouseInteraction = value;
					this.MouseInteracted?.Invoke(this, value);
				}
			}
		}

		public double LastMouseMove
		{
			get
			{
				return _lastMouseMove;
			}
			set
			{
				if (_lastMouseMove != value)
				{
					_lastMouseMove = value;
					this.MouseMoved?.Invoke(this, value);
				}
			}
		}

		public double LastMouseClick
		{
			get
			{
				return _lastMouseClick;
			}
			set
			{
				if (_lastMouseClick != value)
				{
					_lastMouseClick = value;
					this.MouseClicked?.Invoke(this, value);
				}
			}
		}

		public event EventHandler<double> Interacted;

		public event EventHandler<double> KeyInteracted;

		public event EventHandler<double> MouseInteracted;

		public event EventHandler<double> MouseMoved;

		public event EventHandler<double> MouseClicked;

		public void Run(GameTime gameTime)
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Invalid comparison between Unknown and I4
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Invalid comparison between Unknown and I4
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			double now = gameTime.get_TotalGameTime().TotalMilliseconds;
			LastKeyInteraction = ((GameService.Input.get_Keyboard().get_KeysDown().Count > 0) ? now : LastKeyInteraction);
			MouseState mouse = GameService.Input.get_Mouse().get_State();
			LastMouseClick = (((int)((MouseState)(ref mouse)).get_LeftButton() == 1 || (int)((MouseState)(ref mouse)).get_RightButton() == 1) ? now : LastMouseClick);
			LastMouseMove = ((((MouseState)(ref mouse)).get_Position() != _lastMousePosition) ? now : LastMouseMove);
			LastMouseInteraction = Math.Max(LastMouseMove, LastMouseClick);
			LastInteraction = Math.Max(LastMouseInteraction, LastKeyInteraction);
			_lastMousePosition = ((MouseState)(ref mouse)).get_Position();
		}
	}
}
