using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.ArcDps;
using Blish_HUD.ArcDps.Models;
using Gw2Sharp.WebApi.V2.Models;

namespace Ideka.CustomCombatText
{
	public readonly struct Message
	{
		public readonly Ev Ev;

		public readonly Ag Src;

		public readonly Ag Dst;

		public readonly string? SkillName;

		public readonly Skill? Skill;

		public readonly Trait? Trait;

		public readonly SkillFallback? SkillFallback;

		public readonly bool IsSelf;

		public readonly bool SrcIsPet;

		public readonly bool DstIsPet;

		public readonly bool IsOnTarget;

		public readonly bool IsFromTarget;

		public MessageCategory Category { get; init; }

		public EventResult Result { get; init; }

		public int Value { get; init; }

		public int Barrier { get; init; }

		private static ulong SelfId { get; set; }

		private static ushort SelfInstId { get; set; }

		private static ulong TargetId { get; set; } = ulong.MaxValue;


		private static ushort TargetInstId { get; set; } = ushort.MaxValue;


		public bool LandedStrike
		{
			get
			{
				EventResult result = Result;
				if ((uint)result <= 2u)
				{
					return true;
				}
				return false;
			}
		}

		public bool MissedStrike
		{
			get
			{
				EventResult result = Result;
				if ((uint)(result - 3) <= 3u)
				{
					return true;
				}
				return false;
			}
		}

		public bool LandedCondi
		{
			get
			{
				EventResult result = Result;
				if ((uint)(result - 7) <= 4u)
				{
					return true;
				}
				return false;
			}
		}

		public bool LandedOtherDoT => Result == EventResult.DamageTick;

		public bool LandedDamage
		{
			get
			{
				if (!LandedStrike && !LandedCondi)
				{
					return LandedOtherDoT;
				}
				return true;
			}
		}

		public bool IsOut
		{
			get
			{
				MessageCategory category = Category;
				if (category == MessageCategory.PlayerOut || category == MessageCategory.PetOut)
				{
					return true;
				}
				return false;
			}
		}

		public bool IsIn
		{
			get
			{
				MessageCategory category = Category;
				if (category == MessageCategory.PlayerIn || category == MessageCategory.PetIn)
				{
					return true;
				}
				return false;
			}
		}

		public Message(CombatEvent cbt, ulong targetId)
		{
			CombatEvent cbt2 = cbt;
			Category = MessageCategory.PlayerOut;
			Result = EventResult.Strike;
			Value = 0;
			Barrier = 0;
			Ev = cbt2.get_Ev();
			Src = cbt2.get_Src();
			Dst = cbt2.get_Dst();
			SkillName = (cbt2.get_SkillName().All(char.IsDigit) ? null : cbt2.get_SkillName());
			Skill = (CTextModule.SkillData.Items.TryGetValue((int)cbt2.get_Ev().get_SkillId(), out var skill) ? skill : null);
			Trait = CTextModule.TraitData.Items.Values.FirstOrDefault((Trait x) => x.get_Name() == cbt2.get_SkillName());
			SkillFallback = (CTextModule.SkillFallbacks.TryGetValue((int)cbt2.get_Ev().get_SkillId(), out var fallback) ? fallback : null);
			IsSelf = cbt2.get_Src().get_Id() == cbt2.get_Dst().get_Id() || (CTextModule.Settings.PetToMasterIsSelf.Value && cbt2.get_Ev().get_SrcMasterInstId() == cbt2.get_Ev().get_DstInstId()) || (CTextModule.Settings.MasterToPetIsSelf.Value && cbt2.get_Ev().get_DstMasterInstId() == cbt2.get_Ev().get_SrcInstId());
			SrcIsPet = cbt2.get_Ev().get_SrcMasterInstId() == SelfInstId;
			DstIsPet = cbt2.get_Ev().get_DstMasterInstId() == SelfInstId;
			IsOnTarget = cbt2.get_Dst().get_Id() == targetId;
			IsFromTarget = cbt2.get_Src().get_Id() == targetId;
		}

		public bool CanMerge(Message other)
		{
			if (Ev.get_SkillId() == other.Ev.get_SkillId())
			{
				goto IL_0083;
			}
			if (CTextModule.Settings.MergeAttackChains.Value)
			{
				Skill? skill = Skill;
				int? num = ((skill != null) ? skill!.get_NextChain() : null);
				if (num.HasValue)
				{
					int next = num.GetValueOrDefault();
					if (next == other.Ev.get_SkillId() && Src.get_Id() == other.Src.get_Id())
					{
						goto IL_0083;
					}
				}
			}
			goto IL_00d7;
			IL_0083:
			if (Category == other.Category || (IsOut && other.IsOut))
			{
				if (Result != other.Result && (!LandedStrike || !other.LandedStrike))
				{
					if (MissedStrike)
					{
						return other.MissedStrike;
					}
					return false;
				}
				return true;
			}
			goto IL_00d7;
			IL_00d7:
			return false;
		}

		public static CombatEvent CreateEvent(ulong srcId, ulong dstId, ushort srcInstId, ushort dstInstId, bool friendly, bool buff, int value, int barrier, uint skillId, PhysicalResult result)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Expected I4, but got Unknown
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Expected O, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Expected O, but got Unknown
			Ag src = new Ag("Test Source", srcId, 0u, 0u, (srcId == SelfId) ? 1u : 0u, (ushort)0);
			Ag dst = new Ag("Test Target", dstId, 0u, 0u, (dstId == SelfId) ? 1u : 0u, (!friendly) ? ((ushort)1) : ((ushort)0));
			return new CombatEvent(new Ev(0uL, src.get_Id(), dst.get_Id(), (!buff) ? value : 0, buff ? value : 0, (uint)barrier, skillId, srcInstId, dstInstId, (ushort)0, (ushort)0, (IFF)((!friendly) ? 1 : 0), false, (byte)(int)result, (Activation)0, (BuffRemove)0, false, false, false, (StateChange)0, false, false, false, (byte)0, (byte)0, (byte)0, (byte)0), src, dst, "Test Skill", 0uL, 1uL);
		}

