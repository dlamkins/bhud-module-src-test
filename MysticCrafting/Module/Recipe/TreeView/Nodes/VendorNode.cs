using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MysticCrafting.Models.Vendor;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Recipe.TreeView.Controls;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Recipe.TreeView.Nodes
{
	public class VendorNode : TreeNodeSelect
	{
		public VendorSellsItem VendorItem { get; }

		public override string PathName => VendorItem.FullName;

		public VendorNode(VendorSellsItem vendorItem, Container parent)
		{
			VendorItem = vendorItem;
			base.Parent = parent;
			Build();
		}

		public override void Build()
		{
			ClearChildren();
			Label nameLabel = new Label
			{
				Parent = this,
				Text = VendorItem.VendorName.Truncate(30),
				BasicTooltipText = VendorItem.VendorName,
				Location = new Point(40, 10),
				Font = GameService.Content.DefaultFont16,
				StrokeText = true,
				AutoSizeWidth = true
			};
			CurrenciesContainer currencyContainer = new CurrenciesContainer(this)
			{
				Location = new Point(350, 10),
				Price = VendorItem.MapToCurrencyQuantities().ToList()
			};
			string quantityText = "";
			if (VendorItem.ItemQuantity == 1)
			{
				quantityText = MysticCrafting.Module.Strings.Recipe.PriceEach;
			}
			else if (VendorItem.ItemQuantity > 1)
			{
				quantityText = string.Format(MysticCrafting.Module.Strings.Recipe.PricePerNumber, VendorItem.ItemQuantity);
			}
			if (!string.IsNullOrEmpty(quantityText))
			{
				new Label
				{
					Parent = this,
					Text = quantityText,
					Location = new Point(currencyContainer.Right, 10),
					Font = GameService.Content.DefaultFont16,
					StrokeText = true,
					AutoSizeWidth = true
				};
			}
			if (!string.IsNullOrEmpty(VendorItem.Requirement))
			{
				new Image
				{
					Parent = this,
					Texture = ServiceContainer.TextureRepository.Textures.ExclamationMark,
					BasicTooltipText = MysticCrafting.Module.Strings.Recipe.Requirement + ": " + VendorItem.Requirement,
					Size = new Point(30, 30),
					Location = new Point(40 + nameLabel.Width, 5)
				};
			}
			base.Build();
		}
	}
}
