using System.ComponentModel.Composition;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Ideka.BHUDCommon;
using Microsoft.Xna.Framework;

namespace Ideka.HitboxView
{
	[Export(typeof(Module))]
	public class HitboxModule : Module
	{
		private HitboxDraw _hitbox;

		private GenericSetting<Color> _hitboxColor;

		private GenericSetting<Color> _hitboxOutlineColor;

		private KeyBindingSetting _toggleHitboxKey;

		private GenericSetting<bool> _hitboxVisible;

		private GenericSetting<bool> _hitboxSmoothing;

		private SliderSetting _gamePing;

		[ImportingConstructor]
		public HitboxModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Expected O, but got Unknown
			_hitboxColor = settings.Generic<Color>("HitboxColor", Color.get_White(), () => Strings.SettingHitboxColor, () => Strings.SettingHitboxColorText);
			_hitboxOutlineColor = settings.Generic<Color>("HitboxOutlineColor", Color.get_Black(), () => Strings.SettingHitboxOutlineColor, () => Strings.SettingHitboxOutlineColorText);
			_toggleHitboxKey = settings.KeyBinding("ToggleHitboxKey", new KeyBinding(), () => Strings.SettingToggleHitboxKey, () => Strings.SettingToggleHitboxKeyText);
			_hitboxVisible = settings.Generic("HitboxVisible", defaultValue: true, () => Strings.SettingHitboxVisible, () => Strings.SettingHitboxVisibleText);
			_hitboxSmoothing = settings.Generic("HitboxSmoothing", defaultValue: true, () => Strings.SettingHitboxSmoothing, () => Strings.SettingHitboxSmoothingText);
			_gamePing = settings.Slider("GamePing", 100, 0, 1000, () => Strings.SettingGamePing, () => Strings.SettingGamePingText, (int min, int max) => string.Format(Strings.SettingGamePingValidation, min, max));
		}

		protected override void Initialize()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			HitboxDraw hitboxDraw = new HitboxDraw();
			((Control)hitboxDraw).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			hitboxDraw.Color = _hitboxColor.Value;
			hitboxDraw.OutlineColor = _hitboxOutlineColor.Value;
			((Control)hitboxDraw).set_Visible(_hitboxVisible.Value);
			hitboxDraw.Smoothing = _hitboxSmoothing.Value;
			hitboxDraw.Ping = _gamePing.Value;
			_hitbox = hitboxDraw;
			_hitboxColor.Changed = delegate(Color value)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				_hitbox.Color = value;
			};
			_hitboxOutlineColor.Changed = delegate(Color value)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				_hitbox.OutlineColor = value;
			};
			_toggleHitboxKey.OnTrigger(delegate
			{
				_hitboxVisible.Value = !_hitboxVisible.Value;
			});
			_hitboxVisible.Changed = delegate(bool value)
			{
				((Control)_hitbox).set_Visible(value);
			};
			_hitboxSmoothing.Changed = delegate(bool value)
			{
				_hitbox.Smoothing = value;
			};
			_gamePing.Changed = delegate(int value)
			{
				_hitbox.Ping = value;
			};
		}

		protected override void Unload()
		{
			_hitboxColor?.Dispose();
			_toggleHitboxKey?.Dispose();
			_hitboxVisible?.Dispose();
			_hitboxSmoothing?.Dispose();
			_gamePing?.Dispose();
			HitboxDraw hitbox = _hitbox;
			if (hitbox != null)
			{
				((Control)hitbox).Dispose();
			}
		}
	}
}
