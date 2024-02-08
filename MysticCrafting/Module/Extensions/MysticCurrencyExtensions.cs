using System.Linq;
using Blish_HUD;
using MysticCrafting.Models.Commerce;

namespace MysticCrafting.Module.Extensions
{
	public static class MysticCurrencyExtensions
	{
		public static string LocalizedName(this MysticCurrency currency)
		{
			return currency.Localizations.FirstOrDefault((MysticCurrencyLocalization l) => l.Locale == GameService.Overlay.UserLocale.Value)?.Name ?? currency.Name;
		}

		public static string LocalizedDescription(this MysticCurrency currency)
		{
			return currency.Localizations.FirstOrDefault((MysticCurrencyLocalization l) => l.Locale == GameService.Overlay.UserLocale.Value)?.Description ?? currency.Description;
		}
	}
}
