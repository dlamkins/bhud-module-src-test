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

		private readonly Label _destroyLabel;

		private readonly Label _instuctionLabel;

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
				Common.SetProperty(ref _state, value, OnStateSwitched);
			}
		}

		public override SubModuleType SubModuleType => SubModuleType.ItemDestruction;

		public ItemDestruction(SettingCollection settings)
			: base(settings)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			SubModuleUI uI_Elements = UI_Elements;
			MouseContainer mouseContainer = new MouseContainer();
			((Control)mouseContainer).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Container)mouseContainer).set_WidthSizingMode((SizingMode)1);
			((Container)mouseContainer).set_HeightSizingMode((SizingMode)1);
			mouseContainer.Background = new DetailedTexture(156003);
			mouseContainer.TexturePadding = new RectangleDimensions(50, 50, 0, 0);
			mouseContainer.BorderColor = Color.get_Black();
			mouseContainer.BorderWidth = new RectangleDimensions(3);
			((Control)mouseContainer).set_Visible(base.Enabled);
			mouseContainer.ContentPadding = new RectangleDimensions(5);
			mouseContainer.MouseOffset = new Point(25);
			MouseContainer item = mouseContainer;
			_mouseContainer = mouseContainer;
			uI_Elements.Add((Control)(object)item);
			Rectangle p = default(Rectangle);
			((Rectangle)(ref p))._002Ector(0, 0, 0, 0);
			BorderedImage borderedImage = new BorderedImage();
			((Control)borderedImage).set_Parent((Container)(object)_mouseContainer);
			((Control)borderedImage).set_Size(new Point(48));
			((Control)borderedImage).set_Location(new Point(0, ((Rectangle)(ref p)).get_Bottom()));
			borderedImage.BorderWidth = new RectangleDimensions(2);
			((Control)borderedImage).set_BackgroundColor(Color.get_Black() * 0.5f);
			_itemPreview = borderedImage;
			p = ((Control)_itemPreview).get_LocalBounds();
			FlowPanel flowPanel2 = new FlowPanel();
			((Control)flowPanel2).set_Parent((Container)(object)_mouseContainer);
			((Container)flowPanel2).set_WidthSizingMode((SizingMode)1);
			((Container)flowPanel2).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)flowPanel2).set_FlowDirection((ControlFlowDirection)3);
			((Control)flowPanel2).set_Location(new Point(((Rectangle)(ref p)).get_Right() + 5, ((Rectangle)(ref p)).get_Top()));
			((FlowPanel)flowPanel2).set_ControlPadding(new Vector2(5f));
			flowPanel2.ContentPadding = new RectangleDimensions(5, 0, 0, 0);
			FlowPanel flowPanel = flowPanel2;
			Label label = new Label();
			((Control)label).set_Parent((Container)(object)flowPanel);
			((Label)label).set_Font(GameService.Content.get_DefaultFont18());
			((Label)label).set_TextColor(Colors.Chardonnay);
			((Label)label).set_AutoSizeWidth(true);
			((Control)label).set_Height(GameService.Content.get_DefaultFont18().get_LineHeight());
			((Label)label).set_Text($"{SubModuleType}".SplitStringOnUppercase());
			Label label2 = new Label();
			((Control)label2).set_Parent((Container)(object)flowPanel);
			((Label)label2).set_Font(GameService.Content.get_DefaultFont14());
			((Label)label2).set_TextColor(Color.get_Lime());
			((Label)label2).set_AutoSizeWidth(true);
			((Control)label2).set_Height(GameService.Content.get_DefaultFont14().get_LineHeight());
			((Label)label2).set_Text("No Item selected yet.");
			_destroyLabel = label2;
			Label label3 = new Label();
			((Control)label3).set_Parent((Container)(object)flowPanel);
			((Label)label3).set_Font(GameService.Content.get_DefaultFont16());
			((Label)label3).set_TextColor(Color.get_White());
			((Label)label3).set_AutoSizeWidth(true);
			((Control)label3).set_Height(GameService.Content.get_DefaultFont16().get_LineHeight());
			((Label)label3).set_Text("SHIFT + Left Click on item!");
			_instuctionLabel = label3;
		}

		private void OnStateSwitched(object sender, ValueChangedEventArgs<ItemDestructionState> e)
		{
			_lastAction = Common.Now();
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
				((Label)_instuctionLabel).set_Text(instructions[State]);
				break;
			case ItemDestructionState.Selected:
				((Label)_instuctionLabel).set_Text(instructions[State]);
				break;
			case ItemDestructionState.Dragged:
				((Label)_instuctionLabel).set_Text(instructions[State]);
				break;
			case ItemDestructionState.Destroyed:
				((Label)_instuctionLabel).set_Text(instructions[State]);
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
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			base.DefineSettings(settings);
			_modifierToChat = settings.DefineSetting<KeyBinding>("_modifierToChat", new KeyBinding((Keys)160), (Func<string>)null, (Func<string>)null);
			_disableOnRightClick = settings.DefineSetting<bool>("_disableOnRightClick", true, (Func<string>)null, (Func<string>)null);
		}

		protected override void Disable()
		{
			base.Disable();
			((Control)_mouseContainer).Hide();
			State = ItemDestructionState.Disabled;
		}

		protected override void Enable()
		{
			base.Enable();
			((Control)_mouseContainer).Show();
			State = ItemDestructionState.None;
		}

		public override void Unload()
		{
			base.Unload();
			GameService.Input.get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)Mouse_LeftMouseButtonReleased);
			GameService.Input.get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)Mouse_LeftMouseButtonPressed);
			GameService.Input.get_Mouse().remove_RightMouseButtonReleased((EventHandler<MouseEventArgs>)Mouse_RightMouseButtonPressed);
		}

		public override void Load()
		{
			base.Load();
			GameService.Input.get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)Mouse_LeftMouseButtonReleased);
			GameService.Input.get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)Mouse_LeftMouseButtonPressed);
			GameService.Input.get_Mouse().add_RightMouseButtonReleased((EventHandler<MouseEventArgs>)Mouse_RightMouseButtonPressed);
		}

		private void Mouse_RightMouseButtonPressed(object sender, MouseEventArgs e)
		{
			if (base.Enabled && _disableOnRightClick.get_Value())
			{
				State = ItemDestructionState.None;
			}
		}

		private async void Mouse_LeftMouseButtonReleased(object sender, MouseEventArgs e)
		{
			if (!(Common.Now() - _lastAction < 150.0) && base.Enabled && State == ItemDestructionState.Selected)
			{
				await Task.Delay(50);
				if (GameService.Gw2Mumble.get_UI().get_IsTextInputFocused())
				{
					State = ItemDestructionState.Dragged;
					await Paste();
				}
			}
		}

		private async void Mouse_LeftMouseButtonPressed(object sender, MouseEventArgs e)
		{
			if (!(Common.Now() - _lastAction < 150.0) && base.Enabled)
			{
				if (GameService.Input.get_Keyboard().get_KeysDown().Contains((Keys)160))
				{
					ClientWindowService clientWindowService = BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.Services.ClientWindowService;
					SharedSettings _sharedSettings = BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.Services.SharedSettings;
					User32Dll.RECT wndBounds = clientWindowService.WindowBounds;
					ScreenModeSetting? screenMode = GameService.GameIntegration.get_GfxSettings().get_ScreenMode();
					Point p = (Point)(((screenMode.HasValue ? ScreenModeSetting.op_Implicit(screenMode.GetValueOrDefault()) : null) == ScreenModeSetting.op_Implicit(ScreenModeSetting.get_Windowed())) ? new Point(_sharedSettings.WindowOffset.Left, _sharedSettings.WindowOffset.Top) : Point.get_Zero());
					double factor = GameService.Graphics.get_UIScaleMultiplier();
					Point point = e.get_MousePosition().Add(new Point(-32));
					((Image)_itemPreview).set_Texture(ScreenCapture.CaptureRegion(wndBounds, p, new Rectangle(point, new Point(64)), factor, new Point(64)));
					_copiedText = await CopyItemFromChat();
					((Label)_destroyLabel).set_Text("Copied Name: " + _copiedText);
					((Label)_destroyLabel).set_Text(_copiedText);
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
				Keyboard.Press((VirtualKeyShort)162, true);
				Keyboard.Stroke((VirtualKeyShort)86, true);
				await Task.Delay(25);
				Keyboard.Release((VirtualKeyShort)162, true);
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
				_lastAction = Common.Now();
				await Task.Delay(50);
				Keyboard.Release((VirtualKeyShort)160, true);
				await Task.Delay(5);
				Keyboard.Press((VirtualKeyShort)162, true);
				Keyboard.Stroke((VirtualKeyShort)65, true);
				await Task.Delay(25);
				Keyboard.Stroke((VirtualKeyShort)67, true);
				await Task.Delay(50);
				Keyboard.Release((VirtualKeyShort)162, true);
				Keyboard.Stroke((VirtualKeyShort)8, true);
				Keyboard.Stroke((VirtualKeyShort)13, true);
				await Task.Delay(5);
				text = await ClipboardUtil.get_WindowsClipboardService().GetTextAsync();
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
				await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text);
				_lastAction = Common.Now();
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
			Func<string> localizedLabelContent = () => strings.DisableOnRightClick_Name;
			Func<string> localizedTooltip = () => strings.DisableOnRightClick_Tooltip;
			int width2 = width - 16;
			Checkbox checkbox = new Checkbox();
			((Control)checkbox).set_Height(20);
			((Checkbox)checkbox).set_Checked(_disableOnRightClick.get_Value());
			checkbox.CheckedChangedAction = delegate(bool b)
			{
				_disableOnRightClick.set_Value(b);
			};
			UI.WrapWithLabel(localizedLabelContent, localizedTooltip, (Container)(object)contentFlowPanel, width2, (Control)(object)checkbox);
		}
	}
}
