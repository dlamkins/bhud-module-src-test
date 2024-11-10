using Blish_HUD.Input;
using Blish_HUD.Settings;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Res;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.BuildsManager.Services
{
	public class Settings : BaseSettingsModel
	{
		public SettingEntry<TemplateSortBehavior> SortBehavior { get; set; }

		public SettingEntry<Point> MainWindowLocation { get; set; }

		public SettingEntry<bool> ShowCornerIcon { get; set; }

		public SettingEntry<bool> SetFilterOnTemplateCreate { get; set; }

		public SettingEntry<bool> ResetFilterOnTemplateCreate { get; set; }

		public SettingEntry<bool> RequireVisibleTemplate { get; set; }

		public SettingEntry<bool> ShowQuickFilterPanelOnTabOpen { get; set; }

		public SettingEntry<bool> ShowQuickFilterPanelOnWindowOpen { get; set; }

		public SettingEntry<bool> AutoSetFilterProfession { get; set; }

		public SettingEntry<bool> AutoSetFilterSpecialization { get; private set; }

		public SettingEntry<double> QuickFiltersPanelFadeDelay { get; private set; }

		public SettingEntry<double> QuickFiltersPanelFadeDuration { get; private set; }

		public SettingEntry<KeyBinding> ToggleWindowKey { get; set; }

		public SettingEntry<bool> QuickFiltersPanelFade { get; private set; }

		public Settings(SettingCollection settingCollection)
			: base(settingCollection)
		{
		}

		protected override void InitializeSettings(SettingCollection settings)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			base.InitializeSettings(settings);
			SettingCollection internalSettings = settings.AddSubCollection("Internal", renderInUi: false, lazyLoaded: false);
			MainWindowLocation = internalSettings.DefineSetting<Point>("MainWindowLocation", new Point(100, 100));
			SortBehavior = internalSettings.DefineSetting("SortBehavior", TemplateSortBehavior.ByProfession);
			ShowQuickFilterPanelOnWindowOpen = internalSettings.DefineSetting("ShowQuickFilterPanelOnWindowOpen", defaultValue: false);
			ShowQuickFilterPanelOnTabOpen = internalSettings.DefineSetting("ShowQuickFilterPanelOnTabOpen", defaultValue: true);
			ShowCornerIcon = internalSettings.DefineSetting("ShowCornerIcon", defaultValue: true);
			RequireVisibleTemplate = internalSettings.DefineSetting("RequireVisibleTemplate", defaultValue: true);
			SetFilterOnTemplateCreate = internalSettings.DefineSetting("SetFilterOnTemplateCreate", defaultValue: false);
			ResetFilterOnTemplateCreate = internalSettings.DefineSetting("ResetFilterOnTemplateCreate", defaultValue: true);
			QuickFiltersPanelFade = internalSettings.DefineSetting("QuickFiltersPanelFade", defaultValue: true);
			QuickFiltersPanelFadeDuration = internalSettings.DefineSetting("QuickFiltersPanelFadeDuration", 1000.0);
			QuickFiltersPanelFadeDelay = internalSettings.DefineSetting("QuickFiltersPanelFadeDelay", 5000.0);
			AutoSetFilterProfession = internalSettings.DefineSetting("AutoSetFilterProfession", defaultValue: false, () => strings.AutoSetProfession_Name, () => strings.AutoSetProfession_Tooltip);
			AutoSetFilterSpecialization = internalSettings.DefineSetting("AutoSetFilterSpecialization", defaultValue: false, () => strings.AutoSetFilterSpecialization_Name, () => strings.AutoSetFilterSpecialization_Tooltip);
			ToggleWindowKey = internalSettings.DefineSetting("ToggleWindowKey", new KeyBinding(ModifierKeys.Shift, (Keys)66), () => string.Format(strings_common.ToggleItem, BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleName), () => string.Format(strings_common.ToggleItem, BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleName));
		}
	}
}
