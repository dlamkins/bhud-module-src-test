using Blish_HUD.Input;
using Blish_HUD.Settings;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Res;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.BuildsManager.Services
{
	public class Settings : BaseSettingsModel
	{
		private SettingCollection _settingCollection;

		public SettingEntry<TemplateSortBehavior> SortBehavior { get; set; }

		public SettingEntry<bool> ShowCornerIcon { get; set; }

		public SettingEntry<bool> ShowQuickFilterPanelOnTabOpen { get; set; }

		public SettingEntry<bool> ShowQuickFilterPanelOnWindowOpen { get; set; }

		public SettingEntry<bool> AutoSetFilterProfession { get; set; }

		public SettingEntry<bool> AutoSetFilterSpecialization { get; private set; }

		public SettingEntry<double> QuickFiltersPanelFadeDelay { get; private set; }

		public SettingEntry<double> QuickFiltersPanelFadeDuration { get; private set; }

		public SettingEntry<KeyBinding> ToggleWindowKey { get; set; }

		public SettingCollection SettingCollection
		{
			get
			{
				return _settingCollection;
			}
			set
			{
				Common.SetProperty(ref _settingCollection, value, new ValueChangedEventHandler<SettingCollection>(OnSettingCollectionChanged));
			}
		}

		public SettingEntry<bool> QuickFiltersPanelFade { get; private set; }

		private void InitializeSettings(SettingCollection settings)
		{
			SettingCollection = settings;
			SettingCollection internalSettings = settings.AddSubCollection("Internal", renderInUi: false, lazyLoaded: false);
			SortBehavior = internalSettings.DefineSetting("SortBehavior", TemplateSortBehavior.ByProfession);
			ShowQuickFilterPanelOnWindowOpen = internalSettings.DefineSetting("ShowQuickFilterPanelOnWindowOpen", defaultValue: false);
			ShowQuickFilterPanelOnTabOpen = internalSettings.DefineSetting("ShowQuickFilterPanelOnTabOpen", defaultValue: true);
			ShowCornerIcon = internalSettings.DefineSetting("ShowCornerIcon", defaultValue: true);
			QuickFiltersPanelFade = internalSettings.DefineSetting("QuickFiltersPanelFade", defaultValue: true);
			QuickFiltersPanelFadeDuration = internalSettings.DefineSetting("QuickFiltersPanelFadeDuration", 1000.0);
			QuickFiltersPanelFadeDelay = internalSettings.DefineSetting("QuickFiltersPanelFadeDelay", 5000.0);
			AutoSetFilterProfession = internalSettings.DefineSetting("AutoSetFilterProfession", defaultValue: false, () => strings.AutoSetProfession_Name, () => strings.AutoSetProfession_Tooltip);
			AutoSetFilterSpecialization = internalSettings.DefineSetting("AutoSetFilterSpecialization", defaultValue: false, () => strings.AutoSetFilterSpecialization_Name, () => strings.AutoSetFilterSpecialization_Tooltip);
			ToggleWindowKey = internalSettings.DefineSetting("ToggleWindowKey", new KeyBinding(ModifierKeys.Shift, (Keys)66), () => string.Format(strings_common.ToggleItem, BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleName), () => string.Format(strings_common.ToggleItem, BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleName));
		}

		private void OnSettingCollectionChanged(object sender, ValueChangedEventArgs<SettingCollection> e)
		{
			if (e.NewValue != null)
			{
				InitializeSettings(e.NewValue);
			}
		}
	}
}
