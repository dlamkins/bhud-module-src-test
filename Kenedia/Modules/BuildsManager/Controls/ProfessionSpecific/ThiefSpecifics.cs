using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls.ProfessionSpecific
{
	public class ThiefSpecifics : ProfessionSpecifics
	{
		private readonly DetailedTexture _barBackground = new DetailedTexture(1636710);

		private readonly DetailedTexture _specterBar = new DetailedTexture(2468316);

		private readonly DetailedTexture[] _initiative = new DetailedTexture[12]
		{
			new DetailedTexture(156440),
			new DetailedTexture(156440),
			new DetailedTexture(156440),
			new DetailedTexture(156440),
			new DetailedTexture(156440),
			new DetailedTexture(156440),
			new DetailedTexture(156440),
			new DetailedTexture(156440),
			new DetailedTexture(156440),
			new DetailedTexture(156440),
			new DetailedTexture(156440),
			new DetailedTexture(156440)
		};

		protected override SkillIcon[] Skills { get; } = new SkillIcon[2]
		{
			new SkillIcon(),
			new SkillIcon()
		};


		public ThiefSpecifics(TemplatePresenter template, Data data)
			: base(template, data)
		{
		}

		public override void RecalculateLayout()
		{
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int xOffset = 90;
			int? num = base.TemplatePresenter?.Template?.EliteSpecialization?.Id;
			if (num.HasValue && num.GetValueOrDefault() == 71)
			{
				for (int j = 0; j < _initiative.Length; j++)
				{
					_initiative[j].Bounds = new Rectangle(xOffset + 90 + j * 18, 55 - j % 2 * 13, 26, 26);
				}
				Skills[0].Bounds = new Rectangle(xOffset + 3, 50, 42, 42);
				Skills[1].Bounds = new Rectangle(xOffset + 3 + 45, 50, 42, 42);
				_barBackground.Bounds = new Rectangle(xOffset + 90, 80, 170, 12);
				_specterBar.Bounds = new Rectangle(xOffset + 91, 81, 168, 10);
			}
			else
			{
				for (int i = 0; i < _initiative.Length; i++)
				{
					_initiative[i].Bounds = new Rectangle(xOffset + 90 + i * 13, 63 - i % 2 * 13, 26, 26);
				}
				Skills[0].Bounds = new Rectangle(xOffset + 3, 50, 42, 42);
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			int? num = base.TemplatePresenter.Template.EliteSpecialization?.Id;
			if (num.HasValue && num.GetValueOrDefault() == 71)
			{
				Skills[1].Draw(this, spriteBatch, base.RelativeMousePosition);
				_barBackground.Draw(this, spriteBatch);
				_specterBar.Draw(this, spriteBatch);
				for (int i = 0; i < 9; i++)
				{
					_initiative[i].Draw(this, spriteBatch);
				}
				spriteBatch.DrawStringOnCtrl(this, "100%", Control.Content.DefaultFont12, _specterBar.Bounds, Color.get_White(), wrap: false, HorizontalAlignment.Center, VerticalAlignment.Bottom);
			}
			else
			{
				for (int j = 0; j < _initiative.Length; j++)
				{
					_initiative[j].Draw(this, spriteBatch);
				}
			}
			Skills[0].Draw(this, spriteBatch, base.RelativeMousePosition);
		}

		protected override void ApplyTemplate()
		{
			if (base.TemplatePresenter?.Template == null || !base.Data.IsLoaded)
			{
				return;
			}
			base.ApplyTemplate();
			Dictionary<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> skills = base.Data?.Professions?[ProfessionType.Thief]?.Skills;
			if (skills != null)
			{
				int? num = base.TemplatePresenter?.Template?.EliteSpecialization?.Id;
				if (num.HasValue)
				{
					num.GetValueOrDefault();
					_ = 71;
				}
				Skills[0].Skill = GetSkill(SkillSlot.Profession1);
				Skills[1].Skill = GetSkill(SkillSlot.Profession2);
			}
			Kenedia.Modules.BuildsManager.DataModels.Professions.Skill? GetSkill(SkillSlot slot)
			{
				Kenedia.Modules.BuildsManager.DataModels.Professions.Skill skill = null;
				foreach (Kenedia.Modules.BuildsManager.DataModels.Professions.Skill item in skills.Values.Where((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Slot == slot))
				{
					if (skill == null)
					{
						skill = ((item.Specialization == base.TemplatePresenter?.Template?.EliteSpecialization?.Id || item.Specialization == 0) ? item : skill);
					}
					if (item.Specialization == base.TemplatePresenter?.Template?.EliteSpecialization?.Id && skill.Specialization == 0)
					{
						skill = item;
					}
				}
				return skill;
			}
		}
	}
}
