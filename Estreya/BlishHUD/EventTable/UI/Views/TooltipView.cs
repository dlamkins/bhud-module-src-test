using System;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Estreya.BlishHUD.EventTable.Extensions;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class TooltipView : View, ITooltipView, IView
	{
		private string Title { get; set; }

		private string Description { get; set; }

		private string Icon { get; set; }

		public TooltipView(string title, string description)
			: this()
		{
			Title = title;
			Description = description;
		}

		public TooltipView(string title, string description, string icon)
			: this(title, description)
		{
			Icon = icon;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Expected O, but got Unknown
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			buildPanel.set_HeightSizingMode((SizingMode)1);
			buildPanel.set_WidthSizingMode((SizingMode)1);
			Image val = new Image();
			((Control)val).set_Size(new Point(48, 48));
			((Control)val).set_Location(new Point(8, 8));
			((Control)val).set_Parent(buildPanel);
			val.set_Texture(EventTableModule.ModuleInstance.ContentsManager.GetIcon(Icon));
			Image image = val;
			Label val2 = new Label();
			val2.set_AutoSizeHeight(false);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Location(new Point(((Control)image).get_Right() + 8, ((Control)image).get_Top()));
			((Control)val2).set_Height(((Control)image).get_Height() / 2);
			((Control)val2).set_Padding(new Thickness(0f, 8f, 0f, 0f));
			val2.set_HorizontalAlignment((HorizontalAlignment)0);
			val2.set_VerticalAlignment((VerticalAlignment)1);
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_Text(Title);
			((Control)val2).set_Parent(buildPanel);
			Label nameLabel = val2;
			Label val3 = new Label();
			val3.set_AutoSizeHeight(true);
			val3.set_AutoSizeWidth(false);
			((Control)val3).set_Location(new Point(((Control)nameLabel).get_Left(), ((Control)image).get_Top() + ((Control)image).get_Height() / 2));
			((Control)val3).set_Width(Math.Max(((Control)nameLabel).get_Width(), 200));
			((Control)val3).set_Padding(new Thickness(0f, 8f, 0f, 0f));
			val3.set_HorizontalAlignment((HorizontalAlignment)0);
			val3.set_VerticalAlignment((VerticalAlignment)1);
			val3.set_TextColor(StandardColors.get_DisabledText());
			val3.set_WrapText(true);
			val3.set_Text(Description);
			((Control)val3).set_Parent(buildPanel);
		}
	}
}
