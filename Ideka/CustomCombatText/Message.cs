using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.GameServices.ArcDps.V2.Models;
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
		public readonly CombatEvent Ev;

		public readonly Agent Src;

		public readonly Agent Dst;

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

		public Message(CombatCallback cbt, MessageContext ctx)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0395: Unknown result type (might be due to invalid IL or missing references)
			Message message = this;
			Ev = ((CombatCallback)(ref cbt)).get_Event();
			Src = ((CombatCallback)(ref cbt)).get_Source();
			Dst = ((CombatCallback)(ref cbt)).get_Destination();
			SkillId = (StaticData.SkillRedirects.TryGetValue((int)((CombatEvent)(ref Ev)).get_SkillId(), out var id) ? id : ((int)((CombatEvent)(ref Ev)).get_SkillId()));
			Skill = (CTextModule.SkillData.Items.TryGetValue(SkillId, out var x4) ? x4 : null);
			HsSkill = (CTextModule.HsSkillData.Items.TryGetValue(SkillId, out var x3) ? x3 : null);
			HsInfo = HsSkill?.Palettes.SelectMany((int paletteId) => (!CTextModule.HsPaletteData.Items.TryGetValue(paletteId, out var palette)) ? Array.Empty<(Palette, SlotGroup, SkillInfo)>() : palette.Groups.SelectMany((SlotGroup group) => from info in @group.Candidates
				where info.Skill == message.SkillId
				select (palette, @group, info))).ToArray() ?? Array.Empty<(Palette, SlotGroup, SkillInfo)>();
			HashSet<uint> hashSet = new HashSet<uint>();
			foreach (uint item in ((IEnumerable<(Palette, SlotGroup, SkillInfo)>)HsInfo).Select((Func<(Palette, SlotGroup, SkillInfo), uint?>)delegate((Palette palette, SlotGroup group, SkillInfo info) x)
			{
				int? previousChainSkillIndex = x.info.PreviousChainSkillIndex;
				if (previousChainSkillIndex.HasValue)
				{
					int valueOrDefault = previousChainSkillIndex.GetValueOrDefault();
					SkillInfo skillInfo = x.group.Candidates.ElementAtOrDefault(valueOrDefault);
					if (skillInfo != null)
					{
						return (uint)skillInfo.Skill;
					}
				}
				return null;
			}).WhereNotNull())
			{
				hashSet.Add(item);
			}
			PreviousSkillChainIds = hashSet;
			Trait = CTextModule.TraitData.Items.Values.FirstOrDefault(delegate(Trait x)
			{
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				if (x.get_Name() == ((CombatCallback)(ref cbt)).get_SkillName() && CTextModule.SpecData.Items.TryGetValue(x.get_Specialization(), out var value))
				{
					string profession = value.get_Profession();
					Agent source = ((CombatCallback)(ref cbt)).get_Source();
					return profession == $"{(object)(ProfessionType)(byte)((Agent)(ref source)).get_Profession()}";
				}
				return false;
			});
			IsBoonOrCondi = StaticData.BoonAndCondi.Contains(SkillId);
			IsSelf = ((Agent)(ref Src)).get_Id() == ((Agent)(ref Dst)).get_Id() || (CTextModule.Settings.PetToMasterIsSelf.Value && ((CombatEvent)(ref Ev)).get_SourceMasterInstanceId() == ((CombatEvent)(ref Ev)).get_DestinationInstanceId()) || (CTextModule.Settings.MasterToPetIsSelf.Value && ((CombatEvent)(ref Ev)).get_DestinationMasterInstanceId() == ((CombatEvent)(ref Ev)).get_SourceInstanceId());
			SrcIsPet = ((CombatEvent)(ref Ev)).get_SourceMasterInstanceId() == ctx.SelfInstId;
			DstIsPet = ((CombatEvent)(ref Ev)).get_DestinationMasterInstanceId() == ctx.SelfInstId;
			IsOnTarget = ((Agent)(ref Dst)).get_Id() == ctx.TargetId;
			IsFromTarget = ((Agent)(ref Src)).get_Id() == ctx.TargetId;
			string rawName = ((((CombatCallback)(ref cbt)).get_SkillName().All(char.IsDigit) || ((CombatCallback)(ref cbt)).get_SkillName() == "") ? null : ((CombatCallback)(ref cbt)).get_SkillName());
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
			if ((SkillId == other.SkillId || (CTextModule.Settings.MergeAttackChains.Value && other.PreviousSkillChainIds.Contains(((CombatEvent)(ref Ev)).get_SkillId()) && ((Agent)(ref Src)).get_Id() == ((Agent)(ref other.Src)).get_Id())) && (Category == other.Category || (IsOut && other.IsOut)))
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
