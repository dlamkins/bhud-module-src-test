using System;
using System.Collections.Generic;
using Gw2Sharp.ChatLinks;
using Gw2Sharp.Models;

namespace Kenedia.Modules.BuildsManager
{
	public class BuildTemplate
	{
		private string _TemplateCode;

		public API.Profession Profession;

		public List<SpecLine> SpecLines = new List<SpecLine>
		{
			new SpecLine
			{
				Index = 0
			},
			new SpecLine
			{
				Index = 1
			},
			new SpecLine
			{
				Index = 2
			}
		};

		public List<API.Skill> Skills_Terrestrial = new List<API.Skill>
		{
			new API.Skill
			{
				PaletteId = 4572
			},
			new API.Skill
			{
				PaletteId = 4614
			},
			new API.Skill
			{
				PaletteId = 4651
			},
			new API.Skill
			{
				PaletteId = 4564
			},
			new API.Skill
			{
				PaletteId = 4554
			}
		};

		public List<API.Skill> InactiveSkills_Terrestrial = new List<API.Skill>
		{
			new API.Skill
			{
				PaletteId = 4572
			},
			new API.Skill
			{
				PaletteId = 4614
			},
			new API.Skill
			{
				PaletteId = 4651
			},
			new API.Skill
			{
				PaletteId = 4564
			},
			new API.Skill
			{
				PaletteId = 4554
			}
		};

		public List<API.Skill> Skills_Aquatic = new List<API.Skill>
		{
			new API.Skill
			{
				PaletteId = 4572
			},
			new API.Skill
			{
				PaletteId = 4614
			},
			new API.Skill
			{
				PaletteId = 4651
			},
			new API.Skill
			{
				PaletteId = 4564
			},
			new API.Skill
			{
				PaletteId = 4554
			}
		};

		public List<API.Skill> InactiveSkills_Aquatic = new List<API.Skill>
		{
			new API.Skill
			{
				PaletteId = 4572
			},
			new API.Skill
			{
				PaletteId = 4614
			},
			new API.Skill
			{
				PaletteId = 4651
			},
			new API.Skill
			{
				PaletteId = 4564
			},
			new API.Skill
			{
				PaletteId = 4554
			}
		};

		public List<API.Legend> Legends_Terrestrial = new List<API.Legend>
		{
			new API.Legend(),
			new API.Legend()
		};

		public List<API.Legend> Legends_Aquatic = new List<API.Legend>
		{
			new API.Legend(),
			new API.Legend()
		};

		public EventHandler Changed;

		public string TemplateCode
		{
			get
			{
				_TemplateCode = ParseBuildCode();
				return _TemplateCode;
			}
			private set
			{
				_TemplateCode = value;
			}
		}

