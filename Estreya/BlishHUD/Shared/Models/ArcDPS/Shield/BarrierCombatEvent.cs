using Blish_HUD.ArcDps.Models;

namespace Estreya.BlishHUD.Shared.Models.ArcDPS.Shield
{
	public class BarrierCombatEvent : CombatEvent
	{
		public override Ag Source => base.Src;

		public override Ag Destination => base.Dst;

		public uint Value => base.Ev.get_OverStackValue();

		public BarrierCombatEvent(Ev ev, Ag src, Ag dst, CombatEventCategory category, CombatEventType type, CombatEventState state)
			: base(ev, src, dst, category, type, state)
		{
		}
	}
}
