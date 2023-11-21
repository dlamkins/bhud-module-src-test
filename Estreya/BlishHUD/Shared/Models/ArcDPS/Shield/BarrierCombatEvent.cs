using Blish_HUD.ArcDps.Models;

namespace Estreya.BlishHUD.Shared.Models.ArcDPS.Shield
{
	public class BarrierCombatEvent : CombatEvent
	{
		public override Ag Source => base.Src;

		public override Ag Destination => base.Dst;

		public uint Value => base.Ev.get_OverStackValue();

		public BarrierCombatEvent(CombatEvent combatEvent, CombatEventCategory category, CombatEventType type, CombatEventState state)
			: base(combatEvent, category, type, state)
		{
		}
	}
}
