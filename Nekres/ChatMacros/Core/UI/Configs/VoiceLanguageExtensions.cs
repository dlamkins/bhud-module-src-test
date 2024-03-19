using System.Globalization;
using Nekres.ChatMacros.Properties;

namespace Nekres.ChatMacros.Core.UI.Configs
{
	public static class VoiceLanguageExtensions
	{
		public static CultureInfo Culture(this VoiceLanguage lang)
		{
			return lang switch
			{
				VoiceLanguage.English => CultureInfo.GetCultureInfo(9), 
				VoiceLanguage.German => CultureInfo.GetCultureInfo(7), 
				VoiceLanguage.French => CultureInfo.GetCultureInfo(12), 
				VoiceLanguage.Spanish => CultureInfo.GetCultureInfo(10), 
				_ => null, 
			};
		}

		public static string ToDisplayString(this VoiceLanguage lang)
		{
			return lang switch
			{
				VoiceLanguage.English => Resources.English, 
				VoiceLanguage.German => Resources.German, 
				VoiceLanguage.French => Resources.French, 
				VoiceLanguage.Spanish => Resources.Spanish, 
				_ => string.Empty, 
			};
		}
	}
}
