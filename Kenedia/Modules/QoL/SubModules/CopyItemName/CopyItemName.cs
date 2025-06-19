using System;
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

namespace Kenedia.Modules.QoL.SubModules.CopyItemName
{
	public class CopyItemName : SubModule
	{
		public enum ReturnType
		{
			BracketAmountName,
			AmountName,
			Name
		}

		private readonly BorderedImage _itemPreview;

		private readonly MouseContainer _mouseContainer;

		private readonly Kenedia.Modules.Core.Controls.Label _destroyLabel;

		private SettingEntry<bool> _disableOnCopy;

		private SettingEntry<bool> _disableOnRightClick;

		private SettingEntry<KeyBinding> _modifierToChat;

		private SettingEntry<ReturnType> _returnType;

		public override SubModuleType SubModuleType => SubModuleType.CopyItemName;

		public CopyItemName(SettingCollection settings)
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
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
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
				Text = "No Item name copied yet."
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = flowPanel,
				Font = GameService.Content.DefaultFont16,
				TextColor = Color.get_White(),
				AutoSizeWidth = true,
				AutoSizeHeight = true,
				Height = GameService.Content.DefaultFont16.get_LineHeight(),
				Text = "SHIFT + Left Click on item to copy its item name!"
			};
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			base.DefineSettings(settings);
			_disableOnCopy = settings.DefineSetting("_disableOnCopy", defaultValue: true);
			_disableOnRightClick = settings.DefineSetting("_disableOnRightClick", defaultValue: true);
			_modifierToChat = settings.DefineSetting("_modifierToChat", new KeyBinding((Keys)160));
			_returnType = settings.DefineSetting("_returnType", ReturnType.Name);
		}

		public override void Update(GameTime gameTime)
		{
			if (base.Enabled)
			{
				_mouseContainer.Visible = GameService.Input.Keyboard.KeysDown.Contains((Keys)160) || GameService.Input.Keyboard.KeysDown.Contains((Keys)161);
			}
		}

		protected override void Disable()
		{
			base.Disable();
			_mouseContainer.Visible = false;
		}

		protected override void Enable()
		{
			base.Enable();
			_mouseContainer.Visible = true;
		}

		public override void Load()
		{
			base.Load();
			GameService.Input.Mouse.LeftMouseButtonPressed += Mouse_LeftMouseButtonPressed;
			GameService.Input.Mouse.RightMouseButtonReleased += Mouse_RightMouseButtonPressed;
		}

		public override void Unload()
		{
			base.Unload();
			GameService.Input.Mouse.LeftMouseButtonPressed -= Mouse_LeftMouseButtonPressed;
			GameService.Input.Mouse.RightMouseButtonReleased -= Mouse_RightMouseButtonPressed;
		}

		private void Mouse_RightMouseButtonPressed(object sender, MouseEventArgs e)
		{
			if (base.Enabled && _disableOnRightClick.Value)
			{
				Disable();
			}
		}

		private async void Mouse_LeftMouseButtonPressed(object sender, MouseEventArgs e)
		{
			if (base.Enabled && (GameService.Input.Keyboard.KeysDown.Contains((Keys)160) || GameService.Input.Keyboard.KeysDown.Contains((Keys)161)))
			{
				ClientWindowService clientWindowService = BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.CoreServices.ClientWindowService;
				SharedSettings _sharedSettings = BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.CoreServices.SharedSettings;
				User32Dll.RECT wndBounds = clientWindowService.WindowBounds;
				ScreenModeSetting? screenMode = GameService.GameIntegration.GfxSettings.ScreenMode;
				Point p = (Point)(((screenMode.HasValue ? ((string)screenMode.GetValueOrDefault()) : null) == (string)ScreenModeSetting.Windowed) ? new Point(_sharedSettings.WindowOffset.Left, _sharedSettings.WindowOffset.Top) : Point.get_Zero());
				double factor = GameService.Graphics.UIScaleMultiplier;
				Point point = e.MousePosition.Add(new Point(-32));
				_itemPreview.Texture = ScreenCapture.CaptureRegion(wndBounds, p, new Rectangle(point, new Point(64)), factor, new Point(64));
				await GetItemNameFromChat();
			}
		}

		private async Task GetItemNameFromChat()
		{
			_ = 6;
			try
			{
				KeyboardLayoutType layout = BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.Settings.KeyboardLayout.Value;
				await Task.Delay(50);
				_ = _modifierToChat.Value.PrimaryKey;
				Keyboard.Release(VirtualKeyShort.LSHIFT, sendToSystem: true);
				Keyboard.Release(VirtualKeyShort.RSHIFT, sendToSystem: true);
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
				string text3 = await ClipboardUtil.WindowsClipboardService.GetTextAsync();
				if (string.IsNullOrEmpty(text3))
				{
					return;
				}
				string[] items = text3.Split('[');
				text3 = items.Last();
				if (_returnType.Value != 0)
				{
					if (text3.StartsWith("["))
					{
						text3 = text3.Substring(1);
					}
					if (text3.EndsWith("]"))
					{
						text3 = text3.Substring(0, text3.Length - 1);
					}
				}
				else
				{
					text3 = "[" + text3;
				}
				if (_returnType.Value == ReturnType.Name)
				{
					text3 = text3.RemoveLeadingNumbers();
					text3 = text3.TrimStart();
				}
				await ClipboardUtil.WindowsClipboardService.SetTextAsync(text3);
				_destroyLabel.Text = text3;
				if (_disableOnCopy.Value)
				{
					Disable();
				}
			}
			catch
			{
			}
		}

		public override void CreateSettingsPanel(Kenedia.Modules.Core.Controls.FlowPanel flowPanel, int width)
		{
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
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
			UI.WrapWithLabel(() => strings.DisableOnCopy_Name, () => strings.DisableOnCopy_Tooltip, contentFlowPanel, width - 16, new Kenedia.Modules.Core.Controls.Checkbox
			{
				Height = 20,
				Checked = _disableOnCopy.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_disableOnCopy.Value = b;
				}
			});
			UI.WrapWithLabel(() => strings.DisableOnRightClick_Name, () => strings.DisableOnRightClick_Tooltip, contentFlowPanel, width - 16, new Kenedia.Modules.Core.Controls.Checkbox
			{
				Height = 20,
				Checked = _disableOnRightClick.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_disableOnRightClick.Value = b;
				}
			});
			UI.WrapWithLabel(() => strings.ReturnType_Name, () => strings.ReturnType_Tooltip, contentFlowPanel, width - 16, new Kenedia.Modules.Core.Controls.Dropdown
			{
				Location = new Point(250, 0),
				Parent = contentFlowPanel,
				SetLocalizedItems = () => new List<string>
				{
					$"{ReturnType.Name}".SplitStringOnUppercase(),
					$"{ReturnType.AmountName}".SplitStringOnUppercase(),
					$"{ReturnType.BracketAmountName}".SplitStringOnUppercase()
				},
				SelectedItem = $"{_returnType.Value}",
				ValueChangedAction = delegate(string b)
				{
					_returnType.Value = (Enum.TryParse<ReturnType>(b.RemoveSpaces(), out var result) ? result : _returnType.Value);
				}
			});
		}
	}
}
