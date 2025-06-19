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
			_mouseContainer = new MouseContainer
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
			Rectangle p = default(Rectangle);
			((Rectangle)(ref p))._002Ector(0, 0, 0, 0);
			p = new BorderedImage
			{
				Parent = _mouseContainer,
				Size = new Point(48),
				Location = new Point(0, ((Rectangle)(ref p)).get_Bottom()),
				BorderWidth = new RectangleDimensions(1),
				BackgroundColor = Color.get_Black() * 0.5f,
				BorderColor = Color.get_Black() * 0.8f,
				Texture = base.Icon.Texture
			}.LocalBounds;
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
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = flowPanel,
				Font = GameService.Content.DefaultFont16,
				TextColor = Color.get_White(),
				AutoSizeWidth = true,
				AutoSizeHeight = true,
				Height = GameService.Content.DefaultFont16.get_LineHeight(),
				Text = "SHIFT + Left Click on item to open the wiki page!\nYou must release the SHIFT button after the click!"
			};
		}

		public override void Update(GameTime gameTime)
		{
			if (base.Enabled)
			{
				_mouseContainer.Visible = GameService.Input.Keyboard.KeysDown.Contains((Keys)160) || GameService.Input.Keyboard.KeysDown.Contains((Keys)161);
			}
		}

		protected override void Enable()
		{
			base.Enable();
		}

		protected override void Disable()
		{
			base.Disable();
			_mouseContainer.Visible = false;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			base.DefineSettings(settings);
			_disableOnSearch = settings.DefineSetting("_disableOnSearch", defaultValue: true);
			_disableOnRightClick = settings.DefineSetting("_disableOnRightClick", defaultValue: true);
			_modifierToChat = settings.DefineSetting("_modifierToChat", new KeyBinding((Keys)160));
			_language = settings.DefineSetting("_language", WikiLocale.Locale.Default);
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
					if (GameService.Gw2Mumble.UI.IsTextInputFocused && !GameService.Input.Keyboard.KeysDown.Contains((Keys)161) && !GameService.Input.Keyboard.KeysDown.Contains((Keys)160))
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
					Keyboard.Release(VirtualKeyShort.LSHIFT);
					await Task.Delay(10);
				}
				int delay = 40;
				Keyboard.Press(VirtualKeyShort.LCONTROL, sendToSystem: true);
				await Task.Delay(delay);
				Keyboard.Stroke(VirtualKeyShort.LEFT, sendToSystem: true);
				await Task.Delay(delay);
				bool hasWiki = await ClipboardUtil.WindowsClipboardService.SetTextAsync(GetWikiCommand());
				if (hasWiki)
				{
					Keyboard.Stroke(VirtualKeyShort.KEY_V, sendToSystem: true);
					await Task.Delay(delay);
				}
				Keyboard.Release(VirtualKeyShort.LCONTROL, sendToSystem: true);
				if (!hasWiki)
				{
					Keyboard.Stroke(VirtualKeyShort.BACK, sendToSystem: true);
					Keyboard.Stroke(VirtualKeyShort.RETURN, sendToSystem: true);
					return;
				}
				await Task.Delay(delay);
				Keyboard.Stroke(VirtualKeyShort.RETURN, sendToSystem: true);
				await Task.Delay(delay);
				await Task.Delay(300);
				GameService.GameIntegration.Gw2Instance.FocusGw2();
				if (_disableOnSearch.Value)
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
			return _language.Value switch
			{
				WikiLocale.Locale.English => "/wiki en:", 
				WikiLocale.Locale.German => "/wiki de: ", 
				WikiLocale.Locale.French => "/wiki fr: ", 
				WikiLocale.Locale.Spanish => "/wiki es:", 
				_ => "/wiki ", 
			};
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
			UI.WrapWithLabel(() => strings.DisableOnSearch_Name, () => strings.DisableOnSearch_Tooltip, contentFlowPanel, width - 16, new Kenedia.Modules.Core.Controls.Checkbox
			{
				Height = 20,
				Checked = _disableOnSearch.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_disableOnSearch.Value = b;
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
			Enum.GetValues(typeof(WikiLocale.Locale));
			UI.WrapWithLabel(() => strings.Language_Name, () => strings.Language_Tooltip, contentFlowPanel, width - 16, new Kenedia.Modules.Core.Controls.Dropdown
			{
				Height = 20,
				SelectedItem = WikiLocale.ToDisplayString(_language.Value),
				SetLocalizedItems = () => WikiLocale.Locales.Values.ToList(),
				ValueChangedAction = delegate(string s)
				{
					_language.Value = WikiLocale.FromDisplayString(s);
				}
			});
		}
	}
}
