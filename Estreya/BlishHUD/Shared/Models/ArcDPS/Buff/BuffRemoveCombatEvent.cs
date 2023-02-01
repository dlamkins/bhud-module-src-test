using Blish_HUD.ArcDps.Models;

namespace Estreya.BlishHUD.Shared.Models.ArcDPS.Buff
{
	public class BuffRemoveCombatEvent : CombatEvent
	{
		public override Ag Source => base.Dst;

		public override Ag Destination => Source;

		public int RemainingTimeRemovedAsDuration => base.Ev.get_Value();

		public int RemainingTimeRemovedAsIntensity => base.Ev.get_BuffDmg();

		public int StacksRemoved => base.Ev.get_Result();

		public BuffRemoveCombatEvent(Ev ev, Ag src, Ag dst, CombatEventCategory category, CombatEventType type, CombatEventState state)
			: base(ev, src, dst, category, type, state)
		{
		}
	}
}
