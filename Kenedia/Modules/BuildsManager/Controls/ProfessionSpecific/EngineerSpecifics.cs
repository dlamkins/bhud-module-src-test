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
		private readonly DetailedTexture[] _protocols = new DetailedTexture[7]
		{
			new DetailedTexture(3680130)
			{
				TextureRegion = new Rectangle(14, 14, 100, 100)
			},
			new DetailedTexture(3680134)
			{
				TextureRegion = new Rectangle(14, 14, 100, 100)
			},
			new DetailedTexture(3680128)
			{
				TextureRegion = new Rectangle(14, 14, 100, 100)
			},
			new DetailedTexture(3680132)
			{
				TextureRegion = new Rectangle(14, 14, 100, 100)
			},
			new DetailedTexture(3680127)
			{
				TextureRegion = new Rectangle(14, 14, 100, 100)
			},
			new DetailedTexture(3680135)
			{
				TextureRegion = new Rectangle(14, 14, 100, 100)
			},
			new DetailedTexture(3680131)
			{
				TextureRegion = new Rectangle(14, 14, 100, 100)
			}
		};

		private readonly DetailedTexture _target = new DetailedTexture(156812);

		private readonly DetailedTexture _return = new DetailedTexture(156816);

		private readonly DetailedTexture _combatState = new DetailedTexture(2572084);

		private DetailedTexture[] _selectors = new DetailedTexture[3]
		{
			new DetailedTexture(157138, 157140),
			new DetailedTexture(157138, 157140),
			new DetailedTexture(157138, 157140)
		};

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
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
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
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0351: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0363: Unknown result type (might be due to invalid IL or missing references)
			//IL_0369: Unknown result type (might be due to invalid IL or missing references)
			//IL_036e: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int xOffset = 90;
			Skills[0].TextureRegion = new Rectangle(14, 14, 100, 100);
			switch (base.TemplatePresenter.Template?.EliteSpecialization?.Id)
			{
			case 57:
			{
				for (int i = 0; i < 5; i++)
				{
					Skills[i].Bounds = new Rectangle(xOffset + 20 + i * 44 + ((i == 4) ? 10 : 0), 36, 42, 42);
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
				for (int j = 1; j < 4; j++)
				{
					Skills[j].Bounds = new Rectangle(xOffset + 15 - 44 + j * 34, 40, 32, 32);
				}
				break;
			}
			default:
			{
				for (int k = 0; k < 5; k++)
				{
					Skills[k].Bounds = new Rectangle(xOffset + 30 + k * 44, 55, 42, 42);
					if (k > 0 && k < 4)
					{
						DetailedTexture obj = _selectors[k - 1];
						Rectangle bounds = Skills[k].Bounds;
						int left = ((Rectangle)(ref bounds)).get_Left();
						bounds = Skills[k].Bounds;
						obj.Bounds = new Rectangle(left, ((Rectangle)(ref bounds)).get_Top() - 8, Skills[k].Bounds.Width, 10);
					}
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
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_030e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0344: Unknown result type (might be due to invalid IL or missing references)
			//IL_0349: Unknown result type (might be due to invalid IL or missing references)
			//IL_0353: Unknown result type (might be due to invalid IL or missing references)
			//IL_0359: Unknown result type (might be due to invalid IL or missing references)
			//IL_037a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Unknown result type (might be due to invalid IL or missing references)
			//IL_0390: Unknown result type (might be due to invalid IL or missing references)
			//IL_0396: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0408: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_042a: Unknown result type (might be due to invalid IL or missing references)
			//IL_042f: Unknown result type (might be due to invalid IL or missing references)
			//IL_044e: Unknown result type (might be due to invalid IL or missing references)
			//IL_04af: Unknown result type (might be due to invalid IL or missing references)
			//IL_0510: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			switch (base.TemplatePresenter.Template?.EliteSpecialization?.Id)
			{
			case 57:
			{
				for (int l = 0; l < 5; l++)
				{
					Skills[l].Draw(this, spriteBatch, base.RelativeMousePosition);
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
				for (int k = 0; k < 4; k++)
				{
					Skills[k].Draw(this, spriteBatch, base.RelativeMousePosition);
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
			case 75:
			{
				for (int j = 0; j < 5; j++)
				{
					Skills[j].Draw(this, spriteBatch, base.RelativeMousePosition);
				}
				DetailedTexture[] selectors = _selectors;
				for (int m = 0; m < selectors.Length; m++)
				{
					selectors[m].Draw(this, spriteBatch, base.RelativeMousePosition);
				}
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
			SkillDictionary skills = base.Data?.Professions?[ProfessionType.Engineer]?.Skills;
			if (skills == null)
			{
				return;
			}
			switch (base.TemplatePresenter.Template.EliteSpecialization?.Id)
			{
			case 75:
				Skills[0].Skill = GetToolbeltSkill(SkillSlot.Profession1);
				Skills[1].Skill = null;
				Skills[1].Texture = _protocols[0].Texture;
				Skills[2].Skill = null;
				Skills[2].Texture = _protocols[1].Texture;
				Skills[3].Skill = null;
				Skills[3].Texture = _protocols[2].Texture;
				Skills[4].Skill = skills.Get(76642);
				break;
			case 70:
			{
				Skills[0].Skill = ((Enviroment == Enviroment.Terrestrial) ? skills.Get(63089) : skills.Get(63210));
				int adeptSkill = (base.TemplatePresenter.Template.Specializations.Specialization3.Traits.Adept?.Skills?.FirstOrDefault()).GetValueOrDefault();
				int masterSkill = (base.TemplatePresenter.Template.Specializations.Specialization3.Traits.Master?.Skills?.FirstOrDefault()).GetValueOrDefault();
				int grandmasterSkill = (base.TemplatePresenter.Template.Specializations.Specialization3.Traits.GrandMaster?.Skills?.FirstOrDefault()).GetValueOrDefault();
				Skills[1].Skill = skills.Values.FirstOrDefault((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Id == adeptSkill);
				Skills[2].Skill = skills.Values.FirstOrDefault((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Id == masterSkill);
				Skills[3].Skill = skills.Values.FirstOrDefault((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Id == grandmasterSkill);
				break;
			}
			case 43:
				Skills[0].Skill = GetToolbeltSkill(SkillSlot.Profession1);
				Skills[1].Skill = GetToolbeltSkill(SkillSlot.Profession2);
				Skills[2].Skill = GetToolbeltSkill(SkillSlot.Profession3);
				Skills[3].Skill = GetToolbeltSkill(SkillSlot.Profession4);
				Skills[4].Skill = skills.Values.FirstOrDefault((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Id == 56920);
				break;
			case 57:
				Skills[0].Skill = GetToolbeltSkill(SkillSlot.Profession1);
				Skills[1].Skill = GetToolbeltSkill(SkillSlot.Profession2);
				Skills[2].Skill = GetToolbeltSkill(SkillSlot.Profession3);
				Skills[3].Skill = GetToolbeltSkill(SkillSlot.Profession4);
				Skills[4].Skill = skills.Values.FirstOrDefault((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Id == 42938);
				break;
			default:
				Skills[0].Skill = GetToolbeltSkill(SkillSlot.Profession1);
				Skills[1].Skill = GetToolbeltSkill(SkillSlot.Profession2);
				Skills[2].Skill = GetToolbeltSkill(SkillSlot.Profession3);
				Skills[3].Skill = GetToolbeltSkill(SkillSlot.Profession4);
				Skills[4].Skill = GetToolbeltSkill(SkillSlot.Profession5);
				break;
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

		protected override void DisposeControl()
		{
			base.DisposeControl();
			base.TemplatePresenter.SkillChanged -= new SkillChangedEventHandler(Template_SkillChanged);
		}
	}
}
