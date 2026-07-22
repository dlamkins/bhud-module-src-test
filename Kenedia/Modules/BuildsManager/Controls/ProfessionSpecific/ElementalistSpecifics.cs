using System.Linq;
using Blish_HUD;
using Blish_HUD.Input;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls.ProfessionSpecific
{
	public class ElementalistSpecifics : ProfessionSpecifics
	{
		private readonly DetailedTexture _catalistSeparator = new DetailedTexture(2492046);

		private readonly DetailedTexture _evokerBackground = new DetailedTexture(3680660);

		private readonly DetailedTexture _evokerInnerRingBackground = new DetailedTexture(3680673);

		private readonly DetailedTexture _evokerRingBackground = new DetailedTexture(3680673);

		private readonly DetailedTexture _evokerRing = new DetailedTexture(3680672);

		private readonly (Microsoft.Xna.Framework.Rectangle bounds, Microsoft.Xna.Framework.Color color)[] _backgrounds = new(Microsoft.Xna.Framework.Rectangle, Microsoft.Xna.Framework.Color)[5]
		{
			(Microsoft.Xna.Framework.Rectangle.Empty, new Microsoft.Xna.Framework.Color(255, 125, 0)),
			(Microsoft.Xna.Framework.Rectangle.Empty, new Microsoft.Xna.Framework.Color(0, 170, 255)),
			(Microsoft.Xna.Framework.Rectangle.Empty, new Microsoft.Xna.Framework.Color(165, 101, 255)),
			(Microsoft.Xna.Framework.Rectangle.Empty, new Microsoft.Xna.Framework.Color(231, 195, 22)),
			(Microsoft.Xna.Framework.Rectangle.Empty, Microsoft.Xna.Framework.Color.Transparent)
		};

		private Microsoft.Xna.Framework.Rectangle _catalystEnergy;

		private Microsoft.Xna.Framework.Color _catalystEnergyColor;

		public DetailedTexture Selector { get; } = new DetailedTexture(157138, 157140);


		protected override SkillIcon[] Skills { get; } = new SkillIcon[5]
		{
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon()
		};


		public ElementalistSpecifics(TemplatePresenter template, Data data)
			: base(template, data)
		{
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
			int xOffset = 70;
			Microsoft.Xna.Framework.Rectangle lastRect = new Microsoft.Xna.Framework.Rectangle(xOffset + 25, 52, 0, 0);
			for (int i = 0; i < Skills.Length; i++)
			{
				SkillIcon skill = Skills[i];
				bool main = (i == 0 && base.TemplatePresenter.MainAttunement == AttunementType.Fire) || (i == 1 && base.TemplatePresenter.MainAttunement == AttunementType.Water) || (i == 2 && base.TemplatePresenter.MainAttunement == AttunementType.Air) || (i == 3 && base.TemplatePresenter.MainAttunement == AttunementType.Earth);
				bool secondary = (i == 0 && base.TemplatePresenter.AltAttunement == AttunementType.Fire) || (i == 1 && base.TemplatePresenter.AltAttunement == AttunementType.Water) || (i == 2 && base.TemplatePresenter.AltAttunement == AttunementType.Air) || (i == 3 && base.TemplatePresenter.AltAttunement == AttunementType.Earth);
				_backgrounds[i].bounds = (main ? new Microsoft.Xna.Framework.Rectangle(lastRect.Right + 4, 47, 44, 44) : (secondary ? new Microsoft.Xna.Framework.Rectangle(lastRect.Right + 4, 49, 39, 39) : new Microsoft.Xna.Framework.Rectangle(lastRect.Right + 4, 54, 34, 0)));
				skill.Bounds = (main ? new Microsoft.Xna.Framework.Rectangle(lastRect.Right + 6, 49, 40, 40) : (secondary ? new Microsoft.Xna.Framework.Rectangle(lastRect.Right + 6, 51, 35, 35) : new Microsoft.Xna.Framework.Rectangle(lastRect.Right + 4 + ((i == 4) ? 20 : 0), (i == 4) ? 52 : 54, (i == 4) ? 38 : 34, (i == 4) ? 38 : 34)));
				if (i == 4)
				{
					_catalystEnergy = new Microsoft.Xna.Framework.Rectangle(skill.Bounds.Left, skill.Bounds.Top - 4, skill.Bounds.Width, 4);
				}
				int? num = base.TemplatePresenter.Template.EliteSpecialization?.Id;
				if (num.HasValue && num.GetValueOrDefault() == 80)
				{
					if (i == 4)
					{
						skill.Bounds = new Microsoft.Xna.Framework.Rectangle(lastRect.Right + 4 + 20, 55, 34, 34);
						Selector.Bounds = new Microsoft.Xna.Framework.Rectangle(skill.Bounds.Left, skill.Bounds.Top - 8, skill.Bounds.Width, 10);
					}
					_evokerBackground.Bounds = new Microsoft.Xna.Framework.Rectangle(lastRect.Right + 4, 40, 116, 58);
					_evokerRingBackground.Bounds = new Microsoft.Xna.Framework.Rectangle(lastRect.Right + 4, 40, 116, 58);
					_evokerRing.Bounds = new Microsoft.Xna.Framework.Rectangle(lastRect.Right + 4, 40, 116, 58);
				}
				lastRect = skill.Bounds;
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

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Microsoft.Xna.Framework.Rectangle bounds)
		{
			base.PaintAfterChildren(spriteBatch, bounds);
			RecalculateLayout();
			switch (base.TemplatePresenter.Template.EliteSpecialization?.Id)
			{
			case 80:
			{
				_evokerBackground.Draw(this, spriteBatch);
				DetailedTexture evokerRingBackground = _evokerRingBackground;
				Microsoft.Xna.Framework.Color? color = Microsoft.Xna.Framework.Color.Orange;
				evokerRingBackground.Draw(this, spriteBatch, null, color);
				_evokerRing.Draw(this, spriteBatch);
				SkillIcon[] skills = Skills;
				for (int k = 0; k < skills.Length; k++)
				{
					skills[k].Draw(this, spriteBatch, base.RelativeMousePosition);
				}
				Selector.Draw(this, spriteBatch, base.RelativeMousePosition);
				break;
			}
			case 67:
			{
				for (int j = 0; j < 4; j++)
				{
					spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _backgrounds[j].bounds, _backgrounds[j].color);
				}
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _catalystEnergy, _catalystEnergyColor);
				SkillIcon[] skills = Skills;
				for (int k = 0; k < skills.Length; k++)
				{
					skills[k].Draw(this, spriteBatch, base.RelativeMousePosition);
				}
				break;
			}
			default:
			{
				for (int i = 0; i < 4; i++)
				{
					spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _backgrounds[i].bounds, _backgrounds[i].color);
					Skills[i].Draw(this, spriteBatch, base.RelativeMousePosition);
				}
				break;
			}
			}
			SetTooltipSkill();
		}

		protected override void ApplyTemplate()
		{
			if (base.TemplatePresenter?.Template == null || !base.Data.IsLoaded)
			{
				return;
			}
			base.ApplyTemplate();
			SkillDictionary skills = base.Data?.Professions?[ProfessionType.Elementalist]?.Skills;
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
				switch (base.TemplatePresenter.Template.EliteSpecialization?.Id)
				{
				case 80:
					Skills[4].Skill = skills.Get(76643);
					break;
				case 67:
					Skills[4].Skill = ((base.TemplatePresenter.MainAttunement == AttunementType.Fire) ? skills.Get(62813) : ((base.TemplatePresenter.MainAttunement == AttunementType.Water) ? skills.Get(62723) : ((base.TemplatePresenter.MainAttunement == AttunementType.Air) ? skills.Get(62940) : ((base.TemplatePresenter.MainAttunement == AttunementType.Earth) ? skills.Get(62837) : null))));
					break;
				default:
					Skills[4].Skill = null;
					break;
				}
				_catalystEnergyColor = ((base.TemplatePresenter.MainAttunement == AttunementType.Fire) ? _backgrounds[0].color : ((base.TemplatePresenter.MainAttunement == AttunementType.Water) ? _backgrounds[1].color : ((base.TemplatePresenter.MainAttunement == AttunementType.Air) ? _backgrounds[2].color : ((base.TemplatePresenter.MainAttunement == AttunementType.Earth) ? _backgrounds[3].color : Microsoft.Xna.Framework.Color.Black))));
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

		protected override void DisposeControl()
		{
			base.TemplatePresenter.AttunementChanged -= new AttunementChangedEventHandler(AttunementChanged);
			base.DisposeControl();
		}
	}
}
