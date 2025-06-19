using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.GameIntegration.GfxSettings;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Kenedia.Modules.Core.Utility.WindowsUtil;
using Kenedia.Modules.QoL.Res;
using Kenedia.Modules.QoL.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.QoL.SubModules.ItemDestruction
{
	internal class ItemDestruction : SubModule
	{
		private enum ItemDestructionState
		{
			Disabled,
			None,
			Selected,
			Dragged,
			Destroyed
		}

		private readonly BorderedImage _itemPreview;

		private readonly Kenedia.Modules.Core.Controls.Label _destroyLabel;

		private readonly Kenedia.Modules.Core.Controls.Label _instuctionLabel;

		private readonly MouseContainer _mouseContainer;

		private double _lastAction;

		private double _tick;

		private ItemDestructionState _state;

		private string _copiedText;

		private SettingEntry<bool> _disableOnRightClick;

		private SettingEntry<KeyBinding> _modifierToChat;

		private ItemDestructionState State
		{
			get
			{
				return _state;
			}
			set
			{
				Common.SetProperty(ref _state, value, new ValueChangedEventHandler<ItemDestructionState>(OnStateSwitched));
			}
		}

		public override SubModuleType SubModuleType => SubModuleType.ItemDestruction;

		public ItemDestruction(SettingCollection settings)
			: base(settings)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			SubModuleUI uI_Elements = UI_Elements;
			MouseContainer obj = new MouseContainer
			{
				Parent = GameService.Graphics.SpriteScreen,
				WidthSizingMode = SizingMode.AutoSize,
				HeightSizingMode = SizingMode.AutoSize,
				Background = new DetailedTexture(156003),
				TexturePadding = new RectangleDimensions(50, 50, 0, 0),
				BorderColor = Color.get_Black(),
				BorderWidth = new RectangleDimensions(3),
				Visible = base.Enabled,
				ContentPadding = new RectangleDimensions(5),
				MouseOffset = new Point(25),
				ZIndex = int.MaxValue
			};
			MouseContainer item = obj;
			_mouseContainer = obj;
			uI_Elements.Add(item);
			Rectangle p = default(Rectangle);
			((Rectangle)(ref p))._002Ector(0, 0, 0, 0);
			_itemPreview = new BorderedImage
			{
				Parent = _mouseContainer,
				Size = new Point(48),
				Location = new Point(0, ((Rectangle)(ref p)).get_Bottom()),
				BorderWidth = new RectangleDimensions(2),
				BackgroundColor = Color.get_Black() * 0.5f
			};
			p = _itemPreview.LocalBounds;
			Kenedia.Modules.Core.Controls.FlowPanel flowPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = _mouseContainer,
				WidthSizingMode = SizingMode.AutoSize,
				HeightSizingMode = SizingMode.AutoSize,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				Location = new Point(((Rectangle)(ref p)).get_Right() + 5, ((Rectangle)(ref p)).get_Top()),
				ControlPadding = new Vector2(5f),
				ContentPadding = new RectangleDimensions(5, 0, 0, 0)
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = flowPanel,
				Font = GameService.Content.DefaultFont18,
				TextColor = ContentService.Colors.Chardonnay,
				AutoSizeWidth = true,
				Height = GameService.Content.DefaultFont18.get_LineHeight(),
				Text = $"{SubModuleType}".SplitStringOnUppercase()
			};
			_destroyLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = flowPanel,
				Font = GameService.Content.DefaultFont14,
				TextColor = Color.get_Lime(),
				AutoSizeWidth = true,
				Height = GameService.Content.DefaultFont14.get_LineHeight(),
				Text = "No Item selected yet."
			};
			_instuctionLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = flowPanel,
				Font = GameService.Content.DefaultFont16,
				TextColor = Color.get_White(),
				AutoSizeWidth = true,
				Height = GameService.Content.DefaultFont16.get_LineHeight(),
				Text = "SHIFT + Left Click on item!"
			};
		}

		private void OnStateSwitched(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<ItemDestructionState> e)
		{
			_lastAction = Common.Now;
			Dictionary<ItemDestructionState, string> instructions = new Dictionary<ItemDestructionState, string>
			{
				{
					ItemDestructionState.None,
					"SHIFT + Left Click on item!"
				},
				{
					ItemDestructionState.Selected,
					"Drag the item out of your inventory!"
				},
				{
					ItemDestructionState.Dragged,
					"Press 'Yes'!"
				},
				{
					ItemDestructionState.Destroyed,
					"SHIFT + Left Click on item!"
				}
			};
			switch (State)
			{
			case ItemDestructionState.None:
				_instuctionLabel.Text = instructions[State];
				break;
			case ItemDestructionState.Selected:
				_instuctionLabel.Text = instructions[State];
				break;
			case ItemDestructionState.Dragged:
				_instuctionLabel.Text = instructions[State];
				break;
			case ItemDestructionState.Destroyed:
				_instuctionLabel.Text = instructions[State];
				break;
			}
		}

		public override void Update(GameTime gameTime)
		{
			if (gameTime.get_TotalGameTime().TotalMilliseconds - _tick > 500.0)
			{
				_tick = gameTime.get_TotalGameTime().TotalMilliseconds;
			}
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			base.DefineSettings(settings);
			_modifierToChat = settings.DefineSetting("_modifierToChat", new KeyBinding((Keys)160));
			_disableOnRightClick = settings.DefineSetting("_disableOnRightClick", defaultValue: true);
		}

		protected override void Disable()
		{
			base.Disable();
			_mouseContainer.Hide();
			State = ItemDestructionState.Disabled;
		}

		protected override void Enable()
		{
			base.Enable();
			_mouseContainer.Show();
			State = ItemDestructionState.None;
		}

		public override void Unload()
		{
			base.Unload();
			GameService.Input.Mouse.LeftMouseButtonReleased -= Mouse_LeftMouseButtonReleased;
			GameService.Input.Mouse.LeftMouseButtonPressed -= Mouse_LeftMouseButtonPressed;
			GameService.Input.Mouse.RightMouseButtonReleased -= Mouse_RightMouseButtonPressed;
		}

		public override void Load()
		{
			base.Load();
			GameService.Input.Mouse.LeftMouseButtonReleased += Mouse_LeftMouseButtonReleased;
			GameService.Input.Mouse.LeftMouseButtonPressed += Mouse_LeftMouseButtonPressed;
			GameService.Input.Mouse.RightMouseButtonReleased += Mouse_RightMouseButtonPressed;
		}

		private void Mouse_RightMouseButtonPressed(object sender, MouseEventArgs e)
		{
			if (base.Enabled && _disableOnRightClick.Value)
			{
				Disable();
				State = ItemDestructionState.None;
			}
		}

		private async void Mouse_LeftMouseButtonReleased(object sender, MouseEventArgs e)
		{
			if (!(Common.Now - _lastAction < 150.0) && base.Enabled && State == ItemDestructionState.Selected)
			{
				await Task.Delay(50);
				if (GameService.Gw2Mumble.UI.IsTextInputFocused)
				{
					State = ItemDestructionState.Dragged;
					await Paste();
				}
			}
		}

		private async void Mouse_LeftMouseButtonPressed(object sender, MouseEventArgs e)
		{
			if (!(Common.Now - _lastAction < 150.0) && base.Enabled)
			{
				if (GameService.Input.Keyboard.KeysDown.Contains((Keys)160) || GameService.Input.Keyboard.KeysDown.Contains((Keys)161))
				{
					ClientWindowService clientWindowService = BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.CoreServices.ClientWindowService;
					SharedSettings _sharedSettings = BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.CoreServices.SharedSettings;
					User32Dll.RECT wndBounds = clientWindowService.WindowBounds;
					ScreenModeSetting? screenMode = GameService.GameIntegration.GfxSettings.ScreenMode;
					Point p = (Point)(((screenMode.HasValue ? ((string)screenMode.GetValueOrDefault()) : null) == (string)ScreenModeSetting.Windowed) ? new Point(_sharedSettings.WindowOffset.Left, _sharedSettings.WindowOffset.Top) : Point.get_Zero());
					double factor = GameService.Graphics.UIScaleMultiplier;
					Point point = e.MousePosition.Add(new Point(-32));
					_itemPreview.Texture = ScreenCapture.CaptureRegion(wndBounds, p, new Rectangle(point, new Point(64)), factor, new Point(64));
					_copiedText = await CopyItemFromChat();
					_destroyLabel.Text = "Copied Name: " + _copiedText;
					_destroyLabel.Text = _copiedText;
					State = ItemDestructionState.Selected;
				}
				else if (State == ItemDestructionState.Dragged)
				{
					State = ItemDestructionState.Destroyed;
				}
			}
		}

		private async Task Paste()
		{
			try
			{
				Keyboard.Press(VirtualKeyShort.LCONTROL, sendToSystem: true);
				Keyboard.Stroke(VirtualKeyShort.KEY_V, sendToSystem: true);
				await Task.Delay(25);
				Keyboard.Release(VirtualKeyShort.LCONTROL, sendToSystem: true);
			}
			catch
			{
			}
		}

		private async Task<string> CopyItemFromChat()
		{
			string text = string.Empty;
			try
			{
				KeyboardLayoutType layout = BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.Settings.KeyboardLayout.Value;
				_lastAction = Common.Now;
				await Task.Delay(50);
				Keyboard.Release(VirtualKeyShort.LSHIFT, sendToSystem: true);
				await Task.Delay(5);
				Keyboard.Press(VirtualKeyShort.LCONTROL, sendToSystem: true);
				VirtualKeyShort key = ((layout != KeyboardLayoutType.AZERTY) ? VirtualKeyShort.KEY_A : VirtualKeyShort.KEY_Q);
				Keyboard.Stroke(key, sendToSystem: true);
				await Task.Delay(25);
				Keyboard.Stroke(VirtualKeyShort.KEY_C, sendToSystem: true);
				await Task.Delay(50);
				Keyboard.Release(VirtualKeyShort.LCONTROL, sendToSystem: true);
				Keyboard.Stroke(VirtualKeyShort.BACK, sendToSystem: true);
				Keyboard.Stroke(VirtualKeyShort.RETURN, sendToSystem: true);
				await Task.Delay(5);
				text = await ClipboardUtil.WindowsClipboardService.GetTextAsync();
				if (string.IsNullOrEmpty(text))
				{
					return string.Empty;
				}
				string[] items = text.Split('[');
				text = items.Last();
				if (text.StartsWith("["))
				{
					text = text.Substring(1);
				}
				if (text.EndsWith("]"))
				{
					text = text.Substring(0, text.Length - 1);
				}
				await ClipboardUtil.WindowsClipboardService.SetTextAsync(text);
				_lastAction = Common.Now;
			}
			catch
			{
			}
			return text;
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
			UI.WrapWithLabel(() => strings.DisableOnRightClick_Name, () => strings.DisableOnRightClick_Tooltip, contentFlowPanel, width - 16, new Kenedia.Modules.Core.Controls.Checkbox
			{
				Height = 20,
				Checked = _disableOnRightClick.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_disableOnRightClick.Value = b;
				}
			});
		}
	}
}
