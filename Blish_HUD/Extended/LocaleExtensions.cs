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
	}
}
