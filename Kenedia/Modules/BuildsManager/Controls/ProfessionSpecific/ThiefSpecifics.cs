using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;
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

		private Rectangle _separatorBounds;

		private int _initiativeCount;

		private readonly DetailedTexture[] _initiative = new DetailedTexture[15]
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
			new DetailedTexture(156440),
			new DetailedTexture(156440),
			new DetailedTexture(156440),
			new DetailedTexture(156440)
		};

		protected override SkillIcon[] Skills { get; } = new SkillIcon[3]
		{
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon()
		};


		public ThiefSpecifics(TemplatePresenter template, Data data)
			: base(template, data)
		{
			template.SpecializationChanged += new SpecializationChangedEventHandler(Template_SpecializationChanged);
		}

		private void Template_SpecializationChanged(object sender, SpecializationChangedEventArgs e)
		{
			ApplyTemplate();
			RecalculateLayout();
			Invalidate();
		}

		public override void RecalculateLayout()
		{
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_031c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0339: Unknown result type (might be due to invalid IL or missing references)
			//IL_0356: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int xOffset = 90;
			switch (base.TemplatePresenter?.Template?.EliteSpecialization?.Id)
			{
			case 77:
			{
				xOffset = 55;
				Skills[0].Bounds = new Rectangle(xOffset + 3, 50, 42, 42);
				Rectangle bounds = Skills[0].Bounds;
				int num = ((Rectangle)(ref bounds)).get_Right() + 6;
				bounds = Skills[0].Bounds;
				_separatorBounds = new Rectangle(num, ((Rectangle)(ref bounds)).get_Top(), 2, Skills[0].Bounds.Height + 2);
				for (int j = 1; j < 3; j++)
				{
					Skills[j].Bounds = new Rectangle(((Rectangle)(ref _separatorBounds)).get_Right() + (j - 1) * 44 + 6, 50, 42, 42);
				}
				for (int i = 0; i < _initiativeCount; i++)
				{
					DetailedTexture initiative = ((i < _initiative.Length) ? _initiative[i] : null);
					if (initiative != null)
					{
						initiative.Bounds = new Rectangle(xOffset + 200 + i / 3 * ((_initiativeCount == 12) ? 26 : 20) + ((i % 3 == 1) ? ((_initiativeCount == 12) ? 13 : 10) : 0), 40 + i % 3 * 15, 26, 26);
					}
				}
				return;
			}
			case 7:
			{
				for (int k = 0; k < _initiativeCount; k++)
				{
					DetailedTexture initiative2 = ((k < _initiative.Length) ? _initiative[k] : null);
					if (initiative2 != null)
					{
						initiative2.Bounds = new Rectangle(xOffset + ((_initiativeCount == 12) ? 100 : 80) + k * 13, 55 - k % 2 * 15, 26, 26);
					}
				}
				Skills[0].Bounds = new Rectangle(xOffset - 20 + 3, 50, 42, 42);
				Skills[1].Bounds = new Rectangle(xOffset + 3 + 45, 50, 42, 42);
				_barBackground.Bounds = new Rectangle(xOffset + 90, 80, 170, 12);
				_specterBar.Bounds = new Rectangle(xOffset + 91, 81, 168, 10);
				return;
			}
			case 71:
			{
				for (int l = 0; l < _initiativeCount; l++)
				{
					DetailedTexture initiative3 = ((l < _initiative.Length) ? _initiative[l] : null);
					if (initiative3 != null)
					{
						initiative3.Bounds = new Rectangle(xOffset + 90 + l * ((_initiativeCount == 12) ? 13 : 10), 55 - l % 2 * 13, 26, 26);
					}
				}
				Skills[0].Bounds = new Rectangle(xOffset + 3, 50, 42, 42);
				Skills[1].Bounds = new Rectangle(xOffset + 3 + 45, 50, 42, 42);
				_barBackground.Bounds = new Rectangle(xOffset + 90, 80, 170, 12);
				_specterBar.Bounds = new Rectangle(xOffset + 91, 81, 168, 10);
				return;
			}
			}
			for (int m = 0; m < _initiativeCount; m++)
			{
				DetailedTexture initiative4 = ((m < _initiative.Length) ? _initiative[m] : null);
				if (initiative4 != null)
				{
					initiative4.Bounds = new Rectangle(xOffset + ((_initiativeCount == 12) ? 100 : 80) + m * 13, 55 - m % 2 * 15, 26, 26);
				}
			}
			Skills[0].Bounds = new Rectangle(xOffset + 3, 50, 42, 42);
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0346: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			switch (base.TemplatePresenter.Template.EliteSpecialization?.Id)
			{
			case 77:
			{
				for (int k = 0; k < 3; k++)
				{
					Skills[k].Draw(this, spriteBatch, base.RelativeMousePosition);
				}
				for (int l = 0; l < _initiativeCount; l++)
				{
					((l < _initiative.Length) ? _initiative[l] : null)?.Draw(this, spriteBatch);
				}
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _separatorBounds, Color.get_Black());
				break;
			}
			case 71:
			{
				Skills[1].Draw(this, spriteBatch, base.RelativeMousePosition);
				_barBackground.Draw(this, spriteBatch);
				_specterBar.Draw(this, spriteBatch);
				for (int j = 0; j < _initiativeCount; j++)
				{
					((j < _initiative.Length) ? _initiative[j] : null)?.Draw(this, spriteBatch);
				}
				spriteBatch.DrawStringOnCtrl(this, "100%", Control.Content.DefaultFont12, _specterBar.Bounds, Color.get_White(), wrap: false, HorizontalAlignment.Center, VerticalAlignment.Bottom);
				break;
			}
			default:
			{
				for (int i = 0; i < _initiativeCount; i++)
				{
					((i < _initiative.Length) ? _initiative[i] : null)?.Draw(this, spriteBatch);
				}
				break;
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
			SkillDictionary skills = base.Data?.Professions?[ProfessionType.Thief]?.Skills;
			if (skills != null)
			{
				bool hasTrickery = (base.TemplatePresenter?.Template?.Specializations.Any(delegate(BuildSpecialization x)
				{
					Kenedia.Modules.BuildsManager.DataModels.Professions.Specialization specialization = x.Specialization;
					return specialization != null && specialization.Id == 44;
				})).GetValueOrDefault();
				_initiativeCount = (hasTrickery ? 15 : 12);
				int? num = base.TemplatePresenter?.Template?.EliteSpecialization?.Id;
				if (num.HasValue && num.GetValueOrDefault() == 77)
				{
					Skills[0].Skill = GetSkill(SkillSlot.Profession1);
					Skills[1].Skill = null;
					Skills[2].Skill = null;
				}
				else
				{
					Skills[0].Skill = GetSkill(SkillSlot.Profession1);
					Skills[1].Skill = GetSkill(SkillSlot.Profession2);
				}
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

		protected override void DisposeControl()
		{
			base.TemplatePresenter.SpecializationChanged -= new SpecializationChangedEventHandler(Template_SpecializationChanged);
			base.DisposeControl();
		}
	}
}
