using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using Oberyn.AnglerAssociate.Models;
using Oberyn.AnglerAssociate.Services;

namespace Oberyn.AnglerAssociate.Controls
{
	public class MainView : Panel
	{
		private static readonly Dictionary<Cycle, Region[]> RegionsByCycle = new Dictionary<Cycle, Region[]>
		{
			{
				Cycle.Tyria,
				new Region[4]
				{
					Region.Tyria,
					Region.CrystalDesert,
					Region.HornOfMaguuma,
					Region.Janthir
				}
			},
			{
				Cycle.CanthaCastora,
				new Region[2]
				{
					Region.Cantha,
					Region.Castora
				}
			},
			{
				Cycle.Global,
				new Region[1]
			}
		};

		private readonly ContentsManager _contentsManager;

		private readonly AchievementProgressService _achievementProgress;

		private readonly Label _dailyLabel;

		private readonly DayNightBanner _tyriaBanner;

		private readonly DayNightBanner _canthaBanner;

		private readonly Dropdown _continentDropdown;

		private readonly Dropdown _regionDropdown;

		private readonly Dropdown _achievementDropdown;

		private readonly Dropdown _baitDropdown;

		private readonly Dropdown _availableNowDropdown;

		private readonly Dropdown _hideCollectedDropdown;

		private readonly TextBox _searchBox;

		private readonly Panel _tableRows;

		private readonly List<Panel> _rowControls = new List<Panel>();

		private readonly Dictionary<string, Region> _regionDisplayToValue = new Dictionary<string, Region>();

		private readonly Dictionary<string, Bait> _baitDisplayToValue = new Dictionary<string, Bait>();

		private const int RowHeight = 40;

		private const int TableTop = 25;

		private const double PixelsPerChar = 7.0;

