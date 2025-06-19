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

		private GameStatusType _loggedInSinceLastCutscene;

		private readonly List<int> _introMaps = new List<int> { 573, 458, 138, 379, 432 };

		private readonly List<int> _starterMaps = new List<int> { 15, 19, 28, 34, 35 };

		private readonly GameStateDetectionService _gameStateDetectionService;

		public override SubModuleType SubModuleType => SubModuleType.SkipCutscenes;

		public SettingEntry<KeyBinding> Cancel_Key { get; private set; }

		public SkipCutscenes(SettingCollection settings, GameStateDetectionService gameStateDetectionService)
			: base(settings)
		{
			_gameStateDetectionService = gameStateDetectionService;
			GameService.GameIntegration.Gw2Instance.Gw2LostFocus += Gw2Instance_Gw2LostFocus;
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
			base.DefineSettings(settings);
			Cancel_Key = settings.DefineSetting("Cancel_Key", new KeyBinding((Keys)27));
			Cancel_Key.Value.Enabled = true;
			Cancel_Key.Value.Activated += Cancel_Key_Activated;
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
			_gameStateDetectionService.GameStateChanged += new EventHandler<GameStateChangedEventArgs>(On_GameStateChanged);
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
			if (!base.Enabled)
			{
				return;
			}
			switch (e.Status)
			{
			case GameStatusType.Vista:
				if (_loggedInSinceLastCutscene == GameStatusType.Ingame)
				{
					_loggedInSinceLastCutscene = GameStatusType.Vista;
					await SkipVista();
				}
				break;
			case GameStatusType.Cutscene:
				if (_loggedInSinceLastCutscene == GameStatusType.Ingame)
				{
					_loggedInSinceLastCutscene = GameStatusType.Cutscene;
					await SkipCutscene();
				}
				break;
			default:
				Cancel();
				_loggedInSinceLastCutscene = e.Status;
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
				Mouse.SetPosition(_mousePosition.X, _mousePosition.Y, sendToSystem: true);
			}
			_cts?.Cancel();
			_cts = null;
			_mousePosition = Point.get_Zero();
		}

		public override void Unload()
		{
			base.Unload();
			_gameStateDetectionService.GameStateChanged -= new EventHandler<GameStateChangedEventArgs>(On_GameStateChanged);
			Cancel_Key.Value.Activated -= Cancel_Key_Activated;
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
				User32Dll.RECT pos = BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.CoreServices.ClientWindowService.WindowBounds;
				Point p = new Point(pos.Right - 50, pos.Bottom - 35);
				Point j = GameService.Input.Mouse.Position;
				double factor = GameService.Graphics.UIScaleMultiplier;
				ScreenModeSetting? screenMode = GameService.GameIntegration.GfxSettings.ScreenMode;
				RectangleDimensions offset = (((screenMode.HasValue ? ((string)screenMode.GetValueOrDefault()) : null) == (string)ScreenModeSetting.Windowed) ? BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.CoreServices.SharedSettings.WindowOffset : new RectangleDimensions(0));
				_mousePosition = new Point(pos.Left + (int)((double)j.X * factor) + offset.Left, pos.Top + offset.Top + (int)((double)j.Y * factor));
				for (int i = 0; i < 3; i++)
				{
					Mouse.SetPosition(p.X, p.Y, sendToSystem: true);
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
					Mouse.Click(MouseButton.LEFT, p.X, p.Y, sendToSystem: true);
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
			UI.WrapWithLabel(() => string.Format(strings.ShowInHotbar_Name, $"{SubModuleType}"), () => string.Format(strings.ShowInHotbar_Description, $"{SubModuleType}"), contentFlowPanel, width - 16, new Kenedia.Modules.Core.Controls.Checkbox
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
		}
	}
}
