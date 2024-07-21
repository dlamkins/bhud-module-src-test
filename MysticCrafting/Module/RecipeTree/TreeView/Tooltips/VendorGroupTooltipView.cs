using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Vendor;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Models.Vendor;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.RecipeTree.TreeView.Tooltips
{
	public class VendorGroupTooltipView : View, ITooltipView, IView
	{
		private List<Control> _controls = new List<Control>();

		protected bool Initialized;

		private VendorSellsItemGroup Group { get; set; }

		public Container BuildPanel { get; set; }

		public VendorGroupTooltipView(VendorSellsItemGroup vendorGroup)
			: this()
		{
			Group = vendorGroup;
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
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			int yPos = 10;
			foreach (Vendor vendor in Group.Vendors.Take(5))
			{
				AsyncTexture2D icon = ServiceContainer.TextureRepository.GetVendorIconTexture(vendor.IconFile);
				if (icon != null)
				{
					Image val = new Image(icon);
					((Control)val).set_Parent(BuildPanel);
					((Control)val).set_Size(new Point(30, 30));
					((Control)val).set_Location(new Point(5, yPos));
				}
				Label val2 = new Label();
				((Control)val2).set_Parent(BuildPanel);
				val2.set_Text(vendor.Name);
				val2.set_TextColor(ColorHelper.VendorName);
				val2.set_Font(GameService.Content.get_DefaultFont16());
				((Control)val2).set_Location(new Point((icon == null) ? 15 : 40, yPos));
				val2.set_StrokeText(true);
				val2.set_AutoSizeWidth(true);
				Label nameLabel = val2;
				yPos += 25;
				foreach (string location in vendor.Locations.Take(8))
				{
					Label val3 = new Label();
					((Control)val3).set_Parent(BuildPanel);
					val3.set_Text(location);
					val3.set_Font(GameService.Content.get_DefaultFont14());
					((Control)val3).set_Location(new Point(((Control)nameLabel).get_Left(), yPos));
					val3.set_TextColor(Color.get_LightGray());
					val3.set_StrokeText(true);
					val3.set_AutoSizeWidth(true);
					yPos += 20;
				}
				if (vendor.Locations.Count > 8)
				{
					Label val4 = new Label();
					((Control)val4).set_Parent(BuildPanel);
					val4.set_Text(string.Format(Recipe.VendorsMore, vendor.Locations.Count - 8));
					val4.set_Font(GameService.Content.get_DefaultFont14());
					((Control)val4).set_Location(new Point(((Control)nameLabel).get_Left(), yPos));
					val4.set_TextColor(Color.get_LightGray());
					val4.set_StrokeText(true);
					val4.set_AutoSizeWidth(true);
					yPos += 15;
				}
				yPos += 10;
			}
			yPos += 10;
			if (Group.Vendors.Count > 10)
			{
				Label val5 = new Label();
				((Control)val5).set_Parent(BuildPanel);
				val5.set_Text(string.Format(Recipe.VendorsMore, Group.Vendors.Count - 5));
				val5.set_Font(GameService.Content.get_DefaultFont16());
				((Control)val5).set_Location(new Point(5, yPos));
				val5.set_TextColor(Color.get_LightGray());
				val5.set_StrokeText(true);
				val5.set_AutoSizeWidth(true);
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
