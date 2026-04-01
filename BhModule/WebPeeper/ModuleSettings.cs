using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using CefHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace BhModule.WebPeeper
{
	internal class ModuleSettings
	{
		private const string _defaultSearchUrl = "https://www.google.com/search?q={text} site:wiki.guildwars2.com";

		private const string _defaultHomeUrl = "https://wiki.guildwars2.com/";

		private const string _defaultBgColor = "#00000000";

		public SettingEntry<CefAvailableVersion> CefVersion { get; private set; }

		public SettingEntry<CefAvailableVersion> CefErrorVersion { get; private set; }

		public SettingEntry<KeyBinding> SettingsKey { get; private set; }

		public SettingEntry<KeyBinding> WebWindowKey { get; private set; }

		public SettingEntry<KeyBinding> ZoomInKey { get; private set; }

		public SettingEntry<KeyBinding> ZoomOutKey { get; private set; }

		public SettingEntry<KeyBinding> CaptureKeyboardKey { get; private set; }

		public SettingEntry<KeyBinding> BrowserDevToolsKey { get; private set; }

		public SettingEntry<string> HomeUrl { get; private set; }

		public SettingEntry<string> SearchUrl { get; private set; }

		public SettingEntry<string> WebBgColor { get; private set; }

		public SettingEntry<float> SoundVolume { get; private set; }

		public SettingEntry<float> WebWindowOpacity { get; private set; }

		public SettingEntry<bool> IsAutoPauseWeb { get; private set; }

		public SettingEntry<bool> IsAutoQuitProcess { get; private set; }

		public SettingEntry<bool> IsMobileLayout { get; private set; }

		public SettingEntry<bool> IsUseTouch { get; private set; }

		public SettingEntry<bool> IsCleanMode { get; private set; }

		public SettingEntry<bool> IsFollowBhFps { get; private set; }

		public SettingEntry<bool> IsBlockKeybinds { get; private set; }

		public SettingEntry<bool> IsShowWarning { get; private set; }

		private CefService CefService => WebPeeperModule.Instance.CefService;

		public ModuleSettings(SettingCollection settings)
		{
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Expected O, but got Unknown
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Expected O, but got Unknown
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Expected O, but got Unknown
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Expected O, but got Unknown
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Expected O, but got Unknown
			CefVersion = settings.DefineSetting<CefAvailableVersion>("CefVersion", CefAvailableVersion.v103, (Func<string>)(() => "CEF Version"), (Func<string>)(() => $"Browser core version, each version (excluding {CefService.DefaultVersion}) requires an additional 200 MB download."));
			CefVersion.add_SettingChanged((EventHandler<ValueChangedEventArgs<CefAvailableVersion>>)OnCefVersionChanged);
			CefErrorVersion = settings.DefineSetting<CefAvailableVersion>("CefErrorVersion", CefAvailableVersion.v103, (Func<string>)(() => ""), (Func<string>)(() => ""));
			SettingsKey = settings.DefineSetting<KeyBinding>("SettingsKey", new KeyBinding((ModifierKeys)1, (Keys)123), (Func<string>)(() => "Settings Toggle"), (Func<string>)(() => ""));
			SettingsKey.get_Value().add_Activated((EventHandler<EventArgs>)ToggleSettings);
			SettingsKey.get_Value().set_Enabled(true);
			WebWindowKey = settings.DefineSetting<KeyBinding>("WebWindowKey", new KeyBinding((Keys)123), (Func<string>)(() => "Web Window Toggle"), (Func<string>)(() => ""));
			WebWindowKey.get_Value().add_Activated((EventHandler<EventArgs>)ToggleWebWindow);
			WebWindowKey.get_Value().set_Enabled(true);
			ZoomInKey = settings.DefineSetting<KeyBinding>("ZoomInKey", new KeyBinding((ModifierKeys)1, (Keys)107), (Func<string>)(() => "Zoom In"), (Func<string>)(() => "Only works when the cursor is within the web area."));
			ZoomInKey.get_Value().add_Activated((EventHandler<EventArgs>)OnZoomInActivated);
			ZoomInKey.get_Value().set_Enabled(true);
			ZoomOutKey = settings.DefineSetting<KeyBinding>("ZoomOutKey", new KeyBinding((ModifierKeys)1, (Keys)109), (Func<string>)(() => "Zoom Out"), (Func<string>)(() => "Only works when the cursor is within the web area."));
			ZoomOutKey.get_Value().add_Activated((EventHandler<EventArgs>)OnZoomOutActivated);
			ZoomOutKey.get_Value().set_Enabled(true);
			CaptureKeyboardKey = settings.DefineSetting<KeyBinding>("CaptureKeyboardKey", new KeyBinding((ModifierKeys)1, (Keys)32), (Func<string>)(() => "Focus the Blish-HUD Window"), (Func<string>)(() => "For web input field, only works when the cursor is within the web area. In theory it would auto-focus when caret is flashing."));
			CaptureKeyboardKey.get_Value().add_Activated((EventHandler<EventArgs>)OnCaptureKeyboardActivated);
			CaptureKeyboardKey.get_Value().set_Enabled(true);
			SearchUrl = settings.DefineSetting<string>("SearchUrl", "https://www.google.com/search?q={text} site:wiki.guildwars2.com", (Func<string>)(() => "Search Engine"), (Func<string>)(() => "{text} is represent text variable."));
			SearchUrl.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnSearchUrlChanged);
			HomeUrl = settings.DefineSetting<string>("HomeUrl", "https://wiki.guildwars2.com/", (Func<string>)(() => "Home Page"), (Func<string>)(() => ""));
			HomeUrl.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnHomeUrlChanged);
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
			WebBgColor.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnWebBgColorChanged);
			SoundVolume = settings.DefineSetting<float>("SoundVolume", 1f, (Func<string>)(() => $"Sound Volume < {Math.Round(SoundVolume.get_Value(), 2) * 100.0} >"), (Func<string>)(() => ""));
			SettingComplianceExtensions.SetRange(SoundVolume, 0f, 1f);
			SoundVolume.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnSoundVolumeChanged);
			WebWindowOpacity = settings.DefineSetting<float>("WebWindowOpacity", 1f, (Func<string>)(() => $"Window Opacity < {Math.Round(WebWindowOpacity.get_Value(), 2)} >"), (Func<string>)(() => ""));
			SettingComplianceExtensions.SetRange(WebWindowOpacity, 0.1f, 1f);
			WebWindowOpacity.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnWebWindowOpacityChanged);
			IsAutoPauseWeb = settings.DefineSetting<bool>("IsAutoPauseWeb", false, (Func<string>)(() => "Pause the Web Process while Close the Web Window"), (Func<string>)(() => ""));
			IsAutoQuitProcess = settings.DefineSetting<bool>("IsAutoQuitProcess", false, (Func<string>)(() => "Quit the Web Process while Close the Web Window"), (Func<string>)(() => ""));
			IsAutoQuitProcess.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnIsAutoQuitProcessChanged);
			SettingComplianceExtensions.SetDisabled((SettingEntry)(object)IsAutoPauseWeb, IsAutoQuitProcess.get_Value());
			IsMobileLayout = settings.DefineSetting<bool>("IsMobileLayout", true, (Func<string>)(() => "Use Mobile Website"), (Func<string>)(() => "Whether use mobile User-Agent."));
			IsMobileLayout.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnIsMobileLayoutChanged);
			IsUseTouch = settings.DefineSetting<bool>("IsUseTouch", false, (Func<string>)(() => "Simulate Touch"), (Func<string>)(() => "Left mouse button send touch event instead. It is useful for mobile websites."));
			IsCleanMode = settings.DefineSetting<bool>("IsCleanMode", false, (Func<string>)(() => "Auto Clean User Data"), (Func<string>)(() => "Deletes all data of the previous session each time " + ((Module)WebPeeperModule.Instance).get_Name() + " opens."));
			IsFollowBhFps = settings.DefineSetting<bool>("IsFollowBhFps", false, (Func<string>)(() => "Same as Blish-HUD FPS Setting"), (Func<string>)(() => "Default is locked at 30 FPS, up to 60 FPS if checked."));
			IsFollowBhFps.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnIsFollowBhFpsChanged);
			IsBlockKeybinds = settings.DefineSetting<bool>("IsBlockKeybinds", true, (Func<string>)(() => "Block All Blish-HUD Keybinds while the Web is Accepting Input"), (Func<string>)(() => "Uncheck if keybinds fail after typing."));
			IsShowWarning = settings.DefineSetting<bool>("IsShowWarning", true, (Func<string>)(() => "Show Outdated Warning"), (Func<string>)(() => ""));
		}

		public void Load()
		{
			((Control)WebPeeperModule.InstanceSettingsMenuItem).add_PropertyChanged((PropertyChangedEventHandler)OnSettingsHidden);
			((Control)GameService.Overlay.get_BlishHudWindow()).add_Hidden((EventHandler<EventArgs>)OnSettingsHidden);
		}

		public void Unload()
		{
			SettingsKey.get_Value().remove_Activated((EventHandler<EventArgs>)ToggleSettings);
			WebWindowKey.get_Value().remove_Activated((EventHandler<EventArgs>)ToggleWebWindow);
			CaptureKeyboardKey.get_Value().remove_Activated((EventHandler<EventArgs>)OnCaptureKeyboardActivated);
			ZoomInKey.get_Value().remove_Activated((EventHandler<EventArgs>)OnZoomInActivated);
			ZoomOutKey.get_Value().remove_Activated((EventHandler<EventArgs>)OnZoomOutActivated);
			((Control)WebPeeperModule.InstanceSettingsMenuItem).remove_PropertyChanged((PropertyChangedEventHandler)OnSettingsHidden);
			((Control)GameService.Overlay.get_BlishHudWindow()).remove_Hidden((EventHandler<EventArgs>)OnSettingsHidden);
			WebPeeperSettingsView.UpdateSoundVolumeTitle = null;
			WebPeeperSettingsView.UpdateWebWindowOpacityTitle = null;
			WebPeeperSettingsView.UpdateIsAutoPauseWebState = null;
			CefVersionSettingView.UpdateView = null;
			WebPeeperSettingsView.DisposeRootFlowPanel?.Invoke();
			CefVersion.remove_SettingChanged((EventHandler<ValueChangedEventArgs<CefAvailableVersion>>)OnCefVersionChanged);
			SearchUrl.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnSearchUrlChanged);
			HomeUrl.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnHomeUrlChanged);
			WebBgColor.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnWebBgColorChanged);
			SoundVolume.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnSoundVolumeChanged);
			WebWindowOpacity.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnWebWindowOpacityChanged);
			IsAutoQuitProcess.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnIsAutoQuitProcessChanged);
			IsMobileLayout.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnIsMobileLayoutChanged);
			IsFollowBhFps.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnIsFollowBhFpsChanged);
		}

		private void OnCefVersionChanged(object sender, EventArgs e)
		{
			CefService.ApplySettingVersion();
		}

		private void OnSearchUrlChanged(object sender, ValueChangedEventArgs<string> e)
		{
			try
			{
				UriBuilder uriBuilder = new UriBuilder(e.get_NewValue());
				if (!($"{uriBuilder.Uri}" == e.get_NewValue()))
				{
					Task.Delay(10).ContinueWith(delegate
					{
						string absoluteUri;
						SearchUrl.set_Value(absoluteUri = uriBuilder.Uri.AbsoluteUri);
						return absoluteUri;
					});
				}
			}
			catch
			{
				Task.Delay(10).ContinueWith(delegate
				{
					string result;
					SearchUrl.set_Value(result = "https://www.google.com/search?q={text} site:wiki.guildwars2.com");
					return result;
				});
			}
		}

		private void OnHomeUrlChanged(object sender, ValueChangedEventArgs<string> e)
		{
			try
			{
				UriBuilder uriBuilder = new UriBuilder(e.get_NewValue());
				if (!($"{uriBuilder.Uri}" == e.get_NewValue()))
				{
					Task.Delay(10).ContinueWith(delegate
					{
						string absoluteUri;
						HomeUrl.set_Value(absoluteUri = uriBuilder.Uri.AbsoluteUri);
						return absoluteUri;
					});
				}
			}
			catch
			{
				Task.Delay(10).ContinueWith(delegate
				{
					string result;
					HomeUrl.set_Value(result = "https://wiki.guildwars2.com/");
					return result;
				});
			}
		}

		private void OnWebBgColorChanged(object sender, ValueChangedEventArgs<string> e)
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
		}

		private void OnSoundVolumeChanged(object sender, ValueChangedEventArgs<float> e)
		{
			WebPeeperSettingsView.UpdateSoundVolumeTitle?.Invoke();
			if (CefService.LibLoadStarted)
			{
				SetSoundVolume(e.get_NewValue());
			}
		}

		private void OnWebWindowOpacityChanged(object sender, ValueChangedEventArgs<float> e)
		{
			UiService uiService = WebPeeperModule.Instance.UiService;
			if (uiService != null && ((Control)uiService.BrowserWindow).get_Opacity() > 0f)
			{
				((Control)uiService.BrowserWindow).set_Opacity(e.get_NewValue());
			}
			WebPeeperSettingsView.UpdateWebWindowOpacityTitle?.Invoke();
		}

		private void OnIsAutoQuitProcessChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			SettingComplianceExtensions.SetDisabled((SettingEntry)(object)IsAutoPauseWeb, e.get_NewValue());
			WebPeeperSettingsView.UpdateIsAutoPauseWebState?.Invoke();
		}

		private void OnIsMobileLayoutChanged(object sender, EventArgs e)
		{
			if (CefService.LibLoadStarted)
			{
				CefService.ApplyUserAgent();
			}
		}

		private void OnIsFollowBhFpsChanged(object sender, EventArgs e)
		{
			if (CefService.LibLoadStarted)
			{
				CefService.ApplyFrameRate();
			}
		}

		private void OnSettingsHidden(object sender, EventArgs e)
		{
			PropertyChangedEventArgs propertyChangedEventArgs = e as PropertyChangedEventArgs;
			if (propertyChangedEventArgs == null || (!(propertyChangedEventArgs.PropertyName != "Selected") && !WebPeeperModule.InstanceSettingsMenuItem.get_Selected()))
			{
				WebPeeperModule.Instance.DownloadService.Download(CefService.Versions[CefVersion.get_Value()]);
			}
		}

		public int GetFrameRate()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Invalid comparison between Unknown and I4
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Invalid comparison between Unknown and I4
			int result = 30;
			if (IsFollowBhFps.get_Value())
			{
				FramerateMethod frameLimiter = GameService.Graphics.get_FrameLimiter();
				int num = (((int)frameLimiter == 1) ? 30 : (((int)frameLimiter != 2) ? 60 : 60));
				result = num;
			}
			return result;
		}

		public void RedownloadCef()
		{
			CefErrorVersion.set_Value(CefVersion.get_Value());
		}

		private void ToggleSettings(object sender, EventArgs e)
		{
			WebPeeperModule.Instance.UiService?.ToggleSettings();
		}

		private void ToggleWebWindow(object sender, EventArgs e)
		{
			WebPeeperModule.Instance.UiService?.ToggleBrowser();
		}

		private void ShowBrowserDevTools(object sender, EventArgs e)
		{
			if (CefService.LibLoadStarted)
			{
				CefService.ShowDevTools();
			}
		}

		private void OnCaptureKeyboardActivated(object sender, EventArgs e)
		{
			UiService uiService = WebPeeperModule.Instance.UiService;
			int num;
			if (uiService == null)
			{
				num = 1;
			}
			else
			{
				BrowserWindow browserWindow = uiService.BrowserWindow;
				num = ((!((browserWindow != null) ? new bool?(((Control)browserWindow).get_Visible()) : null).GetValueOrDefault()) ? 1 : 0);
			}
			if (num == 0 && CefService.LibLoadStarted)
			{
				FocusBHWindow();
			}
		}

		private void FocusBHWindow()
		{
			WebPeeperModule.Logger.Debug("ModuleSettings.FocusBHWindow: bring Blish.HUD to the foreground");
			if (((Game)WebPeeperModule.BlishHudInstance).get_Window().IsForeground())
			{
				WebPeeperModule.Logger.Debug("ModuleSettings.FocusBHWindow: Blish.HUD already foreground");
				return;
			}
			Utils.SetForegroundWindow(WebPeeperModule.BlishHudInstance.get_FormHandle());
			Browser.FocusBlurredElement();
		}

		private void OnZoomInActivated(object sender, EventArgs e)
		{
			WebPainter instance = WebPainter.Instance;
			if (instance != null && ((Control)instance).get_MouseOver())
			{
				ZoomInWeb();
			}
		}

		private void ZoomInWeb()
		{
			Browser.Zoom(1f);
		}

		private void OnZoomOutActivated(object sender, EventArgs e)
		{
			WebPainter instance = WebPainter.Instance;
			if (instance != null && ((Control)instance).get_MouseOver())
			{
				ZoomOutWeb();
			}
		}

		private void ZoomOutWeb()
		{
			Browser.Zoom(-1f);
		}

		private void SetSoundVolume(float val)
		{
			Browser.SetVolume(val);
		}
	}
}
