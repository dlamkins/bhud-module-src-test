using System;
using Blish_HUD.Settings;
using Ideka.BHUDCommon;
using Ideka.NetCommon;

namespace Ideka.CustomCombatText
{
	public class ModuleSettings : IDisposable
	{
		private readonly DisposableCollection _dc = new DisposableCollection();

		public GenericSetting<string> FontName { get; }

		public SliderSetting FontSize { get; }

		public GenericSetting<bool> AutocropIcons { get; }

		public GenericSetting<float> AutocropTolerance { get; }

		public SliderSetting MinIconMargin { get; }

		public SliderSetting MergeMaxMsStrikes { get; }

		public SliderSetting MergeMaxMsBuffs { get; }

		public GenericSetting<bool> MergeAttackChains { get; }

		public GenericSetting<bool> MultiIconMessages { get; }

		public GenericSetting<bool> PetToMasterIsSelf { get; }

		public GenericSetting<bool> MasterToPetIsSelf { get; }

		public GenericSetting<bool> Debug { get; }

		public ModuleSettings(SettingCollection settings)
		{
			FontName = _dc.Add(settings.Generic("FontName", "", () => Strings.SettingFontName, () => Strings.SettingFontNameText));
			FontSize = _dc.Add(settings.Slider("FontSize", 32, 1, 100, () => Strings.SettingFontSize, () => Strings.SettingFontSizeText));
			AutocropIcons = _dc.Add(settings.Generic("AutocropIcons", defaultValue: true, () => Strings.SettingAutocropIcons, () => Strings.SettingAutocropIconsText));
			AutocropTolerance = _dc.Add(settings.PercentageSlider("AutocropTolerance", 0.12f, 0f, 0.5f, () => Strings.SettingAutocropTolerance, () => Strings.SettingAutocropToleranceText));
			MinIconMargin = _dc.Add(settings.Slider("MinIconMargin", 5, 0, 20, () => Strings.SettingMinIconMargin, () => Strings.SettingMinIconMarginText));
			MergeMaxMsStrikes = _dc.Add(settings.Slider("MergeMaxMsStrikes", 1500, -1, 2000, () => Strings.SettingMergeMaxMsStrikes, () => Strings.SettingMergeMaxMsStrikesText));
			MergeMaxMsBuffs = _dc.Add(settings.Slider("MergeMaxMsBuffs", 200, -1, 2000, () => Strings.SettingMergeMaxMsBuffs, () => Strings.SettingMergeMaxMsBuffsText));
			MergeAttackChains = _dc.Add(settings.Generic("MergeAttackChains", defaultValue: true, () => Strings.SettingMergeAttackChains, () => Strings.SettingMergeAttackChainsText));
			MultiIconMessages = _dc.Add(settings.Generic("MultiIconMessages", defaultValue: true, () => Strings.SettingMultiIconMessages, () => Strings.SettingMultiIconMessagesText));
			PetToMasterIsSelf = _dc.Add(settings.Generic("PetToMasterIsSelf", defaultValue: true, () => Strings.SettingPetToMasterIsSelf, () => Strings.SettingPetToMasterIsSelfText));
			MasterToPetIsSelf = _dc.Add(settings.Generic("MasterToPetIsSelf", defaultValue: true, () => Strings.SettingMasterToPetIsSelf, () => Strings.SettingMasterToPetIsSelfText));
			Debug = _dc.Add(settings.Generic("Debug", defaultValue: false, () => Strings.SettingDebug, () => Strings.SettingDebugText));
		}

		public void Dispose()
		{
			_dc.Dispose();
		}
	}
}
