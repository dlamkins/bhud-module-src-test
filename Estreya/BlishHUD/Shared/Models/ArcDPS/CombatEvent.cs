using System;
using Blish_HUD.ArcDps.Models;
using Estreya.BlishHUD.Shared.Models.GW2API.Skills;

namespace Estreya.BlishHUD.Shared.Models.ArcDPS
{
	public abstract class CombatEvent
	{
		protected Ev Ev { get; private set; }

		protected Ag Src { get; private set; }

		protected Ag Dst { get; private set; }

		public abstract Ag Source { get; }

		public abstract Ag Destination { get; }

		public uint SkillId => Ev.get_SkillId();

		public CombatEventCategory Category { get; }

		public CombatEventType Type { get; }

		public CombatEventState State { get; }

		public Skill Skill { get; set; }

		public CombatEvent(Ev ev, Ag src, Ag dst, CombatEventCategory category, CombatEventType type, CombatEventState state)
		{
			Ev = ev;
			Src = src;
			Dst = dst;
			Category = category;
			Type = type;
			State = state;
		}

		public void Dispose()
		{
			Ev = null;
			Src = null;
			Dst = null;
			Skill = null;
		}

		public static CombatEventState GetState(Ev ev)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			if (ev == null)
			{
				throw new ArgumentNullException("ev", "Ev can't be null.");
			}
			if ((int)ev.get_IsStateChange() != 0)
			{
				return CombatEventState.STATECHANGE;
			}
			if ((int)ev.get_IsStateChange() == 0 && (int)ev.get_IsActivation() != 0)
			{
				return CombatEventState.ACTIVATION;
			}
			if ((int)ev.get_IsStateChange() == 0 && (int)ev.get_IsActivation() == 0 && (int)ev.get_IsBuffRemove() != 0 && ev.get_Buff())
			{
				return CombatEventState.BUFFREMOVE;
			}
			if ((int)ev.get_IsStateChange() == 0 && (int)ev.get_IsActivation() == 0 && (int)ev.get_IsBuffRemove() == 0)
			{
				if (!ev.get_Buff() || ev.get_BuffDmg() != 0 || ev.get_Value() == 0)
				{
					return CombatEventState.NORMAL;
				}
				return CombatEventState.BUFFAPPLY;
			}
			throw new ArgumentOutOfRangeException("ev", "Event state invalid.");
		}
	}
}
