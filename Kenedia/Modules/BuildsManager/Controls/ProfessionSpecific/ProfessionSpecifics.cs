using System;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.BuildsManager.Controls.ProfessionSpecific
{
	public abstract class ProfessionSpecifics : Panel
	{
		private TemplatePresenter? _templatePresenter;

		protected virtual SkillIcon[] Skills { get; } = Array.Empty<SkillIcon>();


		protected SkillTooltip SkillTooltip { get; }

		public TemplatePresenter TemplatePresenter
		{
			get
			{
				return _templatePresenter;
			}
			set
			{
				Common.SetProperty<TemplatePresenter>(ref _templatePresenter, value, new ValueChangedEventHandler<TemplatePresenter>(SetTemplatePresenter));
			}
		}

		public ProfessionSpecifics(TemplatePresenter template)
		{
			TemplatePresenter = template;
			base.ClipsBounds = false;
			ZIndex = 1073741823;
			base.Tooltip = (SkillTooltip = new SkillTooltip());
			ApplyTemplate();
		}

		private void SetTemplatePresenter(object sender, ValueChangedEventArgs<TemplatePresenter> e)
		{
			if (e.OldValue != null)
			{
				e.OldValue!.LegendChanged -= new LegendChangedEventHandler(OnLegendChanged);
				e.OldValue!.EliteSpecializationChanged -= new SpecializationChangedEventHandler(OnEliteSpecializationChanged);
				e.OldValue!.TemplateChanged -= new ValueChangedEventHandler<Template>(OnTemplateChanged);
				e.OldValue!.TraitChanged -= new TraitChangedEventHandler(OnTraitChanged);
				e.OldValue!.SkillChanged -= new SkillChangedEventHandler(OnSkillChanged);
			}
			if (e.NewValue != null)
			{
				e.NewValue!.LegendChanged += new LegendChangedEventHandler(OnLegendChanged);
				e.NewValue!.EliteSpecializationChanged += new SpecializationChangedEventHandler(OnEliteSpecializationChanged);
				e.NewValue!.TemplateChanged += new ValueChangedEventHandler<Template>(OnTemplateChanged);
				e.NewValue!.TraitChanged += new TraitChangedEventHandler(OnTraitChanged);
				e.NewValue!.SkillChanged += new SkillChangedEventHandler(OnSkillChanged);
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
			TemplatePresenter = null;
		}

		protected void SetTooltipSkill()
		{
			SkillTooltip.Skill = Skills.FirstOrDefault((SkillIcon x) => x.Hovered)?.Skill;
		}
	}
}
