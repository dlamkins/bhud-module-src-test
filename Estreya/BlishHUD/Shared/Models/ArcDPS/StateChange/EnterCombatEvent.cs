using Blish_HUD.ArcDps.Models;

namespace Estreya.BlishHUD.Shared.Models.ArcDPS.StateChange
{
	public class EnterCombatEvent : CombatEvent
	{
		public override Ag Source => base.Src;

		public override Ag Destination => null;

		public ulong Subgroup => base.Dst.get_Id();

		public EnterCombatEvent(CombatEvent combatEvent, CombatEventCategory category, CombatEventType type, CombatEventState state)
			: base(combatEvent, category, type, state)
		{
		}
	}
}
