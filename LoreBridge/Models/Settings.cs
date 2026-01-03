using System;
using Blish_HUD.Input;
using Blish_HUD.Settings;

namespace LoreBridge.Models
{
	public class Settings
	{
		public readonly SettingEntry<bool> ContentUsePolicyConfirmation = settings.DefineSetting<bool>("ContentUsePolicyConfirmation", false, (Func<string>)null, (Func<string>)null);

		public readonly SettingEntry<int> TranslationLanguage = settings.DefineSetting<int>("Translation.Language", 19, (Func<string>)null, (Func<string>)null);

		public readonly SettingEntry<int> TranslationTranslator = settings.DefineSetting<int>("Translation.Translator", 2, (Func<string>)null, (Func<string>)null);

		public readonly SettingEntry<string> TranslationLibreTranslateUrl = settings.DefineSetting<string>("Translation.Translator.LibreTranslate.Url", "", (Func<string>)null, (Func<string>)null);

		public readonly SettingEntry<int> AreaFontSize = settings.DefineSetting<int>("Area.FontSize", 20, (Func<string>)null, (Func<string>)null);

		public readonly SettingEntry<KeyBinding> ToggleCapturerHotkey = settings.DefineSetting<KeyBinding>("Hotkeys.ToggleCapturer", new KeyBinding(), (Func<string>)(() => "Select translation area"), (Func<string>)null);

		public readonly SettingEntry<KeyBinding> ToggleTranslationWindowHotKey = settings.DefineSetting<KeyBinding>("Hotkeys.ToggleTranslationWindow", new KeyBinding(), (Func<string>)(() => "Toggle chat window"), (Func<string>)null);

		public readonly SettingEntry<bool> TranslationAutoTranslateNpcDialogs = settings.DefineSetting<bool>("Translation.AutoTranslateNpcDialogs", false, (Func<string>)null, (Func<string>)null);

		public readonly SettingEntry<bool> WindowColoredNames = settings.DefineSetting<bool>("Window.ColoredNames", true, (Func<string>)null, (Func<string>)null);

		public readonly SettingEntry<bool> WindowFixed = settings.DefineSetting<bool>("Window.Fixed", false, (Func<string>)null, (Func<string>)null);

		public readonly SettingEntry<int> WindowFontSize = settings.DefineSetting<int>("Window.FontSize", 20, (Func<string>)null, (Func<string>)null);

		public readonly SettingEntry<int> WindowHeight = settings.DefineSetting<int>("Window.Height", 240, (Func<string>)null, (Func<string>)null);

		public readonly SettingEntry<int> WindowLocationX = settings.DefineSetting<int>("Window.Location.X", 200, (Func<string>)null, (Func<string>)null);

		public readonly SettingEntry<int> WindowLocationY = settings.DefineSetting<int>("Window.Location.Y", 200, (Func<string>)null, (Func<string>)null);

		public readonly SettingEntry<bool> WindowShowTime = settings.DefineSetting<bool>("Window.ShowTime", true, (Func<string>)null, (Func<string>)null);

		public readonly SettingEntry<bool> WindowTransparent = settings.DefineSetting<bool>("Window.Transparent", false, (Func<string>)null, (Func<string>)null);

		public readonly SettingEntry<bool> WindowVisible = settings.DefineSetting<bool>("Window.Visible", false, (Func<string>)null, (Func<string>)null);

		public readonly SettingEntry<int> WindowWidth = settings.DefineSetting<int>("Window.Width", 480, (Func<string>)null, (Func<string>)null);

		public Settings(SettingCollection settings)
		{
		}//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown

	}
}