		public string ParseBuildCode()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Invalid comparison between Unknown and I4
			string code = "";
			BuildChatLink build;
			bool rev;
			int? obj;
			if (Profession != null)
			{
				build = new BuildChatLink();
				build.set_Profession((ProfessionType)Enum.Parse(typeof(ProfessionType), Profession.Id));
				rev = (int)build.get_Profession() == 9;
				if (rev)
				{
					API.Legend legend = Legends_Terrestrial[0];
					int num;
					if (legend == null)
					{
						num = 0;
					}
					else
					{
						_ = legend.Id;
						num = 1;
					}
					if (num != 0)
					{
						obj = Legends_Terrestrial[0]?.Id;
						goto IL_0095;
					}
				}
				obj = 0;
				goto IL_0095;
			}
			goto IL_09fb;
			IL_03da:
			int? obj2;
			int? num2 = (int?)obj2;
			build.set_RevenantInactiveAquaticUtility3SkillPaletteId((ushort)num2.Value);
			API.Skill skill = Skills_Terrestrial[0];
			build.set_TerrestrialHealingSkillPaletteId((ushort)((skill == null || skill.Id <= 0) ? new int?(0) : Skills_Terrestrial[0]?.PaletteId).Value);
			API.Skill skill2 = Skills_Terrestrial[1];
			build.set_TerrestrialUtility1SkillPaletteId((ushort)((skill2 == null || skill2.Id <= 0) ? new int?(0) : Skills_Terrestrial[1]?.PaletteId).Value);
			API.Skill skill3 = Skills_Terrestrial[2];
			build.set_TerrestrialUtility2SkillPaletteId((ushort)((skill3 == null || skill3.Id <= 0) ? new int?(0) : Skills_Terrestrial[2]?.PaletteId).Value);
			API.Skill skill4 = Skills_Terrestrial[3];
			build.set_TerrestrialUtility3SkillPaletteId((ushort)((skill4 == null || skill4.Id <= 0) ? new int?(0) : Skills_Terrestrial[3]?.PaletteId).Value);
			API.Skill skill5 = Skills_Terrestrial[4];
			build.set_TerrestrialEliteSkillPaletteId((ushort)((skill5 == null || skill5.Id <= 0) ? new int?(0) : Skills_Terrestrial[4]?.PaletteId).Value);
			API.Skill skill6 = Skills_Aquatic[0];
			build.set_AquaticHealingSkillPaletteId((ushort)((skill6 == null || skill6.Id <= 0) ? new int?(0) : Skills_Aquatic[0]?.PaletteId).Value);
			API.Skill skill7 = Skills_Aquatic[1];
			build.set_AquaticUtility1SkillPaletteId((ushort)((skill7 == null || skill7.Id <= 0) ? new int?(0) : Skills_Aquatic[1]?.PaletteId).Value);
			API.Skill skill8 = Skills_Aquatic[2];
			build.set_AquaticUtility2SkillPaletteId((ushort)((skill8 == null || skill8.Id <= 0) ? new int?(0) : Skills_Aquatic[2]?.PaletteId).Value);
			API.Skill skill9 = Skills_Aquatic[3];
			build.set_AquaticUtility3SkillPaletteId((ushort)((skill9 == null || skill9.Id <= 0) ? new int?(0) : Skills_Aquatic[3]?.PaletteId).Value);
			API.Skill skill10 = Skills_Aquatic[4];
			build.set_AquaticEliteSkillPaletteId((ushort)((skill10 == null || skill10.Id <= 0) ? new int?(0) : Skills_Aquatic[4]?.PaletteId).Value);
			SpecLine specLine = SpecLines[0];
			List<API.Trait> selectedTraits = SpecLines[0].Traits;
			build.set_Specialization1Id((byte)((specLine.Specialization != null) ? ((byte)specLine.Specialization.Id) : 0));
			if (specLine.Specialization != null)
			{
				build.set_Specialization1Trait1Index((byte)((selectedTraits.Count > 0 && selectedTraits[0] != null) ? ((byte)(selectedTraits[0].Order + 1)) : 0));
				build.set_Specialization1Trait2Index((byte)((selectedTraits.Count > 1 && selectedTraits[1] != null) ? ((byte)(selectedTraits[1].Order + 1)) : 0));
				build.set_Specialization1Trait3Index((byte)((selectedTraits.Count > 2 && selectedTraits[2] != null) ? ((byte)(selectedTraits[2].Order + 1)) : 0));
			}
			specLine = SpecLines[1];
			selectedTraits = SpecLines[1].Traits;
			build.set_Specialization2Id((byte)((specLine.Specialization != null) ? ((byte)specLine.Specialization.Id) : 0));
			if (specLine.Specialization != null)
			{
				build.set_Specialization2Trait1Index((byte)((selectedTraits.Count > 0 && selectedTraits[0] != null) ? ((byte)(selectedTraits[0].Order + 1)) : 0));
				build.set_Specialization2Trait2Index((byte)((selectedTraits.Count > 1 && selectedTraits[1] != null) ? ((byte)(selectedTraits[1].Order + 1)) : 0));
				build.set_Specialization2Trait3Index((byte)((selectedTraits.Count > 2 && selectedTraits[2] != null) ? ((byte)(selectedTraits[2].Order + 1)) : 0));
			}
			specLine = SpecLines[2];
			selectedTraits = SpecLines[2].Traits;
			build.set_Specialization3Id((byte)((specLine.Specialization != null) ? ((byte)specLine.Specialization.Id) : 0));
			if (specLine.Specialization != null)
			{
				build.set_Specialization3Trait1Index((byte)((selectedTraits.Count > 0 && selectedTraits[0] != null) ? ((byte)(selectedTraits[0].Order + 1)) : 0));
				build.set_Specialization3Trait2Index((byte)((selectedTraits.Count > 1 && selectedTraits[1] != null) ? ((byte)(selectedTraits[1].Order + 1)) : 0));
				build.set_Specialization3Trait3Index((byte)((selectedTraits.Count > 2 && selectedTraits[2] != null) ? ((byte)(selectedTraits[2].Order + 1)) : 0));
			}
			byte[] bytes = ((Gw2ChatLink)build).ToArray();
			((Gw2ChatLink)build).Parse(bytes);
			code = ((object)build).ToString();
			goto IL_09fb;
			IL_037d:
			int? obj3;
			num2 = (int?)obj3;
			build.set_RevenantInactiveAquaticUtility2SkillPaletteId((ushort)num2.Value);
			if (rev)
			{
				API.Skill skill11 = InactiveSkills_Aquatic[3];
				int num3;
				if (skill11 == null)
				{
					num3 = 0;
				}
				else
				{
					_ = skill11.PaletteId;
					num3 = 1;
				}
				if (num3 != 0)
				{
					obj2 = InactiveSkills_Aquatic[3]?.PaletteId;
					goto IL_03da;
				}
			}
			obj2 = 0;
			goto IL_03da;
			IL_0209:
			int? obj4;
			num2 = (int?)obj4;
			build.set_RevenantInactiveTerrestrialUtility3SkillPaletteId((ushort)num2.Value);
			int? obj5;
			if (rev)
			{
				API.Legend legend2 = Legends_Aquatic[0];
				int num4;
				if (legend2 == null)
				{
					num4 = 0;
				}
				else
				{
					_ = legend2.Id;
					num4 = 1;
				}
				if (num4 != 0)
				{
					obj5 = Legends_Aquatic[0]?.Id;
					goto IL_0266;
				}
			}
			obj5 = 0;
			goto IL_0266;
			IL_014f:
			int? obj6;
			num2 = (int?)obj6;
			build.set_RevenantInactiveTerrestrialUtility1SkillPaletteId((ushort)num2.Value);
			int? obj7;
			if (rev)
			{
				API.Skill skill12 = InactiveSkills_Terrestrial[2];
				int num5;
				if (skill12 == null)
				{
					num5 = 0;
				}
				else
				{
					_ = skill12.PaletteId;
					num5 = 1;
				}
				if (num5 != 0)
				{
					obj7 = InactiveSkills_Terrestrial[2]?.PaletteId;
					goto IL_01ac;
				}
			}
			obj7 = 0;
			goto IL_01ac;
			IL_01ac:
			num2 = obj7;
			build.set_RevenantInactiveTerrestrialUtility2SkillPaletteId((ushort)num2.Value);
			if (rev)
			{
				API.Skill skill13 = InactiveSkills_Terrestrial[3];
				int num6;
				if (skill13 == null)
				{
					num6 = 0;
				}
				else
				{
					_ = skill13.PaletteId;
					num6 = 1;
				}
				if (num6 != 0)
				{
					obj4 = InactiveSkills_Terrestrial[3]?.PaletteId;
					goto IL_0209;
				}
			}
			obj4 = 0;
			goto IL_0209;
			IL_09fb:
			return code;
			IL_00f2:
			int? obj8;
			num2 = (int?)obj8;
			build.set_RevenantInactiveTerrestrialLegend((byte)num2.Value);
			if (rev)
			{
				API.Skill skill14 = InactiveSkills_Terrestrial[1];
				int num7;
				if (skill14 == null)
				{
					num7 = 0;
				}
				else
				{
					_ = skill14.PaletteId;
					num7 = 1;
				}
				if (num7 != 0)
				{
					obj6 = InactiveSkills_Terrestrial[1]?.PaletteId;
					goto IL_014f;
				}
			}
			obj6 = 0;
			goto IL_014f;
			IL_02c3:
			int? obj9;
			num2 = (int?)obj9;
			build.set_RevenantInactiveAquaticLegend((byte)num2.Value);
			int? obj10;
			if (rev)
			{
				API.Skill skill15 = InactiveSkills_Aquatic[1];
				int num8;
				if (skill15 == null)
				{
					num8 = 0;
				}
				else
				{
					_ = skill15.PaletteId;
					num8 = 1;
				}
				if (num8 != 0)
				{
					obj10 = InactiveSkills_Aquatic[1]?.PaletteId;
					goto IL_0320;
				}
			}
			obj10 = 0;
			goto IL_0320;
			IL_0320:
			num2 = obj10;
			build.set_RevenantInactiveAquaticUtility1SkillPaletteId((ushort)num2.Value);
			if (rev)
			{
				API.Skill skill16 = InactiveSkills_Aquatic[2];
				int num9;
				if (skill16 == null)
				{
					num9 = 0;
				}
				else
				{
					_ = skill16.PaletteId;
					num9 = 1;
				}
				if (num9 != 0)
				{
					obj3 = InactiveSkills_Aquatic[2]?.PaletteId;
					goto IL_037d;
				}
			}
			obj3 = 0;
			goto IL_037d;
			IL_0095:
			num2 = obj;
			build.set_RevenantActiveTerrestrialLegend((byte)num2.Value);
			if (rev)
			{
				API.Legend legend3 = Legends_Terrestrial[1];
				int num10;
				if (legend3 == null)
				{
					num10 = 0;
				}
				else
				{
					_ = legend3.Id;
					num10 = 1;
				}
				if (num10 != 0)
				{
					obj8 = Legends_Terrestrial[1]?.Id;
					goto IL_00f2;
				}
			}
			obj8 = 0;
			goto IL_00f2;
			IL_0266:
			num2 = obj5;
			build.set_RevenantActiveAquaticLegend((byte)num2.Value);
			if (rev)
			{
				API.Legend legend4 = Legends_Aquatic[1];
				int num11;
				if (legend4 == null)
				{
					num11 = 0;
				}
				else
				{
					_ = legend4.Id;
					num11 = 1;
				}
				if (num11 != 0)
				{
					obj9 = Legends_Aquatic[1]?.Id;
					goto IL_02c3;
				}
			}
			obj9 = 0;
			goto IL_02c3;
		}

