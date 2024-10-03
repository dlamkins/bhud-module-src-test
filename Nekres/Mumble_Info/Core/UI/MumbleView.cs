using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using Nekres.Mumble_Info.Core.UI.Controls;

namespace Nekres.Mumble_Info.Core.UI
{
	internal class MumbleView : View<MumblePresenter>
	{
		private BitmapFont _font = GameService.Content.GetFont((FontFace)0, (FontSize)16, (FontStyle)0);

		private const int SCROLLBAR_WIDTH = 13;

		private readonly Color _grey = new Color(168, 168, 168);

		private readonly Color _orange = new Color(252, 168, 0);

		private readonly Color _red = new Color(252, 84, 84);

		private readonly Color _softRed = new Color(250, 148, 148);

		private readonly Color _green = new Color(0, 168, 0);

		private readonly Color _lemonGreen = new Color(84, 252, 84);

		private readonly Color _cyan = new Color(84, 252, 252);

		private readonly Color _blue = new Color(0, 168, 252);

		private readonly Color _brown = new Color(158, 81, 44);

		private readonly Color _yellow = new Color(252, 252, 84);

		private readonly Color _softYellow = new Color(250, 250, 148);

		public MumbleView()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			base.WithPresenter(new MumblePresenter(this, MumbleInfoModule.Instance.MumbleConfig.get_Value()));
		}

