using Blish_HUD;
using Gw2Sharp.WebApi;

namespace Kenedia.Modules.Core.Interfaces
{
	public interface ILocalizable
	{
		void UserLocale_SettingChanged(object sender = null, ValueChangedEventArgs<Locale> e = null);
	}
}
