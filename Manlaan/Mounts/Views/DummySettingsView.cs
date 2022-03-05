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
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Expected O, but got Unknown
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
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
		}
	}
}