		protected override void Build(Container buildPanel)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Expected O, but got Unknown
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Expected O, but got Unknown
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Expected O, but got Unknown
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Expected I4, but got Unknown
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_0397: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_040b: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_051d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0522: Unknown result type (might be due to invalid IL or missing references)
			//IL_052e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0539: Unknown result type (might be due to invalid IL or missing references)
			//IL_0540: Unknown result type (might be due to invalid IL or missing references)
			//IL_0552: Unknown result type (might be due to invalid IL or missing references)
			//IL_0559: Unknown result type (might be due to invalid IL or missing references)
			//IL_0564: Unknown result type (might be due to invalid IL or missing references)
			//IL_056e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0579: Unknown result type (might be due to invalid IL or missing references)
			//IL_0583: Unknown result type (might be due to invalid IL or missing references)
			//IL_058f: Expected O, but got Unknown
			//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0663: Unknown result type (might be due to invalid IL or missing references)
			//IL_069a: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0702: Unknown result type (might be due to invalid IL or missing references)
			//IL_0709: Unknown result type (might be due to invalid IL or missing references)
			//IL_071b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0722: Unknown result type (might be due to invalid IL or missing references)
			//IL_072d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0737: Unknown result type (might be due to invalid IL or missing references)
			//IL_0742: Unknown result type (might be due to invalid IL or missing references)
			//IL_074c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0758: Expected O, but got Unknown
			//IL_0781: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_084b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0850: Unknown result type (might be due to invalid IL or missing references)
			//IL_085c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0867: Unknown result type (might be due to invalid IL or missing references)
			//IL_086e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0880: Unknown result type (might be due to invalid IL or missing references)
			//IL_0887: Unknown result type (might be due to invalid IL or missing references)
			//IL_0892: Unknown result type (might be due to invalid IL or missing references)
			//IL_089c: Unknown result type (might be due to invalid IL or missing references)
			//IL_08a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_08b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_08bd: Expected O, but got Unknown
			//IL_08e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_090b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0946: Unknown result type (might be due to invalid IL or missing references)
			//IL_0972: Unknown result type (might be due to invalid IL or missing references)
			//IL_09e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a06: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a41: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aa0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ac5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ae3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b4b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bb6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bbb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bc7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bd2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bd9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0beb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bf2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bfd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c07: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c12: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c1c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c28: Expected O, but got Unknown
			//IL_0c28: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c2d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c39: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c40: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c4f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c57: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c63: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c6e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c70: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c7b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c80: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c8c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c93: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ca2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0caa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cb6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cda: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cdc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d10: Unknown result type (might be due to invalid IL or missing references)
			//IL_0da3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dda: Unknown result type (might be due to invalid IL or missing references)
			//IL_0de6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e4e: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(buildPanel);
			((Panel)val).set_CanCollapse(false);
			((Control)val).set_Width(buildPanel.get_ContentRegion().Width);
			((Control)val).set_Height(buildPanel.get_ContentRegion().Height);
			((Panel)val).set_CanScroll(true);
			FlowPanel flowContainer = val;
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)flowContainer);
			((Panel)val2).set_CanCollapse(false);
			((Control)val2).set_Width(((Container)flowContainer).get_ContentRegion().Width - 13);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_FlowDirection((ControlFlowDirection)3);
			val2.set_OuterControlPadding(new Vector2(8f, 10f));
			((Panel)val2).set_ShowBorder(true);
			FlowPanel pnlConfigs = val2;
			Checkbox val3 = new Checkbox();
			((Control)val3).set_Parent((Container)(object)pnlConfigs);
			val3.set_Text("Swap YZ");
			((Control)val3).set_BasicTooltipText("Swap values of Y and Z for 3D coordinates.");
			((Control)val3).set_Height(25);
			val3.set_Checked(((Presenter<MumbleView, MumbleConfig>)base.get_Presenter()).get_Model().SwapYZ);
			val3.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object _, CheckChangedEvent e)
			{
				((Presenter<MumbleView, MumbleConfig>)base.get_Presenter()).get_Model().SwapYZ = !e.get_Checked();
			});
			KeybindingAssigner val4 = new KeybindingAssigner(((Presenter<MumbleView, MumbleConfig>)base.get_Presenter()).get_Model().Shortcut);
			((Control)val4).set_Parent((Container)(object)pnlConfigs);
			val4.set_KeyBindingName("Mumble Info Panel");
			((Control)val4).set_BasicTooltipText("Open or close the Mumble Info panel.");
			FlowPanel val5 = new FlowPanel();
			((Control)val5).set_Parent((Container)(object)flowContainer);
			((Panel)val5).set_Title("Avatar");
			((Control)val5).set_Width(((Container)flowContainer).get_ContentRegion().Width - 13);
			((Container)val5).set_HeightSizingMode((SizingMode)1);
			val5.set_ControlPadding(new Vector2(5f, 2f));
			val5.set_OuterControlPadding(new Vector2(10f, 2f));
			((Panel)val5).set_Collapsed(false);
			FlowPanel pnlAvatar = val5;
			DynamicLabel dynamicLabel = new DynamicLabel(base.get_Presenter().GetRace);
			((Control)dynamicLabel).set_Parent((Container)(object)pnlAvatar);
			((Control)dynamicLabel).set_Width(((Container)pnlAvatar).get_ContentRegion().Width);
			((Control)dynamicLabel).set_Height(25);
			((Label)dynamicLabel).set_Font(_font);
			dynamicLabel.StrokeTextData = true;
			DynamicLabel dynamicLabel2 = new DynamicLabel(base.get_Presenter().GetPlayerProfession);
			((Control)dynamicLabel2).set_Parent((Container)(object)pnlAvatar);
			((Control)dynamicLabel2).set_Width(((Container)pnlAvatar).get_ContentRegion().Width);
			((Control)dynamicLabel2).set_Height(25);
			((Label)dynamicLabel2).set_Font(_font);
			dynamicLabel2.Icon = MumbleInfoModule.Instance.Api.GetClassIcon((int)GameService.Gw2Mumble.get_PlayerCharacter().get_Profession(), GameService.Gw2Mumble.get_PlayerCharacter().get_Specialization());
			dynamicLabel2.StrokeTextData = true;
			DynamicLabel dynamicLabel3 = new DynamicLabel(base.get_Presenter().GetPlayerPosition);
			((Control)dynamicLabel3).set_Parent((Container)(object)pnlAvatar);
			((Control)dynamicLabel3).set_Width(((Container)pnlAvatar).get_ContentRegion().Width);
			((Control)dynamicLabel3).set_Height(25);
			((Label)dynamicLabel3).set_Font(_font);
			((Label)dynamicLabel3).set_Text("Position: ");
			((Label)dynamicLabel3).set_StrokeText(true);
			((Label)dynamicLabel3).set_TextColor(_green);
			dynamicLabel3.TextDataColor = _lemonGreen;
			((Control)dynamicLabel3).add_MouseEntered((EventHandler<MouseEventArgs>)OnLabelEnter);
			((Control)dynamicLabel3).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs _)
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				OnLabelLeft(o, _green, _lemonGreen);
			});
			((Control)dynamicLabel3).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await base.get_Presenter().CopyToClipboard(base.get_Presenter().GetPlayerPosition(markerPackFormat: true));
			});
			DynamicLabel dynamicLabel4 = new DynamicLabel(base.get_Presenter().GetPlayerDirection);
			((Control)dynamicLabel4).set_Parent((Container)(object)pnlAvatar);
			((Control)dynamicLabel4).set_Width(((Container)pnlAvatar).get_ContentRegion().Width);
			((Control)dynamicLabel4).set_Height(25);
			((Label)dynamicLabel4).set_Font(_font);
			((Label)dynamicLabel4).set_Text("Direction: ");
			((Label)dynamicLabel4).set_StrokeText(true);
			((Label)dynamicLabel4).set_TextColor(_green);
			dynamicLabel4.TextDataColor = _lemonGreen;
			((Control)dynamicLabel4).add_MouseEntered((EventHandler<MouseEventArgs>)OnLabelEnter);
			((Control)dynamicLabel4).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs _)
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				OnLabelLeft(o, _green, _lemonGreen);
			});
			((Control)dynamicLabel4).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await base.get_Presenter().CopyToClipboard(base.get_Presenter().GetPlayerDirection(markerPackFormat: true));
			});
			DynamicLabel dynamicLabel5 = new DynamicLabel(base.get_Presenter().GetClosestWaypoint);
			((Control)dynamicLabel5).set_Parent((Container)(object)pnlAvatar);
			((Control)dynamicLabel5).set_Width(((Container)pnlAvatar).get_ContentRegion().Width);
			((Control)dynamicLabel5).set_Height(25);
			((Label)dynamicLabel5).set_Font(_font);
			dynamicLabel5.Icon = MumbleInfoModule.Instance.Api.WaypointIcon;
			dynamicLabel5.StrokeTextData = true;
			((Control)dynamicLabel5).add_MouseEntered((EventHandler<MouseEventArgs>)OnLabelEnter);
			((Control)dynamicLabel5).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs _)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				OnLabelLeft(o, Color.get_White());
			});
			((Control)dynamicLabel5).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				if (MumbleInfoModule.Instance.Api.ClosestWaypoint != null)
				{
					await base.get_Presenter().CopyToClipboard(MumbleInfoModule.Instance.Api.ClosestWaypoint.get_ChatLink());
				}
			});
			DynamicLabel dynamicLabel6 = new DynamicLabel(base.get_Presenter().GetClosestPoi);
			((Control)dynamicLabel6).set_Parent((Container)(object)pnlAvatar);
			((Control)dynamicLabel6).set_Width(((Container)pnlAvatar).get_ContentRegion().Width);
			((Control)dynamicLabel6).set_Height(25);
			((Label)dynamicLabel6).set_Font(_font);
			dynamicLabel6.Icon = MumbleInfoModule.Instance.Api.PoiIcon;
			dynamicLabel6.StrokeTextData = true;
			((Control)dynamicLabel6).add_MouseEntered((EventHandler<MouseEventArgs>)OnLabelEnter);
			((Control)dynamicLabel6).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs _)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				OnLabelLeft(o, Color.get_White());
			});
			((Control)dynamicLabel6).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				if (MumbleInfoModule.Instance.Api.ClosestPoi != null)
				{
					await base.get_Presenter().CopyToClipboard(MumbleInfoModule.Instance.Api.ClosestPoi.get_ChatLink());
				}
			});
			FlowPanel val6 = new FlowPanel();
			((Control)val6).set_Parent((Container)(object)flowContainer);
			((Panel)val6).set_Title("Camera");
			((Control)val6).set_Width(((Container)flowContainer).get_ContentRegion().Width - 13);
			((Container)val6).set_HeightSizingMode((SizingMode)1);
			val6.set_ControlPadding(new Vector2(5f, 2f));
			val6.set_OuterControlPadding(new Vector2(10f, 2f));
			((Panel)val6).set_Collapsed(false);
			FlowPanel pnlCamera = val6;
			DynamicLabel dynamicLabel7 = new DynamicLabel(base.get_Presenter().GetCameraPosition);
			((Control)dynamicLabel7).set_Parent((Container)(object)pnlCamera);
			((Control)dynamicLabel7).set_Width(((Container)pnlCamera).get_ContentRegion().Width);
			((Control)dynamicLabel7).set_Height(25);
			((Label)dynamicLabel7).set_Font(_font);
			((Label)dynamicLabel7).set_Text("Position: ");
			((Label)dynamicLabel7).set_StrokeText(true);
			((Label)dynamicLabel7).set_TextColor(_red);
			dynamicLabel7.TextDataColor = _softRed;
			((Control)dynamicLabel7).add_MouseEntered((EventHandler<MouseEventArgs>)OnLabelEnter);
			((Control)dynamicLabel7).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs _)
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				OnLabelLeft(o, _red, _softRed);
			});
			((Control)dynamicLabel7).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await base.get_Presenter().CopyToClipboard(base.get_Presenter().GetCameraPosition(markerPackFormat: true));
			});
			DynamicLabel dynamicLabel8 = new DynamicLabel(base.get_Presenter().GetCameraDirection);
			((Control)dynamicLabel8).set_Parent((Container)(object)pnlCamera);
			((Control)dynamicLabel8).set_Width(((Container)pnlCamera).get_ContentRegion().Width);
			((Control)dynamicLabel8).set_Height(25);
			((Label)dynamicLabel8).set_Font(_font);
			((Label)dynamicLabel8).set_Text("Direction: ");
			((Label)dynamicLabel8).set_StrokeText(true);
			((Label)dynamicLabel8).set_TextColor(_red);
			dynamicLabel8.TextDataColor = _softRed;
			((Control)dynamicLabel8).add_MouseEntered((EventHandler<MouseEventArgs>)OnLabelEnter);
			((Control)dynamicLabel8).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs _)
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				OnLabelLeft(o, _red, _softRed);
			});
			((Control)dynamicLabel8).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await base.get_Presenter().CopyToClipboard(base.get_Presenter().GetCameraDirection(markerPackFormat: true));
			});
			FlowPanel val7 = new FlowPanel();
			((Control)val7).set_Parent((Container)(object)flowContainer);
			((Panel)val7).set_Title("User Interface");
			((Control)val7).set_Width(((Container)flowContainer).get_ContentRegion().Width - 13);
			((Container)val7).set_HeightSizingMode((SizingMode)1);
			val7.set_ControlPadding(new Vector2(5f, 2f));
			val7.set_OuterControlPadding(new Vector2(10f, 2f));
			((Panel)val7).set_Collapsed(false);
			FlowPanel pnlUserInterface = val7;
			DynamicLabel dynamicLabel9 = new DynamicLabel(base.get_Presenter().GetUiSize);
			((Control)dynamicLabel9).set_Parent((Container)(object)pnlUserInterface);
			((Control)dynamicLabel9).set_Width(((Container)pnlUserInterface).get_ContentRegion().Width);
			((Control)dynamicLabel9).set_Height(25);
			((Label)dynamicLabel9).set_Font(_font);
			((Label)dynamicLabel9).set_Text("UI Size: ");
			((Label)dynamicLabel9).set_StrokeText(true);
			DynamicLabel dynamicLabel10 = new DynamicLabel(base.get_Presenter().GetCompassBounds);
			((Control)dynamicLabel10).set_Parent((Container)(object)pnlUserInterface);
			((Control)dynamicLabel10).set_Width(((Container)pnlUserInterface).get_ContentRegion().Width);
			((Control)dynamicLabel10).set_Height(25);
			((Label)dynamicLabel10).set_Font(_font);
			((Label)dynamicLabel10).set_Text("Compass: ");
			((Label)dynamicLabel10).set_StrokeText(true);
			((Control)dynamicLabel10).add_MouseEntered((EventHandler<MouseEventArgs>)OnLabelEnter);
			((Control)dynamicLabel10).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs _)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				OnLabelLeft(o, Color.get_White());
			});
			((Control)dynamicLabel10).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await base.get_Presenter().CopyToClipboard(base.get_Presenter().GetCompassBounds());
			});
			FlowPanel val8 = new FlowPanel();
			((Control)val8).set_Parent((Container)(object)flowContainer);
			((Panel)val8).set_Title("Map");
			((Control)val8).set_Width(((Container)flowContainer).get_ContentRegion().Width - 13);
			((Container)val8).set_HeightSizingMode((SizingMode)1);
			val8.set_ControlPadding(new Vector2(5f, 2f));
			val8.set_OuterControlPadding(new Vector2(10f, 2f));
			((Panel)val8).set_Collapsed(false);
			FlowPanel pnlMap = val8;
			DynamicLabel dynamicLabel11 = new DynamicLabel(base.get_Presenter().GetContinent);
			((Control)dynamicLabel11).set_Parent((Container)(object)pnlMap);
			((Control)dynamicLabel11).set_Width(((Container)pnlMap).get_ContentRegion().Width);
			((Control)dynamicLabel11).set_Height(25);
			((Label)dynamicLabel11).set_Font(_font);
			dynamicLabel11.TextDataColor = _softYellow;
			dynamicLabel11.StrokeTextData = true;
			DynamicLabel dynamicLabel12 = new DynamicLabel(base.get_Presenter().GetMap);
			((Control)dynamicLabel12).set_Parent((Container)(object)pnlMap);
			((Control)dynamicLabel12).set_Width(((Container)pnlMap).get_ContentRegion().Width);
			((Control)dynamicLabel12).set_Height(25);
			((Label)dynamicLabel12).set_Font(_font);
			((Label)dynamicLabel12).set_StrokeText(true);
			dynamicLabel12.TextDataColor = _yellow;
			dynamicLabel12.StrokeTextData = true;
			((Control)dynamicLabel12).add_MouseEntered((EventHandler<MouseEventArgs>)OnLabelEnter);
			((Control)dynamicLabel12).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs _)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				OnLabelLeft(o, Color.get_White(), _yellow);
			});
			((Control)dynamicLabel12).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await base.get_Presenter().CopyToClipboard(base.get_Presenter().GetMap());
			});
			DynamicLabel dynamicLabel13 = new DynamicLabel(base.get_Presenter().GetSector);
			((Control)dynamicLabel13).set_Parent((Container)(object)pnlMap);
			((Control)dynamicLabel13).set_Width(((Container)pnlMap).get_ContentRegion().Width);
			((Control)dynamicLabel13).set_Height(25);
			((Label)dynamicLabel13).set_Font(_font);
			dynamicLabel13.TextDataColor = _orange;
			dynamicLabel13.StrokeTextData = true;
			DynamicLabel dynamicLabel14 = new DynamicLabel(base.get_Presenter().GetMapType);
			((Control)dynamicLabel14).set_Parent((Container)(object)pnlMap);
			((Control)dynamicLabel14).set_Width(((Container)pnlMap).get_ContentRegion().Width);
			((Control)dynamicLabel14).set_Height(25);
			((Label)dynamicLabel14).set_Font(_font);
			((Label)dynamicLabel14).set_Text("Type: ");
			((Label)dynamicLabel14).set_StrokeText(true);
			DynamicLabel dynamicLabel15 = new DynamicLabel(base.get_Presenter().GetMapPosition);
			((Control)dynamicLabel15).set_Parent((Container)(object)pnlMap);
			((Control)dynamicLabel15).set_Width(((Container)pnlMap).get_ContentRegion().Width);
			((Control)dynamicLabel15).set_Height(25);
			((Label)dynamicLabel15).set_Font(_font);
			((Label)dynamicLabel15).set_TextColor(_red);
			((Label)dynamicLabel15).set_Text("Position: ");
			((Label)dynamicLabel15).set_StrokeText(true);
			dynamicLabel15.TextDataColor = _softRed;
			((Control)dynamicLabel15).add_MouseEntered((EventHandler<MouseEventArgs>)OnLabelEnter);
			((Control)dynamicLabel15).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs _)
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				OnLabelLeft(o, _red, _softRed);
			});
			((Control)dynamicLabel15).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await base.get_Presenter().CopyToClipboard(base.get_Presenter().GetMapPosition(markerPackFormat: true));
			});
			DynamicLabel dynamicLabel16 = new DynamicLabel(base.get_Presenter().GetMapHash);
			((Control)dynamicLabel16).set_Parent((Container)(object)pnlMap);
			((Control)dynamicLabel16).set_Width(((Container)pnlMap).get_ContentRegion().Width);
			((Control)dynamicLabel16).set_Height(25);
			((Label)dynamicLabel16).set_Font(_font);
			((Label)dynamicLabel16).set_Text("Hash: ");
			((Label)dynamicLabel16).set_StrokeText(true);
			((Control)dynamicLabel16).add_MouseEntered((EventHandler<MouseEventArgs>)OnLabelEnter);
			((Control)dynamicLabel16).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs _)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				OnLabelLeft(o, Color.get_White());
			});
			((Control)dynamicLabel16).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await base.get_Presenter().CopyToClipboard(base.get_Presenter().GetMapHash(discordRichPresenceFormat: true));
			});
			FlowPanel val9 = new FlowPanel();
			((Control)val9).set_Parent((Container)(object)flowContainer);
			((Panel)val9).set_Title("Info");
			((Control)val9).set_Width(((Container)flowContainer).get_ContentRegion().Width - 13);
			((Container)val9).set_HeightSizingMode((SizingMode)1);
			val9.set_ControlPadding(new Vector2(5f, 2f));
			val9.set_OuterControlPadding(new Vector2(10f, 2f));
			((Panel)val9).set_Collapsed(false);
			FlowPanel pnlInfo = val9;
			Label val10 = new Label();
			((Control)val10).set_Parent((Container)(object)pnlInfo);
			((Control)val10).set_Width(((Container)pnlInfo).get_ContentRegion().Width);
			((Control)val10).set_Height(25);
			val10.set_Font(_font);
			val10.set_Text(BlishUtil.GetVersion());
			val10.set_TextColor(_grey);
			Label val11 = new Label();
			((Control)val11).set_Parent((Container)(object)pnlInfo);
			((Control)val11).set_Width(((Container)pnlInfo).get_ContentRegion().Width);
			((Control)val11).set_Height(25);
			val11.set_Font(_font);
			val11.set_Text($"Mumble Link v{GameService.Gw2Mumble.get_Info().get_Version()}");
			val11.set_TextColor(_grey);
			DynamicLabel dynamicLabel17 = new DynamicLabel(base.get_Presenter().GetProcessId);
			((Control)dynamicLabel17).set_Parent((Container)(object)pnlInfo);
			((Control)dynamicLabel17).set_Width(((Container)pnlInfo).get_ContentRegion().Width);
			((Control)dynamicLabel17).set_Height(25);
			((Label)dynamicLabel17).set_Font(_font);
			((Label)dynamicLabel17).set_Text("PID: ");
			((Label)dynamicLabel17).set_StrokeText(true);
			((Control)dynamicLabel17).add_MouseEntered((EventHandler<MouseEventArgs>)OnLabelEnter);
			((Control)dynamicLabel17).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs _)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				OnLabelLeft(o, Color.get_White());
			});
			((Control)dynamicLabel17).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await base.get_Presenter().CopyToClipboard(base.get_Presenter().GetProcessId());
			});
			DynamicLabel dynamicLabel18 = new DynamicLabel(base.get_Presenter().GetServerAddress);
			((Control)dynamicLabel18).set_Parent((Container)(object)pnlInfo);
			((Control)dynamicLabel18).set_Width(((Container)pnlInfo).get_ContentRegion().Width);
			((Control)dynamicLabel18).set_Height(25);
			((Label)dynamicLabel18).set_Font(_font);
			((Label)dynamicLabel18).set_Text("Server Addr.: ");
			((Label)dynamicLabel18).set_StrokeText(true);
			((Label)dynamicLabel18).set_TextColor(_blue);
			dynamicLabel18.TextDataColor = _cyan;
			((Control)dynamicLabel18).add_MouseEntered((EventHandler<MouseEventArgs>)OnLabelEnter);
			((Control)dynamicLabel18).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs _)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				OnLabelLeft(o, Color.get_White(), _cyan);
			});
			((Control)dynamicLabel18).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await base.get_Presenter().CopyToClipboard(base.get_Presenter().GetServerAddress());
			});
			DynamicLabel dynamicLabel19 = new DynamicLabel(base.get_Presenter().GetShardId);
			((Control)dynamicLabel19).set_Parent((Container)(object)pnlInfo);
			((Control)dynamicLabel19).set_Width(((Container)pnlInfo).get_ContentRegion().Width);
			((Control)dynamicLabel19).set_Height(25);
			((Label)dynamicLabel19).set_Font(_font);
			((Label)dynamicLabel19).set_Text("Shard ID: ");
			((Label)dynamicLabel19).set_StrokeText(true);
			((Control)dynamicLabel19).add_MouseEntered((EventHandler<MouseEventArgs>)OnLabelEnter);
			((Control)dynamicLabel19).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs _)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				OnLabelLeft(o, Color.get_White());
			});
			((Control)dynamicLabel19).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await base.get_Presenter().CopyToClipboard(base.get_Presenter().GetShardId());
			});
			buildPanel.add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				((Control)flowContainer).set_Size(new Point(e.get_CurrentRegion().Width, e.get_CurrentRegion().Height));
			});
			((Container)flowContainer).add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_006b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0084: Unknown result type (might be due to invalid IL or missing references)
				//IL_00df: Unknown result type (might be due to invalid IL or missing references)
				((Control)pnlConfigs).set_Width(e.get_CurrentRegion().Width - 13);
				((Control)pnlAvatar).set_Width(e.get_CurrentRegion().Width - 13);
				((Control)pnlCamera).set_Width(e.get_CurrentRegion().Width - 13);
				((Control)pnlUserInterface).set_Width(e.get_CurrentRegion().Width - 13);
				((Control)pnlMap).set_Width(e.get_CurrentRegion().Width - 13);
				((Control)pnlInfo).set_Width(e.get_CurrentRegion().Width - 13);
				foreach (Control item in ((IEnumerable<Control>)((Container)flowContainer).get_Children()).Skip(1).SelectMany((Control x) => (IEnumerable<Control>)((Container)x).get_Children()))
				{
					item.set_Width(item.get_Parent().get_ContentRegion().Width);
				}
			});
		}

		private void OnLabelEnter(object sender, MouseEventArgs e)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			Label lbl = (Label)((sender is Label) ? sender : null);
			if (lbl != null)
			{
				lbl.set_TextColor(Color.get_LightBlue());
				DynamicLabel dynLbl = lbl as DynamicLabel;
				if (dynLbl != null)
				{
					dynLbl.TextDataColor = Color.get_LightBlue();
				}
			}
		}

		private void OnLabelLeft(object sender, Color textColor, Color textDataColor = default(Color))
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			Label lbl = (Label)((sender is Label) ? sender : null);
			if (lbl != null)
			{
				lbl.set_TextColor(textColor);
				DynamicLabel dynLbl = lbl as DynamicLabel;
				if (dynLbl != null)
				{
					dynLbl.TextDataColor = ((textDataColor == default(Color)) ? textColor : textDataColor);
				}
			}
		}
	}
}
