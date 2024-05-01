using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.ArcDps.Models;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using HsAPI;
using Ideka.BHUDCommon;
using Ideka.NetCommon;

namespace Ideka.CustomCombatText
{
	public readonly struct Message
	{
		public readonly Ev Ev;

		public readonly Ag Src;

		public readonly Ag Dst;

		public readonly uint SkillId;

		public readonly string? SkillName;

		public readonly int? SkillIconId;

		public readonly Skill? Skill;

		public readonly Skill? HsSkill;

		public readonly (Palette palette, SlotGroup group, SkillInfo info)[] HsSkillInfo;

		public readonly Trait? Trait;

		public readonly HashSet<uint> PreviousSkillChainIds;

		public readonly bool IsBoonOrCondi;

		public readonly bool IsSelf;

		public readonly bool SrcIsPet;

		public readonly bool DstIsPet;

		public readonly bool IsOnTarget;

		public readonly bool IsFromTarget;

		private static readonly MessageContext Context = new MessageContext();

		public DateTime Timestamp { get; init; }

		public MessageCategory Category { get; init; }

		public EventResult Result { get; init; }

		public int Value { get; init; }

		public int Barrier { get; init; }

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

		private Message(CombatEvent cbt, MessageContext ctx)
		{
			//IL_0388: Unknown result type (might be due to invalid IL or missing references)
			CombatEvent cbt2 = cbt;
			Category = MessageCategory.PlayerOut;
			Result = EventResult.Strike;
			Value = 0;
			Barrier = 0;
			Timestamp = DateTime.UtcNow;
			Ev = cbt2.get_Ev();
			Src = cbt2.get_Src();
			Dst = cbt2.get_Dst();
			SkillId = (StaticData.SkillRedirects.TryGetValue(Ev.get_SkillId(), out var id) ? id : Ev.get_SkillId());
			Skill = (CTextModule.SkillData.Items.TryGetValue((int)SkillId, out var x4) ? x4 : null);
			HsSkill = (CTextModule.HsSkills.TryGetValue((int)SkillId, out var x3) ? x3 : null);
			uint skillId = SkillId;
			HsSkillInfo = HsSkill?.Palettes.SelectMany((int paletteId) => (!CTextModule.HsPalettes.TryGetValue(paletteId, out var palette)) ? Array.Empty<(Palette, SlotGroup, SkillInfo)>() : palette.Groups.SelectMany((SlotGroup group) => from info in @group.Candidates
				where info.Skill == skillId
				select (palette, @group, info))).ToArray() ?? Array.Empty<(Palette, SlotGroup, SkillInfo)>();
			PreviousSkillChainIds = new HashSet<uint>(((IEnumerable<(Palette, SlotGroup, SkillInfo)>)HsSkillInfo).Select((Func<(Palette, SlotGroup, SkillInfo), uint?>)delegate((Palette palette, SlotGroup group, SkillInfo info) x)
			{
				int? previousChainSkillIndex = x.info.PreviousChainSkillIndex;
				if (previousChainSkillIndex.HasValue)
				{
					int valueOrDefault = previousChainSkillIndex.GetValueOrDefault();
					SkillInfo skillInfo = x.group.Candidates.Skip(valueOrDefault).FirstOrDefault();
					if (skillInfo != null)
					{
						return (uint)skillInfo.Skill;
					}
				}
				return null;
			}).WhereNotNull());
			Trait = CTextModule.TraitData.Items.Values.FirstOrDefault((Trait x) => x.get_Name() == cbt2.get_SkillName() && CTextModule.SpecData.Items.TryGetValue(x.get_Specialization(), out var value) && value.get_Profession() == $"{(object)(ProfessionType)(byte)cbt2.get_Src().get_Profession()}");
			IsBoonOrCondi = StaticData.BoonAndCondi.Contains(SkillId);
			IsSelf = Src.get_Id() == Dst.get_Id() || (CTextModule.Settings.PetToMasterIsSelf.Value && Ev.get_SrcMasterInstId() == Ev.get_DstInstId()) || (CTextModule.Settings.MasterToPetIsSelf.Value && Ev.get_DstMasterInstId() == Ev.get_SrcInstId());
			SrcIsPet = Ev.get_SrcMasterInstId() == ctx.SelfInstId;
			DstIsPet = Ev.get_DstMasterInstId() == ctx.SelfInstId;
			IsOnTarget = Dst.get_Id() == ctx.TargetId;
			IsFromTarget = Src.get_Id() == ctx.TargetId;
			string rawName = ((cbt2.get_SkillName().All(char.IsDigit) || cbt2.get_SkillName() == "") ? null : cbt2.get_SkillName());
			Skill? skill = Skill;
			string name = ((skill != null) ? skill!.get_Name() : null) ?? rawName ?? HsSkill?.Name;
			SkillName = (string.IsNullOrEmpty(name) ? null : name);
			int? skillIconId;
			if (!StaticData.IconOverrides.TryGetValue(SkillId, out var x2))
			{
				Skill? skill2 = Skill;
				int? num = ApiCache.TryExtractAssetId((skill2 != null) ? skill2!.get_Icon() : null);
				if (!num.HasValue)
				{
					int? num2 = HsSkill?.Icon;
					if (!num2.HasValue)
					{
						Trait? trait = Trait;
						skillIconId = ApiCache.TryExtractAssetId((trait != null) ? new RenderUrl?(trait!.get_Icon()) : null);
					}
					else
					{
						skillIconId = num2;
					}
				}
				else
				{
					skillIconId = num;
				}
			}
			else
			{
				skillIconId = x2;
			}
			SkillIconId = skillIconId;
		}

		public bool CanMerge(Message other)
		{
			if ((SkillId == other.SkillId || (CTextModule.Settings.MergeAttackChains.Value && other.PreviousSkillChainIds.Contains(Ev.get_SkillId()) && Src.get_Id() == other.Src.get_Id())) && (Category == other.Category || (IsOut && other.IsOut)))
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
			return false;
		}

		public static LogEntry Log(byte[] raw)
		{
			return new LogEntry(raw, Context.Clone());
		}

		public static IEnumerable<Message> Interpret(CombatEvent cbt, MessageContext? ctx = null)
		{
			if (ctx == null)
			{
				ctx = Context;
			}
			if (cbt.get_Ev() == null)
			{
				Ag src2 = cbt.get_Src();
				if (src2 != null && src2.get_Elite() == 1)
				{
					ctx!.TargetId = cbt.get_Src().get_Id();
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
				ctx!.SelfId = cbt.get_Src().get_Id();
				ctx!.SelfInstId = ev.get_SrcInstId();
			}
			Ag dst2 = cbt.get_Dst();
			if (dst2 != null && dst2.get_Self() == 1)
			{
				ctx!.SelfId = cbt.get_Dst().get_Id();
				ctx!.SelfInstId = ev.get_DstInstId();
			}
			Ag src4 = cbt.get_Src();
			if (((src4 != null) ? new ulong?(src4.get_Id()) : null) == ctx!.TargetId)
			{
				ctx!.TargetInstId = ev.get_SrcInstId();
			}
			Ag dst3 = cbt.get_Dst();
			if (((dst3 != null) ? new ulong?(dst3.get_Id()) : null) == ctx!.TargetId)
			{
				ctx!.TargetInstId = ev.get_DstInstId();
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
					value /= 10;
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
			else if (ev.get_SrcMasterInstId() == ctx!.SelfInstId)
			{
				categories.Add(MessageCategory.PetOut);
			}
			if (dst.get_Self() == 1)
			{
				categories.Add(MessageCategory.PlayerIn);
			}
			else if (ev.get_DstMasterInstId() == ctx!.SelfInstId)
			{
				categories.Add(MessageCategory.PetIn);
			}
			foreach (EventResult result in results)
			{
				int effectiveValue = ((result == EventResult.Barrier) ? barrier : value);
				int effectiveBarrier = ((result != EventResult.Barrier) ? barrier : 0);
				foreach (MessageCategory category in categories)
				{
					yield return new Message(cbt, ctx)
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
