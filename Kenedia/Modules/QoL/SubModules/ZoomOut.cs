using System;
using Blish_HUD;
using Blish_HUD.Controls.Extern;
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
		private int MumbleTick;

		private Point Resolution;

		private bool InGame;

		private float Zoom;

		private int ZoomTicks;

		public SettingEntry<KeyBinding> ManualMaxZoomOut;

		public ZoomOut()
		{
			Name = "Zoom Out";
			ModuleIcon = QoL.ModuleInstance.TextureManager.getIcon("ZoomOut", _Icons.ModuleIcon);
			ModuleIconHovered = QoL.ModuleInstance.TextureManager.getIcon("ZoomOut", _Icons.ModuleIcon_HoveredWhite);
			ModuleIcon_Active = QoL.ModuleInstance.TextureManager.getIcon("ZoomOut", _Icons.ModuleIcon_Active);
			ModuleIconHovered_Active = QoL.ModuleInstance.TextureManager.getIcon("ZoomOut", _Icons.ModuleIcon_Active_HoveredWhite);
			Initialize();
			LoadData();
		}

		public override void DefineSettings(SettingCollection settings)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			ToggleModule_Key = settings.DefineSetting<KeyBinding>(Name + "ToggleModule_Key", new KeyBinding((ModifierKeys)1, (Keys)96), (Func<string>)(() => string.Format(common.Toggle, Name)), (Func<string>)null);
			ManualMaxZoomOut = settings.DefineSetting<KeyBinding>("ManualMaxZoomOut", new KeyBinding((Keys)0), (Func<string>)(() => common.ManualMaxZoomOut_Name), (Func<string>)(() => common.ManualMaxZoomOut_Tooltip));
			ManualMaxZoomOut.get_Value().set_Enabled(true);
			ManualMaxZoomOut.get_Value().add_Activated((EventHandler<EventArgs>)ManualMaxZoomOut_Triggered);
			ToggleModule_Key.get_Value().set_Enabled(true);
			ToggleModule_Key.get_Value().add_Activated((EventHandler<EventArgs>)ToggleModule_Key_Activated);
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
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			Gw2MumbleService Mumble = GameService.Gw2Mumble;
			if (Zoom < Mumble.get_PlayerCamera().get_FieldOfView())
			{
				ZoomTicks += 2;
			}
			else if (ZoomTicks > 0)
			{
				Mouse.RotateWheel(-25, false, -1, -1, false);
				ZoomTicks--;
			}
			MouseState mouse = Mouse.GetState();
			if ((((int)((MouseState)(ref mouse)).get_LeftButton() != 0) ? 1 : 0) == 1 || GameService.Graphics.get_Resolution() != Resolution)
			{
				Resolution = GameService.Graphics.get_Resolution();
				MumbleTick = Mumble.get_Tick() + 5;
				return;
			}
			if (!GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && InGame && Mumble.get_Tick() > MumbleTick)
			{
				Keyboard.Stroke((VirtualKeyShort)27, false);
				Mouse.Click((MouseButton)0, 5, 5, false);
				MumbleTick = Mumble.get_Tick() + 1;
			}
			InGame = GameService.GameIntegration.get_Gw2Instance().get_IsInGame();
			Zoom = Mumble.get_PlayerCamera().get_FieldOfView();
		}

		public override void UpdateLanguage(object sender, EventArgs e)
		{
			base.UpdateLanguage(sender, e);
		}

		public override void Dispose()
		{
			ToggleModule_Key.get_Value().remove_Activated((EventHandler<EventArgs>)ToggleModule_Key_Activated);
			ManualMaxZoomOut.get_Value().remove_Activated((EventHandler<EventArgs>)ManualMaxZoomOut_Triggered);
			base.Dispose();
		}
	}
}
