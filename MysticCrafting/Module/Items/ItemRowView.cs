using System;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Items.Controls;
using MysticCrafting.Module.Items.Presenters;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Items
{
	public class ItemRowView : View
	{
		public EventHandler<MouseEventArgs> OnClick;

		private MysticItem Item { get; set; }

		private bool DisplayDetailsType { get; set; }

		public CommercePrices Prices { get; set; }

		public ItemRowView(MysticItem item, bool displayDetailsType = false)
		{
			Item = item;
			DisplayDetailsType = displayDetailsType;
		}

		protected override void Build(Container buildPanel)
		{
			if (Item != null)
			{
				ItemButton detailsButton = new ItemButton
				{
					Parent = buildPanel,
					Text = Item.Name,
					Icon = ServiceContainer.TextureRepository.GetTexture(Item.Icon),
					Rarity = Item.Rarity,
					Type = Item.DetailsType,
					Item = Item
				};
				MysticArmorItem armorItem = Item as MysticArmorItem;
				if (armorItem != null)
				{
					new Label
					{
						Parent = buildPanel,
						AutoSizeWidth = true,
						Location = new Point(buildPanel.Size.X - 200, buildPanel.Size.Y / 2 - 5),
						Text = armorItem.WeightClass.ToString()
					};
				}
				if (DisplayDetailsType)
				{
					new Label
					{
						Parent = buildPanel,
						AutoSizeWidth = true,
						Location = new Point(buildPanel.Size.X - 350, buildPanel.Size.Y / 2 - 5),
						Text = Item.DetailsType
					};
				}
				new ButtonPresenter().BuildFavoriteButton(Item.Id, buildPanel).Location = new Point(buildPanel.Size.X - 80, 15);
				if (OnClick != null)
				{
					detailsButton.Click += OnClick;
				}
			}
		}
	}
}
