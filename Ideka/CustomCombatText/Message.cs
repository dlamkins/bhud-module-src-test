using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.ArcDps;
using Blish_HUD.ArcDps.Models;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Ideka.BHUDCommon;

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

		public readonly HsSkill? HsSkill;

		public readonly Trait? Trait;

		public readonly bool IsBoonOrCondi;

		public readonly bool IsSelf;

		public readonly bool SrcIsPet;

		public readonly bool DstIsPet;

		public readonly bool IsOnTarget;

		public readonly bool IsFromTarget;

		private static readonly HashSet<uint> BoonAndCondi = new HashSet<uint>
		{
			717u, 718u, 719u, 720u, 721u, 722u, 723u, 725u, 726u, 727u,
			736u, 737u, 738u, 740u, 742u, 743u, 791u, 861u, 873u, 1122u,
			1187u, 19426u, 26766u, 26980u, 27705u, 30328u, 17495u, 17674u, 21632u
		};

		private static readonly Dictionary<uint, uint> SkillRedirects = new Dictionary<uint, uint>
		{
			[1066u] = 1006u,
			[42133u] = 40409u,
			[41993u] = 45994u,
			[45515u] = 41253u,
			[44615u] = 41253u,
			[41698u] = 46089u,
			[42148u] = 46089u,
			[40277u] = 41192u,
			[50913u] = 51040u,
			[55716u] = 55536u,
			[70087u] = 70431u,
			[54922u] = 54912u,
			[63410u] = 63475u,
			[64148u] = 65179u
		};

		private static readonly Dictionary<uint, int> IconOverrides = new Dictionary<uint, int>
		{
			[1006u] = 1234942,
			[1286u] = 1770562,
			[1421u] = 103021,
			[2064u] = 103260,
			[3647u] = 2207786,
			[3652u] = 591891,
			[3822u] = 103149,
			[3828u] = 2261529,
			[3832u] = 103149,
			[3835u] = 103985,
			[3847u] = 2261529,
			[5703u] = 65735,
			[5767u] = 102780,
			[5840u] = 102982,
			[5903u] = 103410,
			[5981u] = 103413,
			[6852u] = 603547,
			[9121u] = 103202,
			[9170u] = 103669,
			[9183u] = 103673,
			[9284u] = 220678,
			[9428u] = 220689,
			[9433u] = 220687,
			[9458u] = 220681,
			[9586u] = 220698,
			[9604u] = 220709,
			[10228u] = 103722,
			[10266u] = 103764,
			[10277u] = 103766,
			[10281u] = 103767,
			[10294u] = 103615,
			[10295u] = 103770,
			[10296u] = 103100,
			[10297u] = 103188,
			[10298u] = 103189,
			[10299u] = 103584,
			[10330u] = 103787,
			[10338u] = 103552,
			[10573u] = 103852,
			[10676u] = 103852,
			[12597u] = 104016,
			[12598u] = 104018,
			[12599u] = 104019,
			[12600u] = 104017,
			[12601u] = 104020,
			[12810u] = 255247,
			[12812u] = 255247,
			[12815u] = 255247,
			[12825u] = 156662,
			[12826u] = 156662,
			[12828u] = 255247,
			[12831u] = 255247,
			[12836u] = 156662,
			[12837u] = 255247,
			[12853u] = 255247,
			[12854u] = 255247,
			[12855u] = 255247,
			[12856u] = 255247,
			[12858u] = 255247,
			[12859u] = 255247,
			[12860u] = 255247,
			[12861u] = 156662,
			[13515u] = 1012374,
			[13594u] = 1012412,
			[13721u] = 1012523,
			[13752u] = 1012494,
			[13814u] = 1012544,
			[13824u] = 2310020,
			[13980u] = 1012635,
			[14024u] = 1012611,
			[14282u] = 2207780,
			[14605u] = 104145,
			[14613u] = 102812,
			[14618u] = 104146,
			[14622u] = 104147,
			[14626u] = 103867,
			[14643u] = 104149,
			[14650u] = 104153,
			[14651u] = 104153,
			[14655u] = 104156,
			[14658u] = 104157,
			[14659u] = 104158,
			[14665u] = 103873,
			[14676u] = 102812,
			[14713u] = 347249,
			[15266u] = 499379,
			[16711u] = 104149,
			[18526u] = 103867,
			[18529u] = 104157,
			[18531u] = 104157,
			[18533u] = 104157,
			[18535u] = 103867,
			[18537u] = 103867,
			[18539u] = 104158,
			[18541u] = 104158,
			[18543u] = 104158,
			[18564u] = 104156,
			[18568u] = 104155,
			[18570u] = 104147,
			[18574u] = 104147,
			[18576u] = 104156,
			[18578u] = 104155,
			[18846u] = 104154,
			[18848u] = 104153,
			[18850u] = 104146,
			[18853u] = 104146,
			[18855u] = 104153,
			[18857u] = 104154,
			[18860u] = 104154,
			[18862u] = 104153,
			[18865u] = 104153,
			[18867u] = 104146,
			[18869u] = 104154,
			[18872u] = 575655,
			[18881u] = 104152,
			[18887u] = 104145,
			[19602u] = 103871,
			[19604u] = 103870,
			[19626u] = 103870,
			[20242u] = 104144,
			[20243u] = 104149,
			[20254u] = 104144,
			[20272u] = 104144,
			[20273u] = 104149,
			[20462u] = 220684,
			[20479u] = 619715,
			[20890u] = 102640,
			[20979u] = 102812,
			[20983u] = 102812,
			[20987u] = 102812,
			[20992u] = 102812,
			[21001u] = 626133,
			[21005u] = 102812,
			[21010u] = 103873,
			[21020u] = 103873,
			[21029u] = 103873,
			[21034u] = 102812,
			[21038u] = 103873,
			[21471u] = 103183,
			[21474u] = 102812,
			[21475u] = 103183,
			[21479u] = 638736,
			[21636u] = 1012280,
			[21765u] = 699529,
			[21776u] = 699531,
			[21795u] = 1012443,
			[22132u] = 103426,
			[22137u] = 103110,
			[22492u] = 104109,
			[22499u] = 1012457,
			[24061u] = 1012328,
			[24241u] = 220684,
			[24244u] = 220684,
			[25595u] = 347247,
			[26977u] = 1013015,
			[27028u] = 2029284,
			[28313u] = 1012989,
			[28388u] = 961449,
			[29168u] = 2128017,
			[29210u] = 2128016,
			[29236u] = 2128013,
			[29599u] = 103034,
			[29604u] = 1012571,
			[29799u] = 699529,
			[29863u] = 1128540,
			[29901u] = 1012537,
			[29997u] = 1012533,
			[30176u] = 1128595,
			[30235u] = 1012852,
			[30301u] = 156662,
			[30316u] = 1012511,
			[30319u] = 156662,
			[30398u] = 1012539,
			[30539u] = 1012573,
			[30564u] = 1012370,
			[30581u] = 2983334,
			[30784u] = 1012638,
			[31187u] = 1058553,
			[31267u] = 1058552,
			[31289u] = 1058558,
			[31354u] = 1012570,
			[31371u] = 1128621,
			[31436u] = 607767,
			[31629u] = 1128531,
			[31640u] = 1128639,
			[31657u] = 1128634,
			[31686u] = 1029987,
			[31707u] = 1128531,
			[31847u] = 2175057,
			[31864u] = 1012889,
			[33142u] = 961476,
			[40071u] = 1770536,
			[40306u] = 3098876,
			[40774u] = 1770534,
			[40787u] = 1770479,
			[41166u] = 102820,
			[41897u] = 500243,
			[42145u] = 1770558,
			[42264u] = 103048,
			[43260u] = 1769970,
			[43470u] = 102801,
			[43485u] = 1770539,
			[43558u] = 1012665,
			[43630u] = 1769933,
			[43759u] = 1770538,
			[44257u] = 1770480,
			[44857u] = 102870,
			[44947u] = 1770018,
			[45534u] = 1770015,
			[46040u] = 1770537,
			[46299u] = 1769977,
			[46470u] = 103101,
			[46808u] = 1770537,
			[46821u] = 1770536,
			[46824u] = 1770538,
			[48272u] = 1228831,
			[49077u] = 1938794,
			[49084u] = 1128528,
			[49103u] = 699528,
			[52370u] = 2039809,
			[52973u] = 2039808,
			[53183u] = 1012533,
			[53337u] = 220696,
			[53406u] = 220706,
			[53471u] = 527173,
			[54935u] = 1128595,
			[54953u] = 2110152,
			[54958u] = 2110169,
			[55047u] = 2128013,
			[56911u] = 2175063,
			[57844u] = 2199298,
			[59510u] = 103820,
			[59595u] = 1012682,
			[60618u] = 699529,
			[62558u] = 2479346,
			[62660u] = 2479344,
			[62671u] = 2479345,
			[62863u] = 2491541,
			[62883u] = 2491654,
			[63173u] = 2503627,
			[63300u] = 2503676,
			[63891u] = 2604865,
			[67415u] = 2479385,
			[68063u] = 1013018,
			[68100u] = 1769925,
			[68216u] = 104142,
			[68679u] = 103262,
			[69253u] = 104019,
			[69289u] = 104017
		};

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

		private Message(CombatEvent cbt, ulong targetId)
		{
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			CombatEvent cbt2 = cbt;
			Category = MessageCategory.PlayerOut;
			Result = EventResult.Strike;
			Value = 0;
			Barrier = 0;
			Ev = cbt2.get_Ev();
			Src = cbt2.get_Src();
			Dst = cbt2.get_Dst();
			SkillId = (SkillRedirects.TryGetValue(Ev.get_SkillId(), out var id) ? id : Ev.get_SkillId());
			Skill = (CTextModule.SkillData.Items.TryGetValue((int)SkillId, out var x4) ? x4 : null);
			HsSkill = (CTextModule.HsSkills.TryGetValue(SkillId, out var x3) ? x3 : null);
			Trait = CTextModule.TraitData.Items.Values.FirstOrDefault((Trait x) => x.get_Name() == cbt2.get_SkillName() && CTextModule.SpecData.Items.TryGetValue(x.get_Specialization(), out var value) && value.get_Profession() == $"{(object)(ProfessionType)(byte)cbt2.get_Src().get_Profession()}");
			IsBoonOrCondi = BoonAndCondi.Contains(SkillId);
			IsSelf = Src.get_Id() == Dst.get_Id() || (CTextModule.Settings.PetToMasterIsSelf.Value && Ev.get_SrcMasterInstId() == Ev.get_DstInstId()) || (CTextModule.Settings.MasterToPetIsSelf.Value && Ev.get_DstMasterInstId() == Ev.get_SrcInstId());
			SrcIsPet = Ev.get_SrcMasterInstId() == SelfInstId;
			DstIsPet = Ev.get_DstMasterInstId() == SelfInstId;
			IsOnTarget = Dst.get_Id() == targetId;
			IsFromTarget = Src.get_Id() == targetId;
			string rawName = ((cbt2.get_SkillName().All(char.IsDigit) || cbt2.get_SkillName() == "") ? null : cbt2.get_SkillName());
			Skill? skill = Skill;
			string name = ((skill != null) ? skill!.get_Name() : null) ?? rawName ?? HsSkill?.Name;
			SkillName = (string.IsNullOrEmpty(name) ? null : name);
			int? skillIconId;
			if (!IconOverrides.TryGetValue(SkillId, out var x2))
			{
				Skill? skill2 = Skill;
				int? num = ApiCache.TryExtractAssetId((skill2 != null) ? skill2!.get_Icon() : null);
				if (!num.HasValue)
				{
					int y;
					int? num2 = (int.TryParse(HsSkill?.Icon, out y) ? new int?(y) : null);
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
			if (SkillId == other.SkillId)
			{
				goto IL_0071;
			}
			if (CTextModule.Settings.MergeAttackChains.Value)
			{
				Skill? skill = Skill;
				int? num = ((skill != null) ? skill!.get_NextChain() : null);
				if (num.HasValue)
				{
					int next = num.GetValueOrDefault();
					if (next == other.SkillId && Src.get_Id() == other.Src.get_Id())
					{
						goto IL_0071;
					}
				}
			}
			goto IL_00c5;
			IL_0071:
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
			goto IL_00c5;
			IL_00c5:
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
