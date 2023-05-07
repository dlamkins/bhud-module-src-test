using System;

namespace Estreya.BlishHUD.Shared.Attributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class TranslationAttribute : Attribute
	{
		public string TranslationKey { get; }

		public string DefaultValue { get; }

		public TranslationAttribute(string translationKey, string defaultValue)
		{
			TranslationKey = translationKey;
			DefaultValue = defaultValue;
		}
	}
}
