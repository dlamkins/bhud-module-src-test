using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;

namespace SongbookOfTyria.UI.Views
{
	public class AboutView : View
	{
		private const int LeftPadding = 50;

		private const int DefaultSpacing = 10;

		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Control)val).set_Width(buildPanel.get_ContentRegion().Width - 50 - 10);
			((Control)val).set_Height(buildPanel.get_ContentRegion().Height);
			((Control)val).set_Location(new Point(50, 0));
			((Control)val).set_Parent(buildPanel);
			val.set_ControlPadding(new Vector2(0f, 10f));
			val.set_OuterControlPadding(new Vector2(0f, 10f));
			FlowPanel aboutPanel = val;
			Label val2 = new Label();
			val2.set_Text("How to play is under construction, check back later");
			val2.set_Font(GameService.Content.get_DefaultFont32());
			val2.set_TextColor(Color.get_White());
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Parent((Container)(object)aboutPanel);
			Label val3 = new Label();
			val3.set_Text("\"Hi guys\" - Netorzin.3189");
			val3.set_Font(GameService.Content.get_DefaultFont16());
			val3.set_TextColor(Color.get_LightGray());
			val3.set_AutoSizeWidth(true);
			val3.set_AutoSizeHeight(true);
			((Control)val3).set_Parent((Container)(object)aboutPanel);
		}

		public AboutView()
			: this()
		{
		}
	}
}
