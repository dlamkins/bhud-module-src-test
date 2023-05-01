using Blish_HUD.ArcDps.Models;

namespace Estreya.BlishHUD.Shared.Models.ArcDPS.StateChange
{
	public class ExitCombatEvent : CombatEvent
	{
		public override Ag Source => base.Src;

		public override Ag Destination => null;

		public ExitCombatEvent(CombatEvent combatEvent, CombatEventCategory category, CombatEventType type, CombatEventState state)
			: base(combatEvent, category, type, state)
		{
		}
	}
}