		public BuildTemplate(string code = null)
		{
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Expected O, but got Unknown
			if (code != null)
			{
				BuildChatLink build = new BuildChatLink();
				IGw2ChatLink chatlink = null;
				if (!Gw2ChatLink.TryParse(code, ref chatlink))
				{
					return;
				}
				TemplateCode = code;
				((Gw2ChatLink)build).Parse(chatlink.ToArray());
				Profession = BuildsManager.Data.Professions.Find(delegate(API.Profession e)
				{
					//IL_000c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0011: Unknown result type (might be due to invalid IL or missing references)
					string id = e.Id;
					ProfessionType profession = build.get_Profession();
					return id == ((object)(ProfessionType)(ref profession)).ToString();
				});
				if (Profession == null)
				{
					return;
				}
				if (build.get_Specialization1Id() != 0)
				{
					SpecLines[0].Specialization = Profession.Specializations.Find((API.Specialization e) => e.Id == build.get_Specialization1Id());
					if (SpecLines[0].Specialization != null)
					{
						SpecLines[0].Traits = new List<API.Trait>();
						API.Specialization spec3 = SpecLines[0].Specialization;
						List<byte> list = new List<byte>(new byte[3]
						{
							build.get_Specialization1Trait1Index(),
							build.get_Specialization1Trait2Index(),
							build.get_Specialization1Trait3Index()
						});
						List<API.Trait> selectedTraits3 = SpecLines[0].Traits;
						int traitIndex3 = 0;
						foreach (byte bit3 in list)
						{
							if (bit3 > 0)
							{
								selectedTraits3.Add(spec3.MajorTraits[traitIndex3 * 3 + bit3 - 1]);
							}
							traitIndex3++;
						}
					}
				}
				if (build.get_Specialization2Id() != 0)
				{
					SpecLines[1].Specialization = Profession.Specializations.Find((API.Specialization e) => e.Id == build.get_Specialization2Id());
					if (SpecLines[1].Specialization != null)
					{
						SpecLines[1].Traits = new List<API.Trait>();
						API.Specialization spec2 = SpecLines[1].Specialization;
						List<byte> list2 = new List<byte>(new byte[3]
						{
							build.get_Specialization2Trait1Index(),
							build.get_Specialization2Trait2Index(),
							build.get_Specialization2Trait3Index()
						});
						List<API.Trait> selectedTraits2 = SpecLines[1].Traits;
						int traitIndex2 = 0;
						foreach (byte bit2 in list2)
						{
							if (bit2 > 0)
							{
								selectedTraits2.Add(spec2.MajorTraits[traitIndex2 * 3 + bit2 - 1]);
							}
							traitIndex2++;
						}
					}
				}
				if (build.get_Specialization3Id() != 0)
				{
					SpecLines[2].Specialization = Profession.Specializations.Find((API.Specialization e) => e.Id == build.get_Specialization3Id());
					if (SpecLines[2].Specialization != null)
					{
						SpecLines[2].Traits = new List<API.Trait>();
						API.Specialization spec = SpecLines[2].Specialization;
						List<byte> list3 = new List<byte>(new byte[3]
						{
							build.get_Specialization3Trait1Index(),
							build.get_Specialization3Trait2Index(),
							build.get_Specialization3Trait3Index()
						});
						List<API.Trait> selectedTraits = SpecLines[2].Traits;
						int traitIndex = 0;
						foreach (byte bit in list3)
						{
							if (bit > 0)
							{
								selectedTraits.Add(spec.MajorTraits[traitIndex * 3 + bit - 1]);
							}
							traitIndex++;
						}
					}
				}
				if (Profession.Id == "Revenant")
				{
					Legends_Terrestrial[0] = Profession.Legends.Find((API.Legend e) => e.Id == build.get_RevenantActiveTerrestrialLegend());
					Legends_Terrestrial[1] = Profession.Legends.Find((API.Legend e) => e.Id == build.get_RevenantInactiveTerrestrialLegend());
					if (Legends_Terrestrial[0] != null)
					{
						API.Legend legend4 = Legends_Terrestrial[0];
						Skills_Terrestrial[0] = legend4.Heal;
						Skills_Terrestrial[1] = legend4.Utilities.Find((API.Skill e) => e.PaletteId == build.get_TerrestrialUtility1SkillPaletteId());
						Skills_Terrestrial[2] = legend4.Utilities.Find((API.Skill e) => e.PaletteId == build.get_TerrestrialUtility2SkillPaletteId());
						Skills_Terrestrial[3] = legend4.Utilities.Find((API.Skill e) => e.PaletteId == build.get_TerrestrialUtility3SkillPaletteId());
						Skills_Terrestrial[4] = legend4.Elite;
					}
					if (Legends_Terrestrial[1] != null)
					{
						API.Legend legend3 = Legends_Terrestrial[1];
						InactiveSkills_Terrestrial[0] = legend3.Heal;
						InactiveSkills_Terrestrial[1] = legend3.Utilities.Find((API.Skill e) => e.PaletteId == build.get_RevenantInactiveTerrestrialUtility1SkillPaletteId());
						InactiveSkills_Terrestrial[2] = legend3.Utilities.Find((API.Skill e) => e.PaletteId == build.get_RevenantInactiveTerrestrialUtility2SkillPaletteId());
						InactiveSkills_Terrestrial[3] = legend3.Utilities.Find((API.Skill e) => e.PaletteId == build.get_RevenantInactiveTerrestrialUtility3SkillPaletteId());
						InactiveSkills_Terrestrial[4] = legend3.Elite;
					}
					Legends_Aquatic[0] = Profession.Legends.Find((API.Legend e) => e.Id == build.get_RevenantActiveAquaticLegend());
					Legends_Aquatic[1] = Profession.Legends.Find((API.Legend e) => e.Id == build.get_RevenantInactiveAquaticLegend());
					if (Legends_Aquatic[0] != null)
					{
						API.Legend legend2 = Legends_Aquatic[0];
						Skills_Aquatic[0] = legend2.Heal;
						Skills_Aquatic[1] = legend2.Utilities.Find((API.Skill e) => e.PaletteId == build.get_AquaticUtility1SkillPaletteId());
						Skills_Aquatic[2] = legend2.Utilities.Find((API.Skill e) => e.PaletteId == build.get_AquaticUtility2SkillPaletteId());
						Skills_Aquatic[3] = legend2.Utilities.Find((API.Skill e) => e.PaletteId == build.get_AquaticUtility3SkillPaletteId());
						Skills_Aquatic[4] = legend2.Elite;
					}
					if (Legends_Aquatic[1] != null)
					{
						API.Legend legend = Legends_Aquatic[1];
						InactiveSkills_Aquatic[0] = legend.Heal;
						InactiveSkills_Aquatic[1] = legend.Utilities.Find((API.Skill e) => e.PaletteId == build.get_RevenantInactiveAquaticUtility1SkillPaletteId());
						InactiveSkills_Aquatic[2] = legend.Utilities.Find((API.Skill e) => e.PaletteId == build.get_RevenantInactiveAquaticUtility2SkillPaletteId());
						InactiveSkills_Aquatic[3] = legend.Utilities.Find((API.Skill e) => e.PaletteId == build.get_RevenantInactiveAquaticUtility3SkillPaletteId());
						InactiveSkills_Aquatic[4] = legend.Elite;
					}
				}
				else
				{
					List<ushort> obj = new List<ushort>
					{
						build.get_TerrestrialHealingSkillPaletteId(),
						build.get_TerrestrialUtility1SkillPaletteId(),
						build.get_TerrestrialUtility2SkillPaletteId(),
						build.get_TerrestrialUtility3SkillPaletteId(),
						build.get_TerrestrialEliteSkillPaletteId()
					};
					int skillindex = 0;
					foreach (ushort pid2 in obj)
					{
						API.Skill skill2 = Profession.Skills.Find((API.Skill e) => e.PaletteId == pid2);
						if (skill2 != null)
						{
							Skills_Terrestrial[skillindex] = skill2;
						}
						skillindex++;
					}
					List<ushort> obj2 = new List<ushort>
					{
						build.get_AquaticHealingSkillPaletteId(),
						build.get_AquaticUtility1SkillPaletteId(),
						build.get_AquaticUtility2SkillPaletteId(),
						build.get_AquaticUtility3SkillPaletteId(),
						build.get_AquaticEliteSkillPaletteId()
					};
					skillindex = 0;
					foreach (ushort pid in obj2)
					{
						API.Skill skill = Profession.Skills.Find((API.Skill e) => e.PaletteId == pid);
						if (skill != null)
						{
							Skills_Aquatic[skillindex] = skill;
						}
						skillindex++;
					}
				}
				foreach (SpecLine specLine in SpecLines)
				{
					specLine.Changed = (EventHandler)Delegate.Combine(specLine.Changed, new EventHandler(OnChanged));
				}
			}
			else
			{
				Profession = BuildsManager.ModuleInstance.CurrentProfession;
			}
		}

