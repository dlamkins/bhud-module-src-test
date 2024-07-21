using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Models.Vendor;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.RecipeTree.TreeView.Tooltips
{
	public class VendorSourceTooltipView : View, ITooltipView, IView
	{
		private List<Control> _controls = new List<Control>();

		protected bool Initialized;

		private VendorSource Source { get; set; }

		public Container BuildPanel { get; set; }

		public VendorSourceTooltipView(VendorSource source)
			: this()
		{
			Source = source;
		}

		protected override void Build(Container buildPanel)
		{
			BuildPanel = buildPanel;
			if (!Initialized)
			{
				Initialize();
			}
		}

		public virtual void Initialize()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Expected O, but got Unknown
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Expected O, but got Unknown
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Expected O, but got Unknown
			AsyncTexture2D icon = ServiceContainer.TextureRepository.Textures.MerchantIconColor;
			List<Control> controls = _controls;
			Image val = new Image(icon);
			((Control)val).set_Parent(BuildPanel);
			((Control)val).set_Size(new Point(25, 25));
			((Control)val).set_Location(new Point(0, 0));
			controls.Add((Control)val);
			List<string> vendorNames = Source.VendorItems.Select((VendorSellsItem v) => v.Vendor.Name).Distinct().ToList();
			List<Control> controls2 = _controls;
			Label val2 = new Label();
			((Control)val2).set_Parent(BuildPanel);
			val2.set_Text(string.Format(Recipe.SoldByVendors, vendorNames.Count));
			val2.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val2).set_Location(new Point(30, 3));
			val2.set_StrokeText(true);
			val2.set_AutoSizeWidth(true);
			controls2.Add((Control)val2);
			int yPos = 40;
			foreach (string vendor in vendorNames.Take(10))
			{
				Label val3 = new Label();
				((Control)val3).set_Parent(BuildPanel);
				val3.set_Text(vendor);
				val3.set_TextColor(ColorHelper.VendorName);
				val3.set_Font(GameService.Content.get_DefaultFont14());
				((Control)val3).set_Location(new Point(5, yPos));
				val3.set_StrokeText(true);
				val3.set_AutoSizeWidth(true);
				Label vendorLabel = val3;
				_controls.Add((Control)(object)vendorLabel);
				yPos += 20;
			}
			yPos += 10;
			if (vendorNames.Count > 10)
			{
				Label val4 = new Label();
				((Control)val4).set_Parent(BuildPanel);
				val4.set_Text(string.Format(Recipe.VendorsMore, vendorNames.Count - 10));
				val4.set_Font(GameService.Content.get_DefaultFont14());
				((Control)val4).set_Location(new Point(5, yPos));
				val4.set_TextColor(Color.get_White());
				val4.set_StrokeText(true);
				val4.set_AutoSizeWidth(true);
				Label moreLabel = val4;
				_controls.Add((Control)(object)moreLabel);
			}
		}

		protected override void Unload()
		{
			_controls?.SafeDispose();
			_controls?.Clear();
			BuildPanel = null;
			((View<IPresenter>)this).Unload();
		}
	}
}
