using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using RaidClears.Localization;

namespace RaidClears.Features.Raids
{
	public class MentorProgressExamplePopupPanel : Panel
	{
		private const int IconSize = 48;

		private const int ContentPadding = 8;

		private bool _isDragging;

		private Point _dragStart;

		public MentorProgressExamplePopupPanel()
			: this()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Expected O, but got Unknown
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Expected O, but got Unknown
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(300, 72));
			((Panel)this).set_BackgroundTexture(Service.Textures?.DatAsset(156112));
			((Panel)this).set_ShowBorder(false);
			((Panel)this).set_ShowTint(false);
			Image val = new Image();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Size(new Point(48, 48));
			((Control)val).set_Location(new Point(8, (((Control)this).get_Height() - 48) / 2));
			val.set_Texture(Service.Textures?.DatAsset(1203237) ?? AsyncTexture2D.op_Implicit(Textures.get_Pixel()));
			int contentLeft = 64;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Location(new Point(contentLeft, 8));
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			val2.set_Font(Control.get_Content().get_DefaultFont16());
			val2.set_TextColor(Color.get_LightGoldenrodYellow());
			val2.set_Text(Strings.MentorProgress_ExamplePopup_Title);
			Label bossNameLabel = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Location(new Point(contentLeft, ((Control)bossNameLabel).get_Bottom() + 2));
			val3.set_AutoSizeWidth(true);
			val3.set_Font(Control.get_Content().get_DefaultFont18());
			val3.set_TextColor(new Color(218, 165, 32));
			val3.set_Text("+1");
			Label deltaLabel = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Location(new Point(((Control)deltaLabel).get_Right() + 4, ((Control)bossNameLabel).get_Bottom() + 4));
			val4.set_AutoSizeWidth(true);
			val4.set_Font(Control.get_Content().get_DefaultFont14());
			val4.set_TextColor(Color.get_LightGoldenrodYellow());
			val4.set_Text("99 / 1000");
			((Control)this).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				_isDragging = true;
				_dragStart = GameService.Input.get_Mouse().get_Position();
			});
			((Control)this).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				_isDragging = false;
			});
		}

		public void UpdateDrag()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			if (_isDragging)
			{
				Point current = GameService.Input.get_Mouse().get_Position();
				Point delta = current - _dragStart;
				((Control)this).set_Location(((Control)this).get_Location() + delta);
				_dragStart = current;
			}
		}
	}
}
