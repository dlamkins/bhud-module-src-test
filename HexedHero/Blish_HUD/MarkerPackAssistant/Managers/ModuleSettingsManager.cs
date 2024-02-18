using System;
using Blish_HUD.Settings;
using HexedHero.Blish_HUD.MarkerPackAssistant.Objects;

namespace HexedHero.Blish_HUD.MarkerPackAssistant.Managers
{
	public class ModuleSettingsManager
	{
		private static Lazy<ModuleSettingsManager> instance = new Lazy<ModuleSettingsManager>(() => new ModuleSettingsManager());

		public static ModuleSettingsManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new Lazy<ModuleSettingsManager>(() => new ModuleSettingsManager());
				}
				return instance.Value;
			}
		}

		public SettingCollection Settings { get; private set; }

		public ModuleSettings ModuleSettings { get; private set; }

		private ModuleSettingsManager()
		{
			Load();
		}

		private void Load()
		{
		}

		public void Unload()
		{
			instance = null;
		}

		public void DefineSettings(SettingCollection settings)
		{
			ModuleSettings = new ModuleSettings(settings);
		}
	}
}
