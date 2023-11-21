using Blish_HUD.ArcDps;
using Blish_HUD.ArcDps.Models;

namespace Estreya.BlishHUD.Shared.Models.ArcDPS.StateChange
{
	public abstract class StateChangeCombatEvent : CombatEvent
	{
		public StateChange StateChange => base.Ev.get_IsStateChange();

		public StateChangeCombatEvent(CombatEvent combatEvent, CombatEventCategory category, CombatEventType type, CombatEventState state)
			: base(combatEvent, category, type, state)
		{
		}
	}
}
