using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.Core.Services
{
	public class InputDetectionService
	{
		private readonly List<Keys> _ignoredKeys = new List<Keys>(1) { (Keys)0 };

		private readonly List<Keys> _noKeys = new List<Keys>();

		private Point _lastMousePosition;

		public bool Enabled { get; set; } = true;


		public double LastInteraction
		{
			[CompilerGenerated]
			get
			{
				return _003CLastInteraction_003Ek__BackingField;
			}
			set
			{
				if (_003CLastInteraction_003Ek__BackingField != value)
				{
					_003CLastInteraction_003Ek__BackingField = value;
					this.Interacted?.Invoke(this, value);
				}
			}
		}

		public double LastKeyInteraction
		{
			[CompilerGenerated]
			get
			{
				return _003CLastKeyInteraction_003Ek__BackingField;
			}
			set
			{
				if (_003CLastKeyInteraction_003Ek__BackingField != value)
				{
					_003CLastKeyInteraction_003Ek__BackingField = value;
					this.KeyInteracted?.Invoke(this, value);
				}
			}
		}

		public double LastMouseInteraction
		{
			[CompilerGenerated]
			get
			{
				return _003CLastMouseInteraction_003Ek__BackingField;
			}
			set
			{
				if (_003CLastMouseInteraction_003Ek__BackingField != value)
				{
					_003CLastMouseInteraction_003Ek__BackingField = value;
					this.MouseInteracted?.Invoke(this, value);
				}
			}
		}

		public double LastMouseMove
		{
			[CompilerGenerated]
			get
			{
				return _003CLastMouseMove_003Ek__BackingField;
			}
			set
			{
				if (_003CLastMouseMove_003Ek__BackingField != value)
				{
					_003CLastMouseMove_003Ek__BackingField = value;
					this.MouseMoved?.Invoke(this, value);
				}
			}
		}

		public double LastMouseClick
		{
			[CompilerGenerated]
			get
			{
				return _003CLastMouseClick_003Ek__BackingField;
			}
			set
			{
				if (_003CLastMouseClick_003Ek__BackingField != value)
				{
					_003CLastMouseClick_003Ek__BackingField = value;
					this.MouseClicked?.Invoke(this, value);
				}
			}
		}

		public double LastClickOrKey
		{
			[CompilerGenerated]
			get
			{
				return _003CLastClickOrKey_003Ek__BackingField;
			}
			set
			{
				if (_003CLastClickOrKey_003Ek__BackingField != value)
				{
					_003CLastClickOrKey_003Ek__BackingField = value;
					this.ClickedOrKey?.Invoke(this, value);
				}
			}
		}

		public event EventHandler<double> Interacted;

		public event EventHandler<double> KeyInteracted;

		public event EventHandler<double> MouseInteracted;

		public event EventHandler<double> MouseMoved;

		public event EventHandler<double> MouseClicked;

		public event EventHandler<double> ClickedOrKey;

		public void Run(GameTime gameTime)
		{
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Invalid comparison between Unknown and I4
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Invalid comparison between Unknown and I4
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Invalid comparison between Unknown and I4
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			if (Enabled)
			{
				double now = gameTime.get_TotalGameTime().TotalMilliseconds;
				IEnumerable<Keys> enumerable;
				if (GameService.Input.Keyboard.KeysDown.Count <= 0)
				{
					IEnumerable<Keys> noKeys = _noKeys;
					enumerable = noKeys;
				}
				else
				{
					enumerable = GameService.Input.Keyboard.KeysDown.Except(_ignoredKeys).Distinct();
				}
				IEnumerable<Keys> keys = enumerable;
				LastKeyInteraction = ((keys.Count() > 0) ? now : LastKeyInteraction);
				MouseState mouse = GameService.Input.Mouse.State;
				LastMouseClick = (((int)((MouseState)(ref mouse)).get_LeftButton() == 1 || (int)((MouseState)(ref mouse)).get_RightButton() == 1 || (int)((MouseState)(ref mouse)).get_MiddleButton() == 1) ? now : LastMouseClick);
				LastMouseMove = ((((MouseState)(ref mouse)).get_Position() != _lastMousePosition) ? now : LastMouseMove);
				LastMouseInteraction = Math.Max(LastMouseMove, LastMouseClick);
				LastClickOrKey = Math.Max(LastKeyInteraction, LastMouseClick);
				LastInteraction = Math.Max(LastMouseInteraction, LastKeyInteraction);
				_lastMousePosition = ((MouseState)(ref mouse)).get_Position();
			}
		}
	}
}
