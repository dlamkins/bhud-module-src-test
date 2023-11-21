using Blish_HUD.ArcDps.Models;

namespace Estreya.BlishHUD.Shared.Models.ArcDPS.Buff
{
	public class BuffApplyCombatEvent : CombatEvent
	{
		public override Ag Source => base.Src;

		public override Ag Destination => base.Dst;

		public int AppliedDuration => base.Ev.get_Value();

		public BuffApplyCombatEvent(CombatEvent combatEvent, CombatEventCategory category, CombatEventType type, CombatEventState state)
			: base(combatEvent, category, type, state)
		{
		}
	}
}
