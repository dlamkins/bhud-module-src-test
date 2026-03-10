using System;
using Blish_HUD.Settings;

namespace BhModule.PathingCategoryExplorerPlugin
{
	public class ModuleSettings
	{
		private readonly PathingCategoryExplorerPluginModule _module;

		public SettingEntry<bool> AddDeselectRecursively { get; private set; }

		public SettingEntry<bool> AddSelectRecursively { get; private set; }

		public SettingEntry<bool> AddDeselectAllOthers { get; private set; }

		public SettingEntry<bool> FixNodeExpansionBug { get; private set; }

		public ModuleSettings(PathingCategoryExplorerPluginModule module, SettingCollection settings)
		{
			_module = module;
			AddDeselectRecursively = settings.DefineSetting<bool>("AddDeselectRecursively", true, (Func<string>)(() => "Add Deselect Recursively"), (Func<string>)(() => ""));
			AddSelectRecursively = settings.DefineSetting<bool>("AddSelectRecursively", true, (Func<string>)(() => "Add Select Recursively"), (Func<string>)(() => ""));
			AddDeselectAllOthers = settings.DefineSetting<bool>("AddDeselectAllOthers", true, (Func<string>)(() => "Add Deselect All Othres"), (Func<string>)(() => ""));
			FixNodeExpansionBug = settings.DefineSetting<bool>("FixNodeExpansionBug", true, (Func<string>)(() => "Fix Node Expansion Bug"), (Func<string>)(() => "Fix crash when checking a node after parent re-expansion."));
		}

		public void Unload()
		{
			PathingCategoryExplorerPluginSettingsView.DisposeRootFlowPanel?.Invoke();
		}
	}
}
