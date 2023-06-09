using System;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Eclipse1807.BlishHUD.FishingBuddy.Properties;
using Microsoft.Xna.Framework;

namespace Eclipse1807.BlishHUD.FishingBuddy.Views
{
	public class SettingsView : View
	{
		protected override void Build(Container buildPanel)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Expected O, but got Unknown
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Expected O, but got Unknown
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Expected O, but got Unknown
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_030d: Expected O, but got Unknown
			//IL_030e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_0330: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			//IL_0344: Expected O, but got Unknown
			//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ef: Expected O, but got Unknown
			//IL_040a: Unknown result type (might be due to invalid IL or missing references)
			//IL_040f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0416: Unknown result type (might be due to invalid IL or missing references)
			//IL_0422: Unknown result type (might be due to invalid IL or missing references)
			//IL_042c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0435: Expected O, but got Unknown
			//IL_0450: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			//IL_045c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0468: Unknown result type (might be due to invalid IL or missing references)
			//IL_0472: Unknown result type (might be due to invalid IL or missing references)
			//IL_047b: Expected O, but got Unknown
			//IL_0496: Unknown result type (might be due to invalid IL or missing references)
			//IL_049b: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c1: Expected O, but got Unknown
			//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0507: Expected O, but got Unknown
			//IL_0522: Unknown result type (might be due to invalid IL or missing references)
			//IL_0527: Unknown result type (might be due to invalid IL or missing references)
			//IL_052e: Unknown result type (might be due to invalid IL or missing references)
			//IL_053b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0545: Unknown result type (might be due to invalid IL or missing references)
			//IL_0565: Unknown result type (might be due to invalid IL or missing references)
			//IL_056a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0571: Unknown result type (might be due to invalid IL or missing references)
			//IL_057e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0588: Unknown result type (might be due to invalid IL or missing references)
			//IL_0596: Unknown result type (might be due to invalid IL or missing references)
			//IL_059b: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e6: Expected O, but got Unknown
			//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0609: Unknown result type (might be due to invalid IL or missing references)
			//IL_0611: Unknown result type (might be due to invalid IL or missing references)
			//IL_061d: Expected O, but got Unknown
			//IL_068e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0693: Unknown result type (might be due to invalid IL or missing references)
			//IL_069a: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b9: Expected O, but got Unknown
			//IL_06d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Height(((Control)buildPanel).get_Height());
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			Panel parentPanel = val;
			IView settingFishCaught_View = SettingView.FromType((SettingEntry)(object)FishingBuddyModule._ignoreCaughtFish, ((Control)buildPanel).get_Width());
			ViewContainer val2 = new ViewContainer();
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Control)val2).set_Location(new Point(10, 10));
			((Control)val2).set_Parent((Container)(object)parentPanel);
			ViewContainer settingFishCaught_Container = val2;
			settingFishCaught_Container.Show(settingFishCaught_View);
			IView settingFishWorldClass_View = SettingView.FromType((SettingEntry)(object)FishingBuddyModule._includeWorldClass, ((Control)buildPanel).get_Width());
			ViewContainer val3 = new ViewContainer();
			((Container)val3).set_WidthSizingMode((SizingMode)2);
			((Control)val3).set_Location(new Point(160, ((Control)settingFishCaught_Container).get_Location().Y));
			((Control)val3).set_Parent((Container)(object)parentPanel);
			val3.Show(settingFishWorldClass_View);
			IView settingFishSaltwater_View = SettingView.FromType((SettingEntry)(object)FishingBuddyModule._includeSaltwater, ((Control)buildPanel).get_Width());
			ViewContainer val4 = new ViewContainer();
			((Container)val4).set_WidthSizingMode((SizingMode)2);
			((Control)val4).set_Location(new Point(310, ((Control)settingFishCaught_Container).get_Location().Y));
			((Control)val4).set_Parent((Container)(object)parentPanel);
			val4.Show(settingFishSaltwater_View);
			Label val5 = new Label();
			((Control)val5).set_Location(new Point(470, ((Control)settingFishCaught_Container).get_Location().Y));
			((Control)val5).set_Width(75);
			val5.set_AutoSizeHeight(false);
			val5.set_WrapText(false);
			((Control)val5).set_Parent((Container)(object)parentPanel);
			val5.set_Text(Strings.Orientation + ": ");
			Label settingFishPanelOrientation_Label = val5;
			Dropdown val6 = new Dropdown();
			((Control)val6).set_Location(new Point(((Control)settingFishPanelOrientation_Label).get_Right() + 8, ((Control)settingFishPanelOrientation_Label).get_Top() - 4));
			((Control)val6).set_Width(100);
			((Control)val6).set_Parent((Container)(object)parentPanel);
			Dropdown settingFishPanelOrientation_Dropdown = val6;
			string[] fishPanelOrientations = FishingBuddyModule._fishPanelOrientations;
			foreach (string s in fishPanelOrientations)
			{
				settingFishPanelOrientation_Dropdown.get_Items().Add(Strings.ResourceManager.GetString(s, Strings.Culture));
			}
			settingFishPanelOrientation_Dropdown.set_SelectedItem(FishingBuddyModule._fishPanelOrientation.get_Value());
			settingFishPanelOrientation_Dropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				FishingBuddyModule._fishPanelOrientation.set_Value(settingFishPanelOrientation_Dropdown.get_SelectedItem());
			});
			IView settingFishDrag_View = SettingView.FromType((SettingEntry)(object)FishingBuddyModule._dragFishPanel, ((Control)buildPanel).get_Width());
			ViewContainer val7 = new ViewContainer();
			((Container)val7).set_WidthSizingMode((SizingMode)2);
			((Control)val7).set_Location(new Point(10, ((Control)settingFishCaught_Container).get_Bottom() + 5));
			((Control)val7).set_Parent((Container)(object)parentPanel);
			ViewContainer settingFishDrag_Container = val7;
			settingFishDrag_Container.Show(settingFishDrag_View);
			IView settingFishRarity_View = SettingView.FromType((SettingEntry)(object)FishingBuddyModule._showRarityBorder, ((Control)buildPanel).get_Width());
			ViewContainer val8 = new ViewContainer();
			((Container)val8).set_WidthSizingMode((SizingMode)2);
			((Control)val8).set_Location(new Point(160, ((Control)settingFishCaught_Container).get_Bottom() + 5));
			((Control)val8).set_Parent((Container)(object)parentPanel);
			val8.Show(settingFishRarity_View);
			IView settingFishUncatchable_View = SettingView.FromType((SettingEntry)(object)FishingBuddyModule._displayUncatchableFish, ((Control)buildPanel).get_Width());
			ViewContainer val9 = new ViewContainer();
			((Container)val9).set_WidthSizingMode((SizingMode)2);
			((Control)val9).set_Location(new Point(310, ((Control)settingFishCaught_Container).get_Bottom() + 5));
			((Control)val9).set_Parent((Container)(object)parentPanel);
			val9.Show(settingFishUncatchable_View);
			Label val10 = new Label();
			((Control)val10).set_Location(new Point(470, ((Control)settingFishCaught_Container).get_Bottom() + 5));
			((Control)val10).set_Width(75);
			val10.set_AutoSizeHeight(false);
			val10.set_WrapText(false);
			((Control)val10).set_Parent((Container)(object)parentPanel);
			val10.set_Text(Strings.Direction + ": ");
			Label settingFishPanelDirection_Label = val10;
			Dropdown val11 = new Dropdown();
			((Control)val11).set_Location(new Point(((Control)settingFishPanelDirection_Label).get_Right() + 8, ((Control)settingFishPanelDirection_Label).get_Top() - 4));
			((Control)val11).set_Width(100);
			((Control)val11).set_Parent((Container)(object)parentPanel);
			Dropdown settingFishPanelDirection_Dropdown = val11;
			fishPanelOrientations = FishingBuddyModule._fishPanelDirections;
			foreach (string s2 in fishPanelOrientations)
			{
				settingFishPanelDirection_Dropdown.get_Items().Add(Strings.ResourceManager.GetString(s2, Strings.Culture));
			}
			settingFishPanelDirection_Dropdown.set_SelectedItem(FishingBuddyModule._fishPanelDirection.get_Value());
			settingFishPanelDirection_Dropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				FishingBuddyModule._fishPanelDirection.set_Value(settingFishPanelDirection_Dropdown.get_SelectedItem());
			});
			IView settingFishSize_View = SettingView.FromType((SettingEntry)(object)FishingBuddyModule._fishImgSize, ((Control)buildPanel).get_Width());
			ViewContainer val12 = new ViewContainer();
			((Container)val12).set_WidthSizingMode((SizingMode)2);
			((Control)val12).set_Location(new Point(10, ((Control)settingFishDrag_Container).get_Bottom() + 8));
			((Control)val12).set_Parent((Container)(object)parentPanel);
			ViewContainer settingFishSize_Container = val12;
			settingFishSize_Container.Show(settingFishSize_View);
			IView settingFishTooltip_View = SettingView.FromType((SettingEntry)(object)FishingBuddyModule._fishPanelTooltipDisplay, ((Control)buildPanel).get_Width());
			ViewContainer val13 = new ViewContainer();
			((Container)val13).set_WidthSizingMode((SizingMode)2);
			((Control)val13).set_Location(new Point(10, ((Control)settingFishSize_Container).get_Bottom() + 5));
			((Control)val13).set_Parent((Container)(object)parentPanel);
			ViewContainer settingFishTooltip_Container = val13;
			settingFishTooltip_Container.Show(settingFishTooltip_View);
			IView settingBaitDrag_View = SettingView.FromType((SettingEntry)(object)FishingBuddyModule._dragBaitPanel, ((Control)buildPanel).get_Width());
			ViewContainer val14 = new ViewContainer();
			((Container)val14).set_WidthSizingMode((SizingMode)2);
			((Control)val14).set_Location(new Point(10, ((Control)settingFishTooltip_Container).get_Bottom() + 5));
			((Control)val14).set_Parent((Container)(object)parentPanel);
			ViewContainer settingBaitDrag_Container = val14;
			settingBaitDrag_Container.Show(settingBaitDrag_View);
			IView settingBaitSize_View = SettingView.FromType((SettingEntry)(object)FishingBuddyModule._baitImgSize, ((Control)buildPanel).get_Width());
			ViewContainer val15 = new ViewContainer();
			((Container)val15).set_WidthSizingMode((SizingMode)0);
			((Control)val15).set_Location(new Point(10, ((Control)settingBaitDrag_Container).get_Bottom() + 8));
			((Control)val15).set_Parent((Container)(object)parentPanel);
			ViewContainer settingBaitSize_Container = val15;
			settingBaitSize_Container.Show(settingBaitSize_View);
			IView settingClockDrag_View = SettingView.FromType((SettingEntry)(object)FishingBuddyModule._dragTimeOfDayClock, ((Control)buildPanel).get_Width());
			ViewContainer val16 = new ViewContainer();
			((Container)val16).set_WidthSizingMode((SizingMode)2);
			((Control)val16).set_Location(new Point(10, ((Control)settingBaitSize_Container).get_Bottom() + 5));
			((Control)val16).set_Parent((Container)(object)parentPanel);
			ViewContainer settingClock_Container = val16;
			settingClock_Container.Show(settingClockDrag_View);
			IView settingClockShow_View = SettingView.FromType((SettingEntry)(object)FishingBuddyModule._hideTimeOfDay, ((Control)buildPanel).get_Width());
			ViewContainer val17 = new ViewContainer();
			((Container)val17).set_WidthSizingMode((SizingMode)2);
			((Control)val17).set_Location(new Point(160, ((Control)settingClock_Container).get_Top()));
			((Control)val17).set_Parent((Container)(object)parentPanel);
			val17.Show(settingClockShow_View);
			IView settingTimeLabel_View = SettingView.FromType((SettingEntry)(object)FishingBuddyModule._settingClockLabel, ((Control)buildPanel).get_Width());
			ViewContainer val18 = new ViewContainer();
			((Container)val18).set_WidthSizingMode((SizingMode)2);
			((Control)val18).set_Location(new Point(310, ((Control)settingClock_Container).get_Top()));
			((Control)val18).set_Parent((Container)(object)parentPanel);
			val18.Show(settingTimeLabel_View);
			Label val19 = new Label();
			((Control)val19).set_Location(new Point(470, ((Control)settingClock_Container).get_Top()));
			((Control)val19).set_Width(75);
			val19.set_AutoSizeHeight(false);
			val19.set_WrapText(false);
			((Control)val19).set_Parent((Container)(object)parentPanel);
			val19.set_Text(Strings.LabelAlign + ": ");
			Label settingTimeLabelAlign_Label = val19;
			Dropdown val20 = new Dropdown();
			((Control)val20).set_Location(new Point(((Control)settingTimeLabelAlign_Label).get_Right() + 8, ((Control)settingTimeLabelAlign_Label).get_Top() - 4));
			((Control)val20).set_Width(100);
			((Control)val20).set_Parent((Container)(object)parentPanel);
			Dropdown settingimeLabelAlign_Dropdown = val20;
			fishPanelOrientations = FishingBuddyModule._verticalAlignmentOptions;
			foreach (string s3 in fishPanelOrientations)
			{
				settingimeLabelAlign_Dropdown.get_Items().Add(s3);
			}
			settingimeLabelAlign_Dropdown.set_SelectedItem(FishingBuddyModule._settingClockAlign.get_Value());
			settingimeLabelAlign_Dropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				FishingBuddyModule._settingClockAlign.set_Value(settingimeLabelAlign_Dropdown.get_SelectedItem());
			});
			IView settingClockSize_View = SettingView.FromType((SettingEntry)(object)FishingBuddyModule._timeOfDayImgSize, ((Control)buildPanel).get_Width());
			ViewContainer val21 = new ViewContainer();
			((Container)val21).set_WidthSizingMode((SizingMode)2);
			((Control)val21).set_Location(new Point(10, ((Control)settingClock_Container).get_Bottom() + 8));
			((Control)val21).set_Parent((Container)(object)parentPanel);
			ViewContainer settingClockSize_Container = val21;
			settingClockSize_Container.Show(settingClockSize_View);
			IView settingCombat_View = SettingView.FromType((SettingEntry)(object)FishingBuddyModule._hideInCombat, ((Control)buildPanel).get_Width());
			ViewContainer val22 = new ViewContainer();
			((Container)val22).set_WidthSizingMode((SizingMode)2);
			((Control)val22).set_Location(new Point(10, ((Control)settingClockSize_Container).get_Bottom() + 5));
			((Control)val22).set_Parent((Container)(object)parentPanel);
			val22.Show(settingCombat_View);
		}

		public SettingsView()
			: this()
		{
		}
	}
}
