using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Gw2Sharp.ChatLinks;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Kenedia.Modules.QoL.Res;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.QoL.SubModules.WaypointPaste
{
	public class WaypointPaste : SubModule
	{
		private double _ticks;

		private SettingEntry<KeyBinding> _pasteWaypoint;

		private SettingEntry<string> _waypoint;

		private SettingEntry<bool> _pasteCurrentClipboardWaypointFirst;

		public override SubModuleType SubModuleType => SubModuleType.WaypointPaste;

		public WaypointPaste(SettingCollection settings)
			: base(settings)
		{
		}

		public override void Update(GameTime gameTime)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			base.DefineSettings(settings);
			_pasteCurrentClipboardWaypointFirst = settings.DefineSetting("_pasteCurrentClipboardWaypointFirst", defaultValue: false);
			_pasteWaypoint = settings.DefineSetting("_pasteWaypoint", new KeyBinding((Keys)0));
			_waypoint = settings.DefineSetting("_waypoint", "[&BCAJAAA=]");
			_pasteWaypoint.Value.Enabled = true;
			_pasteWaypoint.Value.Activated += PasteWaypoint_Activated;
		}

		private async void PasteWaypoint()
		{
			if (!GameService.Gw2Mumble.Info.IsGameFocused)
			{
				return;
			}
			try
			{
				_ticks = Common.Now;
				string waypoint = _waypoint.Value;
				if (_pasteCurrentClipboardWaypointFirst.Value)
				{
					string currentContent = await ClipboardUtil.WindowsClipboardService.GetTextAsync();
					IGw2ChatLink chatLink = default(IGw2ChatLink);
					if (!string.IsNullOrEmpty(currentContent) && Gw2ChatLink.TryParse(currentContent, ref chatLink) && chatLink != null && (int)chatLink.get_Type() == 4)
					{
						waypoint = currentContent;
					}
				}
				await Input.SendKey((Keys)13);
				bool isReady = false;
				List<Keys> modifiers = new List<Keys>
				{
					(Keys)161,
					(Keys)160,
					(Keys)165,
					(Keys)164,
					(Keys)163,
					(Keys)162
				};
				modifiers.ForEach(delegate(Keys e)
				{
					//IL_0000: Unknown result type (might be due to invalid IL or missing references)
					Keyboard.Release((VirtualKeyShort)e, sendToSystem: true);
				});
				await Task.Delay(5);
				for (int i = 0; i < 500; i++)
				{
					if (GameService.Gw2Mumble.UI.IsTextInputFocused && !GameService.Input.Keyboard.KeysDown.ContainsAny(modifiers.ToArray()))
					{
						isReady = true;
						break;
					}
					await Task.Delay(5);
				}
				if (!isReady || !(await ClipboardUtil.WindowsClipboardService.SetTextAsync("/g1 ")))
				{
					return;
				}
				await Input.SendKey((Keys[])(object)new Keys[1] { (Keys)162 }, (Keys)86, sendToSystem: true);
				await Task.Delay(50);
				if (!(await ClipboardUtil.WindowsClipboardService.SetTextAsync("/w ")))
				{
					return;
				}
				await Input.SendKey((Keys[])(object)new Keys[1] { (Keys)162 }, (Keys)86, sendToSystem: true);
				if (await ClipboardUtil.WindowsClipboardService.SetTextAsync(GameService.Gw2Mumble.PlayerCharacter.Name))
				{
					await Input.SendKey((Keys[])(object)new Keys[1] { (Keys)162 }, (Keys)86, sendToSystem: true);
					await Input.SendKey((Keys)9, sendToSystem: true);
					if (await ClipboardUtil.WindowsClipboardService.SetTextAsync(waypoint))
					{
						await Input.SendKey((Keys[])(object)new Keys[1] { (Keys)162 }, (Keys)86, sendToSystem: true);
						await Input.SendKey((Keys)13);
						_ticks = Common.Now;
						base.Enabled = false;
					}
				}
			}
			catch
			{
			}
		}

		private void PasteWaypoint_Activated(object sender, EventArgs e)
		{
			PasteWaypoint();
		}

		protected override void Enable()
		{
			base.Enable();
			PasteWaypoint();
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
			UI.WrapWithLabel(() => string.Format(strings.ShowInHotbar_Name, base.Name), () => string.Format(strings.ShowInHotbar_Description, base.Name), contentFlowPanel, width - 16, new Kenedia.Modules.Core.Controls.Checkbox
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
				KeyBinding = _pasteWaypoint.Value,
				KeybindChangedAction = delegate(KeyBinding kb)
				{
					//IL_0019: Unknown result type (might be due to invalid IL or missing references)
					_pasteWaypoint.Value = new KeyBinding
					{
						ModifierKeys = kb.ModifierKeys,
						PrimaryKey = kb.PrimaryKey,
						Enabled = kb.Enabled,
						IgnoreWhenInTextField = true
					};
				},
				SetLocalizedKeyBindingName = () => strings.PasteWaypointHotkey_Name,
				SetLocalizedTooltip = () => strings.PasteWaypointHotkey_Tooltip
			};
			UI.WrapWithLabel(() => strings.WaypointChatcode_Name, () => strings.WaypointChatcode_Tooltip, contentFlowPanel, width - 16, new Kenedia.Modules.Core.Controls.TextBox
			{
				Text = _waypoint.Value,
				TextChangedAction = delegate(string txt)
				{
					_waypoint.Value = txt;
				}
			});
			UI.WrapWithLabel(() => strings.PasteWaypointFromClipboard_Name, () => strings.PasteWaypointFromClipboard_Tooltip, contentFlowPanel, width - 16, new Kenedia.Modules.Core.Controls.Checkbox
			{
				Checked = _pasteCurrentClipboardWaypointFirst.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_pasteCurrentClipboardWaypointFirst.Value = b;
				}
			});
		}
	}
}
