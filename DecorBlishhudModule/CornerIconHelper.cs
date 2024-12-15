using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using DecorBlishhudModule.CustomControls.CustomTab;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DecorBlishhudModule
{
	public static class CornerIconHelper
	{
		public static CornerIcon CreateLoadingIcon(Texture2D _homesteadIconUnactive, Texture2D homesteadIconHover, CustomTabbedWindow2 decorWindow, out LoadingSpinner loadingSpinner)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Expected O, but got Unknown
			//IL_00a2: Expected O, but got Unknown
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(_homesteadIconUnactive));
			((Control)val).set_BasicTooltipText("Decor");
			val.set_Priority(1645843523);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			val.set_HoverIcon(AsyncTexture2D.op_Implicit(homesteadIconHover));
			Rectangle absoluteBounds = ((Control)val).get_AbsoluteBounds();
			Point iconPosition = ((Rectangle)(ref absoluteBounds)).get_Location();
			Point spinnerOffset = default(Point);
			((Point)(ref spinnerOffset))._002Ector(35, 0);
			LoadingSpinner val2 = new LoadingSpinner();
			((Control)val2).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val2).set_BasicTooltipText("Decor is fetching data...");
			((Control)val2).set_Size(new Point(32, 32));
			((Control)val2).set_Location(iconPosition + spinnerOffset);
			((Control)val2).set_Visible(true);
			loadingSpinner = val2;
			return val;
		}
	}
}
