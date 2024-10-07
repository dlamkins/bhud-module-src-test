using System;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.Core.Models;

namespace Kenedia.Modules.BuildsManager.Controls.ProfessionSpecific
{
	public abstract class ProfessionSpecifics : Panel
	{
		protected virtual SkillIcon[] Skills { get; } = Array.Empty<SkillIcon>();


		protected SkillTooltip SkillTooltip { get; }

		public TemplatePresenter TemplatePresenter { get; }

		public ProfessionSpecifics(TemplatePresenter templatePresenter)
		{
			TemplatePresenter = templatePresenter;
			base.ClipsBounds = false;
			ZIndex = 1073741823;
			base.Tooltip = (SkillTooltip = new SkillTooltip());
			SetTemplatePresenter();
			ApplyTemplate();
		}

		private void SetTemplatePresenter()
		{
			if (TemplatePresenter != null)
			{
				TemplatePresenter.LegendChanged += new LegendChangedEventHandler(OnLegendChanged);
				TemplatePresenter.EliteSpecializationChanged += new SpecializationChangedEventHandler(OnEliteSpecializationChanged);
				TemplatePresenter.TemplateChanged += new ValueChangedEventHandler<Template>(OnTemplateChanged);
				TemplatePresenter.TraitChanged += new TraitChangedEventHandler(OnTraitChanged);
				TemplatePresenter.SkillChanged += new SkillChangedEventHandler(OnSkillChanged);
			}
		}

		private void OnSkillChanged(object sender, SkillChangedEventArgs e)
		{
			ApplyTemplate();
		}

		private void OnTraitChanged(object sender, TraitChangedEventArgs e)
		{
			ApplyTemplate();
		}

		private void OnEliteSpecializationChanged(object sender, SpecializationChangedEventArgs e)
		{
			ApplyTemplate();
		}

		private void OnTemplateChanged(object sender, ValueChangedEventArgs<Template> e)
		{
			ApplyTemplate();
		}

		private void OnLoaded(object sender, EventArgs e)
		{
			ApplyTemplate();
		}

		private void OnLegendChanged(object sender, LegendChangedEventArgs e)
		{
			ApplyTemplate();
		}

		protected virtual void ApplyTemplate()
		{
			SetTooltipSkill();
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			base.OnMouseMoved(e);
			SetTooltipSkill();
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			if (TemplatePresenter != null)
			{
				TemplatePresenter.LegendChanged -= new LegendChangedEventHandler(OnLegendChanged);
				TemplatePresenter.EliteSpecializationChanged -= new SpecializationChangedEventHandler(OnEliteSpecializationChanged);
				TemplatePresenter.TemplateChanged -= new ValueChangedEventHandler<Template>(OnTemplateChanged);
				TemplatePresenter.TraitChanged -= new TraitChangedEventHandler(OnTraitChanged);
				TemplatePresenter.SkillChanged -= new SkillChangedEventHandler(OnSkillChanged);
			}
		}

		protected void SetTooltipSkill()
		{
			SkillTooltip.Skill = Skills.FirstOrDefault((SkillIcon x) => x.Hovered)?.Skill;
		}
	}
}
