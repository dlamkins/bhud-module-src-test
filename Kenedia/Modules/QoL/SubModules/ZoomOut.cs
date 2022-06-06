using System;
using Blish_HUD;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Kenedia.Modules.QoL.Classes;
using Kenedia.Modules.QoL.Strings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.QoL.SubModules
{
	public class ZoomOut : SubModule
	{
		private bool MouseScrolled;

		private float Distance;

		private float Zoom;

		private int ZoomTicks;

		public SettingEntry<KeyBinding> ManualMaxZoomOut;

		public SettingEntry<bool> ZoomOnCameraChange;

		public SettingEntry<bool> AllowManualZoom;

		public ZoomOut()
		{
			Name = "Zoom Out";
			ModuleIcon = QoL.ModuleInstance.TextureManager.getIcon("ZoomOut", _Icons.ModuleIcon);
			ModuleIconHovered = QoL.ModuleInstance.TextureManager.getIcon("ZoomOut", _Icons.ModuleIcon_HoveredWhite);
			ModuleIcon_Active = QoL.ModuleInstance.TextureManager.getIcon("ZoomOut", _Icons.ModuleIcon_Active);
			ModuleIconHovered_Active = QoL.ModuleInstance.TextureManager.getIcon("ZoomOut", _Icons.ModuleIcon_Active_HoveredWhite);
		}

		public override void DefineSettings(SettingCollection settings)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Expected O, but got Unknown
			base.DefineSettings(settings);
			ToggleModule_Key = settings.DefineSetting<KeyBinding>(Name + "ToggleModule_Key", new KeyBinding((ModifierKeys)1, (Keys)96), (Func<string>)(() => string.Format(common.Toggle, Name)), (Func<string>)null);
			ManualMaxZoomOut = settings.DefineSetting<KeyBinding>("ManualMaxZoomOut", new KeyBinding((Keys)0), (Func<string>)(() => common.ManualMaxZoomOut_Name), (Func<string>)(() => common.ManualMaxZoomOut_Tooltip));
			ZoomOnCameraChange = settings.DefineSetting<bool>("ZoomOnCameraChange", true, (Func<string>)(() => common.ZoomOnCameraChange_Name), (Func<string>)(() => common.ZoomOnCameraChange_Tooltip));
			AllowManualZoom = settings.DefineSetting<bool>("AllowManualZoom", true, (Func<string>)(() => common.AllowManualZoom_Name), (Func<string>)(() => common.AllowManualZoom_Tooltip));
			ManualMaxZoomOut.get_Value().set_Enabled(true);
			ManualMaxZoomOut.get_Value().add_Activated((EventHandler<EventArgs>)ManualMaxZoomOut_Triggered);
			ToggleModule_Key.get_Value().set_Enabled(true);
			ToggleModule_Key.get_Value().add_Activated((EventHandler<EventArgs>)ToggleModule_Key_Activated);
			Enabled = settings.DefineSetting<bool>(Name + "Enabled", true, (Func<string>)(() => string.Format(common.Enable_Name, Name)), (Func<string>)(() => string.Format(common.Enable_Tooltip, Name)));
			ShowOnBar = settings.DefineSetting<bool>(Name + "ShowOnBar", true, (Func<string>)(() => string.Format(common.ShowIcon_Name, Name)), (Func<string>)(() => string.Format(common.ShowIcon_Tooltip, Name)));
		}

		private void ToggleModule_Key_Activated(object sender, EventArgs e)
		{
			ToggleModule();
		}

		public override void ToggleModule()
		{
			base.ToggleModule();
		}

		public override void Initialize()
		{
			base.Initialize();
			GameService.Input.get_Mouse().add_MouseWheelScrolled((EventHandler<MouseEventArgs>)Mouse_MouseWheelScrolled);
			Gw2MumbleService gw2Mumble = GameService.Gw2Mumble;
			gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
			gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacter_NameChanged);
			LoadData();
		}

		private void PlayerCharacter_NameChanged(object sender, ValueEventArgs<string> e)
		{
			ZoomTicks = 0;
		}

		private void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
		{
			ZoomTicks = 0;
		}

		private void Mouse_MouseWheelScrolled(object sender, MouseEventArgs e)
		{
			MouseScrolled = true;
		}

		private void ManualMaxZoomOut_Triggered(object sender, EventArgs e)
		{
			ZoomTicks = 40;
		}

		public override void LoadData()
		{
			Loaded = true;
		}

		public override void Update(GameTime gameTime)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			Gw2MumbleService Mumble = GameService.Gw2Mumble;
			float cameraDistance = Math.Max(Mumble.get_PlayerCamera().get_Position().Z, Mumble.get_PlayerCharacter().get_Position().Z) - Math.Min(Mumble.get_PlayerCamera().get_Position().Z, Mumble.get_PlayerCharacter().get_Position().Z);
			float delta = Math.Max(Distance, cameraDistance) - Math.Min(Distance, cameraDistance);
			double threshold = (AllowManualZoom.get_Value() ? 0.5 : 0.25);
			if (Mumble.get_UI().get_IsMapOpen())
			{
				ZoomTicks = 0;
				return;
			}
			if (cameraDistance == Distance)
			{
				ZoomTicks /= 2;
			}
			if ((double)delta > threshold)
			{
				if (ZoomOnCameraChange.get_Value() && (!AllowManualZoom.get_Value() || !MouseScrolled) && Distance != 0f)
				{
					ZoomTicks = 2;
				}
				MouseScrolled = false;
				Distance = cameraDistance;
			}
			if (Zoom < Mumble.get_PlayerCamera().get_FieldOfView())
			{
				ZoomTicks += 2;
			}
			else if (ZoomTicks > 0)
			{
				Mouse.RotateWheel(-25, false, -1, -1, false);
				ZoomTicks--;
			}
			Zoom = Mumble.get_PlayerCamera().get_FieldOfView();
		}

		public override void UpdateLanguage(object sender, EventArgs e)
		{
			base.UpdateLanguage(sender, e);
		}

		public override void Dispose()
		{
			Gw2MumbleService gw2Mumble = GameService.Gw2Mumble;
			ToggleModule_Key.get_Value().remove_Activated((EventHandler<EventArgs>)ToggleModule_Key_Activated);
			ManualMaxZoomOut.get_Value().remove_Activated((EventHandler<EventArgs>)ManualMaxZoomOut_Triggered);
			GameService.Input.get_Mouse().remove_MouseWheelScrolled((EventHandler<MouseEventArgs>)Mouse_MouseWheelScrolled);
			gw2Mumble.get_PlayerCharacter().remove_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacter_NameChanged);
			gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
			base.Dispose();
		}
	}
}
