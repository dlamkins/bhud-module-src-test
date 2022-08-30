using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nekres.Stopwatch.Core.Controllers;
using Nekres.Stopwatch.UI.Models;
using Nekres.Stopwatch.UI.Views;

namespace Stopwatch
{
	[Export(typeof(Module))]
	public class StopwatchModule : Module
	{
		internal static readonly Logger Logger = Logger.GetLogger<StopwatchModule>();

		internal SettingEntry<KeyBinding> Toggle;

		internal SettingEntry<bool> StartOnMovementEnabled;

		internal SettingEntry<KeyBinding> Reset;

		internal SettingEntry<KeyBinding> SetStartTime;

		internal SettingEntry<FontSize> FontSize;

		internal SettingEntry<float> BackgroundOpacity;

		internal SettingEntry<float> SoundVolume;

		internal SettingEntry<bool> TickingSoundDisabledSetting;

		internal SettingEntry<bool> BeepSoundDisabledSetting;

		internal SettingEntry<Color> FontColor;

		internal SettingEntry<Point> Position;

		internal SettingEntry<TimeSpan> StartTime;

		private StopwatchController _stopwatchController;

		internal static StopwatchModule ModuleInstance { get; private set; }

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public StopwatchModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected O, but got Unknown
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Expected O, but got Unknown
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Expected O, but got Unknown
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_038a: Unknown result type (might be due to invalid IL or missing references)
			SettingCollection hotkeys = settings.AddSubCollection("Control Options", true, false);
			Toggle = hotkeys.DefineSetting<KeyBinding>("toggleKey", new KeyBinding((Keys)164), (Func<string>)(() => "Toggle"), (Func<string>)(() => "Starts or pauses the stopwatch."));
			Reset = hotkeys.DefineSetting<KeyBinding>("resetKey", new KeyBinding((ModifierKeys)2, (Keys)82), (Func<string>)(() => "Reset"), (Func<string>)(() => "Rewinds and stops the stopwatch."));
			SetStartTime = hotkeys.DefineSetting<KeyBinding>("setStartTimeKey", new KeyBinding((ModifierKeys)2, (Keys)67), (Func<string>)(() => "Set Goal Time"), (Func<string>)(() => "Set a goal time and make the stopwatch count down into the negative."));
			SettingCollection general = settings.AddSubCollection("General", true, false);
			StartOnMovementEnabled = general.DefineSetting<bool>("startOnMovement", false, (Func<string>)(() => "Wait for Character Movement"), (Func<string>)(() => "When you activate the stopwatch it will delay its start until the moment you move from where you toggled it.\nIn competitive modes it will wait for camera movement instead."));
			FontSize = general.DefineSetting<FontSize>("fontSize", (FontSize)36, (Func<string>)(() => "Font Size"), (Func<string>)(() => "Sets the font size of the timer."));
			FontColor = general.DefineSetting<Color>("fontColor", Color.get_White(), (Func<string>)(() => "Font Color"), (Func<string>)(() => "Sets the font color of the timer."));
			BackgroundOpacity = general.DefineSetting<float>("backgroundOpacity", 30f, (Func<string>)(() => "Background Opacity"), (Func<string>)(() => "Sets the transparency of the background."));
			SettingCollection audio = settings.AddSubCollection("Sound Options", true, false);
			TickingSoundDisabledSetting = audio.DefineSetting<bool>("tickingSfxDisabled", false, (Func<string>)(() => "Disable Ticking Sound"), (Func<string>)(() => "Disables the ticking sounds"));
			BeepSoundDisabledSetting = audio.DefineSetting<bool>("beepSfxDisabled", false, (Func<string>)(() => "Disable Beep Alerts"), (Func<string>)(() => "Disables the beeping alerts during a count from three to zero."));
			SoundVolume = audio.DefineSetting<float>("soundVolume", 80f, (Func<string>)(() => "Audio Volume"), (Func<string>)(() => "Sets the volume of the audio effects"));
			SettingCollection hiddenSettingsCache = settings.AddSubCollection("hiddenSettingsCache", false, false);
			Position = hiddenSettingsCache.DefineSetting<Point>("position", new Point(180, 60), (Func<string>)null, (Func<string>)null);
			StartTime = hiddenSettingsCache.DefineSetting<TimeSpan>("startTime", TimeSpan.Zero, (Func<string>)null, (Func<string>)null);
		}

