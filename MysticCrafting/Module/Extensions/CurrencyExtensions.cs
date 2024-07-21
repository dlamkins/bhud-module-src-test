using System.Linq;
using Atzie.MysticCrafting.Models.Currencies;
using Blish_HUD;

namespace MysticCrafting.Module.Extensions
{
	public static class CurrencyExtensions
	{
		public static string LocalizedName(this Currency currency)
		{
			return currency.Localizations?.FirstOrDefault((CurrencyLocalization l) => l.Locale == GameService.Overlay.get_UserLocale().get_Value())?.Name ?? currency?.Name ?? string.Empty;
		}

		public static string LocalizedDescription(this Currency currency)
		{
			return currency.Localizations?.FirstOrDefault((CurrencyLocalization l) => l.Locale == GameService.Overlay.get_UserLocale().get_Value())?.Description ?? currency.Description;
		}
	}
}
