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
			if (!Mumble.UI.IsMapOpen && Mumble.Info.IsGameFocused && !(gameTime.get_TotalGameTime().TotalMilliseconds - _ticks < 25.0))
			{
				if (gameTime.get_TotalGameTime().TotalMilliseconds - _saveDistanceTicks < 0.0)
				{
					_distance = ComputeCameraDistance();
				}
				_ticks = gameTime.get_TotalGameTime().TotalMilliseconds;
				double threshold = 1.0;
				float distance = ComputeCameraDistance();
				float delta = Math.Max(_distance, distance) - Math.Min(_distance, distance);
				if (_zoomOnCameraChange.Value && (double)delta >= threshold)
				{
					ZoomCameraOut(distance);
					_distance = distance;
				}
				if (_zoomOnFoVChange.Value && Mumble.PlayerCamera.FieldOfView != _fieldOfView)
				{
					ZoomCameraOut(distance);
					_distance = distance;
					_fieldOfView = Mumble.PlayerCamera.FieldOfView;
				}
			}
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			base.DefineSettings(settings);
			_zoomOutKey = Settings.DefineSetting("_zoomOutKey", new KeyBinding((Keys)34));
			_manualMaxZoom = Settings.DefineSetting("_manualMaxZoom", new KeyBinding((Keys)0));
			_zoomOnCameraChange = settings.DefineSetting("_zoomOnCameraChange", defaultValue: true);
			_allowManualZoom = settings.DefineSetting("_allowManualZoom", defaultValue: true);
			_zoomOnFoVChange = settings.DefineSetting("_zoomOnFoVChange", defaultValue: true);
			_manualMaxZoom.Value.Enabled = true;
			_manualMaxZoom.Value.Activated += ManualMaxZoomOut;
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
			GameService.Input.Mouse.MouseWheelScrolled += Mouse_MouseWheelScrolled;
			Gw2MumbleService gw2Mumble = GameService.Gw2Mumble;
			gw2Mumble.CurrentMap.MapChanged += CurrentMap_MapChanged;
			gw2Mumble.PlayerCharacter.NameChanged += PlayerCharacter_NameChanged;
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
			if (_allowManualZoom.Value)
			{
				float distance = (_distance = ComputeCameraDistance());
				_saveDistanceTicks = Common.Now + 1000.0;
			}
		}

		private float ComputeCameraDistance()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			Gw2MumbleService gw2Mumble = GameService.Gw2Mumble;
			PlayerCamera camera = GameService.Gw2Mumble.PlayerCamera;
			Vector3 ppos = gw2Mumble.PlayerCharacter.Position;
			return camera.Position.Distance3D(ppos);
		}

		public override void Unload()
		{
			base.Unload();
			GameService.Input.Mouse.MouseWheelScrolled -= Mouse_MouseWheelScrolled;
			Gw2MumbleService gw2Mumble = GameService.Gw2Mumble;
			gw2Mumble.CurrentMap.MapChanged -= CurrentMap_MapChanged;
			gw2Mumble.PlayerCharacter.NameChanged -= PlayerCharacter_NameChanged;
		}

		protected override void SwitchLanguage()
		{
			base.SwitchLanguage();
		}

		public override void CreateSettingsPanel(Kenedia.Modules.Core.Controls.FlowPanel flowPanel, int width)
		{
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			Kenedia.Modules.Core.Controls.Panel headerPanel = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = flowPanel,
				Width = width,
				HeightSizingMode = SizingMode.AutoSize,
				ShowBorder = true,
				CanCollapse = true,
				TitleIcon = base.Icon.Texture,
				Title = SubModuleType.ToString()
			};
			Kenedia.Modules.Core.Controls.FlowPanel contentFlowPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = headerPanel,
				HeightSizingMode = SizingMode.AutoSize,
				WidthSizingMode = SizingMode.Fill,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ContentPadding = new RectangleDimensions(5, 2),
				ControlPadding = new Vector2(0f, 2f)
			};
			Kenedia.Modules.Core.Utility.UI.WrapWithLabel(() => string.Format(strings.ShowInHotbar_Name, $"{SubModuleType}"), () => string.Format(strings.ShowInHotbar_Description, $"{SubModuleType}"), contentFlowPanel, width - 16, new Kenedia.Modules.Core.Controls.Checkbox
			{
				Height = 20,
				Checked = base.ShowInHotbar.Value,
				CheckedChangedAction = delegate(bool b)
				{
					base.ShowInHotbar.Value = b;
				}
			});
			new Kenedia.Modules.Core.Controls.KeybindingAssigner
			{
				Parent = contentFlowPanel,
				Width = width - 16,
				KeyBinding = base.HotKey.Value,
				KeybindChangedAction = delegate(KeyBinding kb)
				{
					//IL_0019: Unknown result type (might be due to invalid IL or missing references)
					base.HotKey.Value = new KeyBinding
					{
						ModifierKeys = kb.ModifierKeys,
						PrimaryKey = kb.PrimaryKey,
						Enabled = kb.Enabled,
						IgnoreWhenInTextField = true
					};
				},
				SetLocalizedKeyBindingName = () => string.Format(strings.HotkeyEntry_Name, $"{SubModuleType}"),
				SetLocalizedTooltip = () => string.Format(strings.HotkeyEntry_Description, $"{SubModuleType}")
			};
			new Kenedia.Modules.Core.Controls.KeybindingAssigner
			{
				Parent = contentFlowPanel,
				Width = width - 16,
				KeyBinding = _zoomOutKey.Value,
				KeybindChangedAction = delegate(KeyBinding kb)
				{
					//IL_0019: Unknown result type (might be due to invalid IL or missing references)
					_zoomOutKey.Value = new KeyBinding
					{
						ModifierKeys = kb.ModifierKeys,
						PrimaryKey = kb.PrimaryKey,
						Enabled = kb.Enabled,
						IgnoreWhenInTextField = true
					};
				},
				SetLocalizedKeyBindingName = () => strings.ZoomOutKey_Name,
				SetLocalizedTooltip = () => strings.ZoomOutKey_Tooltip
			};
			new Kenedia.Modules.Core.Controls.KeybindingAssigner
			{
				Parent = contentFlowPanel,
				Width = width - 16,
				KeyBinding = _manualMaxZoom.Value,
				KeybindChangedAction = delegate(KeyBinding kb)
				{
					//IL_0019: Unknown result type (might be due to invalid IL or missing references)
					_manualMaxZoom.Value = new KeyBinding
					{
						ModifierKeys = kb.ModifierKeys,
						PrimaryKey = kb.PrimaryKey,
						Enabled = kb.Enabled,
						IgnoreWhenInTextField = true
					};
				},
				SetLocalizedKeyBindingName = () => strings.ManualZoom_Name,
				SetLocalizedTooltip = () => strings.ManualZoom_Tooltip
			};
			Kenedia.Modules.Core.Utility.UI.WrapWithLabel(() => strings.ZoomOnCameraChange_Name, () => strings.ZoomOnCameraChange_Tooltip, contentFlowPanel, width - 16, new Kenedia.Modules.Core.Controls.Checkbox
			{
				Height = 20,
				Checked = _zoomOnCameraChange.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_zoomOnCameraChange.Value = b;
				}
			});
			Kenedia.Modules.Core.Utility.UI.WrapWithLabel(() => strings.ZoomOnFoVChange_Name, () => strings.ZoomOnFoVChange_Tooltip, contentFlowPanel, width - 16, new Kenedia.Modules.Core.Controls.Checkbox
			{
				Height = 20,
				Checked = _zoomOnFoVChange.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_zoomOnFoVChange.Value = b;
				}
			});
			Kenedia.Modules.Core.Utility.UI.WrapWithLabel(() => strings.AllowManualZoom_Name, () => strings.AllowManualZoom_Tooltip, contentFlowPanel, width - 16, new Kenedia.Modules.Core.Controls.Checkbox
			{
				Height = 20,
				Checked = _allowManualZoom.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_allowManualZoom.Value = b;
				}
			});
		}
	}
}
