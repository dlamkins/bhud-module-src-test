using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Input;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace Charr.Timers_BlishHUD.Models.Triggers
{
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class KeyTrigger : Trigger
	{
		private Keys _key;

		private ModifierKeys _keyModifier;

		private EventHandler<KeyboardEventArgs> _keyPressedHandler;

		private bool _keysPressed;

		[JsonProperty("key")]
		public string Key { get; set; }

		[JsonProperty("keyModifier")]
		public string KeyModifier { get; set; }

		public override string Initialize()
		{
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
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
					return "invalid position";
				}
				List<float> antipode = base.Antipode;
				if ((antipode == null || antipode.Count != 3) && base.Radius <= 0f)
				{
					return "invalid radius/size";
				}
			}
			if (!base.CombatRequired && !base.OutOfCombatRequired && !base.EntryRequired && !base.DepartureRequired && Key == null)
			{
				return "No possible trigger conditions.";
			}
			if (Key != null)
			{
				try
				{
					_key = (Keys)Enum.Parse(typeof(Keys), Key, ignoreCase: true);
					_keyModifier = (ModifierKeys)((KeyModifier != null) ? ((int)(ModifierKeys)Enum.Parse(typeof(ModifierKeys), KeyModifier, ignoreCase: true)) : 0);
				}
				catch (Exception ex)
				{
					return ex.Message;
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
				GameService.Input.get_Keyboard().add_KeyPressed(_keyPressedHandler);
				_keysPressed = false;
			}
		}

		public override void Disable()
		{
			if (_enabled)
			{
				_enabled = false;
				GameService.Input.get_Keyboard().remove_KeyPressed(_keyPressedHandler);
				_keysPressed = false;
			}
		}

		public override void Reset()
		{
			_keysPressed = false;
		}

		public override bool Triggered()
		{
			if (Key == null)
			{
				return true;
			}
			return _keysPressed;
		}

		private void HandleKeyPressEvents(object sender, KeyboardEventArgs args)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			if (!_enabled || args.get_Key() != _key || (ModifierKeys)(GameService.Input.get_Keyboard().get_ActiveModifiers() & _keyModifier) != _keyModifier || (!TimersModule.ModuleInstance._debugModeSetting.get_Value() && base.CombatRequired && !GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat()) || (base.OutOfCombatRequired && GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat()))
			{
				return;
			}
			if (base.EntryRequired || base.DepartureRequired)
			{
				float x = GameService.Gw2Mumble.get_PlayerCharacter().get_Position().X;
				float y = GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Y;
				float z = GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Z;
				bool playerInArea = ((base.Antipode == null || base.Antipode.Count != 3) ? (Math.Sqrt(Math.Pow(x - base.Position[0], 2.0) + Math.Pow(y - base.Position[1], 2.0) + Math.Pow(z - base.Position[2], 2.0)) <= (double)base.Radius) : (x >= Math.Min(base.Position[0], base.Antipode[0]) && x <= Math.Max(base.Position[0], base.Antipode[0]) && y >= Math.Min(base.Position[1], base.Antipode[1]) && y <= Math.Max(base.Position[1], base.Antipode[1]) && z >= Math.Min(base.Position[2], base.Antipode[2]) && z <= Math.Max(base.Position[2], base.Antipode[2])));
				if ((base.EntryRequired && !playerInArea) || (base.DepartureRequired && playerInArea))
				{
					return;
				}
			}
			_keysPressed = true;
		}
	}
}
