using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Input;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls.ProfessionSpecific
{
	public class ElementalistSpecifics : ProfessionSpecifics
	{
		private readonly DetailedTexture _catalistSeparator = new DetailedTexture(2492046);

		private readonly (Rectangle bounds, Color color)[] _backgrounds = new(Rectangle, Color)[5]
		{
			(Rectangle.get_Empty(), new Color(255, 125, 0)),
			(Rectangle.get_Empty(), new Color(0, 170, 255)),
			(Rectangle.get_Empty(), new Color(165, 101, 255)),
			(Rectangle.get_Empty(), new Color(231, 195, 22)),
			(Rectangle.get_Empty(), Color.get_Transparent())
		};

		private Rectangle _catalystEnergy;

		private Color _catalystEnergyColor;

		protected override SkillIcon[] Skills { get; } = new SkillIcon[5]
		{
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon()
		};


		public ElementalistSpecifics(TemplatePresenter template)
			: base(template)
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			template.AttunementChanged += new AttunementChangedEventHandler(AttunementChanged);
		}

		private void AttunementChanged(object sender, AttunementChangedEventArgs e)
		{
			ApplyTemplate();
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			CalculateSkillBounds();
		}

		private void CalculateSkillBounds()
		{
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			int xOffset = 70;
			Rectangle lastRect = default(Rectangle);
			((Rectangle)(ref lastRect))._002Ector(xOffset + 25, 52, 0, 0);
			for (int i = 0; i < Skills.Length; i++)
			{
				SkillIcon skill = Skills[i];
				bool main = (i == 0 && base.TemplatePresenter.MainAttunement == AttunementType.Fire) || (i == 1 && base.TemplatePresenter.MainAttunement == AttunementType.Water) || (i == 2 && base.TemplatePresenter.MainAttunement == AttunementType.Air) || (i == 3 && base.TemplatePresenter.MainAttunement == AttunementType.Earth);
				bool secondary = (i == 0 && base.TemplatePresenter.AltAttunement == AttunementType.Fire) || (i == 1 && base.TemplatePresenter.AltAttunement == AttunementType.Water) || (i == 2 && base.TemplatePresenter.AltAttunement == AttunementType.Air) || (i == 3 && base.TemplatePresenter.AltAttunement == AttunementType.Earth);
				_backgrounds[i].bounds = (main ? new Rectangle(((Rectangle)(ref lastRect)).get_Right() + 4, 47, 44, 44) : (secondary ? new Rectangle(((Rectangle)(ref lastRect)).get_Right() + 4, 49, 39, 39) : new Rectangle(((Rectangle)(ref lastRect)).get_Right() + 4, 54, 34, 0)));
				skill.Bounds = (main ? new Rectangle(((Rectangle)(ref lastRect)).get_Right() + 6, 49, 40, 40) : (secondary ? new Rectangle(((Rectangle)(ref lastRect)).get_Right() + 6, 51, 35, 35) : new Rectangle(((Rectangle)(ref lastRect)).get_Right() + 4 + ((i == 4) ? 20 : 0), (i == 4) ? 52 : 54, (i == 4) ? 38 : 34, (i == 4) ? 38 : 34)));
				lastRect = skill.Bounds;
				if (i == 4)
				{
					Rectangle bounds = skill.Bounds;
					int left = ((Rectangle)(ref bounds)).get_Left();
					bounds = skill.Bounds;
					_catalystEnergy = new Rectangle(left, ((Rectangle)(ref bounds)).get_Top() - 4, skill.Bounds.Width, 4);
				}
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			AttunementType attunement = GetAttunement();
			if (attunement != 0)
			{
				base.TemplatePresenter.SetAttunement(attunement);
			}
		}

		private AttunementType GetAttunement()
		{
			if (!Skills[0].Hovered)
			{
				if (!Skills[1].Hovered)
				{
					if (!Skills[2].Hovered)
					{
						if (!Skills[3].Hovered)
						{
							return AttunementType.None;
						}
						return AttunementType.Earth;
					}
					return AttunementType.Air;
				}
				return AttunementType.Water;
			}
			return AttunementType.Fire;
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			int? num = base.TemplatePresenter.Template.EliteSpecialization?.Id;
			if (num.HasValue && num.GetValueOrDefault() == 67)
			{
				for (int i = 0; i < 4; i++)
				{
					spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _backgrounds[i].bounds, _backgrounds[i].color);
				}
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _catalystEnergy, _catalystEnergyColor);
				SkillIcon[] skills = Skills;
				for (int k = 0; k < skills.Length; k++)
				{
					skills[k].Draw(this, spriteBatch, base.RelativeMousePosition);
				}
			}
			else
			{
				for (int j = 0; j < 4; j++)
				{
					spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _backgrounds[j].bounds, _backgrounds[j].color);
					Skills[j].Draw(this, spriteBatch, base.RelativeMousePosition);
				}
			}
			SetTooltipSkill();
		}

		protected override void ApplyTemplate()
		{
			//IL_0389: Unknown result type (might be due to invalid IL or missing references)
			//IL_039c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03af: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03da: Unknown result type (might be due to invalid IL or missing references)
			if (base.TemplatePresenter?.Template == null)
			{
				return;
			}
			base.ApplyTemplate();
			Dictionary<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> skills = BuildsManager.Data?.Professions?[ProfessionType.Elementalist]?.Skills;
			if (skills != null)
			{
				SkillIcon obj = Skills[0];
				Kenedia.Modules.BuildsManager.DataModels.Professions.Specialization? eliteSpecialization = base.TemplatePresenter.Template.EliteSpecialization;
				obj.Skill = ((eliteSpecialization != null && eliteSpecialization!.Id == 48 && base.TemplatePresenter.MainAttunement == AttunementType.Fire) ? skills.Values.FirstOrDefault((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Id == 29706) : GetSkill(SkillSlot.Profession1));
				SkillIcon obj2 = Skills[1];
				Kenedia.Modules.BuildsManager.DataModels.Professions.Specialization? eliteSpecialization2 = base.TemplatePresenter.Template.EliteSpecialization;
				obj2.Skill = ((eliteSpecialization2 != null && eliteSpecialization2!.Id == 48 && base.TemplatePresenter.MainAttunement == AttunementType.Water) ? skills.Values.FirstOrDefault((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Id == 29415) : GetSkill(SkillSlot.Profession2));
				SkillIcon obj3 = Skills[2];
				Kenedia.Modules.BuildsManager.DataModels.Professions.Specialization? eliteSpecialization3 = base.TemplatePresenter.Template.EliteSpecialization;
				obj3.Skill = ((eliteSpecialization3 != null && eliteSpecialization3!.Id == 48 && base.TemplatePresenter.MainAttunement == AttunementType.Air) ? skills.Values.FirstOrDefault((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Id == 29719) : GetSkill(SkillSlot.Profession3));
				SkillIcon obj4 = Skills[3];
				Kenedia.Modules.BuildsManager.DataModels.Professions.Specialization? eliteSpecialization4 = base.TemplatePresenter.Template.EliteSpecialization;
				obj4.Skill = ((eliteSpecialization4 != null && eliteSpecialization4!.Id == 48 && base.TemplatePresenter.MainAttunement == AttunementType.Earth) ? skills.Values.FirstOrDefault((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Id == 29618) : GetSkill(SkillSlot.Profession4));
				Skills[4].Skill = ((base.TemplatePresenter.MainAttunement == AttunementType.Fire) ? skills.Values.FirstOrDefault((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Id == 62813) : ((base.TemplatePresenter.MainAttunement == AttunementType.Water) ? skills.Values.FirstOrDefault((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Id == 62723) : ((base.TemplatePresenter.MainAttunement == AttunementType.Air) ? skills.Values.FirstOrDefault((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Id == 62940) : ((base.TemplatePresenter.MainAttunement == AttunementType.Earth) ? skills.Values.FirstOrDefault((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Id == 62837) : null))));
				_catalystEnergyColor = ((base.TemplatePresenter.MainAttunement == AttunementType.Fire) ? _backgrounds[0].color : ((base.TemplatePresenter.MainAttunement == AttunementType.Water) ? _backgrounds[1].color : ((base.TemplatePresenter.MainAttunement == AttunementType.Air) ? _backgrounds[2].color : ((base.TemplatePresenter.MainAttunement == AttunementType.Earth) ? _backgrounds[3].color : Color.get_Black()))));
				RecalculateLayout();
			}
			Kenedia.Modules.BuildsManager.DataModels.Professions.Skill? GetSkill(SkillSlot slot)
			{
				Kenedia.Modules.BuildsManager.DataModels.Professions.Skill skill = null;
				foreach (Kenedia.Modules.BuildsManager.DataModels.Professions.Skill item in skills.Values.Where((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Slot == slot))
				{
					if (skill == null)
					{
						skill = ((item.Specialization == base.TemplatePresenter.Template.EliteSpecialization?.Id || item.Specialization == 0) ? item : skill);
					}
				}
				return skill;
			}
		}
	}
}
