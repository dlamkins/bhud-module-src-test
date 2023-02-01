using Blish_HUD.ArcDps.Models;

namespace Estreya.BlishHUD.Shared.Models.ArcDPS.Damage
{
	public class DamageCombatEvent : CombatEvent
	{
		public override Ag Source => base.Src;

		public override Ag Destination => base.Dst;

		public bool IsBuffDamage => base.Ev.get_Buff();

		public int Value
		{
			get
			{
				if (!IsBuffDamage)
				{
					return base.Ev.get_Value();
				}
				return base.Ev.get_BuffDmg();
			}
		}

		public DamageCombatEvent(Ev ev, Ag src, Ag dst, CombatEventCategory category, CombatEventType type, CombatEventState state)
			: base(ev, src, dst, category, type, state)
		{
		}
	}
}
