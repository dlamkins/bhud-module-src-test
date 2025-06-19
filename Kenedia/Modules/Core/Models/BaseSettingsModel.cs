using System;
using Blish_HUD.Settings;

namespace Kenedia.Modules.Core.Models
{
	public class BaseSettingsModel : IDisposable
	{
		private bool _isDisposed;

		public SettingCollection SettingCollection { get; }

		public BaseSettingsModel(SettingCollection settingCollection)
		{
			SettingCollection = settingCollection;
			InitializeSettings(settingCollection);
		}

		protected virtual void InitializeSettings(SettingCollection settings)
		{
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				OnDispose();
			}
		}

		protected virtual void OnDispose()
		{
		}
	}
}
