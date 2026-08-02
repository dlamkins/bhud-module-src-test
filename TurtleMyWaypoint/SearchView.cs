using System;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace TurtleMyWaypoint
{
	internal class SearchView : View
	{
		private readonly struct WaypointMatch
		{
			public readonly string WaypointName;

			public readonly string MapName;

			public readonly string Code;

			public WaypointMatch(string waypointName, string mapName, string code)
			{
				WaypointName = waypointName;
				MapName = mapName;
				Code = code;
			}
		}

		private const int SearchBoxHeight = 32;

		private const int SearchBoxSpacing = 10;

		private const int PanelHeaderHeight = 10;

		private const int PanelBottomPadding = 40;

		private const int PanelInnerMargin = 28;

		private const int LabelMarginLeft = 10;

		private const int RightMargin = 16;

		private const int ButtonWidth = 90;

		private const int ButtonHeight = 24;

		private const int RowHeight = 28;

		private const int RowSpacing = 6;

		private readonly int _contentWidth;

		private readonly int _contentHeight;

		private Panel _resultsPanel;

		public SearchView(int contentWidth, int contentHeight)
			: this()
		{
			_contentWidth = contentWidth;
			_contentHeight = contentHeight;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Expected O, but got Unknown
			TextBox val = new TextBox();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Size(new Point(_contentWidth, 32));
			((TextInputBase)val).set_PlaceholderText(Strings.SearchPlaceholder);
			TextBox searchBox = val;
			Panel val2 = new Panel();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Location(new Point(0, 42));
			((Control)val2).set_Size(new Point(_contentWidth, _contentHeight - 32 - 10 - 10));
			val2.set_CanScroll(true);
			_resultsPanel = val2;
			((TextInputBase)searchBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				UpdateResults(((TextInputBase)searchBox).get_Text());
			});
			UpdateResults(string.Empty);
		}

		private static WaypointMatch[] FindMatches(string query)
		{
			return (from w in WaypointData.AllMaps.SelectMany((MapEntry map) => from i in Enumerable.Range(0, map.Codes.Length)
					select new WaypointMatch(map.WaypointName(i), map.Name, map.Codes[i]))
				where w.WaypointName.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0
				select w).OrderBy((WaypointMatch w) => w.WaypointName, StringComparer.OrdinalIgnoreCase).ToArray();
		}

		private void UpdateResults(string query)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Expected O, but got Unknown
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			((Container)_resultsPanel).ClearChildren();
			query = query?.Trim() ?? string.Empty;
			WaypointMatch[] matches = ((query.Length == 0) ? Array.Empty<WaypointMatch>() : FindMatches(query));
			if (matches.Length == 0)
			{
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)_resultsPanel);
				val.set_Text((query.Length == 0) ? Strings.SearchHint : Strings.NoResults);
				((Control)val).set_Location(new Point(0, 0));
				((Control)val).set_Size(new Point(_contentWidth, _contentHeight));
				val.set_Font(GameService.Content.get_DefaultFont16());
				val.set_HorizontalAlignment((HorizontalAlignment)1);
				val.set_VerticalAlignment((VerticalAlignment)1);
				val.set_TextColor(Color.get_LightGray());
				return;
			}
			int panelWidth = _contentWidth - 28;
			int rowsHeight = matches.Length * 34;
			int panelHeight = 10 + rowsHeight + 40;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)_resultsPanel);
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Size(new Point(panelWidth, panelHeight));
			val2.set_ShowBorder(true);
			val2.set_Title($"{Strings.SearchResults} ({matches.Length})");
			Panel resultsListPanel = val2;
			int labelWidth = panelWidth - 10 - 16 - 90 - 10;
			int y = 10;
			WaypointMatch[] array = matches;
			for (int i = 0; i < array.Length; i++)
			{
				WaypointMatch match = array[i];
				Label val3 = new Label();
				((Control)val3).set_Parent((Container)(object)resultsListPanel);
				val3.set_Text(match.WaypointName + " (" + match.MapName + ")");
				((Control)val3).set_Location(new Point(10, y));
				((Control)val3).set_Size(new Point(labelWidth, 28));
				val3.set_Font(GameService.Content.get_DefaultFont16());
				val3.set_VerticalAlignment((VerticalAlignment)1);
				val3.set_WrapText(false);
				StandardButton val4 = new StandardButton();
				((Control)val4).set_Parent((Container)(object)resultsListPanel);
				val4.set_Text(Strings.Copy);
				((Control)val4).set_Size(new Point(90, 24));
				((Control)val4).set_Location(new Point(panelWidth - 16 - 90, y + 2));
				string code = match.Code;
				((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					MapRowBuilder.TrySetClipboardText(code);
				});
				y += 34;
			}
		}

		protected override void Unload()
		{
		}
	}
}
