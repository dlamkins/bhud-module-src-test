using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;

namespace Eclipse1807.BlishHUD.FishingBuddy.Views
{
	public class SettingsView : View
	{
		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Expected O, but got Unknown
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Expected O, but got Unknown
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Expected O, but got Unknown
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Expected O, but got Unknown
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Expected O, but got Unknown
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
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
			IView settingFishDrag_View = SettingView.FromType((SettingEntry)(object)FishingBuddyModule._dragFishPanel, ((Control)buildPanel).get_Width());
			ViewContainer val5 = new ViewContainer();
			((Container)val5).set_WidthSizingMode((SizingMode)2);
			((Control)val5).set_Location(new Point(10, ((Control)settingFishCaught_Container).get_Bottom() + 5));
			((Control)val5).set_Parent((Container)(object)parentPanel);
			ViewContainer settingFishDrag_Container = val5;
			settingFishDrag_Container.Show(settingFishDrag_View);
			IView settingFishRarity_View = SettingView.FromType((SettingEntry)(object)FishingBuddyModule._showRarityBorder, ((Control)buildPanel).get_Width());
			ViewContainer val6 = new ViewContainer();
			((Container)val6).set_WidthSizingMode((SizingMode)2);
			((Control)val6).set_Location(new Point(160, ((Control)settingFishCaught_Container).get_Bottom() + 5));
			((Control)val6).set_Parent((Container)(object)parentPanel);
			val6.Show(settingFishRarity_View);
			IView settingFishUncatchable_View = SettingView.FromType((SettingEntry)(object)FishingBuddyModule._displayUncatchableFish, ((Control)buildPanel).get_Width());
			ViewContainer val7 = new ViewContainer();
			((Container)val7).set_WidthSizingMode((SizingMode)2);
			((Control)val7).set_Location(new Point(310, ((Control)settingFishCaught_Container).get_Bottom() + 5));
			((Control)val7).set_Parent((Container)(object)parentPanel);
			val7.Show(settingFishUncatchable_View);
			IView settingFishSize_View = SettingView.FromType((SettingEntry)(object)FishingBuddyModule._fishImgSize, ((Control)buildPanel).get_Width());
			ViewContainer val8 = new ViewContainer();
			((Container)val8).set_WidthSizingMode((SizingMode)2);
			((Control)val8).set_Location(new Point(10, ((Control)settingFishDrag_Container).get_Bottom() + 8));
			((Control)val8).set_Parent((Container)(object)parentPanel);
			ViewContainer settingFishSize_Container = val8;
			settingFishSize_Container.Show(settingFishSize_View);
			IView settingClockDrag_View = SettingView.FromType((SettingEntry)(object)FishingBuddyModule._dragTimeOfDayClock, ((Control)buildPanel).get_Width());
			ViewContainer val9 = new ViewContainer();
			((Container)val9).set_WidthSizingMode((SizingMode)2);
			((Control)val9).set_Location(new Point(10, ((Control)settingFishSize_Container).get_Bottom() + 5));
			((Control)val9).set_Parent((Container)(object)parentPanel);
			ViewContainer settingClock_Container = val9;
			settingClock_Container.Show(settingClockDrag_View);
			IView settingClockShow_View = SettingView.FromType((SettingEntry)(object)FishingBuddyModule._hideTimeOfDay, ((Control)buildPanel).get_Width());
			ViewContainer val10 = new ViewContainer();
			((Container)val10).set_WidthSizingMode((SizingMode)2);
			((Control)val10).set_Location(new Point(160, ((Control)settingFishSize_Container).get_Bottom() + 5));
			((Control)val10).set_Parent((Container)(object)parentPanel);
			val10.Show(settingClockShow_View);
			IView settingClockSize_View = SettingView.FromType((SettingEntry)(object)FishingBuddyModule._timeOfDayImgSize, ((Control)buildPanel).get_Width());
			ViewContainer val11 = new ViewContainer();
			((Container)val11).set_WidthSizingMode((SizingMode)2);
			((Control)val11).set_Location(new Point(10, ((Control)settingClock_Container).get_Bottom() + 5));
			((Control)val11).set_Parent((Container)(object)parentPanel);
			ViewContainer settingClockSize_Container = val11;
			settingClockSize_Container.Show(settingClockSize_View);
			IView settingCombat_View = SettingView.FromType((SettingEntry)(object)FishingBuddyModule._hideInCombat, ((Control)buildPanel).get_Width());
			ViewContainer val12 = new ViewContainer();
			((Container)val12).set_WidthSizingMode((SizingMode)2);
			((Control)val12).set_Location(new Point(10, ((Control)settingClockSize_Container).get_Bottom() + 5));
			((Control)val12).set_Parent((Container)(object)parentPanel);
			val12.Show(settingCombat_View);
		}

		public SettingsView()
			: this()
		{
		}
	}
}