		public MainView(ContentsManager contentsManager, AchievementProgressService achievementProgress, int contentHeight)
			: this()
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected O, but got Unknown
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Expected O, but got Unknown
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Expected O, but got Unknown
			//IL_032b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0330: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0344: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Expected O, but got Unknown
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0385: Unknown result type (might be due to invalid IL or missing references)
			//IL_0395: Expected O, but got Unknown
			//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0400: Unknown result type (might be due to invalid IL or missing references)
			//IL_0403: Unknown result type (might be due to invalid IL or missing references)
			//IL_040d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0418: Unknown result type (might be due to invalid IL or missing references)
			//IL_0421: Unknown result type (might be due to invalid IL or missing references)
			//IL_0426: Unknown result type (might be due to invalid IL or missing references)
			//IL_042d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0438: Unknown result type (might be due to invalid IL or missing references)
			//IL_043b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0445: Unknown result type (might be due to invalid IL or missing references)
			//IL_0450: Unknown result type (might be due to invalid IL or missing references)
			//IL_045f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0464: Unknown result type (might be due to invalid IL or missing references)
			//IL_046b: Unknown result type (might be due to invalid IL or missing references)
			//IL_046e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0478: Unknown result type (might be due to invalid IL or missing references)
			//IL_0488: Expected O, but got Unknown
			//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_04af: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c9: Expected O, but got Unknown
			//IL_050b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0510: Unknown result type (might be due to invalid IL or missing references)
			//IL_0517: Unknown result type (might be due to invalid IL or missing references)
			//IL_051a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0524: Unknown result type (might be due to invalid IL or missing references)
			//IL_0534: Expected O, but got Unknown
			//IL_0587: Unknown result type (might be due to invalid IL or missing references)
			//IL_058c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0593: Unknown result type (might be due to invalid IL or missing references)
			//IL_059d: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d4: Expected O, but got Unknown
			_contentsManager = contentsManager;
			_achievementProgress = achievementProgress;
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Today's daily: " + DailyFisherRotation.GetToday());
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Width(300);
			((Control)val).set_Height(24);
			_dailyLabel = val;
			DayNightBanner dayNightBanner = new DayNightBanner(contentsManager, "tyria", "Tyria", Cycle.Tyria);
			((Control)dayNightBanner).set_Parent((Container)(object)this);
			((Control)dayNightBanner).set_Location(new Point(0, 40));
			_tyriaBanner = dayNightBanner;
			DayNightBanner dayNightBanner2 = new DayNightBanner(contentsManager, "cantha", "Cantha/Castora", Cycle.CanthaCastora);
			((Control)dayNightBanner2).set_Parent((Container)(object)this);
			((Control)dayNightBanner2).set_Location(new Point(130, 40));
			_canthaBanner = dayNightBanner2;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("Search fish");
			((Control)val2).set_Location(new Point(0, 460));
			((Control)val2).set_Width(250);
			((Control)val2).set_Height(20);
			TextBox val3 = new TextBox();
			((Control)val3).set_Parent((Container)(object)this);
			((TextInputBase)val3).set_PlaceholderText("Fish name...");
			((Control)val3).set_Location(new Point(0, 480));
			((Control)val3).set_Width(225);
			((Control)val3).set_Height(24);
			_searchBox = val3;
			((TextInputBase)_searchBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				RebuildRows();
			});
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text("X");
			((Control)val4).set_Location(new Point(228, 480));
			((Control)val4).set_Width(22);
			((Control)val4).set_Height(24);
			val4.set_HorizontalAlignment((HorizontalAlignment)1);
			val4.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val4).set_BasicTooltipText("Clear search");
			((Control)val4).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				((TextInputBase)_searchBox).set_Text(string.Empty);
				RebuildRows();
			});
			int col1 = 340;
			int col2 = 560;
			int col3 = 780;
			int y = 0;
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text("Filter by continent");
			((Control)val5).set_Location(new Point(col1, y));
			((Control)val5).set_Width(200);
			((Control)val5).set_Height(20);
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text("Filter by region");
			((Control)val6).set_Location(new Point(col2, y));
			((Control)val6).set_Width(200);
			((Control)val6).set_Height(20);
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)this);
			val7.set_Text("Filter by bait");
			((Control)val7).set_Location(new Point(col3, y));
			((Control)val7).set_Width(200);
			((Control)val7).set_Height(20);
			y += 20;
			Dropdown val8 = new Dropdown();
			((Control)val8).set_Parent((Container)(object)this);
			((Control)val8).set_Location(new Point(col1, y));
			((Control)val8).set_Width(200);
			_continentDropdown = val8;
			foreach (Cycle cycle in RegionsByCycle.Keys)
			{
				_continentDropdown.get_Items().Add(CycleLabel(cycle));
			}
			_continentDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				RebuildRegionDropdown();
				RebuildAchievementDropdown();
				RebuildBaitDropdown();
				RebuildRows();
			});
			Dropdown val9 = new Dropdown();
			((Control)val9).set_Parent((Container)(object)this);
			((Control)val9).set_Location(new Point(col2, y));
			((Control)val9).set_Width(200);
			_regionDropdown = val9;
			_regionDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				RebuildAchievementDropdown();
				RebuildBaitDropdown();
				RebuildRows();
			});
			Dropdown val10 = new Dropdown();
			((Control)val10).set_Parent((Container)(object)this);
			((Control)val10).set_Location(new Point(col3, y));
			((Control)val10).set_Width(200);
			_baitDropdown = val10;
			_baitDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				RebuildRows();
			});
			y += 30;
			Label val11 = new Label();
			((Control)val11).set_Parent((Container)(object)this);
			val11.set_Text("Filter by achievement");
			((Control)val11).set_Location(new Point(col1, y));
			((Control)val11).set_Width(200);
			((Control)val11).set_Height(20);
			Label val12 = new Label();
			((Control)val12).set_Parent((Container)(object)this);
			val12.set_Text("Hide collected");
			((Control)val12).set_Location(new Point(col2, y));
			((Control)val12).set_Width(200);
			((Control)val12).set_Height(20);
			Label val13 = new Label();
			((Control)val13).set_Parent((Container)(object)this);
			val13.set_Text("Show only available now");
			((Control)val13).set_Location(new Point(col3, y));
			((Control)val13).set_Width(200);
			((Control)val13).set_Height(20);
			y += 20;
			Dropdown val14 = new Dropdown();
			((Control)val14).set_Parent((Container)(object)this);
			((Control)val14).set_Location(new Point(col1, y));
			((Control)val14).set_Width(200);
			_achievementDropdown = val14;
			_achievementDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				RebuildRows();
			});
			Dropdown val15 = new Dropdown();
			((Control)val15).set_Parent((Container)(object)this);
			((Control)val15).set_Location(new Point(col2, y));
			((Control)val15).set_Width(200);
			_hideCollectedDropdown = val15;
			_hideCollectedDropdown.get_Items().Add("No");
			_hideCollectedDropdown.get_Items().Add("Yes");
			_hideCollectedDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				RebuildRows();
			});
			Dropdown val16 = new Dropdown();
			((Control)val16).set_Parent((Container)(object)this);
			((Control)val16).set_Location(new Point(col3, y));
			((Control)val16).set_Width(200);
			_availableNowDropdown = val16;
			_availableNowDropdown.get_Items().Add("No");
			_availableNowDropdown.get_Items().Add("Yes");
			_availableNowDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				RebuildRows();
			});
			y += 40;
			BuildTableHeader(340, y);
			Panel val17 = new Panel();
			((Control)val17).set_Parent((Container)(object)this);
			((Control)val17).set_Location(new Point(340, y + 25));
			((Control)val17).set_Width(712);
			((Control)val17).set_Height(Math.Max(contentHeight - (y + 25) - 10, 100));
			val17.set_CanScroll(true);
			_tableRows = val17;
			RebuildRegionDropdown();
			RebuildAchievementDropdown();
			RebuildBaitDropdown();
			RebuildRows();
		}

		public void RefreshBanners()
		{
			_tyriaBanner.Refresh();
			_canthaBanner.Refresh();
		}

		private static string CycleLabel(Cycle cycle)
		{
			return cycle switch
			{
				Cycle.Tyria => "Tyria", 
				Cycle.CanthaCastora => "Cantha/Castora", 
				Cycle.Global => "Global", 
				_ => cycle.ToString(), 
			};
		}

		private void BuildTableHeader(int x, int y)
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			string[] headers = new string[6] { "Fish", "Found in", "Rarity", "Hole", "Bait", "Time" };
			int[] widths = new int[6] { 150, 170, 60, 130, 80, 80 };
			int cx = x;
			BitmapFont headerFont = GameService.Content.GetFont((FontFace)0, (FontSize)16, (FontStyle)2);
			for (int i = 0; i < headers.Length; i++)
			{
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)this);
				val.set_Text(headers[i]);
				val.set_Font(headerFont);
				((Control)val).set_Location(new Point(cx, y));
				((Control)val).set_Width(widths[i]);
				((Control)val).set_Height(24);
				val.set_HorizontalAlignment((HorizontalAlignment)((i != 0) ? 1 : 0));
				cx += widths[i];
			}
		}

		private Cycle SelectedCycle()
		{
			int index = ((_continentDropdown.get_SelectedItem() != null) ? _continentDropdown.get_Items().IndexOf(_continentDropdown.get_SelectedItem()) : 0);
			return RegionsByCycle.Keys.ElementAt(Math.Max(index, 0));
		}

		private void RebuildRegionDropdown()
		{
			Cycle cycle = SelectedCycle();
			_regionDropdown.get_Items().Clear();
			_regionDisplayToValue.Clear();
			Region[] array = RegionsByCycle[cycle];
			foreach (Region region in array)
			{
				string display = EnumDisplay.Format(region);
				_regionDropdown.get_Items().Add(display);
				_regionDisplayToValue[display] = region;
			}
			if (_regionDropdown.get_Items().Count > 0)
			{
				_regionDropdown.set_SelectedItem(_regionDropdown.get_Items()[0]);
			}
		}

		private Region? SelectedRegion()
		{
			if (_regionDropdown.get_SelectedItem() == null)
			{
				return null;
			}
			if (!_regionDisplayToValue.TryGetValue(_regionDropdown.get_SelectedItem(), out var region))
			{
				return null;
			}
			return region;
		}

		private void RebuildAchievementDropdown()
		{
			_achievementDropdown.get_Items().Clear();
			_achievementDropdown.get_Items().Add("All");
			Region? region = SelectedRegion();
			if (!region.HasValue)
			{
				return;
			}
			foreach (string name2 in from name in (from f in FishCatalog.All
					where f.Region == region.Value && f.Collection != null
					select f.Collection).Distinct()
				orderby name
				select name)
			{
				_achievementDropdown.get_Items().Add(name2);
			}
			_achievementDropdown.set_SelectedItem("All");
		}

		private void RebuildBaitDropdown()
		{
			_baitDropdown.get_Items().Clear();
			_baitDisplayToValue.Clear();
			_baitDropdown.get_Items().Add("All");
			Region? region = SelectedRegion();
			if (!region.HasValue)
			{
				return;
			}
			foreach (Bait bait in from b in (from f in FishCatalog.All
					where f.Region == region.Value
					select f.Bait).Distinct()
				orderby b.ToString()
				select b)
			{
				string display = EnumDisplay.Format(bait);
				_baitDropdown.get_Items().Add(display);
				_baitDisplayToValue[display] = bait;
			}
			_baitDropdown.set_SelectedItem("All");
		}

		private void RebuildRows()
		{
			foreach (Panel rowControl in _rowControls)
			{
				((Control)rowControl).Dispose();
			}
			_rowControls.Clear();
			bool onlyAvailableNow = _availableNowDropdown.get_SelectedItem() == "Yes";
			bool hideCollected = _hideCollectedDropdown.get_SelectedItem() == "Yes";
			string searchText = ((TextInputBase)_searchBox).get_Text()?.Trim();
			IEnumerable<Fish> fish;
			if (!string.IsNullOrEmpty(searchText))
			{
				fish = FishCatalog.All.Where((Fish f) => f.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0);
			}
			else
			{
				Region? region = SelectedRegion();
				if (!region.HasValue)
				{
					return;
				}
				string achievementFilter = _achievementDropdown.get_SelectedItem();
				string baitFilter = _baitDropdown.get_SelectedItem();
				fish = FishCatalog.All.Where((Fish f) => f.Region == region.Value);
				if (achievementFilter != null && achievementFilter != "All")
				{
					fish = fish.Where((Fish f) => f.Collection == achievementFilter);
				}
				if (baitFilter != null && baitFilter != "All" && _baitDisplayToValue.TryGetValue(baitFilter, out var baitValue))
				{
					fish = fish.Where((Fish f) => f.Bait == baitValue);
				}
				if (onlyAvailableNow)
				{
					fish = fish.Where(IsAvailableNow);
				}
				if (hideCollected)
				{
					fish = fish.Where((Fish f) => !_achievementProgress.IsFishCaught(f));
				}
			}
			int rowY = 0;
			foreach (Fish f2 in fish)
			{
				Panel row = BuildRow(f2, rowY);
				_rowControls.Add(row);
				rowY += 40;
			}
		}

		private static bool IsAvailableNow(Fish fish)
		{
			if (fish.Cycle == Cycle.Global)
			{
				return fish.TimeOfDay == TimeOfDay.Any;
			}
			TimeOfDay state = TyrianClock.GetState(fish.Cycle).State;
			return fish.IsCatchableAt(state);
		}

		private Panel BuildRow(Fish fish, int y)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)_tableRows);
			((Control)val).set_Location(new Point(0, y));
			((Control)val).set_Width(670);
			((Control)val).set_Height(40);
			Panel row = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)row);
			val2.set_Text(fish.Name);
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Width(150);
			((Control)val2).set_Height(40);
			val2.set_VerticalAlignment((VerticalAlignment)1);
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)row);
			val3.set_Text(FormatFoundInShort(fish));
			((Control)val3).set_BasicTooltipText(FormatFoundInFull(fish));
			((Control)val3).set_Location(new Point(150, 0));
			((Control)val3).set_Width(170);
			((Control)val3).set_Height(40);
			val3.set_VerticalAlignment((VerticalAlignment)1);
			val3.set_HorizontalAlignment((HorizontalAlignment)1);
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)row);
			val4.set_Text(fish.Rarity.ToString());
			val4.set_TextColor(RarityColors.Get(fish.Rarity));
			((Control)val4).set_Location(new Point(320, 0));
			((Control)val4).set_Width(60);
			((Control)val4).set_Height(40);
			val4.set_VerticalAlignment((VerticalAlignment)1);
			val4.set_HorizontalAlignment((HorizontalAlignment)1);
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)row);
			val5.set_Text(FormatHolesShort(fish));
			((Control)val5).set_BasicTooltipText(FormatHolesFull(fish));
			((Control)val5).set_Location(new Point(380, 0));
			((Control)val5).set_Width(130);
			((Control)val5).set_Height(40);
			val5.set_VerticalAlignment((VerticalAlignment)1);
			val5.set_HorizontalAlignment((HorizontalAlignment)1);
			BuildBaitCell(row, fish, new Point(510, 0));
			BuildTimeOfDayCell(row, fish, new Point(590, 0));
			return row;
		}

		private static string TruncateToWidth(string text, int columnWidthPx)
		{
			int maxChars = (int)((double)columnWidthPx / 7.0);
			if (text.Length <= maxChars)
			{
				return text;
			}
			return text.Substring(0, Math.Max(maxChars - 3, 1)) + "...";
		}

		private static string FormatFoundInShort(Fish fish)
		{
			if (string.IsNullOrEmpty(fish.FoundIn))
			{
				return EnumDisplay.Format(fish.Location);
			}
			string firstPart = fish.FoundIn.Split(new string[2] { " and ", "," }, StringSplitOptions.None)[0].Trim();
			return TruncateToWidth((firstPart.Length < fish.FoundIn.Length) ? (firstPart + ", ...") : firstPart, 170);
		}

		private static string FormatFoundInFull(Fish fish)
		{
			if (!string.IsNullOrEmpty(fish.FoundIn))
			{
				return fish.FoundIn;
			}
			return EnumDisplay.Format(fish.Location);
		}

		private void BuildTimeOfDayCell(Panel row, Fish fish, Point location)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			List<AsyncTexture2D> textures = TimeOfDayIcons.GetTextures(_contentsManager, fish);
			string tooltip = FormatTimeOfDayFull(fish);
			int totalWidth = textures.Count * 28 + (textures.Count - 1) * 4;
			int startX = location.X + (80 - totalWidth) / 2;
			int iconY = location.Y + 6;
			for (int i = 0; i < textures.Count; i++)
			{
				Image val = new Image();
				((Control)val).set_Parent((Container)(object)row);
				val.set_Texture(textures[i]);
				((Control)val).set_Location(new Point(startX + i * 32, iconY));
				((Control)val).set_Size(new Point(28, 28));
				((Control)val).set_BasicTooltipText(tooltip);
			}
		}

		private void BuildBaitCell(Panel row, Fish fish, Point location)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			AsyncTexture2D texture = BaitIcons.GetTexture(fish.Bait);
			if (texture != null)
			{
				Image val = new Image();
				((Control)val).set_Parent((Container)(object)row);
				val.set_Texture(texture);
				((Control)val).set_Location(new Point(location.X + 24, 4));
				((Control)val).set_Size(new Point(32, 32));
				((Control)val).set_BasicTooltipText(EnumDisplay.Format(fish.Bait));
			}
			else
			{
				string baitText = EnumDisplay.Format(fish.Bait);
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)row);
				val2.set_Text(TruncateToWidth(baitText, 80));
				((Control)val2).set_BasicTooltipText(baitText);
				((Control)val2).set_Location(location);
				((Control)val2).set_Width(80);
				((Control)val2).set_Height(40);
				val2.set_VerticalAlignment((VerticalAlignment)1);
				val2.set_HorizontalAlignment((HorizontalAlignment)1);
			}
		}

		private static string FormatHolesShort(Fish fish)
		{
			List<string> holes = (from h in new FishingHole?[4] { fish.Hole1, fish.Hole2, fish.Hole3, fish.Hole4 }
				where h.HasValue
				select EnumDisplay.Format(h.Value)).ToList();
			if (holes.Count == 0)
			{
				return "Any";
			}
			return TruncateToWidth((holes.Count == 1) ? holes[0] : (holes[0] + ", ..."), 130);
		}

		private static string FormatHolesFull(Fish fish)
		{
			IEnumerable<string> holes = from h in new FishingHole?[4] { fish.Hole1, fish.Hole2, fish.Hole3, fish.Hole4 }
				where h.HasValue
				select EnumDisplay.Format(h.Value);
			string joined = string.Join(", ", holes);
			if (!string.IsNullOrEmpty(joined))
			{
				return joined;
			}
			return "Any";
		}

		private static string FormatTimeOfDayFull(Fish fish)
		{
			if (fish.TimeOfDay == TimeOfDay.Any)
			{
				if (!fish.HigherChance.HasValue)
				{
					return "Any";
				}
				return $"Any (favors {fish.HigherChance})";
			}
			if (fish.TimeOfDay2.HasValue)
			{
				string text = $"{fish.TimeOfDay}/{fish.TimeOfDay2}";
				if (!fish.HigherChance.HasValue)
				{
					return text;
				}
				return $"{text} (favors {fish.HigherChance})";
			}
			return fish.TimeOfDay.ToString();
		}
	}
}
