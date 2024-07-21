using System;
using Blish_HUD.Settings;

namespace MysticCrafting.Module.Update
{
	internal class ModuleUpdateHandler
	{
		private SettingEntry<Version> LastAcknowledgedUpdate { get; set; }

		private SettingEntry<bool> NotifyOfNewReleases { get; set; }

		private void DefineUpdateSettings(SettingCollection settingCollection)
		{
			LastAcknowledgedUpdate = settingCollection.DefineSetting<Version>("LastAcknowledgedRelease", new Version(0, 0, 0, 0), (Func<string>)null, (Func<string>)null);
			NotifyOfNewReleases = settingCollection.DefineSetting<bool>("NotifyOfNewRelease", true, (Func<string>)null, (Func<string>)null);
		}

		public void AcknowledgeUpdate(Version version)
		{
			LastAcknowledgedUpdate.set_Value(version);
		}
	}
}
