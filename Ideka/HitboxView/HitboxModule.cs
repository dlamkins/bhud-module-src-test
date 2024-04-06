using System.ComponentModel.Composition;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Ideka.BHUDCommon;
using Ideka.NetCommon;
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

		private readonly DisposableCollection _dc = new DisposableCollection();

		[ImportingConstructor]
		public HitboxModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Expected O, but got Unknown
			_hitboxColor = _dc.Add(settings.Generic<Color>("HitboxColor", Color.get_White(), () => Strings.SettingHitboxColor, () => Strings.SettingHitboxColorText));
			_hitboxOutlineColor = _dc.Add(settings.Generic<Color>("HitboxOutlineColor", Color.get_Black(), () => Strings.SettingHitboxOutlineColor, () => Strings.SettingHitboxOutlineColorText));
			_toggleHitboxKey = _dc.Add(settings.KeyBinding("ToggleHitboxKey", new KeyBinding(), () => Strings.SettingToggleHitboxKey, () => Strings.SettingToggleHitboxKeyText));
			_hitboxVisible = _dc.Add(settings.Generic("HitboxVisible", defaultValue: true, () => Strings.SettingHitboxVisible, () => Strings.SettingHitboxVisibleText));
			_hitboxSmoothing = _dc.Add(settings.Generic("HitboxSmoothing", defaultValue: true, () => Strings.SettingHitboxSmoothing, () => Strings.SettingHitboxSmoothingText));
			_gamePing = _dc.Add(settings.Slider("GamePing", 100, 0, 1000, () => Strings.SettingGamePing, () => Strings.SettingGamePingText, (int min, int max) => string.Format(Strings.SettingGamePingValidation, min, max)));
		}

		protected override void Initialize()
		{
			DisposableCollection dc = _dc;
			HitboxDraw hitboxDraw = new HitboxDraw();
			((Control)hitboxDraw).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_hitbox = dc.Add<HitboxDraw>(hitboxDraw);
			_dc.Add(_hitboxColor.OnChangedAndNow(delegate(Color value)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				_hitbox.Color = value;
			}));
			_dc.Add(_hitboxOutlineColor.OnChangedAndNow(delegate(Color value)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				_hitbox.OutlineColor = value;
			}));
			_dc.Add(_toggleHitboxKey.OnActivated(delegate
			{
				_hitboxVisible.Value = !_hitboxVisible.Value;
			}));
			_dc.Add(_hitboxVisible.OnChangedAndNow(delegate(bool value)
			{
				((Control)_hitbox).set_Visible(value);
			}));
			_dc.Add(_hitboxSmoothing.OnChangedAndNow(delegate(bool value)
			{
				_hitbox.Smoothing = value;
			}));
			_dc.Add(_gamePing.OnChangedAndNow(delegate(int value)
			{
				_hitbox.Ping = value;
			}));
		}

		protected override void Unload()
		{
			_dc.Dispose();
		}
	}
}
