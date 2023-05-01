using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.ArcDps;
using Blish_HUD.ArcDps.Models;
using Estreya.BlishHUD.Shared.Models.ArcDPS;
using Estreya.BlishHUD.Shared.Models.ArcDPS.Buff;
using Estreya.BlishHUD.Shared.Models.ArcDPS.Damage;
using Estreya.BlishHUD.Shared.Models.ArcDPS.Heal;
using Estreya.BlishHUD.Shared.Models.ArcDPS.Shield;
using Estreya.BlishHUD.Shared.Models.ArcDPS.StateChange;
using Estreya.BlishHUD.Shared.Models.GW2API.Skills;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.Shared.Services
{
	public class ArcDPSService : ManagedService
	{
		private const int MAX_PARSE_PER_LOOP = 50;

		private SkillService _skillState;

		private ConcurrentQueue<RawCombatEventArgs> _rawCombatEventQueue;

		private ConcurrentQueue<(CombatEvent combatEvent, CombatEventType scope)> _parsedCombatEventQueue;

		private bool _checkedFirstFrame;

		private bool _lastState = true;

		public ushort _selfInstId;

		public event EventHandler Started;

		public event EventHandler Stopped;

		public event EventHandler Unavailable;

		public event EventHandler<CombatEvent> AreaCombatEvent;

		public event EventHandler<CombatEvent> LocalCombatEvent;

		public ArcDPSService(ServiceConfiguration configuration, SkillService skillState)
			: base(configuration)
		{
			_skillState = skillState;
		}

		protected override Task Clear()
		{
			_rawCombatEventQueue = new ConcurrentQueue<RawCombatEventArgs>();
			_parsedCombatEventQueue = new ConcurrentQueue<(CombatEvent, CombatEventType)>();
			return Task.CompletedTask;
		}

		protected override Task Initialize()
		{
			_rawCombatEventQueue = new ConcurrentQueue<RawCombatEventArgs>();
			_parsedCombatEventQueue = new ConcurrentQueue<(CombatEvent, CombatEventType)>();
			GameService.ArcDps.add_RawCombatEvent((EventHandler<RawCombatEventArgs>)ArcDps_RawCombatEvent);
			return Task.CompletedTask;
		}

		protected override Task InternalReload()
		{
			return Task.CompletedTask;
		}

		protected override void InternalUnload()
		{
			GameService.ArcDps.remove_RawCombatEvent((EventHandler<RawCombatEventArgs>)ArcDps_RawCombatEvent);
			_skillState = null;
			_rawCombatEventQueue = null;
			_parsedCombatEventQueue = null;
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			if (!_checkedFirstFrame && !GameService.ArcDps.get_Running())
			{
				Logger.Debug("ArcDPS Service not available.");
				this.Unavailable?.Invoke(this, EventArgs.Empty);
				_checkedFirstFrame = true;
				_lastState = false;
			}
			else if (GameService.ArcDps.get_Running() != _lastState)
			{
				if (GameService.ArcDps.get_Running())
				{
					Logger.Debug("ArcDPS Service started.");
					this.Started?.Invoke(this, EventArgs.Empty);
				}
				else
				{
					Logger.Debug("ArcDPS Service stopped.");
					this.Stopped?.Invoke(this, EventArgs.Empty);
				}
				_lastState = GameService.ArcDps.get_Running();
			}
			for (int parseCounter = 0; parseCounter < 50; parseCounter++)
			{
				if (!_rawCombatEventQueue.TryDequeue(out var eventData))
				{
					break;
				}
				foreach (CombatEvent parsedCombatEvent in ParseCombatEvent(eventData))
				{
					AddSkill(parsedCombatEvent, eventData.get_CombatEvent().get_SkillName());
					EmitEvent(parsedCombatEvent, eventData.get_EventType());
				}
			}
		}

		protected override Task Load()
		{
			return Task.CompletedTask;
		}

		protected override Task Save()
		{
			return Task.CompletedTask;
		}

		public void SimulateCombatEvent(RawCombatEventArgs rawCombatEventArgs)
		{
			if (base.Running)
			{
				ArcDps_RawCombatEvent(null, rawCombatEventArgs);
			}
		}

		private void ArcDps_RawCombatEvent(object _, RawCombatEventArgs rawCombatEventArgs)
		{
			try
			{
				_rawCombatEventQueue.Enqueue(rawCombatEventArgs);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed adding combat event to queue.");
			}
		}

		private List<CombatEvent> ParseCombatEvent(RawCombatEventArgs rawCombatEventArgs)
		{
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Expected O, but got Unknown
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Expected O, but got Unknown
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Expected O, but got Unknown
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			List<CombatEvent> combatEvents = new List<CombatEvent>();
			if (!base.Running)
			{
				return combatEvents;
			}
			try
			{
				CombatEvent rawCombatEvent = rawCombatEventArgs.get_CombatEvent();
				Ev ev = rawCombatEvent.get_Ev();
				Ag src = rawCombatEvent.get_Src();
				Ag dst = rawCombatEvent.get_Dst();
				ulong targetAgentId = 0uL;
				if (ev != null)
				{
					string skillName = rawCombatEvent.get_SkillName();
					if (string.IsNullOrWhiteSpace(src.get_Name()))
					{
						src = new Ag("Unknown Source", src.get_Id(), src.get_Profession(), src.get_Elite(), src.get_Self(), src.get_Team());
					}
					if (string.IsNullOrWhiteSpace(dst.get_Name()))
					{
						dst = new Ag("Unknown Target", dst.get_Id(), dst.get_Profession(), dst.get_Elite(), dst.get_Self(), dst.get_Team());
					}
					if (string.IsNullOrWhiteSpace(skillName))
					{
						skillName = "Unknown Skill";
					}
					if (src.get_Self() == 1)
					{
						_selfInstId = ev.get_SrcInstId();
					}
					if (dst.get_Self() == 1)
					{
						_selfInstId = ev.get_DstInstId();
					}
					rawCombatEvent = new CombatEvent(ev, src, dst, skillName, rawCombatEvent.get_Id(), rawCombatEvent.get_Revision());
					combatEvents.AddRange(HandleNormalCombatEvents(rawCombatEvent, _selfInstId, targetAgentId, rawCombatEventArgs.get_EventType()));
					combatEvents.AddRange(HandleActivationEvents(rawCombatEvent, _selfInstId, targetAgentId, rawCombatEventArgs.get_EventType()));
					combatEvents.AddRange(HandleStatechangeEvents(rawCombatEvent, _selfInstId, targetAgentId, rawCombatEventArgs.get_EventType()));
					combatEvents.AddRange(HandleBuffEvents(rawCombatEvent, _selfInstId, targetAgentId, rawCombatEventArgs.get_EventType()));
					return combatEvents;
				}
				if (src != null)
				{
					targetAgentId = src.get_Id();
					return combatEvents;
				}
				return combatEvents;
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed parsing combat event:");
				return combatEvents;
			}
		}

		private List<CombatEvent> HandleStatechangeEvents(CombatEvent combatEvent, uint selfInstId, ulong targetAgentId, CombatEventType scope)
		{
			return new List<CombatEvent>();
		}

		private List<CombatEvent> HandleActivationEvents(CombatEvent combatEvent, uint selfInstId, ulong targetAgentId, CombatEventType scope)
		{
			return new List<CombatEvent>();
		}

		private List<CombatEvent> HandleBuffEvents(CombatEvent combatEvent, uint selfInstId, ulong targetAgentId, CombatEventType scope)
		{
			List<CombatEvent> combatEvents = new List<CombatEvent>();
			combatEvent.get_Ev();
			CombatEvent parsedCombatEvent = null;
			switch (CombatEvent.GetState(combatEvent.get_Ev()))
			{
			case CombatEventState.BUFFAPPLY:
				parsedCombatEvent = GetCombatEvent(combatEvent, (combatEvent.get_Src().get_Self() != 1) ? CombatEventCategory.PLAYER_IN : CombatEventCategory.PLAYER_OUT, CombatEventType.BUFF);
				break;
			case CombatEventState.BUFFREMOVE:
				parsedCombatEvent = GetCombatEvent(combatEvent, (combatEvent.get_Src().get_Self() == 1) ? CombatEventCategory.PLAYER_IN : CombatEventCategory.PLAYER_OUT, CombatEventType.BUFF);
				break;
			}
			if (parsedCombatEvent != null)
			{
				combatEvents.Add(parsedCombatEvent);
			}
			return combatEvents;
		}

		private List<CombatEvent> HandleNormalCombatEvents(CombatEvent combatEvent, ulong selfInstId, ulong targetAgentId, CombatEventType scope)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Expected O, but got Unknown
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Expected O, but got Unknown
			//IL_0310: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Unknown result type (might be due to invalid IL or missing references)
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			//IL_0340: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0375: Expected O, but got Unknown
			//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0402: Unknown result type (might be due to invalid IL or missing references)
			//IL_0408: Unknown result type (might be due to invalid IL or missing references)
			//IL_0420: Unknown result type (might be due to invalid IL or missing references)
			//IL_044f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Expected O, but got Unknown
			//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fe: Expected O, but got Unknown
			List<CombatEvent> combatEvents = new List<CombatEvent>();
			List<CombatEventType> types = new List<CombatEventType>();
			Ev ev = combatEvent.get_Ev();
			if ((int)ev.get_IsStateChange() != 0)
			{
				return combatEvents;
			}
			if ((int)ev.get_IsActivation() != 0)
			{
				return combatEvents;
			}
			if ((int)ev.get_IsBuffRemove() != 0)
			{
				return combatEvents;
			}
			if (ev.get_Buff())
			{
				ev.get_IsBuffRemove();
				if (ev.get_BuffDmg() > 0)
				{
					if (ev.get_OverStackValue() != 0)
					{
						int buffDmg2 = ev.get_BuffDmg() - (int)ev.get_OverStackValue();
						ev = new Ev(ev.get_Time(), ev.get_SrcAgent(), ev.get_DstAgent(), ev.get_Value(), buffDmg2, ev.get_OverStackValue(), ev.get_SkillId(), ev.get_SrcInstId(), ev.get_DstInstId(), ev.get_SrcMasterInstId(), ev.get_DstMasterInstId(), ev.get_Iff(), ev.get_Buff(), ev.get_Result(), ev.get_IsActivation(), ev.get_IsBuffRemove(), ev.get_IsNinety(), ev.get_IsFifty(), ev.get_IsMoving(), ev.get_IsStateChange(), ev.get_IsFlanking(), ev.get_IsShields(), ev.get_IsOffCycle(), ev.get_Pad61(), ev.get_Pad62(), ev.get_Pad63(), ev.get_Pad64());
						types.Add(CombatEventType.SHIELD_RECEIVE);
					}
					if (ev.get_BuffDmg() > 0)
					{
						types.Add(CombatEventType.HOT);
					}
				}
				else if (ev.get_BuffDmg() < 0)
				{
					if (ev.get_OverStackValue() != 0)
					{
						int buffDmg = ev.get_BuffDmg() + (int)ev.get_OverStackValue();
						ev = new Ev(ev.get_Time(), ev.get_SrcAgent(), ev.get_DstAgent(), ev.get_Value(), buffDmg, ev.get_OverStackValue(), ev.get_SkillId(), ev.get_SrcInstId(), ev.get_DstInstId(), ev.get_SrcMasterInstId(), ev.get_DstMasterInstId(), ev.get_Iff(), ev.get_Buff(), ev.get_Result(), ev.get_IsActivation(), ev.get_IsBuffRemove(), ev.get_IsNinety(), ev.get_IsFifty(), ev.get_IsMoving(), ev.get_IsStateChange(), ev.get_IsFlanking(), ev.get_IsShields(), ev.get_IsOffCycle(), ev.get_Pad61(), ev.get_Pad62(), ev.get_Pad63(), ev.get_Pad64());
						types.Add(CombatEventType.SHIELD_REMOVE);
					}
					if (ev.get_BuffDmg() < 0)
					{
						switch (ev.get_SkillId())
						{
						case 723u:
							types.Add(CombatEventType.POISON);
							break;
						case 736u:
							types.Add(CombatEventType.BLEEDING);
							break;
						case 737u:
							types.Add(CombatEventType.BURNING);
							break;
						case 861u:
							types.Add(CombatEventType.CONFUSION);
							break;
						case 873u:
							types.Add(CombatEventType.RETALIATION);
							break;
						case 19426u:
							types.Add(CombatEventType.TORMENT);
							break;
						default:
							types.Add(CombatEventType.DOT);
							break;
						}
					}
				}
			}
			else if (ev.get_Value() > 0)
			{
				if (ev.get_OverStackValue() != 0)
				{
					int value2 = ev.get_Value() + (int)ev.get_OverStackValue();
					ev = new Ev(ev.get_Time(), ev.get_SrcAgent(), ev.get_DstAgent(), value2, ev.get_BuffDmg(), ev.get_OverStackValue(), ev.get_SkillId(), ev.get_SrcInstId(), ev.get_DstInstId(), ev.get_SrcMasterInstId(), ev.get_DstMasterInstId(), ev.get_Iff(), ev.get_Buff(), ev.get_Result(), ev.get_IsActivation(), ev.get_IsBuffRemove(), ev.get_IsNinety(), ev.get_IsFifty(), ev.get_IsMoving(), ev.get_IsStateChange(), ev.get_IsFlanking(), ev.get_IsShields(), ev.get_IsOffCycle(), ev.get_Pad61(), ev.get_Pad62(), ev.get_Pad63(), ev.get_Pad64());
					types.Add(CombatEventType.SHIELD_RECEIVE);
				}
				if (ev.get_Value() > 0)
				{
					types.Add(CombatEventType.HEAL);
				}
			}
			else
			{
				if (ev.get_OverStackValue() != 0)
				{
					int value = ev.get_Value() + (int)ev.get_OverStackValue();
					ev = new Ev(ev.get_Time(), ev.get_SrcAgent(), ev.get_DstAgent(), value, ev.get_BuffDmg(), ev.get_OverStackValue(), ev.get_SkillId(), ev.get_SrcInstId(), ev.get_DstInstId(), ev.get_SrcMasterInstId(), ev.get_DstMasterInstId(), ev.get_Iff(), ev.get_Buff(), ev.get_Result(), ev.get_IsActivation(), ev.get_IsBuffRemove(), ev.get_IsNinety(), ev.get_IsFifty(), ev.get_IsMoving(), ev.get_IsStateChange(), ev.get_IsFlanking(), ev.get_IsShields(), ev.get_IsOffCycle(), ev.get_Pad61(), ev.get_Pad62(), ev.get_Pad63(), ev.get_Pad64());
					types.Add(CombatEventType.SHIELD_REMOVE);
				}
				if (ev.get_OverStackValue() == 0 || ev.get_Value() <= 0)
				{
					switch (ev.get_Result())
					{
					case 0:
					case 2:
					case 5:
						types.Add(CombatEventType.PHYSICAL);
						break;
					case 1:
						types.Add(CombatEventType.CRIT);
						break;
					case 3:
						types.Add(CombatEventType.BLOCK);
						break;
					case 4:
						types.Add(CombatEventType.EVADE);
						break;
					case 6:
						types.Add(CombatEventType.INVULNERABLE);
						break;
					case 7:
						types.Add(CombatEventType.MISS);
						break;
					}
				}
			}
			combatEvent = new CombatEvent(ev, combatEvent.get_Src(), combatEvent.get_Dst(), combatEvent.get_SkillName(), combatEvent.get_Id(), combatEvent.get_Revision());
			foreach (CombatEventType type in types)
			{
				List<CombatEventCategory> categories = new List<CombatEventCategory>();
				if (combatEvent.get_Src().get_Self() == 1)
				{
					if (combatEvent.get_Dst().get_Self() != 1)
					{
						categories.Add(CombatEventCategory.PLAYER_OUT);
					}
				}
				else if (ev.get_SrcMasterInstId() == selfInstId)
				{
					categories.Add(CombatEventCategory.PET_OUT);
				}
				if (combatEvent.get_Dst().get_Self() == 1)
				{
					categories.Add(CombatEventCategory.PLAYER_IN);
				}
				else if (ev.get_DstMasterInstId() == selfInstId)
				{
					categories.Add(CombatEventCategory.PET_IN);
				}
				foreach (CombatEventCategory category in categories)
				{
					CombatEvent parsedCombatEvent = GetCombatEvent(combatEvent, category, type);
					if (parsedCombatEvent != null)
					{
						combatEvents.Add(parsedCombatEvent);
					}
				}
			}
			return combatEvents;
		}

		private CombatEvent GetCombatEvent(CombatEvent combatEvent, CombatEventCategory category, CombatEventType type)
		{
			return CombatEvent.GetState(combatEvent.get_Ev()) switch
			{
				CombatEventState.NORMAL => GetNormalCombatEvent(combatEvent, category, type), 
				CombatEventState.STATECHANGE => GetStateChangeCombatEvent(combatEvent, category, type), 
				CombatEventState.BUFFREMOVE => new BuffRemoveCombatEvent(combatEvent, category, type, CombatEventState.BUFFREMOVE), 
				CombatEventState.BUFFAPPLY => new BuffApplyCombatEvent(combatEvent, category, type, CombatEventState.BUFFAPPLY), 
				_ => null, 
			};
		}

		private CombatEvent GetNormalCombatEvent(CombatEvent combatEvent, CombatEventCategory category, CombatEventType type)
		{
			switch (type)
			{
			case CombatEventType.NONE:
			case CombatEventType.PHYSICAL:
			case CombatEventType.CRIT:
			case CombatEventType.BLEEDING:
			case CombatEventType.BURNING:
			case CombatEventType.POISON:
			case CombatEventType.CONFUSION:
			case CombatEventType.RETALIATION:
			case CombatEventType.TORMENT:
			case CombatEventType.DOT:
			case CombatEventType.BLOCK:
			case CombatEventType.EVADE:
			case CombatEventType.INVULNERABLE:
			case CombatEventType.MISS:
				return new DamageCombatEvent(combatEvent, category, type, CombatEventState.NORMAL);
			case CombatEventType.HEAL:
			case CombatEventType.HOT:
				return new HealCombatEvent(combatEvent, category, type, CombatEventState.NORMAL);
			case CombatEventType.SHIELD_RECEIVE:
			case CombatEventType.SHIELD_REMOVE:
				return new BarrierCombatEvent(combatEvent, category, type, CombatEventState.NORMAL);
			default:
				return null;
			}
		}

		private CombatEvent GetStateChangeCombatEvent(CombatEvent combatEvent, CombatEventCategory category, CombatEventType type)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Expected I4, but got Unknown
			StateChange isStateChange = combatEvent.get_Ev().get_IsStateChange();
			return (isStateChange - 1) switch
			{
				0 => new EnterCombatEvent(combatEvent, category, type, CombatEventState.STATECHANGE), 
				1 => new ExitCombatEvent(combatEvent, category, type, CombatEventState.STATECHANGE), 
				_ => null, 
			};
		}

		private void AddSkill(CombatEvent combatEvent, string skillNameByArcDPS)
		{
			Skill skill2 = _skillState.GetBy((Skill skill) => skill.Id == (int)combatEvent.SkillId && skill.Name == skillNameByArcDPS);
			if (skill2 == null)
			{
				if (_skillState.AddMissingSkill((int)combatEvent.SkillId, skillNameByArcDPS))
				{
					Logger.Debug($"Failed to fetch skill \"{combatEvent.SkillId}\". ArcDPS reports: {skillNameByArcDPS}");
				}
				skill2 = SkillService.UnknownSkill;
			}
			else
			{
				_skillState.RemoveMissingSkill((int)combatEvent.SkillId);
			}
			combatEvent.Skill = skill2;
		}

		private void EmitEvent(CombatEvent combatEvent, CombatEventType scope)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Invalid comparison between Unknown and I4
			try
			{
				if ((int)scope != 0)
				{
					if ((int)scope == 1)
					{
						this.LocalCombatEvent?.Invoke(this, combatEvent);
					}
				}
				else
				{
					this.AreaCombatEvent?.Invoke(this, combatEvent);
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed emit event:");
			}
		}
	}
}
