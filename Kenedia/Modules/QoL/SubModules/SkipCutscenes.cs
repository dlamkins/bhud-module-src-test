using System;
using System.Collections.Generic;
using System.Drawing;
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
		private enum State
		{
			Ready,
			MouseMoved,
			Clicked,
			MouseMovedBack,
			Clicked_Again,
			Menu_Opened,
			Menu_Closed,
			Done
		}

		private enum CinematicState
		{
			Ready,
			InitialSleep,
			Clicked_Once,
			Sleeping,
			Clicked_Twice,
			Done
		}

		private State ModuleState;

		private CinematicState CutsceneState;

		private Point MouseStartPosition;

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

		public SkipCutscenes()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Name = "Skip Cutscenes";
			ModuleIcon = QoL.ModuleInstance.TextureManager.getIcon("SkipCutscenes", _Icons.ModuleIcon);
			ModuleIconHovered = QoL.ModuleInstance.TextureManager.getIcon("SkipCutscenes", _Icons.ModuleIcon_HoveredWhite);
			ModuleIcon_Active = QoL.ModuleInstance.TextureManager.getIcon("SkipCutscenes", _Icons.ModuleIcon_Active);
			ModuleIconHovered_Active = QoL.ModuleInstance.TextureManager.getIcon("SkipCutscenes", _Icons.ModuleIcon_Active_HoveredWhite);
		}

		public override void DefineSettings(SettingCollection settings)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Expected O, but got Unknown
			base.DefineSettings(settings);
			ToggleModule_Key = settings.DefineSetting<KeyBinding>(Name + "ToggleModule_Key", new KeyBinding((Keys)0), (Func<string>)(() => string.Format(common.Toggle, Name)), (Func<string>)null);
			SettingCollection internal_settings = settings.AddSubCollection(Name + " Internal Settings", false, false);
			Cancel_Key = internal_settings.DefineSetting<KeyBinding>(Name + "Cancel_Key", new KeyBinding((Keys)27), (Func<string>)null, (Func<string>)null);
			Cancel_Key.get_Value().set_Enabled(true);
			Cancel_Key.get_Value().add_Activated((EventHandler<EventArgs>)Cancel_Key_Activated);
			ToggleModule_Key.get_Value().set_Enabled(true);
			ToggleModule_Key.get_Value().add_Activated((EventHandler<EventArgs>)ToggleModule_Key_Activated);
			Enabled = settings.DefineSetting<bool>(Name + "Enabled", true, (Func<string>)(() => $"Enable {Name}"), (Func<string>)null);
			ShowOnBar = settings.DefineSetting<bool>(Name + "ShowOnBar", true, (Func<string>)(() => string.Format("Show Icon", Name)), (Func<string>)null);
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
			LoadData();
		}

		private void PlayerCharacter_NameChanged(object sender, ValueEventArgs<string> e)
		{
			IntroCutscene = false;
		}

		private void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			if (base.Active)
			{
				MumbleTick = GameService.Gw2Mumble.get_Tick();
				Vector3 p = (PPos = GameService.Gw2Mumble.get_PlayerCharacter().get_Position());
				if (IntroCutscene && StarterMaps.Contains(GameService.Gw2Mumble.get_CurrentMap().get_Id()))
				{
					Ticks.global += 1250.0;
					CutsceneState = CinematicState.InitialSleep;
					ModuleState = State.Ready;
				}
			}
		}

		private void ToggleModule_Key_Activated(object sender, EventArgs e)
		{
			ToggleModule();
		}

		private void Cancel_Key_Activated(object sender, EventArgs e)
		{
			if (base.Active)
			{
				Ticks.global += 2500.0;
				ClickAgain = false;
				SleptBeforeClick = false;
				MumbleTick = GameService.Gw2Mumble.get_Tick() + 5;
			}
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
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0343: Unknown result type (might be due to invalid IL or missing references)
			//IL_0348: Unknown result type (might be due to invalid IL or missing references)
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
			}
			else
			{
				if (!(gameTime.get_TotalGameTime().TotalMilliseconds - Ticks.global > 0.0))
				{
					return;
				}
				Ticks.global = gameTime.get_TotalGameTime().TotalMilliseconds;
				WindowUtil.GetWindowRect(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle(), out var pos);
				Point p = new Point(GameService.Graphics.get_Resolution().X + pos.Left - 35, GameService.Graphics.get_Resolution().Y + pos.Top);
				Point mousePos = Mouse.GetPosition();
				mousePos = new Point(mousePos.X, mousePos.Y);
				if (!_inGame && (Mumble.get_Tick() > MumbleTick || CutsceneState != CinematicState.Done))
				{
					if (CutsceneState == CinematicState.Clicked_Once)
					{
						Ticks.global = gameTime.get_TotalGameTime().TotalMilliseconds + 2500.0;
						CutsceneState = CinematicState.Sleeping;
						ModuleState = State.Ready;
					}
					else if (CutsceneState == CinematicState.Ready)
					{
						Ticks.global = gameTime.get_TotalGameTime().TotalMilliseconds + 250.0;
						CutsceneState = CinematicState.InitialSleep;
						ModuleState = State.Ready;
					}
					else if (CutsceneState == CinematicState.Sleeping || CutsceneState == CinematicState.InitialSleep)
					{
						if (ModuleState == State.Ready)
						{
							Mouse.SetPosition(p.X, p.Y, true);
							MouseStartPosition = new Point(mousePos.X, mousePos.Y);
							ModuleState = State.MouseMoved;
						}
						else if (ModuleState == State.MouseMoved)
						{
							Mouse.Click((MouseButton)0, p.X, p.Y, true);
							ModuleState = State.Clicked;
						}
						else if (ModuleState == State.Clicked)
						{
							Mouse.SetPosition(MouseStartPosition.X, MouseStartPosition.Y, true);
							ModuleState = State.MouseMovedBack;
							CutsceneState = ((CutsceneState == CinematicState.Ready) ? CinematicState.Clicked_Once : CinematicState.Clicked_Twice);
						}
					}
					else if (CutsceneState == CinematicState.Clicked_Twice)
					{
						if (ModuleState == State.MouseMovedBack)
						{
							Mouse.Click((MouseButton)0, 15, 15, false);
							ModuleState = State.Menu_Opened;
						}
						else if (ModuleState == State.Menu_Opened)
						{
							Keyboard.Stroke((VirtualKeyShort)27, false);
							ModuleState = State.Menu_Closed;
							CutsceneState = CinematicState.Done;
						}
					}
				}
				else
				{
					if (ModuleState == State.Ready && CutsceneState == CinematicState.Ready)
					{
						return;
					}
					if (ModuleState == State.Clicked && MouseStartPosition != Point.get_Zero())
					{
						if (ModuleState == State.Clicked)
						{
							Mouse.Click((MouseButton)0, 15, 15, false);
							ModuleState = State.Menu_Opened;
						}
						else if (ModuleState == State.Menu_Opened)
						{
							Keyboard.Stroke((VirtualKeyShort)27, false);
							ModuleState = State.Menu_Closed;
						}
						Mouse.SetPosition(MouseStartPosition.X, MouseStartPosition.Y, true);
						CutsceneState = CinematicState.Done;
					}
					ModuleState = State.Ready;
					CutsceneState = CinematicState.Ready;
					MouseStartPosition = Point.get_Zero();
				}
			}
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
