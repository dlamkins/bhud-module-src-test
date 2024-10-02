using System;
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

		private readonly Color _lemonGreen = new Color(84, 252, 84);

		private readonly Color _cyan = new Color(84, 252, 252);

		private readonly Color _red = new Color(252, 84, 84);

		public MumbleView()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
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
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Expected O, but got Unknown
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Expected O, but got Unknown
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Expected I4, but got Unknown
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0412: Unknown result type (might be due to invalid IL or missing references)
			//IL_0480: Unknown result type (might be due to invalid IL or missing references)
			//IL_0485: Unknown result type (might be due to invalid IL or missing references)
			//IL_0491: Unknown result type (might be due to invalid IL or missing references)
			//IL_049c: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ef: Expected O, but got Unknown
			//IL_0518: Unknown result type (might be due to invalid IL or missing references)
			//IL_0548: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0620: Unknown result type (might be due to invalid IL or missing references)
			//IL_0625: Unknown result type (might be due to invalid IL or missing references)
			//IL_0631: Unknown result type (might be due to invalid IL or missing references)
			//IL_063c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0643: Unknown result type (might be due to invalid IL or missing references)
			//IL_0652: Unknown result type (might be due to invalid IL or missing references)
			//IL_0659: Unknown result type (might be due to invalid IL or missing references)
			//IL_0664: Unknown result type (might be due to invalid IL or missing references)
			//IL_066e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0679: Unknown result type (might be due to invalid IL or missing references)
			//IL_0683: Unknown result type (might be due to invalid IL or missing references)
			//IL_068f: Expected O, but got Unknown
			//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0705: Unknown result type (might be due to invalid IL or missing references)
			//IL_072a: Unknown result type (might be due to invalid IL or missing references)
			//IL_072f: Unknown result type (might be due to invalid IL or missing references)
			//IL_073b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0746: Unknown result type (might be due to invalid IL or missing references)
			//IL_074d: Unknown result type (might be due to invalid IL or missing references)
			//IL_075c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0763: Unknown result type (might be due to invalid IL or missing references)
			//IL_076e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0778: Unknown result type (might be due to invalid IL or missing references)
			//IL_0783: Unknown result type (might be due to invalid IL or missing references)
			//IL_078d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0799: Expected O, but got Unknown
			//IL_07c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_081b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0847: Unknown result type (might be due to invalid IL or missing references)
			//IL_087b: Unknown result type (might be due to invalid IL or missing references)
			//IL_08a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0921: Unknown result type (might be due to invalid IL or missing references)
			//IL_0946: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a07: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a0c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a18: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a23: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a2a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a39: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a40: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a4b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a55: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a60: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a6a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a76: Expected O, but got Unknown
			//IL_0a76: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a7b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a87: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a8e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a9d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aa5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0abd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ac2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ace: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ad5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ae4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0af8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b46: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b93: Unknown result type (might be due to invalid IL or missing references)
			//IL_0be0: Unknown result type (might be due to invalid IL or missing references)
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
			((Control)val2).set_Width(((Container)flowContainer).get_ContentRegion().Width);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_FlowDirection((ControlFlowDirection)4);
			val2.set_OuterControlPadding(new Vector2(5f, 5f));
			FlowPanel pnlConfigs = val2;
			Checkbox val3 = new Checkbox();
			((Control)val3).set_Parent((Container)(object)pnlConfigs);
			val3.set_Text("Swap YZ");
			val3.set_Checked(((Presenter<MumbleView, MumbleConfig>)base.get_Presenter()).get_Model().SwapYZ);
			val3.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object _, CheckChangedEvent e)
			{
				((Presenter<MumbleView, MumbleConfig>)base.get_Presenter()).get_Model().SwapYZ = !e.get_Checked();
			});
			FlowPanel val4 = new FlowPanel();
			((Control)val4).set_Parent((Container)(object)flowContainer);
			((Panel)val4).set_Title("Avatar");
			((Control)val4).set_Width(((Container)flowContainer).get_ContentRegion().Width);
			((Container)val4).set_HeightSizingMode((SizingMode)1);
			val4.set_ControlPadding(new Vector2(5f, 2f));
			val4.set_OuterControlPadding(new Vector2(10f, 2f));
			((Panel)val4).set_Collapsed(false);
			FlowPanel pnlAvatar = val4;
			DynamicLabel dynamicLabel = new DynamicLabel(base.get_Presenter().GetRace);
			((Control)dynamicLabel).set_Parent((Container)(object)pnlAvatar);
			((Control)dynamicLabel).set_Width(((Container)pnlAvatar).get_ContentRegion().Width);
			((Control)dynamicLabel).set_Height(25);
			((Label)dynamicLabel).set_Font(_font);
			((Label)dynamicLabel).set_StrokeText(true);
			DynamicLabel dynamicLabel2 = new DynamicLabel(base.get_Presenter().GetPlayerProfession);
			((Control)dynamicLabel2).set_Parent((Container)(object)pnlAvatar);
			((Control)dynamicLabel2).set_Width(((Container)pnlAvatar).get_ContentRegion().Width);
			((Control)dynamicLabel2).set_Height(25);
			((Label)dynamicLabel2).set_Font(_font);
			dynamicLabel2.Icon = MumbleInfoModule.Instance.Api.GetClassIcon((int)GameService.Gw2Mumble.get_PlayerCharacter().get_Profession(), GameService.Gw2Mumble.get_PlayerCharacter().get_Specialization());
			DynamicLabel dynamicLabel3 = new DynamicLabel(base.get_Presenter().GetPlayerPosition);
			((Control)dynamicLabel3).set_Parent((Container)(object)pnlAvatar);
			((Control)dynamicLabel3).set_Width(((Container)pnlAvatar).get_ContentRegion().Width);
			((Control)dynamicLabel3).set_Height(25);
			((Label)dynamicLabel3).set_Font(_font);
			dynamicLabel3.Prefix = "Position: ";
			((Label)dynamicLabel3).set_TextColor(_lemonGreen);
			((Control)dynamicLabel3).add_MouseEntered((EventHandler<MouseEventArgs>)OnLabelEnter);
			((Control)dynamicLabel3).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs _)
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				OnLabelLeft(o, _lemonGreen);
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
			dynamicLabel4.Prefix = "Direction: ";
			((Label)dynamicLabel4).set_TextColor(_lemonGreen);
			((Control)dynamicLabel4).add_MouseEntered((EventHandler<MouseEventArgs>)OnLabelEnter);
			((Control)dynamicLabel4).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs _)
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				OnLabelLeft(o, _lemonGreen);
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
			((Control)dynamicLabel5).add_MouseEntered((EventHandler<MouseEventArgs>)OnLabelEnter);
			((Control)dynamicLabel5).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs _)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				OnLabelLeft(o, Color.get_White());
			});
			((Control)dynamicLabel5).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await base.get_Presenter().CopyToClipboard(MumbleInfoModule.Instance.Api.ClosestWaypoint.get_ChatLink());
			});
			DynamicLabel dynamicLabel6 = new DynamicLabel(base.get_Presenter().GetClosestPoi);
			((Control)dynamicLabel6).set_Parent((Container)(object)pnlAvatar);
			((Control)dynamicLabel6).set_Width(((Container)pnlAvatar).get_ContentRegion().Width);
			((Control)dynamicLabel6).set_Height(25);
			((Label)dynamicLabel6).set_Font(_font);
			dynamicLabel6.Icon = MumbleInfoModule.Instance.Api.PoiIcon;
			((Control)dynamicLabel6).add_MouseEntered((EventHandler<MouseEventArgs>)OnLabelEnter);
			((Control)dynamicLabel6).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs _)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				OnLabelLeft(o, Color.get_White());
			});
			((Control)dynamicLabel6).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await base.get_Presenter().CopyToClipboard(MumbleInfoModule.Instance.Api.ClosestPoi.get_ChatLink());
			});
			FlowPanel val5 = new FlowPanel();
			((Control)val5).set_Parent((Container)(object)flowContainer);
			((Panel)val5).set_Title("Camera");
			((Control)val5).set_Width(((Container)flowContainer).get_ContentRegion().Width);
			((Container)val5).set_HeightSizingMode((SizingMode)1);
			val5.set_ControlPadding(new Vector2(5f, 2f));
			val5.set_OuterControlPadding(new Vector2(10f, 2f));
			((Panel)val5).set_Collapsed(false);
			FlowPanel pnlCamera = val5;
			DynamicLabel dynamicLabel7 = new DynamicLabel(base.get_Presenter().GetCameraPosition);
			((Control)dynamicLabel7).set_Parent((Container)(object)pnlCamera);
			((Control)dynamicLabel7).set_Width(((Container)pnlCamera).get_ContentRegion().Width);
			((Control)dynamicLabel7).set_Height(25);
			((Label)dynamicLabel7).set_Font(_font);
			dynamicLabel7.Prefix = "Position: ";
			((Label)dynamicLabel7).set_TextColor(_red);
			((Control)dynamicLabel7).add_MouseEntered((EventHandler<MouseEventArgs>)OnLabelEnter);
			((Control)dynamicLabel7).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs _)
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				OnLabelLeft(o, _red);
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
			dynamicLabel8.Prefix = "Direction: ";
			((Label)dynamicLabel8).set_TextColor(_red);
			((Control)dynamicLabel8).add_MouseEntered((EventHandler<MouseEventArgs>)OnLabelEnter);
			((Control)dynamicLabel8).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs _)
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				OnLabelLeft(o, _red);
			});
			((Control)dynamicLabel8).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await base.get_Presenter().CopyToClipboard(base.get_Presenter().GetCameraDirection(markerPackFormat: true));
			});
			FlowPanel val6 = new FlowPanel();
			((Control)val6).set_Parent((Container)(object)flowContainer);
			((Panel)val6).set_Title("User Interface");
			((Control)val6).set_Width(((Container)flowContainer).get_ContentRegion().Width);
			((Container)val6).set_HeightSizingMode((SizingMode)1);
			val6.set_ControlPadding(new Vector2(5f, 2f));
			val6.set_OuterControlPadding(new Vector2(10f, 2f));
			((Panel)val6).set_Collapsed(false);
			FlowPanel pnlUserInterface = val6;
			DynamicLabel dynamicLabel9 = new DynamicLabel(base.get_Presenter().GetUiSize);
			((Control)dynamicLabel9).set_Parent((Container)(object)pnlUserInterface);
			((Control)dynamicLabel9).set_Width(((Container)pnlUserInterface).get_ContentRegion().Width);
			((Control)dynamicLabel9).set_Height(25);
			((Label)dynamicLabel9).set_Font(_font);
			DynamicLabel dynamicLabel10 = new DynamicLabel(base.get_Presenter().GetCompassBounds);
			((Control)dynamicLabel10).set_Parent((Container)(object)pnlUserInterface);
			((Control)dynamicLabel10).set_Width(((Container)pnlUserInterface).get_ContentRegion().Width);
			((Control)dynamicLabel10).set_Height(25);
			((Label)dynamicLabel10).set_Font(_font);
			FlowPanel val7 = new FlowPanel();
			((Control)val7).set_Parent((Container)(object)flowContainer);
			((Panel)val7).set_Title("Map");
			((Control)val7).set_Width(((Container)flowContainer).get_ContentRegion().Width);
			((Container)val7).set_HeightSizingMode((SizingMode)1);
			val7.set_ControlPadding(new Vector2(5f, 2f));
			val7.set_OuterControlPadding(new Vector2(10f, 2f));
			((Panel)val7).set_Collapsed(false);
			FlowPanel pnlMap = val7;
			DynamicLabel dynamicLabel11 = new DynamicLabel(base.get_Presenter().GetContinent);
			((Control)dynamicLabel11).set_Parent((Container)(object)pnlMap);
			((Control)dynamicLabel11).set_Width(((Container)pnlMap).get_ContentRegion().Width);
			((Control)dynamicLabel11).set_Height(25);
			((Label)dynamicLabel11).set_Font(_font);
			((Label)dynamicLabel11).set_TextColor(_cyan);
			DynamicLabel dynamicLabel12 = new DynamicLabel(base.get_Presenter().GetMap);
			((Control)dynamicLabel12).set_Parent((Container)(object)pnlMap);
			((Control)dynamicLabel12).set_Width(((Container)pnlMap).get_ContentRegion().Width);
			((Control)dynamicLabel12).set_Height(25);
			((Label)dynamicLabel12).set_Font(_font);
			((Label)dynamicLabel12).set_StrokeText(true);
			((Label)dynamicLabel12).set_TextColor(_cyan);
			DynamicLabel dynamicLabel13 = new DynamicLabel(base.get_Presenter().GetSector);
			((Control)dynamicLabel13).set_Parent((Container)(object)pnlMap);
			((Control)dynamicLabel13).set_Width(((Container)pnlMap).get_ContentRegion().Width);
			((Control)dynamicLabel13).set_Height(25);
			((Label)dynamicLabel13).set_Font(_font);
			((Label)dynamicLabel13).set_TextColor(_cyan);
			DynamicLabel dynamicLabel14 = new DynamicLabel(base.get_Presenter().GetMapType);
			((Control)dynamicLabel14).set_Parent((Container)(object)pnlMap);
			((Control)dynamicLabel14).set_Width(((Container)pnlMap).get_ContentRegion().Width);
			((Control)dynamicLabel14).set_Height(25);
			((Label)dynamicLabel14).set_Font(_font);
			DynamicLabel dynamicLabel15 = new DynamicLabel(base.get_Presenter().GetMapPosition);
			((Control)dynamicLabel15).set_Parent((Container)(object)pnlMap);
			((Control)dynamicLabel15).set_Width(((Container)pnlMap).get_ContentRegion().Width);
			((Control)dynamicLabel15).set_Height(25);
			((Label)dynamicLabel15).set_Font(_font);
			((Label)dynamicLabel15).set_TextColor(_red);
			((Control)dynamicLabel15).add_MouseEntered((EventHandler<MouseEventArgs>)OnLabelEnter);
			((Control)dynamicLabel15).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs _)
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				OnLabelLeft(o, _red);
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
			((Control)dynamicLabel16).add_MouseEntered((EventHandler<MouseEventArgs>)OnLabelEnter);
			((Control)dynamicLabel16).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs _)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				OnLabelLeft(o, Color.get_White());
			});
			((Control)dynamicLabel16).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await base.get_Presenter().CopyToClipboard(base.get_Presenter().GetMapHash(discordRichPresenceFormat: true));
			});
			FlowPanel val8 = new FlowPanel();
			((Control)val8).set_Parent((Container)(object)flowContainer);
			((Panel)val8).set_Title("Info");
			((Control)val8).set_Width(((Container)flowContainer).get_ContentRegion().Width);
			((Container)val8).set_HeightSizingMode((SizingMode)1);
			val8.set_ControlPadding(new Vector2(5f, 2f));
			val8.set_OuterControlPadding(new Vector2(10f, 2f));
			((Panel)val8).set_Collapsed(false);
			FlowPanel pnlInfo = val8;
			Label val9 = new Label();
			((Control)val9).set_Parent((Container)(object)pnlInfo);
			((Control)val9).set_Width(((Container)pnlInfo).get_ContentRegion().Width);
			((Control)val9).set_Height(25);
			val9.set_Font(_font);
			val9.set_Text(BlishUtil.GetVersion());
			Label val10 = new Label();
			((Control)val10).set_Parent((Container)(object)pnlInfo);
			((Control)val10).set_Width(((Container)pnlInfo).get_ContentRegion().Width);
			((Control)val10).set_Height(25);
			val10.set_Font(_font);
			val10.set_Text($"Mumble Link v{GameService.Gw2Mumble.get_Info().get_Version()}");
			DynamicLabel dynamicLabel17 = new DynamicLabel(base.get_Presenter().GetProcessId);
			((Control)dynamicLabel17).set_Parent((Container)(object)pnlInfo);
			((Control)dynamicLabel17).set_Width(((Container)pnlInfo).get_ContentRegion().Width);
			((Control)dynamicLabel17).set_Height(25);
			((Label)dynamicLabel17).set_Font(_font);
			DynamicLabel dynamicLabel18 = new DynamicLabel(base.get_Presenter().GetServerAddress);
			((Control)dynamicLabel18).set_Parent((Container)(object)pnlInfo);
			((Control)dynamicLabel18).set_Width(((Container)pnlInfo).get_ContentRegion().Width);
			((Control)dynamicLabel18).set_Height(25);
			((Label)dynamicLabel18).set_Font(_font);
			DynamicLabel dynamicLabel19 = new DynamicLabel(base.get_Presenter().GetShardId);
			((Control)dynamicLabel19).set_Parent((Container)(object)pnlInfo);
			((Control)dynamicLabel19).set_Width(((Container)pnlInfo).get_ContentRegion().Width);
			((Control)dynamicLabel19).set_Height(25);
			((Label)dynamicLabel19).set_Font(_font);
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
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Unknown result type (might be due to invalid IL or missing references)
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				((Control)pnlAvatar).set_Width(e.get_CurrentRegion().Width);
				((Control)pnlCamera).set_Width(e.get_CurrentRegion().Width);
				((Control)pnlUserInterface).set_Width(e.get_CurrentRegion().Width);
				((Control)pnlMap).set_Width(e.get_CurrentRegion().Width);
				((Control)pnlInfo).set_Width(e.get_CurrentRegion().Width);
			});
		}

		private void OnLabelEnter(object sender, MouseEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Label)sender).set_TextColor(Color.get_LightBlue());
		}

		private void OnLabelLeft(object sender, Color color)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Label)sender).set_TextColor(color);
		}
	}
}
