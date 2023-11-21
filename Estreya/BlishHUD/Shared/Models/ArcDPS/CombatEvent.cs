using System;
using Blish_HUD.ArcDps.Models;
using Estreya.BlishHUD.Shared.Models.GW2API.Skills;

namespace Estreya.BlishHUD.Shared.Models.ArcDPS
{
	public abstract class CombatEvent
	{
		public CombatEvent RawCombatEvent { get; private set; }

		protected Ev Ev
		{
			get
			{
				CombatEvent rawCombatEvent = RawCombatEvent;
				if (rawCombatEvent == null)
				{
					return null;
				}
				return rawCombatEvent.get_Ev();
			}
		}

		protected Ag Src
		{
			get
			{
				CombatEvent rawCombatEvent = RawCombatEvent;
				if (rawCombatEvent == null)
				{
					return null;
				}
				return rawCombatEvent.get_Src();
			}
		}

		protected Ag Dst
		{
			get
			{
				CombatEvent rawCombatEvent = RawCombatEvent;
				if (rawCombatEvent == null)
				{
					return null;
				}
				return rawCombatEvent.get_Dst();
			}
		}

		public abstract Ag Source { get; }

		public abstract Ag Destination { get; }

		public uint SkillId
		{
			get
			{
				Ev ev = Ev;
				if (ev == null)
				{
					return 0u;
				}
				return ev.get_SkillId();
			}
		}

		public CombatEventCategory Category { get; }

		public CombatEventType Type { get; }

		public CombatEventState State { get; }

		public Skill Skill { get; set; }

		public CombatEvent(CombatEvent combatEvent, CombatEventCategory category, CombatEventType type, CombatEventState state)
		{
			RawCombatEvent = combatEvent;
			Category = category;
			Type = type;
			State = state;
		}

		public void Dispose()
		{
			RawCombatEvent = null;
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
			//IL_0059: Invalid comparison between Unknown and I4
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
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
			if ((int)ev.get_IsStateChange() == 18)
			{
				return CombatEventState.BUFFAPPLY;
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
