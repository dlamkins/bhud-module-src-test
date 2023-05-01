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
		private string Title { get; set; }

		private string Description { get; set; }

		private AsyncTexture2D Icon { get; set; }

		public TooltipView(string title, string description, TranslationService translationService, Gw2ApiManager apiManager = null, IconService iconService = null)
			: base(apiManager, iconService, translationService)
		{
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
			//IL_00ce: Expected O, but got Unknown
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
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
			Label nameLabel = val2;
			Label val3 = new Label();
			val3.set_AutoSizeHeight(true);
			val3.set_AutoSizeWidth(false);
			((Control)val3).set_Location(new Point(((Control)nameLabel).get_Left(), ((Control)image).get_Top() + ((Control)image).get_Height() / 2));
			((Control)val3).set_Padding(new Thickness(0f, 8f, 0f, 0f));
			val3.set_HorizontalAlignment((HorizontalAlignment)0);
			val3.set_VerticalAlignment((VerticalAlignment)1);
			val3.set_TextColor(StandardColors.get_DisabledText());
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
