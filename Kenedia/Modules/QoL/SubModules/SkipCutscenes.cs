using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
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
	public class SkipCutscenes : SubModule
	{
		private int MumbleTick;

		private Point Resolution;

		private Vector3 PPos = Vector3.get_Zero();

		private bool InGame;

		private bool ModuleActive;

		private bool ClickAgain;

		private bool SleptBeforeClick;

		private bool IntroCutscene;

		public SettingEntry<KeyBinding> Cancel_Key;

		private List<int> IntroMaps = new List<int> { 573, 458, 138, 379, 432 };

		private List<int> StarterMaps = new List<int> { 15, 19, 28, 34, 35 };

		private void Click()
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			Point mousePos = Mouse.GetPosition();
			mousePos = new Point(mousePos.X, mousePos.Y);
			WindowUtil.GetWindowRect(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle(), out var pos);
			Point p = new Point(GameService.Graphics.get_Resolution().X + pos.Left - 35, GameService.Graphics.get_Resolution().Y + pos.Top);
			Mouse.SetPosition(p.X, p.Y, true);
			Thread.Sleep(25);
			Mouse.Click((MouseButton)0, p.X, p.Y, true);
			Thread.Sleep(10);
			Mouse.SetPosition(mousePos.X, mousePos.Y, true);
		}

		public SkipCutscenes()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Name = "Skip Cutscenes";
			ModuleIcon = QoL.ModuleInstance.TextureManager.getIcon("SkipCutscenes", _Icons.ModuleIcon);
			ModuleIconHovered = QoL.ModuleInstance.TextureManager.getIcon("SkipCutscenes", _Icons.ModuleIcon_HoveredWhite);
			ModuleIcon_Active = QoL.ModuleInstance.TextureManager.getIcon("SkipCutscenes", _Icons.ModuleIcon_Active);
			ModuleIconHovered_Active = QoL.ModuleInstance.TextureManager.getIcon("SkipCutscenes", _Icons.ModuleIcon_Active_HoveredWhite);
			Initialize();
			LoadData();
		}

		public override void DefineSettings(SettingCollection settings)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			ToggleModule_Key = settings.DefineSetting<KeyBinding>(Name + "ToggleModule_Key", new KeyBinding((Keys)0), (Func<string>)(() => string.Format(common.Toggle, Name)), (Func<string>)null);
			SettingCollection internal_settings = settings.AddSubCollection(Name + " Internal Settings", false, false);
			Cancel_Key = internal_settings.DefineSetting<KeyBinding>(Name + "Cancel_Key", new KeyBinding((Keys)27), (Func<string>)null, (Func<string>)null);
			Cancel_Key.get_Value().set_Enabled(true);
			Cancel_Key.get_Value().add_Activated((EventHandler<EventArgs>)Cancel_Key_Activated);
			ToggleModule_Key.get_Value().set_Enabled(true);
			ToggleModule_Key.get_Value().add_Activated((EventHandler<EventArgs>)ToggleModule_Key_Activated);
		}

		public override void ToggleModule()
		{
			base.ToggleModule();
		}

		public override void Initialize()
		{
			base.Initialize();
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacter_NameChanged);
		}

		private void PlayerCharacter_NameChanged(object sender, ValueEventArgs<string> e)
		{
			IntroCutscene = false;
		}

		private void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			ClickAgain = false;
			SleptBeforeClick = false;
			Ticks.global += 2000.0;
			MumbleTick = GameService.Gw2Mumble.get_Tick();
			Vector3 p = (PPos = GameService.Gw2Mumble.get_PlayerCharacter().get_Position());
			if (IntroCutscene && StarterMaps.Contains(GameService.Gw2Mumble.get_CurrentMap().get_Id()))
			{
				Thread.Sleep(1250);
				Click();
			}
		}

		private void ToggleModule_Key_Activated(object sender, EventArgs e)
		{
			ToggleModule();
		}

		private void Cancel_Key_Activated(object sender, EventArgs e)
		{
			Ticks.global += 2500.0;
			ClickAgain = false;
			SleptBeforeClick = false;
			MumbleTick = GameService.Gw2Mumble.get_Tick() + 5;
		}

		public override void LoadData()
		{
			Loaded = true;
		}

		public override void Update(GameTime gameTime)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			Gw2MumbleService Mumble = GameService.Gw2Mumble;
			Point resolution = GameService.Graphics.get_Resolution();
			bool _inGame = GameService.GameIntegration.get_Gw2Instance().get_IsInGame();
			if (IntroMaps.Contains(Mumble.get_CurrentMap().get_Id()))
			{
				IntroCutscene = true;
			}
			if (GameService.Graphics.get_Resolution() != resolution)
			{
				Resolution = resolution;
				MumbleTick = Mumble.get_Tick() + 5;
				return;
			}
			if (!_inGame && (InGame || ClickAgain) && GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus())
			{
				if (Mumble.get_Tick() > MumbleTick)
				{
					Click();
					ClickAgain = true;
					MumbleTick = Mumble.get_Tick();
					Ticks.global = gameTime.get_TotalGameTime().TotalMilliseconds + 250.0;
				}
				else if (ClickAgain)
				{
					if (!SleptBeforeClick)
					{
						Ticks.global = gameTime.get_TotalGameTime().TotalMilliseconds + 3500.0;
						SleptBeforeClick = true;
						return;
					}
					ClickAgain = false;
					Click();
					Mouse.Click((MouseButton)0, 15, 15, false);
					Thread.Sleep(5);
					Keyboard.Stroke((VirtualKeyShort)27, false);
				}
			}
			else
			{
				ClickAgain = false;
				SleptBeforeClick = false;
			}
			InGame = GameService.GameIntegration.get_Gw2Instance().get_IsInGame();
		}

		public override void UpdateLanguage(object sender, EventArgs e)
		{
			base.UpdateLanguage(sender, e);
		}

		public override void Dispose()
		{
			Cancel_Key.get_Value().remove_Activated((EventHandler<EventArgs>)Cancel_Key_Activated);
			ToggleModule_Key.get_Value().remove_Activated((EventHandler<EventArgs>)ToggleModule_Key_Activated);
			base.Dispose();
		}
	}
}
