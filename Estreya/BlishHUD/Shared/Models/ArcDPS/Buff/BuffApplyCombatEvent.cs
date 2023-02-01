using Blish_HUD.ArcDps.Models;

namespace Estreya.BlishHUD.Shared.Models.ArcDPS.Buff
{
	public class BuffApplyCombatEvent : CombatEvent
	{
		public override Ag Source => base.Src;

		public override Ag Destination => base.Dst;

		public int AppliedDuration => base.Ev.get_Value();

		public BuffApplyCombatEvent(Ev ev, Ag src, Ag dst, CombatEventCategory category, CombatEventType type, CombatEventState state)
			: base(ev, src, dst, category, type, state)
		{
		}
	}
}