		public void SwapLegends()
		{
			if (Profession.Id == "Revenant")
			{
				API.Legend tLegend = Legends_Terrestrial[0];
				Legends_Terrestrial[0] = Legends_Terrestrial[1];
				Legends_Terrestrial[1] = tLegend;
				API.Skill tSkill1 = Skills_Terrestrial[1];
				API.Skill tSkill2 = Skills_Terrestrial[2];
				API.Skill tSkill3 = Skills_Terrestrial[3];
				Skills_Terrestrial[0] = Legends_Terrestrial[0]?.Heal;
				Skills_Terrestrial[1] = InactiveSkills_Terrestrial[1];
				Skills_Terrestrial[2] = InactiveSkills_Terrestrial[2];
				Skills_Terrestrial[3] = InactiveSkills_Terrestrial[3];
				Skills_Terrestrial[4] = Legends_Terrestrial[0]?.Elite;
				InactiveSkills_Terrestrial[1] = tSkill1;
				InactiveSkills_Terrestrial[2] = tSkill2;
				InactiveSkills_Terrestrial[3] = tSkill3;
				tLegend = Legends_Aquatic[0];
				Legends_Aquatic[0] = Legends_Aquatic[1];
				Legends_Aquatic[1] = tLegend;
				tSkill1 = Skills_Aquatic[1];
				tSkill2 = Skills_Aquatic[2];
				tSkill3 = Skills_Aquatic[3];
				Skills_Aquatic[0] = Legends_Aquatic[0]?.Heal;
				Skills_Aquatic[1] = InactiveSkills_Aquatic[1];
				Skills_Aquatic[2] = InactiveSkills_Aquatic[2];
				Skills_Aquatic[3] = InactiveSkills_Aquatic[3];
				Skills_Aquatic[4] = Legends_Aquatic[0]?.Elite;
				InactiveSkills_Aquatic[1] = tSkill1;
				InactiveSkills_Aquatic[2] = tSkill2;
				InactiveSkills_Aquatic[3] = tSkill3;
				if (BuildsManager.ModuleInstance.Selected_Template.Build == this)
				{
					BuildsManager.ModuleInstance.OnSelected_Template_Edit(null, null);
				}
			}
		}

		private void OnChanged(object sender, EventArgs e)
		{
			Changed?.Invoke(this, EventArgs.Empty);
		}
	}
}
