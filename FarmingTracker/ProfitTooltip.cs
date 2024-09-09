using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace FarmingTracker
{
	public class ProfitTooltip : DisposableTooltip
	{
		public CoinsPanel ProfitPerHourPanel { get; }

		public ProfitTooltip(Services services)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, 10f));
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Control)val).set_Parent((Container)(object)this);
			FlowPanel rootFlowPanel = val;
			BitmapFont font = services.FontService.Fonts[(FontSize)16];
			Label val2 = new Label();
			val2.set_Text("Rough profit when selling everything to vendor and on trading post. Click help button in 'Summary' tab of the main window for more info.");
			val2.set_Font(font);
			val2.set_WrapText(true);
			((Control)val2).set_Width(420);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Parent((Container)(object)rootFlowPanel);
			ProfitPerHourPanel = new CoinsPanel(null, font, services.TextureService, (Container)(object)rootFlowPanel);
			Label val3 = new Label();
			val3.set_Text(" Profit per hour");
			val3.set_Font(font);
			val3.set_AutoSizeHeight(true);
			val3.set_AutoSizeWidth(true);
			((Control)val3).set_Parent((Container)(object)ProfitPerHourPanel);
		}
	}
}
