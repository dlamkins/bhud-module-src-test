using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Services;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.Shared.UI.Views
{
	public class TooltipView : BaseView, ITooltipView, IView
	{
		private string Title { get; }

		private string Description { get; }

		private AsyncTexture2D Icon { get; }

		public Color NameTextColor { get; set; } = Color.get_White();


		public Color DescriptionTextColor { get; set; } = StandardColors.get_DisabledText();


		public TooltipView(string title, string description, TranslationService translationService, Gw2ApiManager apiManager = null, IconService iconService = null)
			: base(apiManager, iconService, translationService)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			Title = title;
			Description = description;
		}

		public TooltipView(string title, string description, AsyncTexture2D icon, TranslationService translationService, Gw2ApiManager apiManager = null, IconService iconService = null)
			: this(title, description, translationService, apiManager, iconService)
		{
			Icon = icon;
		}

		protected override void InternalBuild(Panel parent)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Expected O, but got Unknown
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			((Container)parent).set_HeightSizingMode((SizingMode)1);
			((Container)parent).set_WidthSizingMode((SizingMode)1);
			Image val = new Image();
			((Control)val).set_Size(new Point(48, 48));
			((Control)val).set_Location(new Point(8, 8));
			((Control)val).set_Parent((Container)(object)parent);
			val.set_Texture(Icon);
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
			((Control)val2).set_Parent((Container)(object)parent);
			val2.set_TextColor(NameTextColor);
			Label nameLabel = val2;
			Label val3 = new Label();
			val3.set_AutoSizeHeight(true);
			val3.set_AutoSizeWidth(false);
			((Control)val3).set_Location(new Point(((Control)nameLabel).get_Left(), ((Control)image).get_Top() + ((Control)image).get_Height() / 2));
			((Control)val3).set_Padding(new Thickness(0f, 8f, 0f, 0f));
			val3.set_HorizontalAlignment((HorizontalAlignment)0);
			val3.set_VerticalAlignment((VerticalAlignment)1);
			val3.set_TextColor(DescriptionTextColor);
			val3.set_WrapText(true);
			val3.set_Text(Description);
			((Control)val3).set_Parent((Container)(object)parent);
			((Control)val3).set_Width(Math.Max(((Control)nameLabel).get_Width(), 500));
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
