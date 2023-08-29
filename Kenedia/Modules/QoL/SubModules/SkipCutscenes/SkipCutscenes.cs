using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Intern;
using Blish_HUD.GameIntegration.GfxSettings;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Kenedia.Modules.Core.Utility.WindowsUtil;
using Kenedia.Modules.QoL.Res;
using Kenedia.Modules.QoL.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.QoL.SubModules.SkipCutscenes
{
	public class SkipCutscenes : SubModule
	{
		private enum InteractStateType
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

		private enum CinematicStateType
		{
			Ready,
			InitialSleep,
			Clicked_Once,
			Sleeping,
			Clicked_Twice,
			Done
		}

		private readonly Logger _logger = Logger.GetLogger(typeof(SkipCutscenes));

		private CancellationTokenSource _cts;

		private Point _mousePosition;

		private readonly List<int> _introMaps = new List<int> { 573, 458, 138, 379, 432 };

		private readonly List<int> _starterMaps = new List<int> { 15, 19, 28, 34, 35 };

		private readonly GameStateDetectionService _gameStateDetectionService;

		public override SubModuleType SubModuleType => SubModuleType.SkipCutscenes;

		public SettingEntry<KeyBinding> Cancel_Key { get; private set; }

		public SkipCutscenes(SettingCollection settings, GameStateDetectionService gameStateDetectionService)
			: base(settings)
		{
			_gameStateDetectionService = gameStateDetectionService;
			GameService.GameIntegration.get_Gw2Instance().add_Gw2LostFocus((EventHandler<EventArgs>)Gw2Instance_Gw2LostFocus);
		}

		private void Gw2Instance_Gw2LostFocus(object sender, EventArgs e)
		{
			if (base.Enabled)
			{
				Cancel();
			}
		}

		public override void Update(GameTime gameTime)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			base.DefineSettings(settings);
			Cancel_Key = settings.DefineSetting<KeyBinding>("Cancel_Key", new KeyBinding((Keys)27), (Func<string>)null, (Func<string>)null);
			Cancel_Key.get_Value().set_Enabled(true);
			Cancel_Key.get_Value().add_Activated((EventHandler<EventArgs>)Cancel_Key_Activated);
		}

		private void Cancel_Key_Activated(object sender, EventArgs e)
		{
			if (base.Enabled)
			{
				_logger.Debug("Escape got pressed manually. Lets cancel!");
				Cancel();
			}
		}

		public override void Load()
		{
			base.Load();
			_gameStateDetectionService.GameStateChanged += On_GameStateChanged;
		}

		protected override void Enable()
		{
			base.Enable();
			_gameStateDetectionService.Enabled = true;
		}

		protected override void Disable()
		{
			base.Disable();
			_gameStateDetectionService.Enabled = false;
		}

		private async void On_GameStateChanged(object sender, GameStateChangedEventArgs e)
		{
			_logger.Info($"Gamestate changed to {e.Status}");
			switch (e.Status)
			{
			case GameStatusType.Vista:
				if (base.Enabled)
				{
					await SkipVista();
				}
				break;
			case GameStatusType.Cutscene:
				if (base.Enabled)
				{
					await SkipCutscene();
				}
				break;
			default:
				if (base.Enabled)
				{
					Cancel();
				}
				break;
			}
		}

		private void Cancel()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			if (_mousePosition != Point.get_Zero())
			{
				Mouse.SetPosition(_mousePosition.X, _mousePosition.Y, true);
			}
			_cts?.Cancel();
			_cts = null;
			_mousePosition = Point.get_Zero();
		}

		public override void Unload()
		{
			base.Unload();
			_gameStateDetectionService.GameStateChanged -= On_GameStateChanged;
			Cancel_Key.get_Value().remove_Activated((EventHandler<EventArgs>)Cancel_Key_Activated);
		}

		private async Task SkipCutscene()
		{
			_ = 3;
			try
			{
				if (_cts == null)
				{
					_cts = new CancellationTokenSource();
				}
				_logger.Info("SkipCutscene");
				_logger.Info("Press Escape and wait a bit");
				await Input.SendKey((Keys)27);
				await Task.Delay(250, _cts.Token);
				if (_cts == null || _cts.Token.IsCancellationRequested)
				{
					return;
				}
				_logger.Info("We are still in the cutscene lets try with mouse.");
				User32Dll.RECT pos = BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.Services.ClientWindowService.WindowBounds;
				Point p = new Point(pos.Right - 50, pos.Bottom - 35);
				Point j = GameService.Input.get_Mouse().get_Position();
				double factor = GameService.Graphics.get_UIScaleMultiplier();
				ScreenModeSetting? screenMode = GameService.GameIntegration.get_GfxSettings().get_ScreenMode();
				RectangleDimensions offset = (((screenMode.HasValue ? ScreenModeSetting.op_Implicit(screenMode.GetValueOrDefault()) : null) == ScreenModeSetting.op_Implicit(ScreenModeSetting.get_Windowed())) ? BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.Services.SharedSettings.WindowOffset : new RectangleDimensions(0));
				_mousePosition = new Point(pos.Left + (int)((double)j.X * factor) + offset.Left, pos.Top + offset.Top + (int)((double)j.Y * factor));
				for (int i = 0; i < 3; i++)
				{
					Mouse.SetPosition(p.X, p.Y, true);
					if (_cts == null || _cts.Token.IsCancellationRequested)
					{
						break;
					}
					await Task.Delay(25, _cts.Token);
					if (_cts == null || _cts.Token.IsCancellationRequested)
					{
						break;
					}
					_logger.Info("Click with the mouse in the bottom right corner.");
					Mouse.Click((MouseButton)0, p.X, p.Y, true);
					if (_cts == null || _cts.Token.IsCancellationRequested)
					{
						break;
					}
					await Task.Delay(125, _cts.Token);
					if (_cts == null || _cts.Token.IsCancellationRequested)
					{
						break;
					}
				}
			}
			catch (TaskCanceledException)
			{
			}
		}

		private async Task SkipVista()
		{
			try
			{
				if (_cts == null)
				{
					_cts = new CancellationTokenSource();
				}
				_logger.Info("Skip Vista with Escape.");
				await Input.SendKey((Keys)27);
			}
			catch (TaskCanceledException)
			{
			}
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
				KeyBinding val = new KeyBinding();
				val.set_ModifierKeys(kb.get_ModifierKeys());
				val.set_PrimaryKey(kb.get_PrimaryKey());
				val.set_Enabled(kb.get_Enabled());
				val.set_IgnoreWhenInTextField(true);
				hotKey.set_Value(val);
			};
			keybindingAssigner.SetLocalizedKeyBindingName = () => string.Format(strings.HotkeyEntry_Name, $"{SubModuleType}");
			keybindingAssigner.SetLocalizedTooltip = () => string.Format(strings.HotkeyEntry_Description, $"{SubModuleType}");
		}
	}
}
