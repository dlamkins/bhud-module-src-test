using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace BhModule.WebPeeper
{
	public class ModuleSettings
	{
		private const string _defaultSearchUrl = "https://www.google.com/search?q={text} site:wiki.guildwars2.com";

		private const string _defaultHomeUrl = "https://wiki.guildwars2.com/";

		private const string _defaultBgColor = "#00000000";

		public SettingEntry<KeyBinding> SettingsKey { get; private set; }

		public SettingEntry<KeyBinding> WebWindowKey { get; private set; }

		public SettingEntry<KeyBinding> ZoomInKey { get; private set; }

		public SettingEntry<KeyBinding> ZoomOutKey { get; private set; }

		public SettingEntry<KeyBinding> CaptureKeyboardKey { get; private set; }

		public SettingEntry<string> HomeUrl { get; private set; }

		public SettingEntry<string> SearchUrl { get; private set; }

		public SettingEntry<string> WebBgColor { get; private set; }

		public SettingEntry<float> WebWindowOpacity { get; private set; }

		public SettingEntry<bool> IsAutoPauseWeb { get; private set; }

		public SettingEntry<bool> IsAutoQuitProcess { get; private set; }

		public SettingEntry<bool> IsMobileLayout { get; private set; }

		public SettingEntry<bool> IsUseTouch { get; private set; }

		public SettingEntry<bool> IsCleanMode { get; private set; }

		public SettingEntry<bool> IsFollowBhFps { get; private set; }

		public SettingEntry<bool> IsBlockKeybinds { get; private set; }

		public ModuleSettings(SettingCollection settings)
		{
			InitUISetting(settings);
		}

		private void InitUISetting(SettingCollection settings)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Expected O, but got Unknown
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Expected O, but got Unknown
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Expected O, but got Unknown
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Expected O, but got Unknown
			SettingsKey = settings.DefineSetting<KeyBinding>("SettingsKey", new KeyBinding((ModifierKeys)1, (Keys)123), (Func<string>)(() => "Settings Toggle"), (Func<string>)(() => ""));
			SettingsKey.get_Value().add_Activated((EventHandler<EventArgs>)ToggleSettings);
			SettingsKey.get_Value().set_Enabled(true);
			WebWindowKey = settings.DefineSetting<KeyBinding>("WebWindowKey", new KeyBinding((Keys)123), (Func<string>)(() => "Web Window Toggle"), (Func<string>)(() => ""));
			WebWindowKey.get_Value().add_Activated((EventHandler<EventArgs>)ToggleWebWindow);
			WebWindowKey.get_Value().set_Enabled(true);
			ZoomInKey = settings.DefineSetting<KeyBinding>("ZoomInKey", new KeyBinding((ModifierKeys)1, (Keys)107), (Func<string>)(() => "Zoom In"), (Func<string>)(() => "Only works when the cursor is within the web area."));
			ZoomInKey.get_Value().add_Activated((EventHandler<EventArgs>)ZoomInWeb);
			ZoomInKey.get_Value().set_Enabled(true);
			ZoomOutKey = settings.DefineSetting<KeyBinding>("ZoomOutKey", new KeyBinding((ModifierKeys)1, (Keys)109), (Func<string>)(() => "Zoom Out"), (Func<string>)(() => "Only works when the cursor is within the web area."));
			ZoomOutKey.get_Value().add_Activated((EventHandler<EventArgs>)ZoomOutWeb);
			ZoomOutKey.get_Value().set_Enabled(true);
			CaptureKeyboardKey = settings.DefineSetting<KeyBinding>("CaptureKeyboardKey", new KeyBinding((ModifierKeys)1, (Keys)32), (Func<string>)(() => "Focus the Blish-HUD Window"), (Func<string>)(() => "For web input field, only works when the cursor is within the web area. In theory it would auto-focus when caret is flashing."));
			CaptureKeyboardKey.get_Value().add_Activated((EventHandler<EventArgs>)FocusBHWindow);
			CaptureKeyboardKey.get_Value().set_Enabled(true);
			SearchUrl = settings.DefineSetting<string>("SearchUrl", "https://www.google.com/search?q={text} site:wiki.guildwars2.com", (Func<string>)(() => "Search Engine"), (Func<string>)(() => "{text} is represent text variable."));
			SearchUrl.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object sender, ValueChangedEventArgs<string> e)
			{
				if (string.IsNullOrWhiteSpace(e.get_NewValue()))
				{
					Task.Delay(10).ContinueWith(delegate
					{
						string result3;
						SearchUrl.set_Value(result3 = "https://www.google.com/search?q={text} site:wiki.guildwars2.com");
						return result3;
					});
				}
			});
			HomeUrl = settings.DefineSetting<string>("HomeUrl", "https://wiki.guildwars2.com/", (Func<string>)(() => "Home Page"), (Func<string>)(() => ""));
			HomeUrl.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object s, ValueChangedEventArgs<string> e)
			{
				if (string.IsNullOrWhiteSpace(e.get_NewValue()))
				{
					Task.Delay(10).ContinueWith(delegate
					{
						string result2;
						HomeUrl.set_Value(result2 = "https://wiki.guildwars2.com/");
						return result2;
					});
				}
			});
			WebBgColor = settings.DefineSetting<string>("WebBgColor", "#00000000", (Func<string>)(() => "Web Background      "), (Func<string>)(() => "Default is transparent."));
			SettingComplianceExtensions.SetValidation<string>(WebBgColor, (Func<string, SettingValidationResult>)delegate(string color)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				try
				{
					ColorHelper.FromHex(color);
					return new SettingValidationResult(true, (string)null);
				}
				catch
				{
					return new SettingValidationResult(false, (string)null);
				}
			});
			WebBgColor.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object s, ValueChangedEventArgs<string> e)
			{
				if (string.IsNullOrWhiteSpace(e.get_NewValue()))
				{
					Task.Delay(10).ContinueWith(delegate
					{
						string result;
						WebBgColor.set_Value(result = "#00000000");
						return result;
					});
				}
				WebPainter.Instance?.ApplyBgTexture();
			});
			WebWindowOpacity = settings.DefineSetting<float>("WebWindowOpacity", 1f, (Func<string>)(() => $"Window Opacity < {Math.Round(WebWindowOpacity.get_Value(), 2)} >"), (Func<string>)(() => ""));
			SettingComplianceExtensions.SetRange(WebWindowOpacity, 0.1f, 1f);
			WebWindowOpacity.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)delegate(object sender, ValueChangedEventArgs<float> e)
			{
				UIService uIService = WebPeeperModule.Instance.UIService;
				if (uIService != null && ((Control)uIService.BrowserWindow).get_Opacity() > 0f)
				{
					((Control)uIService.BrowserWindow).set_Opacity(e.get_NewValue());
				}
				WebPeeperSettingsView.UpdateWebWindowOpacityTitle();
			});
			IsAutoPauseWeb = settings.DefineSetting<bool>("IsAutoPauseWeb", false, (Func<string>)(() => "Pause the Web Process while Close the Web Window"), (Func<string>)(() => ""));
			IsAutoQuitProcess = settings.DefineSetting<bool>("IsAutoQuitProcess", false, (Func<string>)(() => "Quit the Web Process while Close the Web Window"), (Func<string>)(() => ""));
			IsAutoQuitProcess.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object sender, ValueChangedEventArgs<bool> e)
			{
				SettingComplianceExtensions.SetDisabled((SettingEntry)(object)IsAutoPauseWeb, e.get_NewValue());
				WebPeeperSettingsView.UpdateIsAutoPauseWebState();
			});
			SettingComplianceExtensions.SetDisabled((SettingEntry)(object)IsAutoPauseWeb, IsAutoQuitProcess.get_Value());
			IsMobileLayout = settings.DefineSetting<bool>("IsMobileLayout", true, (Func<string>)(() => "Use Mobile Website"), (Func<string>)(() => "Whether use mobile User-Agent."));
			IsMobileLayout.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
			{
				WebPeeperModule.Instance.CefService.ApplyUserAgent();
			});
			IsUseTouch = settings.DefineSetting<bool>("IsUseTouch", false, (Func<string>)(() => "Simulate Touch"), (Func<string>)(() => "Left mouse button send touch event instead. It is useful for mobile websites."));
			IsCleanMode = settings.DefineSetting<bool>("IsCleanMode", false, (Func<string>)(() => "Auto Clean User-Data"), (Func<string>)(() => "Clear cache and user-data while WebPeeper module initialize."));
			IsFollowBhFps = settings.DefineSetting<bool>("IsFollowBhFps", false, (Func<string>)(() => "Same as Blish-HUD FPS Setting"), (Func<string>)(() => "Default is locked at 30 FPS, up to 60 FPS if unchecked."));
			IsBlockKeybinds = settings.DefineSetting<bool>("IsBlockKeybinds", true, (Func<string>)(() => "Block All Blish-HUD Keybinds while the Web is Accepting Input"), (Func<string>)(() => "Uncheck if keybinds fail after typing."));
		}

		public void Unload()
		{
			SettingsKey.get_Value().remove_Activated((EventHandler<EventArgs>)ToggleSettings);
			WebWindowKey.get_Value().remove_Activated((EventHandler<EventArgs>)ToggleWebWindow);
			CaptureKeyboardKey.get_Value().remove_Activated((EventHandler<EventArgs>)FocusBHWindow);
			ZoomInKey.get_Value().remove_Activated((EventHandler<EventArgs>)ZoomInWeb);
			ZoomOutKey.get_Value().remove_Activated((EventHandler<EventArgs>)ZoomOutWeb);
		}

		private void ToggleSettings(object sender, EventArgs e)
		{
			WebPeeperModule.Instance.UIService?.ToggleSettings();
		}

		private void ToggleWebWindow(object sender, EventArgs e)
		{
			WebPeeperModule.Instance.UIService?.ToggleBrowser();
		}

		private void FocusBHWindow(object sender, EventArgs e)
		{
			UIService uIService = WebPeeperModule.Instance.UIService;
			int num;
			if (uIService == null)
			{
				num = 0;
			}
			else
			{
				BrowserWindow browserWindow = uIService.BrowserWindow;
				num = ((((browserWindow != null) ? new bool?(((Control)browserWindow).get_Visible()) : null) == false) ? 1 : 0);
			}
			if (num == 0)
			{
				Utils.SetForegroundWindow(WebPeeperModule.BlishHudInstance.get_FormHandle());
				WebPeeperModule.Instance.CefService?.FocusBlurredElement();
			}
		}

		private void ZoomInWeb(object sender, EventArgs e)
		{
			if (WebPainter.Instance != null && ((Control)WebPainter.Instance).get_MouseOver())
			{
				WebPainter.Instance.Zoom(1f);
			}
		}

		private void ZoomOutWeb(object sender, EventArgs e)
		{
			if (WebPainter.Instance != null && ((Control)WebPainter.Instance).get_MouseOver())
			{
				WebPainter.Instance.Zoom(-1f);
			}
		}
	}
}
