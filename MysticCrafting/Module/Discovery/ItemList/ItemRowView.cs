using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Discovery.ItemList.Controls;
using MysticCrafting.Module.Discovery.ItemList.Presenters;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Helpers;
using MysticCrafting.Module.Recipe.TreeView.Controls;
using MysticCrafting.Module.Recipe.TreeView.Tooltips;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Discovery.ItemList
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
			if (Item == null)
			{
				return;
			}
			ItemButton detailsButton = new ItemButton
			{
				Parent = buildPanel,
				Text = Item.LocalizedName().Truncate(35),
				Rarity = Item.Rarity,
				Type = LocalizationHelper.TranslateMenuItem(Item.DetailsType),
				Item = Item,
				Size = new Point(344, 60)
			};
			AsyncTexture2D iconTexture = ServiceContainer.TextureRepository.GetTexture(Item.Icon);
			if (iconTexture != null)
			{
				try
				{
					new Image(iconTexture)
					{
						Parent = buildPanel,
						Location = new Point(3, 4),
						Size = new Point(52, 52),
						Tooltip = new DisposableTooltip(new ItemTooltipView(Item, 0))
					};
				}
				catch (Exception)
				{
				}
			}
			if (Item.WeightClass != 0)
			{
				new Label
				{
					Parent = buildPanel,
					AutoSizeWidth = true,
					Location = new Point(buildPanel.Size.X - 300, buildPanel.Size.Y / 2 - 10),
					Text = Item.WeightClass.ToString()
				};
			}
			if (DisplayDetailsType)
			{
				new Label
				{
					Parent = buildPanel,
					AutoSizeWidth = true,
					Location = new Point(buildPanel.Size.X - 450, buildPanel.Size.Y / 2 - 10),
					Text = Item.DetailsType
				};
			}
			if (Item.HasSkin() && Item.DefaultSkin.HasValue)
			{
				bool skinUnlocked = ServiceContainer.PlayerUnlocksService.ItemUnlocked(Item.DefaultSkin.GetValueOrDefault());
				new Image(skinUnlocked ? ServiceContainer.TextureRepository.Textures.Checkmark : ServiceContainer.TextureRepository.Textures.Lock)
				{
					Parent = buildPanel,
					Location = new Point(buildPanel.Size.X - 190, buildPanel.Size.Y / 2 - 20),
					Size = new Point(40, 40),
					BasicTooltipText = (skinUnlocked ? Common.SkinUnlocked : Common.SkinLocked)
				};
			}
			if (Item.RarityEnum == MysticItemRarity.Legendary)
			{
				int unlockedCount2 = ServiceContainer.PlayerUnlocksService.LegendaryUnlockedCount(Item.GameId);
				int? maxCount = Item.GetMaxCount();
				new Label
				{
					Parent = buildPanel,
					AutoSizeWidth = true,
					Location = new Point(buildPanel.Size.X - 110, buildPanel.Size.Y / 2 - 10),
					Font = GameService.Content.DefaultFont14,
					Text = $"{unlockedCount2}/{maxCount}",
					BasicTooltipText = string.Format(MysticCrafting.Module.Strings.Recipe.TooltipUnlockedItem, unlockedCount2, maxCount)
				};
			}
			else
			{
				int unlockedCount = ServiceContainer.PlayerItemService.GetItemCount(Item.GameId);
				new Label
				{
					Parent = buildPanel,
					AutoSizeWidth = true,
					Location = new Point(buildPanel.Size.X - 110, buildPanel.Size.Y / 2 - 10),
					Font = GameService.Content.DefaultFont14,
					Text = $"{unlockedCount}",
					BasicTooltipText = string.Format(MysticCrafting.Module.Strings.Recipe.TooltipCollectedItem, unlockedCount)
				};
			}
			new ButtonPresenter().BuildFavoriteButton(Item.GameId, buildPanel).Location = new Point(buildPanel.Size.X - 60, buildPanel.Size.Y / 2 - 20);
			if (OnClick != null)
			{
				detailsButton.Click += OnClick;
			}
		}
	}
}
