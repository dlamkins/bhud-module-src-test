using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace WvWPipTally.Controls
{
	public sealed class MatchOverviewHelpWindow : StandardWindow
	{
		private static readonly Rectangle BgWindowRegion = new Rectangle(40, 26, 913, 691);

		private static readonly Rectangle BgContentRegion = new Rectangle(70, 36, 839, 605);

		private const int ImageWidth = 820;

		private const int ImageHeight = 597;

		public MatchOverviewHelpWindow(AsyncTexture2D background, AsyncTexture2D emblem, AsyncTexture2D helpTexture)
			: this(background, BgWindowRegion, BgContentRegion, new Point(920, 760))
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).set_Title("Match Overview");
			((WindowBase2)this).set_Subtitle("Example");
			if (emblem != null)
			{
				((WindowBase2)this).set_Emblem(AsyncTexture2D.op_Implicit(emblem));
			}
			((WindowBase2)this).set_Id("WvWPipTally_HelpWindow");
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_CanClose(true);
			((WindowBase2)this).set_CanCloseWithEscape(true);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			Label val = new Label();
			val.set_Text("Press B, stay on Skirmish Details with Current Match Rewards visible, then Scan.");
			val.set_Font(GameService.Content.get_DefaultFont14());
			val.set_TextColor(Color.get_LightGray());
			val.set_ShowShadow(true);
			val.set_WrapText(true);
			((Control)val).set_Width(820);
			((Control)val).set_Height(28);
			((Control)val).set_Location(new Point(8, 6));
			((Control)val).set_Parent((Container)(object)this);
			if (helpTexture != null)
			{
				Image val2 = new Image(helpTexture);
				((Control)val2).set_Location(new Point(8, 40));
				((Control)val2).set_Width(820);
				((Control)val2).set_Height(597);
				((Control)val2).set_Parent((Container)(object)this);
			}
			else
			{
				Label val3 = new Label();
				val3.set_Text("Help image missing from module package.");
				val3.set_Font(GameService.Content.get_DefaultFont14());
				val3.set_TextColor(Color.get_Orange());
				val3.set_AutoSizeWidth(true);
				val3.set_AutoSizeHeight(true);
				((Control)val3).set_Location(new Point(8, 40));
				((Control)val3).set_Parent((Container)(object)this);
			}
		}

		public void ShowCentered()
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			int screenW = ((Control)GameService.Graphics.get_SpriteScreen()).get_Width();
			int screenH = ((Control)GameService.Graphics.get_SpriteScreen()).get_Height();
			((Control)this).set_Location(new Point(Math.Max(10, (screenW - ((Control)this).get_Width()) / 2), Math.Max(10, (screenH - ((Control)this).get_Height()) / 2)));
			((Control)this).Show();
		}
	}
}
