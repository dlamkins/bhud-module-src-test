using System.Linq;
using Gw2Sharp.WebApi;

namespace Blish_HUD.Extended
{
	public static class LocaleExtensions
	{
		public static string Code(this Locale locale)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected I4, but got Unknown
			return (int)locale switch
			{
				0 => "en", 
				1 => "es", 
				2 => "de", 
				3 => "fr", 
				4 => "kr", 
				5 => "zh", 
				_ => "en", 
			};
		}

		public static Locale SupportedOrDefault(this Locale locale, params Locale[] supported)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected I4, but got Unknown
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			if (supported == null || !supported.Any())
			{
				return (Locale)((locale - 1) switch
				{
					0 => locale, 
					1 => locale, 
					2 => locale, 
					_ => 0, 
				});
			}
			if (!supported.Contains(locale))
			{
				return supported.FirstOrDefault();
			}
			return locale;
		}
	}
}
