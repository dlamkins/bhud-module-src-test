using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using rp.spark.Services;

namespace rp.spark.UI.Views
{
	internal static class ProfileEditorUI
	{
		public const int SaveY = 515;

		public const int StatusY = 520;

		public const int HeaderY = 560;

		public static Label AddSaveFooter(Container parent, ProfileEditorSession session, int saveY = 515, int statusY = 520, int headerY = 560, int statusX = 170, int statusWidth = 560)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Expected O, but got Unknown
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			StandardButton val = new StandardButton();
			val.set_Text("Save Profile");
			((Control)val).set_Location(new Point(0, saveY));
			((Control)val).set_Size(new Point(150, 35));
			((Control)val).set_Parent(parent);
			SparkUiActions.BindClick(val, () => session.SaveAsync(), session.SetStatus, "Couldn't save profile.");
			Label result = AddStatusLabel(parent, session.StatusText, new Point(statusX, statusY), new Point(statusWidth, 30));
			AddHeaderLabel(parent, session, headerY);
			return result;
		}

		public static Label AddStatusLabel(Container parent, string text, Point location, Point size)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text(text ?? string.Empty);
			((Control)val).set_Location(location);
			((Control)val).set_Size(size);
			((Control)val).set_Parent(parent);
			return val;
		}

		public static Label AddHeaderLabel(Container parent, ProfileEditorSession session, int y = 560)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text(session.GetHeaderText());
			val.set_Font(GameService.Content.get_DefaultFont12());
			val.set_TextColor(new Color(220, 220, 220));
			((Control)val).set_Location(new Point(0, y));
			((Control)val).set_Size(new Point(760, 25));
			((Control)val).set_Parent(parent);
			return val;
		}

		public static void ShowUnavailableMessage(Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			val.set_Text("SPARK can't connect to GW2 to retrieve your info.");
			val.set_Font(GameService.Content.get_DefaultFont18());
			val.set_TextColor(new Color(255, 233, 180));
			val.set_StrokeText(true);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Size(new Point(760, 30));
			((Control)val).set_Parent(parent);
			Label val2 = new Label();
			val2.set_Text("Please log in to a character to use SPARK.");
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_TextColor(Color.get_White());
			val2.set_WrapText(true);
			((Control)val2).set_Location(new Point(0, 40));
			((Control)val2).set_Size(new Point(760, 80));
			((Control)val2).set_Parent(parent);
		}

		public static void AddLabel(Container parent, string text, int y, int x = 0, int width = 250)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			val.set_Text(text);
			((Control)val).set_Location(new Point(x, y));
			((Control)val).set_Size(new Point(width, 25));
			((Control)val).set_Parent(parent);
		}
	}
}
