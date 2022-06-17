using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Input;
using Newtonsoft.Json;

namespace Charr.Timers_BlishHUD.Models.Triggers
{
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class KeyTrigger : Trigger
	{
		private EventHandler<KeyboardEventArgs> _keyPressedHandler;

		private bool _keysPressed;

		[JsonProperty("keyBind")]
		public int KeyBind { get; set; }

		public override string Initialize()
		{
			if (base.CombatRequired && base.OutOfCombatRequired)
			{
				return "requireCombat and requireOutOfCombat cannot both be set to true";
			}
			if (base.EntryRequired && base.DepartureRequired)
			{
				return "requireEntry and requireDeparture cannot both be set to true";
			}
			if (base.EntryRequired || base.DepartureRequired)
			{
				List<float> position = base.Position;
				if (position == null || position.Count != 3)
				{
					List<float> position2 = base.Position;
					if (position2 == null || position2.Count != 2)
					{
						return "invalid position";
					}
				}
				List<float> antipode = base.Antipode;
				if ((antipode == null || antipode.Count != 3) && base.Radius <= 0f)
				{
					return "invalid radius/size";
				}
			}
			_keyPressedHandler = HandleKeyPressEvents;
			_initialized = true;
			return null;
		}

		public override void Enable()
		{
			if (!_enabled)
			{
				_enabled = true;
				GameService.Input.Keyboard.KeyPressed += _keyPressedHandler;
				_keysPressed = false;
			}
		}

		public override void Disable()
		{
			if (_enabled)
			{
				_enabled = false;
				GameService.Input.Keyboard.KeyPressed -= _keyPressedHandler;
				_keysPressed = false;
			}
		}

		public override void Reset()
		{
			_keysPressed = false;
		}

		public override bool Triggered()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			if ((int)TimersModule.ModuleInstance._keyBindSettings[KeyBind].Value.PrimaryKey == 0)
			{
				return false;
			}
			return _keysPressed;
		}

		private void HandleKeyPressEvents(object sender, KeyboardEventArgs args)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			if (!_enabled || args.Key != TimersModule.ModuleInstance._keyBindSettings[KeyBind].Value.PrimaryKey || GameService.Input.Keyboard.ActiveModifiers != TimersModule.ModuleInstance._keyBindSettings[KeyBind].Value.ModifierKeys || (!TimersModule.ModuleInstance._debugModeSetting.Value && base.CombatRequired && !GameService.Gw2Mumble.PlayerCharacter.IsInCombat) || (base.OutOfCombatRequired && GameService.Gw2Mumble.PlayerCharacter.IsInCombat))
			{
				return;
			}
			if (base.EntryRequired || base.DepartureRequired)
			{
				float x = GameService.Gw2Mumble.PlayerCharacter.Position.X;
				float y = GameService.Gw2Mumble.PlayerCharacter.Position.Y;
				float z = GameService.Gw2Mumble.PlayerCharacter.Position.Z;
				bool playerInArea = ((base.Antipode == null || base.Antipode.Count != 3) ? (((base.Position.Count == 2) ? Math.Sqrt(Math.Pow(x - base.Position[0], 2.0) + Math.Pow(y - base.Position[1], 2.0)) : Math.Sqrt(Math.Pow(x - base.Position[0], 2.0) + Math.Pow(y - base.Position[1], 2.0) + Math.Pow(z - base.Position[2], 2.0))) <= (double)base.Radius) : (x >= Math.Min(base.Position[0], base.Antipode[0]) && x <= Math.Max(base.Position[0], base.Antipode[0]) && y >= Math.Min(base.Position[1], base.Antipode[1]) && y <= Math.Max(base.Position[1], base.Antipode[1]) && z >= Math.Min(base.Position[2], base.Antipode[2]) && z <= Math.Max(base.Position[2], base.Antipode[2])));
				if ((base.EntryRequired && !playerInArea) || (base.DepartureRequired && playerInArea))
				{
					return;
				}
			}
			_keysPressed = true;
		}
	}
}
