using System;
using System.Collections.Generic;
using Blish_HUD;
using Newtonsoft.Json;

namespace Charr.Timers_BlishHUD.Models.Triggers
{
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class LocationTrigger : Trigger
	{
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
					return "invalid position";
				}
				List<float> antipode = base.Antipode;
				if ((antipode == null || antipode.Count != 3) && base.Radius <= 0f)
				{
					return "invalid radius/size";
				}
			}
			if (!base.CombatRequired && !base.OutOfCombatRequired && !base.EntryRequired && !base.DepartureRequired)
			{
				return "No possible trigger conditions.";
			}
			_initialized = true;
			return null;
		}

		public override void Enable()
		{
			_enabled = true;
		}

		public override void Disable()
		{
			_enabled = false;
		}

		public override void Reset()
		{
		}

		public override bool Triggered()
		{
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			if (!_enabled)
			{
				return false;
			}
			if (!TimersModule.ModuleInstance._debugModeSetting.get_Value() && base.CombatRequired && !GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat())
			{
				return false;
			}
			if (base.OutOfCombatRequired && GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat())
			{
				return false;
			}
			if (base.EntryRequired || base.DepartureRequired)
			{
				float x = GameService.Gw2Mumble.get_PlayerCharacter().get_Position().X;
				float y = GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Y;
				float z = GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Z;
				bool playerInArea = ((base.Antipode == null || base.Antipode.Count != 3) ? (Math.Sqrt(Math.Pow(x - base.Position[0], 2.0) + Math.Pow(y - base.Position[1], 2.0) + Math.Pow(z - base.Position[2], 2.0)) <= (double)base.Radius) : (x >= Math.Min(base.Position[0], base.Antipode[0]) && x <= Math.Max(base.Position[0], base.Antipode[0]) && y >= Math.Min(base.Position[1], base.Antipode[1]) && y <= Math.Max(base.Position[1], base.Antipode[1]) && z >= Math.Min(base.Position[2], base.Antipode[2]) && z <= Math.Max(base.Position[2], base.Antipode[2])));
				if ((base.EntryRequired && !playerInArea) || (base.DepartureRequired && playerInArea))
				{
					return false;
				}
			}
			return true;
		}
	}
}
