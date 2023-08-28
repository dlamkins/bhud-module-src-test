using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Blish_HUD.Settings;
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
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			base.DefineSettings(settings);
			_pasteWaypoint = settings.DefineSetting<KeyBinding>("_pasteWaypoint", new KeyBinding((Keys)0), (Func<string>)null, (Func<string>)null);
			_waypoint = settings.DefineSetting<string>("_waypoint", "[&BCAJAAA=]", (Func<string>)null, (Func<string>)null);
			_pasteWaypoint.get_Value().set_Enabled(true);
			_pasteWaypoint.get_Value().add_Activated((EventHandler<EventArgs>)PasteWaypoint_Activated);
		}

		private async void PasteWaypoint()
		{
			if (!GameService.Gw2Mumble.get_Info().get_IsGameFocused())
			{
				return;
			}
			try
			{
				_ticks = Common.Now();
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
					Keyboard.Release((VirtualKeyShort)(short)e, true);
				});
				await Task.Delay(5);
				for (int i = 0; i < 500; i++)
				{
					if (GameService.Gw2Mumble.get_UI().get_IsTextInputFocused() && !GameService.Input.get_Keyboard().get_KeysDown().ContainsAny(modifiers.ToArray()))
					{
						isReady = true;
						break;
					}
					await Task.Delay(5);
				}
				if (!isReady || !(await ClipboardUtil.get_WindowsClipboardService().SetTextAsync("/g1 ")))
				{
					return;
				}
				await Input.SendKey((Keys[])(object)new Keys[1] { (Keys)162 }, (Keys)86, sendToSystem: true);
				await Task.Delay(50);
				if (!(await ClipboardUtil.get_WindowsClipboardService().SetTextAsync("/w ")))
				{
					return;
				}
				await Input.SendKey((Keys[])(object)new Keys[1] { (Keys)162 }, (Keys)86, sendToSystem: true);
				if (await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(GameService.Gw2Mumble.get_PlayerCharacter().get_Name()))
				{
					await Input.SendKey((Keys[])(object)new Keys[1] { (Keys)162 }, (Keys)86, sendToSystem: true);
					await Input.SendKey((Keys)9, sendToSystem: true);
					if (await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(_waypoint.get_Value()))
					{
						await Input.SendKey((Keys[])(object)new Keys[1] { (Keys)162 }, (Keys)86, sendToSystem: true);
						await Input.SendKey((Keys)13);
						_ticks = Common.Now();
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
			Func<string> localizedLabelContent = () => strings.WaypointChatcode_Name;
			Func<string> localizedTooltip = () => strings.WaypointChatcode_Tooltip;
			int width2 = width - 16;
			TextBox textBox = new TextBox();
			((TextInputBase)textBox).set_Text(_waypoint.get_Value());
			textBox.TextChangedAction = delegate(string txt)
			{
				_waypoint.set_Value(txt);
			};
			UI.WrapWithLabel(localizedLabelContent, localizedTooltip, (Container)(object)contentFlowPanel, width2, (Control)(object)textBox);
			KeybindingAssigner keybindingAssigner = new KeybindingAssigner();
			((Control)keybindingAssigner).set_Parent((Container)(object)contentFlowPanel);
			((Control)keybindingAssigner).set_Width(width - 16);
			((KeybindingAssigner)keybindingAssigner).set_KeyBinding(_pasteWaypoint.get_Value());
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
				SettingEntry<KeyBinding> pasteWaypoint = _pasteWaypoint;
				KeyBinding val = new KeyBinding();
				val.set_ModifierKeys(kb.get_ModifierKeys());
				val.set_PrimaryKey(kb.get_PrimaryKey());
				val.set_Enabled(kb.get_Enabled());
				val.set_IgnoreWhenInTextField(true);
				pasteWaypoint.set_Value(val);
			};
			keybindingAssigner.SetLocalizedKeyBindingName = () => strings.PasteWaypointHotkey_Name;
			keybindingAssigner.SetLocalizedTooltip = () => strings.PasteWaypointHotkey_Tooltip;
		}
	}
}
