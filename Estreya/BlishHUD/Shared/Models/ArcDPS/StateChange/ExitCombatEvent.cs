using Blish_HUD.ArcDps.Models;

namespace Estreya.BlishHUD.Shared.Models.ArcDPS.StateChange
{
	public class ExitCombatEvent : CombatEvent
	{
		public override Ag Source => base.Src;

		public override Ag Destination => null;

		public ExitCombatEvent(Ev ev, Ag src, Ag dst, CombatEventCategory category, CombatEventType type, CombatEventState state)
			: base(ev, src, dst, category, type, state)
		{
		}
	}
}
