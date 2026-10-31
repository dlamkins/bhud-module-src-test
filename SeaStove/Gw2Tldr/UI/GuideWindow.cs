using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using SeaStove.Gw2Tldr.Data;

namespace SeaStove.Gw2Tldr.UI
{
	public sealed class GuideWindow : IDisposable
	{
		private readonly UtilityWindow _window;

		private readonly GuideCatalog _catalog;

		private readonly Action<string> _rememberGuide;

		private readonly Action<GuideEntry> _onGuideSelected;

		private readonly TextBox _search;

		private readonly SearchResultPanel _searchField;

		private readonly Panel _encounters;

		private readonly Label _results;

		private readonly DarkButton _back;

		private readonly DarkButton _sortTimer;

		private readonly DarkButton _sortExpansion;

		private readonly DarkButton _sortAlphabetical;

		private string _metaSort = "Timers";

		private readonly DarkButton[] _fractalSortButtons = new DarkButton[5];

		private int _fractalTier;

		private readonly List<Tuple<GuideEntry, Panel, Label>> _metaRows = new List<Tuple<GuideEntry, Panel, Label>>();

		private long _lastTimerUpdate = -1L;

		private bool _timerWindowVisible;

		private readonly object _searchGate = new object();

		private string _searchQuery = "";

		private bool _searchPending;

		private string _category;

		private string _section;

		private string _group;

		private static readonly string[] BrowserCategories = new string[4] { "raids", "fractals", "metas", "dungeons" };

		private static readonly string[] CategoryLabels = new string[4] { "Raid encounters", "Fractals", "Meta events", "Dungeons" };

		public GuideEntry CurrentGuide { get; private set; }

		private static bool InCategory(GuideEntry guide, string category)
		{
			if (!(category == "raids"))
			{
				return guide.Category == category;
			}
			if (!(guide.Category == "raids"))
			{
				return guide.Category == "strikes";
			}
			return true;
		}

