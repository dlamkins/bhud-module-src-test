using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls.ProfessionSpecific
{
	public class EngineerSpecifics : ProfessionSpecifics
	{
		private readonly DetailedTexture _target = new DetailedTexture(156812);

		private readonly DetailedTexture _return = new DetailedTexture(156816);

		private readonly DetailedTexture _combatState = new DetailedTexture(2572084);

		private Color _healthColor = new Color(162, 17, 11);

		private Rectangle _healthRectangle;

		private Enviroment Enviroment = Enviroment.Terrestrial;

		private readonly DetailedTexture _energyBg = new DetailedTexture(1636718);

		private readonly DetailedTexture _energy = new DetailedTexture(1636719);

		private readonly DetailedTexture _overheat = new DetailedTexture(1636720);

		private Rectangle _separatorBounds;

		protected override SkillIcon[] Skills { get; } = new SkillIcon[6]
		{
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon()
		};


		public EngineerSpecifics(TemplatePresenter template, Data data)
			: base(template, data)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			template.SkillChanged += new SkillChangedEventHandler(Template_SkillChanged);
		}

		private void Template_SkillChanged(object sender, SkillChangedEventArgs e)
		{
			ApplyTemplate();
		}

		public override void RecalculateLayout()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0301: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int xOffset = 90;
			Skills[0].TextureRegion = new Rectangle(14, 14, 100, 100);
			switch (base.TemplatePresenter.Template?.EliteSpecialization?.Id)
			{
			case 57:
			{
				for (int j = 0; j < 5; j++)
				{
					Skills[j].Bounds = new Rectangle(xOffset + 20 + j * 44 + ((j == 4) ? 10 : 0), 36, 42, 42);
				}
				_energyBg.Bounds = new Rectangle(xOffset + 5, 83, 250, 12);
				_overheat.Bounds = new Rectangle(xOffset + 5, 83, 250, 12);
				_energy.TextureRegion = new Rectangle(0, 0, _energy.Texture.Width / 3 * 2, _energy.Texture.Height);
				_energy.Bounds = new Rectangle(xOffset + 5, 83, 205, 12);
				_energy.TextureRegion = new Rectangle(0, 0, _energy.Texture.Width / 3 * 2, _energy.Texture.Height);
				break;
			}
			case 70:
			{
				Skills[0].Bounds = new Rectangle(xOffset + 175, 40, 56, 56);
				_target.Bounds = new Rectangle(xOffset + 5, 5, 32, 32);
				_return.Bounds = new Rectangle(xOffset + 5 + 34, 5, 32, 32);
				_combatState.Bounds = new Rectangle(xOffset + 5 + 68, 5, 32, 32);
				_healthRectangle = new Rectangle(xOffset + 5, 81, 170, 14);
				for (int k = 1; k < 4; k++)
				{
					Skills[k].Bounds = new Rectangle(xOffset + 15 - 44 + k * 34, 40, 32, 32);
				}
				break;
			}
			default:
			{
				for (int i = 0; i < 5; i++)
				{
					Skills[i].Bounds = new Rectangle(xOffset + 30 + i * 44, 55, 42, 42);
				}
				break;
			}
			}
			Template template = base.TemplatePresenter.Template;
			if (template != null && template.EliteSpecialization?.Id == 43)
			{
				Skills[4].TextureRegion = new Rectangle(6, 6, 51, 51);
			}
			Rectangle p = Skills[3].Bounds;
			_separatorBounds = new Rectangle(((Rectangle)(ref p)).get_Right() + 6, p.Y, 2, p.Height);
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_030b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Unknown result type (might be due to invalid IL or missing references)
			//IL_034b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0351: Unknown result type (might be due to invalid IL or missing references)
			//IL_0372: Unknown result type (might be due to invalid IL or missing references)
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_037e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0388: Unknown result type (might be due to invalid IL or missing references)
			//IL_038e: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0400: Unknown result type (might be due to invalid IL or missing references)
			//IL_0406: Unknown result type (might be due to invalid IL or missing references)
			//IL_0422: Unknown result type (might be due to invalid IL or missing references)
			//IL_0427: Unknown result type (might be due to invalid IL or missing references)
			//IL_0446: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			switch (base.TemplatePresenter.Template?.EliteSpecialization?.Id)
			{
			case 57:
			{
				for (int k = 0; k < 5; k++)
				{
					Skills[k].Draw(this, spriteBatch, base.RelativeMousePosition);
				}
				_energyBg.Draw(this, spriteBatch);
				_overheat.Draw(this, spriteBatch);
				_energy.Draw(this, spriteBatch);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _separatorBounds, Color.get_Black());
				break;
			}
			case 70:
			{
				_target.Draw(this, spriteBatch);
				_return.Draw(this, spriteBatch);
				_combatState.Draw(this, spriteBatch);
				for (int j = 0; j < 4; j++)
				{
					Skills[j].Draw(this, spriteBatch, base.RelativeMousePosition);
				}
				Color borderColor = Color.get_Black();
				Rectangle b = _healthRectangle;
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _healthRectangle, Rectangle.get_Empty(), _healthColor);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref b)).get_Left(), ((Rectangle)(ref b)).get_Top(), b.Width, 1), Rectangle.get_Empty(), borderColor * 0.6f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref b)).get_Left(), ((Rectangle)(ref b)).get_Bottom() - 1, b.Width, 1), Rectangle.get_Empty(), borderColor * 0.6f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref b)).get_Left(), ((Rectangle)(ref b)).get_Top(), 1, b.Height), Rectangle.get_Empty(), borderColor * 0.6f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref b)).get_Right() - 1, ((Rectangle)(ref b)).get_Top(), 1, b.Height), Rectangle.get_Empty(), borderColor * 0.6f);
				spriteBatch.DrawStringOnCtrl(this, "100%", GameService.Content.DefaultFont14, _healthRectangle, Color.get_White(), wrap: false, HorizontalAlignment.Center);
				break;
			}
			default:
			{
				for (int i = 0; i < 5; i++)
				{
					Skills[i].Draw(this, spriteBatch, base.RelativeMousePosition);
				}
				break;
			}
			}
		}

		protected override void ApplyTemplate()
		{
			if (base.TemplatePresenter?.Template == null || !base.Data.IsLoaded)
			{
				return;
			}
			base.ApplyTemplate();
			Dictionary<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> skills = base.Data?.Professions?[ProfessionType.Engineer]?.Skills;
			if (skills == null)
			{
				return;
			}
			switch (base.TemplatePresenter.Template.EliteSpecialization?.Id)
			{
			case 70:
			{
				Skills[0].Skill = ((Enviroment == Enviroment.Terrestrial) ? skills[63089] : skills[63210]);
				int adeptSkill = (base.TemplatePresenter.Template.Specializations.Specialization3.Traits.Adept?.Skills?.FirstOrDefault()).GetValueOrDefault();
				int masterSkill = (base.TemplatePresenter.Template.Specializations.Specialization3.Traits.Master?.Skills?.FirstOrDefault()).GetValueOrDefault();
				int grandmasterSkill = (base.TemplatePresenter.Template.Specializations.Specialization3.Traits.GrandMaster?.Skills?.FirstOrDefault()).GetValueOrDefault();
				Skills[1].Skill = skills.Values.FirstOrDefault((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Id == adeptSkill);
				Skills[2].Skill = skills.Values.FirstOrDefault((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Id == masterSkill);
				Skills[3].Skill = skills.Values.FirstOrDefault((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Id == grandmasterSkill);
				break;
			}
			case 43:
				Skills[4].Skill = skills.Values.FirstOrDefault((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Id == 56920);
				break;
			case 57:
				Skills[4].Skill = skills.Values.FirstOrDefault((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Id == 42938);
				break;
			default:
				Skills[4].Skill = GetToolbeltSkill(SkillSlot.Profession5);
				break;
			}
			Kenedia.Modules.BuildsManager.DataModels.Professions.Specialization? eliteSpecialization = base.TemplatePresenter.Template.EliteSpecialization;
			if (eliteSpecialization == null || eliteSpecialization!.Id != 70)
			{
				Skills[0].Skill = GetToolbeltSkill(SkillSlot.Profession1);
				Skills[1].Skill = GetToolbeltSkill(SkillSlot.Profession2);
				Skills[2].Skill = GetToolbeltSkill(SkillSlot.Profession3);
				Skills[3].Skill = GetToolbeltSkill(SkillSlot.Profession4);
			}
			Kenedia.Modules.BuildsManager.DataModels.Professions.Skill GetToolbeltSkill(SkillSlot slot)
			{
				SkillSlotType state = SkillSlotType.Active;
				SkillSlotType enviroment = SkillSlotType.Terrestrial;
				Dictionary<SkillSlotType, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> buildSkills = base.TemplatePresenter.Template?.Skills.Where((KeyValuePair<SkillSlotType, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> e) => e.Key.HasFlag(state | enviroment)).ToDictionary((KeyValuePair<SkillSlotType, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> e) => e.Key, (KeyValuePair<SkillSlotType, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> e) => e.Value);
				switch (slot)
				{
				case SkillSlot.Profession1:
					if (buildSkills[state | enviroment | SkillSlotType.Heal] != null)
					{
						if (!buildSkills[state | enviroment | SkillSlotType.Heal].ToolbeltSkill.HasValue || !skills.TryGetValue(buildSkills[state | enviroment | SkillSlotType.Heal].ToolbeltSkill.Value, out var skill))
						{
							return null;
						}
						return skill;
					}
					break;
				case SkillSlot.Profession2:
					if (buildSkills[state | enviroment | SkillSlotType.Utility_1] != null)
					{
						if (!buildSkills[state | enviroment | SkillSlotType.Utility_1].ToolbeltSkill.HasValue || !skills.TryGetValue(buildSkills[state | enviroment | SkillSlotType.Utility_1].ToolbeltSkill.Value, out var skill2))
						{
							return null;
						}
						return skill2;
					}
					break;
				case SkillSlot.Profession3:
					if (buildSkills[state | enviroment | SkillSlotType.Utility_2] != null)
					{
						if (!buildSkills[state | enviroment | SkillSlotType.Utility_2].ToolbeltSkill.HasValue || !skills.TryGetValue(buildSkills[state | enviroment | SkillSlotType.Utility_2].ToolbeltSkill.Value, out var skill4))
						{
							return null;
						}
						return skill4;
					}
					break;
				case SkillSlot.Profession4:
					if (buildSkills[state | enviroment | SkillSlotType.Utility_3] != null)
					{
						if (!buildSkills[state | enviroment | SkillSlotType.Utility_3].ToolbeltSkill.HasValue || !skills.TryGetValue(buildSkills[state | enviroment | SkillSlotType.Utility_3].ToolbeltSkill.Value, out var skill5))
						{
							return null;
						}
						return skill5;
					}
					break;
				case SkillSlot.Profession5:
					if (buildSkills[state | enviroment | SkillSlotType.Elite] != null)
					{
						if (!buildSkills[state | enviroment | SkillSlotType.Elite].ToolbeltSkill.HasValue || !skills.TryGetValue(buildSkills[state | enviroment | SkillSlotType.Elite].ToolbeltSkill.Value, out var skill3))
						{
							return null;
						}
						return skill3;
					}
					break;
				}
				return null;
			}
		}
	}
}
