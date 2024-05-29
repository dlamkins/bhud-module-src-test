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
	public class Message
	{
		public readonly Ev Ev;

		public readonly Ag Src;

		public readonly Ag Dst;

		public readonly int SkillId;

		public readonly string? SkillName;

		public readonly int? SkillIconId;

		public readonly Skill? Skill;

		public readonly Skill? HsSkill;

		public readonly (Palette palette, SlotGroup group, SkillInfo info)[] HsInfo;

		public readonly Trait? Trait;

		public readonly HashSet<uint> PreviousSkillChainIds;

		public readonly bool IsBoonOrCondi;

		public readonly bool IsSelf;

		public readonly bool SrcIsPet;

		public readonly bool DstIsPet;

		public readonly bool IsOnTarget;

		public readonly bool IsFromTarget;

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

		public Message(CombatEvent cbt, MessageContext ctx)
		{
			//IL_0359: Unknown result type (might be due to invalid IL or missing references)
			CombatEvent cbt2 = cbt;
			base._002Ector();
			Message message = this;
			Ev = cbt2.get_Ev();
			Src = cbt2.get_Src();
			Dst = cbt2.get_Dst();
			SkillId = (StaticData.SkillRedirects.TryGetValue((int)Ev.get_SkillId(), out var id) ? id : ((int)Ev.get_SkillId()));
			Skill = (CTextModule.SkillData.Items.TryGetValue(SkillId, out var x4) ? x4 : null);
			HsSkill = (CTextModule.HsSkills.TryGetValue(SkillId, out var x3) ? x3 : null);
			HsInfo = HsSkill?.Palettes.SelectMany((int paletteId) => (!CTextModule.HsPalettes.TryGetValue(paletteId, out var palette)) ? Array.Empty<(Palette, SlotGroup, SkillInfo)>() : palette.Groups.SelectMany((SlotGroup group) => from info in @group.Candidates
				where info.Skill == message.SkillId
				select (palette, @group, info))).ToArray() ?? Array.Empty<(Palette, SlotGroup, SkillInfo)>();
			PreviousSkillChainIds = new HashSet<uint>(((IEnumerable<(Palette, SlotGroup, SkillInfo)>)HsInfo).Select((Func<(Palette, SlotGroup, SkillInfo), uint?>)delegate((Palette palette, SlotGroup group, SkillInfo info) x)
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
	}
}