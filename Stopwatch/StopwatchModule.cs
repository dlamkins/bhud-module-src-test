using System;
using System.ComponentModel.Composition;
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

		internal SettingEntry<KeyBinding> Start;

		internal SettingEntry<KeyBinding> Stop;

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
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected O, but got Unknown
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Expected O, but got Unknown
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Expected O, but got Unknown
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Expected O, but got Unknown
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Expected O, but got Unknown
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_0435: Unknown result type (might be due to invalid IL or missing references)
			SettingCollection hotkeys = settings.AddSubCollection("Control Options", true, false);
			Toggle = hotkeys.DefineSetting<KeyBinding>("toggleKey", new KeyBinding((ModifierKeys)2, (Keys)220), (Func<string>)(() => "Toggle"), (Func<string>)(() => "Starts or stops the stopwatch."));
			Start = hotkeys.DefineSetting<KeyBinding>("startKey", new KeyBinding((Keys)0), (Func<string>)(() => "Start"), (Func<string>)(() => "Starts the stopwatch."));
			Stop = hotkeys.DefineSetting<KeyBinding>("stopKey", new KeyBinding((Keys)0), (Func<string>)(() => "Stop"), (Func<string>)(() => "Stops the stopwatch."));
			Reset = hotkeys.DefineSetting<KeyBinding>("resetKey", new KeyBinding((ModifierKeys)2, (Keys)27), (Func<string>)(() => "Reset"), (Func<string>)(() => "Rewinds the stopwatch."));
			SetStartTime = hotkeys.DefineSetting<KeyBinding>("setStartTimeKey", new KeyBinding((ModifierKeys)2, (Keys)9), (Func<string>)(() => "Set Goal Time"), (Func<string>)(() => "Set a goal time and make the stopwatch count down into the negative."));
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

		public override IView GetSettingsView()
		{
			return (IView)(object)new CustomSettingsView(new CustomSettingsModel(SettingsManager.get_ModuleSettings()));
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			Toggle.get_Value().set_Enabled(true);
			Start.get_Value().set_Enabled(true);
			Stop.get_Value().set_Enabled(true);
			SetStartTime.get_Value().set_Enabled(true);
			Reset.get_Value().set_Enabled(true);
			Toggle.get_Value().add_Activated((EventHandler<EventArgs>)OnToggleActivated);
			Stop.get_Value().add_Activated((EventHandler<EventArgs>)OnStopActivated);
			Start.get_Value().add_Activated((EventHandler<EventArgs>)OnStartActivated);
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
				if (_stopwatchController.IsRunning)
				{
					_stopwatchController.Stop();
				}
				else
				{
					_stopwatchController.Start(StartTime.get_Value());
				}
			}
		}

		private void OnStopActivated(object o, EventArgs e)
		{
			if (GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus() && !GameService.Gw2Mumble.get_UI().get_IsTextInputFocused() && _stopwatchController.IsRunning)
			{
				_stopwatchController.Stop();
			}
		}

		private void OnStartActivated(object o, EventArgs e)
		{
			if (GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus() && !GameService.Gw2Mumble.get_UI().get_IsTextInputFocused() && !_stopwatchController.IsRunning)
			{
				_stopwatchController.Start();
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
			Stop.get_Value().remove_Activated((EventHandler<EventArgs>)OnStopActivated);
			Start.get_Value().remove_Activated((EventHandler<EventArgs>)OnStartActivated);
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
