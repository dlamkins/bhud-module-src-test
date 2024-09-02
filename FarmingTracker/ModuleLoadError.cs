using System;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;

namespace FarmingTracker
{
	public class ModuleLoadError : IDisposable
	{
		private ErrorSettingsView _errorSettingsView;

		private ErrorWindow _errorWindow;

		public bool HasModuleLoadFailed { get; set; }

		public void InitializeErrorSettingsViewAndShowErrorWindow(string errorWindowTitle, string errorText)
		{
			HasModuleLoadFailed = true;
			_errorSettingsView = new ErrorSettingsView(errorText);
			_errorWindow = new ErrorWindow(errorWindowTitle, errorText);
			((Control)_errorWindow).Show();
		}

		public IView CreateErrorSettingsView()
		{
			return (IView)(object)_errorSettingsView;
		}

		public void Dispose()
		{
			ErrorWindow errorWindow = _errorWindow;
			if (errorWindow != null)
			{
				((Control)errorWindow).Dispose();
			}
		}
	}
}
