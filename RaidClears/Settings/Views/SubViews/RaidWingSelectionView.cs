using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.SubViews
{
	public class RaidWingSelectionView : View
	{
		private readonly RaidSettings _settings;

		public RaidWingSelectionView(RaidSettings settings)
			: this()
		{
			_settings = settings;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Expected O, but got Unknown
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Expected O, but got Unknown
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Expected O, but got Unknown
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanel panel = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel);
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_OuterControlPadding(new Vector2(20f, 5f));
			((Control)val).set_Parent((Container)(object)panel);
			((Panel)val).set_ShowTint(false);
			((Panel)val).set_ShowBorder(false);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(((Control)panel).get_Width() - 40);
			((Panel)val).set_BackgroundTexture(AsyncTexture2D.op_Implicit(Service.Textures!.HoTLogo));
			panel.AddChildPanel((Panel)(object)FlowPanelExtensions.AddSpace(val).AddString(Strings.Settings_Raid_Hot_Heading).AddSetting((IEnumerable<SettingEntry>?)_settings.RaidWings.Take(4))
				.AddSpace()
				.AddSpace());
			FlowPanel val2 = new FlowPanel();
			val2.set_FlowDirection((ControlFlowDirection)3);
			val2.set_OuterControlPadding(new Vector2(20f, 5f));
			((Control)val2).set_Parent((Container)(object)panel);
			((Panel)val2).set_ShowTint(false);
			((Panel)val2).set_ShowBorder(false);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Width(((Control)panel).get_Width() - 40);
			((Panel)val2).set_BackgroundTexture(AsyncTexture2D.op_Implicit(Service.Textures!.PoFLogo));
			panel.AddChildPanel((Panel)(object)FlowPanelExtensions.AddSpace(val2).AddString(Strings.Settings_Raid_PoF_Heading).AddSetting((IEnumerable<SettingEntry>?)_settings.RaidWings.Skip(4).Take(3))
				.AddSpace()
				.AddSpace()
				.AddSpace());
			FlowPanel val3 = new FlowPanel();
			val3.set_FlowDirection((ControlFlowDirection)3);
			val3.set_OuterControlPadding(new Vector2(20f, 5f));
			((Control)val3).set_Parent((Container)(object)panel);
			((Panel)val3).set_ShowTint(false);
			((Panel)val3).set_ShowBorder(false);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			((Control)val3).set_Width(((Control)panel).get_Width() - 40);
			((Panel)val3).set_BackgroundTexture(AsyncTexture2D.op_Implicit(Service.Textures!.JWLogo));
			panel.AddChildPanel((Panel)(object)FlowPanelExtensions.AddSpace(val3).AddString(Strings.Settings_Raid_JW_Heading).AddSetting((IEnumerable<SettingEntry>?)_settings.RaidWings.Skip(7).Take(1))
				.AddSpace()
				.AddSpace()
				.AddSpace()
				.AddSpace()
				.AddSpace());
		}
	}
}
