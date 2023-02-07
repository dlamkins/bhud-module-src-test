using Blish_HUD.ArcDps;
using Blish_HUD.ArcDps.Models;

namespace Estreya.BlishHUD.Shared.Models.ArcDPS.StateChange
{
	public abstract class StateChangeCombatEvent : CombatEvent
	{
		public StateChange StateChange => base.Ev.get_IsStateChange();

		public StateChangeCombatEvent(Ev ev, Ag src, Ag dst, CombatEventCategory category, CombatEventType type, CombatEventState state)
			: base(ev, src, dst, category, type, state)
		{
		}
	}
}
