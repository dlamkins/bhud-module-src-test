using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Gw2Mumble;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Kenedia.Modules.QoL.Res;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.QoL.SubModules.ZoomOut
{
	public class ZoomOut : SubModule
	{
		private float _distance;

		private float _fieldOfView;

		private double _ticks;

		private double _saveDistanceTicks;

		private SettingEntry<KeyBinding> _zoomOutKey;

		private SettingEntry<KeyBinding> _manualMaxZoom;

		private SettingEntry<bool> _zoomOnCameraChange;

		private SettingEntry<bool> _allowManualZoom;

		private SettingEntry<bool> _zoomOnFoVChange;

		public bool MouseScrolled { get; private set; }

		public override SubModuleType SubModuleType => SubModuleType.ZoomOut;

		public ZoomOut(SettingCollection settings)
			: base(settings)
		{
		}

		public override void Update(GameTime gameTime)
		{
			if (!base.Enabled)
			{
				return;
			}
			Gw2MumbleService Mumble = GameService.Gw2Mumble;
			if (!Mumble.get_UI().get_IsMapOpen() && Mumble.get_Info().get_IsGameFocused() && !(gameTime.get_TotalGameTime().TotalMilliseconds - _ticks < 25.0))
			{
				if (gameTime.get_TotalGameTime().TotalMilliseconds - _saveDistanceTicks < 0.0)
				{
					_distance = ComputeCameraDistance();
				}
				_ticks = gameTime.get_TotalGameTime().TotalMilliseconds;
				double threshold = 1.0;
				float distance = ComputeCameraDistance();
				float delta = Math.Max(_distance, distance) - Math.Min(_distance, distance);
				if (_zoomOnCameraChange.get_Value() && (double)delta >= threshold)
				{
					ZoomCameraOut(distance);
					_distance = distance;
				}
				if (_zoomOnFoVChange.get_Value() && Mumble.get_PlayerCamera().get_FieldOfView() != _fieldOfView)
				{
					ZoomCameraOut(distance);
					_distance = distance;
					_fieldOfView = Mumble.get_PlayerCamera().get_FieldOfView();
				}
			}
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			base.DefineSettings(settings);
			_zoomOutKey = Settings.DefineSetting<KeyBinding>("_zoomOutKey", new KeyBinding((Keys)34), (Func<string>)null, (Func<string>)null);
			_manualMaxZoom = Settings.DefineSetting<KeyBinding>("_manualMaxZoom", new KeyBinding((Keys)0), (Func<string>)null, (Func<string>)null);
			_zoomOnCameraChange = settings.DefineSetting<bool>("_zoomOnCameraChange", true, (Func<string>)null, (Func<string>)null);
			_allowManualZoom = settings.DefineSetting<bool>("_allowManualZoom", true, (Func<string>)null, (Func<string>)null);
			_zoomOnFoVChange = settings.DefineSetting<bool>("_zoomOnFoVChange", true, (Func<string>)null, (Func<string>)null);
			_manualMaxZoom.get_Value().set_Enabled(true);
			_manualMaxZoom.get_Value().add_Activated((EventHandler<EventArgs>)ManualMaxZoomOut);
		}

		private void ManualMaxZoomOut(object sender, EventArgs e)
		{
			float distance = ComputeCameraDistance();
			for (int i = 0; (float)i < (25f - distance) * 2f; i++)
			{
				_zoomOutKey.Press();
			}
		}

		private void ZoomCameraOut(float distance)
		{
			for (int i = 0; (float)i < (25f - distance) * 2f; i++)
			{
				_zoomOutKey.Press();
			}
		}

		protected override void Enable()
		{
			base.Enable();
		}

		protected override void Disable()
		{
			base.Disable();
		}

		public override void Load()
		{
			base.Load();
			GameService.Input.get_Mouse().add_MouseWheelScrolled((EventHandler<MouseEventArgs>)Mouse_MouseWheelScrolled);
			Gw2MumbleService gw2Mumble = GameService.Gw2Mumble;
			gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
			gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacter_NameChanged);
		}

		private void PlayerCharacter_NameChanged(object sender, ValueEventArgs<string> e)
		{
			_ticks = 0.0;
		}

		private void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
		{
			_ticks = 0.0;
		}

		private void Mouse_MouseWheelScrolled(object sender, MouseEventArgs e)
		{
			if (_allowManualZoom.get_Value())
			{
				float distance = (_distance = ComputeCameraDistance());
				_saveDistanceTicks = Common.Now() + 1000.0;
			}
		}

		private float ComputeCameraDistance()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			Gw2MumbleService gw2Mumble = GameService.Gw2Mumble;
			PlayerCamera camera = GameService.Gw2Mumble.get_PlayerCamera();
			Vector3 ppos = gw2Mumble.get_PlayerCharacter().get_Position();
			return camera.get_Position().Distance3D(ppos);
		}

		public override void Unload()
		{
			base.Unload();
			GameService.Input.get_Mouse().remove_MouseWheelScrolled((EventHandler<MouseEventArgs>)Mouse_MouseWheelScrolled);
			Gw2MumbleService gw2Mumble = GameService.Gw2Mumble;
			gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
			gw2Mumble.get_PlayerCharacter().remove_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacter_NameChanged);
		}

		protected override void SwitchLanguage()
		{
			base.SwitchLanguage();
		}

		public override void CreateSettingsPanel(FlowPanel flowPanel, int width)
		{
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = new Panel();
			((Control)panel).set_Parent((Container)(object)flowPanel);
			((Control)panel).set_Width(width);
			((Container)panel).set_HeightSizingMode((SizingMode)1);
			((Panel)panel).set_ShowBorder(true);
			((Panel)panel).set_CanCollapse(true);
			panel.TitleIcon = base.Icon.Texture;
			((Panel)panel).set_Title(SubModuleType.ToString());
			Panel headerPanel = panel;
			FlowPanel flowPanel2 = new FlowPanel();
			((Control)flowPanel2).set_Parent((Container)(object)headerPanel);
			((Container)flowPanel2).set_HeightSizingMode((SizingMode)1);
			((Container)flowPanel2).set_WidthSizingMode((SizingMode)2);
			((FlowPanel)flowPanel2).set_FlowDirection((ControlFlowDirection)3);
			flowPanel2.ContentPadding = new RectangleDimensions(5, 2);
			((FlowPanel)flowPanel2).set_ControlPadding(new Vector2(0f, 2f));
			FlowPanel contentFlowPanel = flowPanel2;
			Func<string> localizedLabelContent = () => string.Format(strings.ShowInHotbar_Name, $"{SubModuleType}");
			Func<string> localizedTooltip = () => string.Format(strings.ShowInHotbar_Description, $"{SubModuleType}");
			int width2 = width - 16;
			Checkbox checkbox = new Checkbox();
			((Control)checkbox).set_Height(20);
			((Checkbox)checkbox).set_Checked(base.ShowInHotbar.get_Value());
			checkbox.CheckedChangedAction = delegate(bool b)
			{
				base.ShowInHotbar.set_Value(b);
			};
			UI.WrapWithLabel(localizedLabelContent, localizedTooltip, (Container)(object)contentFlowPanel, width2, (Control)(object)checkbox);
			KeybindingAssigner keybindingAssigner = new KeybindingAssigner();
			((Control)keybindingAssigner).set_Parent((Container)(object)contentFlowPanel);
			((Control)keybindingAssigner).set_Width(width - 16);
			((KeybindingAssigner)keybindingAssigner).set_KeyBinding(base.HotKey.get_Value());
			keybindingAssigner.KeybindChangedAction = delegate(KeyBinding kb)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Expected O, but got Unknown
				SettingEntry<KeyBinding> hotKey = base.HotKey;
				KeyBinding val3 = new KeyBinding();
				val3.set_ModifierKeys(kb.get_ModifierKeys());
				val3.set_PrimaryKey(kb.get_PrimaryKey());
				val3.set_Enabled(kb.get_Enabled());
				val3.set_IgnoreWhenInTextField(true);
				hotKey.set_Value(val3);
			};
			keybindingAssigner.SetLocalizedKeyBindingName = () => string.Format(strings.HotkeyEntry_Name, $"{SubModuleType}");
			keybindingAssigner.SetLocalizedTooltip = () => string.Format(strings.HotkeyEntry_Description, $"{SubModuleType}");
			KeybindingAssigner keybindingAssigner2 = new KeybindingAssigner();
			((Control)keybindingAssigner2).set_Parent((Container)(object)contentFlowPanel);
			((Control)keybindingAssigner2).set_Width(width - 16);
			((KeybindingAssigner)keybindingAssigner2).set_KeyBinding(_zoomOutKey.get_Value());
			keybindingAssigner2.KeybindChangedAction = delegate(KeyBinding kb)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Expected O, but got Unknown
				SettingEntry<KeyBinding> zoomOutKey = _zoomOutKey;
				KeyBinding val2 = new KeyBinding();
				val2.set_ModifierKeys(kb.get_ModifierKeys());
				val2.set_PrimaryKey(kb.get_PrimaryKey());
				val2.set_Enabled(kb.get_Enabled());
				val2.set_IgnoreWhenInTextField(true);
				zoomOutKey.set_Value(val2);
			};
			keybindingAssigner2.SetLocalizedKeyBindingName = () => strings.ZoomOutKey_Name;
			keybindingAssigner2.SetLocalizedTooltip = () => strings.ZoomOutKey_Tooltip;
			KeybindingAssigner keybindingAssigner3 = new KeybindingAssigner();
			((Control)keybindingAssigner3).set_Parent((Container)(object)contentFlowPanel);
			((Control)keybindingAssigner3).set_Width(width - 16);
			((KeybindingAssigner)keybindingAssigner3).set_KeyBinding(_manualMaxZoom.get_Value());
			keybindingAssigner3.KeybindChangedAction = delegate(KeyBinding kb)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Expected O, but got Unknown
				SettingEntry<KeyBinding> manualMaxZoom = _manualMaxZoom;
				KeyBinding val = new KeyBinding();
				val.set_ModifierKeys(kb.get_ModifierKeys());
				val.set_PrimaryKey(kb.get_PrimaryKey());
				val.set_Enabled(kb.get_Enabled());
				val.set_IgnoreWhenInTextField(true);
				manualMaxZoom.set_Value(val);
			};
			keybindingAssigner3.SetLocalizedKeyBindingName = () => strings.ManualZoom_Name;
			keybindingAssigner3.SetLocalizedTooltip = () => strings.ManualZoom_Tooltip;
			Func<string> localizedLabelContent2 = () => strings.ZoomOnCameraChange_Name;
			Func<string> localizedTooltip2 = () => strings.ZoomOnCameraChange_Tooltip;
			int width3 = width - 16;
			Checkbox checkbox2 = new Checkbox();
			((Control)checkbox2).set_Height(20);
			((Checkbox)checkbox2).set_Checked(_zoomOnCameraChange.get_Value());
			checkbox2.CheckedChangedAction = delegate(bool b)
			{
				_zoomOnCameraChange.set_Value(b);
			};
			UI.WrapWithLabel(localizedLabelContent2, localizedTooltip2, (Container)(object)contentFlowPanel, width3, (Control)(object)checkbox2);
			Func<string> localizedLabelContent3 = () => strings.ZoomOnFoVChange_Name;
			Func<string> localizedTooltip3 = () => strings.ZoomOnFoVChange_Tooltip;
			int width4 = width - 16;
			Checkbox checkbox3 = new Checkbox();
			((Control)checkbox3).set_Height(20);
			((Checkbox)checkbox3).set_Checked(_zoomOnFoVChange.get_Value());
			checkbox3.CheckedChangedAction = delegate(bool b)
			{
				_zoomOnFoVChange.set_Value(b);
			};
			UI.WrapWithLabel(localizedLabelContent3, localizedTooltip3, (Container)(object)contentFlowPanel, width4, (Control)(object)checkbox3);
			Func<string> localizedLabelContent4 = () => strings.AllowManualZoom_Name;
			Func<string> localizedTooltip4 = () => strings.AllowManualZoom_Tooltip;
			int width5 = width - 16;
			Checkbox checkbox4 = new Checkbox();
			((Control)checkbox4).set_Height(20);
			((Checkbox)checkbox4).set_Checked(_allowManualZoom.get_Value());
			checkbox4.CheckedChangedAction = delegate(bool b)
			{
				_allowManualZoom.set_Value(b);
			};
			UI.WrapWithLabel(localizedLabelContent4, localizedTooltip4, (Container)(object)contentFlowPanel, width5, (Control)(object)checkbox4);
		}
	}
}
