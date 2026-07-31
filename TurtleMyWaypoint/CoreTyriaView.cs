using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;

namespace TurtleMyWaypoint
{
	internal class CoreTyriaView : View
	{
		private const int PanelHeaderHeight = 10;

		private const int PanelBottomPadding = 40;

		private const int PanelSpacing = 10;

		private const int PanelInnerMargin = 28;

		private readonly int _contentWidth;

		private readonly int _contentHeight;

		public CoreTyriaView(int contentWidth, int contentHeight)
			: this()
		{
			_contentWidth = contentWidth;
			_contentHeight = contentHeight;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Expected O, but got Unknown
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Size(new Point(_contentWidth, _contentHeight - 10));
			val.set_CanScroll(true);
			Panel scrollPanel = val;
			int panelWidth = _contentWidth - 28;
			int y = 0;
			RegionSection[] coreTyria = WaypointData.CoreTyria;
			for (int i = 0; i < coreTyria.Length; i++)
			{
				RegionSection section = coreTyria[i];
				int rowsHeight = MapRowBuilder.MeasureHeight(section.Maps, panelWidth);
				int panelHeight = 10 + rowsHeight + 40;
				Panel val2 = new Panel();
				((Control)val2).set_Parent((Container)(object)scrollPanel);
				((Control)val2).set_Location(new Point(0, y));
				((Control)val2).set_Size(new Point(panelWidth, panelHeight));
				val2.set_ShowBorder(true);
				val2.set_Title(section.RegionName);
				MapRowBuilder.BuildRows((Container)val2, section.Maps, panelWidth, 10);
				y += panelHeight + 10;
			}
		}

		protected override void Unload()
		{
		}
	}
}
