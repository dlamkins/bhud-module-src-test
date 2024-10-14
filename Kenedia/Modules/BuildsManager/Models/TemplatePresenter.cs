using System;
using Gw2Sharp.Models;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.BuildsManager.Models
{
	public class TemplatePresenter
	{
		private Template _template;

		private GameModeType _gameMode;

		private AttunementType _mainAttunement = AttunementType.Fire;

		private AttunementType _altAttunement = AttunementType.Fire;

		private LegendSlotType _legendSlot = LegendSlotType.TerrestrialActive;

		public Template Template
		{
			get
			{
				return _template;
			}
			private set
			{
				Common.SetProperty(ref _template, value, new ValueChangedEventHandler<Template>(On_TemplateChanged));
			}
		}

		public AttunementType MainAttunement
		{
			get
			{
				return _mainAttunement;
			}
			private set
			{
				Common.SetProperty(ref _mainAttunement, value);
			}
		}

		public AttunementType AltAttunement
		{
			get
			{
				return _altAttunement;
			}
			private set
			{
				Common.SetProperty(ref _altAttunement, value);
			}
		}

		public LegendSlotType LegendSlot
		{
			get
			{
				return _legendSlot;
			}
			set
			{
				Common.SetProperty(ref _legendSlot, value, new ValueChangedEventHandler<LegendSlotType>(OnLegendSlotChanged));
			}
		}

		public GameModeType GameMode
		{
			get
			{
				return _gameMode;
			}
			set
			{
				Common.SetProperty(ref _gameMode, value, new ValueChangedEventHandler<GameModeType>(On_GameModeChanged));
			}
		}

		public bool IsPve => GameMode == GameModeType.PvE;

		public bool IsPvp => GameMode == GameModeType.PvP;

		public bool IsWvw => GameMode == GameModeType.WvW;

		public TemplateFactory TemplateFactory { get; }

		public Data Data { get; }

		public event EventHandler BuildCodeChanged;

		public event EventHandler GearCodeChanged;

		public event TemplateSlotChangedEventHandler? TemplateSlotChanged;

		public event ValueChangedEventHandler<string> NameChanged;

		public event ValueChangedEventHandler<Template> TemplateChanged;

		public event ValueChangedEventHandler<LegendSlotType> LegendSlotChanged;

		public event ValueChangedEventHandler<GameModeType> GameModeChanged;

		public event AttunementChangedEventHandler AttunementChanged;

		public event DictionaryItemChangedEventHandler<PetSlotType, Pet> PetChanged;

		public event ValueChangedEventHandler<ProfessionType> ProfessionChanged;

		public event ValueChangedEventHandler<Races> RaceChanged;

		public event DictionaryItemChangedEventHandler<SkillSlotType, Skill> SkillChanged_OLD;

		public event ValueChangedEventHandler<Specialization> EliteSpecializationChanged_OLD;

		public event LegendChangedEventHandler? LegendChanged;

		public event SkillChangedEventHandler SkillChanged;

		public event TraitChangedEventHandler TraitChanged;

		public event SpecializationChangedEventHandler SpecializationChanged;

		public event SpecializationChangedEventHandler EliteSpecializationChanged;

		public TemplatePresenter(TemplateFactory templateFactory, Data data)
		{
			TemplateFactory = templateFactory;
			Data = data;
			Template = TemplateFactory.CreateTemplate(string.Empty);
			Data.Loaded += new EventHandler(Data_Loaded);
		}

		private void Data_Loaded(object sender, EventArgs e)
		{
			On_TemplateChanged(this, new ValueChangedEventArgs<Template>(Template, Template));
		}

		public void SetTemplate(Template? template)
		{
			if (template == null)
			{
				template = TemplateFactory.CreateTemplate();
			}
			Template = template;
		}

		private void On_TemplateChanged(object sender, ValueChangedEventArgs<Template> e)
		{
			if (e.OldValue != null)
			{
				e.OldValue!.RaceChanged -= new ValueChangedEventHandler<Races>(On_RaceChanged);
				e.OldValue!.BuildCodeChanged -= new EventHandler(On_BuildChanged);
				e.OldValue!.GearCodeChanged -= new EventHandler(On_GearChanged);
				e.OldValue!.ProfessionChanged -= new ValueChangedEventHandler<ProfessionType>(On_ProfessionChanged);
				e.OldValue!.LegendChanged -= new LegendChangedEventHandler(On_LegendChanged);
				e.OldValue!.NameChanged -= new ValueChangedEventHandler<string>(On_NameChanged);
				e.OldValue!.TemplateSlotChanged -= new TemplateSlotChangedEventHandler(OnTemplateSlotChanged);
				e.OldValue!.SkillChanged -= new SkillChangedEventHandler(OnSkillChanged);
				e.OldValue!.TraitChanged -= new TraitChangedEventHandler(OnTraitChanged);
				e.OldValue!.EliteSpecializationChanged -= new SpecializationChangedEventHandler(OnEliteSpecializationChanged);
				e.OldValue!.SpecializationChanged -= new SpecializationChangedEventHandler(OnSpecializationChanged);
			}
			e.NewValue?.Load();
			if (e.NewValue != null)
			{
				RegisterEvents(e.NewValue);
			}
			this.TemplateChanged?.Invoke(this, e);
		}

		private void RegisterEvents(Template template)
		{
			if (template != null)
			{
				template.RaceChanged += new ValueChangedEventHandler<Races>(On_RaceChanged);
				template.BuildCodeChanged += new EventHandler(On_BuildChanged);
				template.GearCodeChanged += new EventHandler(On_GearChanged);
				template.ProfessionChanged += new ValueChangedEventHandler<ProfessionType>(On_ProfessionChanged);
				template.LegendChanged += new LegendChangedEventHandler(On_LegendChanged);
				template.NameChanged += new ValueChangedEventHandler<string>(On_NameChanged);
				template.TemplateSlotChanged += new TemplateSlotChangedEventHandler(OnTemplateSlotChanged);
				template.SkillChanged += new SkillChangedEventHandler(OnSkillChanged);
				template.TraitChanged += new TraitChangedEventHandler(OnTraitChanged);
				template.SpecializationChanged += new SpecializationChangedEventHandler(OnSpecializationChanged);
				template.EliteSpecializationChanged += new SpecializationChangedEventHandler(OnEliteSpecializationChanged);
			}
		}

		private void OnTemplateSlotChanged(object sender, TemplateSlotChangedEventArgs e)
		{
			this.TemplateSlotChanged?.Invoke(sender, e);
		}

		private void OnEliteSpecializationChanged(object sender, SpecializationChangedEventArgs e)
		{
			this.EliteSpecializationChanged?.Invoke(sender, e);
			SetAttunement(MainAttunement);
		}

		private void OnSpecializationChanged(object sender, SpecializationChangedEventArgs e)
		{
			this.SpecializationChanged?.Invoke(sender, e);
		}

		private void OnTraitChanged(object sender, TraitChangedEventArgs e)
		{
			this.TraitChanged?.Invoke(sender, e);
		}

		private void OnSkillChanged(object sender, SkillChangedEventArgs e)
		{
			this.SkillChanged?.Invoke(sender, e);
		}

		private void On_NameChanged(object sender, ValueChangedEventArgs<string> e)
		{
			this.NameChanged?.Invoke(sender, e);
		}

		private void On_GearChanged(object sender, EventArgs e)
		{
			this.GearCodeChanged?.Invoke(sender, e);
		}

		private void On_RaceChanged(object sender, ValueChangedEventArgs<Races> e)
		{
			this.RaceChanged?.Invoke(sender, e);
		}

		private void On_BuildChanged(object sender, EventArgs e)
		{
			this.BuildCodeChanged?.Invoke(sender, e);
		}

		private void On_EliteSpecializationChanged(object sender, ValueChangedEventArgs<Specialization> e)
		{
			this.EliteSpecializationChanged_OLD?.Invoke(sender, e);
			this.BuildCodeChanged?.Invoke(sender, e);
		}

		private void On_LegendChanged(object sender, LegendChangedEventArgs e)
		{
			this.LegendChanged?.Invoke(sender, e);
			this.BuildCodeChanged?.Invoke(sender, e);
		}

		private void On_ProfessionChanged(object sender, ValueChangedEventArgs<ProfessionType> e)
		{
			this.ProfessionChanged?.Invoke(sender, e);
			this.BuildCodeChanged?.Invoke(sender, e);
		}

		private void OnLegendSlotChanged(object sender, ValueChangedEventArgs<LegendSlotType> e)
		{
			this.LegendSlotChanged?.Invoke(sender, e);
		}

		private void On_GameModeChanged(object sender, ValueChangedEventArgs<GameModeType> e)
		{
			this.GameModeChanged?.Invoke(sender, e);
		}

		private void OnAttunementChanged(object sender, AttunementChangedEventArgs e)
		{
			this.AttunementChanged?.Invoke(sender, e);
		}

		public void SetAttunement(AttunementType attunement)
		{
			AttunementSlotType slot = ((MainAttunement == attunement) ? ((AltAttunement != attunement) ? AttunementSlotType.Alt : AttunementSlotType.Main) : AttunementSlotType.Main);
			if (MainAttunement != attunement || AltAttunement != attunement)
			{
				AttunementType previous = MainAttunement;
				MainAttunement = attunement;
				AltAttunement = ((Template.EliteSpecializationId == 56) ? previous : AttunementType.None);
				OnAttunementChanged(this, new AttunementChangedEventArgs(slot, previous, attunement));
			}
		}

		public void SwapLegend()
		{
			LegendSlotType legendSlot;
			switch (LegendSlot)
			{
			case LegendSlotType.AquaticActive:
			case LegendSlotType.TerrestrialActive:
				legendSlot = LegendSlotType.TerrestrialInactive;
				break;
			case LegendSlotType.AquaticInactive:
			case LegendSlotType.TerrestrialInactive:
				legendSlot = LegendSlotType.TerrestrialActive;
				break;
			default:
				legendSlot = LegendSlotType.TerrestrialActive;
				break;
			}
			LegendSlot = legendSlot;
		}

		public void InvokeTemplateSwitch()
		{
			this.TemplateChanged?.Invoke(this, new ValueChangedEventArgs<Template>(_template, _template));
		}
	}
}
