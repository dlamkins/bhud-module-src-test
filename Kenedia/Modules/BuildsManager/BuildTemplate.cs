using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Gw2Mumble;
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

		public List<API.Skill> Skills_Terrestial = new List<API.Skill>
		{
			new API.Skill(),
			new API.Skill(),
			new API.Skill(),
			new API.Skill(),
			new API.Skill()
		};

		public List<API.Skill> Skills_Aquatic = new List<API.Skill>
		{
			new API.Skill(),
			new API.Skill(),
			new API.Skill(),
			new API.Skill(),
			new API.Skill()
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
			string code = "";
			if (Profession != null)
			{
				BuildChatLink build = new BuildChatLink();
				build.set_Profession((ProfessionType)Enum.Parse(typeof(ProfessionType), Profession.Id));
				build.set_AquaticHealingSkillPaletteId((ushort)((Skills_Aquatic[0] != null && Skills_Aquatic[0].PaletteId != 0) ? ((ushort)Skills_Aquatic[0].PaletteId) : 0));
				build.set_AquaticUtility1SkillPaletteId((ushort)((Skills_Aquatic[1] != null && Skills_Aquatic[1].PaletteId != 0) ? ((ushort)Skills_Aquatic[1].PaletteId) : 0));
				build.set_AquaticUtility2SkillPaletteId((ushort)((Skills_Aquatic[2] != null && Skills_Aquatic[2].PaletteId != 0) ? ((ushort)Skills_Aquatic[2].PaletteId) : 0));
				build.set_AquaticUtility3SkillPaletteId((ushort)((Skills_Aquatic[3] != null && Skills_Aquatic[3].PaletteId != 0) ? ((ushort)Skills_Aquatic[3].PaletteId) : 0));
				build.set_AquaticEliteSkillPaletteId((ushort)((Skills_Aquatic[4] != null && Skills_Aquatic[4].PaletteId != 0) ? ((ushort)Skills_Aquatic[4].PaletteId) : 0));
				build.set_TerrestrialHealingSkillPaletteId((ushort)((Skills_Terrestial[0] != null && Skills_Terrestial[0].PaletteId != 0) ? ((ushort)Skills_Terrestial[0].PaletteId) : 0));
				build.set_TerrestrialUtility1SkillPaletteId((ushort)((Skills_Terrestial[1] != null && Skills_Terrestial[1].PaletteId != 0) ? ((ushort)Skills_Terrestial[1].PaletteId) : 0));
				build.set_TerrestrialUtility2SkillPaletteId((ushort)((Skills_Terrestial[2] != null && Skills_Terrestial[2].PaletteId != 0) ? ((ushort)Skills_Terrestial[2].PaletteId) : 0));
				build.set_TerrestrialUtility3SkillPaletteId((ushort)((Skills_Terrestial[3] != null && Skills_Terrestial[3].PaletteId != 0) ? ((ushort)Skills_Terrestial[3].PaletteId) : 0));
				build.set_TerrestrialEliteSkillPaletteId((ushort)((Skills_Terrestial[4] != null && Skills_Terrestial[4].PaletteId != 0) ? ((ushort)Skills_Terrestial[4].PaletteId) : 0));
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
			}
			return code;
		}

		public BuildTemplate(string code = null)
		{
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Expected O, but got Unknown
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
					string id2 = e.Id;
					ProfessionType profession2 = build.get_Profession();
					return id2 == ((object)(ProfessionType)(ref profession2)).ToString();
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
						Skills_Terrestial[skillindex] = skill2;
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
				foreach (SpecLine specLine in SpecLines)
				{
					specLine.Changed = (EventHandler)Delegate.Combine(specLine.Changed, new EventHandler(OnChanged));
				}
				return;
			}
			PlayerCharacter player = GameService.Gw2Mumble.get_PlayerCharacter();
			if (player != null)
			{
				Profession = BuildsManager.Data.Professions.Find(delegate(API.Profession e)
				{
					//IL_000c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0011: Unknown result type (might be due to invalid IL or missing references)
					string id = e.Id;
					ProfessionType profession = player.get_Profession();
					return id == ((object)(ProfessionType)(ref profession)).ToString();
				});
			}
		}

		private void OnChanged(object sender, EventArgs e)
		{
			BuildsManager.Logger.Debug("Template Changed: ");
			Changed?.Invoke(this, EventArgs.Empty);
		}
	}
}