		public GuideWindow(GuideCatalog catalog, string selectedId, Action<string> rememberGuide, Action<GuideEntry> onGuideSelected)
		{
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Expected O, but got Unknown
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Expected O, but got Unknown
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Expected O, but got Unknown
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			GuideWindow guideWindow = this;
			_catalog = catalog;
			_rememberGuide = rememberGuide;
			_onGuideSelected = onGuideSelected;
			CurrentGuide = catalog.Guides.FirstOrDefault((GuideEntry guide) => guide.Id == selectedId) ?? catalog.Guides.FirstOrDefault();
			_window = new UtilityWindow("Browse encounters", "GW2 TLDR", new Point(250, 180), resizable: true);
			((Control)_window).Hide();
			SearchResultPanel searchResultPanel = new SearchResultPanel();
			((Control)searchResultPanel).set_Parent((Container)(object)_window.Body);
			((Control)searchResultPanel).set_Location(new Point(10, 4));
			((Control)searchResultPanel).set_Size(new Point(495, 34));
			_searchField = searchResultPanel;
			ToolbarIcon toolbarIcon = new ToolbarIcon("search");
			((Control)toolbarIcon).set_Parent((Container)(object)_searchField);
			((Control)toolbarIcon).set_Location(new Point(7, 5));
			((Control)toolbarIcon).set_Size(new Point(22, 22));
			((Control)toolbarIcon).set_BasicTooltipText("Search guides");
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)_searchField);
			((Control)val).set_Location(new Point(35, 4));
			((Control)val).set_Width(450);
			val.set_HideBackground(true);
			((TextInputBase)val).set_PlaceholderText("Search all encounters, zones, and mechanics");
			_search = val;
			DarkButton darkButton = new DarkButton();
			((Control)darkButton).set_Parent((Container)(object)_window.Body);
			((Control)darkButton).set_Location(new Point(10, 42));
			((Control)darkButton).set_Width(60);
			((Label)darkButton).set_Text("‹ Back");
			_back = darkButton;
			((Control)_back).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				guideWindow.GoBack();
			});
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)_window.Body);
			((Control)val2).set_Location(new Point(80, 42));
			((Control)val2).set_Size(new Point(425, 28));
			val2.set_TextColor(Color.get_LightGray());
			_results = val2;
			Panel val3 = new Panel();
			((Control)val3).set_Parent((Container)(object)_window.Body);
			((Control)val3).set_Location(new Point(10, 78));
			((Control)val3).set_Size(new Point(500, 470));
			val3.set_CanScroll(true);
			_encounters = val3;
			DarkButton darkButton2 = new DarkButton();
			((Control)darkButton2).set_Parent((Container)(object)_window.Body);
			((Control)darkButton2).set_Location(new Point(10, 78));
			((Control)darkButton2).set_Size(new Point(90, 26));
			((Label)darkButton2).set_Text("Timers");
			_sortTimer = darkButton2;
			DarkButton darkButton3 = new DarkButton();
			((Control)darkButton3).set_Parent((Container)(object)_window.Body);
			((Control)darkButton3).set_Location(new Point(105, 78));
			((Control)darkButton3).set_Size(new Point(105, 26));
			((Label)darkButton3).set_Text("Expansion");
			_sortExpansion = darkButton3;
			DarkButton darkButton4 = new DarkButton();
			((Control)darkButton4).set_Parent((Container)(object)_window.Body);
			((Control)darkButton4).set_Location(new Point(215, 78));
			((Control)darkButton4).set_Size(new Point(90, 26));
			((Label)darkButton4).set_Text("A-Z");
			_sortAlphabetical = darkButton4;
			((Control)_sortTimer).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				guideWindow._metaSort = "Timers";
				guideWindow._section = null;
				guideWindow.RefreshMatches();
			});
			((Control)_sortExpansion).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				guideWindow._metaSort = "Expansion";
				guideWindow._section = null;
				guideWindow.RefreshMatches();
			});
			((Control)_sortAlphabetical).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				guideWindow._metaSort = "A-Z";
				guideWindow._section = null;
				guideWindow.RefreshMatches();
			});
			for (int index = 0; index < _fractalSortButtons.Length; index++)
			{
				int tier = index;
				DarkButton darkButton5 = new DarkButton();
				((Control)darkButton5).set_Parent((Container)(object)_window.Body);
				((Control)darkButton5).set_Location(new Point(10 + index * 75, 78));
				((Control)darkButton5).set_Size(new Point(70, 26));
				DarkButton button = darkButton5;
				((Control)button).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					guideWindow._fractalTier = tier;
					guideWindow.RefreshMatches();
				});
				_fractalSortButtons[index] = button;
			}
			_window.WindowResized += OnWindowResized;
			((TextInputBase)_search).add_TextChanged((EventHandler<EventArgs>)OnSearchChanged);
			RefreshMatches();
		}

		public void SetCurrentGuide(GuideEntry guide)
		{
			CurrentGuide = guide;
		}

		public void Toggle()
		{
			if (((Control)_window).get_Visible())
			{
				((Control)_window).Hide();
			}
			else
			{
				((Control)_window).Show();
			}
		}

		public void Hide()
		{
			((Control)_window).Hide();
		}

		private void OnSearchChanged(object sender, EventArgs args)
		{
			string query = ((TextInputBase)_search).get_Text() ?? "";
			lock (_searchGate)
			{
				_searchQuery = query;
				_searchPending = true;
			}
		}

		public void Update()
		{
			_window.UpdateDrag();
			long minute = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 60;
			if (((Control)_window).get_Visible() && _metaRows.Count > 0 && (!_timerWindowVisible || _lastTimerUpdate != minute))
			{
				_lastTimerUpdate = minute;
				UpdateMetaRows();
			}
			_timerWindowVisible = ((Control)_window).get_Visible();
			lock (_searchGate)
			{
				if (!_searchPending)
				{
					return;
				}
				_searchPending = false;
			}
			RefreshMatches();
		}

		private void OnWindowResized()
		{
			((Control)_searchField).set_Width(((Control)_window.Body).get_Width() - 20);
			((Control)_search).set_Width(((Control)_searchField).get_Width() - 45);
			((Control)_results).set_Width(((Control)_window.Body).get_Width() - 90);
			((Control)_encounters).set_Width(((Control)_window.Body).get_Width() - 20);
			((Control)_encounters).set_Height(Math.Max(1, ((Control)_window.Body).get_Height() - ((Control)_encounters).get_Top() - 12));
			foreach (SearchResultPanel card in ((IEnumerable)((Container)_encounters).get_Children()).OfType<SearchResultPanel>())
			{
				((Control)card).set_Width(((Control)_encounters).get_Width() - 40);
				foreach (Label item in from label in ((IEnumerable)((Container)card).get_Children()).OfType<Label>()
					where ((Control)label).get_Width() >= 400
					select label)
				{
					((Control)item).set_Width(((Control)card).get_Width() - 20);
				}
			}
		}

		private void GoBack()
		{
			if (_group != null)
			{
				_group = null;
			}
			else if (_section != null)
			{
				_section = null;
			}
			else
			{
				_category = null;
			}
			RefreshMatches();
		}

		private void AddChoice(string title, string detail, int top, Action choose)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			SearchResultPanel searchResultPanel = new SearchResultPanel();
			((Control)searchResultPanel).set_Parent((Container)(object)_encounters);
			((Control)searchResultPanel).set_Location(new Point(6, top));
			((Control)searchResultPanel).set_Size(new Point(((Control)_encounters).get_Width() - 40, 58));
			SearchResultPanel card = searchResultPanel;
			EventHandler<MouseEventArgs> click = delegate
			{
				choose();
			};
			((Control)card).add_Click(click);
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)card);
			((Control)val).set_Location(new Point(10, 4));
			((Control)val).set_Size(new Point(((Control)card).get_Width() - 20, 24));
			val.set_Text(title);
			val.set_TextColor(Color.get_White());
			((Control)val).add_Click(click);
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)card);
			((Control)val2).set_Location(new Point(10, 29));
			((Control)val2).set_Size(new Point(((Control)card).get_Width() - 20, 22));
			val2.set_Text(detail);
			val2.set_TextColor(Color.get_LightGray());
			((Control)val2).add_Click(click);
		}

		private void RefreshMatches()
		{
			//IL_0772: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_07df: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0807: Unknown result type (might be due to invalid IL or missing references)
			//IL_0811: Unknown result type (might be due to invalid IL or missing references)
			//IL_0842: Unknown result type (might be due to invalid IL or missing references)
			//IL_0843: Unknown result type (might be due to invalid IL or missing references)
			//IL_0854: Unknown result type (might be due to invalid IL or missing references)
			//IL_0859: Unknown result type (might be due to invalid IL or missing references)
			//IL_0861: Unknown result type (might be due to invalid IL or missing references)
			//IL_0866: Unknown result type (might be due to invalid IL or missing references)
			//IL_0870: Unknown result type (might be due to invalid IL or missing references)
			//IL_087d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0887: Unknown result type (might be due to invalid IL or missing references)
			//IL_08c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0900: Unknown result type (might be due to invalid IL or missing references)
			//IL_0905: Unknown result type (might be due to invalid IL or missing references)
			//IL_090f: Unknown result type (might be due to invalid IL or missing references)
			//IL_091c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0926: Unknown result type (might be due to invalid IL or missing references)
			//IL_0940: Expected O, but got Unknown
			//IL_09d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_09fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a19: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a1e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a26: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a30: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a3a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a3f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a49: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a5f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a60: Unknown result type (might be due to invalid IL or missing references)
			string query;
			lock (_searchGate)
			{
				query = _searchQuery;
			}
			_metaRows.Clear();
			Control[] array = ((Container)_encounters).get_Children().ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Dispose();
			}
			int top = 6;
			bool searching = !string.IsNullOrWhiteSpace(query);
			((Control)_back).set_Visible(!searching && _category != null);
			bool showMetaSort = !searching && _category == "metas" && _section == null;
			DarkButton sortTimer = _sortTimer;
			DarkButton sortExpansion = _sortExpansion;
			bool flag;
			((Control)_sortAlphabetical).set_Visible(flag = showMetaSort);
			bool visible;
			((Control)sortExpansion).set_Visible(visible = flag);
			((Control)sortTimer).set_Visible(visible);
			((Label)_sortTimer).set_Text(((_metaSort == "Timers") ? "• " : "") + "Timers");
			((Label)_sortExpansion).set_Text(((_metaSort == "Expansion") ? "• " : "") + "Expansion");
			((Label)_sortAlphabetical).set_Text(((_metaSort == "A-Z") ? "• " : "") + "A-Z");
			bool showFractalSort = !searching && _category == "fractals";
			for (int index2 = 0; index2 < _fractalSortButtons.Length; index2++)
			{
				((Control)_fractalSortButtons[index2]).set_Visible(showFractalSort);
				((Label)_fractalSortButtons[index2]).set_Text(((index2 == _fractalTier) ? "• " : "") + ((index2 == 0) ? "A-Z" : ("T" + index2)));
			}
			((Control)_encounters).set_Top((!searching && _category == null) ? 42 : ((showMetaSort || showFractalSort) ? 110 : 78));
			((Control)_encounters).set_Height(Math.Max(1, ((Control)_window.Body).get_Height() - ((Control)_encounters).get_Top() - 12));
			if (!searching && _category == null)
			{
				_results.set_Text("");
				for (int index = 0; index < BrowserCategories.Length; index++)
				{
					string category = BrowserCategories[index];
					AddChoice(detail: _catalog.Guides.Count((GuideEntry guide) => InCategory(guide, category)) + " guides", title: CategoryLabels[index], top: top, choose: delegate
					{
						_category = category;
						_section = null;
						_group = null;
						RefreshMatches();
					});
					top += 64;
				}
				_window.RefreshTheme();
				return;
			}
			if (!searching && _section == null && _category != "fractals" && (_category != "metas" || _metaSort == "Expansion"))
			{
				_results.set_Text(CategoryLabels[Array.IndexOf(BrowserCategories, _category)] + " • Choose a section");
				foreach (string section in from value in (from guide in _catalog.Guides
						where InCategory(guide, _category)
						select guide.Section).Distinct()
					orderby value
					select value)
				{
					string current2 = section;
					AddChoice(detail: _catalog.Guides.Count((GuideEntry guide) => InCategory(guide, _category) && guide.Section == current2) + " guides", title: current2, top: top, choose: delegate
					{
						_section = current2;
						_group = null;
						RefreshMatches();
					});
					top += 64;
				}
				return;
			}
			if (!searching && _category == "raids" && _group == null && _catalog.Guides.Any((GuideEntry guide) => guide.Category == "raids" && guide.Section == _section))
			{
				_results.set_Text(_section + " • Choose a wing");
				foreach (string group in from value in (from guide in _catalog.Guides
						where InCategory(guide, _category) && guide.Section == _section
						select guide.Group).Distinct()
					orderby value
					select value)
				{
					string current = group;
					int count2 = _catalog.Guides.Count((GuideEntry guide) => InCategory(guide, _category) && guide.Section == _section && guide.Group == current);
					AddChoice(current, count2 + ((count2 == 1) ? " encounter" : " encounters"), top, delegate
					{
						_group = current;
						RefreshMatches();
					});
					top += 64;
				}
				_window.RefreshTheme();
				return;
			}
			List<GuideEntry> matches = (from guide in _catalog.Search((searching || _category == "raids") ? null : _category, query)
				where searching || (InCategory(guide, _category) && (_section == null || guide.Section == _section) && (_group == null || guide.Group == _group))
				select guide).ToList();
			if (!searching && _category == "metas" && _section == null && _metaSort == "Timers")
			{
				matches = SortMetas(matches, DateTimeOffset.UtcNow);
			}
			if (!searching && _category == "fractals")
			{
				matches = _catalog.SortFractals(matches, _fractalTier);
			}
			_results.set_Text((matches.Count == 0) ? "No guides match. Try another search." : (matches.Count + " guides found"));
			foreach (GuideEntry guide2 in matches)
			{
				SearchResultPanel searchResultPanel = new SearchResultPanel();
				((Control)searchResultPanel).set_Parent((Container)(object)_encounters);
				((Control)searchResultPanel).set_Location(new Point(6, top));
				((Control)searchResultPanel).set_Size(new Point(((Control)_encounters).get_Width() - 40, (guide2.Category == "metas") ? 100 : 78));
				((Control)searchResultPanel).set_BasicTooltipText(guide2.DisplayName);
				SearchResultPanel card = searchResultPanel;
				EventHandler<MouseEventArgs> choose = delegate
				{
					CurrentGuide = guide2;
					_rememberGuide(guide2.Id);
					_onGuideSelected(guide2);
					((Control)_window).Hide();
				};
				((Control)card).add_Click(choose);
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)card);
				((Control)val).set_Location(new Point(10, 4));
				((Control)val).set_Size(new Point(((Control)card).get_Width() - 20, 23));
				val.set_Text(guide2.Name + (guide2.HasChallengeMode ? "  CM" : ""));
				val.set_TextColor(Color.get_White());
				((Control)val).add_Click(choose);
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)card);
				((Control)val2).set_Location(new Point(10, 28));
				((Control)val2).set_Size(new Point(((Control)card).get_Width() - 20, 22));
				string metadata = guide2.Metadata;
				string levels = _catalog.FractalLevels(guide2);
				val2.set_Text(metadata + ((levels != null) ? (" • " + levels) : ""));
				val2.set_TextColor(Color.get_LightGray());
				((Control)val2).add_Click(choose);
				if (guide2.Category == "metas")
				{
					Label val3 = new Label();
					((Control)val3).set_Parent((Container)(object)card);
					((Control)val3).set_Location(new Point(10, 51));
					((Control)val3).set_Size(new Point(((Control)card).get_Width() - 20, 22));
					val3.set_Text(FormatMetaTimer(guide2, DateTimeOffset.UtcNow));
					Label timer = val3;
					((Control)timer).add_Click(choose);
					_metaRows.Add(Tuple.Create<GuideEntry, Panel, Label>(guide2, (Panel)(object)card, timer));
				}
				int iconTop = ((guide2.Category == "metas") ? 76 : 53);
				int x = 10;
				foreach (IGrouping<string, Mechanic> count in from mechanic in guide2.GetMechanics(challengeMode: false)
					group mechanic by mechanic.Type)
				{
					MechanicIcon mechanicIcon = new MechanicIcon(count.Key, Color.get_LightGray());
					((Control)mechanicIcon).set_Parent((Container)(object)card);
					((Control)mechanicIcon).set_Location(new Point(x, iconTop));
					((Control)mechanicIcon).set_Size(new Point(18, 18));
					((Control)mechanicIcon).set_BasicTooltipText(count.Key);
					((Control)mechanicIcon).add_Click(choose);
					Label val4 = new Label();
					((Control)val4).set_Parent((Container)(object)card);
					((Control)val4).set_Location(new Point(x + 21, iconTop - 1));
					((Control)val4).set_Size(new Point(24, 22));
					val4.set_Text(count.Count().ToString());
					val4.set_TextColor(Color.get_LightGray());
					((Control)val4).add_Click(choose);
					x += 47;
				}
				top += ((guide2.Category == "metas") ? 106 : 84);
			}
			_window.RefreshTheme();
		}

		private List<GuideEntry> SortMetas(IEnumerable<GuideEntry> guides, DateTimeOffset now)
		{
			return (from item in (from guide in guides
					select new
					{
						guide = guide,
						timer = _catalog.NextMeta(guide, now)
					} into item
					orderby (item.timer != null) ? ((!item.timer.Active) ? 1 : 0) : 2, (item.timer != null) ? ((!item.timer.Active) ? item.timer.Start : (DateTimeOffset.MaxValue - (item.timer.Start - DateTimeOffset.MinValue))) : DateTimeOffset.MaxValue
					select item).ThenBy(item => item.guide.Name, StringComparer.OrdinalIgnoreCase)
				select item.guide).ToList();
		}

		private string FormatMetaTimer(GuideEntry guide, DateTimeOffset now)
		{
			MetaSchedule.Occurrence timer = _catalog.NextMeta(guide, now);
			if (timer == null)
			{
				return "No fixed timer available";
			}
			if (timer.Active)
			{
				return "Active (started " + (int)Math.Floor((now - timer.Start).TotalMinutes) + "m ago)";
			}
			return "Next: " + timer.Start.ToLocalTime().ToString("t") + " (in " + Math.Max(0, (int)Math.Ceiling((timer.Start - now).TotalMinutes)) + "m)";
		}

		private void UpdateMetaRows()
		{
			DateTimeOffset now = DateTimeOffset.UtcNow;
			foreach (Tuple<GuideEntry, Panel, Label> row2 in _metaRows)
			{
				row2.Item3.set_Text(FormatMetaTimer(row2.Item1, now));
			}
			if (_category != "metas" || _metaSort != "Timers" || _section != null || !string.IsNullOrWhiteSpace(((TextInputBase)_search).get_Text()))
			{
				return;
			}
			List<GuideEntry> sorted = SortMetas(_metaRows.Select((Tuple<GuideEntry, Panel, Label> row) => row.Item1), now);
			int index;
			for (index = 0; index < sorted.Count; index++)
			{
				((Control)_metaRows.First((Tuple<GuideEntry, Panel, Label> row) => row.Item1.Id == sorted[index].Id).Item2).set_Top(6 + index * 106);
			}
		}

		public void Dispose()
		{
			_window.WindowResized -= OnWindowResized;
			((TextInputBase)_search).remove_TextChanged((EventHandler<EventArgs>)OnSearchChanged);
			((Control)_window).Dispose();
		}
	}
}
