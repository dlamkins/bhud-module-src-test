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

		private readonly Label _destroyLabel;

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
			((Control)mouseContainer).set_ZIndex(int.MaxValue);
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
			((Label)label2).set_Text("No Item name copied yet.");
			_destroyLabel = label2;
			Label label3 = new Label();
			((Control)label3).set_Parent((Container)(object)flowPanel);
			((Label)label3).set_Font(GameService.Content.get_DefaultFont16());
			((Label)label3).set_TextColor(Color.get_White());
			((Label)label3).set_AutoSizeWidth(true);
			((Label)label3).set_AutoSizeHeight(true);
			((Control)label3).set_Height(GameService.Content.get_DefaultFont16().get_LineHeight());
			((Label)label3).set_Text("SHIFT + Left Click on item to copy its item name!");
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			base.DefineSettings(settings);
			_disableOnCopy = settings.DefineSetting<bool>("_disableOnCopy", true, (Func<string>)null, (Func<string>)null);
			_disableOnRightClick = settings.DefineSetting<bool>("_disableOnRightClick", true, (Func<string>)null, (Func<string>)null);
			_modifierToChat = settings.DefineSetting<KeyBinding>("_modifierToChat", new KeyBinding((Keys)160), (Func<string>)null, (Func<string>)null);
			_returnType = settings.DefineSetting<ReturnType>("_returnType", ReturnType.Name, (Func<string>)null, (Func<string>)null);
		}

		public override void Update(GameTime gameTime)
		{
			if (base.Enabled)
			{
				((Control)_mouseContainer).set_Visible(GameService.Input.get_Keyboard().get_KeysDown().Contains((Keys)160) || GameService.Input.get_Keyboard().get_KeysDown().Contains((Keys)161));
			}
		}

		protected override void Disable()
		{
			base.Disable();
			((Control)_mouseContainer).set_Visible(false);
		}

		protected override void Enable()
		{
			base.Enable();
			((Control)_mouseContainer).set_Visible(true);
		}

		public override void Load()
		{
			base.Load();
			GameService.Input.get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)Mouse_LeftMouseButtonPressed);
			GameService.Input.get_Mouse().add_RightMouseButtonReleased((EventHandler<MouseEventArgs>)Mouse_RightMouseButtonPressed);
		}

		public override void Unload()
		{
			base.Unload();
			GameService.Input.get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)Mouse_LeftMouseButtonPressed);
			GameService.Input.get_Mouse().remove_RightMouseButtonReleased((EventHandler<MouseEventArgs>)Mouse_RightMouseButtonPressed);
		}

		private void Mouse_RightMouseButtonPressed(object sender, MouseEventArgs e)
		{
			if (base.Enabled && _disableOnRightClick.get_Value())
			{
				Disable();
			}
		}

		private async void Mouse_LeftMouseButtonPressed(object sender, MouseEventArgs e)
		{
			if (base.Enabled && (GameService.Input.get_Keyboard().get_KeysDown().Contains((Keys)160) || GameService.Input.get_Keyboard().get_KeysDown().Contains((Keys)161)))
			{
				ClientWindowService clientWindowService = BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.Services.ClientWindowService;
				SharedSettings _sharedSettings = BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.Services.SharedSettings;
				User32Dll.RECT wndBounds = clientWindowService.WindowBounds;
				ScreenModeSetting? screenMode = GameService.GameIntegration.get_GfxSettings().get_ScreenMode();
				Point p = (Point)(((screenMode.HasValue ? ScreenModeSetting.op_Implicit(screenMode.GetValueOrDefault()) : null) == ScreenModeSetting.op_Implicit(ScreenModeSetting.get_Windowed())) ? new Point(_sharedSettings.WindowOffset.Left, _sharedSettings.WindowOffset.Top) : Point.get_Zero());
				double factor = GameService.Graphics.get_UIScaleMultiplier();
				Point point = e.get_MousePosition().Add(new Point(-32));
				((Image)_itemPreview).set_Texture(ScreenCapture.CaptureRegion(wndBounds, p, new Rectangle(point, new Point(64)), factor, new Point(64)));
				await GetItemNameFromChat();
			}
		}

		private async Task GetItemNameFromChat()
		{
			_ = 6;
			try
			{
				KeyboardLayoutType layout = BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.Settings.KeyboardLayout.get_Value();
				await Task.Delay(50);
				_modifierToChat.get_Value().get_PrimaryKey();
				Keyboard.Release((VirtualKeyShort)160, true);
				Keyboard.Release((VirtualKeyShort)161, true);
				await Task.Delay(5);
				Keyboard.Press((VirtualKeyShort)162, true);
				VirtualKeyShort val = ((layout != KeyboardLayoutType.AZERTY) ? ((VirtualKeyShort)65) : ((VirtualKeyShort)81));
				Keyboard.Stroke(val, true);
				await Task.Delay(25);
				Keyboard.Stroke((VirtualKeyShort)67, true);
				await Task.Delay(50);
				Keyboard.Release((VirtualKeyShort)162, true);
				Keyboard.Stroke((VirtualKeyShort)8, true);
				Keyboard.Stroke((VirtualKeyShort)13, true);
				await Task.Delay(5);
				string text3 = await ClipboardUtil.get_WindowsClipboardService().GetTextAsync();
				if (string.IsNullOrEmpty(text3))
				{
					return;
				}
				string[] items = text3.Split('[');
				text3 = items.Last();
				if (_returnType.get_Value() != 0)
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
				if (_returnType.get_Value() == ReturnType.Name)
				{
					text3 = text3.RemoveLeadingNumbers();
					text3 = text3.TrimStart();
				}
				await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text3);
				((Label)_destroyLabel).set_Text(text3);
				if (_disableOnCopy.get_Value())
				{
					Disable();
				}
			}
			catch
			{
			}
		}

		public override void CreateSettingsPanel(FlowPanel flowPanel, int width)
		{
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
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
			Func<string> localizedLabelContent2 = () => strings.DisableOnCopy_Name;
			Func<string> localizedTooltip2 = () => strings.DisableOnCopy_Tooltip;
			int width3 = width - 16;
			Checkbox checkbox2 = new Checkbox();
			((Control)checkbox2).set_Height(20);
			((Checkbox)checkbox2).set_Checked(_disableOnCopy.get_Value());
			checkbox2.CheckedChangedAction = delegate(bool b)
			{
				_disableOnCopy.set_Value(b);
			};
			UI.WrapWithLabel(localizedLabelContent2, localizedTooltip2, (Container)(object)contentFlowPanel, width3, (Control)(object)checkbox2);
			Func<string> localizedLabelContent3 = () => strings.DisableOnRightClick_Name;
			Func<string> localizedTooltip3 = () => strings.DisableOnRightClick_Tooltip;
			int width4 = width - 16;
			Checkbox checkbox3 = new Checkbox();
			((Control)checkbox3).set_Height(20);
			((Checkbox)checkbox3).set_Checked(_disableOnRightClick.get_Value());
			checkbox3.CheckedChangedAction = delegate(bool b)
			{
				_disableOnRightClick.set_Value(b);
			};
			UI.WrapWithLabel(localizedLabelContent3, localizedTooltip3, (Container)(object)contentFlowPanel, width4, (Control)(object)checkbox3);
			Func<string> localizedLabelContent4 = () => strings.ReturnType_Name;
			Func<string> localizedTooltip4 = () => strings.ReturnType_Tooltip;
			int width5 = width - 16;
			Dropdown dropdown = new Dropdown();
			((Control)dropdown).set_Location(new Point(250, 0));
			((Control)dropdown).set_Parent((Container)(object)contentFlowPanel);
			dropdown.SetLocalizedItems = () => new List<string>
			{
				$"{ReturnType.Name}".SplitStringOnUppercase(),
				$"{ReturnType.AmountName}".SplitStringOnUppercase(),
				$"{ReturnType.BracketAmountName}".SplitStringOnUppercase()
			};
			((Dropdown)dropdown).set_SelectedItem($"{_returnType.get_Value()}");
			dropdown.ValueChangedAction = delegate(string b)
			{
				_returnType.set_Value(Enum.TryParse<ReturnType>(b.RemoveSpaces(), out var result) ? result : _returnType.get_Value());
			};
			UI.WrapWithLabel(localizedLabelContent4, localizedTooltip4, (Container)(object)contentFlowPanel, width5, (Control)(object)dropdown);
		}
	}
}
