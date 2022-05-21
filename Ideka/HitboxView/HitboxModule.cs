using System;
using System.ComponentModel.Composition;
using Blish_HUD;
using Blish_HUD.Entities;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;

namespace Ideka.HitboxView
{
	[Export(typeof(Module))]
	public class HitboxModule : Module
	{
		private static readonly (int min, int max) PingRange = (0, 1000);

		private HitboxEntity _hitbox;

		private SettingEntry<Color> _hitboxColor;

		private SettingEntry<KeyBinding> _toggleHitboxKey;

		private SettingEntry<bool> _hitboxVisible;

		private SettingEntry<bool> _hitboxSmoothing;

		private SettingEntry<string> _gamePingString;

		private SettingEntry<int> _gamePing;

		private bool _reflecting;

		internal static SettingsManager SettingsManager => ((Module)Instance).ModuleParameters.get_SettingsManager();

		internal static ContentsManager ContentsManager => ((Module)Instance).ModuleParameters.get_ContentsManager();

		internal static DirectoriesManager DirectoriesManager => ((Module)Instance).ModuleParameters.get_DirectoriesManager();

		internal static Gw2ApiManager Gw2ApiManager => ((Module)Instance).ModuleParameters.get_Gw2ApiManager();

		private static HitboxModule Instance { get; set; }

		[ImportingConstructor]
		public HitboxModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Expected O, but got Unknown
			_hitboxColor = settings.DefineSetting<Color>("HitboxColor", Color.get_White(), (Func<string>)(() => Strings.SettingHitboxColor), (Func<string>)(() => Strings.SettingHitboxColorText));
			_toggleHitboxKey = settings.DefineSetting<KeyBinding>("ToggleHitboxKey", new KeyBinding(), (Func<string>)(() => Strings.SettingToggleHitboxKey), (Func<string>)(() => Strings.SettingToggleHitboxKeyText));
			_hitboxVisible = settings.DefineSetting<bool>("HitboxVisible", true, (Func<string>)(() => Strings.SettingHitboxVisible), (Func<string>)(() => Strings.SettingHitboxVisibleText));
			_hitboxSmoothing = settings.DefineSetting<bool>("HitboxSmoothing", true, (Func<string>)(() => Strings.SettingHitboxSmoothing), (Func<string>)(() => Strings.SettingHitboxSmoothingText));
			_gamePingString = settings.DefineSetting<string>("GamePingString", "100", (Func<string>)(() => Strings.SettingGamePing), (Func<string>)(() => Strings.SettingGamePingText));
			_gamePing = settings.DefineSetting<int>("GamePing", 100, (Func<string>)(() => " "), (Func<string>)(() => Strings.SettingGamePingText));
			SettingComplianceExtensions.SetRange(_gamePing, PingRange.min, PingRange.max);
			SettingComplianceExtensions.SetValidation<string>(_gamePingString, (Func<string, SettingValidationResult>)((string value) => new SettingValidationResult(ParsePingString(value, out var ping) && $"{ping}" == value.Trim(), string.Format(Strings.SettingGamePingValidation, PingRange.min, PingRange.max))));
		}

		protected override void Initialize()
		{
			_hitbox = new HitboxEntity
			{
				Smoothing = _hitboxSmoothing.get_Value(),
				Ping = _gamePing.get_Value()
			};
			if (_hitboxVisible.get_Value())
			{
				GameService.Graphics.get_World().AddEntity((IEntity)(object)_hitbox);
			}
			_hitboxColor.add_SettingChanged((EventHandler<ValueChangedEventArgs<Color>>)HitboxColorChanged);
			_toggleHitboxKey.get_Value().set_Enabled(true);
			_toggleHitboxKey.get_Value().add_Activated((EventHandler<EventArgs>)HitboxToggled);
			_hitboxVisible.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)HitboxVisibleChanged);
			_hitboxSmoothing.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)HitboxSmoothingChanged);
			_gamePingString.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)GamePingStringChanged);
			_gamePing.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)GamePingChanged);
		}

		private void HitboxColorChanged(object sender, ValueChangedEventArgs<Color> e)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			_hitbox.Color = _hitboxColor.get_Value();
		}

		private void HitboxToggled(object sender, EventArgs e)
		{
			_hitboxVisible.set_Value(!_hitboxVisible.get_Value());
		}

		private void HitboxVisibleChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			_hitbox.Reset();
			GameService.Graphics.get_World().RemoveEntity((IEntity)(object)_hitbox);
			if (_hitboxVisible.get_Value())
			{
				GameService.Graphics.get_World().AddEntity((IEntity)(object)_hitbox);
			}
		}

		private void HitboxSmoothingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			_hitbox.Smoothing = _hitboxSmoothing.get_Value();
		}

		private void GamePingStringChanged(object sender, ValueChangedEventArgs<string> e)
		{
			if (!_reflecting)
			{
				_reflecting = true;
				_gamePing.set_Value(ParsePingString(_gamePingString.get_Value(), out var ping) ? ping : _gamePing.get_Value());
				_reflecting = false;
				_hitbox.Ping = _gamePing.get_Value();
			}
		}

		private void GamePingChanged(object sender, ValueChangedEventArgs<int> e)
		{
			if (!_reflecting)
			{
				_reflecting = true;
				_gamePingString.set_Value($"{_gamePing.get_Value()}");
				_reflecting = false;
				_hitbox.Ping = _gamePing.get_Value();
			}
		}

		protected override void Unload()
		{
			_hitboxColor.remove_SettingChanged((EventHandler<ValueChangedEventArgs<Color>>)HitboxColorChanged);
			_toggleHitboxKey.get_Value().remove_Activated((EventHandler<EventArgs>)HitboxToggled);
			_hitboxVisible.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)HitboxVisibleChanged);
			_hitboxSmoothing.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)HitboxSmoothingChanged);
			_gamePingString.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)GamePingStringChanged);
			_gamePing.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)GamePingChanged);
			GameService.Graphics.get_World().RemoveEntity((IEntity)(object)_hitbox);
			_hitbox?.Dispose();
			Instance = null;
		}

		private static bool ParsePingString(string pingString, out int ping)
		{
			bool result = int.TryParse(pingString, out ping);
			ping = Math.Min(Math.Max(ping, PingRange.min), PingRange.max);
			return result;
		}
	}
}
