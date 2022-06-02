using System;
using System.ComponentModel.Composition;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;

namespace Ideka.HitboxView
{
	[Export(typeof(Module))]
	public class HitboxModule : Module
	{
		private HitboxDraw _hitbox;

		private SettingEntry<Color> _hitboxColor;

		private SettingEntry<KeyBinding> _toggleHitboxKey;

		private SettingEntry<bool> _hitboxVisible;

		private SettingEntry<bool> _hitboxSmoothing;

		private SliderEntry _gamePing;

		[ImportingConstructor]
		public HitboxModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
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
			_gamePing = new SliderEntry(settings, "GamePing", 100, 0, 1000, () => Strings.SettingGamePing, () => Strings.SettingGamePingText, (int min, int max) => string.Format(Strings.SettingGamePingValidation, min, max));
		}

		protected override void Initialize()
		{
			HitboxDraw hitboxDraw = new HitboxDraw();
			((Control)hitboxDraw).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			hitboxDraw.Smoothing = _hitboxSmoothing.get_Value();
			hitboxDraw.Ping = _gamePing.Value;
			_hitbox = hitboxDraw;
			if (_hitboxVisible.get_Value())
			{
				((Control)_hitbox).Show();
			}
			else
			{
				((Control)_hitbox).Hide();
			}
			_hitboxColor.add_SettingChanged((EventHandler<ValueChangedEventArgs<Color>>)HitboxColorChanged);
			_toggleHitboxKey.get_Value().set_Enabled(true);
			_toggleHitboxKey.get_Value().add_Activated((EventHandler<EventArgs>)HitboxToggled);
			_hitboxVisible.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)HitboxVisibleChanged);
			_hitboxSmoothing.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)HitboxSmoothingChanged);
			_gamePing.Changed = delegate(int value)
			{
				_hitbox.Ping = value;
			};
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
			if (_hitboxVisible.get_Value())
			{
				((Control)_hitbox).Show();
			}
			else
			{
				((Control)_hitbox).Hide();
			}
		}

		private void HitboxSmoothingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			_hitbox.Smoothing = _hitboxSmoothing.get_Value();
		}

		protected override void Unload()
		{
			_hitboxColor.remove_SettingChanged((EventHandler<ValueChangedEventArgs<Color>>)HitboxColorChanged);
			_toggleHitboxKey.get_Value().remove_Activated((EventHandler<EventArgs>)HitboxToggled);
			_hitboxVisible.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)HitboxVisibleChanged);
			_hitboxSmoothing.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)HitboxSmoothingChanged);
			_gamePing?.Dispose();
			HitboxDraw hitbox = _hitbox;
			if (hitbox != null)
			{
				((Control)hitbox).Dispose();
			}
		}
	}
}
