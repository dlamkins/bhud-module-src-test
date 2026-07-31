using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;

namespace TurtleMyWaypoint
{
	internal class MapWaypointsView : View
	{
		private const int PanelHeaderHeight = 10;

		private const int PanelBottomPadding = 40;

		private const int PanelInnerMargin = 28;

		private readonly MapEntry[] _maps;

		private readonly string _panelTitleEn;

		private readonly string _panelTitleFr;

		private readonly int _contentWidth;

		private readonly int _contentHeight;

		public MapWaypointsView(MapEntry[] maps, string panelTitleEn, string panelTitleFr, int contentWidth, int contentHeight)
			: this()
		{
			_maps = maps;
			_panelTitleEn = panelTitleEn;
			_panelTitleFr = panelTitleFr;
			_contentWidth = contentWidth;
			_contentHeight = contentHeight;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Expected O, but got Unknown
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Expected O, but got Unknown
			if (_maps.Length == 0)
			{
				Label val = new Label();
				((Control)val).set_Parent(buildPanel);
				val.set_Text(Strings.NoDataYet);
				((Control)val).set_Location(new Point(0, 0));
				((Control)val).set_Size(new Point(_contentWidth, _contentHeight));
				val.set_Font(GameService.Content.get_DefaultFont16());
				val.set_HorizontalAlignment((HorizontalAlignment)1);
				val.set_VerticalAlignment((VerticalAlignment)1);
				val.set_TextColor(Color.get_LightGray());
			}
			else
			{
				Panel val2 = new Panel();
				((Control)val2).set_Parent(buildPanel);
				((Control)val2).set_Location(new Point(0, 0));
				((Control)val2).set_Size(new Point(_contentWidth, _contentHeight - 10));
				val2.set_CanScroll(true);
				Panel scrollPanel = val2;
				int panelWidth = _contentWidth - 28;
				int rowsHeight = MapRowBuilder.MeasureHeight(_maps, panelWidth);
				int panelHeight = 10 + rowsHeight + 40;
				Panel val3 = new Panel();
				((Control)val3).set_Parent((Container)(object)scrollPanel);
				((Control)val3).set_Location(new Point(0, 0));
				((Control)val3).set_Size(new Point(panelWidth, panelHeight));
				val3.set_ShowBorder(true);
				val3.set_Title(Strings.IsFrench ? _panelTitleFr : _panelTitleEn);
				MapRowBuilder.BuildRows((Container)val3, _maps, panelWidth, 10);
			}
		}

		protected override void Unload()
		{
		}
	}
}
