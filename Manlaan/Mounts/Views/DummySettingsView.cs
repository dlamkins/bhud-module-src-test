using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;

namespace Manlaan.Mounts.Views
{
	internal class DummySettingsView : View
	{
		private ContentsManager contentsManager;

		public DummySettingsView(ContentsManager contentsManager)
			: this()
		{
			this.contentsManager = contentsManager;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Expected O, but got Unknown
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Location(new Point(200, 140));
			val.set_AutoSizeWidth(true);
			val.set_StrokeText(true);
			val.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)18, (FontStyle)1));
			val.set_Text("Settings have moved to the ");
			val.set_HorizontalAlignment((HorizontalAlignment)1);
			Label text1_label = val;
			Image val2 = new Image();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Size(new Point(32, 32));
			((Control)val2).set_Location(new Point(((Control)text1_label).get_Right() + 3, ((Control)text1_label).get_Bottom() - 32));
			val2.set_Texture(AsyncTexture2D.op_Implicit(contentsManager.GetTexture("514394-grey.png")));
			Image _btnTab = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent(buildPanel);
			((Control)val3).set_Location(new Point(((Control)_btnTab).get_Right() + 3, ((Control)text1_label).get_Top()));
			val3.set_AutoSizeWidth(true);
			val3.set_StrokeText(true);
			val3.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)18, (FontStyle)1));
			val3.set_Text("tab!");
			Label text2_label = val3;
		}
	}
}
