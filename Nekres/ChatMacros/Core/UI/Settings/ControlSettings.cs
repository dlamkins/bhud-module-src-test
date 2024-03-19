using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using Nekres.ChatMacros.Core.UI.Configs;
using Nekres.ChatMacros.Properties;

namespace Nekres.ChatMacros.Core.UI.Settings
{
	internal class ControlSettings : View
	{
		private ControlsConfig _config;

		public ControlSettings(ControlsConfig config)
			: this()
		{
			_config = config;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Expected O, but got Unknown
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Expected O, but got Unknown
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(buildPanel.get_ContentRegion().Width);
			((Control)val).set_Height(buildPanel.get_ContentRegion().Height);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_OuterControlPadding(new Vector2(5f, 5f));
			val.set_ControlPadding(new Vector2(5f, 5f));
			((Panel)val).set_CanCollapse(true);
			((Panel)val).set_Title(Resources.Control_Options);
			FlowPanel miscSettings = val;
			KeybindingAssigner val2 = new KeybindingAssigner(_config.OpenQuickAccess);
			((Control)val2).set_Parent((Container)(object)miscSettings);
			val2.set_KeyBindingName(Resources.Open_Quick_Access);
			((Control)val2).set_BasicTooltipText(Resources.Show_or_hide_the_quick_access_menu_to_all_active_macros_);
			FlowPanel val3 = new FlowPanel();
			((Control)val3).set_Parent((Container)(object)miscSettings);
			((Control)val3).set_Width(((Container)miscSettings).get_ContentRegion().Width);
			((Control)val3).set_Height(((Container)miscSettings).get_ContentRegion().Height);
			val3.set_FlowDirection((ControlFlowDirection)3);
			val3.set_OuterControlPadding(new Vector2(5f, 5f));
			val3.set_ControlPadding(new Vector2(5f, 5f));
			((Panel)val3).set_CanCollapse(true);
			((Panel)val3).set_Title(Resources.User_Interface);
			FlowPanel userInterface = val3;
			KeybindingAssigner val4 = new KeybindingAssigner(_config.ChatMessage);
			((Control)val4).set_Parent((Container)(object)userInterface);
			val4.set_KeyBindingName(Resources.Chat_Message);
			((Control)val4).set_BasicTooltipText(Resources.Give_focus_to_the_chat_edit_box_);
			KeybindingAssigner val5 = new KeybindingAssigner(_config.SquadBroadcastMessage);
			((Control)val5).set_Parent((Container)(object)userInterface);
			val5.set_KeyBindingName(Resources.Squad_Broadcast_Message);
			((Control)val5).set_BasicTooltipText(Resources.Give_focus_to_the_chat_edit_box_);
			((View<IPresenter>)this).Build(buildPanel);
		}
	}
}
