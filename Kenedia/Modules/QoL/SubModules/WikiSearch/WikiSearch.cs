using System;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Kenedia.Modules.QoL.Res;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.QoL.SubModules.WikiSearch
{
	public class WikiSearch : SubModule
	{
		private readonly MouseContainer _mouseContainer;

		private SettingEntry<bool> _disableOnRightClick;

		private SettingEntry<bool> _disableOnSearch;

		private SettingEntry<KeyBinding> _modifierToChat;

		private SettingEntry<WikiLocale.Locale> _language;

		public override SubModuleType SubModuleType => SubModuleType.WikiSearch;

		public WikiSearch(SettingCollection settings)
			: base(settings)
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
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
			_mouseContainer = mouseContainer;
			Rectangle p = default(Rectangle);
			((Rectangle)(ref p))._002Ector(0, 0, 0, 0);
			BorderedImage borderedImage = new BorderedImage();
			((Control)borderedImage).set_Parent((Container)(object)_mouseContainer);
			((Control)borderedImage).set_Size(new Point(48));
			((Control)borderedImage).set_Location(new Point(0, ((Rectangle)(ref p)).get_Bottom()));
			borderedImage.BorderWidth = new RectangleDimensions(1);
			((Control)borderedImage).set_BackgroundColor(Color.get_Black() * 0.5f);
			borderedImage.BorderColor = Color.get_Black() * 0.8f;
			((Image)borderedImage).set_Texture(base.Icon.Texture);
			p = ((Control)borderedImage).get_LocalBounds();
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
			((Label)label2).set_Font(GameService.Content.get_DefaultFont16());
			((Label)label2).set_TextColor(Color.get_White());
			((Label)label2).set_AutoSizeWidth(true);
			((Label)label2).set_AutoSizeHeight(true);
			((Control)label2).set_Height(GameService.Content.get_DefaultFont16().get_LineHeight());
			((Label)label2).set_Text("SHIFT + Left Click on item to open the wiki page!\nYou must release the SHIFT button after the click!");
		}

		public override void Update(GameTime gameTime)
		{
			if (base.Enabled)
			{
				((Control)_mouseContainer).set_Visible(GameService.Input.get_Keyboard().get_KeysDown().Contains((Keys)160) || GameService.Input.get_Keyboard().get_KeysDown().Contains((Keys)161));
			}
		}

		protected override void Enable()
		{
			base.Enable();
		}

		protected override void Disable()
		{
			base.Disable();
			((Control)_mouseContainer).set_Visible(false);
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			base.DefineSettings(settings);
			_disableOnSearch = settings.DefineSetting<bool>("_disableOnSearch", true, (Func<string>)null, (Func<string>)null);
			_disableOnRightClick = settings.DefineSetting<bool>("_disableOnRightClick", true, (Func<string>)null, (Func<string>)null);
			_modifierToChat = settings.DefineSetting<KeyBinding>("_modifierToChat", new KeyBinding((Keys)160), (Func<string>)null, (Func<string>)null);
			_language = settings.DefineSetting<WikiLocale.Locale>("_language", WikiLocale.Locale.Default, (Func<string>)null, (Func<string>)null);
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
				await OpenWikiForItemFromChat();
			}
		}

		private async Task OpenWikiForItemFromChat()
		{
			_ = 9;
			try
			{
				await Task.Delay(50);
				bool isReady = false;
				for (int j = 0; j < 500; j++)
				{
					if (GameService.Gw2Mumble.get_UI().get_IsTextInputFocused() && !GameService.Input.get_Keyboard().get_KeysDown().Contains((Keys)161) && !GameService.Input.get_Keyboard().get_KeysDown().Contains((Keys)160))
					{
						isReady = true;
						break;
					}
					await Task.Delay(5);
				}
				if (!isReady)
				{
					return;
				}
				for (int j = 0; j < 5; j++)
				{
					Keyboard.Release((VirtualKeyShort)160, false);
					await Task.Delay(10);
				}
				int delay = 40;
				Keyboard.Press((VirtualKeyShort)162, true);
				await Task.Delay(delay);
				Keyboard.Stroke((VirtualKeyShort)37, true);
				await Task.Delay(delay);
				bool hasWiki = await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(GetWikiCommand());
				if (hasWiki)
				{
					Keyboard.Stroke((VirtualKeyShort)86, true);
					await Task.Delay(delay);
				}
				Keyboard.Release((VirtualKeyShort)162, true);
				if (!hasWiki)
				{
					Keyboard.Stroke((VirtualKeyShort)8, true);
					Keyboard.Stroke((VirtualKeyShort)13, true);
					return;
				}
				await Task.Delay(delay);
				Keyboard.Stroke((VirtualKeyShort)13, true);
				await Task.Delay(delay);
				await Task.Delay(300);
				GameService.GameIntegration.get_Gw2Instance().FocusGw2();
				if (_disableOnSearch.get_Value())
				{
					Disable();
				}
			}
			catch
			{
			}
		}

		private string GetWikiCommand()
		{
			return _language.get_Value() switch
			{
				WikiLocale.Locale.English => "/wiki en:", 
				WikiLocale.Locale.German => "/wiki de: ", 
				WikiLocale.Locale.French => "/wiki fr: ", 
				WikiLocale.Locale.Spanish => "/wiki es:", 
				_ => "/wiki ", 
			};
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
			Func<string> localizedLabelContent2 = () => strings.DisableOnSearch_Name;
			Func<string> localizedTooltip2 = () => strings.DisableOnSearch_Tooltip;
			int width3 = width - 16;
			Checkbox checkbox2 = new Checkbox();
			((Control)checkbox2).set_Height(20);
			((Checkbox)checkbox2).set_Checked(_disableOnSearch.get_Value());
			checkbox2.CheckedChangedAction = delegate(bool b)
			{
				_disableOnSearch.set_Value(b);
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
			Enum.GetValues(typeof(WikiLocale.Locale));
			Func<string> localizedLabelContent4 = () => strings.Language_Name;
			Func<string> localizedTooltip4 = () => strings.Language_Tooltip;
			int width5 = width - 16;
			Dropdown dropdown = new Dropdown();
			((Control)dropdown).set_Height(20);
			((Dropdown)dropdown).set_SelectedItem(WikiLocale.ToDisplayString(_language.get_Value()));
			dropdown.SetLocalizedItems = () => WikiLocale.Locales.Values.ToList();
			dropdown.ValueChangedAction = delegate(string s)
			{
				_language.set_Value(WikiLocale.FromDisplayString(s));
			};
			UI.WrapWithLabel(localizedLabelContent4, localizedTooltip4, (Container)(object)contentFlowPanel, width5, (Control)(object)dropdown);
		}
	}
}
