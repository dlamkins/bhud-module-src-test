using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework.Input;

namespace Manlaan.CommanderMarkers.Settings.Controls
{
	public class NuclearOptionButton : StandardButton
	{
		private bool _safetyKeysPressed;

		public NuclearOptionButton()
			: this()
		{
			((Control)this).set_Enabled(false);
			GameService.Input.get_Keyboard().add_KeyPressed((EventHandler<KeyboardEventArgs>)SafetySwitch);
			GameService.Input.get_Keyboard().add_KeyReleased((EventHandler<KeyboardEventArgs>)SafetySwitch);
		}

		private void SafetySwitch(object sender, KeyboardEventArgs e)
		{
			IReadOnlyList<Keys> KeysDown = GameService.Input.get_Keyboard().get_KeysDown();
			_safetyKeysPressed = (KeysDown.Contains((Keys)162) || KeysDown.Contains((Keys)163)) && (KeysDown.Contains((Keys)160) || KeysDown.Contains((Keys)161));
			((Control)this).set_Enabled(_safetyKeysPressed);
		}

		protected override void DisposeControl()
		{
			GameService.Input.get_Keyboard().remove_KeyPressed((EventHandler<KeyboardEventArgs>)SafetySwitch);
			GameService.Input.get_Keyboard().remove_KeyReleased((EventHandler<KeyboardEventArgs>)SafetySwitch);
		}
	}
}