		public static IEnumerable<Message> Test()
		{
			return Interpret(CreateEvent(SelfId, TargetId, SelfInstId, TargetInstId, friendly: false, buff: false, -1234, -7890, 14502u, (PhysicalResult)0));
		}

		public static IEnumerable<Message> Interpret(CombatEvent cbt)
		{
			if (cbt.get_Ev() == null)
			{
				Ag src2 = cbt.get_Src();
				if (src2 != null && src2.get_Elite() == 1)
				{
					TargetId = cbt.get_Src().get_Id();
				}
			}
			Ev ev = cbt.get_Ev();
			if (ev == null)
			{
				yield break;
			}
			Ag src3 = cbt.get_Src();
			if (src3 != null && src3.get_Self() == 1)
			{
				SelfId = cbt.get_Src().get_Id();
				SelfInstId = ev.get_SrcInstId();
			}
			Ag dst2 = cbt.get_Dst();
			if (dst2 != null && dst2.get_Self() == 1)
			{
				SelfId = cbt.get_Dst().get_Id();
				SelfInstId = ev.get_DstInstId();
			}
			Ag src4 = cbt.get_Src();
			if (((src4 != null) ? new ulong?(src4.get_Id()) : null) == TargetId)
			{
				TargetInstId = ev.get_SrcInstId();
			}
			Ag dst3 = cbt.get_Dst();
			if (((dst3 != null) ? new ulong?(dst3.get_Id()) : null) == TargetId)
			{
				TargetInstId = ev.get_DstInstId();
			}
			Ag dst = cbt.get_Dst();
			if (dst == null)
			{
				yield break;
			}
			Ag src = cbt.get_Src();
			if (src == null || (int)ev.get_IsStateChange() != 0 || (int)ev.get_IsActivation() != 0 || (int)ev.get_IsBuffRemove() != 0)
			{
				yield break;
			}
			HashSet<EventResult> results = new HashSet<EventResult>();
			int barrier = (int)ev.get_OverStackValue();
			int value;
			if (ev.get_Buff())
			{
				value = ev.get_BuffDmg();
				if (value > 0)
				{
					if (barrier > 0)
					{
						value -= barrier;
						results.Add(EventResult.Barrier);
					}
					if (value > 0)
					{
						results.Add(EventResult.HealTick);
					}
				}
				else if (value < 0)
				{
					if (barrier < 0)
					{
						value -= barrier;
					}
					if (barrier <= 0 || value < 0)
					{
						switch (ev.get_SkillId())
						{
						case 736u:
							results.Add(EventResult.Bleeding);
							break;
						case 737u:
							results.Add(EventResult.Burning);
							break;
						case 723u:
							results.Add(EventResult.Poison);
							break;
						case 861u:
							results.Add(EventResult.Confusion);
							break;
						case 19426u:
							results.Add(EventResult.Torment);
							break;
						default:
							results.Add(EventResult.DamageTick);
							break;
						}
					}
				}
			}
			else
			{
				value = ev.get_Value();
				if (ev.get_Result() == 10)
				{
					if (ev.get_SkillId() != 23276)
					{
						results.Add(EventResult.Breakbar);
					}
				}
				else if (value > 0)
				{
					if (barrier > 0)
					{
						value -= barrier;
						results.Add(EventResult.Barrier);
					}
					if (value > 0)
					{
						results.Add(EventResult.Heal);
					}
				}
				else
				{
					if (barrier < 0)
					{
						value -= barrier;
					}
					if (barrier <= 0 || value < 0)
					{
						switch (ev.get_Result())
						{
						case 0:
							results.Add(EventResult.Strike);
							break;
						case 1:
							results.Add(EventResult.Crit);
							break;
						case 2:
							results.Add(EventResult.Glance);
							break;
						case 3:
							results.Add(EventResult.Block);
							break;
						case 4:
							results.Add(EventResult.Evade);
							break;
						case 6:
							results.Add(EventResult.Invuln);
							break;
						case 7:
							results.Add(EventResult.Miss);
							break;
						case 5:
							results.Add(EventResult.Interrupt);
							break;
						}
					}
				}
			}
			HashSet<MessageCategory> categories = new HashSet<MessageCategory>();
			if (src.get_Self() == 1)
			{
				categories.Add(MessageCategory.PlayerOut);
			}
			else if (ev.get_SrcMasterInstId() == SelfInstId)
			{
				categories.Add(MessageCategory.PetOut);
			}
			if (dst.get_Self() == 1)
			{
				categories.Add(MessageCategory.PlayerIn);
			}
			else if (ev.get_DstMasterInstId() == SelfInstId)
			{
				categories.Add(MessageCategory.PetIn);
			}
			foreach (EventResult result in results)
			{
				int effectiveValue = ((result == EventResult.Barrier) ? barrier : value);
				int effectiveBarrier = ((result != EventResult.Barrier) ? barrier : 0);
				foreach (MessageCategory category in categories)
				{
					yield return new Message(cbt, TargetId)
					{
						Category = category,
						Result = result,
						Value = Math.Abs(effectiveValue),
						Barrier = Math.Abs(effectiveBarrier)
					};
				}
			}
		}
	}
}