		protected override void Initialize()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			_stopwatchController = new StopwatchController
			{
				FontColor = FontColor.get_Value(),
				FontSize = FontSize.get_Value(),
				BackgroundOpacity = BackgroundOpacity.get_Value() / 100f,
				AudioVolume = Math.Min(1f, SoundVolume.get_Value() / 1000f),
				Position = Position.get_Value()
			};
		}

		protected override async Task LoadAsync()
		{
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new CustomSettingsView(new CustomSettingsModel(SettingsManager.get_ModuleSettings()));
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			Toggle.get_Value().set_Enabled(true);
			SetStartTime.get_Value().set_Enabled(true);
			Reset.get_Value().set_Enabled(true);
			Toggle.get_Value().add_Activated((EventHandler<EventArgs>)OnToggleActivated);
			SetStartTime.get_Value().add_Activated((EventHandler<EventArgs>)SetStartTimeActivated);
			Reset.get_Value().add_Activated((EventHandler<EventArgs>)OnResetActivated);
			SoundVolume.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnSoundVolumeSettingChanged);
			FontSize.add_SettingChanged((EventHandler<ValueChangedEventArgs<FontSize>>)OnFontSizeSettingChanged);
			FontColor.add_SettingChanged((EventHandler<ValueChangedEventArgs<Color>>)OnFontColorSettingChanged);
			BackgroundOpacity.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnBackgroundOpacitySettingChanged);
			Position.add_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)OnPositionSettingChanged);
			((Module)this).OnModuleLoaded(e);
		}

		private void OnToggleActivated(object o, EventArgs e)
		{
			if (GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus() && !GameService.Gw2Mumble.get_UI().get_IsTextInputFocused())
			{
				_stopwatchController.Toggle();
			}
		}

		private void SetStartTimeActivated(object o, EventArgs e)
		{
			if (GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus() && !GameService.Gw2Mumble.get_UI().get_IsTextInputFocused())
			{
				_stopwatchController.StartAt();
			}
		}

		private void OnResetActivated(object o, EventArgs e)
		{
			_stopwatchController.Reset();
		}

		private void OnSoundVolumeSettingChanged(object o, ValueChangedEventArgs<float> e)
		{
			_stopwatchController.AudioVolume = e.get_NewValue() / 1000f;
		}

		private void OnFontSizeSettingChanged(object o, ValueChangedEventArgs<FontSize> e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			_stopwatchController.FontSize = e.get_NewValue();
		}

		private void OnFontColorSettingChanged(object o, ValueChangedEventArgs<Color> e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			_stopwatchController.FontColor = e.get_NewValue();
		}

		private void OnBackgroundOpacitySettingChanged(object o, ValueChangedEventArgs<float> e)
		{
			_stopwatchController.BackgroundOpacity = e.get_NewValue() / 100f;
		}

		private void OnPositionSettingChanged(object o, ValueChangedEventArgs<Point> e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			_stopwatchController.Position = e.get_NewValue();
		}

		protected override void Update(GameTime gameTime)
		{
			_stopwatchController?.Update();
		}

		protected override void Unload()
		{
			Toggle.get_Value().remove_Activated((EventHandler<EventArgs>)OnToggleActivated);
			SetStartTime.get_Value().remove_Activated((EventHandler<EventArgs>)SetStartTimeActivated);
			Reset.get_Value().remove_Activated((EventHandler<EventArgs>)OnResetActivated);
			SoundVolume.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnSoundVolumeSettingChanged);
			FontSize.remove_SettingChanged((EventHandler<ValueChangedEventArgs<FontSize>>)OnFontSizeSettingChanged);
			FontColor.remove_SettingChanged((EventHandler<ValueChangedEventArgs<Color>>)OnFontColorSettingChanged);
			BackgroundOpacity.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnBackgroundOpacitySettingChanged);
			Position.remove_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)OnPositionSettingChanged);
			_stopwatchController?.Dispose();
			ModuleInstance = null;
		}
	}
}
