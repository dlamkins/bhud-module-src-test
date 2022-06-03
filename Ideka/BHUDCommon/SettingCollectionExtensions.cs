using System;
using Blish_HUD.Input;
using Blish_HUD.Settings;

namespace Ideka.BHUDCommon
{
	internal static class SettingCollectionExtensions
	{
		public static GenericSetting<T> Generic<T>(this SettingCollection settings, string key, T defaultValue, Func<string> displayNameFunc = null, Func<string> descriptionFunc = null)
		{
			return new GenericSetting<T>(settings, key, defaultValue, displayNameFunc, descriptionFunc);
		}

		public static KeyBindingSetting KeyBinding(this SettingCollection settings, string key, KeyBinding defaultValue, Func<string> displayNameFunc = null, Func<string> descriptionFunc = null)
		{
			return new KeyBindingSetting(settings, key, defaultValue, displayNameFunc, descriptionFunc);
		}

		public static SliderSetting Slider(this SettingCollection settings, string key, int defaultValue, int minValue, int maxValue, Func<string> displayNameFunc = null, Func<string> descriptionFunc = null, Func<int, int, string> validationErrorFunc = null)
		{
			return new SliderSetting(settings, key, defaultValue, minValue, maxValue, displayNameFunc, descriptionFunc, validationErrorFunc);
		}
	}
}
