using System;
using System.Globalization;
using Blish_HUD;
using flakysalt.CharacterKeybinds.Resources;

namespace flakysalt.CharacterKeybinds.Services
{
	public class LocaService : IDisposable
	{
		public static LocaService Instance;

		public event EventHandler<EventArgs> LocaleChanged;

		public LocaService()
		{
			Instance = this;
			GameService.Overlay.add_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnLocaleChanged);
		}

		private void OnLocaleChanged(object sender, ValueEventArgs<CultureInfo> e)
		{
			Loca.Culture = e.get_Value();
			this.LocaleChanged?.Invoke(this, EventArgs.Empty);
		}

		public void Dispose()
		{
			GameService.Overlay.remove_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnLocaleChanged);
		}
	}
}
